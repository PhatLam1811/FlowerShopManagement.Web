using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace FlowerShopManagement.Application.Models;

public class UserModel
{
    public string _id { get; set; }

    // Account
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; } = Role.Customer;

    // Profile
    public string Name { get; set; }
    public string Avatar { get; set; } = "user.png";
    public Gender Gender { get; set; }
    public DateTime BirthYear { get; set; }
    public List<InforDeliveryModel> InforDelivery = new List<InforDeliveryModel>();

    public List<string> FavProductIds { get; set; }

    // Extra
    public DateTime CreatedDate { get; set; }
    public DateTime LastModified { get; set; }

    // More Extra

    public IFormFile? FormFile { get; set; } // help to generate user avatar, no need to store on dB nha 

    public UserModel(User entity)
    {
        _id = entity._id;

        Email = entity.email;
        PhoneNumber = entity.phoneNumber;
        Password = entity.password;
        Role = entity.role;

        Name = entity.name;
        Avatar = entity.avatar;
        Gender = entity.gender;
        BirthYear = entity.birthYear;
        foreach(var i in entity.inforDelivery)
        {
            InforDelivery.Add(new InforDeliveryModel(i));

        }
        FavProductIds = entity.favProductIds;

        CreatedDate = entity.createdDate;
        LastModified = entity.lastModified;
    }

    public UserModel()
    {
        //_id = new Guid().ToString();
        //_password = string.Empty;
        //CreatedDate = DateTime.Now;
        //Role = Role.Customer;
        //Gender = Gender.Female;
        //Addresses = new string[2];
    }

    public void ToEntity(ref User entity)
    {
        entity._id = _id;

        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
        entity.password = Password;
        entity.role = Role;

        entity.name = Name;
        entity.avatar = Avatar;
        entity.gender = Gender;
        entity.birthYear = BirthYear;
        foreach (var i in InforDelivery)
        {
            entity.inforDelivery.Add(i.ToEntity());

        }
       
        entity.favProductIds = FavProductIds;

        entity.createdDate = CreatedDate;
        entity.lastModified = LastModified;
    }
    public User ToEntity()
    {
        var entity = new User();

        entity._id = _id;
        entity.password = Password;
        entity.role = Role;
        entity.phoneNumber = PhoneNumber;
        entity.gender = Gender;
        entity.birthYear = BirthYear;
        foreach (var i in InforDelivery)
        {
            entity.inforDelivery.Add(i.ToEntity());

        }

        entity.createdDate = CreatedDate;
        entity.lastModified = LastModified;
        entity.name = Name;
        entity.email = Email;
        entity.avatar = Avatar;
        entity.favProductIds = FavProductIds;

        return entity;
    }

	public User ToNewEntity()
    {
        var entity = new User();

        entity.password = Password;
        entity.name = Name;
        entity.avatar = Avatar;
        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
        entity.gender = Gender;
        entity.birthYear = BirthYear;
        foreach (var i in InforDelivery)
        {
            entity.inforDelivery.Add(i.ToEntity());

        }


        return entity;
    }
    public async Task<User> ToNewEntity(string wwwRootPath)
    {
        var entity = new User();

        if (this.FormFile != null && this.FormFile.Length > 0)
        {
            string fileName = this.FormFile.FileName;
            string path = Path.Combine(wwwRootPath + "/avatar/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await this.FormFile.CopyToAsync(fileStream);
                entity.avatar = this.FormFile.FileName;
            }
        }
        entity.name = Name;
        entity.password = Password;
        entity.email = Email;
        entity.phoneNumber = PhoneNumber;
        entity.gender = Gender;
        entity.birthYear = BirthYear;
        foreach (var i in InforDelivery)
        {
            entity.inforDelivery.Add(i.ToEntity());

        }


        return entity;
    }
    public bool IsPasswordMatched(string encryptedPassword)
    {
        return Password == encryptedPassword;
       
    }
}

public class UserBasicInfoModel
{
	public string _id { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;


	// Profile
	public string Name { get; set; } = string.Empty;
	public string Avatar { get; set; } = "user.png";
	public Gender Gender { get; set; } = Gender.Male;
	public DateTime BirthYear { get; set; } = DateTime.UtcNow;
	// More Extra

	public IFormFile? FormFile { get; set; } // help to generate user avatar, no need to store on dB nha 

    public UserBasicInfoModel(User entity)
    {
        _id = entity._id;

        Email = entity.email;
        PhoneNumber = entity.phoneNumber;
        Name = entity.name;
        Avatar = entity.avatar;
        Gender = entity.gender;
        BirthYear = entity.birthYear;

    }
    public UserBasicInfoModel() { }

	public async Task ChangesTracking(User editUser,string root)
	{
		//_id = editUser._id;
		if (editUser.email != this.Email)
		{
			editUser.email = Email;

		}
		if (editUser.phoneNumber != this.PhoneNumber)
		{
			editUser.phoneNumber = PhoneNumber;

		}
		if (editUser.name != this.Name)
		{
			editUser.name = Name;

		}
		if (editUser.birthYear != this.BirthYear)
		{
			editUser.birthYear = BirthYear;
		}
		if (editUser.gender != this.Gender)
		{
			editUser.gender = Gender;

		}
		//change avatar
		if (this.FormFile != null && this.FormFile.Length > 0)
		{
			string fileName = this.FormFile.FileName;
			string path = Path.Combine(root + "/avatar/", fileName);
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await this.FormFile.CopyToAsync(fileStream);
				editUser.avatar = this.FormFile.FileName;
			}
		}
		//editUser.Avatar = Avatar;

		//editUser.Addresses = Addresses;
	}
	
	
}