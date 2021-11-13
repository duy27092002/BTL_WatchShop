using Microsoft.AspNetCore.Http;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            ViewBag.SimilarPros = db.SanPhams.Where(
                t => t.TrangThai == 1 &&
                t.MaDM == sanPham.MaDM &&
                t.MaSP != id
                ).Take(6);

            // tăng số lượt xem cho mỗi sản phẩm khi người dùng nhấp vào xem chi tiết
            sanPham.LuotXem++;
            db.SaveChanges();

            #region Duy trì hàng hóa đã xem
            if (Session["IdCustomer"] != null)
            {
                // khởi tạo tên của cookie đại diện cho danh sách sp đã xem
                string viewedByCusId = "viewed-" + (int)Session["IdCustomer"];

                // lấy cookie cũ
                var cookie = Request.Cookies[viewedByCusId];

                // nếu chưa có cookie thì tạo mới
                if (cookie == null)
                {
                    cookie = new HttpCookie(viewedByCusId);
                }

                // nếu k trùng lặp mã sp
                if (CheckViewedProd(id) != -1) 
                {
                    // bổ sung sản phẩm đã xem vào cookie (viewedByCusId, maSP)
                    cookie.Values.Add(viewedByCusId, id.ToString());

                    // đặt thời gian tồn tại của cookie là 1 ngày = 24h
                    cookie.Expires = DateTime.Now.AddHours(24);

                    // gửi cookie về client để lưu lại
                    Response.Cookies.Add(cookie);
                }

                // cắt chuỗi dạng "1,2" => mảng(string): "1", "2" --- với 1, 2 lần lượt là mã sản phẩm
                var handleString = cookie[viewedByCusId].Split(',');

                // tạo danh sách chứa mã sản phẩm đã xem từ cookie với key = viewed-(mã khách hàng)
                var productIdList = new List<int>();

                for (var i = 0; i < handleString.Count(); i++)
                {
                    int getProductId = Convert.ToInt32(handleString[i]);
                    productIdList.Add(getProductId);
                }

                // truy vấn sản phẩm đã xem
                List<SanPham> viewedPros = new List<SanPham>();
                foreach (var item in productIdList)
                {
                    var getProInfo = db.SanPhams.Where(t => t.MaSP.Equals(item)).FirstOrDefault();
                    viewedPros.Add(getProInfo);
                }

                ViewBag.ViewedProducts = viewedPros;
            }
            #endregion

            return View(sanPham);
        }

        // kiểm tra xem trong cookie của khách hàng có sp đã xem trùng lặp k?
        private int CheckViewedProd(int? id)
        {
            string viewedByCusId = "viewed-" + (int)Session["IdCustomer"];

            // lấy cookie cũ
            var cookie = Request.Cookies[viewedByCusId];

            // nếu chưa có cookie thì tạo mới
            if (cookie == null)
            {
                cookie = new HttpCookie(viewedByCusId);
            }

            // lấy danh sách mã sp trong cookie khách hàng
            var getViewedProdByCookie = cookie[viewedByCusId];

            // nếu đã tồn tại cookie của khách hàng
            if (getViewedProdByCookie != null)
            {
                var handleString = cookie[viewedByCusId].Split(',');
                // kiểm tra sự trùng lặp
                for (var x = 0; x < handleString.Count(); x++)
                {
                    if (Convert.ToInt32(handleString[x]) == id)
                    {
                        // nếu trùng lặp
                        return -1;
                    }
                }
            }

            // nếu k trùng lặp
            return 1;
        }

        // đánh giá sản phẩm của người dùng
        [HttpPost]
        public ActionResult Evaluate(DanhGiaSP getEvaluate)
        {
            // kiểm tra đánh giá trùng lặp
            var temp = db.DanhGiaSPs.Where(
                t => t.MaDH == getEvaluate.MaDH &&
                t.MaKH == getEvaluate.MaKH &&
                t.MaSP == getEvaluate.MaSP
                );

            // nếu không có thì thêm vào bảng DanhGiaSP
            if (temp.Count() <= 0)
            {
                db.DanhGiaSPs.Add(getEvaluate);

                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false });
                }
            } else // nếu có thì trả ra thông báo: đã có đánh giá rồi
            {
                return Json(new { error = true });
            }
        }
    }
}