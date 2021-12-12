using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaSanPham))]
    public partial class SanPham_Test
    {
        //[NotMapped]
        //public HttpPostedFileBase ImageUpload { get; set; }
    }

    public class MetaSanPham
    {
        [Required(ErrorMessage = "Không được để trống tên sản phẩm")]
        public string TenSP { get; set; }

        [Required(ErrorMessage = "Không được để trống giá tiền")]
        public int Gia { get; set; }

        [Required(ErrorMessage = "Không được để trống số lượng")]
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Không được để trống hình ảnh")]
        public string HinhAnh { get; set; }

        [AllowHtml]
        public string ChiTietSP { get; set; }
    }
}