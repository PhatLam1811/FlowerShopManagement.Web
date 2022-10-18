using FlowerShopManagement.Core.Common;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class Staff : IPerson
{
    public string? _id { get; private set; }
    public string _username { get; set; }
    public string _password { get; set; }
    public string? _fullName { get; set; }
    public string? _avatar { get; set; }
    public int _age { get; set; }
    public Genders _gender { get; set; }
    public AccountTypes _type { get; set; }

    public Staff()
    {
        //_id = Guid.NewGuid().ToString();
        _username = "";
        _password = "";
        _fullName = "";
        _avatar = "";
        _age = 16;
        _gender = Genders.male;
        _type = AccountTypes.staff;      
    }

    public Staff(
        string username, string password,
        string displayName = "", string avatar = "", int age = 16, 
        Genders gender = Genders.male, AccountTypes type = AccountTypes.staff)
    {
        _id = Guid.NewGuid().ToString();
        _username = username;
        _password = password;
        _fullName = displayName != "" ? displayName : username;
        _avatar = avatar;
        _age = age;
        _gender = gender;
        _type = type;
    }

    public Staff(Staff s)
    {
        _id = s._id;
        _username = s._username;
        _password = s._password;
        _fullName = s._fullName;
        _avatar = s._avatar;
        _age = s._age;
        _gender = s._gender;
        _type= s._type;
    }
}
