using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerShopManagement.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class NewMongoTestController : ControllerBase
{
    private IUserRepository _userRepository;
    private IHttpContextAccessor _httpContextAccessor;
    private string test;

    public NewMongoTestController(
        IUserRepository userRepository, 
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        test = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    //[HttpGet]
    //public async Task<IEnumerable<User>> GetAllUsers()
    //{
    //    return await _userRepository.GetAll();
    //}

    //[HttpGet("{id}/user")]
    //public async Task<User> GetUserById(string id)
    //{
    //    return await _userRepository.GetById(id);
    //}

    //[HttpGet("{email}/user")]
    //public async Task<User> GetUserByEmail(string email)
    //{
    //    return await _userRepository.GetByField("email", email);
    //}

    //[HttpGet("{phoneNumber}/user")]
    //public async Task<User> GetUserByPhoneNumber(string phoneNumber)
    //{
    //    return await _userRepository.GetByField("phoneNumber", phoneNumber);
    //}

    [HttpPost]
    public async Task<bool> CreateNewUserWith(string email, string phoneNumber, string password)
    {
        User newUser = new();

        newUser.email = email;
        newUser.phoneNumber = phoneNumber;
        newUser.password = password;



        return await _userRepository.Add(newUser);
    }

    //[HttpPut("{id}")]
    //public async Task<bool> UpdateUserPassword(string id, string newPassword)
    //{
    //    User user = await _userRepository.GetById(id);

    //    user.password = newPassword;

    //    return await _userRepository.UpdateById(id, user);
    //}

    [HttpPut("{newEmail}")]
    public async Task<bool> UpdateUserEmail(string oldEmail, string newEmail)
    {
        User user = await _userRepository.GetByField("email", oldEmail);

        user.email = newEmail;

        return await _userRepository.UpdateByField("email", oldEmail, user);
    }

    //[HttpDelete("{id}")]
    //public async Task<bool> DeleteUserById(string id)
    //{
    //    return await _userRepository.RemoveById(id);
    //}

    [HttpDelete("{email}")]
    public async Task<bool> DeleteUserByEmail(string email)
    {
        return await _userRepository.RemoveByField("email", email);
    }

    [HttpGet]
    public string? GetTest([FromQuery] IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext?.User?.FindFirst("4af0041e-89de-46d4-9ce1-6f25fb0c267f")?.Value;
    }
}
