using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Controllers
{
    public class HomeController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();
        public ActionResult Index()
        {
            ViewBag.Categories = db.DanhMucs.Where(t => t.TrangThai == 1).ToList();
            ViewBag.Products = db.SanPhams.Where(t => t.TrangThai == 1).Take(6).ToList();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}