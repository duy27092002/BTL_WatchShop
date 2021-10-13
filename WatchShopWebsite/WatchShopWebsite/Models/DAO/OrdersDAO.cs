using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Models.DAO
{
    public class OrdersDAO
    {
        public CTDonHang OrderDetails { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public byte OrderStatus { get; set; }
    }
}