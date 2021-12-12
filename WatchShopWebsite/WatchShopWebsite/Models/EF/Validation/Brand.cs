using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaThuongHieu))]
    public partial class ThuongHieu
    {
    }

    public class MetaThuongHieu
    {
        [Required(ErrorMessage = "Không được để trống tên thương hiệu")]
        public string TenTH { get; set; }
    }
}