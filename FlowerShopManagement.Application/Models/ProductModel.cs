using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Models;

public class ProductModel

{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public int UniPrice { get; set; }
    public int Amount { get; set; }
    public Color Color { get; set; }
    public float WholesaleDiscount { get; set; }

    public ProductModel(Product entity)
    {
        Id = entity._id;
        Picture = entity._picture;
        Name = entity._name;
        Amount = entity._amount;
        WholesaleDiscount = entity._wholesaleDiscount;
    }

    public ProductModel(string id, int amount)
    {
        Id = id;
        Picture = "";
        Name = "";
        Amount = amount;
        WholesaleDiscount = 0;
    }

    public ProductModel()
    {
        Id = new Guid().ToString();
        Picture = "";
        Name = "";
    }

    public bool IsEqualProduct(string id)
    {
        if (id == Id) 
            return true;
        return false;
    }

    public Product ToEntity()
    {
        if (Id == null) Id = Guid.NewGuid().ToString();
        return new Product(id: Id, name: Name, picture: Picture, uniPrice: UniPrice, amount: Amount, wholesaleDiscount: WholesaleDiscount);

    }
}


public class ProductDetailModel
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public int UniPrice { get; set; } = 0; 
    public int Amount { get; set; } = 0;
    public float WholesaleDiscount { get; set; } = 0;
    public Color Color { get; set; } = Color.Sample;
    public string Description { get; set; } = string.Empty; 
    public string Material { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Maintainment { get; set; } = string.Empty;


    public List<Categories> Categories { get; set; } = new List<Categories>();
    public ProductDetailModel(Product entity)
    {
        Id = entity._id;
        Picture = entity._picture;
        Name = entity._name;
        Amount = entity._amount;
        WholesaleDiscount = entity._wholesaleDiscount;
        Color = entity._color;
        Description = entity._description;
        Material = entity._material;
        Size = entity._size;
        Maintainment = entity._maintainment;
    }

    public ProductDetailModel()
    {
        Id = new Guid().ToString();
        Picture = "";
        Name = "";
    }

    public ProductDetailModel(string id, int amount)
    {
        Id = id;
        Amount = amount;
    }

    public bool IsEqualProduct(string id)
    {
        if (id == Id)
            return true;
        return false;
    }

    public Product ToEntity()
    {
        if (Id == null) Id = Guid.NewGuid().ToString();
        return new Product(id: Id, name: Name, picture: Picture, uniPrice: UniPrice, amount: Amount,
            wholesaleDiscount: WholesaleDiscount, categories: Categories, color: Color, 
            description: Description, material: Material, size: Size, maintainment: Maintainment);
    }
}