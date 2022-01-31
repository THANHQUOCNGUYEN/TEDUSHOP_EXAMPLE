using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Commom;
using TeduShop.Data;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface IProductService
    {
        Product Add(Product Product);
        void Update(Product Product);
        Product Delete(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetLastest(int top);
        //danh sach cách sản phẩm liên quan
        IEnumerable<Product> GetReatedProducts(int id, int top);
        IEnumerable<Product> GetHotProduct(int top);
        IEnumerable<Product> GetAll(string keyword);
        IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize,string sort, out int totalRow);
        Product GetById(int id);
        void Save();
        IEnumerable<Tag> GetListTagByProductId(int id);
        void IncreaseView(int id);
        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow);
        Tag GetTag(string tagId);

        bool SellProduct(int productId, int quantity);

        IEnumerable<string> getListProductbyName(string keyword);
        IEnumerable<Product> Search(string keyword, int page, int pageSize,string sort, out int totalRow);

    }
    public class ProductService : IProductService
    {
        IProductRepository _ProductRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
        IUnitOfWork _unitOfWork;
        public ProductService(IProductRepository ProductRepository,IProductTagRepository productTagRepository,ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            this._ProductRepository = ProductRepository;
            this._unitOfWork = unitOfWork;
            this._productTagRepository = productTagRepository;
            this._tagRepository = tagRepository;  
        }
        public Product Add(Product Product)
        {
            var product = _ProductRepository.Add(Product);
            _unitOfWork.Commit();
            if(!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');
                for(var i = 0;i<tags.Length;i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if(_tagRepository.Count(x=>x.ID== tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                    }
                    //Post Tag thi luon luon phai tao
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = Product.ID;
                    productTag.TagID = tagId;

                    _productTagRepository.Add(productTag);
                }
                _unitOfWork.Commit();
            }
            return product;
        }

        public Product Delete(int id)
        {
            return _ProductRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _ProductRepository.GetAll();
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _ProductRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));

            }
            else
            {
                return _ProductRepository.GetAll();

            }
        }

      

        public Product GetById(int id)
        {
            return _ProductRepository.GetSingleById(id);
        }

        //sản phẩm bán chạy
        public IEnumerable<Product> GetHotProduct(int top)
        {
            return _ProductRepository.GetMulti(x => x.Status && x.HotFlag == true).OrderByDescending(x => x.CreateDate).Take(top);
        }
        //sản phẩm mới nhất
        public IEnumerable<Product> GetLastest(int top)
        {
            return _ProductRepository.GetMulti(x => x.Status).OrderByDescending(x => x.CreateDate).Take(top);
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(int categoryId, int page, int pageSize,string sort, out int totalRow)
        {
            var query = _ProductRepository.GetMulti(x => x.Status && x.CategoryID == categoryId);
            switch (sort)
            {
                   
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);//xem nhiều nhất hoặc count nhiều nhất
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);//sắp xếp tăng dần theo giá
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);

        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _ProductRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));
            switch (sort)
            {

                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);//xem nhiều nhất
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);//khuyến mãi
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);//sắp xếp tăng dần theo giá
                    break;
                case "amount":
                    query = query.OrderByDescending(x => x.Price);//sắp xếp giảm dần theo giá
                    break;

                default:
                    query = query.OrderByDescending(x => x.CreateDate);//mới thêm gần nhất
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);

        }

        //lấy danh sách sản phẩm theo từ khóa tìm kiếm
        public IEnumerable<string> getListProductbyName(string keyword)
        {
            var query = new TeduShopDbContext().Products.Where(s => s.Name.Contains(keyword) && s.Status).Select(x => x.Name);
            return query;
        }

        public IEnumerable<Product> GetListProductByTag(string tagId,int page,int pageSize,out int totalRow)
        {
            var model = _ProductRepository.GetListProductBytag(tagId, page, pageSize, out totalRow);

            return model;
        }

        public IEnumerable<Tag> GetListTagByProductId(int id)
        {
            //lay danh sach Tag theo ProductID
            return _productTagRepository.GetMulti(x => x.ProductID == id, new string[] { "Tag" }).Select(y => y.Tag);

        }

        //Lấy sản phẩm liên quan với số lượng
        public IEnumerable<Product> GetReatedProducts(int id, int top) 
        {
            var product = _ProductRepository.GetSingleById(id);
            return _ProductRepository.GetMulti(x => x.Status && x.CategoryID == product.CategoryID).OrderByDescending(x => x.CreateDate).Take(top);
        }

        public Tag GetTag(string tagId)
        {
            return _tagRepository.GetSingleByCondition(x=>x.ID==tagId);
        }

        public void IncreaseView(int id)
        {
            //tang viewcount
            var product = _ProductRepository.GetSingleById(id);
            if(product.ViewCount.HasValue)
            {
                product.ViewCount += 1;
            }
            else
                product.ViewCount = 1;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, out int totalRow)
        {
            throw new NotImplementedException();
        }


        //sell products
        public bool SellProduct(int productId, int quantity)
        {
            var product = _ProductRepository.GetSingleById(productId);
            if(product.Quantity<quantity)
            {
                return false;
            }

            product.Quantity -= quantity;
            return true;
        }

        public void Update(Product Product)
        {
            _ProductRepository.Update(Product);
            if (!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(x => x.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;


                    }
                    //Post Tag thi luon luon phai tao
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = Product.ID;
                    productTag.TagID = tagId;

                    _productTagRepository.Add(productTag);
                }
            }
        }
    }
}
