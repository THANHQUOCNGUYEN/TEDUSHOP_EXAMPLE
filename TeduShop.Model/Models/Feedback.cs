using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Model.Models
{
    [Table("Feelbacks")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [StringLength(250)]
        [Required]
        public string Name { get; set; }
        [StringLength(250)]
        [Required]
        public string Email { get; set; }
        [StringLength(500)]
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
