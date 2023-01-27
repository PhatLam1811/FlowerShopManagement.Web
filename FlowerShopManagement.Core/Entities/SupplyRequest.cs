namespace FlowerShopManagement.Core.Entities;

public class SupplyRequest
{
    public string _id;

    public RequestSupplier suppliers;
    public List<RequestProduct> details;
    public RequestedStaff createdBy;
    public RequestStatus status;

    public DateTime createdDate;

    public SupplyRequest(
        RequestSupplier suppliers, 
        List<RequestProduct> details, 
        RequestedStaff createdBy)
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

public class RequestedStaff
{
    public string _id = string.Empty;
    public string name = string.Empty;
}

public class RequestSupplier
{
    public string _id = string.Empty;
    public string name = string.Empty;
    public string email = string.Empty;
}

public class RequestProduct
{
    public string _id = string.Empty;
    public string name = string.Empty;
    public double price = 0.0d;
    public int requestQty = 0;
    public string note = string.Empty;
}