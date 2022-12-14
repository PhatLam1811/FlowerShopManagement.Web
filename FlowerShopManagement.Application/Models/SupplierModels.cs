using FlowerShopManagement.Core.Entities;
using System.ComponentModel;

namespace FlowerShopManagement.Application.Models;

public class SupplierDetailModel : SupplierModel
{
    private string _id;

    public string Address { get; set; }
    public string Description { get; set; }

    public SupplierDetailModel() : base()
    {
       
    }

    public SupplierDetailModel(Supplier supplier) : base(supplier)
    {
        _id = supplier._id;
        Address = supplier.address;
        Description = supplier.description;
    }
}

public class SupplierModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public SupplierModel()
    {
        Name = "";
        Email = "";
        PhoneNumber = "";
    }

    public SupplierModel(Supplier supplier)
    {
        Name = supplier.name;
        Email = supplier.email;
        PhoneNumber = supplier.phoneNumber;
    }
}
