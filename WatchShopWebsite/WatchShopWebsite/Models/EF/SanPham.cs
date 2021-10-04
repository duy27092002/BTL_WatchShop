﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WatchShopWebsite.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.CTDonHangs = new HashSet<CTDonHang>();
            this.CTHDNhaps = new HashSet<CTHDNhap>();
            this.CTHDXuats = new HashSet<CTHDXuat>();
            this.DonHangs = new HashSet<DonHang>();
            this.GioHangs = new HashSet<GioHang>();

            HinhAnh = "~/Content/images/items/add-product.png";
            LuotXem = 0;
        }
    
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Không được để trống tên sản phẩm")]
        public string TenSP { get; set; }
        public int MaTH { get; set; }
        public int MaDM { get; set; }

        [Required(ErrorMessage = "Không được để trống giá tiền")]
        public int Gia { get; set; }

        [Required(ErrorMessage = "Không được để trống số lượng")]
        public int SoLuong { get; set; }
        public int MaNSX { get; set; }

        [Required(ErrorMessage = "Không được để trống hình ảnh")]
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public byte TrangThai { get; set; }
        public byte LoaiSP { get; set; }
        public Nullable<int> LuotXem { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDonHang> CTDonHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHDNhap> CTHDNhaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHDXuat> CTHDXuats { get; set; }
        public virtual DanhMuc DanhMuc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }
        public virtual NhaSanXuat NhaSanXuat { get; set; }
        public virtual ThuongHieu ThuongHieu { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}