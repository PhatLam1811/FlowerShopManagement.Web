using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Models;

public class UserDetailsModel : UserModel
{
    private string _id;

    public Role Role { get; set; } = Role.Customer;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public Gender Gender { get; set; } = Gender.Female;
    public int BirthYear { get; set; }
    public string[] Addresses { get; set; }

    public UserDetailsModel(User entity) : base(entity)
    {
        _id = entity._id;
        Role = entity.role;
        CreatedDate = entity.createdDate;
        Gender = entity.gender;
        BirthYear = entity.birthYear;
        Addresses = entity.addresses;
    }

    public UserDetailsModel() : base()
    {
        _id = new Guid().ToString();
        Addresses = new string[2];
    }

    public new void ToEntity(ref User entity)
    {
        base.ToEntity(ref entity);
        
        entity._id = _id;
        entity.role = Role;
        entity.createdDate = CreatedDate;
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
    public string PhoneNumber { get; set; }

    public UserModel()
    {
        Name = "";
        Avatar = "";
        Email = "";
        PhoneNumber = "";
    }

    public UserModel(User entity)
    {
        Name = entity.name;
        Avatar = entity.avatar;
        Email = entity.email;
        PhoneNumber = entity.phoneNumber;
    }

    public void ToEntity(ref User entity)
    {
        entity.name = Name;
        entity.avatar = Avatar;
        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
    }
}
