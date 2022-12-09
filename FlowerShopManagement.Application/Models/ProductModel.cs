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


public class NewProductModel

{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public int UniPrice { get; set; }
    public int Amount { get; set; }
    public float WholesaleDiscount { get; set; }
    public List<Categories> Categories { get; set; } = new List<Categories>();
    public NewProductModel(Product entity)
    {
        Id = entity._id;
        Picture = entity._picture;
        Name = entity._name;
        Amount = entity._amount;
        WholesaleDiscount = entity._wholesaleDiscount;
    }

    public NewProductModel(string id, int amount)
    {
        Id = id;
        Picture = "";
        Name = "";
        Amount = amount;
        WholesaleDiscount = 0;
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
        return new Product(id: Id, name: Name, picture: Picture, uniPrice: UniPrice, amount: Amount, wholesaleDiscount: WholesaleDiscount, categories: Categories);

    }
}