using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Models;

public class UserModel
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string Avatar { get; set; }
    public Gender Gender { get; set; }
    public int BirthYear { get; set; }
    public string[] Addresses { get; set; }

    public UserModel(User entity)
    {
        Email = entity.email;
        PhoneNumber = entity.phoneNumber;
        FullName = entity.name;
        Avatar = entity.avatar;
        Gender = entity.gender;
        BirthYear = entity.birthYear;
        Addresses = entity.addresses;
    }

    public User ToEntity(User entity)
    {
        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
        entity.name = FullName;
        entity.avatar = Avatar;
        entity.gender = Gender;
        entity.birthYear = BirthYear;
        entity.addresses = Addresses;

        return entity;
    }
}

public class CustomerModel : UserModel
{
    public Cart cart { get; set; }

    public CustomerModel(User entity, Cart cart) : base(entity)
    {
        this.cart = cart;
    } 
}
