using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.MongoDB.Interfaces;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
{
    public Task<bool> Add(TEntity entity);

    public Task<List<TEntity>> GetAll();

    public Task<TEntity> GetById(string id);

    public Task<TEntity> GetByField(string fieldName, IComparable value);

    public Task<bool> RemoveById(string id);

    public Task<bool> RemoveByField(string fieldName, IComparable value);

    public Task<bool> UpdateById(string id, TEntity entity);

    public Task<bool> UpdateByField(string fieldName, IComparable value, TEntity entity);
}

public interface IUserRepository : IBaseRepository<User> 
{
    public Task<User> GetByEmailOrPhoneNb(string emailOrPhoneNb);
    public Task<List<User>> GetByRole(Role role);
}

public interface ICartRepository : IBaseRepository<Cart> { }

public interface IOrderRepository : IBaseRepository<Order> { }

public interface ISupplierRepository : IBaseRepository<Supplier> { }

public interface IProductRepository : IBaseRepository<Product> 
{
    public Task<List<Product>> GetAllLowOnStock(int minimumAmount);
}
public interface IVoucherRepository : IBaseRepository<Voucher> { }
