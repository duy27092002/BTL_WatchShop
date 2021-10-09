using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.DAO;

namespace WatchShopWebsite.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            // lấy danh sách sản phẩm trong giỏ hàng
            ViewBag.GetCart = (List<CartDAO>)Session["cart"];

            return View();
        }
    }
}