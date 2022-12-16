using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Web.ViewModels
{
    public class OrderVM
    {
        public OrderModel? Order = new OrderModel();
        public UserDetailsModel? Customer = null;
        public List<ProductModel>? ProductModels = new List<ProductModel>();
        public List<ProductModel>? AllProductModels = new List<ProductModel>();
        public List<UserDetailsModel>? customerMs= new List<UserDetailsModel>();

        public bool isOkay = false;

    }
}