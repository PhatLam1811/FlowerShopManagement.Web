using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Web.ViewModels;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    //--------------------------------------Admin Order Controller--------------------------------------------------
    [Area("Admin")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        //Services
        ISaleService _saleServices;
        IStockService _stockServices;
        IUserService _userServices;
        IStaffService _staffService;
        //Repositories
        IOrderRepository _orderRepository;
        IProductRepository _productRepository;
        IUserRepository _userRepository;

        public OrderController(ISaleService saleServices, IOrderRepository orderRepository, IProductRepository productRepository,
            IUserRepository userRepository, IStockService stockServices, IUserService userServices, IStaffService staffService)
        {
            _orderRepository = orderRepository;
            _saleServices = saleServices;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _stockServices = stockServices;
            _userServices = userServices;
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filter = "")
        {
            ViewBag.Order = true;
            //Set up default values for OrderPage

            ViewData["Categories"] = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            List<OrderModel> orderMs = await _saleServices.GetUpdatedOrders(_orderRepository);
            if (filter != "" && orderMs != null && filter != "All")
            {
                orderMs = orderMs.Where(o => o.Status.ToString() == filter).ToList();
            }

            return View(orderMs);
        }

        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            //Should get a new one because an admin updates data realtime
            if (id == null) return NotFound();
            var order = await _saleServices.GetADetailOrder(id, _orderRepository);
            if (order != null)
            {
                return View(order);
            }
            return RedirectToAction("Index");
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(OrderModel orderModel)
        {
            //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

            //If order get null or Id null ( somehow ) => notfound
            if (orderModel == null || orderModel.Id == null) return NotFound();
            //Check if the order still exists
            var order = await _orderRepository.GetById(orderModel.Id); // check exist might be implemented in sale services somehow....
            if (order != null)
            {
                //If order != null => we will update this order by using directly orderModel.Id
                //Check orderModel for sure if losing some data
                var result = await _orderRepository.UpdateById(orderModel.Id, orderModel.ToEntity());
                if (result != false)
                {
                    //Update successfully, we pull new list of orders
                    List<OrderModel> orderMs = await _saleServices.GetUpdatedOrders(_orderRepository);

                    return PartialView(/*Coult be a UPDATED ViewModel in here*/);//for example: a _ViewAll 

                }
            }
            return RedirectToAction("Index");
        }

        //no use currently
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Detele(OrderModel orderModel)
        {
            //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

            //If order get null or Id null ( somehow ) => notfound
            if (orderModel == null || orderModel.Id == null) return NotFound();
            //Check if the order still exists
            var order = await _orderRepository.GetById(orderModel.Id);
            if (order != null)
            {
                //If order != null => we will detele this order by using directly orderModel.Id
                //Check orderModel for sure if losing some data
                var result = await _orderRepository.RemoveById(orderModel.Id);
                if (result == false)
                    return RedirectToAction($"Unable to remove {result}");
                else
                {
                    //Detele successfully, we pull a new list of orders
                    List<OrderModel> orderMs = await _saleServices.GetUpdatedOrders(_orderRepository);

                    return PartialView(/*Coult be a UPDATED ViewModel in here*/); //For example: a _ViewAll
                }
            }
            return NotFound();
        }

        //Open an CreatePage
        [Route("Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Set up default values for OrderPage

            ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

            List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);
            List<UserDetailsModel>? customerMs = await _staffService.GetUsersAsync();
            if (productMs == null || customerMs == null) return NotFound();

            OrderVM orderVM = new OrderVM();

            orderVM.AllProductModels = productMs; // List of products
            orderVM.customerMs = customerMs;// List of customers
            if (SetOrderModel(orderVM))
                return View(orderVM); // This will be an view for dialog / modal
            else
                return NotFound();
        }

        // Confirm and create an Order
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(UserModel userModel)
        {
            OrderVM? orderVM = GetOrderModel();
            if (orderVM == null || orderVM.CurrentProductModels == null) return NotFound();
            orderVM.Order.Products = orderVM.CurrentProductModels;
            var result = await _saleServices.CreateOfflineOrder(orderVM.Order, userModel, _orderRepository, _userRepository, _productRepository);
            if (result == true)
            {
                return RedirectToAction("Index"); // A updated _ViewAll table
            }
            return NotFound(); // Can be changed to Redirect
        }

        [Route("PickItemDialog")]
        [HttpPost]
        public async Task<IActionResult> PickItemDialog(string filter = "") // Also handle if there is a filter / search
        {
            OrderVM? orderVM = GetOrderModel();
            if (orderVM == null || orderVM.Order == null) { return NotFound(); }
            //List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);
            List<ProductModel> productMs = orderVM.AllProductModels ?? await _stockServices.GetUpdatedProducts(_productRepository);
            if (filter != "")
            {
                productMs = productMs.Where(i => i.Name.Contains(filter)).ToList();
            }

            orderVM.AllProductModels = productMs;
           
            if (SetOrderModel(orderVM))
                return PartialView("_PickItem", productMs); // This will be an view for dialog / modal
            else
                return NotFound();
        }

        [Route("PickedItems")]
        public IActionResult PickedItems(List<string> ids, List<int> amounts) // These 2 list should have the same size // need 1 more parameters like PickItemDialog viewmodel
        {
            OrderVM? orderVM = GetOrderModel();

            if (orderVM == null || orderVM.Order == null || ids == null || amounts == null
                || orderVM.CurrentProductModels == null || orderVM.AllProductModels == null) return NotFound();

            _saleServices.PickItems(ids, amounts, orderVM.CurrentProductModels, orderVM.AllProductModels);

            if (SetOrderModel(orderVM))
            {
                return PartialView("_PickedItemsTable", orderVM.CurrentProductModels);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("DeleteItem")]
        [HttpPost]
        public IActionResult DeleteItem(string id)
        {
            OrderVM? orderVM = null;
            string s = TempData["orderVM1"] as string ?? TempData["orderVM"] as string ?? TempData["orderVM2"] as string ?? "";


            if (s != "")
            {
                orderVM = JsonConvert.DeserializeObject<OrderVM>(s);
            }
            //CurrentOrder will be ready for the next request

            if (orderVM != null && orderVM.CurrentProductModels != null)
            {

                orderVM.CurrentProductModels = orderVM.CurrentProductModels.Where(p => p.Id != id).ToList();
                TempData["orderVM2"] = JsonConvert.SerializeObject(orderVM, Formatting.Indented);
                return PartialView("_PickedItemsTable", orderVM.CurrentProductModels ?? new List<ProductModel>());

            }
            return NotFound();

        }

        [Route("PickCustomerDialog")]
        [HttpPost]
        public async Task<IActionResult> PickCustomerDialog(string filter = "") // Also handle if there is a filter / search
        {

            OrderVM? orderVM = GetOrderModel();

            List<UserDetailsModel>? userMS = await _staffService.GetUsersAsync();
            if (orderVM == null || userMS == null) return NotFound();

            orderVM.customerMs = userMS;
            if (SetOrderModel(orderVM))
                return PartialView("_PickCustomer", userMS);
            else
                return NotFound();
        }


        [Route("PickedCustomers")]
        [HttpPost]
        public async Task<IActionResult> PickedCustomers(string phone)
        {
            OrderVM? orderVM = GetOrderModel();


            var user = await _staffService.GetUserByPhone(phone);
            if (user == null || orderVM == null) return NotFound();

            orderVM.CurrentCustomer = user;

            if (SetOrderModel(orderVM))
                return PartialView("_PickedCustomerTable", user); // This will be an update view for current customer table
            else
                return NotFound();

        }

        [Route("CompletedCheck")]
        public async Task<IActionResult> CompletedCheck(string id)
        {

            if (id == null) return NotFound();
            var updatingOrder = await _saleServices.GetADetailOrder(id, _orderRepository);
            if (updatingOrder == null || updatingOrder.Id == null || updatingOrder.Status != Status.Delivering) return NotFound();


            updatingOrder.Status = Status.Delivered;
            var result = await _orderRepository.UpdateById(updatingOrder.Id, updatingOrder.ToEntity());

            if (result == true)
                return RedirectToAction("Index");
            else
            {
                return NotFound();
            }
        }
        //Confirm that cus Order is ready and on way delivering

        [Route("ConfirmCheck/id={id}")]
        public async Task<IActionResult> ConfirmCheck(string id)
        {
            if (id == null) return NotFound();
            var order = await _saleServices.GetADetailOrder(id, _orderRepository);
            var productList = await _stockServices.GetUpdatedProducts(_productRepository);
            if (order == null || productList == null || order.Id == null || order.Status != Status.Waiting) return NotFound();

            var result = await _saleServices.VerifyOnlineOrder(order, _orderRepository, _productRepository);

            if (result == true)
                return RedirectToAction("Index");
            else
                return NotFound();

        }

        [Route("CanceledCheck")]
        public async Task<IActionResult> CanceledCheck(string id)
        {

            if (id == null) return NotFound();
            var order = await _saleServices.GetADetailOrder(id, _orderRepository);
            if (order == null || order.Id == null) return NotFound();


            order.Status = Status.Canceled;
            var result = await _orderRepository.UpdateById(order.Id, order.ToEntity());

            if (result == true)
                return RedirectToAction("Index");
            else
                return NotFound();

        }

        private OrderVM? GetOrderModel()
        {
            OrderVM? orderVM = null;
            string s = TempData["orderVM1"] as string ?? TempData["orderVM"] as string ?? TempData["orderVM2"] as string ?? "";
            if (String.IsNullOrEmpty(s)) return null;

            orderVM = JsonConvert.DeserializeObject<OrderVM>(s);
            return orderVM;
        }
        private bool SetOrderModel(OrderVM? orderVM = null)
        {
            if (orderVM == null) return false;
            if (TempData["orderVM1"] == null)
            {
                TempData["orderVM1"] = JsonConvert.SerializeObject(orderVM, Formatting.Indented);
            }
            else
            {
                TempData["orderVM"] = JsonConvert.SerializeObject(orderVM, Formatting.Indented);
            }
            return true;
        }
    }
}
