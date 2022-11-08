using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly IMongoDBContext _mongoDbContext;
    protected readonly IMongoCollection<TEntity> _mongoDbCollection;

    public BaseRepository(IMongoDBContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
        _mongoDbCollection = _mongoDbContext.GetCollection<TEntity>(typeof(TEntity).Name + 's');
    }

    // Filters Configuration
    private static FilterDefinition<TEntity> idFilter(string id) => Builders<TEntity>.Filter.Eq("_id", id);
    private static FilterDefinition<TEntity> customFilter(string fieldName, IComparable value)
        => Builders<TEntity>.Filter.Eq(fieldName, value);

    // CRUD operations
    public virtual async Task<bool> Add(TEntity entity)
    {
        try
        {
            await _mongoDbCollection.InsertOneAsync(entity);
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        try
        {
            return await _mongoDbCollection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<TEntity> GetByField(string fieldName, IComparable value)
    {
        try
        {
            return await _mongoDbCollection.Find(customFilter(fieldName, value)).SingleOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<TEntity> GetById(string id)
    {
        try
        {
            return await _mongoDbCollection.Find(idFilter(id)).SingleOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<bool> RemoveById(string id)
    {
        try
        {
            var result = await _mongoDbCollection.DeleteOneAsync(idFilter(id));
            return result.IsAcknowledged;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<bool> RemoveByField(string fieldName, IComparable value)
    {
        try
        {
            var result = await _mongoDbCollection.DeleteOneAsync(customFilter(fieldName, value));
            return result.IsAcknowledged;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<bool> UpdateById(string id, TEntity entity)
    {
        try
        {
            var result = await _mongoDbCollection.ReplaceOneAsync(idFilter(id), entity);
            return result.IsAcknowledged;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<bool> UpdateByField(string fieldName, IComparable value, TEntity entity)
    {
        try
        {
            var result = await _mongoDbCollection.ReplaceOneAsync(customFilter(fieldName, value), entity);
            return result.IsAcknowledged;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    // Override disposable function
    public void Dispose() => GC.SuppressFinalize(this);
}

// Specific repositories
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }
}

public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }
}
