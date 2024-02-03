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
        /// FFamily資料查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetFamilyData(Models.WSheetSearchArg arg)
        {
            return Json(this.wsheetService.GetFamilyByCondtioin(arg));
        }

        /// <summary>
        /// FSize資料查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSizeData(Models.WSheetSearchArg arg)
        {
            return Json(this.wsheetService.GetSizeByCondtioin(arg));
        }

        /// <summary>
        /// FCPU資料查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCpuData(Models.WSheetSearchArg arg)
        {
            return Json(this.wsheetService.GetCPUByCondtioin(arg));
        }

        /// <summary>
        /// FGPU資料查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGpuData(Models.WSheetSearchArg arg)
        {
            return Json(this.wsheetService.GetGPUByCondtioin(arg));
        }

        /// <summary>
        /// FDisplay資料查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDisplayData(Models.WSheetSearchArg arg)
        {
            return Json(this.wsheetService.GetDisplayByCondtioin(arg));
        }

        /// <summary>
        /// 多選單資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListData(string ListType)
        {
            return Json(this.wsheetService.getMultiSelectList(ListType));
        }

    }
}