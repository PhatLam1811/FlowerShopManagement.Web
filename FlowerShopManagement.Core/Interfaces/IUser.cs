﻿using FlowerShopManagement.Core.Common;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Interfaces;

public interface IUser : IBaseServices<User>
{
    public Task<bool> Rename(string newName);

    public Task<bool> SetPassword(string newPassword);

    public Task<bool> SetEmailAddress(string newEmail);

    public Task<bool> SetPhoneNumber(string newPhoneNumber);

    public Task<bool> SetAvatar(string newAvatar);

    public Task<bool> SetGender(Genders newGender);

    public Task<bool> SetAge(int newAge);

    public Task<bool> SetAddresses(List<string> newAddressList);
}