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
            string sql = @"select Brand, Family, Model, Display_size+'('+Display_resolution+')' AS Size, brightness_group AS Brightness, Display_OLED AS OLED, Display_touch AS Touchscreen, count(SKU) AS SKU
                            from dbo.Raw$
                            where (Brand=@brand OR @brand='') AND (Family=@family OR @family='') AND (Model=@model OR @model='') AND (Display_Size=@size OR @size='')
                            group by Brand, Family, Model, Display_size, Display_resolution, Display_OLED, Display_touch, brightness_group
                            order by Brand, Family, Model, Size";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@brand", arg.brand == null ? string.Empty : arg.brand));
                cmd.Parameters.Add(new SqlParameter("@family", arg.family == null ? string.Empty : arg.family));
                cmd.Parameters.Add(new SqlParameter("@model", arg.model == null ? string.Empty : arg.model));
                cmd.Parameters.Add(new SqlParameter("@size", arg.size == null ? string.Empty : arg.size));
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
                    brand = row["Brand"].ToString(),
                    family = row["Family"].ToString(),
                    model = row["Model"].ToString(),
                    size = row["Size"].ToString(),
                    brightness = row["Brightness"].ToString(),
                    oled = row["OLED"].ToString(),
                    touchscreen = row["Touchscreen"].ToString(),
                    sku = (int)row["SKU"]
                });
            }
            return result;
        }

        /// <summary>
        /// 取得下拉式選單的資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> getDropDownList(string wsheetType)
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
            }
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.getDropDownListData(dt);            
        }

        /// <summary>
        /// Maping 代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> getDropDownListData(DataTable dt)
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