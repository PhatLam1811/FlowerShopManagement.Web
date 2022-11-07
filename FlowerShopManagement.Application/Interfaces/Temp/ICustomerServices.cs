using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;

namespace FlowerShopManagement.Application.Interfaces.Temp;

public interface ICustomerServices
{
    //List<Customer> Get(string id);

    //Customer Create(Customer customer);

    //void Update(string id, Customer customer);

    //void Remove(string id);

    /*public List<Customer> Get();

    public Customer? Get(string id);

    public void Create(Customer customer);

    public void Remove(string id);

    public void Update(string id, Customer customer);*/

    public Task<Cart> GetCustomerCart(string customerId);

    public bool AddItemToCart(Product newItem, Cart cart, string customerId);
}
