using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class User
{
    public string _id { get; private set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }
    public string password { get; set; }
    public Role role { get; set; }
    public string name { get; set; }
    public string avatar { get; set; }
    public Gender gender { get; set; }
    public int birthYear { get; set; }
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
        this.birthYear = DateTime.Now.Year - 18;
        this.addresses = new string[0];

        // Extra
        this.createdDate = DateTime.Now;
        this.lastModified = DateTime.Now;
    }

    public User(User source)
    {
        // ID
        this._id = source._id;

        // Account
        this.email = source.email;
        this.phoneNumber = source.phoneNumber;
        this.password = source.password;
        this.role = source.role;

        // Profile
        this.name = source.name;
        this.avatar = source.avatar;
        this.gender = source.gender;
        this.birthYear = source.birthYear;
        this.addresses = source.addresses;

        // Extra
        this.createdDate = source.createdDate;
        this.lastModified = source.lastModified;
    }

    public User(
        string id,
        string email, string phoneNb, string password, Role role,
        string name, string avatar, int birthYear, string[] addresses, Gender gender,
        DateTime createdDate, bool isDeleted)
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
