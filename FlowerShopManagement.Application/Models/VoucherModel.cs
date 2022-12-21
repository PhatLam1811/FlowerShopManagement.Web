using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using System.Xml.Linq;
using ValueType = FlowerShopManagement.Core.Enums.ValueType;

namespace FlowerShopManagement.Application.Models;

public class VoucherDetailModel
{
    public string? Code { get; set; }
    public VoucherCategories Categories { get; set; }
    public float Discount { get; set; }
    public ValueType ValueType { get; set; } = ValueType.Percent;
    public double ConditionValue { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public DateTime? ExpiredDate { get; set; } = DateTime.Now.AddDays(1);
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public VoucherStatus State { get; set; } = VoucherStatus.ComingSoon;

    public VoucherDetailModel(Voucher entity)
    {
        Code = entity._id;
        Categories = entity._categories;
        Discount = entity._discount;
        ValueType = entity._valueType;
        ConditionValue= entity._conditionValue;
        Amount = entity._amount;
        ExpiredDate = entity._expiredDate;
        State = entity._state;
        CreatedDate = entity._createdDate;  
    }
    public VoucherDetailModel()
    {
        Code = new Guid().ToString();
    }

    public Voucher ToEntity()
    {
        if (Code == null) return new Voucher();
        return new Voucher(id: Code, discount: Discount, valueType: ValueType, conditionValue: ConditionValue, amount: Amount, expiredDate: ExpiredDate,
            voucherStatus: State, createdDate: CreatedDate);

    }
}


