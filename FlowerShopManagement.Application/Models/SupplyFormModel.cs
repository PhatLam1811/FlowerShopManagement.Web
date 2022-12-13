namespace FlowerShopManagement.Application.Models;

public class SupplyFormModel
{
    public string From { get; set; }
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public List<LowOnStockProductModel> Products { get; set; }
    public List<SupplierModel> Suppliers { get; set; }
    public DateTime CreatedDate { get; set; }

    public SupplyFormModel(List<LowOnStockProductModel> products, List<SupplierModel> suppliers)
    {
        From = "phatlam1811@gmail.com";
        To = new List<string>();
        Subject = "Supply request from Dallas";
        Content = string.Empty;
        Products = products;
        Suppliers = suppliers;
        foreach (var supplier in suppliers)
            To.Add(supplier.Email);
        CreatedDate = DateTime.Now;
    }
}
