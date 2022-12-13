using FlowerShopManagement.Core.Enums;
using ValueType = FlowerShopManagement.Core.Enums.ValueType;

namespace FlowerShopManagement.Core.Entities;

public class Voucher
{
    public string? _id { get; set; }
    public VoucherCategories _categories { get; set; }
    public float _discount { get; set; }
    public ValueType _valueType { get; set; } = ValueType.Percent;
    public double _conditionValue { get; set; } = 0;
    public int _amount { get; set; } = 0;
    public DateTime? _expiredDate { get; set; } = DateTime.Now.AddDays(1);
    public DateTime? _createdDate { get; set; } = DateTime.Now;
    public VoucherStatus _state { get; set; } = VoucherStatus.ComingSoon;
    public Voucher(string? id = null, VoucherCategories categories = VoucherCategories.NewCustomer, float discount = 0,
        ValueType valueType = ValueType.RealValue, double conditionValue = 100, int amount = 0, DateTime? expiredDate = null, DateTime? createdDate = null,
        VoucherStatus voucherStatus = VoucherStatus.ComingSoon)
    {
        if (id != null)
            _id = id;
        _categories = categories;
        _discount = discount;
        _state = voucherStatus;
        _conditionValue = conditionValue;
        _amount = amount;
        if (createdDate != null)
            _createdDate = createdDate;
        if (expiredDate != null)
            _expiredDate = expiredDate;
        _categories = categories;
    }
}
