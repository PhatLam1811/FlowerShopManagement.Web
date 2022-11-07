using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Core.Interfaces;

public interface ICustomer : IUser
{
    public Task<bool> AddNewCustomer(Customer newCustomer);
    public Task<List<Customer>> GetAllCustomers();
    public Task<Customer> GetCustomerById(string id);
    public bool UpdateCustomer(Customer updatedCustomer);
    public bool RemoveCustomerById(string id);
}