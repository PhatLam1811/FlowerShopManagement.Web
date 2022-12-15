using FlowerShopManagement.Application.MyRegex;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Application.Models
{
    public class SignInModel
    {
        [Required]
        [EmailOrPhone(ErrorMessage = "You have to enter an email or phone number valid")]
        public string EmailorPhone { set; get; } = "";

        [Required]
        [MinLength(6, ErrorMessage = "Password have to be greater than 6 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password format invalid")]
        public string Password { set; get; } = "";
    }
}
