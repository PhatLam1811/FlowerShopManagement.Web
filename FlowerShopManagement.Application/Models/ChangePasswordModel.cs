﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Application.Models
{
	public class ChangePasswordModel
	{
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
		public string? ConfirmPassword { get; set; }
	}
}
