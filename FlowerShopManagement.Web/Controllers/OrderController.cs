using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Mvc;
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
        ICustomerfService _customerService;
        IAuthService _authService;

        public OrderController(ISaleService saleServices, IOrderRepository orderRepository, IProductRepository productRepository,
            IUserRepository userRepository, ICustomerfService customerfService, IAuthService authService)
        {
            _orderRepository = orderRepository;
            _saleServices = saleServices;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerService = customerfService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Order = true;
            //Set up default values for OrderPage

            ViewData["Categories"] = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            //List<OrderModel> orderMs = new List<OrderModel>();

            //List<ProductModel> products = new List<ProductModel>();
            //products.Add(new ProductModel() { Name = "Fake Flower", Amount = 2, Color = Color.Blue, UniPrice = 20, Id = "hsksd" });

            //orderMs.Add(new OrderModel() { AccountID = "23", Amount = 3, Date = DateTime.Now, FullName = "Quy ganh team", DeliveryMethod = DeliverryMethods.sampleMethod, DeliveryCharge = 2, PhoneNumber = "93803xxxx", Products = products, Status = Status.Waiting, Total = 200 });

            string? userId = "";
            List<OrderModel>? orderMs = new List<OrderModel>();

            if (this.HttpContext != null)
            {
                userId = _authService.GetUserId();

                orderMs = await _customerService.GetOrdersOfUserAsync(userId, _orderRepository);
            }

            return View(orderMs);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                string? userId = "";
                List<OrderModel>? orderMs = new List<OrderModel>();

                if (this.HttpContext != null)
                {
                    userId = _authService.GetUserId();

                    orderMs = await _customerService.GetOrdersOfUserAsync(userId, _orderRepository);
                }

                if (orderMs?.Count > 0)
                {
                    OrderModel orderM = orderMs.Where(o => o.Id == id).First();
                    return View(orderM);
                }
            }

            return NotFound();
        }
    }
}
