using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Core.Interfaces;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER CRUD **************
// - New adjustments could be made in future updates

public interface ICustomerCRUD
{
    public Task<bool> AddNewCustomer(Customer newCustomer);
    public Task<List<Customer>> GetAllCustomers();
    public Task<Customer> GetCustomerById(string id);
    public bool UpdateCustomer(Customer updatedCustomer);
    public bool RemoveCustomerById(string id);
}