using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TeduShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //phai co nay
            routes.IgnoreRoute("{*botdetect}",
     new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            //Thanh toán thành công
            routes.MapRoute(
            name: "Confirm Order",
            url: "xac-nhan-don-hang.html",
            defaults: new { controller = "ShoppingCart", action = "ConfirmOrder", id = UrlParameter.Optional },
            namespaces: new string[] { "TeduShop.Web.Controllers" }
        );
            //Thanh toán thất bại
            routes.MapRoute(
            name: "Cancel Order",
            url: "huy-don-hang.html",
            defaults: new { controller = "ShoppingCart", action = "CancelOrder", id = UrlParameter.Optional },
            namespaces: new string[] { "TeduShop.Web.Controllers" }
        );
            //Đăng nhập
            routes.MapRoute(
            name: "Login",
            url: "dang-nhap.html",
            defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            namespaces: new string[] { "TeduShop.Web.Controllers" }
        );
            //Search
            routes.MapRoute(
            name: "search.html",
            url: "tim-kiem.html",
            defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
            namespaces: new string[] { "TeduShop.Web.Controllers" }
        );
            //Đăng kí
            routes.MapRoute(
            name: "Register",
            url: "dang-ki.html",
            defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
            namespaces: new string[] { "TeduShop.Web.Controllers" }
        );

            //trang giới thiệu
            routes.MapRoute(
               name: "About",
               url: "gioi-thieu.html",
               defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "TeduShop.Web.Controllers" }
           );

            //Chi tiết ProductCategory
            routes.MapRoute(
               name: "Product Category",
               url: "{alias}.pc-{id}.html",
               defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new string[] { "TeduShop.Web.Controllers" }
           );
            //Chi tiết sản phẩm
            routes.MapRoute(
               name: "Product",
               url: "{alias}.p-{id}.html", //id trùng tên tham số trong action
               defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
               namespaces: new string[] { "TeduShop.Web.Controllers" }
           );

            //TagList
            routes.MapRoute(
               name: "TagList",
               url: "tag/{tagid}.html", //id trùng tên tham số trong action
               defaults: new { controller = "Product", action = "ListByTag", tagid = UrlParameter.Optional },
               namespaces: new string[] { "TeduShop.Web.Controllers" }
           );

            //giỏ hàng
            routes.MapRoute(
               name: "Cart",
               url: "gio-hang.html",
               defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "TeduShop.Web.Controllers" }
           );
            //Liên hệ
            routes.MapRoute(
              name: "Contact",
              url: "lien-he.html",
              defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "TeduShop.Web.Controllers" }
          );

            //thanh toan

            routes.MapRoute(
             name: "CheckOut",
             url: "thanh-toan.html",
             defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
             namespaces: new string[] { "TeduShop.Web.Controllers" }
         );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "TeduShop.Web.Controllers" }
            );
        }
    }
}
