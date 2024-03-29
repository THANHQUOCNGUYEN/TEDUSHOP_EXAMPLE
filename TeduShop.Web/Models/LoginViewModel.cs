﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bạn cần nhập tài khoản")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập mật khẩu")]
        [DataType(DataType.Password)]//dùng identiy thì phải có cái này nó mới nhận ra

        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}