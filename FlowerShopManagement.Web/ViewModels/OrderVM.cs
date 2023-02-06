using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Web.ViewModels
{
    public class OrderVM
    {
        public List<OrderModel> orderMs = new List<OrderModel>();
        public OrderModel Order = new OrderModel();
        public UserModel? CurrentCustomer = null;
        public List<ProductModel>? CurrentProductModels = new List<ProductModel>();
        public List<ProductModel>? AllProductModels = new List<ProductModel>();
        public List<UserModel1>? customerMs = new List<UserModel1>();

        public bool isOkay = false;

    }
}