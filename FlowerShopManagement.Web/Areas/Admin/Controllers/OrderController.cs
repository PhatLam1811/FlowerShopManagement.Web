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

        [Route("Index")]
        [Route("")]
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
            OrderVM orderVM = new OrderVM();
            orderVM.orderMs = orderMs ?? new List<OrderModel>();
            ViewData["Model"] = orderVM;

            return View(orderMs);
        }

        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
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
        [HttpPut]
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

        [Route("Delete")]
        [HttpDelete]
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
            /*
                Set up viewmodel
			    Need a current picked items
                Need a current picked customer
                PageList<>
			*/
            if (productMs == null || customerMs == null) return NotFound();

            OrderVM? orderVM = ViewData["Model"] as OrderVM ?? new OrderVM();

            orderVM.AllProductModels = productMs;
            orderVM.customerMs = customerMs;
            TempData["orderVM"] = JsonConvert.SerializeObject(orderVM, Formatting.Indented);
            return View(orderVM);
        }

        // Confirm and create an Order
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(UserModel userModel)
        {
            OrderVM? orderVM = null;
            if (TempData["orderVM"] != null)
            {
                string s = TempData["orderVM"] as string ?? "";
                if (s != "")
                {
                    orderVM = JsonConvert.DeserializeObject<OrderVM>(s);
                }
            }
            else if (TempData["orderVM1"] != null)
            {
                string s = TempData["orderVM1"] as string ?? "";
                if (s != "")
                {
                    orderVM = JsonConvert.DeserializeObject<OrderVM>(s);
                }
            }
            if (orderVM == null || orderVM.Order == null) return NotFound();
            orderVM.Order.Products = orderVM.ProductModels;
            var result = _saleServices.CreateOfflineOrder(orderVM.Order, userModel, _orderRepository, _userRepository, _productRepository);
            if (result != null)
            {
                List<OrderModel> orders = await _saleServices.GetUpdatedOrders(_orderRepository);
                return RedirectToAction("Index"); // A updated _ViewAll table
            }
            return NotFound(); // Can be changed to Redirect
        }

        [Route("PickItemDialog")]
        [HttpPost]
        public async Task<IActionResult> PickItemDialog(string filter = "") // Also handle if there is a filter / search
        {
            OrderVM? orderVM = null;
            string s = TempData["orderVM1"] as string ?? TempData["orderVM"] as string ?? TempData["orderVM2"] as string ?? "";

          
            if (s != "")
            {
                orderVM = JsonConvert.DeserializeObject<OrderVM>(s);
            }
            List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);
            if (orderVM != null)
            {
                orderVM.AllProductModels = productMs;
                TempData["orderVM1"] = JsonConvert.SerializeObject(orderVM, Formatting.Indented);
            }
            int pageSizes = 99;
            return PartialView("_PickItem", PaginatedList<ProductModel>.CreateAsync(productMs, 1, pageSizes)); // This will be an view for dialog / modal
        }

        [Route("PickedAnItem")]
        public IActionResult PickedAnItem(List<string> ids, List<int> amounts) // These 2 list should have the same size // need 1 more parameters like PickItemDialog viewmodel
        {
            OrderVM? orderVM = null;
            string s = TempData["orderVM1"] as string ?? TempData["orderVM"] as string ?? TempData["orderVM2"] as string ?? "";


            if (s != "")
            {
                orderVM = JsonConvert.DeserializeObject<OrderVM>(s);
            }

            if (orderVM == null || orderVM.Order == null) return NotFound();

            OrderModel orderModel = orderVM.Order;
            if (ids != null && amounts != null && orderVM.ProductModels != null && orderVM.AllProductModels != null)
            {
                var currentProducts = orderVM.ProductModels;
                for (int i = 0; i < amounts.Count && i < ids.Count; i++)
                {
                    if (amounts[i] != 0 && ids[i] != "")
                    {
                        ProductModel? p = currentProducts.Find(o => o.Id != null && o.Id.Equals(ids[i]));
                        if (p != null)
                        {
                            p.Amount += amounts[i];
                        }
                        else
                        {
                            ProductModel? product = orderVM.AllProductModels.FirstOrDefault(o => o.Id != null && o.Id.Equals(ids[i]));
                            if (product != null)
                            {
                                product.Amount = amounts[i];
                                currentProducts.Add(product);

                            }
                        }

                    }
                }

            }
            //CurrentOrder will be ready for the next request
            TempData["orderVM"] = JsonConvert.SerializeObject(orderVM, Newtonsoft.Json.Formatting.Indented);

            return PartialView("_PickedItemsTable", orderVM.ProductModels);
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
           
            if (orderVM != null && orderVM.ProductModels != null)
            {
                
                orderVM.ProductModels = orderVM.ProductModels.Where(p => p.Id != id).ToList();
                TempData["orderVM2"] = JsonConvert.SerializeObject(orderVM, Formatting.Indented);
                return PartialView("_PickedItemsTable", orderVM.ProductModels??new List<ProductModel>());

            }

            return NotFound();
            //Should update Viewmodel for newest updates

        }

        [Route("PickCustomerDialog")]
        [HttpPost]
        public async Task<IActionResult> PickCustomerDialog(string filter = "") // Also handle if there is a filter / search
        {

            OrderVM? orderVM = null;
            string s = TempData["orderVM1"] as string ?? TempData["orderVM"] as string ?? TempData["orderVM2"] as string ?? "";


            if (s != "")
            {
                orderVM = JsonConvert.DeserializeObject<OrderVM>(s);
            }
            List<UserDetailsModel>? userMS = await _staffService.GetUsersAsync();
            if (orderVM != null)
            {
                orderVM.customerMs = userMS;
                TempData["orderVM1"] = JsonConvert.SerializeObject(orderVM, Formatting.Indented);
            }
            if (userMS != null)
            {
                return PartialView("_PickCustomer", userMS); // This will be an view for dialog / modal
            }
            return NotFound();
        }


        [Route("PickedCustomer")]
        [HttpPost]
        public async Task<IActionResult> PickedCustomer(string phone)
        {
            OrderVM? orderVM = null;
            if (TempData["orderVM1"] != null)
            {
                string s = TempData["orderVM1"] as string ?? "";
                if (s != "")
                {
                    orderVM = JsonConvert.DeserializeObject<OrderVM>(s);
                }
                var user = await _staffService.GetUserByPhone(phone);
                if (user != null && orderVM != null)
                {
                    orderVM.Customer = user;
                    TempData["orderVM"] = JsonConvert.SerializeObject(orderVM, Formatting.Indented);
                    return PartialView("_PickedCustomerTable", user); // This will be an update view for current customer table

                }
            }

            return NotFound();
        }

        [Route("CompletedCheck")]
        public async Task<IActionResult> CompletedCheck(string id)
        {

            if (id == null) return NotFound();
            var orderList = await _saleServices.GetADetailOrder(id, _orderRepository);
            if (orderList == null) return NotFound();


            orderList.Status = Status.Delivered;
            var result = await _orderRepository.UpdateById(orderList.Id, orderList.ToEntity());

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
            if (order == null || productList == null || order.Id == null || order.Status == Status.Purchased || order.Status == Status.Delivered || order.Status == Status.Canceled) return NotFound();

            bool flag = true;

            foreach (var product in order.Products)
            {
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    if (product.Amount > productList.Where(i => i.Id == product.Id).First().Amount)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag == false)
            {

                order.Status = Status.OutOfStock;
            }
            else
            {
                order.Status = Status.Delivering;

            }
            var result = await _orderRepository.UpdateById(order.Id, order.ToEntity());

            if (result == true)
                return RedirectToAction("Index");
            else
            {
                return NotFound();
            }
        }

        [Route("CanceledCheck")]
        public async Task<IActionResult> CanceledCheck(string id)
        {

            if (id == null) return NotFound();
            var order = await _saleServices.GetADetailOrder(id, _orderRepository);
            if (order == null) return NotFound();


            order.Status = Status.Canceled;
            var result = await _orderRepository.UpdateById(order.Id, order.ToEntity());

            if (result == true)
                return RedirectToAction("Index");
            else
            {
                return NotFound();
            }
        }

    }
}
