//------------------------------------------------------------------------------
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
    
    public partial class CTDonHang
    {
        public int MaDonHang { get; set; }
        public int MaSP { get; set; }
        public int SoLuongMua { get; set; }
        public int Gia { get; set; }
        public string HinhAnh { get; set; }
    
        public virtual DonHang DonHang { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}
