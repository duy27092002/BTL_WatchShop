using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaThongTinVanChuyen))]
    public partial class ThongTinVanChuyen
    {
    }

    public class MetaThongTinVanChuyen
    {
        [Required(ErrorMessage = "Không được để trống tên người nhận")]
        public string TenNguoiNhan { get; set; }

        [Required(ErrorMessage = "Không được để trống địa chỉ")]
        public string CTDiaChi { get; set; }

        [Required(ErrorMessage = "Không được để trống số điện thoại")]
        [MaxLength(15, ErrorMessage = "Số điện thoại không được quá 15 ký tự")]
        [RegularExpression(@"^0[9|3]\d{8}$", ErrorMessage = "Sai định dạng số điện thoại")]
        public string SDT { get; set; }
    }
}