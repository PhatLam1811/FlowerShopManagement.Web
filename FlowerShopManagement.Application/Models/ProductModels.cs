using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models;

public class LowOnStockProductModel : ProductModelP
{
    public int inStockAmount { get; set; }

    public LowOnStockProductModel(Product entity) : base(entity)
    {
        inStockAmount = entity._amount;
    }
}

public class ProductModelP
{
    protected string _id { get; set; }
    public string Name { get; set; }
    public List<string> Pictures { get; set; } = new List<string>();

    public int UnitPrice { get; set; }

    public ProductModelP(Product entity)
    {
        _id = entity._id;
        Name = entity._name;
        Pictures = entity._pictures;
        UnitPrice = entity._uniPrice;
    }
}
