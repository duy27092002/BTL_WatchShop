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
    [Authorize(Roles = "Admin")]
    public class ProducersController : Controller
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();

        // GET: Admin/Producers
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // lấy tham số sắp xếp theo tên nhà sản xuất
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

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
            var nhaSanXuats = db.NhaSanXuats.Select(t => t);

            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo tên nhà sản xuất
            // nếu chuỗi rỗng thì trả lại danh sách ban đầu
            if (!String.IsNullOrEmpty(searchString))
            {
                nhaSanXuats = nhaSanXuats.Where(s => s.TenNSX.Contains(searchString));
            }

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "name_desc":
                    nhaSanXuats = nhaSanXuats.OrderByDescending(s => s.TenNSX);
                    break;
                default:
                    nhaSanXuats = nhaSanXuats.OrderBy(s => s.TenNSX);
                    break;
            }
            int pageSize = 10; // số lượng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(nhaSanXuats.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Producers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            return View(nhaSanXuat);
        }

        // GET: Admin/Producers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Producers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNSX,TenNSX,DiaChi,Email,SDT,TrangThai")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                db.NhaSanXuats.Add(nhaSanXuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhaSanXuat);
        }

        // GET: Admin/Producers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            return View(nhaSanXuat);
        }

        // POST: Admin/Producers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNSX,TenNSX,DiaChi,Email,SDT,TrangThai")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaSanXuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhaSanXuat);
        }

        // GET: Admin/Producers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            return View(nhaSanXuat);
        }

        // POST: Admin/Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhaSanXuat nhaSanXuat = db.NhaSanXuats.Find(id);
            db.NhaSanXuats.Remove(nhaSanXuat);
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
