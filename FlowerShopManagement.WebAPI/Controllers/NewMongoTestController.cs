using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class NewMongoTestController : ControllerBase
{
    private IUserRepository _userRepository;

    public NewMongoTestController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userRepository.GetAll();
    }

    //[HttpGet("{id}/user")]
    //public async Task<User> GetUserById(string id)
    //{
    //    return await _userRepository.GetById(id);
    //}

    [HttpGet("{email}/user")]
    public async Task<User> GetUserByEmail(string email)
    {
        return await _userRepository.GetByField("email", email);
    }

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
}
