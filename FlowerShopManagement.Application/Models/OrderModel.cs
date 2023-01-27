using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Models;

public class OrderModel
{
    public string Id = string.Empty;
    public string? AccountID { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public DeliverryMethods DeliveryMethod { get; set; }
    public Status Status { get; set; }
    public List<ProductModel> Products { get; set; } = new List<ProductModel>();
    public string? Notes { get; set; }
    public DateTime? Date { get; set; } = default(DateTime?);
    public int Amount { get; set; } = 0;
    public double Total { get; set; } = 0;
    public double DeliveryCharge { get; set; }
    public string? Address { get; set; }
    // Could be added in future 
    // protected Vouchers _voucher { get; set; }
    public OrderModel()
    {
        Id = Guid.NewGuid().ToString();
        PhoneNumber = "";
        FullName = "Unknown";
        DeliveryMethod = DeliverryMethods.Normal;
        Status = Status.Waiting;
        AccountID = "";
        DeliveryCharge = 0;
        //Products = entity._products;
        Notes = "";
        Address = "";
    }

    public OrderModel(Order entity)
    {
        Id = entity._id;
        PhoneNumber = entity._phoneNumber;
        FullName = entity._customerName;
        DeliveryMethod = entity._deliveryMethod;
        Status = entity._status;
        AccountID = entity._accountID;
        DeliveryCharge = entity._deliveryCharge;
        Address = entity._address;
        //Products = entity._products;
        if (entity._products != null && entity._products.Count > 0)
        {
            foreach (var pro in entity._products)
            {
                Products.Add(new ProductModel(pro));
                Amount += pro._amount;
            }

        }
        Notes = entity._notes;
        Total = entity._total;
        Date = entity._date;
    }

    public Order ToEntity()
    {
        if (Id == null) Id = Guid.NewGuid().ToString();
        List<Product> tempProducts = new List<Product>();
        if (Products != null && Products.Count > 0)
        {
            foreach (var pro in Products)
            {
                tempProducts.Add(pro.ToEntity());
            }

        }
        return new Order(id: Id, products: tempProducts, phoneNumber: PhoneNumber, notes: Notes, deliveryMethod: DeliveryMethod,
            status: Status, customerName: FullName, total: Total, deliveryCharge: DeliveryCharge, accountID: AccountID, date: Date, address: Address);

    }
}

public class OrderDetailModel
{
    public string Id = string.Empty;
    public string? AccountID { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public DeliverryMethods DeliveryMethod { get; set; }
    public Status Status { get; set; }
    public List<ProductDetailModel> Products { get; set; } = new List<ProductDetailModel>();
    public string? Notes { get; set; }
    public DateTime? Date { get; set; } = default(DateTime?);
    public int Amount { get; set; } = 0;
    public double Total { get; set; } = 0;
    public double DeliveryCharge { get; set; }
    public string? Address { get; set; }
    public string? Voucher { get; set; }
    public double Discount { get; set; } = 0;

    public OrderDetailModel()
    {
        Id = Guid.NewGuid().ToString();
        PhoneNumber = "";
        FullName = "Unknown";
        DeliveryMethod = DeliverryMethods.Normal;
        Status = Status.Waiting;
        AccountID = "";
        DeliveryCharge = 0;
        Date = DateTime.Now;
        Notes = "";
        Address = "";
        Voucher = "";
    }

    public OrderDetailModel (Order entity)
    {
        Id = entity._id;
        PhoneNumber = entity._phoneNumber;
        FullName = entity._customerName;
        DeliveryMethod = entity._deliveryMethod;
        Status = entity._status;
        AccountID = entity._accountID;
        DeliveryCharge = entity._deliveryCharge;
        Address = entity._address;
        if (entity._products != null && entity._products.Count > 0)
        {
            foreach (var pro in entity._products)
            {
                Products.Add(new ProductDetailModel(pro));
                Amount += pro._amount;
            }

        }
        Notes = entity._notes;
        Total = entity._total;
        Date = entity._date;
        Voucher = entity._voucher;
        Discount = entity._discount;
    }

    public Order ToEntity()
    {
        if (Id == null) Id = Guid.NewGuid().ToString();
        List<Product> tempProducts = new List<Product>();
        if (Products != null && Products.Count > 0)
        {
            foreach (var pro in Products)
            {
                tempProducts.Add(pro.ToEntity());
            }

        }
        return new Order(id: Id, products: tempProducts, phoneNumber: PhoneNumber, notes: Notes, deliveryMethod: DeliveryMethod,
            status: Status, customerName: FullName, total: Total, deliveryCharge: DeliveryCharge, accountID: AccountID, date: Date, address: Address, voucher: Voucher, discount: Discount);

    }
}
