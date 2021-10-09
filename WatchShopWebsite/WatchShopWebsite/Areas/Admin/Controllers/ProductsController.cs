using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();

        // GET: Admin/Products
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // lấy tham số sắp xếp theo tên đồng hồ hoặc giá tiền
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // truy vấn danh sách sp trong db
            var sanPhams = db.SanPhams.Include(s => s.DanhMuc).Include(s => s.NhaSanXuat).Include(s => s.ThuongHieu);

            // nếu có chuỗi tìm kiếm thì tiến hành tìm kiếm theo tên sản phẩm
            // nếu chuỗi rỗng thì trả lại danh sách ban đầu
            if (!String.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(s => s.TenSP.Contains(searchString));
            }

            // các trường hợp sắp xếp thứ tự 
            switch (sortOrder)
            {
                case "name_desc":
                    sanPhams = sanPhams.OrderByDescending(s => s.TenSP);
                    break;
                case "price_desc":
                    sanPhams = sanPhams.OrderByDescending(s => s.Gia);
                    break;
                case "price":
                    sanPhams = sanPhams.OrderBy(s => s.Gia);
                    break;
                default:
                    sanPhams = sanPhams.OrderBy(s => s.TenSP);
                    break;
            }
            int pageSize = 10; // số lượng trên 1 trang
            int pageNumber = (page ?? 1); // nếu có giá trị của page thì lấy số đó, còn nếu null hoặc rỗng thì trả page = 1
            return View(sanPhams.ToPagedList(pageNumber, pageSize));
        }


        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            SanPham product = new SanPham();
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");
            ViewBag.MaTH = new SelectList(db.ThuongHieux, "MaTH", "TenTH");
            return View(product);
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "MaSP,TenSP,MaTH,MaDM,Gia,SoLuong,MaNSX,HinhAnh,MoTa,TrangThai")]*/ SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                if (sanPham.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(sanPham.ImageUpload.FileName);
                    string extention = Path.GetExtension(sanPham.ImageUpload.FileName);
                    fileName += extention;
                    sanPham.HinhAnh = "~/Content/images/items/" + fileName;
                    sanPham.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));

                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
            ViewBag.MaTH = new SelectList(db.ThuongHieux, "MaTH", "TenTH", sanPham.MaTH);
            return View(sanPham);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
            ViewBag.MaTH = new SelectList(db.ThuongHieux, "MaTH", "TenTH", sanPham.MaTH);
            return View(sanPham);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "MaSP,TenSP,MaTH,MaDM,Gia,SoLuong,MaNSX,HinhAnh,MoTa,TrangThai")]*/ SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                if (sanPham.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(sanPham.ImageUpload.FileName);
                    string extention = Path.GetExtension(sanPham.ImageUpload.FileName);
                    fileName += extention;
                    sanPham.HinhAnh = "~/Content/images/items/" + fileName;
                    sanPham.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));

                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } else
                {
                    sanPham.HinhAnh = db.SanPhams.Where(t => t.MaSP == sanPham.MaSP).Select(t => t.HinhAnh).FirstOrDefault();
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
            ViewBag.MaTH = new SelectList(db.ThuongHieux, "MaTH", "TenTH", sanPham.MaTH);
            return View(sanPham);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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

        [HttpGet]
        public async Task<JsonResult> GetAnother(string json, int madm)
        {
            json = json.Replace("[", "").Replace("]", "");
            int[] ids = json == string.Empty ? new int[0] : json.Split(',').Select(t => int.Parse(t)).ToArray();
            var models = await db.SanPhams.Where(t => !ids.Contains(t.MaSP) && t.MaDM == madm).ToArrayAsync();
            return Json(new
            {
                data = models.Select(t => new
                {
                    MaSP = t.MaSP,
                    TenSP = t.TenSP,
                    NSX = t.NhaSanXuat.TenNSX,
                    SoLuong = t.SoLuong,
                    Gia = t.Gia
                }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
