using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Web.Helpers;

namespace FlowerShopManagement.Application.Models;

public class ProductModel

{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    //public string Picture { get; set; } = string.Empty;
    public List<string> Pictures { get; set; } = new List<string>();

    public int UniPrice { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public Color Color { get; set; } = Color.Sample;
    public float WholesaleDiscount { get; set; } = 0;
    public string Category { get; set; } = "Unknown";
    public string Material { get; set; } = "Unknown";
    public bool IsLike { get; set; }
    public ProductModel(Product entity)
    {
        Id = entity._id;
        Pictures = entity._pictures;
        Name = entity._name;
        Amount = entity._amount;
        WholesaleDiscount = entity._wholesaleDiscount;
        UniPrice = entity._uniPrice;
        //Color = entity.colors;
        IsLike = entity._isLike;
        Category = entity._category;
        Material = entity._material;

    }

    public ProductModel(string id, int amount)
    {
        Id = id;
        Pictures = new List<string>();
        Name = "";
        Amount = amount;
        WholesaleDiscount = 0;
        IsLike = false;
    }

    public ProductModel()
    {
        Id = new Guid().ToString();
        Pictures = new List<string>();
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
        return new Product(id: Id, name: Name, picture: Pictures,
            uniPrice: UniPrice, amount: Amount, wholesaleDiscount: WholesaleDiscount, category: Category, isLike: IsLike);

    }
}


public class ProductDetailModel
{
    public string? Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Pictures { get; set; } = new List<string>();
    public int UniPrice { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public float WholesaleDiscount { get; set; } = 0;
    public Color Color { get; set; } = Color.Sample;
    public string Description { get; set; } = string.Empty;
    public string Material { get; set; } = "Unknown";
    public string Size { get; set; } = string.Empty;
    public string Maintainment { get; set; } = string.Empty;
    public string Category { get; set; } = "Unknown";
    public List<IFormFile> FormPicture { get; set; } = new List<IFormFile>();
    public bool IsLike { get; set; } = false;

    public ProductDetailModel(Product entity)
    {
        Id = entity._id;
        Pictures = entity._pictures;
        Name = entity._name;
        Amount = entity._amount;
        Category = entity._category;
        UniPrice = entity._uniPrice;
        WholesaleDiscount = entity._wholesaleDiscount;
        Color = entity._color;
        Description = entity._description;
        Material = entity._material;
        Size = entity._size;
        Maintainment = entity._maintainment;
        Pictures = entity._pictures;
        IsLike = entity._isLike;
    }
    public ProductDetailModel(string id)
    {
        Id = id;
    }

    public ProductDetailModel()
    {
        Id = new Guid().ToString();
        Pictures = new List<string>();
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
        return new Product(id: Id, name: Name, picture: Pictures, uniPrice: UniPrice, amount: Amount,
            wholesaleDiscount: WholesaleDiscount, category: Category, color: Color,
            description: Description, material: Material, size: Size, maintainment: Maintainment, isLike: IsLike);
    }
    public async Task<Product> ToEntityContainingImages(string wwwRootPath)
    {
        if (this.FormPicture != null && this.FormPicture.Count > 0)
        {
            foreach (var image in this.FormPicture)
            {
                if (image.Length > 0)
                {

                    string fileName = image.FileName;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                        this.Pictures.Add(image.FileName);
                    }

                }
            }

        }

        // Add image

        if (Id == null || Id == "00000000-0000-0000-0000-000000000000") Id = Guid.NewGuid().ToString();
        return new Product(id: Id, name: Name, picture: Pictures, uniPrice: UniPrice, amount: Amount,
            wholesaleDiscount: WholesaleDiscount, category: Category, color: Color,
            description: Description, material: Material, size: Size, maintainment: Maintainment, isLike: IsLike);
    }
}
