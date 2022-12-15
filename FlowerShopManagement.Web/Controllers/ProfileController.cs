using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers;

public class ProfileController : Controller
{
    private readonly IAuthService _authServices;
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IAuthService authServices, ILogger<ProfileController> logger, IUserRepository userRepository, IUserService userService)
    {
        _authServices = authServices;
        _logger = logger;
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Profile = true;

        var user = await _userRepository.GetByEmailOrPhoneNb("jah@gmail.com");

        // Create UserModel
        UserDetailsModel user1 = new UserDetailsModel(user);

        return View(user1);
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
