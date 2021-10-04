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

    public partial class NhaCungCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaCungCap()
        {
            this.HoaDonNhaps = new HashSet<HoaDonNhap>();
        }
    
        public int MaNCC { get; set; }

        [Required(ErrorMessage = "Không được để trống tên")]
        public string TenNCC { get; set; }

        [Required(ErrorMessage = "Không được để trống địa chỉ")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Không được để trống email")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Sai định dạng email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Không được để trống số điện thoại")]
        [MaxLength(15, ErrorMessage = "Số điện thoại không được quá 15 ký tự")]
        [RegularExpression(@"^0[9|3]\d{8}$", ErrorMessage = "Sai định dạng số điện thoại")]
        public string SDT { get; set; }
        public byte TrangThai { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; }
    }
}