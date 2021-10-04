using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();

        // GET: Admin/Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(NhanVien model)
        {
            var checkLogin = db.NhanViens.Where(t => t.Email == model.Email
                     && t.MatKhau == model.MatKhau
                     && t.TenDangNhap.ToLower() == model.TenDangNhap.ToLower()
                     && t.TrangThai == 1).ToList();

            if (checkLogin.Count() > 0)
            {
                FormsAuthentication.SetAuthCookie(model.TenDangNhap, false);
                Session["idEmployee"] = checkLogin.FirstOrDefault().MaNV;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "1. Sai thông tin đăng nhập!");
            ModelState.AddModelError("", "2. Có thể bạn đã không được cho phép truy cập vào trang!");
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(NhanVien model)
        {
            if (ModelState.IsValid)
            {
                db.NhanViens.Add(model);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}