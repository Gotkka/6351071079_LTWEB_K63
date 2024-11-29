using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThucHanh_22112024.Models;

namespace ThucHanh_22112024.Controllers
{
    public class CustomerController : Controller
    {
        QLBanXeGanMayEntities qLBanXeGanMayEntities = new QLBanXeGanMayEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection form)
        {
            // Retrieve form values
            string hoten = form["txthoten"];
            string ngay = form["ngay"];
            string thang = form["thang"];
            string nam = form["txtnam"];
            string gioitinh = form["gioitinh"];
            string diachi = form["diachi"];
            string dienthoai = form["dienthoai"];
            string email = form["txtemail"];
            string tendangnhap = form["txttendangnhap"];
            string matkhau = form["txtmatkhau"];
            string nhaplaimatkhau = form["txtnhaplaimatkhau"];

            // Perform basic validation (ensure fields are not empty)
            if (string.IsNullOrEmpty(hoten) || string.IsNullOrEmpty(ngay) || string.IsNullOrEmpty(thang) || string.IsNullOrEmpty(nam))
            {
                // Handle invalid input (e.g., display an error message)
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View();
            }

            // Additional validation (e.g., check if the passwords match)
            if (matkhau != nhaplaimatkhau)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View();
            }

            // Convert the date to DateTime
            DateTime? ngaysinh = null;
            if (!string.IsNullOrEmpty(ngay) && !string.IsNullOrEmpty(thang) && !string.IsNullOrEmpty(nam))
            {
                try
                {
                    ngaysinh = new DateTime(int.Parse(nam), int.Parse(thang), int.Parse(ngay));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Invalid birth date.";
                    return View();
                }
            }

            // Create a new customer (KHACHHANG) object
            var customer = new KHACHHANG
            {
                HoTen = hoten,
                Ngaysinh = ngaysinh,
                Taikhoan = tendangnhap,
                Matkhau = matkhau,
                Email = email,
                DiachiKH = diachi,
                DienthoaiKH = dienthoai
            };

            // Assuming db is your DbContext and KHACHHANGs is the DbSet for customers
            try
            {
                // Save the customer to the database
                qLBanXeGanMayEntities.KHACHHANGs.Add(customer);
                qLBanXeGanMayEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error while saving the data. Please try again later.";
                return View();
            }

            // After successful registration, redirect to another page or show a success message
            ViewBag.SuccessMessage = "Registration successful! Please log in.";

            return RedirectToAction("DangNhap", "Customer"); // Redirect after successful registration
        }

        // GET: DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }

        // POST: DangNhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            // Retrieve form values
            string tendangnhap = form["txttendangnhap"];
            string matkhau = form["txtmatkhau"];

            // Validate input
            if (string.IsNullOrEmpty(tendangnhap) || string.IsNullOrEmpty(matkhau))
            {
                ViewBag.ErrorMessage = "Please enter both username and password.";
                return View();
            }

            // Check the credentials against the database
            var user = qLBanXeGanMayEntities.KHACHHANGs.FirstOrDefault(u => u.Taikhoan == tendangnhap && u.Matkhau == matkhau);

            if (user == null)
            {
                // Invalid login attempt
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }

            // Store user information in session
            Session["User"] = user;  // Store the entire user object (optional)

            // Redirect to the home or dashboard page
            return RedirectToAction("Index", "Motorcycle");
        }



    }
}