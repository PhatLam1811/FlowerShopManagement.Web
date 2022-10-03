namespace FlowerShopManagement.Core.Entities
{
    public class Supplier : BaseEntity
    {
        protected string _name { get; set; }
        protected string _address { get; set; }
        protected string _products { get; set; }    // Type could change depends on how UI displays the suppliers
        protected string _phoneNumber { get; set; }
        protected string _email { get; set; }
        protected string _note { get; set; }
    }
}
