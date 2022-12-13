using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Services;

public class ImportService : IImportService
{
    private readonly IMailService _mailService;

    public ImportService(IMailService mailService)
    {
        _mailService = mailService;
    }

    public void Request(SupplyFormModel supplyForm)
    {
        var mimeMessage = _mailService.CreateMimeMessage(supplyForm);

        _mailService.Send(mimeMessage);
    }

    public SupplyFormModel CreateSupplyForm(List<LowOnStockProductModel> supplyItems, List<SupplierModel> supplier)
    {
        return new SupplyFormModel(supplyItems, supplier);
    }
}
