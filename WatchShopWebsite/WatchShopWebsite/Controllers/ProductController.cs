﻿using Microsoft.AspNetCore.Http;
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

            #region Duy trì hàng hóa đã xem
            if (Session["IdCustomer"] != null)
            {
                // khởi tạo list danh sách sản phẩm đã xem
                var prodsId = new List<string>
                {
                    // thêm sp vào danh sách
                    id.ToString()
                };

                // khởi tạo tên của cookie đại diện cho danh sách sp đã xem
                string viewedByCusId = "viewed-" + (int)Session["IdCustomer"];

                // lấy cookie cũ
                var cookie = Request.Cookies[viewedByCusId];

                // nếu chưa có cookie thì tạo mới
                if (cookie == null)
                {
                    cookie = new HttpCookie(viewedByCusId);
                }

                // bổ sung sản phẩm đã xem vào cookie (maKH, maSP)
                for (var i = 0; i < prodsId.Count; i++)
                {
                    cookie.Values.Add(viewedByCusId, prodsId[i]);
                }

                // đặt thời gian tồn tại của cookie là 1 ngày = 24h
                cookie.Expires = DateTime.Now.AddHours(24);

                // gửi cookie về client để lưu lại
                Response.Cookies.Add(cookie);

                // tạo danh sách chứa mã sản phẩm đã xem từ cookie với key = viewed-(mã khách hàng)
                var productIdList = new List<int>();

                // cắt chuỗi dạng "1,2" => mảng(string): "1", "2" --- với 1, 2 lần lượt là mã sản phẩm
                var handleString = cookie[viewedByCusId].Split(',');

                for (var i = 0; i < handleString.Count(); i++)
                {
                    int getProductId = Convert.ToInt32(handleString[i]);
                    productIdList.Add(getProductId);
                }

                var getProductIdList = productIdList;

                // truy vấn sản phẩm đã xem
                List<SanPham> viewedPros = new List<SanPham>();
                foreach (var item in getProductIdList)
                {
                    var getProInfo = db.SanPhams.Where(t => t.MaSP.Equals(item)).FirstOrDefault();
                    viewedPros.Add(getProInfo);
                }
                ViewBag.ViewedProducts = viewedPros;
            }
            #endregion

            return View(sanPham);
        }

        // đánh giá sản phẩm của người dùng
        public ActionResult Evaluate()
        {
            return View();
        }
    }
}