﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_shop.ViewModel
{
    public class ShoppingViewModel
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ImagePath { get; set; }
        public Decimal Itemprice { get; set; }
        public string Description { get; set; }
        public string CateGory { get; set;  }
    }
}