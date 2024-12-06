using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThucHanh_22112024.Models;
using System.Web.Script.Serialization;

namespace ThucHanh_22112024.Controllers
{
    public class AdminController : Controller
    {
        QLBanXeGanMayEntities qLBanXeGanMayEntities = new QLBanXeGanMayEntities();
        // GET: Admin
        public ActionResult Index()
        {
            // Kiểm tra xem user đã đăng nhập chưa
            if (Session["Admin"] != null)
            {
                var admin = Session["Admin"] as dynamic;
                ViewBag.Hoten = admin.Hoten; // Truyền thông tin sang View
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Admin ad = qLBanXeGanMayEntities.Admins.SingleOrDefault(n => n.UserAdmin == username && n.PassAdmin == password);
            if (ad != null)
            {
                Session["Admin"] = new { UserAdmin = ad.UserAdmin, Hoten = ad.Hoten };
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Notification = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }

        public ActionResult XeGanMay(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            //return View(qLBanXeGanMayEntities.XEGANMAYs.ToList());
            return View(qLBanXeGanMayEntities.XEGANMAYs.ToList().OrderBy(n => n.MaXe).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            //ViewBag.MaLX = new SelectList(qLBanXeGanMayEntities.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            //ViewBag.MaNPP = new SelectList(qLBanXeGanMayEntities.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNhaPhanPhoi");
            // Populate dropdown list for MaLX and MaNPP
            var loaiXeList = qLBanXeGanMayEntities.LOAIXEs
                                     .OrderBy(n => n.TenLoaiXe)
                                     .ToList();
            ViewBag.MaLX = new SelectList(loaiXeList, "MaLX", "TenLoaiXe");

            var nppList = qLBanXeGanMayEntities.NHAPHANPHOIs
                                     .OrderBy(n => n.TenNPP)
                                     .ToList();
            ViewBag.MaNPP = new SelectList(nppList, "MaNPP", "TenNPP");

            return View();

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(XEGANMAY xe, HttpPostedFileBase fileUpLoad)
        {
            var loaiXeList = qLBanXeGanMayEntities.LOAIXEs
                                     .OrderBy(n => n.TenLoaiXe)
                                     .ToList();
            ViewBag.MaLX = new SelectList(loaiXeList, "MaLX", "TenLoaiXe");

            var nppList = qLBanXeGanMayEntities.NHAPHANPHOIs
                                     .OrderBy(n => n.TenNPP)
                                     .ToList();
            ViewBag.MaNPP = new SelectList(nppList, "MaNPP", "TenNPP");

            if (ModelState.IsValid)
            {
                if (fileUpLoad == null)
                {
                    ViewBag.Notification = "Vui lòng chọn ảnh bìa";
                    return View();
                }

                var fileName = Path.GetFileName(fileUpLoad.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/XE"), fileName);

                if (System.IO.File.Exists(path))
                {
                    ViewBag.Notification = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpLoad.SaveAs(path);
                    xe.Anhbia = "Images/XE/" + fileName;

                    qLBanXeGanMayEntities.XEGANMAYs.Add(xe);
                    qLBanXeGanMayEntities.SaveChanges();

                    return RedirectToAction("XeGanMay");
                }
            }
            else
            {
                ViewBag.Notification = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại!";
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            XEGANMAY xe = qLBanXeGanMayEntities.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xe.MaXe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }

        public ActionResult Delete(int id)
        {
            XEGANMAY xe = qLBanXeGanMayEntities.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            ViewBag.MaXe = xe.MaXe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(xe);
        }

        [HttpPost]
        public ActionResult VerifyDelete(int id)
        {
            XEGANMAY xe = qLBanXeGanMayEntities.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            if (xe == null)
            {
                return HttpNotFound();
            }

            // Perform the deletion
            qLBanXeGanMayEntities.XEGANMAYs.Remove(xe);
            qLBanXeGanMayEntities.SaveChanges();

            return RedirectToAction("XeGanMay");
        }

        public ActionResult Edit(int id)
        {
            // Kiểm tra giá trị id nhận được từ URL
            if (id <= 0)
            {
                ViewBag.Notification = "ID không hợp lệ.";
                return RedirectToAction("XeGanMay");
            }

            // Lấy thông tin xe theo MaXe
            var xe = qLBanXeGanMayEntities.XEGANMAYs.SingleOrDefault(x => x.MaXe == id);

            if (xe == null)
            {
                ViewBag.Notification = "Không tìm thấy xe tương ứng.";
                return RedirectToAction("XeGanMay"); // Hoặc chuyển hướng đến trang danh sách xe
            }

            // Lấy danh sách loại xe và nhà phân phối để hiển thị trong dropdown
            ViewBag.MaLX = new SelectList(qLBanXeGanMayEntities.LOAIXEs, "MaLX", "TenLoaiXe", xe.MaLX);
            ViewBag.MaNPP = new SelectList(qLBanXeGanMayEntities.NHAPHANPHOIs, "MaNPP", "TenNPP", xe.MaNPP);

            // Trả về view với mô hình xe đã lấy
            return View(xe);
        }





        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(XEGANMAY xe, HttpPostedFileBase fileUpLoad)
        {
            // Lấy danh sách loại xe và nhà phân phối để hiển thị trong dropdown
            var loaiXeList = qLBanXeGanMayEntities.LOAIXEs.OrderBy(n => n.TenLoaiXe).ToList();
            var nppList = qLBanXeGanMayEntities.NHAPHANPHOIs.OrderBy(n => n.TenNPP).ToList();

            ViewBag.MaLX = new SelectList(loaiXeList, "MaLX", "TenLoaiXe", xe.MaLX);
            ViewBag.MaNPP = new SelectList(nppList, "MaNPP", "TenNPP", xe.MaNPP);

            // Kiểm tra tính hợp lệ của dữ liệu
            if (!ModelState.IsValid)
            {
                ViewBag.Notification = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại!";
                return View(xe);
            }

            // Tìm xe theo MaXe
            var existingXe = qLBanXeGanMayEntities.XEGANMAYs.SingleOrDefault(n => n.MaXe == xe.MaXe);
            if (existingXe == null)
            {
                ViewBag.Notification = "Không tìm thấy xe tương ứng.";
                return View(xe);
            }

            // Cập nhật thông tin xe
            existingXe.TenXe = xe.TenXe;
            existingXe.Giaban = xe.Giaban;
            existingXe.Mota = xe.Mota;
            existingXe.MaLX = xe.MaLX;
            existingXe.MaNPP = xe.MaNPP;
            existingXe.Ngaycapnhat = DateTime.Now;

            // Xử lý ảnh bìa
            if (fileUpLoad != null)
            {
                // Lấy tên file ảnh
                var fileName = Path.GetFileName(fileUpLoad.FileName);

                // Đảm bảo thư mục chứa ảnh đã tồn tại
                var folderPath = Server.MapPath("~/Content/Images/XE");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath); // Tạo thư mục nếu không tồn tại
                }

                // Tạo đường dẫn lưu ảnh
                var path = Path.Combine(folderPath, fileName);

                // Nếu ảnh chưa tồn tại trong thư mục, lưu ảnh mới
                if (!System.IO.File.Exists(path))
                {
                    fileUpLoad.SaveAs(path); // Lưu ảnh vào thư mục
                    existingXe.Anhbia = "Images/XE/" + fileName; // Cập nhật đường dẫn ảnh vào cơ sở dữ liệu
                }
                else
                {
                    // Nếu ảnh đã tồn tại, giữ lại đường dẫn cũ
                    ViewBag.Notification = "Ảnh này đã tồn tại. Vui lòng chọn ảnh khác.";
                    return View(xe);
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            try
            {
                qLBanXeGanMayEntities.SaveChanges();
                return RedirectToAction("XeGanMay"); // Chuyển hướng về trang danh sách xe sau khi lưu thành công
            }
            catch (Exception ex)
            {
                ViewBag.Notification = "Lỗi khi lưu thay đổi: " + ex.Message;
                return View(xe);
            }
        }


        public ActionResult Statistical()
        {
            // Lấy danh sách số lượng xe theo loại xe
            var xeData = qLBanXeGanMayEntities.XEGANMAYs
                                .GroupBy(x => x.MaLX)
                                .Select(g => new
                                {
                                    LoaiXe = g.Key,
                                    SoLuong = g.Count()
                                })
                                .ToList();  // Thực thi trên cơ sở dữ liệu và lấy kết quả

            // Lấy tất cả MaLX từ xeData
            var maLXList = xeData.Select(d => d.LoaiXe).ToList();

            // Lấy tên các loại xe từ bảng LOAIXE chỉ với MaLX có trong xeData
            var loaiXeNames = qLBanXeGanMayEntities.LOAIXEs
                                .Where(l => maLXList.Contains(l.MaLX))
                                .ToList()
                                .ToDictionary(l => l.MaLX.ToString(), l => l.TenLoaiXe);  // Chuyển MaLX thành chuỗi

            // Truyền dữ liệu vào ViewBag
            ViewBag.LoaiXeNames = loaiXeNames;
            ViewBag.XeData = xeData;

            return View();
        }






    }
}