using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Models.DAO
{
    public class CartDAO
    {
        public SanPham SanPham { get; set; }

        public int Quantity { get; set; }
    }
}