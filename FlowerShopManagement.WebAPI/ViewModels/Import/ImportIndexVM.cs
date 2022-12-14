using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.WebAPI.ViewModels.Import;

public class ImportIndexVM
{
    public List<LowOnStockProductModel>? LowOnStockProductModels { get; set; }
    public List<SupplierModel>? Suppliers { get; set; }
    public List<LowOnStockProductModel>? SelectedItems { get; set; } 
    public List<SupplierModel>? SelectedSuppliers { get; set; }
}
