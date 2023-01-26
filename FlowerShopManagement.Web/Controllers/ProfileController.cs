﻿using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Application.Services.UserServices;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using ZstdSharp.Unsafe;

namespace FlowerShopManagement.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
	private readonly IAuthService _authServices;
	private readonly IPersonalService _personalService;
	private readonly IAdminService _adminService;
	private readonly IUserRepository _userRepository;
	private readonly ICustomerfService _customerfService;
	private readonly IStockService _stockService;
	private readonly ILogger<ProfileController> _logger;

	public ProfileController(IAuthService authServices, ILogger<ProfileController> logger
		, IUserRepository userRepository, IPersonalService userService, ICustomerfService customerfService, IStockService stockService, IAdminService adminService)
	{
		_authServices = authServices;
		_logger = logger;
		_userRepository = userRepository;
		_personalService = userService;
		_customerfService = customerfService;
		_stockService = stockService;
		_adminService = adminService;
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
		if (this.HttpContext == null) return NotFound();

		string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
		if (userId == "") return NotFound(userId);

		UserBasicInfoModel userBasicInfoModel = await _customerfService.GetUseBasicInfoById(userId);
		if (userBasicInfoModel == null) return NotFound();
		// Create UserModel


		return PartialView("PersonalInformation", userBasicInfoModel);
	}

	[HttpPost]
	public async Task<IActionResult> PersonalInformation(UserBasicInfoModel model)
	{
		// edit
		try
		{
			if (ModelState.IsValid)
			{
				// Change & save the editted info from front-end to database
				await _customerfService.EditInfoAsync(model);
			}
		}
		catch
		{
			return NotFound(); // Notify failed to edit current user info for some reasons!
		}

		UserBasicInfoModel userBasicInfoModel = await _customerfService.GetUseBasicInfoById(model._id);
		if (userBasicInfoModel == null) return NotFound();
		// Create UserModel


		return PartialView("PersonalInformation", userBasicInfoModel);
		//return PartialView("PersonalInformation", model);
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
				if (currentUser is null) return NotFound();

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
	public async Task<IActionResult> ManageAddress()
	{
		var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		// Unauthenticated user
		if (userId is null) return NotFound();

		var user = await _authServices.GetAuthenticatedUserAsync(userId);
		if (user is null) return NotFound();

		// load list infor
		var infos = user.InforDelivery;
		//////////////////////////////////////

		return PartialView("ManageAddress", infos);
	}

	[HttpGet]
	public async Task<IActionResult> Voucher()
	{
		var vouchers = await _stockService.GetUpdatedVouchers();
		vouchers = vouchers.Where(i => i.State == Core.Enums.VoucherStatus.Using).ToList();
		// load list voucher
		return PartialView("Voucher", vouchers);
	}

	[HttpGet]
	public IActionResult ManageAccount()
	{
		return PartialView("ManageAccount");
	}

	[HttpGet]
	public async Task<IActionResult> CreateInfoDelivery()
	{
        var list = await _adminService.GetAddresses();
        ViewData["Addresses"] = JsonConvert.SerializeObject(list, Formatting.Indented);
        return PartialView(new InforDeliveryModel());
	}

    [Route("FindDistricts")]
    [HttpPost]
    public async Task<List<string>> FindDistricts(string city)
    {
        List<string> districts = await _adminService.FindDistricts(city);
        ViewData["Districts"] = districts;

        return districts;
    }
    [Route("FindWards")]
    [HttpPost]
    public async Task<List<string>> FindWards(string city, string district)
    {
        List<string> wards = await _adminService.FindWards(city,district);
        ViewData["Wards"] = wards;
        return wards;
    }

    [HttpPost]
	public async Task<IActionResult> CreateInfoDelivery(InforDeliveryModel inforDeliveryModel, string city, string district, string ward)
	{
		if(!ModelState.IsValid) return NotFound();
		var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		// Unauthenticated user
		if (userId is null) return NotFound();

		var user = await _authServices.GetAuthenticatedUserAsync(userId);
		if (user is null) return NotFound();

		user.InforDelivery.Add(inforDeliveryModel);
		await _customerfService.EditInfoAsync(user);

		return PartialView("ManageAddress", user.InforDelivery);
	}

	[HttpPost]
	public async Task<IActionResult> RemoveInfoDelivery(string name, string phone, string address)
	{
		if(!ModelState.IsValid) return NotFound();
		var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		// Unauthenticated user
		if (userId is null) return NotFound();

		var user = await _authServices.GetAuthenticatedUserAsync(userId);
		if (user is null) return NotFound();
		if(user.InforDelivery.Count <= 1)
		{
			return NotFound();
		}
        user.InforDelivery = user.InforDelivery.Where(i => !(i.Address.Equals(address) && i.Name.Equals(name) && i.Phone.Equals(phone))).ToList();
		await _customerfService.EditInfoAsync(user);

		return PartialView("ManageAddress", user.InforDelivery);
	}
}
