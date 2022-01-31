using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TeduShop.Commom;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.ApiNganLuong;
using TeduShop.Web.App_Start;
using TeduShop.Web.Infrastructure.Exstensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        IOrderService _orderService;
        ApplicationUserManager _usermanager; //luu thong tin user

        private string merchantId = ConfigHelper.GetByKey("MerchantId");
        private string merchanPassword = ConfigHelper.GetByKey("MerchantPassword");
        private string merchantEmail = ConfigHelper.GetByKey("MerchantEmail");
        private string currentLink = ConfigHelper.GetByKey("CurrentLink");
        public ShoppingCartController(IProductService productService,ApplicationUserManager usermanager,IOrderService orderService)
        {
            _productService = productService;
            _usermanager = usermanager;
            _orderService = orderService;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            //khoi tao no truoc
            if(Session[CommonConstants.SessionCart] == null)
            {
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            }
            return View();
        }

        public ActionResult CheckOut()
        {
            if(Session[CommonConstants.SessionCart] == null)
            {
                return Redirect("/giohang.html"); //redirect den gio hang
            }

            return View();
        }

        public JsonResult GetUser()
        {
            if(Request.IsAuthenticated)//da dang nhap roi
            {
                var userId = User.Identity.GetUserId();
                var user = _usermanager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }

            return Json(new
            {
                status = false
            });
        }
        //tao moi hoa don va orderDetails
        [HttpGet]
        public JsonResult GetAll()
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if(cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            return Json(new
            {
                status = true,
                data = cart
            },JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateOrder(string orderViewModel)//tra ve actionResult hay json deu duoc
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);//chuyen dang string thanh dang OrderViewModel
            var orderNew = new Order();
            orderNew.UpdateOrder(order);

            //laays thong tin dang nhap gan cho order
            if(Request.IsAuthenticated)
            { 
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>) Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            bool isEnough = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = item.Product.PromotionPrice.HasValue?item.Product.PromotionPrice.Value:item.Product.Price;//lấy giá khuyến mãi và giá chánh
                orderDetails.Add(detail);


                //bán sản phẩm với số lượng
               isEnough =  _productService.SellProduct(item.ProductId, item.Quantity);
                break;
            }

            if(isEnough)
            {
                var orderReturl =  _orderService.Create(orderNew, orderDetails);//tra ve dung cai order do

                _productService.Save();//save thanh cong NEU NO LA THANH TOAN TIEN MAT

                if(order.PaymentMethod == "CASH")
                {
                    return Json(new
                    {
                        status = true,
                    });
                }
                //BUID CAI CHUOI VA CHUYEN SANG NGAN LUONG
                else
                {
                    var merchantId = ConfigHelper.GetByKey("MerchantId");
                    var merchanPassword = ConfigHelper.GetByKey("MerchantPassword");
                    var merchantEmail = ConfigHelper.GetByKey("MerchantEmail");
                    var currentLink = ConfigHelper.GetByKey("CurrentLink");
                    
                    //tao request info
                    RequestInfo info = new RequestInfo();
                    info.Merchant_id = merchantId;//thay doi
                    info.Merchant_password = merchanPassword;//thay doi
                    info.Receiver_email = merchantEmail;//thay doi



                    info.cur_code = "vnd";
                    info.bank_code = order.BankCode;//ma so ngan hang 

                    info.Order_code = orderReturl.ID.ToString();//lấy ID của hóa đơn
                    info.Total_amount = orderDetails.Sum(x => x.Quantity * x.Price).ToString();
                    info.fee_shipping = "0";//phi ship
                    info.Discount_amount = "0";
                    info.order_description = "Thanh toán đơn hàng tại thanh quốc Shop";
                    info.return_url = currentLink+"Xac-nhan-don-hang.html";//link cua website
                    info.cancel_url = currentLink+"huy-don-hang.html";//link cua website

                    //truyen cac thong tin tu order
                    info.Buyer_fullname = order.CustomerName;
                    info.Buyer_email = order.CustomerEmail;
                    info.Buyer_mobile = order.CustomerMobile;

                    APICheckoutV3 objNLChecout = new APICheckoutV3();
                    ResponseInfo result = objNLChecout.GetUrlCheckout(info, order.PaymentMethod);
                    if(result.Error_code == "00")
                    {
                        //checkOut thanh cong chuyen sang trang ngan luong !
                        return Json(new
                        {
                            status = true,
                            urlCheckout = result.Checkout_url
                        });
                    }
                    //đảm bảo lỗi kết nối này không được xảy ra
                    else
                    {
                        return Json(new
                        {
                            status = false,
                            message = result.Description//loi ket noi ngan luong
                        });
                    }
                }
            }
            else
            {
                return Json(new
                {
                    status = false,
                    messgage = "Không đủ hàng"
                });
            }
            
        }


        [HttpPost] //chi cho post
        public JsonResult Add(int productId)
        {
            //khoi tao session cart truoc khi lam dieu gi
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if(cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }
            //
            if(cart.Any(x=>x.ProductId == productId))
            {
                foreach(var item in cart)
                {
                    if(item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                //nếu có giá khuyến mãi thì bán theo giá khuyến mãi, còn không thì bán theo giá gốc
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                var product = _productService.GetById(productId);
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);//chuyen sang productviewmodel
                newItem.Quantity = 1;

                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart; //gán session cart = cart
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        //update so luong
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            foreach(var item in cartSession)
            {
                foreach(var jtem in cartViewModel)
                {
                    if(item.ProductId == jtem.ProductId)
                    {
                        item.Quantity = jtem.Quantity;//update số lượng
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            //gán session = null
            Session[CommonConstants.SessionCart] = null;
            return Json(new
            {
                status = true
            });
        }

        //xóa 1 Item trong giỏ hàng
        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if(cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }

            return Json(new
            {
                status = false
            });
        }

        //Thanh toan thanh cong
        public ActionResult ConfirmOrder()
        {
            string token = Request["token"];
            RequestCheckOrder info = new RequestCheckOrder();
            info.Merchant_id = merchantId;
            info.Merchant_password = merchanPassword;
            info.Token = token;

            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);
            if(result.errorCode== "00")
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất";
            }
            else
            {
                ViewBag.Isscuccess = false;
                ViewBag.Result = "Có lỗi xảy ra";//lỗi thanh toán trên ngân lượng
            }    
            return View();
        }

        //thanh toan that bai
        public ActionResult CancelOrder()
        {
            return View();
        }
    }
}