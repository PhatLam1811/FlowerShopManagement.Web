using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Application.Models
{
	public class InforDeliveryModel
	{
		public string? Name { get; set; }
		public string? Phone { get; set; }
		public string? Address { get; set; }
		public bool IsDefault { get; set; }
	}
}
