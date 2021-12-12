using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchShopWebsite.Models.EF
{
    [MetadataType(typeof(MetaBinhLuan))]
    public partial class BinhLuan
    {
    }

    public class MetaBinhLuan
    {
        [Required(ErrorMessage = "Không được để trống bình luận")]
        public string LoiBL { get; set; }
    }
}