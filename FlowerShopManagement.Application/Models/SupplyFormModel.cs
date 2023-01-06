using System.Text;

namespace FlowerShopManagement.Application.Models;

public class SupplyFormModel
{
    public string From { get; }
    public List<string> To { get; }
    public string Subject { get; }
    public string Template { get; }
    public string? HtmlPart { get; private set; }
    public string? TextPart { get; }
    public DateTime CreatedDate { get; }

    public SupplyFormModel()
    {
        From = "phatlam1811@gmail.com";
        To = new List<string>();
        Subject = "Supply request from Dallas";
        Template = "/template/SupplyFormTemplate.html";
        CreatedDate = DateTime.Now;
    }

    public void Configurate(
        List<SupplierModel> suppliers,
        List<ProductModel> products,
        List<int> reqAmounts, 
        string htmlPath)
    {
        // Set receivers addresses
        foreach (var supplier in suppliers) To.Add(supplier.Email);

        // Set html part
        var builder = new StringBuilder();

        using (var reader = new StreamReader(htmlPath))
        {
            while (!reader.EndOfStream)
            {
                var text = reader.ReadLine();
                
                builder.AppendLine(text);

                if (text != null && text.Contains("req-items-tab"))
                {
                    // Table header
                    builder.AppendLine("        <tr>");
                    builder.AppendLine("          <th>#</th>");
                    builder.AppendLine("          <th>Product Name</th>");
                    builder.AppendLine("          <th>Amount</th>");
                    builder.AppendLine("        <tr>");

                    // Pass data to template
                    for (int i = 0; i < products.Count; i++)
                    {
                        builder.AppendLine("        <tr>");
                        builder.AppendLine("          <td class=\"product-index\">" + (i + 1).ToString() + "</td>");
                        builder.AppendLine("          <td>" + products[i].Name + "</td>");
                        builder.AppendLine("          <td>" + reqAmounts[i] + "</td>");
                        builder.AppendLine("        <tr>");
                    }
                }    
            }
        }

        HtmlPart = builder.ToString();
    }
}
