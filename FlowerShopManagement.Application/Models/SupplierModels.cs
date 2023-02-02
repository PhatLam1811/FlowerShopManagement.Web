using FlowerShopManagement.Application.MyRegex;
using FlowerShopManagement.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Application.Models;

public class SupplierModel
{
    [Required]
    public string _id { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z_/s]*$", ErrorMessage = "Invalid name!")]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    [EmailOrPhone(ErrorMessage = "Email or phone number required!")]
    public string Email { get; set; }

    [Required]
    [EmailOrPhone(ErrorMessage = "Email or phone number required!")]
    public string PhoneNumber { get; set; }

    public string Description { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime LastModified { get; set; }

    public SupplierModel()
    {
        _id = Guid.NewGuid().ToString();

        Name = string.Empty;
        Email = string.Empty;
        PhoneNumber = string.Empty;
        Address = string.Empty;
        Description = string.Empty;

        CreatedDate = DateTime.Now;
        LastModified = DateTime.Now;
    }

    public SupplierModel(Supplier supplier)
    {
        _id = supplier._id;

        Name = supplier.name;
        Email = supplier.email;
        PhoneNumber = supplier.phoneNumber;
        Address = supplier.address;
        Description = supplier.description;

        CreatedDate = supplier.createdDate;
        LastModified = supplier.lastModified;
    }

    public Supplier ToEntity()
    {
        return new Supplier()
        {
            _id = _id,

            name = Name,
            address = Address,
            email = Email,
            phoneNumber = PhoneNumber,
            description = Description,

            createdDate = CreatedDate,
            lastModified = LastModified
        };
    }
}
