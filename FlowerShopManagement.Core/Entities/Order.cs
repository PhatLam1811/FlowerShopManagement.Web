using FlowerShopManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Order
{
    public string _id { get; private set; } = string.Empty;

    [Required]
    public string? _accountID { get; set; }
    public string? _customerName { get; set; }
    public string? _phoneNumber { get; set; }
    public DateTime? _date { get; set; }
    public List<Product>? _products { get; set; }
    // Could be added in future 
    // protected Vouchers _voucher { get; set; }
    public long _total { get; set; } = 0;
    public Status _status { get; set; }
    public DeliverryMethods _deliveryMethod { get; set; }
    public long _deliveryCharge { get; set; }
    public string? _notes { get; set; }
    public string? _address { get; set; }

    public Order(string id = "", string? accountID = null, DateTime? date = null, List<Product>? products = null, string? customerName = "", string? phoneNumber = "",
        long total = 0, Status status = Status.sampleStatus, DeliverryMethods deliveryMethod = DeliverryMethods.sampleMethod, long deliveryCharge = 0, string? notes = null, string? address = null)
    {
        if (id != "")
            _id = id;
        _accountID = accountID;
        if (date != null)
            _date = date;
        _products = products;
        _total = total;
        _status = status;
        _deliveryMethod = deliveryMethod;
        _deliveryCharge = deliveryCharge;
        _notes = notes;
        _customerName = customerName;
        _phoneNumber = phoneNumber;
        _address = address;
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
        _customerName = s._customerName;
        _phoneNumber = s._phoneNumber;
        _address = s._address;
    }

    public Order()
    {
    }

}
