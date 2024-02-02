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
        public WSheetSearchArg(string brand, string family, string model, string size)
        {
            this.brand = brand;
            this.family = family;
            this.model = model;
            this.size = size;
        }


        [DisplayName("品牌 Brand")]
        public string brand { get; set; }

        [DisplayName("系列 Family")]
        public string family { get; set; }

        [DisplayName("型號 Model")]
        public string model { get; set; }

        [DisplayName("尺寸 Size")]
        public string size { get; set; }

        [DisplayName("亮度(nits)")]
        public string brightness { get; set; }

        [DisplayName("是否有oled")]
        public string oled { get; set; }

        [DisplayName("是否有觸屏")]
        public string touchscreen { get; set; }

        [DisplayName("sku合計")]
        public int sku { get; set; }
    }
}