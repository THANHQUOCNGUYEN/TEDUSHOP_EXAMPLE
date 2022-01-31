using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeduShop.Data;

namespace TeduShop.Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public TeduShopDbContext _shop;
        public AdminController(TeduShopDbContext shop)
        {
            _shop = shop;
        }
        public ActionResult Index(TeduShopDbContext shop)
        {
            var result = _shop.OrderDetails.ToList();
            return View();
        }
    }
}