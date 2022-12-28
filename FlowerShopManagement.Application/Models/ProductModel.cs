using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace FlowerShopManagement.Application.Models;

public class ProductModel

{
    public string? Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
	public string Picture { get; set; } = string.Empty;
    public int UniPrice { get; set; } = 0;
	public int Amount { get; set; } = 0;
    public Color Color { get; set; } = Color.Sample;
    public float WholesaleDiscount { get; set; } = 0;
    public Categories Categories { get; set; } = Categories.Unknown;
    public bool IsLike { get; set; }

    public ProductModel(Product entity)
    {
        Id = entity._id;
        Picture = entity._picture;
        Name = entity._name;
        Amount = entity._amount;
        WholesaleDiscount = entity._wholesaleDiscount;
        UniPrice = entity._uniPrice;
        //Color = entity.colors;
        Categories = entity._categories;
        IsLike = entity._isLike;
    }

    public ProductModel(string id, int amount)
    {
        Id = id;
        Picture = "";
        Name = "";
        Amount = amount;
        WholesaleDiscount = 0;
        Categories= Categories.Unknown;
        IsLike = false;
    }

    public ProductModel()
    {
        Id = new Guid().ToString();
        Picture = "";
        Name = "";
        IsLike = false;
    }

    public bool IsEqualProduct(string id)
    {
        if (id == Id) 
            return true;
        return false;
    }

    public Product ToEntity()
    {
        if (Id == null || Id == "00000000-0000-0000-0000-000000000000") Id = Guid.NewGuid().ToString();
        return new Product(id: Id, name: Name, picture: Picture, 
            uniPrice: UniPrice, amount: Amount, wholesaleDiscount: WholesaleDiscount, categories: Categories, isLike: IsLike);

    }
}


public class ProductDetailModel
{
    public string? Id { get; set; } = string.Empty;
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
    public Categories Categories { get; set; } =  Categories.Unknown;
    public IFormFile FormPicture { get; set; }
    public bool IsLike { get; set; }

    public ProductDetailModel(Product entity)
    {
        Id = entity._id;
        Picture = entity._picture;
        Name = entity._name;
        Amount = entity._amount;
        Categories = entity._categories;
        UniPrice = entity._uniPrice;
        WholesaleDiscount = entity._wholesaleDiscount;
        Color = entity._color;
        Description = entity._description;
        Material = entity._material;
        Size = entity._size;
        Maintainment = entity._maintainment;
        IsLike = entity._isLike;
    }
	public ProductDetailModel(string id)
	{
		Id = id;
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
        if (Id == null || Id == "00000000-0000-0000-0000-000000000000") Id = Guid.NewGuid().ToString();
        return new Product(id: Id, name: Name, picture: Picture, uniPrice: UniPrice, amount: Amount,
            wholesaleDiscount: WholesaleDiscount, categories: Categories, color: Color, 
            description: Description, material: Material, size: Size, maintainment: Maintainment, isLike: IsLike);
    }
}