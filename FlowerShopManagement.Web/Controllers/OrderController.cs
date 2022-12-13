﻿using FlowerShopManagement.Application.Interfaces;
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
        IImportService _importServices;
        //Repositories
        IOrderRepository _orderRepository;
        IProductRepository _productRepository;
        IUserRepository _userRepository;

        public OrderController(ISaleService saleServices, IOrderRepository orderRepository, IProductRepository productRepository, IImportService importServices,
            IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _saleServices = saleServices;
            _productRepository = productRepository;
            _importServices = importServices;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Order = true;
            //Set up default values for OrderPage

            ViewData["Categories"] = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            List<OrderModel> orderMs = await GetUpdatedOrders();
            return View(/*Coult be a ViewModel in here*/);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id)
        {
            //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
            //Should get a new one because an admin updates data realtime
            var order = await _orderRepository.GetById(id);
            if (order != null)
            {
                return PartialView(/*Coult be a ViewModel in here*/);
            }
            return RedirectToAction("Index");
        }

        [HttpPut]
        public async Task<IActionResult> Update(OrderModel orderModel)
        {
            //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

            //If order get null or Id null ( somehow ) => notfound
            if (orderModel == null || orderModel.Id == null) return NotFound();
            //Check if the order still exists
            var order = await _orderRepository.GetById(orderModel.Id);
            if (order != null)
            {
                //If order != null => we will update this order by using directly orderModel.Id
                //Check orderModel for sure if losing some data
                var result = await _orderRepository.UpdateById(orderModel.Id, orderModel.ToEntity());
                if (result != false)
                {
                    //Update successfully, we pull new list of orders
                    List<OrderModel> orderMs = await GetUpdatedOrders();

                    return PartialView(/*Coult be a UPDATED ViewModel in here*/);//for example: a _ViewAll 

                }
            }
            return RedirectToAction("Index");
        }

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
                    List<OrderModel> orderMs = await GetUpdatedOrders();

                    return PartialView(/*Coult be a UPDATED ViewModel in here*/); //For example: a _ViewAll
                }
            }
            return NotFound();
        }

        //Open an CreatePage
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Set up default values for OrderPage

            ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

            List<ProductModel> productMs = await GetUpdatedProducts();
            List<UserModel> customerMs = await GetUpdatedCustomers();
            /*Set up viewmodel
			 


			*/
            return View(/*Coult be a ViewModel in here*/);
        }

        // Confirm and create an Order
        [HttpPost]
        public async Task<IActionResult> Create(OrderModel orderModel, UserModel userModel)
        {
            var result = _saleServices.CreateOfflineOrder(orderModel, userModel, _orderRepository, _userRepository, _productRepository);
            if (result != null)
            {
                List<OrderModel> orders = await GetUpdatedOrders();
                return PartialView(/*Coult be a ViewModel in here*/); // A updated _ViewAll table
            }
            return NotFound(); // Can be changed to Redirect
        }

        [HttpPost]
        public async Task<IActionResult> PickItemDialog(string filter = "") // Also handle if there is a filter / search
        {
            List<ProductModel> productMs = await GetUpdatedProducts();

            return PartialView(/*Coult be a ViewModel in here*/); // This will be an view for dialog / modal
        }

        [HttpPost]
        public IActionResult PickedAnItem(List<string> ids, List<int> amounts) // These 2 list should have the same size
        {
            OrderModel orderModel = new OrderModel();
            if (ids != null && amounts != null)
            {
                for (int i = 0; i < amounts.Count && i < ids.Count; i++)
                {
                    if (amounts[i] != 0 && ids[i] != "")
                    {
                        ProductModel product = new ProductModel(ids[i], amounts[i]);

                        orderModel.Products.Add(product);
                    }
                }

            }
            //CurrentOrder will be ready for the next request
            TempData["CurrentOrder"] = orderModel;

            return PartialView(/*Coult be a ViewModel in here*/); // This will be an update view for current items table
        }

        [HttpPost]
        public IActionResult DeteleItem(string id)
        {
            OrderModel? orderModel = null;
            if (TempData["CurrentOrder"] != null)
                 orderModel = TempData["CurrentOrder"] as OrderModel;
            if (orderModel != null)
            {
                foreach (var i in orderModel.Products)
                    if (i.IsEqualProduct(id) )
                        orderModel.Products.Remove(i);
            }
            //CurrentOrder will be ready for the next request
            TempData["CurrentOrder"] = orderModel;

            //Should update Viewmodel for newest updates
            return PartialView(/*Coult be a ViewModel in here*/);// This will be an update view for current order table
        }

        [HttpPost]
        public IActionResult PickCustomer(string id)
        {
            OrderModel? orderModel = null;

            var user = _userRepository.GetById(id);
            if (user != null)
            {
                TempData["CurrentUser"] = user;
            }
            return PartialView(/*Coult be a ViewModel in here*/); // This will be an update view for current customer table
        }

        public async Task<List<OrderModel>> GetUpdatedOrders()
        {
            List<Order> orders = await _orderRepository.GetAll();
            List<OrderModel> orderMs = new List<OrderModel>();

            foreach (var o in orders)
            {
                orderMs.Add(new OrderModel(o));
            }
            return orderMs;
        }

        public async Task<List<ProductModel>> GetUpdatedProducts()
        {
            List<Product> products = await _productRepository.GetAll();
            List<ProductModel> productMs = new List<ProductModel>();

            foreach (var o in products)
            {
                productMs.Add(new ProductModel(o));
            }
            return productMs;
        }

        public async Task<List<UserModel>> GetUpdatedCustomers()
        {
            List<User> users = await _userRepository.GetAll();
            users = users.Where(u => u.role.Equals(Role.Customer)).ToList();
            List<UserModel> customers = new List<UserModel>();

            foreach (var o in users)
            {
                customers.Add(new UserModel(o));
            }
            return customers;
        }
    }
}
