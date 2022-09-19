using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Infrustructure.Interfaces
{
    public interface ICustomerDatabaseSettings
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string CustomerCollection { get; set; }
    }
}
