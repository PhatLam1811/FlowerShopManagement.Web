using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    //--------------------------------------Admin Order Controller--------------------------------------------------
    [Area("Admin")]
    [Route("[area]/[controller]")]
    [Authorize(Policy = "StaffOnly")]
    public class OrderController : Controller
    {
        //Services
        ISaleService _saleServices;
        IStockService _stockServices;
        IUserService _userServices;
        IStaffService _staffService;
        IMaterialRepository _materialRepository;
        ICategoryRepository categoryRepository;
        //Repositories
        IOrderRepository _orderRepository;
        IProductRepository _productRepository;
        IUserRepository _userRepository;

        public OrderController(ISaleService saleServices, IOrderRepository orderRepository, IProductRepository productRepository,
            IUserRepository userRepository, IStockService stockServices, IUserService userServices, IStaffService staffService,
            IMaterialRepository materialRepository, ICategoryRepository categoryRepository)
        {
            _orderRepository = orderRepository;
            _saleServices = saleServices;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _stockServices = stockServices;
            _userServices = userServices;
            _staffService = staffService;
            _materialRepository = materialRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Order = true;
            //Set up default values for OrderPage

            ViewData["Status"] = Enum.GetNames(typeof(Status)).Where(s => s != "sampleStatus").ToList();
            List<OrderModel> orderMs = await _saleServices.GetUpdatedOrders(_orderRepository);
            orderMs = orderMs
           .OrderByDescending(i => i.Status == Status.Waiting)
           .ThenByDescending(i => i.Status == Status.Paying)
           .ThenByDescending(i => i.Status == Status.Delivering)
           .ToList();
            int pageSizes = 8;
            return View(PaginatedList<OrderModel>.CreateAsync(orderMs ?? new List<OrderModel>(), 1, pageSizes));
        }

        [Route("Sort")]
        [HttpPost]
        public async Task<IActionResult> Sort(string sortOrder, int? pageNumber, string? currentCategory, string? currentDateOrdering)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentCategory"] = currentCategory;
            ViewData["CurrentDateOrdering"] = currentDateOrdering;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";


            List<OrderModel> orderMs = await _saleServices.GetUpdatedOrders(_orderRepository);

            if (orderMs != null)
            {

                switch (sortOrder)
                {
                    case "amount_desc":
                        orderMs = orderMs.OrderByDescending(s => s.Amount).ToList();
                        break;
                    //case "Date":
                    //    productMs = (List<ProductModel>)productMs.OrderByDescending(s => s.);
                    //      break;
                    case "date_asc":
                        orderMs = orderMs.OrderBy(s => s.Date).ToList();
                        break;
                    default:
                        //case filter

                        break;
                }

                switch (currentCategory)
                {
                    case "Waiting":
                        orderMs = orderMs.Where(s => s.Status == Status.Waiting).ToList();
                        break;
                    case "Paying":
                        orderMs = orderMs.Where(s => s.Status == Status.Paying).ToList();
                        break;
                    case "Purchased":
                        orderMs = orderMs.Where(s => s.Status == Status.Purchased).ToList();
                        break;
                    case "Delivering":
                        orderMs = orderMs.Where(s => s.Status == Status.Delivering).ToList();
                        break;
                    case "Delivered":
                        orderMs = orderMs.Where(s => s.Status == Status.Delivered).ToList();
                        break;
                    case "OutOfStock":
                        orderMs = orderMs.Where(s => s.Status == Status.OutOfStock).ToList();
                        break;
                    case "Canceled":
                        orderMs = orderMs.Where(s => s.Status == Status.Canceled).ToList();
                        break;

                    default:
                        //All
                        break;
                }
                switch (currentDateOrdering)
                {
                    case "descending":
                        orderMs = orderMs.OrderBy(s => s.Date).ToList();
                        break;
                    case "ascending":
                        orderMs = orderMs.OrderByDescending(s => s.Date).ToList();
                        break;
                    default:
                        //All
                        break;
                }

                int pageSize = 8;
                orderMs = orderMs
           .OrderByDescending(i => i.Status == Status.Waiting)
           .ThenByDescending(i => i.Status == Status.Paying)
           .ThenByDescending(i => i.Status == Status.Delivering)
           .ThenByDescending(i => i.Status == Status.Purchased)
           .ToList();
                PaginatedList<OrderModel> objs = PaginatedList<OrderModel>.CreateAsync(orderMs, pageNumber ?? 1, pageSize);


                return Json(new
                {
                    isValid = true,
                    htmlViewAll = Helper.RenderRazorViewToString(this, "_ViewAll", objs),
                    htmlPagination = Helper.RenderRazorViewToString(this, "_Pagination", objs)

                });
                //return PartialView("_ViewAll",PaginatedList<ProductModel>.CreateAsync(productMs, pageNumber ?? 1, pageSize));
            }
            return NotFound();

        }


        //no use currently
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Detele(OrderModel orderModel)
        {

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

        #region Open an CreatePage
        [Route("Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Set up default values for OrderPage

            ViewData["Categories"] = _stockServices.GetCategories();

            List<ProductModel> productMs = await _stockServices.GetUpdatedProducts();
           // List<UserModel1>? customerMs = await _staffService.GetUsersAsync1();
            if (productMs == null ) return NotFound();

            OrderVM orderVM = new OrderVM();

            orderVM.AllProductModels = productMs; // List of products
            //orderVM.customerMs = customerMs;// List of customers
            //orderVM.customerMs = new List<UserModel>();// List of customers
            if (SetOrderModel(orderVM))
                return View(orderVM); // This will be an view for dialog / modal
            else
                return NotFound();
        }

        // Confirm and create an Order
        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(UserModel? userModel)
        {
            OrderVM? orderVM = GetOrderModel();
            if (orderVM == null || orderVM.CurrentProductModels == null) return NotFound();
            orderVM.Order.Products = orderVM.CurrentProductModels;
            var result = await _saleServices.CreateOfflineOrder(orderVM.Order, userModel ?? new UserModel(), _orderRepository, _userRepository, _productRepository);
            if (result != null)
            {
                //return await Edit(orderVM.Order.Id); // A updated _ViewAll table
                return View("Edit", result);

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
            List<ProductModel> productMs = orderVM.AllProductModels ?? await _stockServices.GetUpdatedProducts();
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

            List<UserModel1>? userMS = await _staffService.GetUsersAsync1();
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

        #endregion

        #region Check and Update an order 

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
        public async Task<IActionResult> Update(OrderModel orderModel, string type = "")
        {
            //If order get null or Id null ( somehow ) => notfound
            if (orderModel == null || orderModel.Id == null || type == "") return NotFound();
            //Check if the order still exists

            switch (type)
            {
                case "Cancel":
                    return await CanceledCheck(orderModel);
                case "Confirm":
                    return await ConfirmCheck(orderModel);
                case "Complete":
                    return await CompletedCheck(orderModel);
                default:
                    return RedirectToAction("Index");

            }
        }


        //[Route("CompletedCheck")]
        public async Task<IActionResult> CompletedCheck(OrderModel orderModel)
        {

            var updatingOrder = await _saleServices.GetADetailOrder(orderModel.Id, _orderRepository);
            if (updatingOrder == null || updatingOrder.Id == null || (updatingOrder.Status != Status.Delivering && updatingOrder.Status != Status.Paying)) return NotFound();
            //Update status
            if (updatingOrder.Status == Status.Delivering)
                updatingOrder.Status = Status.Delivered;
            else if (updatingOrder.Status == Status.Paying)
            {
                updatingOrder.Status = Status.Purchased;
            }

            //Update notes
            if (updatingOrder.Notes != orderModel.Notes)
            {
                updatingOrder.Notes = orderModel.Notes;
            }
            updatingOrder.Date = DateTime.Now;
            var result = await _orderRepository.UpdateById(updatingOrder.Id, updatingOrder.ToEntity());

            if (result == true)
                return RedirectToAction("Index");
            else
            {
                return NotFound();
            }
        }
        //Confirm that cus Order is ready and on way delivering

        //[Route("ConfirmCheck/id={id}")]
        public async Task<IActionResult> ConfirmCheck(OrderModel orderModel)
        {

            var updatingOrder = await _saleServices.GetADetailOrder(orderModel.Id, _orderRepository);
            var productList = await _stockServices.GetUpdatedProducts();
            if (updatingOrder == null || productList == null || updatingOrder.Id == null || updatingOrder.Status != Status.Waiting) return NotFound();

            //Update notes
            if (updatingOrder.Notes != orderModel.Notes)
            {
                updatingOrder.Notes = orderModel.Notes;
            }
            updatingOrder.Date = DateTime.Now;

            var result = await _saleServices.VerifyOnlineOrder(updatingOrder, _orderRepository, _productRepository);

            if (result == true)
                return RedirectToAction("Index");
            else
                return NotFound();

        }

        //[Route("CanceledCheck")]
        public async Task<IActionResult> CanceledCheck(OrderModel orderModel)
        {

            var updatingOrder = await _saleServices.GetADetailOrder(orderModel.Id, _orderRepository);
            if (updatingOrder == null || updatingOrder.Id == null || !(updatingOrder.Status != Status.Purchased && updatingOrder.Status != Status.Delivered)) return NotFound();


            updatingOrder.Status = Status.Canceled;//Update notes
            if (updatingOrder.Notes != orderModel.Notes)
            {
                updatingOrder.Notes = orderModel.Notes;
            }
            updatingOrder.Date = DateTime.Now;
            var result = await _orderRepository.UpdateById(updatingOrder.Id, updatingOrder.ToEntity());

            if (result == true)
                return RedirectToAction("Index");
            else
                return NotFound();

        }

        #endregion
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
