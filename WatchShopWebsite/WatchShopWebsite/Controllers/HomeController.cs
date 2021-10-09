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
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();
        public ActionResult Index()
        {
            // lấy danh mục
            ViewBag.Categories = db.DanhMucs.Where(t => t.TrangThai == 1);

            // lấy 3 sản phẩm phổ biến (có nhiều lượt xem)
            ViewBag.Take3PopularPros = db.SanPhams.Where(t => t.TrangThai == 1)
                .OrderByDescending(t => t.LuotXem).Take(3);

            // lấy 6 sản phẩm có ưu đãi
            ViewBag.DiscountPros = db.SanPhams.Where(t => t.TrangThai == 1 && t.LoaiSP == 2).Take(6);

            // lấy 8 sản phẩm đặc biệt
            ViewBag.SpecialPros = db.SanPhams.Where(t => t.LoaiSP == 1 && t.TrangThai == 1).Take(8);

            // lấy 8 sản phẩm phổ biến (có nhiều lượt xem)
            ViewBag.Take8PopularPros = db.SanPhams.Where(t => t.TrangThai == 1)
                .OrderByDescending(t => t.LuotXem).Take(8);

            // lấy 12 sản phẩm loại thường (cho sp đề xuất)
            ViewBag.RegularPros = db.SanPhams.Where(t => t.TrangThai == 1 && t.LoaiSP == 0).Take(12);

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