using FlowerShopManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Application.Models
{
	public class AddressModel
	{
		private static AddressModel? instance;

		public AddressModel(Address address)
		{
			if ( address._id != null)
			{
                this._id = address._id.ToString();

            }
			else
			{
                this._id = Guid.NewGuid().ToString();

            }
            this._cityId = address._cityId;
			this._city = address._city;
			this._district = address._district;
			this._districtID = address._districtID;
			this._commune = address._commune;
			this._communeId = address._communeId;
			this._communeLevel = address._communeLevel;
		}

        public AddressModel()
        {
            
        }

        public string _id { get; set; } = string.Empty;
		public string _city { get; set; } = string.Empty;
		public string _cityId { get; set; } = string.Empty;
		public string _district { get; set; } = string.Empty;
		public string _districtID { get; set; } = string.Empty;
		public string _commune { get; set; } = string.Empty;
		public string _communeId { get; set; } = string.Empty;
		public string _communeLevel { get; set; } = string.Empty;

	}

	public class AddressSingleTon
	{
		private static AddressSingleTon? instance;
		public List<AddressModel> addressModels = new List<AddressModel>();

		public AddressSingleTon(List<Address> addresses)
		{
			foreach (var i in addresses)
			{
				addressModels.Add(new AddressModel(i));
			}
		}

		public static AddressSingleTon GetInstance(List<Address> addresses)
		{
			if (instance == null) instance = new AddressSingleTon(addresses); ;
			return instance;
		}
		public static AddressSingleTon? IsNullOrNot()
		{
			if (instance != null)
				return instance;
			else return null;
		}
	}



}
