using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class SupplyRequest
{
    public string _id;

    public List<Supplier> suppliers;
    public List<RequestedItem> details;
    public RequestStatus status;

    public DateTime createdDate;

    public SupplyRequest()
    {
        _id = Guid.NewGuid().ToString();

        suppliers = new List<Supplier>();
        details = new List<RequestedItem>();
        status = RequestStatus.Pending;

        createdDate = DateTime.Now;
    }
}

public enum RequestStatus
{
    Pending,
    Canceled,
    Completed
}

public struct RequestedItem
{
    string _productId;
    string productName;
    int requestedQty;
    string note;
}