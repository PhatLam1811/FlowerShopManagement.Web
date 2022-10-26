using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Core.Interfaces;

namespace FlowerShopManagement.Core.Entities;

public class User : IUser
{
    public string _id { get; private set; }
    public string? _email { get; set; }
    public string? _phoneNumber { get; set; }
    public string _password { get; set; }
    public AccountTypes _type { get; set; }
    public Profile _profile { get; set; }

    public extern Task<bool> Rename(string newName);
}

public class Customer : User { }

public class Staff : User { }
