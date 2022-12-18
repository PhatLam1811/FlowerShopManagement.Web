using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Application.Models;

public class RegisterModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Invalid email!")]
    public string Email { set; get; } = "";

    [Required]
    [RegularExpression(@"^([\\+]?84[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage ="Invalid phone number!")]
    public string PhoneNumber { set; get; } = "";

    [Required]
    [MinLength(6, ErrorMessage = "Password have to be greater than 6 characters")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password format invalid")]
    public string Password { set; get; } = "";

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
    public string ConfirmPassword { set; get; } = "";
}
