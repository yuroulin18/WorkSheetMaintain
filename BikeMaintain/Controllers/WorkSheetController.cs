using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WorkSheetMaintain.Controllers
{
    public class WorkSheetController : Controller
    {
        Models.WSheetService wsheetService = new Models.WSheetService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// WSheet資料查詢
        /// </summary>
        /// <returns></returns> 
        [HttpPost()]
        public JsonResult GetSearchData(Models.WSheetSearchArg arg)
        {
            Models.WSheetService wsheetService = new Models.WSheetService();
            return Json(wsheetService.GetWSheetkByCondtioin(arg));
        }

        /// <summary>
        /// 下拉式選單資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListData(string ListType)
        {
            return Json(this.wsheetService.getDropDownList(ListType));
        }

    }
}