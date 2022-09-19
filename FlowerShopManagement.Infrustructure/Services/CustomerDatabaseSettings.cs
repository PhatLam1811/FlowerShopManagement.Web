using FlowerShopManagement.Infrustructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Infrustructure.Services
{
    public class CustomerDatabaseSettings : ICustomerDatabaseSettings
    {
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
        public string CustomerCollection { get; set; } = String.Empty;
    }
}
