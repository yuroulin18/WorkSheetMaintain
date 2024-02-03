using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WorkSheetMaintain.Models
{

    public class WSheetService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 依照條件取得Worksheet資料
        /// </summary>
        /// <returns></returns>
        public List<Models.WSheet> GetWSheetkByCondtioin(Models.WSheetSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select Brand, Family, Model, Display_size AS Size, Display_resolution AS Resolution, brightness_group AS Brightness, Display_OLED AS OLED, Display_touch AS Touchscreen, count(SKU) AS SKU, unit, Estimation
                            from dbo.Raw$
                            where (Brand IN (SELECT value FROM STRING_SPLIT(@brand, ','))  OR @brand='') AND (Family IN (SELECT value FROM STRING_SPLIT(@family, ',')) OR @family='') AND
                                  (Model IN (SELECT value FROM STRING_SPLIT(@model, ',')) OR @model='') AND (Display_Size IN (SELECT value FROM STRING_SPLIT(@size, ',')) OR @size='') AND
                                  (Display_resolution IN (SELECT value FROM STRING_SPLIT(@resolution, ',')) OR @resolution = '')
                            group by Brand, Family, Model, Display_size, Display_resolution, Display_OLED, Display_touch, brightness_group, unit, Estimation
                            order by Brand, Family, Model, Size";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@brand", arg.brand == null ? string.Empty : string.Join(",", arg.brand)));
                cmd.Parameters.Add(new SqlParameter("@family", arg.family == null ? string.Empty : string.Join(",", arg.family)));
                cmd.Parameters.Add(new SqlParameter("@model", arg.model == null ? string.Empty : string.Join(",", arg.model)));
                cmd.Parameters.Add(new SqlParameter("@size", arg.size == null ? string.Empty : string.Join(",", arg.size)));
                cmd.Parameters.Add(new SqlParameter("@resolution", arg.resolution == null ? string.Empty : string.Join(",", arg.resolution)));
                //cmd.Parameters.Add(new SqlParameter("@resolution", arg.resolution == null ? string.Empty : arg.resolution));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapWSheetDataToList(dt);
        }

        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="worksheetData"></param>
        /// <returns></returns>
        private List<Models.WSheet> MapWSheetDataToList(DataTable wsheetData)
        {
            List<Models.WSheet> result = new List<WSheet>();
            foreach (DataRow row in wsheetData.Rows)
            {
                result.Add(new WSheet()
                {
                    brand = row["Brand"].ToString().Split(','),
                    family = row["Family"].ToString().Split(','),
                    model = row["Model"].ToString().Split(','),
                    size = row["Size"].ToString().Split(','),
                    resolution = row["Resolution"].ToString().Split(','),
                    brightness = row["Brightness"].ToString(),
                    oled = row["OLED"].ToString(),
                    touchscreen = row["Touchscreen"].ToString(),
                    sku = (int)row["SKU"],
                    unit = (int)row["unit"],
                    Estimation = (int)row["Estimation"]
                });
            }
            return result;
        }

        /// <summary>
        /// 依照條件取得Feature Family資料
        /// </summary>
        /// <returns></returns>
        public List<Models.WSheet> GetFamilyByCondtioin(Models.WSheetSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select Brand, category, Family, count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
                            from dbo.Raw$
                            where (Brand IN (SELECT value FROM STRING_SPLIT(@brand, ','))  OR @brand='') AND (Family IN (SELECT value FROM STRING_SPLIT(@family, ',')) OR @family='')
                            group by Brand, category, Family
                            order by Brand, category, Family";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@brand", arg.brand == null ? string.Empty : string.Join(",", arg.brand)));
                cmd.Parameters.Add(new SqlParameter("@family", arg.family == null ? string.Empty : string.Join(",", arg.family)));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapFamilyDataToList(dt);
        }

        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="ffamilyData"></param>
        /// <returns></returns>
        private List<Models.WSheet> MapFamilyDataToList(DataTable wsheetData)
        {
            List<Models.WSheet> result = new List<WSheet>();
            foreach (DataRow row in wsheetData.Rows)
            {
                result.Add(new WSheet()
                {
                    brand = row["Brand"].ToString().Split(','),
                    category = row["category"].ToString(),
                    family = row["Family"].ToString().Split(','),
                    sku = (int)row["SKU_Count"],
                    sku_Pct = row["SKU_Pct"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 依照條件取得Feature Size資料
        /// </summary>
        /// <returns></returns>
        public List<Models.WSheet> GetSizeByCondtioin(Models.WSheetSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select Brand, Size,  count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
                            from dbo.Raw$
                            where (Brand IN (SELECT value FROM STRING_SPLIT(@brand, ','))  OR @brand='') AND (Size IN (SELECT value FROM STRING_SPLIT(@size, ',')) OR @size='')
                            group by Brand, Size
                            order by Brand, Size";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@brand", arg.brand == null ? string.Empty : string.Join(",", arg.brand)));
                cmd.Parameters.Add(new SqlParameter("@size", arg.size == null ? string.Empty : string.Join(",", arg.size)));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapSizeDataToList(dt);
        }

        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="fsizeData"></param>
        /// <returns></returns>
        private List<Models.WSheet> MapSizeDataToList(DataTable wsheetData)
        {
            List<Models.WSheet> result = new List<WSheet>();
            foreach (DataRow row in wsheetData.Rows)
            {
                result.Add(new WSheet()
                {
                    brand = row["Brand"].ToString().Split(','),
                    size = row["Size"].ToString().Split(','),
                    sku = (int)row["SKU_Count"],
                    sku_Pct = row["SKU_Pct"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 依照條件取得Feature CPU資料
        /// </summary>
        /// <returns></returns>
        public List<Models.WSheet> GetCPUByCondtioin(Models.WSheetSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select Brand, [CPU-Gen], count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
                            from dbo.Raw$
                            where (Brand IN (SELECT value FROM STRING_SPLIT(@brand, ','))  OR @brand='')
                            group by Brand, [CPU-Gen]
                            order by Brand, [CPU-Gen]";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@brand", arg.brand == null ? string.Empty : string.Join(",", arg.brand)));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCPUDataToList(dt);
        }

        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="fcpuData"></param>
        /// <returns></returns>
        private List<Models.WSheet> MapCPUDataToList(DataTable wsheetData)
        {
            List<Models.WSheet> result = new List<WSheet>();
            foreach (DataRow row in wsheetData.Rows)
            {
                result.Add(new WSheet()
                {
                    brand = row["Brand"].ToString().Split(','),
                    cpu = row["CPU-Gen"].ToString(),
                    sku = (int)row["SKU_Count"],
                    sku_Pct = row["SKU_Pct"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 依照條件取得Feature GPU資料
        /// </summary>
        /// <returns></returns>
        public List<Models.WSheet> GetGPUByCondtioin(Models.WSheetSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select Brand, GPU_Group, count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
                            from dbo.Raw$
                            where (Brand IN (SELECT value FROM STRING_SPLIT(@brand, ','))  OR @brand='')
                            group by Brand, GPU_Group
                            order by Brand, GPU_Group";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@brand", arg.brand == null ? string.Empty : string.Join(",", arg.brand)));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapGPUDataToList(dt);
        }

        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="fgpuData"></param>
        /// <returns></returns>
        private List<Models.WSheet> MapGPUDataToList(DataTable wsheetData)
        {
            List<Models.WSheet> result = new List<WSheet>();
            foreach (DataRow row in wsheetData.Rows)
            {
                result.Add(new WSheet()
                {
                    brand = row["Brand"].ToString().Split(','),
                    gpu = row["GPU_Group"].ToString(),
                    sku = (int)row["SKU_Count"],
                    sku_Pct = row["SKU_Pct"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 依照條件取得Feature Display資料
        /// </summary>
        /// <returns></returns>
        public List<Models.WSheet> GetDisplayByCondtioin(Models.WSheetSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"select Brand, Size, Display_resolution, count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
                            from dbo.Raw$
                            where (Brand IN (SELECT value FROM STRING_SPLIT(@brand, ','))  OR @brand='') AND (Display_Size IN (SELECT value FROM STRING_SPLIT(@size, ',')) OR @size='') AND
                                  (Display_resolution IN (SELECT value FROM STRING_SPLIT(@resolution, ',')) OR @resolution = '')
                            group by Brand, Size, Display_resolution
                            order by Brand, Size, Display_resolution";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@brand", arg.brand == null ? string.Empty : string.Join(",", arg.brand)));
                cmd.Parameters.Add(new SqlParameter("@size", arg.size == null ? string.Empty : string.Join(",", arg.size)));
                cmd.Parameters.Add(new SqlParameter("@resolution", arg.resolution == null ? string.Empty : string.Join(",", arg.resolution)));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapDisplayDataToList(dt);
        }

        /// <summary>
        /// Map資料進List
        /// </summary>
        /// <param name="fdisplayData"></param>
        /// <returns></returns>
        private List<Models.WSheet> MapDisplayDataToList(DataTable wsheetData)
        {
            List<Models.WSheet> result = new List<WSheet>();
            foreach (DataRow row in wsheetData.Rows)
            {
                result.Add(new WSheet()
                {
                    brand = row["Brand"].ToString().Split(','),
                    size = row["Size"].ToString().Split(','),
                    resolution = row["Display_resolution"].ToString().Split(','),
                    sku = (int)row["SKU_Count"],
                    sku_Pct = row["SKU_Pct"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 取得多選單的資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> getMultiSelectList(string wsheetType)
        {
            DataTable dt = new DataTable();
            string sql = "";
            switch (wsheetType)
            {
                case "#brandid":
                    sql = @"select distinct Brand AS NAME
                            from dbo.Raw$
                            where Brand is not Null";
                    break;
                case "#familyid":
                    sql = @"select distinct Family AS NAME
                            from dbo.Raw$
                            where Family is not Null";
                    break;
                case "#modelid":
                    sql = @"select distinct Model AS NAME
                            from dbo.Raw$
                            where Model is not Null";
                    break;
                case "#sizelid":
                    sql = @"select distinct Display_size AS NAME
                            from dbo.Raw$
                            where Display_size is not Null";
                    break;
                case "#resolutionid":
                    sql = @"select distinct Display_resolution AS NAME
                            from dbo.Raw$
                            where Display_resolution is not Null";
                    break;
            }
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.getMultiSelectListData(dt);            
        }

        /// <summary>
        /// Maping 代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> getMultiSelectListData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["NAME"].ToString(),
                    Value = row["NAME"].ToString()
                });
            }
            return result;
        }

    }
}