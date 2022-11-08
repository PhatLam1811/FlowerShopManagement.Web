using FlowerShopManagement.Application.Interfaces.Models;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Core.Interfaces;
using System.Data;

namespace FlowerShopManagement.Application.Models;

public class UserModel : IUserModel
{
    public string Id { get; private set; } = String.Empty;
    public string? Email { get; set; } = String.Empty;
    public string? PhoneNumber { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public Roles Role { get; set; }
    public Profile Profile { get; set; }

    //private IUserDAOServices _userDAOServices;

    //public UserModel(IUserDAOServices userDAOServices)
    //{
    //    _userDAOServices = userDAOServices;
    //}

    public void FromEntity(User userEntity)
    {
        Id = userEntity._id;
        Email = userEntity.email;
        PhoneNumber = userEntity.phoneNumber;
        Password = userEntity.password;
        Role = userEntity.role;
        Profile = userEntity.profile;
    }

    public User ToEntity() => new User(Id, Email, PhoneNumber, Password, Role, Profile);

    public Task<bool> Rename(string newName)
    {
        //Profile.fullName = newName;
        //User user = ToEntity();
        //return _userDAOServices.UpdateById(Id, user);
        throw new NotImplementedException();
    }

    public Task<bool> SetPassword(string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetEmailAddress(string newEmail)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetPhoneNumber(string newPhoneNumber)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAvatar(string newAvatar)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetGender(Genders newGender)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAge(int newAge)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetAddresses(List<string> newAddressList)
    {
        throw new NotImplementedException();
    }
}
