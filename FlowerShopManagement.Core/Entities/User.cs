using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class User
{
    public string _id { get; private set; }
    public string? email { get; set; }
    public string phoneNumber { get; set; }
    public string password { get; set; }
    public string role { get; set; }
    public Profile profile { get; set; }
    public DateTime createdDate { get; set; }
    public DateTime lastModified { get; set; }
    public bool isDeleted { get; set; }

    public User()
    {
        _id = Guid.NewGuid().ToString();
        email = string.Empty;
        phoneNumber = string.Empty;
        password = string.Empty;
        role = Roles.Customer;
        profile = new();
        createdDate = DateTime.Now;
        lastModified = DateTime.Now;
        isDeleted = false;
    }

    public User(
        string id,
        string? email,
        string phoneNumber,
        string password,
        Profile? profile = null,
        DateTime? createdDate = null,
        bool isDeleted = false,
        string role = Roles.Customer)
    {
        _id = id;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.password = password;
        this.role = role;
        this.profile = profile != null ? profile : new();
        this.createdDate = createdDate != null ? createdDate.Value : DateTime.Now;
        this.lastModified = DateTime.Now;
        this.isDeleted = isDeleted;
    }
}
