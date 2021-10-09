using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Areas.Admin.Controllers
{
    [Authorize]
    public class ExportsController : Controller
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();

        // GET: Admin/Exports
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // lấy tham số sắp xếp theo thời gian xuất hoặc tổng tiền
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TimeSortParm = String.IsNullOrEmpty(sortOrder) ? "time_asc" : "";
            ViewBag.TotalPriceSortParm = sortOrder == "price" ? "price_desc" : "price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // truy vấn danh sách trong db
            var hoaDonXuats = db.HoaDonXuats.Include(h => h.KhachHang).Include(h => h.NhanVien);

            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo tên nhân viên xuất
            // nếu chuỗi rỗng thì trả lại danh sách ban đầu
            if (!String.IsNullOrEmpty(searchString))
            {
                hoaDonXuats = hoaDonXuats.Where(s => s.NhanVien.HoTen.Contains(searchString));
            }

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "time_asc":
                    hoaDonXuats = hoaDonXuats.OrderBy(s => s.ThoiGianXuat);
                    break;
                case "price_desc":
                    hoaDonXuats = hoaDonXuats.OrderByDescending(s => s.TongGia);
                    break;
                case "price":
                    hoaDonXuats = hoaDonXuats.OrderBy(s => s.TongGia);
                    break;
                default:
                    hoaDonXuats = hoaDonXuats.OrderByDescending(s => s.ThoiGianXuat);
                    break;
            }
            int pageSize = 10; // số lượng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(hoaDonXuats.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Exports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonXuat hoaDonXuat = db.HoaDonXuats.Find(id);
            if (hoaDonXuat == null)
            {
                return HttpNotFound();
            }
            return View(hoaDonXuat);
        }

        // GET: Admin/Exports/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Avatar");
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Avatar");
            return View();
        }

        // POST: Admin/Exports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHDX,MaKH,ThoiGianXuat,MaNV,TongGia")] HoaDonXuat hoaDonXuat)
        {
            if (ModelState.IsValid)
            {
                db.HoaDonXuats.Add(hoaDonXuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Avatar", hoaDonXuat.MaKH);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Avatar", hoaDonXuat.MaNV);
            return View(hoaDonXuat);
        }

        // GET: Admin/Exports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonXuat hoaDonXuat = db.HoaDonXuats.Find(id);
            if (hoaDonXuat == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Avatar", hoaDonXuat.MaKH);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Avatar", hoaDonXuat.MaNV);
            return View(hoaDonXuat);
        }

        // POST: Admin/Exports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHDX,MaKH,ThoiGianXuat,MaNV,TongGia")] HoaDonXuat hoaDonXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoaDonXuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Avatar", hoaDonXuat.MaKH);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "Avatar", hoaDonXuat.MaNV);
            return View(hoaDonXuat);
        }

        // GET: Admin/Exports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonXuat hoaDonXuat = db.HoaDonXuats.Find(id);
            if (hoaDonXuat == null)
            {
                return HttpNotFound();
            }
            return View(hoaDonXuat);
        }

        // POST: Admin/Exports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoaDonXuat hoaDonXuat = db.HoaDonXuats.Find(id);
            db.HoaDonXuats.Remove(hoaDonXuat);
            db.SaveChanges();
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
