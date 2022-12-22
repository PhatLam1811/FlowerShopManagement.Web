using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Web.ViewModels
{
    public class OrderVM
    {
        public List<OrderModel> orderMs = new List<OrderModel>();
        public OrderModel Order = new OrderModel();
        public UserDetailsModel? CurrentCustomer = null;
        public List<ProductModel>? CurrentProductModels = new List<ProductModel>();
        public List<ProductModel>? AllProductModels = new List<ProductModel>();
        public List<UserDetailsModel>? customerMs= new List<UserDetailsModel>();

        public bool isOkay = false;

    }
}