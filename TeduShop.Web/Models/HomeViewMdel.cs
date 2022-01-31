using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class HomeViewMdel
    {
        public IEnumerable<SlideViewModel> Slides{ get; set; } 
        public IEnumerable<ProductViewModel> lastesProducts { get; set; } // san pham noi bat
        public IEnumerable<ProductViewModel> TopSaleProducts { get; set; } //san pham ban chay
    }
}