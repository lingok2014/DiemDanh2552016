using QuanLyDiemDanh.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyDiemDanh.Controllers
{
    public class ThongKeController : Controller
    {
        private DiemDanhSinhVienEntities db = new DiemDanhSinhVienEntities();
        // GET: ThongKe
        public ActionResult Sum()
        {
            return View();
        }
        /// <summary>
        /// Hàm lấy mã lớp
        /// </summary>
        /// <param name="term">ký tự của lớp</param>
        /// <returns>1 list danh sách lớp có trong cơ sở dữ liệu</returns>
        public JsonResult getMaLop(string term)
        {
            //List<string> res = db.LopHocPhans.Where(x => x.lop_ma.StartsWith(term)).Select(y => y.lop_ma).ToList();
            //return Json(res, JsonRequestBehavior.AllowGet);

            List<string> result = db.LopHocPhans.Where(x => x.lop_ma.StartsWith(term)).
               Select(y => y.lop_ma).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getSum(string term)
        {
            var result = (from a in db.SinhViens
                          join b in db.LopHocPhan_SinhVien on a.sv_ma equals b.sv_ma
                          join c in db.DiemDanhs on b.lopsv_stt equals c.lopsv_stt
                          where b.lop_ma.ToLower() == term.ToLower()
                          orderby a.sv_ma
                          select new
                          {
                              a.sv_ma,
                              a.sv_ten,
                              c.dd_vang,
                              tongsobuoi = (from e in db.LopHocPhan_SinhVien
                                            join f in db.DiemDanhs on e.lopsv_stt equals f.lopsv_stt
                                            where e.lop_ma.ToLower() == term.ToLower()
                                            select f.buoihoc).Max()

                          }).ToList();
            ArrayList rows = new ArrayList();//mỗi dòng chứa thông tin vắng học của một sinh viên
            if (result.Count > 0)
            {
                //xử lý
                int tongsobuoi = result[0].tongsobuoi;
                int tongsosinhvien = result.Count / tongsobuoi;
                //Lấy danh sách sinh viên
                ArrayList dsSinhVien = new ArrayList();
                for (int i = 0; i < result.Count; i += tongsobuoi)
                {
                    ArrayList thongTinSV = new ArrayList();
                    thongTinSV.Add(result[i].sv_ma);
                    thongTinSV.Add(result[i].sv_ten);
                    dsSinhVien.Add(thongTinSV);

                }
                rows.Add(dsSinhVien);
                //Tạo mảng chứa thông tin vắng học và có học của sinh viên
                int count = 0;
                for (int i = 0; i < tongsosinhvien; i++)
                {
                    ArrayList thongkeVangHoc = new ArrayList();
                    int vanghoc = 0;
                    int cohoc = 0;
                    for (int j = 0; j < tongsobuoi; j++)
                    {
                        var buoi = result[count++].dd_vang;
                        if (buoi == true)
                        {
                            vanghoc++;
                        }
                        cohoc = tongsobuoi - vanghoc;
                    }
                    thongkeVangHoc.Add(cohoc);
                    thongkeVangHoc.Add(vanghoc);
                    rows.Add(thongkeVangHoc);
                }

            }
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}