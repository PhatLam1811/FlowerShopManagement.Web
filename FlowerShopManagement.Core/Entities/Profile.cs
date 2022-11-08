using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class Profile 
{
    public string fullName { get; set; }
    public string? avatar { get; set; }
    public Genders gender { get; set; }
    public int birthYear { get; set; }
    public string[] addresses { get; set; }

    public Profile()
    {
        fullName = "Authorized User";
        avatar = string.Empty;
        gender = Genders.male;
        birthYear = DateTime.Now.Year - 18;
        addresses = new string[0];
    }

    public Profile(string fullName)
    {
        this.fullName = fullName;
        this.avatar = string.Empty;
        this.gender = Genders.male;
        this.birthYear = DateTime.Now.Year - 18;
        this.addresses = new string[0];
    }
}
