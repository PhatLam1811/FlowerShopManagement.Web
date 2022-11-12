using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlowerShopManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerManagementServices _customerServices;

        public HomeController(ILogger<HomeController> logger, ICustomerManagementServices customerServices)
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
            return View();
        }

        public Task<bool> AddNewCustomer()
        {
            // Hardcode a Customer object for simpleness
            //CustomerModel customer = new Customer();
            //customer.password = "1"; // this should be encrypted later
            //customer.profile.fullName = "Lam Tan Phat";

            //return _customerServices.AddNewCustomer(customer);
            throw new NotImplementedException();
        }

        public bool RemoveCustomerById()
        {
            // Hardcode for simpleness
            //string removedId = "1e69fd8b-ec24-4754-bee5-1151e8c78876";
            //return _customerServices.RemoveCustomerById(removedId);
            throw new NotImplementedException();
        }
        
       
        #endregion

        public IActionResult Index()
        {
            ViewBag.Home = true;
            return View();
        }

        public IActionResult About()
        {
            ViewBag.About = true;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}