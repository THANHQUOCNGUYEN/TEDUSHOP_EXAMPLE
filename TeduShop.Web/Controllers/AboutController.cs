using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeduShop.Data;
using TeduShop.Model.Models;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    public class AboutController : Controller
    {
        public readonly TeduShopDbContext _context;
        public AboutController(TeduShopDbContext context)
        {
            _context = context;
        }
        // GET: About
        public ActionResult Index()
        {
            return View();
        }

    }
}