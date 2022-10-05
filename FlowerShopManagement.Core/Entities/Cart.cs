using FlowerShopManagement.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Cart : BaseEntity
{
    [Required]
    public string _customerId { get; set; }
    public List<Product> _items { get; set; }
    public long _total { get; set; }

    public Cart(string customerId, List<Product> items, long total)
    {
        _customerId = customerId;
        _items = items;
        _total = total;
    }
}
