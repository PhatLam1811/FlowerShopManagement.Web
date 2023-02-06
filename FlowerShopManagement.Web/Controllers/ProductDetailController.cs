using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Web.ViewModels;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace FlowerShopManagement.Web.Controllers;

public class ProductDetailController : Controller
{
	readonly IProductRepository _productRepository;

	public ProductDetailController(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public IActionResult Index()
	{
		return View();
	}

	[HttpGet]
	public async Task<IActionResult> ProductDetail(string? id)
	{
		if (id != null)
		{
			Product? product = await _productRepository.GetById(id);
			ProductDetailModel? productM;
			if (product != null)
			{
				productM = new ProductDetailModel(product);
				TempData["current"] = JsonConvert.SerializeObject(productM, Formatting.Indented);

				return View(productM);
			}
		}
		return NotFound();
	}

	[HttpGet]
	public IActionResult SeeMore()
	{
		string? s = TempData["current"] as string;
		if (s == null) { return NotFound(); }
		var product = JsonConvert.DeserializeObject<ProductDetailModel>(s);

		return PartialView("_SeeMorePartialView", product);
	}
}
