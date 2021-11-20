using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Controllers
{
    public class WishlistController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();

        // thêm vào danh sách yêu thích
        public ActionResult AddToWishlist(int? id)
        {
            // tìm sản phẩm theo id
            SanPham getProductInfo = db.SanPhams.Find(id);

            // nếu tồn tại sản phẩm thì thêm vào danh sách yêu thích, ngược lại thì báo lỗi
            if (getProductInfo != null && id != null && Session["IdCustomer"] != null)
            {
                SPYeuThich wishlist = new SPYeuThich();
                wishlist.MaKH = (int)Session["IdCustomer"];
                wishlist.MaSP = getProductInfo.MaSP;

                // kiểm tra xem trong danh sách đã có sản phẩm này hay chưa?
                var checkProd = db.SPYeuThiches.Where(t => t.MaSP == wishlist.MaSP && t.MaKH == wishlist.MaKH);
                if (checkProd.Count() <= 0)
                {
                    db.SPYeuThiches.Add(wishlist);
                    db.SaveChanges();
                    return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
                }
            }
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        // lấy danh sách sản phẩm yêu thích theo mã khách hàng
        public ActionResult Index(int? id, int pageSize = 4, int page = 1)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);

            if (khachHang == null || id == null || Session["IdCustomer"] == null || khachHang.MaKH != (int)Session["IdCustomer"])
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            var getWishlistByCustomer = db.SPYeuThiches.Where(t => t.MaKH == id).OrderByDescending(t => t.ID).Select(t => t);
            ViewBag.GetCount = getWishlistByCustomer.Count();

            // phân trang
            int pageCount = (int)(((getWishlistByCustomer.Count() * 1.0) / pageSize) + 0.999999);
            pageCount = pageCount > 0 ? pageCount : 1;
            page = page < pageCount ? page : pageCount;
            getWishlistByCustomer = getWishlistByCustomer.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.GetWishlistByCustomer = getWishlistByCustomer;
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = pageCount;

            return View();
        }

        // xóa sản phẩm khỏi danh sách yêu thích
        public ActionResult RemoveWishlist(int? id)
        {
            int getIdCustomer = (int)Session["IdCustomer"];

            SPYeuThich model = db.SPYeuThiches.Where(t => t.MaSP == id && t.MaKH == getIdCustomer).FirstOrDefault();

            if (model == null || id == null || Session["IdCustomer"] == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            db.SPYeuThiches.Remove(model);
            db.SaveChanges();

            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }
    }
}