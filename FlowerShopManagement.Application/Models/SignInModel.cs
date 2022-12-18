using FlowerShopManagement.Application.MyRegex;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Application.Models;

public class SignInModel
{
    [Required]
    [EmailOrPhone(ErrorMessage = "Email or phone number required!")]
    public string EmailorPhone { set; get; } = "";

    [Required]
    [MinLength(6, ErrorMessage = "Password must be greater than 6 characters")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password format invalid")]
    public string Password { set; get; } = "";
}
