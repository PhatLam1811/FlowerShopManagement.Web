using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using static FlowerShopManagement.Web.Helper;
namespace FlowerShopManagement.Web.Controllers
{
    //--------------------------------------Customer Order Controller--------------------------------------------------

    public class OrderController : Controller
    {
        //Services
        ISaleService _saleServices;
        //Repositories
        IOrderRepository _orderRepository;
        IProductRepository _productRepository;
        IUserRepository _userRepository;

        public OrderController(ISaleService saleServices, IOrderRepository orderRepository, IProductRepository productRepository,
            IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _saleServices = saleServices;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Order = true;
            //Set up default values for OrderPage

            ViewData["Categories"] = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            List<OrderModel> orderMs = new List<OrderModel>();

            List<ProductModel> products = new List<ProductModel>();
            products.Add(new ProductModel() { Name = "Fake Flower", Amount = 2, Color = Color.Blue, UniPrice = 20, Id = "hsksd" });

            orderMs.Add(new OrderModel() { AccountID = "23", Amount = 3, Date = DateTime.Now, FullName = "Quy ganh team", DeliveryMethod = DeliverryMethods.sampleMethod, DeliveryCharge = 2, PhoneNumber = "93803xxxx", Products = products, Status = Status.Waiting, Total = 200 });

            return View(orderMs);
        }

        [HttpGet]
        public IActionResult OrderDetail()
        {
            return View();
        }
    }
}
