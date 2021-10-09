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
    public class Employee_AuthController : Controller
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();

        // GET: Admin/Employee_Auth
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
            var nhanVien_VaiTro = db.NhanVien_VaiTro.Include(n => n.NhanVien).Include(n => n.VaiTro);

            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo tên sản phẩm
            // nếu chuỗi rỗng thì trả lại danh sách ban đầu
            if (!String.IsNullOrEmpty(searchString))
            {
                nhanVien_VaiTro = nhanVien_VaiTro.Where(s => s.VaiTro.TenVaiTro.Contains(searchString));
            }

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "name_desc":
                    nhanVien_VaiTro = nhanVien_VaiTro.OrderByDescending(s => s.VaiTro.TenVaiTro);
                    break;
                default:
                    nhanVien_VaiTro = nhanVien_VaiTro.OrderBy(s => s.VaiTro.TenVaiTro);
                    break;
            }
            int pageSize = 10; // số lượng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(nhanVien_VaiTro.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Employee_Auth/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien_VaiTro nhanVien_VaiTro = db.NhanVien_VaiTro.Find(id);
            if (nhanVien_VaiTro == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien_VaiTro);
        }

        // GET: Admin/Employee_Auth/Create
        public ActionResult Create()
        {
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "MaNV");
            ViewBag.MaVaiTro = new SelectList(db.VaiTroes, "MaVaiTro", "TenVaiTro");
            return View();
        }

        // POST: Admin/Employee_Auth/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaVaiTro,MaNV,TrangThai")] NhanVien_VaiTro nhanVien_VaiTro)
        {
            if (ModelState.IsValid)
            {
                db.NhanVien_VaiTro.Add(nhanVien_VaiTro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "MaNV", nhanVien_VaiTro.MaNV);
            ViewBag.MaVaiTro = new SelectList(db.VaiTroes, "MaVaiTro", "TenVaiTro", nhanVien_VaiTro.MaVaiTro);
            return View(nhanVien_VaiTro);
        }

        // GET: Admin/Employee_Auth/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien_VaiTro nhanVien_VaiTro = db.NhanVien_VaiTro.Find(id);
            if (nhanVien_VaiTro == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "MaNV", nhanVien_VaiTro.MaNV);
            ViewBag.MaVaiTro = new SelectList(db.VaiTroes, "MaVaiTro", "TenVaiTro", nhanVien_VaiTro.MaVaiTro);
            return View(nhanVien_VaiTro);
        }

        // POST: Admin/Employee_Auth/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaVaiTro,MaNV,TrangThai")] NhanVien_VaiTro nhanVien_VaiTro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien_VaiTro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "MaNV", nhanVien_VaiTro.MaNV);
            ViewBag.MaVaiTro = new SelectList(db.VaiTroes, "MaVaiTro", "TenVaiTro", nhanVien_VaiTro.MaVaiTro);
            return View(nhanVien_VaiTro);
        }

        // GET: Admin/Employee_Auth/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien_VaiTro nhanVien_VaiTro = db.NhanVien_VaiTro.Find(id);
            if (nhanVien_VaiTro == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien_VaiTro);
        }

        // POST: Admin/Employee_Auth/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien_VaiTro nhanVien_VaiTro = db.NhanVien_VaiTro.Find(id);
            db.NhanVien_VaiTro.Remove(nhanVien_VaiTro);
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
