using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class User
{
    public string _id;

    // Account
    public string email;
    public string phoneNumber;
    public string password;
    public Role role;

    // Profile
    public string name;
    public string avatar;
    public Gender gender;
    public DateTime birthYear;
    public Dictionary<string, string> shippingAddresses = new Dictionary<string, string>();
    public List<string> favProductIds;

    // Extra
    public DateTime createdDate;
    public DateTime lastModified;

    public User()
    {
        _id = Guid.NewGuid().ToString();

        email = string.Empty;
        phoneNumber = string.Empty;
        password = string.Empty;
        role = Role.Customer;

        name = "Unnamed";
        avatar = string.Empty;
        gender = Gender.Male;
        birthYear = new DateTime(2000, 01, 01);
        favProductIds = new List<string>();

        createdDate = DateTime.Now;
        lastModified = DateTime.Now;
    }
}
