using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class ProfileCRUD : IProfileCRUD
{
    private IMongoDbDAO _mongoDbDAO;

    public ProfileCRUD(IMongoDbDAO mongoDbDAO) => _mongoDbDAO = mongoDbDAO;

    

    // Implementation
    public async Task<bool> AddNewProfile(Profile newProfile)
    {
        try
        {
            await _mongoDbDAO._profileCollection.InsertOneAsync(newProfile);
            return true;
        }
        catch { return false; }

    }

    public async Task<List<Profile>> GetAllProfiles()
    {
        var results = await _mongoDbDAO._profileCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<Profile> GetProfileById(string id) // get by CustomerAccountID, not its real id
    {
        var filter = Builders<Profile>.Filter.Eq("_accountID", id);
        var result = await _mongoDbDAO._profileCollection.FindAsync<Profile>(filter);
        return result.FirstOrDefault();

    }

    public bool RemoveProfileById(string id)
    {
        try
        {
            _mongoDbDAO._profileCollection.DeleteOne(c => c._id == id);
            return true;
        }
        catch { return false; }

    }

    public bool UpdateProfile(Profile updatedProfile)
    {
        var filter = Builders<Profile>.Filter.Eq("_id", updatedProfile._id);
        try
        {
            _mongoDbDAO._profileCollection.ReplaceOne(filter, updatedProfile, new ReplaceOptions() { IsUpsert = true });
            return true;
        }
        catch { return false; }
    }

    
}

