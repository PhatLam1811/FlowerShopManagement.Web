using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.Models;
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

    public List<SupplyRequestModel> GetSupplyRequests()
    {
        var result = new List<SupplyRequestModel>();
        try
        {
            var requests = _supplyRequestRepository.GetRequests();

            foreach (var request in requests)
            {
                var model = new SupplyRequestModel(request);
                result.Add(model);
            }

            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<SupplyRequestModel?> GetSupplyRequest(string id)
    {
        try
        {
            var request = await _supplyRequestRepository.GetById(id);

            if (request == null) return null;

            return new SupplyRequestModel(request);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public bool SendRequest(SupplyRequestModel form)
    {
        var mimeMessage = _mailService.CreateMimeMessage(form);
        var request = new SupplyRequest(form.reqSupplier, form.Details, form.CreatedBy);

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

    public SupplyRequestModel CreateReqSupplyForm(
        List<ProductModel> products,  
        SupplierModel suppliers, 
        List<int> requestQty,
        string staffId, string staffName, string htmlPath)
    {
        var form = new SupplyRequestModel(
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
                    builder.AppendLine("          <th>No.</th>");
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
