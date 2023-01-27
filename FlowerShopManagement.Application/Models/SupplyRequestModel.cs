using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models;

public class SupplyRequestModel
{
    public string _Id { get; set; }

    public RequestSupplier reqSupplier { get; set; }
    public List<RequestProduct> Details { get; set; }
    public RequestedStaff CreatedBy { get; set; }
    public RequestStatus Status { get; set; }

    public DateTime CreatedDate { get; }
    public string HtmlPart { get; set; }

    public SupplyRequestModel(SupplyRequest entity)
    {
        _Id = entity._id;

        reqSupplier = entity.suppliers;
        Details = entity.details;
        CreatedBy = entity.createdBy;
        Status = entity.status;
        
        CreatedDate = entity.createdDate;
        HtmlPart = string.Empty;
    }

    public SupplyRequestModel(
        SupplierModel supplier,
        List<ProductModel> products,
        List<int> requestQty,
        string staffname,
        string staffId)
    {
        // Init
        _Id = string.Empty;
        Details = new List<RequestProduct>();
        HtmlPart = string.Empty;
        CreatedDate = DateTime.Now;

        // Parse from SupplierModel to RequestSupplier
        reqSupplier = new RequestSupplier()
        {
            _id = supplier._id,
            name = supplier.Name,
            email = supplier.Email
        };

        // Parse from ProductModel to RequestProduct
        for (int i = 0; i < products.Count; i++)
        {
            var reqProduct = new RequestProduct()
            {
                _id = products[i].Id,
                name = products[i].Name,
                price = products[i].UniPrice,
                requestQty = requestQty[i]
            };

            Details.Add(reqProduct);
        }

        var staff = new RequestedStaff()
        {
            _id = staffId,
            name = staffname
        };

        CreatedBy = staff;
    }
}
