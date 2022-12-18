using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Application.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IAuthService _authServices;
    private readonly IPersonalService _personalService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IAuthService authServices, ILogger<ProfileController> logger, IUserRepository userRepository, IPersonalService userService)
    {
        _authServices = authServices;
        _logger = logger;
        _userRepository = userRepository;
        _personalService = userService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Profile = true;

        //var user = await _userRepository.GetByEmailOrPhoneNb("jah@gmail.com");
        var user = await _authServices.GetUserAsync(HttpContext);

        // Create UserModel
        //UserDetailsModel user1 = new UserDetailsModel(user);

        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> PersonalInformation()
    {
        var user = await _userRepository.GetByEmailOrPhoneNb("jah@gmail.com");

        // Create UserModel
        UserDetailsModel user1 = new UserDetailsModel(user);

        return PartialView("PersonalInformation", user1);
    }

    [HttpPost]
    public async Task<IActionResult> PersonalInformation(UserDetailsModel model)
    {
        var user = await _userRepository.GetByEmailOrPhoneNb("jah@gmail.com");

        // edit
        if (ModelState.IsValid)
        {
            try
            {
                // Change & save the editted info from front-end to database
                await _personalService.EditInfoAsync(model);

                return PartialView("PersonalInformation", model); // Notify succesfully editted current user info
            }
            catch
            {
                return NotFound(); // Notify failed to edit current user info for some reasons!
            }
        }

        return PartialView("PersonalInformation", model);
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return PartialView("ChangePassword", new ChangePasswordModel());
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        var user = await _userRepository.GetByEmailOrPhoneNb("jah@gmail.com");

        // Check old password

        if (ModelState.IsValid)
        {
            if (model.NewPassword != model.ConfirmPassword)
                return NotFound(); // Notify the passwords dont match!

            if (model.OldPassword == model.NewPassword)
                return NotFound(); // Notify new password is the same as old password!

            try
            {
                var currentUser = await _authServices.GetUserAsync(this.HttpContext);

                // Verify old password
                var encryptedPass = Validator.MD5Hash(model.OldPassword);
                if (!currentUser.IsPasswordMatched(encryptedPass))
                    return NotFound(); // Old password didnt match! 

                await _personalService.ChangePasswordAsync(currentUser, model.NewPassword);

                return PartialView("ChangePassword", new ChangePasswordModel()); // Notify successfully changed password!
            }
            catch
            {
                return NotFound(); // Notify failed to change the password for some reasons!
            }
        }

        return PartialView("ChangePassword", new ChangePasswordModel());
    }

    [HttpGet]
    public IActionResult ManageAddress()
    {
        // load list infor
        return PartialView("ManageAddress", new List<InforDeliveryModel>());
    }

    [HttpGet]
    public IActionResult Voucher()
    {
        // load list voucher
        return PartialView("Voucher");
    }

    [HttpGet]
    public IActionResult ManageAccount()
    {
        return PartialView("ManageAccount");
    }
}
