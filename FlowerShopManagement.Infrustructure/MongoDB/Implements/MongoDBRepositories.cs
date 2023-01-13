using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    public readonly IMongoDBContext _mongoDbContext;
    protected readonly IMongoCollection<TEntity> _mongoDbCollection;

    public BaseRepository(IMongoDBContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
        _mongoDbCollection = _mongoDbContext.GetCollection<TEntity>(typeof(TEntity).Name + 's');
    }

    // Filters Configuration
    private static FilterDefinition<TEntity> idFilter(string id) => Builders<TEntity>.Filter.Eq("_id", id);
    private static FilterDefinition<TEntity> customFilter(string fieldName, IComparable value) => Builders<TEntity>.Filter.Eq(fieldName, value);

    // Indexes Configuration
    protected string CreateUniqueIndex(FieldDefinition<TEntity, string> field)
    {
        var indexDefine = Builders<TEntity>.IndexKeys.Ascending(field);
        var indexOption = new CreateIndexOptions<TEntity>() { Unique = true };
        var indexModel = new CreateIndexModel<TEntity>(indexDefine, indexOption);
        return _mongoDbCollection.Indexes.CreateOne(indexModel);
    }

    // Aggregations
    protected virtual List<TAggregate> Aggregate<TAggregate>(PipelineDefinition<TEntity, BsonDocument> pipeline)
    {
        try
        {
            var bsonList = _mongoDbCollection.Aggregate(pipeline).ToList();

            var result = new List<TAggregate>();

            foreach (var bsonDoc in bsonList)
            {
                var model = BsonSerializer.Deserialize<TAggregate>(bsonDoc);
                result.Add(model);
            }

            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

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

    public virtual async Task<List<TEntity>> GetAll(int skip = 0, int? limit = null)
    {
        try
        {
            return await _mongoDbCollection.Find(Builders<TEntity>.Filter.Empty).Skip(skip).Limit(limit).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public virtual async Task<TEntity?> GetByField(string fieldName, IComparable value)
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

    public virtual async Task<TEntity?> GetById(string id)
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

    public virtual async Task<List<TEntity>> GetByIds(List<string> ids)
    {
        try
        {
            var filter = Builders<TEntity>.Filter.In("_id", ids.ToArray());
            return await _mongoDbCollection.Find(filter).ToListAsync();
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

    //public async Task<bool> UpdateField(string id, string fieldName, string value)
    //{
    //    var filter = idFilter(id);
    //    var update = Builders<TEntity>.Update.Set(fieldName, value);

    //    try
    //    {
    //        var result = await _mongoDbCollection.UpdateOneAsync(filter, update);
    //        return result.IsAcknowledged;
    //    } 
    //    catch (Exception e)
    //    {
    //        throw new Exception(e.Message);
    //    }
    //}

    // Override disposable function

    public void Dispose() => GC.SuppressFinalize(this);
}

// Specific repositories
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
    {
        CreateUniqueIndex("email");
        CreateUniqueIndex("phoneNumber");
    }

    public async Task<User?> GetByEmailOrPhoneNb(string emailOrPhoneNb)
    {
        var filter = Builders<User>.Filter.Eq("email", emailOrPhoneNb);
        filter |= Builders<User>.Filter.Eq("phoneNumber", emailOrPhoneNb);
        var result = await _mongoDbCollection.FindAsync(filter);
        return result.FirstOrDefault();
    }

    public async Task<List<User>?> GetByRole(Role role)
    {
        var filter = Builders<User>.Filter.Eq("role", role);
        var result = await _mongoDbCollection.FindAsync(filter);
        return result.ToList();
    }
}

public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext)
    {
        CreateUniqueIndex("customerId");
    }
}
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }
}

public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
{
    public MaterialRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }
}

public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }
}

public class VoucherRepository : BaseRepository<Voucher>, IVoucherRepository
{
    public VoucherRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }
}
public class AddressRepository : BaseRepository<Address>, IAddressRepository
{
    public AddressRepository(IMongoDBContext mongoDbContext) : base(mongoDbContext) { }
}