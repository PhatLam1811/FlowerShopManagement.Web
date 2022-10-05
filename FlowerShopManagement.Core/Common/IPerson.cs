using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Common;

public interface IPerson
{
    public string _username { get; set; }
    public string _password { get; set; }
    public string? _fullName { get; set; }
    public string? _avatar { get; set; }
    public int _age { get; set; }
    public Genders _gender { get; set; }
}
