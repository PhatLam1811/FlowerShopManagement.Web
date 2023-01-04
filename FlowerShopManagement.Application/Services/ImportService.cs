using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Application.Services;

public class ImportService : IImportService
{
    private readonly IEmailService _mailService;

    public ImportService(IEmailService mailService)
    {
        _mailService = mailService;
    }

    public void Request(SupplyFormModel supplyForm)
    {
        var mimeMessage = _mailService.CreateMimeMessage(supplyForm);

        _mailService.Send(mimeMessage);
    }

    public SupplyFormModel? CreateSupplyForm(List<ProductDetailModel> productList, List<int> amounts, List<SupplierModel> supplier)
    {
        // All parameters must not be null
        if (productList == null || amounts == null || supplier == null) return null;

        // All lists must have at least one element
        if (productList.Count == 0 || amounts.Count == 0 || supplier.Count == 0) return null;

        // 40 <= amount <= 100
        //foreach (var index in amounts)
        //    if (index < 40 || index > 100) return null;

        // Successfully created a request supply form
        return new SupplyFormModel(productList, amounts, supplier);
    }
}
