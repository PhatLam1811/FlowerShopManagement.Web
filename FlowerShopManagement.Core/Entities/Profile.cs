using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities
{
    public class Profile 
    {
        public string? _id { get; private set; }
        public string? _accountID { get; set; }
        public string? _fullName { get; set; }
        public string? _avatar { get; set; }
        public Genders _gender { get; set; }
        public int _age { get; set; }
        public string[] _addresses { get; set; } = new string[0];
        public string? _phoneNumber { get; set; }
        public string? _email { get; set; }
    }
}
