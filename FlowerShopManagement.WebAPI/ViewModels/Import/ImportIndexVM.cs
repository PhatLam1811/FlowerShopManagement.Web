using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.WebAPI.ViewModels.Import;

public class ImportIndexVM
{
    public List<LowOnStockProductModel>? LowOnStockProductModels { get; set; }
    public List<SupplierModel>? suppliers { get; set; }
}
