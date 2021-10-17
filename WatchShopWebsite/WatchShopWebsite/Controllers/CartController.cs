using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.DAO;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Controllers
{
    public class CartController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();

        // GET: Cart
        public ActionResult Index()
        {
            return View((List<CartDAO>)Session["cart"]);
        }

        // thêm vào giỏ hàng
        public ActionResult AddToCart(int id, int quantity)
        {
            var getProductInfo = db.SanPhams.Find(id);

            // nếu chưa có sản phẩm nào trong giỏ hàng thì tạo session mới
            if (Session["cart"] == null)
            {
                List<CartDAO> cart = new List<CartDAO>();

                cart.Add(new CartDAO { 
                    SanPham = getProductInfo,
                    Quantity = quantity
                });

                Session["cart"] = cart;
                Session["count"] = 1;
            }
            else
            {
                // thiết lập danh sách các sản phẩm đã có trong giỏ hàng
                List<CartDAO> cart = (List<CartDAO>)Session["cart"];

                //kiểm tra sản phẩm có tồn tại trong giỏ hàng chưa???
                int index = isExist(id);

                if (index != -1)
                {
                    //nếu sp tồn tại trong giỏ hàng thì cộng thêm số lượng
                    cart[index].Quantity += quantity;
                }
                else
                {
                    //nếu không tồn tại thì thêm sản phẩm vào giỏ hàng
                    cart.Add(new CartDAO { 
                        SanPham = getProductInfo, 
                        Quantity = quantity
                    });

                    //Tính lại số sản phẩm trong giỏ hàng
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                Session["cart"] = cart;
            }
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        // thêm vào danh sách yêu thích
        public ActionResult AddToWishlist(int id)
        {
            // tìm sản phẩm theo id
            var getProductInfo = db.SanPhams.Find(id);

            // nếu tồn tại sản phẩm thì thêm vào danh sách yêu thích, ngược lại thì báo lỗi
            if (getProductInfo != null)
            {
                //db.SPYeuThichs.Add(new {
                //  MaSP = getProductInfo.MaSP,
                //  MaKH = Session["IdCustomer"]  
                //});
                //db.SaveChanges();
            }
            else
            {
                return Json(new { Message = "Thất bại", JsonRequestBehavior.AllowGet });
            }

            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        private int isExist(int id)
        {
            // thiết lập danh sách các sản phẩm đã có trong giỏ hàng
            List<CartDAO> cart = (List<CartDAO>)Session["cart"];

            // lặp kiểm tra xem có sản phẩm trùng khi thê giỏ hàng hay k?
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].SanPham.MaSP.Equals(id))
                    return i;
            return -1;
        }

        //xóa sản phẩm khỏi giỏ hàng theo id
        public ActionResult Remove(int Id)
        {
            // thiết lập danh sách các sản phẩm đã có trong giỏ hàng
            List<CartDAO> li = (List<CartDAO>)Session["cart"];

            // xóa sp theo mã sản phẩm truyền vào
            li.RemoveAll(x => x.SanPham.MaSP == Id);

            // thiết lập lại session cart sau khi xóa
            Session["cart"] = li;

            // cập nhập số sp trong giỏ hàng
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }
    }
}