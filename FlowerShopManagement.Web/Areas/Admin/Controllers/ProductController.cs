using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
[Route("")]
[Authorize]
public class ProductController : Controller
{
    //Services
    IStockService _stockServices;
    //Repositories
    IProductRepository _productRepository;

    //static list
    List<string> listCategories = new List<string>();
    List<Material> listDetailCategories = new List<Material>();
    List<string> listMaterials = new List<string>();

    public ProductController(IProductRepository productRepository, IStockService stockServices)
    {
        _productRepository = productRepository;
        _stockServices = stockServices;

        //set up for the static list
        var task = Task.Run(async () =>
        {
            listCategories = await _stockServices.GetCategories();
            listDetailCategories = await _stockServices.GetDetailMaterials();
            listMaterials = await _stockServices.GetMaterials();

        });
        task.Wait();

        listCategories.Insert(0, "All");
        listMaterials.Insert(0, "All");
    }

    [Route("Index")]
    [Route("")]
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        //Set up default values for ProductPage

        ViewBag.Product = true;
        ViewData["Categories"] = listCategories.Where(i => i != "Unknown").ToList();
        ViewData["Materials"] = listMaterials.Where(i => i != "Unknown").ToList();

        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);
        int pageSizes = 8;
        ProductVM productVM = new ProductVM() { productModels = PaginatedList<ProductModel>.CreateAsync(productMs, 1, pageSizes), categories = listCategories };
        return View(productVM);

    }

    //Open edit dialog / modal
    [Route("Edit")]
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        //Should get a new one because an admin updates data realtime

        ProductDetailModel editProduct = await _stockServices.GetADetailProduct(id, _productRepository);
        if (editProduct != null)
        {

            ViewData["Categories"] = listCategories.Where(i => i != editProduct.Category && i != "All" && i != "Unknown").ToList();


            return View(editProduct);
        }
        return NotFound();
    }

    [Route("Update")]
    [HttpPost]
    public async Task<IActionResult> Update(ProductDetailModel productModel)
    {
        //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

        //If product get null or Id null ( somehow ) => notfound
        if (productModel == null || productModel.Id == null) return NotFound();

        //Check if the product still exists
        var product = await _productRepository.GetById(productModel.Id); // this may be eleminated
        if (product != null)
        {
            //If product != null => we will update this order by using directly orderModel.Id
            //Check ProductModel for sure if losing some data
            var result = await _productRepository.UpdateById(productModel.Id, productModel.ToEntity());
            if (result != false)
            {
                //Update successfully, we pull new list of products
                List<ProductModel> proMs = await _stockServices.GetUpdatedProducts(_productRepository);

                return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll

            }
        }
        return RedirectToAction("Index");
    }

    [Route("Detele")]
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        //If order get null or Id null ( somehow ) => notfound
        if (id == null) return NotFound();

        //Check productModel for sure if losing some data
        var result = await _productRepository.RemoveById(id);
        if (result == false)
            return NotFound();
        else
        {
            // Update the new way to reload the products in future
            return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll
        }

    }

    //Open an Create Dialog
    [Route("Create")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Categories"] = listCategories.Where(i => i != "Unknown" && i != "All").ToList();
        ViewData["Materials"] = listMaterials.Where(i => i != "Unknown" && i != "All").ToList();

        return View(new ProductDetailModel());
    }

    // Confirm and create an Order
    [Route("Create")]
    [HttpPost]
    public async Task<IActionResult> Create(ProductDetailModel productModel)
    {
        var maintainment = listDetailCategories.FirstOrDefault(i => i._name == productModel.Category);
        if (maintainment == null)
            productModel.Maintainment = "blank";
        else
            productModel.Maintainment = maintainment._maintainment;

        var result = await _stockServices.CreateProduct(productModel, _productRepository);
        if (result == true)
        {
            return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll
        }
        return NotFound(); // Can be changed to Redirect
    }

    [Route("Sort")]
    [HttpPost]
    public async Task<IActionResult> Sort(string sortOrder, string currentFilter, string searchString,
        int? pageNumber, string? currentPrice, string? currentCategory)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["CurrentPrice"] = currentPrice;
        ViewData["CurrentCategory"] = currentCategory;
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        ViewData["CurrentFilter"] = searchString;
        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);
        if (productMs != null)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                productMs = productMs.Where(s => s.Name.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    productMs = productMs.OrderByDescending(s => s.Name).ToList();
                    break;
                //case "Date":
                //    productMs = (List<ProductModel>)productMs.OrderBy(s => s.);
                //      break;
                case "name_asc":
                    productMs = productMs.OrderBy(s => s.Name).ToList();
                    break;
                default:
                    //case filter

                    break;
            }
            switch (currentPrice)
            {
                case "0 - 10":
                    productMs = productMs.Where(s => s.UniPrice > 0 && s.UniPrice <= 10).ToList();
                    break;
                case "11 - 50":
                    productMs = productMs.Where(s => s.UniPrice > 10 && s.UniPrice <= 50).ToList();
                    break;
                case ">50":
                    productMs = productMs.Where(s => s.UniPrice > 50).ToList();
                    break;
                default:

                    break;
            }

            if (currentCategory != null && currentCategory != "All")
            {
                productMs = productMs.Where(s => s.Category.Equals(currentCategory)).ToList();
            }
            int pageSize = 8;
            PaginatedList<ProductModel> objs = PaginatedList<ProductModel>.CreateAsync(productMs, pageNumber ?? 1, pageSize);
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

}
