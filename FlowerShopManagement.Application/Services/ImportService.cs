using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;
using System.Text;

namespace FlowerShopManagement.Application.Services;

public class ImportService : IImportService
{
    private readonly IImportRepository _importRepository;
    private readonly IEmailService _mailService;

    public ImportService(IImportRepository supplyRequestRepository, IEmailService mailService)
    {
        _importRepository = supplyRequestRepository;
        _mailService = mailService;
    }

    public List<ImportModel> GetRequests()
    {
        var result = new List<ImportModel>();
        try
        {
            var requests = _importRepository.GetRequests();

            foreach (var request in requests)
            {
                var model = new ImportModel(request);
                result.Add(model);
            }

            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<ImportModel?> GetRequest(string id)
    {
        try
        {
            var request = await _importRepository.GetById(id);

            if (request == null) return null;

            return new ImportModel(request);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<string?> Verify(string id, List<int> deliveredQties, List<string> notes)
    {
        try
        {
            var importDetail = await _importRepository.GetById(id);

            if (importDetail == null) return "Import not found!";

            for (int i = 0; i < deliveredQties.Count; i++)
            {
                if (importDetail.details[i].orderQty != deliveredQties[i])
                {
                    return "Delivered quantity unmatched!!";
                }

                importDetail.details[i].deliveredQty = deliveredQties[i];
                importDetail.details[i].note = notes[i];
            }

            importDetail.status = Core.Enums.ImportStatus.Completed;

            await _importRepository.UpdateById(id, importDetail);

            return null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public bool SendRequest(ImportModel form)
    {
        var mimeMessage = _mailService.CreateMimeMessage(form);
        var request = new Import(form.Supplier, form.Details, form.CreatedBy);

        try
        {
            // Save request to database before really sending it
            _importRepository.Add(request);

            // Send request
            return _mailService.Send(mimeMessage);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public ImportModel CreateRequestForm(
        List<ProductModel> products,  
        SupplierModel suppliers, 
        List<int> requestQty,
        string staffId, string staffName, string htmlPath)
    {
        var form = new ImportModel(
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
                        builder.AppendLine("          <td>" + form.Details[i].orderQty + "</td>");
                        builder.AppendLine("        <tr>");
                    }
                }
            }
        }

        form.HtmlPart = builder.ToString();

        return form;
    }
}
