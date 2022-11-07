using FlowerShopManagement.Core.Common;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB;

public class UserDAOServices : IBaseServices<User>
{
    private IMongoDBServices _mongoDbServices;
    private const string _usersCollection = Constants.KEY_USERS;

    public UserDAOServices(IMongoDBServices mongoDbContext)
    {
        _mongoDbServices = mongoDbContext;
    }

    public bool Add(User newRecord)
    {
        try
        {
            var usersCollection = _mongoDbServices.ConnectToMongo<User>(_usersCollection);
            usersCollection.InsertOne(newRecord);
            return true;
        } catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<User>> GetAll()
    {
        try
        {
            var usersCollection = _mongoDbServices.ConnectToMongo<User>(_usersCollection);
            var filter = Builders<User>.Filter.Exists("_id");
            var result = await usersCollection.FindAsync(filter);
            return result.ToList();
        } catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<User> GetById(string id)
    {
        try
        {
            var usersCollection = _mongoDbServices.ConnectToMongo<User>(_usersCollection);
            var filter = Builders<User>.Filter.Eq("_id", id);
            var result = await usersCollection.FindAsync(filter);
            return result.FirstOrDefault();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> RemoveById(string id)
    {
        try
        {
            var usersCollection = _mongoDbServices.ConnectToMongo<User>(_usersCollection);
            var filter = Builders<User>.Filter.Eq("_id", id);
            await usersCollection.DeleteOneAsync(filter);
            return true;
        } catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> UpdateById(string id, User updatedRecord)
    {
        try
        {
            var usersCollection = _mongoDbServices.ConnectToMongo<User>(_usersCollection);
            var filter = Builders<User>.Filter.Exists("_id");
            await usersCollection.ReplaceOneAsync(filter, updatedRecord, new ReplaceOptions { IsUpsert = false });
            return true;
        } catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
