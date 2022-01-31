using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TeduShop.Model.Models;

namespace TeduShop.Web.Models
{
    public class FeedbackViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Tên phải nhập")]
        [MaxLength(ErrorMessage = "Tên không được quá 250 kí tự")]
        public string Name { get; set; }
        [MaxLength(250, ErrorMessage = "Email không được quá 250 kí tự")]
        public string Email { get; set; }
        [MaxLength(500, ErrorMessage = "Tin nhắn không được quá 500 kí tự")]
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Phải nhập trạng thái")]
        public bool Status { get; set; }

        public ContactDetailViewModel contactDetail{ get; set; }
    }
}