using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeduShop.Service;
using TeduShop.Commom;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Models;
using AutoMapper;
using TeduShop.Model.Models;
using System.Web.Script.Serialization;
using TeduShop.Data;

namespace TeduShop.Web.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IProductCategoryService _productCategoryService;
        public ProductController(IProductService productService,IProductCategoryService productCategoryService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
        }
        // GET: Product
        public ActionResult Detail(int id)
        {
            var productModel = _productService.GetById(id);
            var viewModel = Mapper.Map<Product, ProductViewModel>(productModel);
            var relatedProduct = _productService.GetReatedProducts(id, 6);//lay so san pham tuong ung trog teamplate
            ViewBag.relatedProducts = Mapper.Map<IEnumerable<Product>,IEnumerable<ProductViewModel>>(relatedProduct);

            //chuyển sang dạng List<string> dang sách các hình ảnh thu nhỏ
            List<string> listImages = new JavaScriptSerializer().Deserialize<List<string>>(viewModel.MoreImages);
            ViewBag.MoreImages = listImages;

            //Hien thi danh sach tags
            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>,IEnumerable<TagViewModel>>(_productService.GetListTagByProductId(id));
            return View(viewModel);
        }

        public ActionResult Category(int id,int page = 1,string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            ViewBag.id = id;
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(id, page, pageSize,sort,out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var category = _productCategoryService.GetById(id);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);

            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")), //trang lon nhat
                Page = page, //trang hien tai
                TotalCount = totalRow, //tong so ban ghi
                TotalPages = totalPage //tong so trang
            };
            return View(paginationSet);
        }

        public ActionResult ListByTag(string tagId,int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByTag(tagId, page, pageSize, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Tag = Mapper.Map<Tag, TagViewModel>(_productService.GetTag(tagId));
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")), //trang lon nhat
                Page = page, //trang hien tai
                TotalCount = totalRow, //tong so ban ghi
                TotalPages = totalPage //tong so trang
            };
            return View(paginationSet);
        }   

        public JsonResult GetListProductByName(string keyword)
        {
            var db = new TeduShopDbContext();
            var model = db.Products.Where(s => s.Status && s.Name.Contains(keyword)).Select(y => y.Name).ToList();
            return Json(new
            {
                data = model
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string keyword, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));//dat trong appsetting
            int totalRow = 0;
            var lstProducts = _productService.Search(keyword, page, pageSize, sort, out totalRow);
            //int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewBag.Keyword = keyword;
            ViewBag.page = page;

            ViewBag.numberPage = totalRow % pageSize == 0 ? totalRow / pageSize : totalRow / pageSize + 1;
            return View(lstProducts);
        }
    }
}