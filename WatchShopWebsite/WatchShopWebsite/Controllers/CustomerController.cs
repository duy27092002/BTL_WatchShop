using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Controllers
{
    public class CustomerController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();

        // GET: Customer
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(KhachHang khachHang)
        {
            var checkLoign = db.KhachHangs.Where(t => t.Email == khachHang.Email &&
                                t.TenDangNhap.ToLower() == khachHang.TenDangNhap.ToLower() &&
                                t.MatKhau == khachHang.MatKhau && t.TrangThai == 1).ToList();
            if (checkLoign.Count() > 0)
            {
                Session["Username"] = checkLoign.FirstOrDefault().TenDangNhap;
                Session["IdCustomer"] = checkLoign.FirstOrDefault().MaKH;

                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError("", "1. Sai thông tin đăng nhập!");
            ModelState.AddModelError("", "2. Có thể bạn đã không được cho phép truy cập vào trang!");

            return View(khachHang);
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                // tạo sẵn 2 trường mặc định
                model.Avatar = "~/Content/images/avatars/add.jpg";
                model.TrangThai = 1;

                db.KhachHangs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult CustomerProfile(int? id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);

            if (khachHang == null || id == null || Session["IdCustomer"] == null || khachHang.MaKH != (int)Session["IdCustomer"])
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            // lấy avatar khách hàng
            var getUrlImg = khachHang.Avatar.ToString().Replace("~", "../../..");
            ViewBag.GetAvatar = getUrlImg;

            // lấy email khách hàng
            ViewBag.GetEmail = khachHang.Email;

            // lấy tổng số đơn hàng
            ViewBag.GetOrdersCount = db.DonHangs.Where(t => t.MaKH == id).Count();

            // lấy tổng số sản phẩm yêu thích
            ViewBag.GetWishlistsCount = db.SPYeuThiches.Where(t => t.MaKH == id).Count();

            // lấy tổng số đơn hàng đang chờ xử lý
            ViewBag.Processing = db.DonHangs.Where(t => t.MaKH == id && t.TrangThaiDH == 0).Count();

            // lấy tổng số đơn hàng đang chờ giao
            ViewBag.PendingDelivery = db.DonHangs.Where(t => t.MaKH == id && t.TrangThaiDH == 1).Count();

            // lấy tổng số đơn hàng đã giao thành công
            ViewBag.Delivered = db.DonHangs.Where(t => t.MaKH == id && t.TrangThaiDH == 2).Count();

            return View();
        }

        public ActionResult EditCustomer(int? id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);

            if (khachHang == null || id == null || Session["IdCustomer"] == null || khachHang.MaKH != (int)Session["IdCustomer"])
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                if (khachHang.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(khachHang.ImageUpload.FileName);
                    string extention = Path.GetExtension(khachHang.ImageUpload.FileName);
                    fileName += extention;
                    khachHang.Avatar = "~/Content/images/avatars/" + fileName;
                    khachHang.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/avatars/"), fileName));
                }
                else
                {
                    khachHang.Avatar = db.KhachHangs.Where(t => t.MaKH == khachHang.MaKH).Select(t => t.Avatar).FirstOrDefault();
                }

                khachHang.TrangThai = 1;
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(khachHang);
        }
    }
}