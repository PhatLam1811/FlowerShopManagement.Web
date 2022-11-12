using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Infrustructure.Interfaces
{
    public interface IOrderRepository
    {
        public Task<bool> AddNewOrder(Order newOrder);
        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrderById(string id);
        public Task<bool> UpdateOrder(Order updatedOrder);
        public Task<bool> RemoveOrderById(string id);
    }
}