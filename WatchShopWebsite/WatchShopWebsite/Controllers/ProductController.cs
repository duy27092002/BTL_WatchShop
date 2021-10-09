using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Controllers
{
    public class ProductController : Controller
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();
        // GET: Product
        public ActionResult Details(int? id)
        {
            SanPham sanPham = db.SanPhams.Find(id);

            if (sanPham == null || id == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            // lấy 6 sản phẩm cùng danh mục
            ViewBag.SimilarPros = db.SanPhams.Where(t => t.TrangThai == 1 &&
                t.MaDM == sanPham.MaDM && t.MaSP != id).Take(6);

            // tăng số lượt xem cho mỗi sản phẩm khi người dùng nhấp vào xem chi tiết
            sanPham.LuotXem++;
            db.SaveChanges();

            // lấy cookie cũ tên viewed
            var viewed = Request.Cookies["viewed"];

            // nếu chưa có cookie thì tạo mới
            if (viewed == null)
            {
                viewed = new HttpCookie("viewed");
            }

            // bổ sung sản phẩm đã xem vào cookie
            viewed.Values[id.ToString()] = id.ToString();

            // đặt thời gian tồn tại của cookie là 1 ngày = 24h
            viewed.Expires = DateTime.Now.AddHours(24);

            // gửi cookie về client để lưu lại
            Response.Cookies.Add(viewed);

            // lấy List<int> chứa mã sản phẩm đã xem từ cookie
            var keys = viewed.Values.AllKeys.Select(t => int.Parse(t)).ToList();

            // truy vẫn sản phẩm đã xem
            ViewBag.ViewedProducts = db.SanPhams.Where(t => keys.Contains(t.MaSP));

            return View(sanPham);
        }
    }
}