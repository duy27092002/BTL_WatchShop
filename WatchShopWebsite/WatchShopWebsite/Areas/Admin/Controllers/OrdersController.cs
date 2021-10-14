using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Areas.Admin.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();

        public ViewResult Index(string sortOrder, int? page)
        {
            // lấy tham số sắp xếp theo cái gì (thời gian hoặc tổng giá)
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TimeSortParm = String.IsNullOrEmpty(sortOrder) ? "time_asc" : "";
            ViewBag.TotalPriceSortParm = sortOrder == "price" ? "price_desc" : "price";

            // truy vấn danh sách đơn hàng trong db
            var donhangs = db.DonHangs.Include(d => d.KhachHang).Include(d => d.ThongTinVanChuyen);

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "time_asc":
                    donhangs = donhangs.OrderBy(s => s.ThoiGianMuaHang);
                    break;
                case "price":
                    donhangs = donhangs.OrderBy(s => s.TongGia);
                    break;
                case "price_desc":
                    donhangs = donhangs.OrderByDescending(s => s.TongGia);
                    break;
                default:
                    donhangs = donhangs.OrderByDescending(s => s.ThoiGianMuaHang);
                    break;
            }
            int pageSize = 10; // số lượng khách hàng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(donhangs.ToPagedList(pageNumber, pageSize));
        }

        // thay đổi trạng thái tuần tự: Xử lý -> giao hàng -> giao hàng thành công
        public RedirectToRouteResult ChangeOStatus(int? id)
        {
            DonHang order = db.DonHangs.Find(id);

            if (order.TrangThaiDH == 0) // nếu đang ở trạng thái "chờ xử lý" thì chuyển sang trạng thái "giao hàng"
            {
                order.TrangThaiDH = 1;
            } else if (order.TrangThaiDH == 1) // nếu đang ở trạng thái "giao hàng" thì chuyển sang trạng thái "giao hàng thành công"
            {
                order.TrangThaiDH = 2;
            }
            else // nếu đang ở trạng thái "giao hàng thành công" thì chuyển sang trạng thái "chờ xử lý"
            {
                order.TrangThaiDH = 0;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewData["CTDonHang"] = await db.CTDonHangs.Where(t => t.MaDonHang == id).ToListAsync();
            return View(donHang);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewData["CTDonHang"] = await db.CTDonHangs.Where(t => t.MaDonHang == id).ToListAsync();
            return View(donHang);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DonHang donHang = await db.DonHangs.FindAsync(id);

            var cTDonHangs = await db.CTDonHangs.Where(t => t.MaDonHang == id).ToListAsync();

            //await UpdateCount(cT_HDNhaps, -1);

            db.CTDonHangs.RemoveRange(cTDonHangs);

            db.DonHangs.Remove(donHang);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
