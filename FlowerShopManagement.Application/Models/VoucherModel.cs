using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;
using ValueType = FlowerShopManagement.Core.Enums.ValueType;

namespace FlowerShopManagement.Application.Models;

public class VoucherDetailModel
{
    [Required]
    public string? Code { get; set; }

    [Required]
    public VoucherCategories Categories { get; set; }

    [Required]
    public float Discount { get; set; }

    [Required]
    public ValueType ValueType { get; set; } = ValueType.Percent;

    [Required]
    public double ConditionValue { get; set; } = 0;

    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid Amount!")]
    public int Amount { get; set; } = 0;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime? ExpiredDate { get; set; } = DateTime.Now.AddDays(1);

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    [Required]
    public VoucherStatus State { get; set; } = VoucherStatus.ComingSoon;

    public VoucherDetailModel(Voucher entity)
    {
        Code = entity._id;
        Categories = entity._categories;
        Discount = entity._discount;
        ValueType = entity._valueType;
        ConditionValue = entity._conditionValue;
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


