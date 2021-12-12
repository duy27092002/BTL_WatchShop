using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaNhanVien))]
    public partial class NhanVien_Test
    {
        //[NotMapped]
        //public HttpPostedFileBase ImageUpload { get; set; }
    }

    public class MetaNhanVien
    {
        [Required(ErrorMessage = "Không được để trống họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Không được để trống tên đăng nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Không được để trống mật khẩu")]
        [MinLength(8, ErrorMessage = "Mật khẩu ít nhất phải 8 ký tự")]
        [MaxLength(50, ErrorMessage = "Mật khẩu tối đa là 50 ký tự")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Không được để trống ngày sinh")]
        public Nullable<System.DateTime> NgaySinh { get; set; }

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