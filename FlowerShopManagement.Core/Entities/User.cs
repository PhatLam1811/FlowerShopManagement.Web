using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Core.Interfaces;

namespace FlowerShopManagement.Core.Entities;

public class User
{
    public string _id { get; private set; } = String.Empty;
    public string? _email { get; set; } = String.Empty;
    public string? _phoneNumber { get; set; } = String.Empty;
    public string _password { get; set; } = String.Empty;
    public AccountTypes _type { get; set; }
    public Profile? _profile { get; set; }
}

public class Customer : User { }

public class Staff : User { }
