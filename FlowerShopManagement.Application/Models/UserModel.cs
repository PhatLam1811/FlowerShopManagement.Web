using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Models;

public class UserModel
{
    public string id { get; set; }
    public string? email { get; set; }
    public string phoneNumber { get; set; }
    public string password { get; set; }
    public Roles role { get; set; }
    public Profile profile { get; set; }

    public UserModel(User entity)
    {
        id = entity._id;
        email = entity.email;
        phoneNumber = entity.phoneNumber;
        password = entity.password;
        role = entity.role;
        profile = entity.profile;
    }

    public User ToEntity() => new User(
        this.id, 
        this.email, 
        this.phoneNumber, 
        this.password, 
        this.profile);
}

public class CustomerModel : UserModel
{
    public Cart cart { get; set; }

    public CustomerModel(User entity, Cart cart) : base(entity)
    {
        this.cart = cart;
    } 
}

public class StaffModel : UserModel
{
    public StaffModel(User entity) : base(entity) { }
}

public class AdminModel : StaffModel
{
    public AdminModel(User entity) : base(entity) { }
}
