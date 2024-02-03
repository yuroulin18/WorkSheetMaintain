using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WorkSheetMaintain.Models
{
    public class WSheetSearchArg
    {
        public WSheetSearchArg()
        {

        }
        public WSheetSearchArg(string[] brand, string[] family, string[] model, string[] size, string[] resolution)
        {
            this.brand = brand;
            this.family = family;
            this.model = model;
            this.size = size;
            this.resolution = resolution;
        }


        [DisplayName("品牌 Brand")]
        public string[] brand { get; set; }

        [DisplayName("系列 Family")]
        public string[] family { get; set; }

        [DisplayName("型號 Model")]
        public string[] model { get; set; }

        [DisplayName("尺寸 Size")]
        public string[] size { get; set; }

        [DisplayName("解析度 Resolution")]
        public string[] resolution { get; set; }

        [DisplayName("亮度(nits)")]
        public string brightness { get; set; }

        [DisplayName("是否有oled")]
        public string oled { get; set; }

        [DisplayName("是否有觸屏")]
        public string touchscreen { get; set; }

        [DisplayName("sku合計")]
        public int sku { get; set; }

        [DisplayName("sku百分比")]
        public string sku_Pct { get; set; }

        [DisplayName("GPU系列")]
        public string gpu { get; set; }

        [DisplayName("CPU型號")]
        public string cpu { get; set; }

        [DisplayName("筆電類型")]
        public string category { get; set; }

        [DisplayName("Unit")]
        public int unit { get; set; }

        [DisplayName("Estimation")]
        public int Estimation { get; set; }
    }
}