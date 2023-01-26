using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using System.Text;

namespace FlowerShopManagement.Application.Services;

public class ImportService : IImportService
{
    private readonly ISupplyRequestRepository _supplyRequestRepository;
    private readonly IEmailService _mailService;

    public ImportService(ISupplyRequestRepository supplyRequestRepository, IEmailService mailService)
    {
        _supplyRequestRepository = supplyRequestRepository;
        _mailService = mailService;
    }

    public bool SendRequest(SupplyFormModel form)
    {
        var mimeMessage = _mailService.CreateMimeMessage(form);
        var request = new SupplyRequest(form.Suppliers, form.Details, form.CreatedBy);

        try
        {
            // Save request to database before really sending it
            _supplyRequestRepository.Add(request);

            // Send request
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
        List<int> requestQty,
        string staffId, string staffName, string htmlPath)
    {
        var form = new SupplyFormModel(
            suppliers, 
            products, requestQty, 
            staffName, staffId);

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
                    for (int i = 0; i < form.Details.Count; i++)
                    {
                        builder.AppendLine("        <tr>");
                        builder.AppendLine("          <td class=\"product-index\">" + (i + 1).ToString() + "</td>");
                        builder.AppendLine("          <td>" + form.Details[i].name + "</td>");
                        builder.AppendLine("          <td>" + form.Details[i].requestQty + "</td>");
                        builder.AppendLine("        <tr>");
                    }
                }
            }
        }

        form.HtmlPart = builder.ToString();

        return form;
    }
}
