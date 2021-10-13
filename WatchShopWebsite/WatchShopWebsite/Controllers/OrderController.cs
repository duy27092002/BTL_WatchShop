using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.DAO;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Controllers
{
    public class OrderController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();

        // view danh sách đơn hàng của khách hàng (lấy dựa vào id khách hàng)
        public ActionResult Index(int? id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);

            if (khachHang == null || id == null || Session["IdCustomer"] == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            var getOrders =  from info in db.ThongTinVanChuyens
                             join o in db.DonHangs on info.MaVC equals o.MaVC
                             join d in db.CTDonHangs on o.MaDonHang equals d.MaDonHang
                             where o.MaKH == id
                             select new OrdersDAO
                             {
                                 // thông tin vận chuyển
                                 Name = info.TenNguoiNhan,
                                 Address = info.CTDiaChi,
                                 PhoneNumber = info.SDT,

                                 // thông tin đơn hàng
                                 OrderID = o.MaDonHang,
                                 OrderDate = o.ThoiGianMuaHang,
                                 Total = o.TongGia,
                                 OrderStatus = o.TrangThaiDH,

                                 // thông tin chi tiết đơn hàng
                                 OrderDetails = d
                             };

            return View(getOrders);
        }

        // view thanh toán đơn hàng + đăng ký thông tin vận chuyển
        // GET: Order
        public ActionResult Checkout()
        {
            if (Session["cart"] != null && Session["IdCustomer"] != null)
            {
                // lấy danh sách sản phẩm trong giỏ hàng
                var listCart = (List<CartDAO>)Session["cart"];
                ViewBag.GetCart = listCart;

                // lấy tổng tiền trong giỏ hàng khi đã sẵn sàng mua hàng
                Session["TotalMoney"] = listCart.Sum(t => t.SanPham.Gia * t.Quantity);
            } else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // thực hiện đăng ký thông tin vận chuyển
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(ThongTinVanChuyen request)
        {
            if (ModelState.IsValid)
            {
                // gán dữ liệu cho bảng ThongTinVanChuyen
                ThongTinVanChuyen obj = new ThongTinVanChuyen();
                obj.TenNguoiNhan = request.TenNguoiNhan;
                obj.CTDiaChi = request.CTDiaChi;
                obj.SDT = request.SDT;
                obj.GhiChu = request.GhiChu;

                // lưu thông tin vận chuyển
                db.ThongTinVanChuyens.Add(obj);
                db.SaveChanges();

                // lấy MaVC vừa mới tạo để lưu vào bảng DonHang
                int getShippingId = obj.MaVC;

                // tiến hành lưu thông tin vào bảng DonHangg
                var listCart = (List<CartDAO>)Session["cart"];

                // gán dữ liệu cho bảng DonHang
                DonHang donHang = new DonHang();
                donHang.MaKH = int.Parse(Session["IdCustomer"].ToString());
                donHang.MaVC = getShippingId;
                donHang.TongGia = listCart.Sum(t => t.SanPham.Gia * t.Quantity);
                donHang.ThoiGianMuaHang = DateTime.Now;
                donHang.TrangThaiDH = 0; // đặt mặc định trạng thái đơn hàng  = 0, tức là đang xử lý

                // lưu dữ liệu trên vào bảng DonHang
                db.DonHangs.Add(donHang);
                db.SaveChanges();

                // lấy MaDonHang vừa mới tạo để lưu vào bảng CTDonHang
                int getOrderId = donHang.MaDonHang;

                List<CTDonHang> listOrderDetails = new List<CTDonHang>();

                foreach (var item in listCart)
                {
                    CTDonHang orderDetails = new CTDonHang();
                    orderDetails.MaDonHang = getOrderId;
                    orderDetails.MaSP = item.SanPham.MaSP;
                    orderDetails.SoLuongMua = item.Quantity;
                    orderDetails.Gia = item.SanPham.Gia;
                    orderDetails.HinhAnh = item.SanPham.HinhAnh;

                    listOrderDetails.Add(orderDetails);
                }

                db.CTDonHangs.AddRange(listOrderDetails);
                db.SaveChanges();

                return RedirectToAction("SuccessfulPurchase");
            }

            return View(request);
        }

        // giao diện khi khách hàng đăng ký thông tin vận chuyển và mua hàng thành công
        public ActionResult SuccessfulPurchase()
        {
            Session.Remove("cart");
            Session.Remove("count");
            Session.Remove("TotalMoney");
            return View();
        }
    }
}