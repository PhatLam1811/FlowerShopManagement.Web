namespace FlowerShopManagement.Core.Entities;

public class GoodsReceivedNote
{
    public string _id;
    public int _GRNNo;
    public string _PoNo;

    public string supplierName;
    public List<SupplyItem> details;
    public string receivedBy;
    public string checkedBy;

    public DateTime date;

    public GoodsReceivedNote()
    {
        _id = Guid.NewGuid().ToString();
        _PoNo = Guid.NewGuid().ToString();

        supplierName = string.Empty;
        details = new List<SupplyItem>();
        receivedBy = string.Empty;
        checkedBy = string.Empty;

        date = DateTime.Now;
    }
}

public struct SupplyItem
{
    string _productId;
    string productName;
    int orderQty;
    int deliveredQty;
    string note;
}
