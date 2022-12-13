using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Web.ViewModels
{
    public class OrderVM
    {
        public OrderModel? Order = new OrderModel();
        public List<ProductModel>? ProductModels = new List<ProductModel>();
        public List<UserModel>? UserModels= null;

    }
}