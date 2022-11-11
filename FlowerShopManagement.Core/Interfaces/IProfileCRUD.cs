using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Core.Interfaces;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER CRUD **************
// - New adjustments could be made in future updates

public interface IProfileCRUD
{
    public Task<bool> AddNewProfile(Profile newProfile);
    public Task<List<Profile>> GetAllProfiles();
    public Task<Profile> GetProfileById(string id);
    public bool UpdateProfile(Profile updatedProfile);
    public bool RemoveProfileById(string id);
}