using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerShopManagement.Web.Controllers
{
    public class CartController : Controller
    {
        ICartRepository _cartRepository;
        IProductRepository _productRepository;
        ICustomerfService _customerService;

        public CartController(ICartRepository cartRepository, ICustomerfService customerfService, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _customerService = customerfService;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            string? userId;
            CartModel? cartM = new CartModel();

            if (this.HttpContext != null)
            {
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    cartM = await _customerService.GetCartOfUserAsync(userId);

                    if(cartM != null && cartM.Items != null && cartM.Items.Count > 0)
                    {
                        foreach (var item in cartM.Items)
                        {
                            var product = await _productRepository.GetById(item._productId);
                            if (product != null)
                            {
                                item.items = new ProductDetailModel(product);
                            }
                            else return NotFound();
                        }
                    }
                }
            }

            return View((cartM == null) ? new CartModel() : cartM);
        }

        [HttpPost]
        public async Task<IActionResult> AddtoCart(string id)
        {
            string? userId;

            if (this.HttpContext != null)
            {
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var result = await _customerService.AddItemToCart(userId, id, 1);
                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOutOfCart(string id)
        {
            string? userId;

            if (this.HttpContext != null)
            {
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var result = await _customerService.RemoveItemToCart(userId, id);
                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAmount(string id, int amount)
        {
            string? userId;

            if (this.HttpContext != null)
            {
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var result = await _customerService.UpdateAmountOfItem(userId, id, amount);
                    if (result)
                    {
                        return RedirectToAction("Index", "Cart");
                    }
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSelection(string id, bool isSelected)
        {
            string? userId;

            if (this.HttpContext != null)
            {
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var result = await _customerService.UpdateSelection(userId, id, isSelected);
                    if (result)
                    {
                        return RedirectToAction("Index", "Cart");
                    }
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> LoadViewTotal()
        {
            string? userId;

            if (this.HttpContext != null)
            {
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    //var result = await _customerService.UpdateSelection(userId, id, isSelected);
                    //if (result)
                    //{
                    //    return RedirectToAction("Index", "Cart");
                    //}
                    var cart = await _customerService.GetCartOfUserAsync(userId);
                    var total = 0;
                    foreach(var item in cart.Items)
                    {
                        total += item.amount * item.items.UniPrice;
                    }
                    cart.Total = total;
                    return PartialView("_ViewTotal", cart);
                }
            }

            return NotFound();
        }
    }
}
