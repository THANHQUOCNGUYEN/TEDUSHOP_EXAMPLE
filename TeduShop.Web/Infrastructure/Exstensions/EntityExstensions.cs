using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeduShop.Model.Models;
using TeduShop.Web.Models;

namespace TeduShop.Web.Infrastructure.Exstensions
{
    //tao ra cac phuong thuc mo rong
    public static class EntityExstensions
    {
        public static void UpdatePostCategory(this PostCategory posCategory, PostCategoryViewModel postVm)
        {
            posCategory.ID = postVm.ID;
            posCategory.Name = postVm.Name;
            posCategory.Description = postVm.Description;
            posCategory.Alias = postVm.Alias;
            posCategory.ParentID = postVm.ParentID;
            posCategory.DisplayOrder = postVm.DisplayOrder;
            posCategory.Image = postVm.Image;
            posCategory.HomeFlag = postVm.HomeFlag;

            //them 7 thuoc tinh ke thua
            posCategory.CreateDate = postVm.CreateDate;
            posCategory.CreateBy = postVm.CreateBy;
            posCategory.UpdateDate = postVm.UpdateDate;
            posCategory.UpdateBy = postVm.UpdateBy;
            posCategory.MetaKeyword = postVm.MetaKeyword;
            posCategory.MetaDesCription = postVm.MetaDesCription;
            posCategory.Status = postVm.Status;

        }

        public static void UpdatePostCategory(this Post post, PostViewModel postVm)
        {
            post.ID = postVm.ID;
            post.Name = postVm.Name;
            post.Description = postVm.Description;
            post.Alias = postVm.Alias;
            post.CategoryID = postVm.CategoryID;
            post.Content = postVm.Content;
            post.Image = postVm.Image;
            post.HomeFlag = postVm.HomeFlag;
            post.ViewCount = postVm.ViewCount;

            post.CreateDate = postVm.CreateDate;
            post.CreateBy = postVm.CreateBy;
            post.UpdateDate = post.UpdateDate;
            post.UpdateBy = post.UpdateBy;
            post.MetaKeyword = post.MetaKeyword;
            post.MetaDesCription = post.MetaDesCription;
            post.Status = post.Status;


        }

        public static void UpdateProductCategory(this ProductCategory productCategory, ProductCategoryViewModel productCategoryVm)
        {
            productCategory.ID = productCategoryVm.ID;
            productCategory.Name = productCategoryVm.Name;
            productCategory.Description = productCategoryVm.Description;
            productCategory.Alias = productCategoryVm.Alias;
            productCategory.ParentID = productCategoryVm.ParentID;
            productCategory.DisplayOrder = productCategoryVm.DisplayOrder;
            productCategory.Image = productCategoryVm.Image;
            productCategory.HomeFlag = productCategoryVm.HomeFlag;

            //them 7 thuoc tinh ke thua
            productCategory.CreateDate = productCategoryVm.CreateDate;
            productCategory.CreateBy = productCategoryVm.CreateBy;
            productCategory.UpdateDate = productCategoryVm.UpdateDate;
            productCategory.UpdateBy = productCategoryVm.UpdateBy;
            productCategory.MetaKeyword = productCategoryVm.MetaKeyword;
            productCategory.MetaDesCription = productCategoryVm.MetaDesCription;
            productCategory.Status = productCategoryVm.Status;


        }

        public static void UpdateProduct(this Product product, ProductViewModel productVm)
        {
            product.ID = productVm.ID;
            product.Name = productVm.Name;
            product.Description = productVm.Description;
            product.Alias = productVm.Alias;
            product.CategoryID = productVm.CategoryID;
            product.Content = productVm.Content;
            product.Image = productVm.Image;
            product.MoreImages = productVm.MoreImages;
            product.Price = productVm.Price;
            product.PromotionPrice = product.PromotionPrice;
            product.Warranty = product.Warranty;
            product.HomeFlag = productVm.HomeFlag;

            //them 7 thuoc tinh ke thua
            product.CreateDate = productVm.CreateDate;
            product.CreateBy = productVm.CreateBy;
            product.UpdateDate = productVm.UpdateDate;
            product.UpdateBy = productVm.UpdateBy;
            product.MetaKeyword = productVm.MetaKeyword;
            product.MetaDesCription = productVm.MetaDesCription;
            product.Status = productVm.Status;
            product.Tags = productVm.Tags;

            product.Quantity = productVm.Quantity;
            product.OriginalPrice = productVm.OriginalPrice; //quan ly gia nhaop
        }

        public static void UpdateFeedback(this Feedback feedback, FeedbackViewModel feedbackViewModel)
        {
            feedback.Name = feedbackViewModel.Name;
            feedback.Email = feedbackViewModel.Email;
            feedback.Message = feedbackViewModel.Message;
            feedback.Status = feedbackViewModel.Status;
            feedback.CreatedDate = DateTime.Now;
        }

        public static void UpdateOrder(this Order order,OrderViewModel orderVm)
        {
            order.CustomerName = orderVm.CustomerName;
            order.CustomerEmail = orderVm.CustomerEmail;
            order.CustomerAddress = orderVm.CustomerAddress;
            order.CustomerMobile = orderVm.CustomerMobile;
            order.PaymentMethod = orderVm.PaymentMethod;
            order.CreatedDate = DateTime.Now;
            order.CreatedBy = orderVm.CreatedBy;//lay trong session dang nhap
            order.Status = orderVm.Status;


        }
    }
}