using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerShopManagement.Web.Controllers;

[Authorize(Policy = "CustomerOnly")]
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

        List<ProductDetailModel> productMs;

        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Unauthenticated user
        if (userId is null) return NotFound();

        // get user
        UserModel? user = await _authServices.GetAuthenticatedUserAsync(userId);

        // get product list like from this user
        if (user != null && user.FavProductIds != null && user.FavProductIds.Count > 0)
        {
            productMs = new List<ProductDetailModel>();

            var temp = await _stockServices.GetUpdatedDetailProducts();

            foreach (var item in temp)
            {
                if (user.FavProductIds.Where(i => i == item.Id).Count() > 0)
                {
                    item.IsLike = true;
                    productMs.Add(item);
                }
            }

            productMs = productMs.OrderBy(i => i.Name).ToList();

            return View(productMs);
        }

        productMs = new List<ProductDetailModel>();

        return View(productMs);
    }
}
