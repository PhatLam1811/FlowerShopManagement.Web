using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
[Route("Admin")]
[Authorize(Policy = "StaffOnly")]
public class ProductController : Controller
{
    //Services
    IStockService _stockServices;
    //Repositories
    IProductRepository _productRepository;

    //static list
    IWebHostEnvironment _webHostEnvironment;
    static List<string> listCategories = new List<string>();
    static List<Material> listDetailMaterial = new List<Material>();
    static List<string> listMaterials = new List<string>();

    public ProductController(IProductRepository productRepository, IStockService stockServices, IWebHostEnvironment webHostEnvironment)
    {
        ViewBag.Product = true;

        _productRepository = productRepository;
        _stockServices = stockServices;

        //set up for the static list
        if (listMaterials.Count <= 0 && listDetailMaterial.Count <= 0 && listCategories.Count <= 0)
        {
            var task = Task.Run(async () =>
                    {
                        listCategories = await _stockServices.GetCategories();
                        listDetailMaterial = await _stockServices.GetDetailMaterials();
                        listMaterials = await _stockServices.GetMaterials();

                    });
            task.Wait();
            listCategories.Insert(0, "All");
            listMaterials.Insert(0, "All");
        }
        _webHostEnvironment = webHostEnvironment;
    }

    [Route("Index")]
    [HttpGet]
    //[Authorize]
    public async Task<IActionResult> Index()
    {
        //Set up default values for ProductPage

        ViewData["Categories"] = listCategories.Where(i => i != "Unknown").ToList();
        ViewData["Materials"] = listMaterials.Where(i => i != "Unknown").ToList();

        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts();
        int pageSizes = 8;
        ProductVM productVM = new ProductVM()
        {
            productModels = PaginatedList<ProductModel>
            .CreateAsync(productMs.OrderBy(i => i.Name).ToList(), 1, pageSizes),
            categories = listCategories
        };
        return View(productVM);

    }

    [Route("Sort")]
    [HttpPost]
    public async Task<IActionResult> Sort(string sortOrder, string currentFilter, string searchString,
        int? pageNumber, string? currentPrice, string currentMaterial, string? currentCategory)
    {
        int pageSize = 8;

        ViewData["CurrentSort"] = sortOrder;
        ViewData["CurrentPrice"] = currentPrice;
        ViewData["CurrentCategory"] = currentCategory;
        ViewData["CurrentMaterial"] = currentMaterial;
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
        List<ProductModel> productMs = await _stockServices.GetUpdatedProducts();
        if (productMs != null)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                productMs = productMs.Where(s => s.Name.Contains(searchString)).ToList();
            }
            // sort order - feature incoming
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
                case "0$ -> 10$":
                    productMs = productMs.Where(s => s.UniPrice > 0 && s.UniPrice <= 10).ToList();
                    break;
                case "11$ -> 50$":
                    productMs = productMs.Where(s => s.UniPrice > 10 && s.UniPrice <= 50).ToList();
                    break;
                case "> 50$":
                    productMs = productMs.Where(s => s.UniPrice > 50).ToList();
                    break;
                default:

                    break;
            }

            if (currentMaterial != null && currentMaterial != "All")
            {
                productMs = productMs.Where(s => s.Material.Equals(currentMaterial)).ToList();
            }
            if (currentCategory != null && currentCategory != "All")
            {
                productMs = productMs.Where(s => s.Category.Equals(currentCategory)).ToList();
            }
            PaginatedList<ProductModel> objs = PaginatedList<ProductModel>
                .CreateAsync(productMs.OrderBy(i => i.Name).ToList(), pageNumber ?? 1, pageSize);
            return Json(new
            {
                isValid = true,
                htmlViewAll = Helper.RenderRazorViewToString(this, "_ViewAll", objs),
                htmlPagination = Helper.RenderRazorViewToString(this, "_Pagination", objs)

            });
        }
        return NotFound();

    }

    //Open edit dialog / modal
    [Route("Edit")]
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        //Should get a new one because an admin updates data realtime

        ProductDetailModel editProduct = await _stockServices.GetADetailProduct(id);
        if (editProduct != null)
        {

            ViewData["Categories"] = listCategories.Where(i => i != editProduct.Category && i != "All" && i != "Unknown").ToList();
            ViewData["Materials"] = listMaterials.Where(i => i != editProduct.Category && i != "All" && i != "Unknown").ToList();

            return View(editProduct);
        }
        return NotFound();
    }

    [Route("Update")]
    [HttpPost]
    public async Task<IActionResult> Update(ProductDetailModel productModel)
    {

        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = listCategories.Where(i => i != productModel.Category && i != "All" && i != "Unknown").ToList();
            ViewData["Materials"] = listMaterials.Where(i => i != productModel.Category && i != "All" && i != "Unknown").ToList();
            return View("Edit", productModel);
        }

        //If product get null or Id null ( somehow ) => notfound
        if (productModel == null || productModel.Id == null) return NotFound();




        if (await _stockServices.UpdateProduct(productModel) == true)
            return RedirectToAction("Index");
        return NotFound();
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
        if (!ModelState.IsValid) { return Create(); }
        var maintainment = listDetailMaterial.FirstOrDefault(i => i._name == productModel.Material);
        if (maintainment == null)
            productModel.Maintainment = "blank";
        else
            productModel.Maintainment = maintainment._maintainment;

        var result = await _stockServices.CreateProduct(productModel);
        if (result == true)
        {
            return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll
        }
        return NotFound(); // Can be changed to Redirect
    }

    [HttpPost("ChoosePictures")]
    public IActionResult ChoosePictures()
    {
        return PartialView("_ChoosePictures", new List<string>());
    }
    [HttpPost]
    [Route("ReChosePictures")]

    public IActionResult ReChosePictures(List<string> Pictures, string eliminate)
    {
        Pictures.Remove(eliminate); 
        return PartialView("_ChoosePictures", Pictures);
    }

    [HttpPost]
    [Route("ChosePictures")]
    public async Task<IActionResult> ChosePictures(List<IFormFile> formfiles, List<string>? pictures)
    {
        var strings = new List<string>();
        if (formfiles != null && formfiles.Count > 0)
        {
            foreach (var picture in formfiles)
            {
                if (picture != null && picture.Length > 0)
                {
                    string fileName = picture.FileName;
                    string path = Path.Combine(_webHostEnvironment.WebRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await picture.CopyToAsync(fileStream);
                        
                    }
                    strings.Add(fileName);
                }

            }

        }
        if(pictures == null) return PartialView("_ChoosePictures", strings);
        foreach (string i in strings)
        {
            if (!pictures.Contains(i))
            {
                pictures.Add(i);
            }
        }
        //Set up default values for ProductPage
        return PartialView("_ChoosePictures", pictures);

    }

    [Route("Category")]
    [HttpGet]
    public IActionResult Category()
    {
        var list = listCategories.Where(i => i != "Unknown" && i != "All").ToList();
        ViewData["Categories"] = list;

        return View(list);
    }

    [Route("AddCategory")]
    [HttpPost]
    public async Task<IActionResult> AddCategory(/*[Required][MinLength(3, ErrorMessage = "Password must be greater than 6 characters")]*/string name)
    {
        var list = listCategories.Where(i => i != "Unknown" && i != "All").ToList();
        if (!ModelState.IsValid) { return Category(); }
        if (list.Any(i => i == name) == true) return BadRequest();
        var result = _stockServices.CreateCategory(name);
        listCategories = await _stockServices.GetCategories();
        return RedirectToAction("Category");
    }
}
