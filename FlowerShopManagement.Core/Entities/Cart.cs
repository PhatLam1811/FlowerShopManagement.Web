using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Cart 
{
    public string? _id { get; private set; } = String.Empty;
    [Required]
    public string _customerId { get; set; } = String.Empty;
    public List<Product>? _items { get; set; }
    public long _total { get; set; }
}
