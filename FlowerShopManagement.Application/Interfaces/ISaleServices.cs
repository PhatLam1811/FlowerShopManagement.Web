using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Core.Interfaces;

namespace FlowerShopManagement.Application.Interfaces;

public interface ISaleServices
{
    public Task<List<Order>> GetOrderListAsync();
    public List<Customer> GetCustomerList();
    public Task<bool> VerifyOrder(string customerId, Order order);
    public bool CheckExistedCustomer(string id);
    public Task<bool> CancelOrder(string customerId, Order order);
    public Task<bool> CreateAnOrder(string customerEmail,string customerPhoneNumnber, Order order);
}
