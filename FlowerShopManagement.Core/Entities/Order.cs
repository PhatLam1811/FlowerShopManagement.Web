using FlowerShopManagement.Core.Common;
using FlowerShopManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Order 
{
    public string? _id { get; private set; }

    [Required]
    public string _accountID { get; set; }
    public DateTime _date { get; set; }
    public List<Product> _products { get; set; }
    // Could be added in future 
    // protected Vouchers _voucher { get; set; }
    public long _total { get; set; } = 0;
    public Status _status { get; set; }
    public DeliverryMethods _deliveryMethod { get; set; }
    public long _deliveryCharge { get; set; }
    public string? _notes { get; set; }

    public Order(
        string accountID, DateTime date, List<Product> products, 
        long total, Status status, DeliverryMethods deliveryMethod, long deliveryCharge, string? notes)
    {
        _accountID = accountID;
        _date = date;
        _products = products;
        _total = total;
        _status = status;
        _deliveryMethod = deliveryMethod;
        _deliveryCharge = deliveryCharge;
        _notes = notes;
    }
    public Order(Order s)
    {
        _accountID = s._accountID;
        _date = s._date;
        _products = s._products;
        _total = s._total;
        _status = s._status;
        _deliveryMethod = s._deliveryMethod;
        _deliveryCharge = s._deliveryCharge;
        _notes = s._notes;
    }
}
