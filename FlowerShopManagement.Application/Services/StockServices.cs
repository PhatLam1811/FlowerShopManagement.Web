using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Enums;
using System.Collections.Generic;
using FlowerShopManagement.Infrustructure.Interfaces;

// ************ THIS IS A SAMPLE INTERFACE FOR CUSTOMER SERVICES **************
// - New adjustments could be made in future updates
// - This should be a use case logic contains the CRUD operation of Customer & Cart objects 

namespace FlowerShopManagement.Core.Services
{
    public class StockServices : IStockServices
    {
        
        public IProductCRUD _productCRUD;
        

        // APPLICATION SERVICES (USE CASES)
        public StockServices(IProductCRUD productCRUD)
        {
            _productCRUD = productCRUD;
        }

       

      

        
    }
}
