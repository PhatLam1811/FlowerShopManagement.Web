using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Web.ViewModels
{
    public class ProductVM
    {
        public PaginatedList<ProductModel>? productModels = null;

        public List<string> categories= new List<string>();
    }
}