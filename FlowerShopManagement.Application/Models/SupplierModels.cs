using FlowerShopManagement.Core.Entities;

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

    public override void ToEntity(ref Supplier entity)
    {
        entity._id = _id;
        entity.name = Name;
        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
        entity.address = Address;
        entity.description = Description;
    }

    public Supplier ToNewEntity()
    {
        var entity = new Supplier();

        entity._id = _id;
        entity.name = Name;
        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
        entity.address = Address;
        entity.description = Description;

        return entity;
    }
}

public class SupplierModel
{
    public string _id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public SupplierModel()
    {
        _id = "";
        Name = "";
        Email = "";
        PhoneNumber = "";
    }

    public SupplierModel(Supplier supplier)
    {
        _id = supplier._id;
        Name = supplier.name;
        Email = supplier.email;
        PhoneNumber = supplier.phoneNumber;
    }

    public virtual void ToEntity(ref Supplier entity)
    {
        entity._id = _id;
        entity.name = Name;
        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
    }
}
