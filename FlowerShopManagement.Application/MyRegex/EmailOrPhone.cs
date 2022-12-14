using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FlowerShopManagement.Application.MyRegex
{
    public class EmailOrPhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            if (Regex.IsMatch(value.ToString(), @"^([\+]?84[-]?|[0])?[1-9][0-9]{8}$")) {
                return true;
            }

            if (Regex.IsMatch(value.ToString(), @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$"))
            {
                return true;
            }

            return false;
        }
    }
}
