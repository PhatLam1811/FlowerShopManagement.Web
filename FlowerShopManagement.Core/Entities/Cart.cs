namespace FlowerShopManagement.Core.Entities
{
    public class Cart : BaseEntity
    {
        protected Guid _accountID { get; set; }
        protected IList<Product> _products = new List<Product>();
        protected long _total = 0;
    }
}
