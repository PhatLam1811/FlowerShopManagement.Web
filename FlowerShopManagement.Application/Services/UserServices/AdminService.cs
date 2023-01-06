using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Hosting;

namespace FlowerShopManagement.Application.Services.UserServices;

public class AdminService : StaffService, IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IAddressRepository _addressRepository;


    public AdminService(
        IUserRepository userRepository,
        ICartRepository cartRepository,
        IWebHostEnvironment webHostEnvironment,
        IAddressRepository addressRepository)
        : base(userRepository, cartRepository, webHostEnvironment)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _webHostEnvironment = webHostEnvironment;
        _addressRepository = addressRepository;
    }

    public async Task<bool> AddStaffAsync(UserModel newStaffModel, Role role)
    {
        try
        {
            if (newStaffModel.FormFile == null) return false;
            var staff = await newStaffModel.ToNewEntity(
                wwwRootPath: _webHostEnvironment.WebRootPath
                );
           // var staff = newStaffModel.ToNewEntity();

            // Set default password - "1"
            var defaultPassword = Validator.MD5Hash("1");
            staff.password = defaultPassword;

            // Set role
            if (role == Role.Customer) return false;
            staff.role = role;

            return await _userRepository.Add(staff);
        }
        catch
        {
            // Failed to add new staff account
            return false;
        }
    }

    public async Task<bool> EditUserRoleAsync(UserModel userModel, Role role)
    {
        var staff = new User();

        try
        {
            // Model to entity
            userModel.ToEntity(ref staff);

            // Set role
            staff.role = role;

            // Set modified date
            staff.lastModified = DateTime.Now;

            // Update database
            return await _userRepository.UpdateById(staff._id, staff);
        }
        catch
        {
            // Failed to edit user's role
            return false;
        }
    }

    public async Task<bool> EditUserAsync(UserModel userModel)
    {
        try
        {
            // Model to entity
            var user = userModel.ToEntity();
            // Set modified date
            user.lastModified = DateTime.Now;

            // Update database
            return await _userRepository.UpdateById(user._id, user);
        }
        catch
        {
            // Failed to edit user's role
            return false;
        }
    }

    public async Task<List<AddressModel>> GetAddresses()
    {
        var singleton = AddressSingleTon.IsNullOrNot();
        if (singleton == null)
        {
            var result = await _addressRepository.GetAll();
            if (result == null) return new List<AddressModel>();

            singleton = AddressSingleTon.GetInstance(result);
        }

        return singleton.addressModels;
    }
}
