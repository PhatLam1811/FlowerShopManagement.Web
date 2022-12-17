using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class User
{
    public string _id { get; set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }
    public string password { get; set; }
    public Role role { get; set; }
    public string name { get; set; }
    public string avatar { get; set; }
    public Gender gender { get; set; }
    public DateTime birthYear { get; set; }
    public string[] addresses { get; set; }
    public DateTime createdDate { get; set; }
    public DateTime lastModified { get; set; }

    public User()
    {
        // ID
        this._id = Guid.NewGuid().ToString();

        // Account
        this.email = string.Empty;
        this.phoneNumber = string.Empty;
        this.password = string.Empty;
        this.role = Role.Customer;

        // Profile
        this.name = string.Empty;
        this.avatar = string.Empty;
        this.gender = Gender.Male;
        this.birthYear = DateTime.Now;
        this.addresses = new string[0];

        // Extra
        this.createdDate = DateTime.Now;
        this.lastModified = DateTime.Now;
    }

    public User(User user)
    {
        // ID
        this._id = user._id;

        // Account
        this.email = user.email;
        this.phoneNumber = user.phoneNumber;
        this.password = user.password;
        this.role = user.role;

        // Profile
        this.name = user.name;
        this.avatar = user.avatar;
        this.gender = user.gender;
        this.birthYear = user.birthYear;
        this.addresses = user.addresses;

        // Extra
        this.createdDate = user.createdDate;
        this.lastModified = user.lastModified;
    }

    public User(
        string id,
        string email, string phoneNb, string password, Role role,
        string name, string avatar, DateTime birthYear, string[] addresses, Gender gender,
        DateTime createdDate)
    {
        // ID
        this._id = id;

        // Account
        this.email = email;
        this.phoneNumber = phoneNb;
        this.password = password;
        this.role = role;

        // Profile
        this.name = name;
        this.avatar = avatar;
        this.gender = gender;
        this.birthYear = birthYear;
        this.addresses = addresses;

        // Extra
        this.createdDate = createdDate;
        this.lastModified = DateTime.Now;
    }
}
