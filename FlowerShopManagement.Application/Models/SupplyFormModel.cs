namespace FlowerShopManagement.Application.Models;

public class SupplyFormModel
{
    public string From { get; set; }
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; } 
    public List<ProductDetailModel> Products { get; set; }
    public List<int> Amounts { get; set; }
    public List<SupplierModel> Suppliers { get; set; }
    public DateTime CreatedDate { get; set; }

    public SupplyFormModel(List<ProductDetailModel> products, List<int> amounts, List<SupplierModel> suppliers)
    {
        From = "phatlam1811@gmail.com";
        To = new List<string>();
        Subject = "Supply request from Dallas";
        Content = string.Empty;
        Products = products;
        Amounts = amounts;
        Suppliers = suppliers;
        foreach (var supplier in suppliers)
            To.Add(supplier.Email);
        CreatedDate = DateTime.Now;
    }
}
