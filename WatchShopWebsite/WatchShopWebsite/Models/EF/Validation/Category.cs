using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaDanhMuc))]
    public partial class DanhMuc
    {
    }

    public class MetaDanhMuc
    {
        [Required(ErrorMessage = "Không được để trống tên danh mục")]
        public string TenDM { get; set; }
    }
}