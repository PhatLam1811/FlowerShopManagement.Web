using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Core.Interfaces;

public interface ICartCRUD
{
    public Task<bool> AddNewCartByCustomerIdAsync(string customerId);
    public Task<Cart> GetCartOfCustomerIdAsync(string customerId);
    public Task<bool> UpdateCartByCustomerIdAsync(string customerId, Cart cart);
}
