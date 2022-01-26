using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_shop.ViewModel
{
    public class OrderModel
    {
        public int OrderId { get; set; }

        public DateTime OrderDate {get; set; }

        public string OrderNumber { get; set; }
    }
}