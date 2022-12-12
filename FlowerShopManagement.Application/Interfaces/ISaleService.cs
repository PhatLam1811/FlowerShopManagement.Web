using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Interfaces;

public interface ISaleService
{
    public Task<List<Order>> GetOrderListAsync();
    public List<UserModel> GetCustomerList();
    public Task<bool> VerifyOrder(string customerId, Order order);
    public bool CheckExistedCustomer(string id);
    public Task<bool> CancelOrder(string customerId, Order order);
    public Task<bool> CreateAnOrder(string customerEmail,string customerPhoneNumnber, Order order);
}
