using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Unauthenticated user
        if (userId is null) return NotFound();

        var user = await _authServices.GetAuthenticatedUserAsync(userId);

        // Create UserModel
        //UserDetailsModel user1 = new UserDetailsModel(user);

        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> PersonalInformation()
    {
        var user = await _userRepository.GetByEmailOrPhoneNb("jah@gmail.com");

        // Create UserModel
        UserModel user1 = new UserModel(user);

        return PartialView("PersonalInformation", user1);
    }

    [HttpPost]
    public async Task<IActionResult> PersonalInformation(UserModel model)
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
        // Check old password

        if (ModelState.IsValid)
        {
            if (model.NewPassword != model.ConfirmPassword)
                return NotFound(); // Notify the passwords dont match!

            if (model.OldPassword == model.NewPassword)
                return NotFound(); // Notify new password is the same as old password!

            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Unauthenticated user
                if (userId is null) return NotFound();

                var currentUser = await _authServices.GetAuthenticatedUserAsync(userId);

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
        var infos = new List<InforDeliveryModel>
        {
            new InforDeliveryModel() { Name = "LHQ", Phone = "204829303", Address = "Viet Nam", IsDefault = true },
            new InforDeliveryModel() { Name = "LTP", Phone = "204829303", Address = "Viet Nam", IsDefault = false }
        };
        //////////////////////////////////////

        return PartialView("ManageAddress", infos);
    }

    [HttpGet]
    public IActionResult Voucher()
    {
        var vouchers = new List<VoucherDetailModel>()
        {
            new VoucherDetailModel() { Code = "CHAOBANMOI", Amount = 200, Categories = Core.Enums.VoucherCategories.NewCustomer, ConditionValue = 20, CreatedDate = DateTime.Now, Discount = 2, State = Core.Enums.VoucherStatus.Using, ValueType = Core.Enums.ValueType.RealValue },
        };
        // load list voucher
        return PartialView("Voucher", vouchers);
    }

    [HttpGet]
    public IActionResult ManageAccount()
    {
        return PartialView("ManageAccount");
    }

    [HttpGet]
    public IActionResult CreateInfoDelivery()
    {
        return View();
    }
}
