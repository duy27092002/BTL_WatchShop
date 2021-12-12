using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaDanhGiaSP))]
    public partial class DanhGiaSP
    {
    }

    public class MetaDanhGiaSP
    {
        [Required(ErrorMessage = "Không được để trống đánh giá")]
        public string LoiDG { get; set; }

        [Required(ErrorMessage = "Vui lòng đánh giá điểm cho sản phẩm này!")]
        public byte Diem { get; set; }
    }
}