using FlowerShopManagement.Application.Models;

namespace FlowerShopManagement.Web.ViewModels
{
    public class UserCreateVM
    {
        public UserModel userModel{ get; set; } = new UserModel();
        public string city { get; set; } = string.Empty;
        public string district { get; set; } = string.Empty;
        public string ward { get; set; } = string.Empty;
        public string detailAddress { get; set; } = string.Empty;
       

    }


}