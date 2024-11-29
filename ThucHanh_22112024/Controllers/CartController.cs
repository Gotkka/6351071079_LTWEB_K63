using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThucHanh_22112024.Models;

namespace ThucHanh_22112024.Controllers
{
    public class CartController : Controller
    {
        // Get the cart from the session
        QLBanXeGanMayEntities qLBanXeGanMayEntities = new QLBanXeGanMayEntities();

        public List<Cart> GetCart()
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }

        // Add a motorcycle to the cart
        public ActionResult AddCart(int id, string returnUrl)
        {
            // Lấy danh sách giỏ hàng từ session
            List<Cart> listCart = GetCart();

            // Tìm sản phẩm trong giỏ hàng theo ID
            Cart product = listCart.FirstOrDefault(n => n.motorcycleId == id);

            if (product == null)
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, tạo mới và thêm vào giỏ hàng
                product = new Cart(id);
                listCart.Add(product);
            }
            else
            {
                // Nếu sản phẩm đã có trong giỏ, tăng số lượng lên 1
                product.quantity++;
            }

            // Lưu lại danh sách giỏ hàng vào session
            Session["Cart"] = listCart;
            Session["TotalQuantity"] = totalQuantity();

            // Điều hướng người dùng về URL trước đó (returnUrl)
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Action("Index", "Motorcycle"); // Hoặc trang mặc định của bạn
            }

            return Redirect(returnUrl);
        }




        // Calculate total quantity of items in the cart
        public int totalQuantity()
        {
            // Lấy danh sách giỏ hàng từ session
            List<Cart> listCart = Session["Cart"] as List<Cart>;

            // Kiểm tra nếu giỏ hàng không null và không rỗng
            if (listCart != null && listCart.Count > 0)
            {
                // Tính tổng số lượng sản phẩm trong giỏ
                return listCart.Sum(n => n.quantity);
            }

            // Nếu giỏ hàng không có sản phẩm, trả về 0
            return 0;
        }


        // Calculate total price of items in the cart
        public double totalPrice()
        {
            // Lấy danh sách giỏ hàng từ session
            List<Cart> listCart = Session["Cart"] as List<Cart>;

            // Kiểm tra nếu giỏ hàng không null và không rỗng
            if (listCart != null && listCart.Count > 0)
            {
                // Tính tổng giá trị các sản phẩm trong giỏ
                return listCart.Sum(n => n.totalMoney);
            }

            // Nếu giỏ hàng không có sản phẩm, trả về 0
            return 0;
        }


        // Show the cart content
        public ActionResult Index()
        {
            if (Session["user"] == null || Session["user"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Customer");
            }
            if (Session["Cart"] == null || !((List<Cart>)Session["Cart"]).Any())
            {
                return RedirectToAction("Index", "Motorcycle");
            }
            // Lấy giỏ hàng từ session
            List<Cart> listCart = GetCart();

            // Kiểm tra nếu giỏ hàng rỗng hoặc không tồn tại
            if (listCart == null || listCart.Count == 0)
            {
                return RedirectToAction("Index", "Motorcycle"); // Nếu giỏ hàng rỗng, chuyển hướng đến trang sản phẩm
            }

            // Tính tổng số lượng và tổng giá trị giỏ hàng
            Session["TotalQuantity"] = totalQuantity();
            Session["TotalPrice"] = totalPrice();

            // Trả về view với danh sách giỏ hàng
            return View(listCart);
        }

        public ActionResult UpdateCart(int id, int quantity)
        {
            List<Cart> listCart = GetCart();
            Cart product = listCart.Find(n => n.motorcycleId == id);
            if (product != null)
            {
                product.quantity = quantity; // Update the quantity of the motorcycle in the cart
            }

            // Recalculate total quantity and total price
            Session["TotalQuantity"] = totalQuantity();
            Session["TotalPrice"] = totalPrice();

            return RedirectToAction("Index");
        }

        // Remove a motorcycle from the cart
        public ActionResult RemoveCart(int id)
        {
            List<Cart> listCart = GetCart();
            Cart product = listCart.Find(n => n.motorcycleId == id);
            if (product != null)
            {
                listCart.Remove(product); // Remove the product from the cart
            }

            // Recalculate total quantity and total price
            Session["TotalQuantity"] = totalQuantity();
            Session["TotalPrice"] = totalPrice();

            return RedirectToAction("Index"); // Redirect to the cart page after removing the item
        }


        public ActionResult RemoveAll()
        {
            List<Cart> listCart = GetCart();
            listCart.Clear(); // Clear all items from the cart
            Session["Cart"] = listCart; // Save the empty list back to session
            Session["TotalQuantity"] = totalQuantity();
            return RedirectToAction("Index", "Motorcycle"); // Redirect to the cart page (or any other page you prefer)
        }


        public ActionResult Order(FormCollection collection)
        {
            KHACHHANG kh = (KHACHHANG)Session["User"];
            List<Cart> cart = GetCart();

            // Check if 'NgayGiao' is null or empty, if so set a default date
            string ngayGiaoString = collection["NgayGiao"];
            DateTime ngayGiaoDate;

            // Default to tomorrow if 'NgayGiao' is not provided
            if (string.IsNullOrEmpty(ngayGiaoString) || !DateTime.TryParse(ngayGiaoString, out ngayGiaoDate))
            {
                ngayGiaoDate = DateTime.Now.AddDays(1); // Default to tomorrow if no valid date provided
            }

            // Create a new order
            DONDATHANG ddh = new DONDATHANG
            {
                MaKH = kh.MaKH,
                Ngaydat = DateTime.Now,
                Ngaygiao = ngayGiaoDate,
                Tinhtranggiaohang = false,
                Dathanhtoan = false
            };

            // Add the cart details to the order
            foreach (var item in cart)
            {
                CHITIETDONTHANG orderDetail = new CHITIETDONTHANG
                {
                    MaDonHang = ddh.MaDonHang, // This should be auto-generated or assigned
                    MaXe = item.Motorcycle.MaXe,
                    Soluong = item.quantity,
                    Dongia = item.Motorcycle.Giaban,
                };
                ddh.CHITIETDONTHANGs.Add(orderDetail);
            }

            // Save the order to the database
            qLBanXeGanMayEntities.DONDATHANGs.Add(ddh);
            qLBanXeGanMayEntities.SaveChanges();

            // Clear the cart after the order is placed
            RemoveAll();

            // Redirect to the order confirmation page
            return RedirectToAction("ConfirmOrder", "Cart");
        }



        public ActionResult ConfirmOrder()
        {
            return View();
        }
    }
}
