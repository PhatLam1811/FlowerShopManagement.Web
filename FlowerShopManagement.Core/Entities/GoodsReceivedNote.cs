namespace FlowerShopManagement.Core.Entities;

public class GoodsReceivedNote
{
    public string _id;
    public int _GRNNo;
    public string _PoNo;

    public string supplierName;
    public List<GoodsReceivedDetails> details;
    public string receivedBy;
    public string checkedBy;

    public DateTime date;

    public GoodsReceivedNote()
    {
        _id = Guid.NewGuid().ToString();
        _PoNo = Guid.NewGuid().ToString();

        supplierName = string.Empty;
        details = new List<GoodsReceivedDetails>();
        receivedBy = string.Empty;
        checkedBy = string.Empty;

        date = DateTime.Now;
    }
}

public struct GoodsReceivedDetails
{
    string productName;
    int orderQty;
    int deliveredQty;
    string note;
}
