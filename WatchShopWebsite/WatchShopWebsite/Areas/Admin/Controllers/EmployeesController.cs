using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Areas.Admin.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();

        // GET: Admin/Employees
        // danh sách tất cả nhân viên
        [Authorize(Roles ="Admin,Nhân viên")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // lấy tham số sắp xếp theo họ tên nhân viên hoặc ngày sinh
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.BirthdaySortParm = sortOrder == "birthday" ? "birthday_desc" : "birthday";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // truy vấn danh sách khách hàng trong db
            var nhanViens = from s in db.NhanViens
                             select s;

            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo TenKH
            // nếu chuỗi rỗng thì trả lại danh sách ban đầu
            if (!String.IsNullOrEmpty(searchString))
            {
                nhanViens = nhanViens.Where(s => s.HoTen.Contains(searchString));
            }

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "name_desc":
                    nhanViens = nhanViens.OrderByDescending(s => s.HoTen);
                    break;
                case "birthday_desc":
                    nhanViens = nhanViens.OrderByDescending(s => s.NgaySinh);
                    break;
                case "birthday":
                    nhanViens = nhanViens.OrderBy(s => s.NgaySinh);
                    break;
                default:
                    nhanViens = nhanViens.OrderBy(s => s.HoTen);
                    break;
            }
            int pageSize = 10; // số lượng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(nhanViens.ToPagedList(pageNumber, pageSize));
        }


        // GET: Admin/Employees/Details/5
        [Authorize(Roles = "Admin,Nhân viên")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: Admin/Employees/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            NhanVien employees = new NhanVien();
            return View(employees);
        }

        // POST: Admin/Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "MaNV,Avatar,HoTen,TenDangNhap,MatKhau,GioiTinh,NgaySinh,DiaChi,Email,SDT,TrangThai")]*/ NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                if (nhanVien.ImageUpload != null)
                {
                    // tạo khi có ảnh đại diện
                    string fileName = Path.GetFileNameWithoutExtension(nhanVien.ImageUpload.FileName);
                    string extention = Path.GetExtension(nhanVien.ImageUpload.FileName);
                    fileName += extention;
                    nhanVien.Avatar = "~/Content/images/avatars/" + fileName;
                    nhanVien.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/avatars/"), fileName));

                    db.NhanViens.Add(nhanVien);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } else
                {
                    // tạo khi k có ảnh đại diện
                    db.NhanViens.Add(nhanVien);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            return View(nhanVien);
        }

        // GET: Admin/Employees/Edit/5
        [Authorize(Roles = "Admin,Nhân viên")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Nhân viên")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "MaNV,Avatar,HoTen,TenDangNhap,MatKhau,GioiTinh,NgaySinh,DiaChi,Email,SDT,TrangThai")]*/ NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                if (nhanVien.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(nhanVien.ImageUpload.FileName);
                    string extention = Path.GetExtension(nhanVien.ImageUpload.FileName);
                    fileName += extention;
                    nhanVien.Avatar = "~/Content/images/avatars/" + fileName;
                    nhanVien.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/avatars/"), fileName));

                    db.Entry(nhanVien).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } else
                {
                    nhanVien.Avatar = db.NhanViens.Where(t => t.MaNV == nhanVien.MaNV).Select(t => t.Avatar).FirstOrDefault();
                    db.Entry(nhanVien).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(nhanVien);
        }

        // GET: Admin/Employees/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/Employees/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
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

        // chuyển trạng thái của nhân viên sang "đã nghỉ việc"
        public RedirectToRouteResult UnactiveEmployee(int id)
        {
            NhanVien emp = db.NhanViens.Find(id);
            emp.TrangThai = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // chuyển trạng thái của khách hàng sang "đang làm việc"
        public RedirectToRouteResult ActiveEmployee(int id)
        {
            NhanVien emp = db.NhanViens.Find(id);
            emp.TrangThai = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}