namespace FlowerShopManagement.Application.Models;

public class SupplyFormModel
{
    public string From { get; }
    public List<string> To { get; set; }
    public string Subject { get; }
    public string? HtmlPart { get; set; }
    public string? TextPart { get; set; }
    public List<ProductModel> Products { get; set; }
    public DateTime CreatedDate { get; }

    public SupplyFormModel()
    {
        From = "phatlam1811@gmail.com";
        To = new List<string>();
        Subject = "Supply request from Dallas";
        Products = new List<ProductModel>();
        CreatedDate = DateTime.Now;
    }
}
