using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaVaiTro))]
    public partial class VaiTro
    {
    }

    public class MetaVaiTro
    {
        [Required(ErrorMessage = "Không được để trống tên vai trò")]
        public string TenVaiTro { get; set; }
    }
}