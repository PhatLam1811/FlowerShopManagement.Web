namespace FlowerShopManagement.Application.Templates;

public class SupplyRequestFormModel
{
    public string From { get; set; } = "phatlam1811@gmail.com";
    public string To { get; set; }
    public string? Subject { get; set; }
    public string? TextPart { get; set; }
    public string? HtmlPart { get; set; }
    public string[] ProductNames { get; set; }
    public string[] ProductPictures { get; set; }
    public int[] SupplyAmounts { get; set; }

    public SupplyRequestFormModel(
        string to, string? subject,
        string? textPart, string? htmlPart,
        string[] productNames, string[] productPictures, int[] supplyAmounts)
    {
        To = to;
        Subject = subject;
        TextPart = textPart;
        HtmlPart = htmlPart;
        ProductNames = productNames;
        ProductPictures = productPictures;
        SupplyAmounts = supplyAmounts;
    }
}
