using Microsoft.AspNetCore.Http;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Controllers
{
    public class ProductController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();
        // GET: Product
        public ActionResult Index(string currentFilter, int? productType, string keyword, int? page)
        {
            if (keyword != null && productType != null)
            {
                page = 1;
            }
            else
            {
                keyword = currentFilter;
            }

            ViewBag.CurrentFilter = keyword;
            ViewBag.CurrentType = productType;


            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo tên sản phẩm
            if (!String.IsNullOrEmpty(keyword) && productType != null)
            {
                // truy vấn danh sách sp trong db
                var sanPhams = db.SanPhams.Select(t => t);

                if (productType == 0) // lấy tất cả sp
                {
                    sanPhams = sanPhams.Where(s => s.TenSP.Contains(keyword) && s.TrangThai == 1).OrderByDescending(s => s.MaSP);
                }
                else if (productType == 1) // lấy sp đặc biệt
                {
                    sanPhams = sanPhams.Where(s => s.TenSP.Contains(keyword) && s.LoaiSP == 1 && s.TrangThai == 1).OrderByDescending(s => s.MaSP);
                }
                else if (productType == 2) // lấy sp ưu đãi
                {
                    sanPhams = sanPhams.Where(s => s.TenSP.Contains(keyword) && s.LoaiSP == 2 && s.TrangThai == 1).OrderByDescending(s => s.MaSP);
                }
                else if (productType == 3) // lấy sản phẩm mới nhất
                {
                    sanPhams = sanPhams.Where(s => s.TenSP.Contains(keyword) && s.TrangThai == 1).OrderByDescending(s => s.MaSP);
                } else
                {
                    return View();
                }

                int pageSize = 8; // số lượng trên 1 trang
                int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1

                return View(sanPhams.ToPagedList(pageNumber, pageSize));
            }

            return View();
        }
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

            #region cách 1
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

            // truy vấn sản phẩm đã xem
            ViewBag.ViewedProducts = db.SanPhams.Where(t => keys.Contains(t.MaSP));
            #endregion

            #region cách 2
            /*// khởi tạo list danh sách sản phẩm đã xem
            var prodsId = new List<string>();

            // khởi tạo tên của cookie đại diện cho danh sách sp đã xem
            string viewedByCusId = "viewed-" + (int)Session["IdCustomer"];

            // thêm sp vào danh sách
            prodsId.Add(id.ToString());

            // lấy cookie cũ
            var cookie = Request.Cookies[viewedByCusId];

            // nếu chưa có cookie thì tạo mới
            if (cookie == null)
            {
                cookie = new HttpCookie(viewedByCusId, prodsId.ToString());
            }

            // bổ sung sản phẩm đã xem vào cookie (maKH, maSP)
            cookie.Values.Add(viewedByCusId, prodsId.ToString());

            // đặt thời gian tồn tại của cookie là 1 ngày = 24h
            cookie.Expires = DateTime.Now.AddHours(24);

            // gửi cookie về client để lưu lại
            Response.Cookies.Add(cookie);

            // lấy danh sách chứa mã sản phẩm đã xem từ cookie với key = viewed-(mã khách hàng)
            var test = new List<string>();
            test.Add(cookie.Values.AllKeys.Select(t => t.Equals(viewedByCusId)).ToString());*/
            #endregion

            return View(sanPham);
        }
    }
}