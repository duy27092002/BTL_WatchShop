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

            if (khachHang == null || id == null || Session["IdCustomer"] == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            // lấy avatar khách hàng
            var getUrlImg = khachHang.Avatar.ToString().Replace("~", "../../..");
            ViewBag.GetAvatar = getUrlImg;

            // lấy email khách hàng
            ViewBag.GetEmail = khachHang.Email;

            return View();
        }

        public ActionResult EditCustomer(int? id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);

            if (khachHang == null || id == null || Session["IdCustomer"] == null)
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

                    db.Entry(khachHang).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    khachHang.Avatar = db.KhachHangs.Where(t => t.MaKH == khachHang.MaKH).Select(t => t.Avatar).FirstOrDefault();

                    db.Entry(khachHang).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(khachHang);
        }
    }
}