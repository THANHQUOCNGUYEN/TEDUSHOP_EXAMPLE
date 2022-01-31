using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class OrderDetailViewModel
    {
        public int ProductID { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }
    }
}