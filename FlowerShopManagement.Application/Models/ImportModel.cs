using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Application.Models;

public class ImportModel
{
    public string _Id { get; set; }
    public int GRNNo { get; set; }
    public string PoNo { get; set; }

    [Required] public SupplierBasic Supplier { get; set; }
    [Required] public List<ImportItem> Details { get; set; }
    [Required] public double Total { get; set; }

    [Required] public UserBasic CreatedBy { get; set; }
    public UserBasic CheckedBy { get; set; }

    [Required] public ImportStatus Status { get; set; }
    [Required] public DateTime CreatedDate { get; }
 
    public string Note { get; set; }
    public string HtmlPart { get; set; }

    public ImportModel(Import entity)
    {
        _Id = entity._id;
        GRNNo = entity._GRNNo;
        PoNo = entity._PoNo;

        Supplier = entity.supplier;
        Details = entity.details;
        Total = entity.total;

        CreatedBy = entity.createdBy;
        CheckedBy = entity.checkedBy;

        Status = entity.status;
        CreatedDate = entity.createdDate;
        
        Note = string.Empty;
        HtmlPart = string.Empty;
    }

    public ImportModel(
        SupplierModel supplier,
        List<ProductModel> products,
        List<int> orderQty,
        string staffname,
        string staffId)
    {
        // Init
        _Id = string.Empty;
        GRNNo = 0;
        PoNo = string.Empty;

        Details = new List<ImportItem>();
        HtmlPart = string.Empty;
        CreatedDate = DateTime.Now;

        // Parse from SupplierModel to RequestSupplier
        Supplier = new SupplierBasic()
        {
            _id = supplier._id,
            name = supplier.Name,
            email = supplier.Email
        };

        // Parse from ProductModel to RequestProduct
        for (int i = 0; i < products.Count; i++)
        {
            var reqProduct = new ImportItem()
            {
                _id = products[i].Id,
                name = products[i].Name,
                price = products[i].UniPrice,
                orderQty = orderQty[i]
            };

            Details.Add(reqProduct);
            Total += reqProduct.price;
        }

        CreatedBy = new UserBasic()
        {
            _id = staffId,
            name = staffname
        };
    }
}
