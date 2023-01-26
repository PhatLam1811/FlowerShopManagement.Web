using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models;

public class SupplyFormModel
{
    public List<RequestSupplier> Suppliers { get; set; }
    public List<RequestProduct> Details { get; set; }
    public RequestedStaff CreatedBy { get; set; }
    public DateTime CreatedDate { get; }
    public string HtmlPart { get; set; }

    public SupplyFormModel(
        List<SupplierModel> suppliers,
        List<ProductModel> products,
        List<int> requestQty,
        string staffname,
        string staffId)
    {
        Suppliers = new List<RequestSupplier>();
        Details = new List<RequestProduct>();
        HtmlPart = string.Empty;
        CreatedDate = DateTime.Now;

        foreach (var supplier in suppliers)
        {
            RequestSupplier reqSupplier;
            reqSupplier._id = supplier._id;
            reqSupplier.name = supplier.Name;
            reqSupplier.email = supplier.Email;

            Suppliers.Add(reqSupplier);
        }

        for (int i = 0; i < products.Count; i++)
        {
            RequestProduct reqProduct = new RequestProduct();
            reqProduct._id = products[i].Id;
            reqProduct.name = products[i].Name;
            reqProduct.requestQty = requestQty[i];

            Details.Add(reqProduct);
        }

        RequestedStaff staff;
        staff._id = staffId;
        staff.name = staffname;

        CreatedBy = staff;
    }
}
