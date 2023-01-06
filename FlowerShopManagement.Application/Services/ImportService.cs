using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using System.Text;

namespace FlowerShopManagement.Application.Services;

public class ImportService : IImportService
{
    private readonly IEmailService _mailService;

    public ImportService(IEmailService mailService)
    {
        _mailService = mailService;
    }

    public bool SendRequest(SupplyFormModel form)
    {
        var mimeMessage = _mailService.CreateMimeMessage(form);

        try
        {
            return _mailService.Send(mimeMessage);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public SupplyFormModel CreateReqSupplyForm(
        List<ProductModel> products,  
        List<SupplierModel> suppliers, 
        List<int> reqAmounts,
        string htmlPath)
    {
        var form = new SupplyFormModel();

        // Set receivers addresses
        foreach (var supplier in suppliers) form.To.Add(supplier.Email);

        form.Products = products;

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

        form.HtmlPart = builder.ToString();

        return form;
    }
}
