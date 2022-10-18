using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities
{
    public class Profile 
    {
        public string? _id { get; private set; }

        protected string? _accountID { get; set; }
        protected string? _fullName { get; set; }
        protected string? _avatar { get; set; }
        protected Genders _gender { get; set; }
        protected int _age { get; set; }
        protected string[] _addresses { get; set; } = new string[0];
        protected string? _phoneNumber { get; set; }
        protected string? _email { get; set; }
    }
}
