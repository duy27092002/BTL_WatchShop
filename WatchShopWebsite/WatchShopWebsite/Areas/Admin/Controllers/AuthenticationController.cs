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
    public class AuthenticationController : Controller
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();

        // GET: Admin/Authentication
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // lấy tham số sắp xếp theo tên vai trò
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
            var vaiTro = db.VaiTroes.Select(t => t);

            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo tên vai trò
            // nếu chuỗi rỗng thì trả lại danh sách ban đầu
            if (!String.IsNullOrEmpty(searchString))
            {
                vaiTro = vaiTro.Where(s => s.TenVaiTro.Contains(searchString));
            }

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "name_desc":
                    vaiTro = vaiTro.OrderByDescending(s => s.TenVaiTro);
                    break;
                default:
                    vaiTro = vaiTro.OrderBy(s => s.TenVaiTro);
                    break;
            }
            int pageSize = 10; // số lượng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(vaiTro.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Authentication/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaiTro vaiTro = db.VaiTroes.Find(id);
            if (vaiTro == null)
            {
                return HttpNotFound();
            }
            return View(vaiTro);
        }

        // GET: Admin/Authentication/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Authentication/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaVaiTro,TenVaiTro, TrangThai")] VaiTro vaiTro)
        {
            if (ModelState.IsValid)
            {
                db.VaiTroes.Add(vaiTro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vaiTro);
        }

        // GET: Admin/Authentication/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaiTro vaiTro = db.VaiTroes.Find(id);
            if (vaiTro == null)
            {
                return HttpNotFound();
            }
            return View(vaiTro);
        }

        // POST: Admin/Authentication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaVaiTro,TenVaiTro, TrangThai")] VaiTro vaiTro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vaiTro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vaiTro);
        }

        // GET: Admin/Authentication/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaiTro vaiTro = db.VaiTroes.Find(id);
            if (vaiTro == null)
            {
                return HttpNotFound();
            }
            return View(vaiTro);
        }

        // POST: Admin/Authentication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VaiTro vaiTro = db.VaiTroes.Find(id);
            db.VaiTroes.Remove(vaiTro);
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
