﻿using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Controllers
{
	public class WishListController : Controller
	{
        private readonly IAuthService _authServices;
        private readonly IStockService _stockServices;
        private readonly IProductRepository _productRepository;

        public WishListController(IAuthService authServices, IStockService stockService, IProductRepository productRepository)
        {
            _authServices = authServices;
            _stockServices = stockService;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
		{
            ViewBag.WishList = true;

            ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

            List<ProductModel> productMs;

            // get user
            UserDetailsModel? user = await _authServices.GetUserAsync(this.HttpContext);

            // get product list like from this user
            if (user != null && user.FavProductIds != null && user.FavProductIds.Count > 0)
            {
                productMs = new List<ProductModel>();

                var temp = await _stockServices.GetUpdatedProducts(_productRepository);

                foreach (var item in temp)
                {
                    if (user.FavProductIds.Where(i => i == item.Id).Count() > 0)
                    {
                        item.IsLike = true;
                        productMs.Add(item);
                    }
                }

                productMs = productMs.OrderBy(i => i.Name).ToList();
            }

            productMs = new List<ProductModel>();
            
            return View(productMs);
		}
	}
}
