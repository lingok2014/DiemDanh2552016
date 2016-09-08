using DevExpress.Web.Mvc;
using QuanLyDiemDanh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;

namespace QuanLyDiemDanh.Controllers
{
    public class ReportsController : Controller
    {
        private DiemDanhSinhVienEntities db = new DiemDanhSinhVienEntities();

        // GET: Reports
        public ActionResult Index()
        {
            ViewData["baocao"] = new RptdsGiaoVien();
            return View();
        }
        public ActionResult DanhSachGiaoVienPartial()
        {
            ViewData["baocao"] = new RptdsGiaoVien();
            return PartialView("DanhSachGiaoVienPartial");
        }
        public ActionResult ExportReportView()
        {
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(new RptdsGiaoVien());
        }


      
    }
}