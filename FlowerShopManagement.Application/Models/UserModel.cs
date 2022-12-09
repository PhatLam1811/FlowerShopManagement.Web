using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Models;

public class UserModel
{
    //public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string CustomerName { get; set; }

    public UserModel(User entity)
    {
        //Email = entity.email;
        PhoneNumber = entity.phoneNumber;
        CustomerName = entity.name;
       
    }

    public User ToEntity(User entity)
    {
        //entity.email = Email;
        entity.phoneNumber = PhoneNumber;
        entity.name = CustomerName;
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
