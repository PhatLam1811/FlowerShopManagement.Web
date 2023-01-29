﻿using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.MongoDB.Interfaces;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
{
    public Task<bool> Add(TEntity entity);

    public Task<List<TEntity>> GetAll(int skip = 0, int? limit = null);
    
    public Task<TEntity?> GetById(string id);

    public Task<List<TEntity>> GetByIds(List<string> ids);

    public Task<TEntity?> GetByField(string fieldName, IComparable value);

    public Task<bool> RemoveById(string id);

    public Task<bool> RemoveByField(string fieldName, IComparable value);

    public Task<bool> UpdateById(string id, TEntity entity);

    public Task<bool> UpdateByField(string fieldName, IComparable value, TEntity entity);
}

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> GetByEmailOrPhoneNb(string emailOrPhoneNb);
    public Task<List<User>?> GetByRole(Role role);
}

public interface ICartRepository : IBaseRepository<Cart> { }

public interface ICategoryRepository : IBaseRepository<Category> { }

public interface IMaterialRepository : IBaseRepository<Material> { }

public interface ISupplierRepository : IBaseRepository<Supplier> { }

public interface IProductRepository : IBaseRepository<Product>
{
    public List<Product>? GetAllLowOnStock(int minimumAmount);
    public Task<List<Product>?> GetProductsById(List<string?> ids);
    public int GetLowOnStockCount(int minimumAmount);
}

public interface IVoucherRepository : IBaseRepository<Voucher> { }
public interface IAddressRepository : IBaseRepository<Address> { }
