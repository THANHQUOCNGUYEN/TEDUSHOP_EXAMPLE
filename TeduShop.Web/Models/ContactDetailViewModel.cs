using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class ContactDetailViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Tên không được trống")]
        
        public string Name { get; set; }
        [MaxLength(50,ErrorMessage ="Số điện thoại không vượt quá 50 kí tự")]
        public string Phone { get; set; }
        [StringLength(250,ErrorMessage ="Email không vượt quá 50 kí tự")]
        public string Email { get; set; }
        [StringLength(250, ErrorMessage = "Website không vượt quá 50 kí tự")]
        public string Address { get; set; }
        public string Orther { get; set; }

        public double? Lat { get; set; }

        public double? Lng { get; set; }
        public bool Status { get; set; }

        [StringLength(250)]
        public string Website { get; set; }
    }
}