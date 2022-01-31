using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeduShop.Model.Models;

namespace TeduShop.Web.Models
{
    //[Serializable]
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Alias { get; set; }
        public int CategoryID { get; set; }
        public string MoreImages { get; set; }

        public decimal Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public int? Warranty { get; set; }
        public string Description { set; get; }
        public string Content { get; set; }


        public bool? HomeFlag { get; set; }
        public bool? HotFlag { get; set; }
        public int? ViewCount { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDesCription { get; set; }
        public bool Status { get; set; }

        public string Tags { get; set; }
        public int Quantity { get; set; }

        public decimal OriginalPrice { get; set; } //them cot nay de map no ra
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual IEnumerable<PostTagViewModel> PostTags { get; set; }
    }
}