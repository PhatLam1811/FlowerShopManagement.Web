namespace FlowerShopManagement.Core.Entities;

public class Category
{
    public string _id { get; set; } = "1";
    public string _name { get; set; } = "Unknown";


    public Category(string id = "1", string name = "Unknown")
    {
        _id = id;
        _name = name;
    }
}

