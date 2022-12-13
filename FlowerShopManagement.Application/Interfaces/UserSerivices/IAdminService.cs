using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Application.Interfaces.UserSerivices;

public interface IAdminService : IStaffService
{
    public Task<bool> AddStaffAsync(UserDetailsModel newStaffModel, Role role);
    public Task<bool> EditUserRoleAsync(UserDetailsModel userModel, Role role);
}
