using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Web.ViewModels
{
    public class OrderVM
    {
        public OrderModel? Order = new OrderModel();
        public List<ProductModel>? ProductModels = new List<ProductModel>();
        public List<ProductModel>? AllProductModels = new List<ProductModel>();
        public List<UserModel>? UserModels= new List<UserModel>();

        public bool isOkay = false;

    }
}