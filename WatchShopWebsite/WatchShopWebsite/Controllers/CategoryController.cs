using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Controllers
{
    public class CategoryController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();

        // GET: Category
        public ActionResult Index(int? id, int pageSize = 8, int page = 1)
        {
            ViewBag.GetCateName = db.DanhMucs.Where(t => t.MaDM == id).Select(t => t.TenDM).FirstOrDefault();

            var getProductsByCate = db.SanPhams.Where(t => t.MaDM == id).ToList();

            ViewBag.GetCountByCateID = getProductsByCate.Count();
            ViewBag.GetCateID = id;

            // phân trang
            int pageCount = (int)(((getProductsByCate.Count * 1.0) / pageSize) + 0.999999);
            pageCount = pageCount > 0 ? pageCount : 1;
            page = page < pageCount ? page : pageCount;
            getProductsByCate = getProductsByCate.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.GetProductsByCate = getProductsByCate;
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = pageCount;

            return View();
        }
    }
}