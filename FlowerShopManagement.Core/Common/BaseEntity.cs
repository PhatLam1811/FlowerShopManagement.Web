namespace FlowerShopManagement.Core.Common;

public class BaseEntity
{
    public string _id { get; set; }

    public BaseEntity() => _id = Guid.NewGuid().ToString();
    public BaseEntity(string id) => _id = id;
}
