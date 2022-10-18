using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlowerShopManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerServices _customerServices;

        public HomeController(ILogger<HomeController> logger, ICustomerServices customerServices)
        {
            _logger = logger;
            _customerServices = customerServices;
        }

        // =======================================================================================================
        // THESE FUNCTIONS BELOW IN THE SAMPLE REGION ARE ONLY USED FOR TESTING & UNDERSTANDING CLEAN ARCHITECTURE
        // - Change the function names in index.cshtml file for testing multiple functions (button onlick event)
        // - Can code some new functions to test (please keep it down to only 1 or 2 more...)
        // =======================================================================================================
        #region Sample function
        public IActionResult AddItemtoCart()
        {
            List<Product> demoList = new List<Product>();

            List<Categories> categories = new List<Categories>();

            Product demoProduct = new Product("demo", "", categories, 10000, 0.1f);

            Cart demoCart = new Cart("jksdahfkjasdhkfjasdh", demoList, 0);

           // demoCart._id = "3604d6b9-1c78-44dc-82a3-821ab0904416";

            _customerServices.AddItemToCart(demoProduct, demoCart, "jksdahfkjasdhkfjasdh");

            return View();
        }

        public Task<bool> AddNewCustomer()
        {
            // Hardcode a Customer object for simpleness
            Customer customer = new Customer();
            customer._username = "lamphat1811";
            customer._password = "1"; // this should be encrypted later
            customer._fullName = "Lam Tan Phat";

            return _customerServices.AddNewCustomer(customer);
        }

        public bool RemoveCustomerById()
        {
            // Hardcode for simpleness
            string removedId = "1e69fd8b-ec24-4754-bee5-1151e8c78876";
            return _customerServices.RemoveCustomerById(removedId);
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Wishes()
        {
            return View();
        }

        public IActionResult ProductDetail()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}