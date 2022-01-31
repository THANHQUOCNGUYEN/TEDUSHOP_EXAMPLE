using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Bạn cần nhập tên.")]
        public string FullName { get; set; }
        [Required(ErrorMessage ="Bạn cần nhập tên đăng nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Bạn cần nhập mật khẩu")]
        [MinLength(6,ErrorMessage ="Mật khẩu có ít nhất 6 kí tự")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Bạn cần nhập Email")]
        [EmailAddress(ErrorMessage ="Địa chỉ email không đúng !")]
        public string Email { get; set; }

        public string Address { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập phone number")]
        public string PhoneNumber { get; set; }

    }
}