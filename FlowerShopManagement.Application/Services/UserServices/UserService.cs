using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FlowerShopManagement.Application.Services.UserServices;

public class UserService : IPersonalService
{
	private readonly IUserRepository _userRepository;
	private readonly ICartRepository _cartRepository;
	

	public UserService(IUserRepository userRepository, ICartRepository cartRepository)
	{
		_userRepository = userRepository;
		_cartRepository = cartRepository;
		
	}

	public async Task<bool> EditInfoAsync(UserModel userModel)
	{
		

		try
		{
			// Model to entity
			var user = userModel.ToEntity();

			// Set last modified date
			user.lastModified = DateTime.Now;

			// Update database
			return await _userRepository.UpdateById(user._id, user);
		}
		catch
		{
			// Failed to edit user's info
			return false;
		}
	}

	public async Task<bool> ChangePasswordAsync(UserModel userModel, string newPassword)
	{
		var user = new User();
		try
		{
			// Model to entity
			userModel.ToEntity(ref user);

			// Encrypt password using MD5
			var encryptedPass = Validator.MD5Hash(newPassword);

			// Set new password
			user.password = encryptedPass;

			// Set last modified date
			user.lastModified = DateTime.Now;

			// Update database
			return await _userRepository.UpdateById(user._id, user);
		}
		catch
		{
			// Failed to change user's password
			return false;
		}
	}

	public async Task<bool> ChangePasswordAsync(string id, string oldPassword, string newPassword)
	{
		var passRgx = new Regex(@"^[a-zA-Z0-9]+$");

		if (oldPassword == null || newPassword == null || newPassword.Length < 6 || !passRgx.IsMatch(newPassword)) return false;

		try
		{
			var user = await _userRepository.GetById(id);
			if (user == null) return false;

			// Encrypt old password using MD5
			var encryptedOldPass = Validator.MD5Hash(oldPassword);
			if (!encryptedOldPass.Equals(user.password)) return false;

			// Encrypt new password using MD5
			var encryptedNewPass = Validator.MD5Hash(newPassword);
			user.password = encryptedNewPass;

			// Set last modified date
			user.lastModified = DateTime.Now;

			// Update database
			return await _userRepository.UpdateById(user._id, user);
		}
		catch
		{
			// Failed to change user's password
			return false;
		}
	}

	public async Task<bool> ResetPasswordAsync(UserModel user)
	{
		// Should have the email verification over here...

		return await ChangePasswordAsync(user, "1");
	}

	public async Task<bool> RemoveAccountAsync(string userId, string userRole)
	{
		try
		{
			// Remove cart if user is customer
			if (userRole == Role.Customer.ToString())
				await _cartRepository.RemoveByField("customerId", userId);

			// Remove user from database
			var result = await _userRepository.RemoveById(userId);

			// Successfully removed user
			return result;
		}
		catch
		{
			// Failed to remove user
			return false;
		}
	}


}