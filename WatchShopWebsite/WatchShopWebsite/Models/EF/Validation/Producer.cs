using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaNhaSanXuat))]
    public partial class NhaSanXuat
    {
    }

    public class MetaNhaSanXuat
    {
        [Required(ErrorMessage = "Không được để trống tên")]
        public string TenNSX { get; set; }

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
    }
}