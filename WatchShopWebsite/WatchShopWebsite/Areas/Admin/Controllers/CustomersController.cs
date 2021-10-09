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
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();

        // GET: Admin/Customers
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // lấy tham số sắp xếp theo cái gì (tên hay ngày sinh)
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
            var khachHangs = from s in db.KhachHangs
                             select s;

            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo TenKH
            // nếu chuỗi rỗng thì trả lại danh sách ban đầu
            if (!String.IsNullOrEmpty(searchString))
            {
                khachHangs = khachHangs.Where(s => s.HoTen.Contains(searchString));
            }

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "name_desc":
                    khachHangs = khachHangs.OrderByDescending(s => s.HoTen);
                    break;
                case "birthday":
                    khachHangs = khachHangs.OrderBy(s => s.NgaySinh);
                    break;
                case "birthday_desc":
                    khachHangs = khachHangs.OrderByDescending(s => s.NgaySinh);
                    break;
                default:
                    khachHangs = khachHangs.OrderBy(s => s.HoTen);
                    break;
            }
            int pageSize = 10; // số lượng khách hàng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(khachHangs.ToPagedList(pageNumber, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // chuyển trạng thái của khách hàng sang ẩn nếu có vấn đề
        public RedirectToRouteResult UnactiveCustomer(int id)
        {
            KhachHang customer = db.KhachHangs.Find(id);
            customer.TrangThai = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // chuyển trạng thái của khách hàng sang hiển thị nếu xảy ra nhầm lẫn
        public RedirectToRouteResult ActiveCustomer(int id)
        {
            KhachHang customer = db.KhachHangs.Find(id);
            customer.TrangThai = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
