namespace FlowerShopManagement.Core.Entities;

public class Supplier
{
    public string _id { get; set; }
    public string name { get; set; }
    public string address { get; set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }
    public string description { get; set; }

    public Supplier()
    {
        _id = Guid.NewGuid().ToString();
        name = string.Empty;
        address = string.Empty;
        email = string.Empty;
        phoneNumber = string.Empty;
        description = string.Empty;
    }
}
