using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    //cài đặt auto mapping để nó tự mapping khóa chính,khóa ngoại
    public class PostCategoryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public int? ParentID { get; set; }
        public int? DisplayOrder { get; set; }
        public string Image { get; set; }
        public bool? HomeFlag { get; set; }
        public virtual IEnumerable<PostViewModel> Posts { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDesCription { get; set; }
        public bool Status { get; set; }
    }
}