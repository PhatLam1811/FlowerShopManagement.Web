using FlowerShopManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Core.Interfaces
{
    public interface IProductCRUD
    {
        public Task<bool> AddNewProduct(Product newProduct);
        public Task<List<Product>> GetAllProducts();
        public Task<Product> GetProductById(string id);
        public bool UpdateProduct(Product updatedProduct);
        public bool RemoveProductById(string id);
    }
}
