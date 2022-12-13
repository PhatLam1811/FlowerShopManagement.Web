using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Models;

public class OfflineCustomerModel
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }

    public OfflineCustomerModel()
    {
        Name = string.Empty;
        PhoneNumber = string.Empty;
    }
}

public class UserDetailsModel : UserModel
{
    private string _id;
    private Role _role;
    private DateTime _createdDate;

    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
    public int BirthYear { get; set; }
    public string[] Addresses { get; set; }

    public UserDetailsModel(User entity) : base(entity)
    {
        _id = entity._id;
        _role = entity.role;
        _createdDate = entity.createdDate;
        PhoneNumber = entity.phoneNumber;
        Gender = entity.gender;
        BirthYear = entity.birthYear;
        Addresses = entity.addresses;
    }

    public new void ToEntity(ref User entity)
    {
        base.ToEntity(ref entity);
        
        entity._id = _id;
        entity.role = _role;
        entity.createdDate = _createdDate;
        entity.phoneNumber = PhoneNumber;
        entity.gender = Gender;
        entity.birthYear = BirthYear;
        entity.addresses = Addresses;
    }

    public User ToNewEntity()
    {
        var entity = new User();

        entity.name = Name;
        entity.avatar = Avatar;
        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
        entity.gender = Gender;
        entity.birthYear = BirthYear;
        entity.addresses = Addresses;

        return entity;
    }
}

public class UserModel
{
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Email { get; set; }

    public UserModel(User entity)
    {
        Name = entity.name;
        Avatar = entity.avatar;
        Email = entity.email;
    }

    public void ToEntity(ref User entity)
    {
        entity.name = Name;
        entity.avatar = Avatar;
        entity.email = Email;
    }
}
