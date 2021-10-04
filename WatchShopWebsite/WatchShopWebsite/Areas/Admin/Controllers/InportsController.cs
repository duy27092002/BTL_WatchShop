using Newtonsoft.Json;
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
    public class InportsController : Controller
    {
        private DB_WatchShopEntities db = new DB_WatchShopEntities();

        // GET: Admin/Inports
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // lấy tham số sắp xếp theo thời gian nhập hoặc tổng tiền
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
            var hoaDonNhaps = db.HoaDonNhaps.Include(h => h.NhaCungCap).Include(h => h.NhanVien);

            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo tên nhân viên nhập hoặc tên nhà cung cấp
            // nếu chuỗi rỗng thì trả lại danh sách ban đầu
            if (!String.IsNullOrEmpty(searchString))
            {
                hoaDonNhaps = hoaDonNhaps.Where(s => s.NhanVien.HoTen.Contains(searchString) ||
                s.NhaCungCap.TenNCC.Contains(searchString));
            }

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "time_asc":
                    hoaDonNhaps = hoaDonNhaps.OrderBy(s => s.ThoiGianNhap);
                    break;
                case "price_desc":
                    hoaDonNhaps = hoaDonNhaps.OrderByDescending(s => s.TongGia);
                    break;
                case "price":
                    hoaDonNhaps = hoaDonNhaps.OrderBy(s => s.TongGia);
                    break;
                default:
                    hoaDonNhaps = hoaDonNhaps.OrderByDescending(s => s.ThoiGianNhap);
                    break;
            }
            int pageSize = 10; // số lượng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(hoaDonNhaps.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Inports/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonNhap hoaDonNhap = db.HoaDonNhaps.Find(id);
            if (hoaDonNhap == null)
            {
                return HttpNotFound();
            }
            ViewData["CT_HoaDonNhap"] = await db.CTHDNhaps.Where(t => t.MaHDN == id).ToListAsync();
            return View(hoaDonNhap);
        }

        // GET: Admin/Inports/Create
        public ActionResult Create()
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.Where(t => t.TrangThai == 1).Select(t => t), "MaNCC", "TenNCC");
            ViewBag.MaNV = new SelectList(db.NhanViens.Where(t => t.TrangThai == 1).Select(t => t), "MaNV", "HoTen");
            ViewBag.DanhMuc = new SelectList(db.DanhMucs.Where(t => t.TrangThai == 1).Select(t => t), "MaDM", "TenDM");
            return View();
        }

        // POST: Admin/Inports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaHDN,MaNCC,ThoiGianNhap,MaNV,TongGia")] HoaDonNhap hoaDonNhap)
        {
            if (ModelState.IsValid)
            {
                if (hoaDonNhap.MaNCC != null && hoaDonNhap.MaNV != null)
                {
                    hoaDonNhap.TongGia = 0;
                    db.HoaDonNhaps.Add(hoaDonNhap);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", hoaDonNhap.MaNCC);
            ViewBag.MaNV = new SelectList(db.NhanViens, "MaNV", "HoTen", hoaDonNhap.MaNV);
            return View(hoaDonNhap);
        }

        // GET: Admin/Inports/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonNhap hoaDonNhap = db.HoaDonNhaps.Find(id);
            if (hoaDonNhap == null)
            {
                return HttpNotFound();
            }
            ViewData["CT_HoaDonNhap"] = await db.CTHDNhaps.Where(t => t.MaHDN == id).ToListAsync();
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.Where(t => t.TrangThai == 1).Select(t => t), "MaNCC", "TenNCC", hoaDonNhap.MaNCC);
            ViewBag.MaNV = new SelectList(db.NhanViens.Where(t => t.TrangThai == 1).Select(t => t), "MaNV", "HoTen", hoaDonNhap.MaNV);
            ViewBag.DanhMuc = new SelectList(db.DanhMucs.Where(t => t.TrangThai == 1).Select(t => t), "MaDM", "TenDM");
            return View(hoaDonNhap);
        }

        // POST: Admin/Inports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaHDN,MaNCC,ThoiGianNhap,MaNV,TongGia")] HoaDonNhap hoaDonNhap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoaDonNhap).State = EntityState.Modified;
                /*db.SaveChanges();*/
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.Where(t => t.TrangThai == 1).Select(t => t), "MaNCC", "TenNCC", hoaDonNhap.MaNCC);
            ViewBag.MaNV = new SelectList(db.NhanViens.Where(t => t.TrangThai == 1).Select(t => t), "MaNV", "HoTen", hoaDonNhap.MaNV);
            return View(hoaDonNhap);
        }

        // GET: Admin/Inports/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDonNhap hoaDonNhap = db.HoaDonNhaps.Find(id);
            if (hoaDonNhap == null)
            {
                return HttpNotFound();
            }
            ViewData["CT_HoaDonNhap"] = await db.CTHDNhaps.Where(t => t.MaHDN == id).ToListAsync();
            return View(hoaDonNhap);
        }

        // POST: Admin/Inports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HoaDonNhap hoaDonNhap = await db.HoaDonNhaps.FindAsync(id);

            var cT_HDNhaps = await db.CTHDNhaps.Where(t => t.MaHDN == id).ToListAsync();

            await UpdateCount(cT_HDNhaps, -1);

            db.CTHDNhaps.RemoveRange(cT_HDNhaps);

            db.HoaDonNhaps.Remove(hoaDonNhap);
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

        [HttpPost]
        public async Task<JsonResult> CreateConfirmed(HoaDonNhap info, string json)
        {
            string msg = "OK";

            List<CTHDNhap> cT_HDNhaps = JsonConvert.DeserializeObject<List<CTHDNhap>>(json);

            try
            {
                info.TongGia = cT_HDNhaps.Select(t => (decimal)t.SoLuong * t.Gia).Sum();

                db.HoaDonNhaps.Add(info);

                await db.SaveChangesAsync();

                foreach (var cT_HDNhap in cT_HDNhaps)
                {
                    cT_HDNhap.MaHDN = info.MaHDN;
                }

                db.CTHDNhaps.AddRange(cT_HDNhaps);

                await UpdateCount(cT_HDNhaps, 1);

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                msg = "Fail";
            }

            return Json(new { msg = msg });
        }

        [HttpPost]
        public async Task<JsonResult> EditConfirmed(HoaDonNhap info, string json)
        {
            string msg = "OK";

            List<CTHDNhap> cT_HDNhaps = JsonConvert.DeserializeObject<List<CTHDNhap>>(json);

            try
            {
                info.TongGia = cT_HDNhaps.Select(t => (decimal)t.SoLuong * t.Gia).Sum();

                //db.HoaDonNhaps.Add(info);
                db.Entry(info).State = EntityState.Modified;

                await db.SaveChangesAsync();

                foreach (var cT_HDNhap in cT_HDNhaps)
                {
                    cT_HDNhap.MaHDN = info.MaHDN;
                }

                db.CTHDNhaps.AddRange(cT_HDNhaps);

                await UpdateCount(cT_HDNhaps, 1);

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                msg = "Fail";
            }

            return Json(new { msg = msg });
        }

        // cập nhật số lượng sp khi thêm và xóa hóa đơn nhập với type = 1 (tăng số lượng), type = -1 (giảm số lượng)
        private async Task UpdateCount(List<CTHDNhap> cT_HDNhaps, int type)
        {
            var maSP = cT_HDNhaps.Select(t => t.MaSP).ToArray();

            var sanPhams = await db.SanPhams.Where(t => maSP.Contains(t.MaSP)).ToListAsync();

            foreach (var sp in sanPhams)
            {
                var count = cT_HDNhaps.Where(t => t.MaSP == sp.MaSP).FirstOrDefault().SoLuong;

                sp.SoLuong += count * type;
            }

            await db.SaveChangesAsync();
        }
    }
}
