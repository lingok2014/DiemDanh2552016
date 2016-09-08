using QuanLyDiemDanh.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDiemDanh.Controllers
{
    public class DiemDanhController : Controller
    {
        private DiemDanhSinhVienEntities db = new DiemDanhSinhVienEntities();
        // GET: DiemDanh
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection diemdanh)//tham so gia? de tranh loi override voi index tren, khong duoc su dung trong action nay
        {
            addButton();
            return View();
        }
        public JsonResult getMaLop(string term)
        {
            //List<string> res = db.LopHocPhans.Where(x => x.lop_ma.StartsWith(term)).Select(y => y.lop_ma).ToList();
            //return Json(res, JsonRequestBehavior.AllowGet);

            List<string> result = db.LopHocPhans.Where(x => x.lop_ma.StartsWith(term)).
               Select(y => y.lop_ma).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDanhSachSinhVien(string term)
        {
            List<string> list = new List<string>();
            list = db.LopHocPhan_SinhVien.Where(x => x.lop_ma.ToLower().Equals(term.ToLower())).Select(y => y.sv_ma).ToList();
            ArrayList svList = new ArrayList();
            foreach (string str in list)
            {
                SinhVien[] query = db.SinhViens.
                    Where(x => x.sv_ma.ToLower().Equals(str.ToLower())).ToArray().Select(y => new SinhVien
                    (y.sv_ma, y.sv_portalID, y.sv_ten)).ToArray();
                if (query.Length != 0)
                    svList.Add(query[0]);
            }
            return Json(svList, JsonRequestBehavior.AllowGet);
        }

        public ArrayList DanhSachSinhVien(string malop)
        {

            List<string> list = new List<string>();
            list = db.LopHocPhan_SinhVien.Where(x => x.lop_ma.ToLower().Equals(malop.ToLower())).Select(y => y.sv_ma).ToList();
            ArrayList svList = new ArrayList();

            foreach (string str in list)
            {
                SinhVien[] query = db.SinhViens.
                    Where(x => x.sv_ma.ToLower().
                    Equals(str.ToLower())).
                    ToArray().
                    Select(y => new SinhVien
                    (y.sv_ma, y.sv_portalID, y.sv_ten))
                    .ToArray();
                if (query.Length != 0)
                    svList.Add(query[0]);
            }
            return svList;
        }
        private void addButton()
        {
            ArrayList svList = new ArrayList();
            string malop = Request.Form["lop_ma"].ToString();
            DateTime dd_ngay = Convert.ToDateTime(Request.Form["dd_ngay"].ToString());
            string dd_phong = Request.Form["dd_phong"].ToString();
            int buoihoc = Convert.ToInt32(Request.Form["dd_buoihoc"].ToString());
            svList = DanhSachSinhVien(malop);

            List<int> checkBuoiHoc = (from a in db.LopHocPhan_SinhVien
                                      join b in db.DiemDanhs on a.lopsv_stt equals b.lopsv_stt
                                      where a.lop_ma.ToLower() == malop.ToLower() && b.buoihoc == buoihoc
                                      select b.buoihoc).Distinct().ToList();

            if (checkBuoiHoc.Count == 0 || checkBuoiHoc[0].ToString() == "")
            {
                ViewBag.Error = "";
                if (Request.Form["chk[]"] == null)
                {
                    for (int i = 0; i < svList.Count; i++)
                    {
                        SinhVien sv = new SinhVien();
                        sv = (SinhVien)svList[i];

                        //Lay ma cua sinh vien trong lop dang xet
                        List<long> ma_lopsv = db.LopHocPhan_SinhVien.Where
                            (x => x.lop_ma.ToLower() == malop.ToLower()
                            && x.sv_ma.ToLower() == sv.sv_ma.ToLower()).
                            Select(y => y.lopsv_stt).ToList();

                        DiemDanh dd = new DiemDanh(ma_lopsv[0], dd_phong, buoihoc, dd_ngay, false, Request.Form["dd_lydo" + sv.sv_ma].ToString(), "GV001");
                        db.DiemDanhs.Add(dd);
                    }
                    db.SaveChanges();
                }

                else
                {
                    string absent = @Request.Form["chk[]"].ToString();

                    //Danh sach mssv cac sinh vien vang hoc
                    string[] absentList = absent.Split(',');
                    for (int i = 0; i < svList.Count; i++)
                    {
                        SinhVien sv = new SinhVien();
                        sv = (SinhVien)svList[i];

                        //lay lopsv_ma cua sinh vien trong lop dang xet
                        List<long> ma_lopsv = db.LopHocPhan_SinhVien.Where(x => x.lop_ma.ToLower() == malop.ToLower() && x.sv_ma.ToLower() == sv.sv_ma.ToLower()).Select(y => y.lopsv_stt).ToList();

                        //Sinh vien vang hoc
                        DiemDanh dd;
                        if (absentList.Contains(sv.sv_ma))
                        {
                            dd = new DiemDanh(ma_lopsv[0],
                                    dd_phong, buoihoc, dd_ngay,
                                    true, Request.Form["dd_lydo" + sv.sv_ma].ToString(), "GV001");

                        }

                        else
                        {
                            dd = new DiemDanh(ma_lopsv[0],
                                    dd_phong, buoihoc, dd_ngay,
                                    false, Request.Form["dd_lydo" + sv.sv_ma].ToString(), "GV001");
                        }

                        db.DiemDanhs.Add(dd);
                    }
                    db.SaveChanges();
                }
            }

            else
            {
                ModelState.AddModelError("Error", "Buổi học này đã điểm danh rồi");
            }
        }
    }
}