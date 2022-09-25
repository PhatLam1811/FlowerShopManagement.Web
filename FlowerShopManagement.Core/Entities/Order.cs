using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities
{
    public class Order : BaseEntity
    {
        protected Guid _accountID { get; set; }
        protected DateTime _date { get; set; }
        protected IList<Product> _products { get; set; } = new List<Product>();
        // Could be added in future 
        // protected Vouchers _voucher { get; set; }
        protected long _total { get; set; } = 0;
        protected Status _status { get; set; }
        protected Deliveries _delivery { get; set; }
        protected long _deliveryCharge { get; set; }
        protected string? _notes { get; set; }
    }
}
