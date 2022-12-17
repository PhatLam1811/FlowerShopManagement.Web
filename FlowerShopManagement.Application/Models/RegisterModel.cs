using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Application.Models
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Email invalid")]
        public string Email { set; get; } = "";
        [Required]
        [RegularExpression(@"^([\\+]?84[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage ="Phone invalid")]
        public string PhoneNumber { set; get; } = "";
        [Required]
        [MinLength(6, ErrorMessage = "Password have to be greater than 6 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password format invalid")]
        public string Password { set; get; } = "";
        [Required]
        public string ConfirmPassword { set; get; } = "";
    }
}
