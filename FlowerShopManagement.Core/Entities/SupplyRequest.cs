namespace FlowerShopManagement.Core.Entities;

public class SupplyRequest
{
    public string _id;

    public List<RequestSupplier> suppliers;
    public List<RequestProduct> details;
    public RequestedStaff createdBy;
    public RequestStatus status;

    public DateTime createdDate;

    public SupplyRequest(List<RequestSupplier> suppliers, List<RequestProduct> details, RequestedStaff createdBy)
    {
        _id = Guid.NewGuid().ToString();

        this.suppliers = suppliers;
        this.details = details;
        this.createdBy = createdBy;
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

public struct RequestedStaff
{
    public string _id;
    public string name;
}

public struct RequestSupplier
{
    public string _id;
    public string name;
    public string email;
}

public struct RequestProduct
{
    public string _id;
    public string name;
    public int requestQty;
    public string note;
}