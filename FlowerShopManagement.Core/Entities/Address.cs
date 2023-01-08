using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Core.Entities
{
	public class Address
	{
		public List<Address> addresses = new List<Address>();

		public object? _id { get; set; } = null;
		public string _city { get; set; } = string.Empty;
		public string _cityId { get; set; } = string.Empty;
		public string _district { get; set; } = string.Empty;
		public string _districtID { get; set; } = string.Empty;
		public string _commune { get; set; } = string.Empty;
		public string _communeId { get; set; } = string.Empty;
		public string _communeLevel { get; set; } = string.Empty;
		public string _englishName { get; set; } = string.Empty;
		public string _detail { get; set; } = string.Empty;
		

	}

}
