using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities
{
    public class Account : BaseEntity
    {
        protected string _username { get; set; }
        protected string _password { get; set; }
        protected string? _fullName { get; set; }
        protected string? _avatar { get; set; }
        protected Genders _gender { get; set; }
        protected AccountTypes _accountType { get; set; }
    }
}
