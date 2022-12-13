using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models;

public class SupplierDetailModel : SupplierModel
{
    public string Address { get; set; }
    public string Products { get; set; }
    public string Note { get; set; }

    public SupplierDetailModel() : base()
    {
       
    }

    public SupplierDetailModel(Supplier supplier) : base(supplier)
    {
        Address = supplier._address;
        Products = supplier._products;
        Note = supplier._note;
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
        Name = supplier._name;
        Email = supplier._email;
        PhoneNumber = supplier._phoneNumber;
    }
}
