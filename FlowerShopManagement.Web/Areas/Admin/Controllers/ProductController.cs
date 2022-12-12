using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //Services
        IStockServices _stockServices;
        //Repositories
        IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository, IStockServices stockServices)
        {
            _productRepository = productRepository;
            _stockServices = stockServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Set up default values for ProductPage

            ViewBag.Product = true;

            ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
            List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);
            int pageSizes = 6;
            return View( PaginatedList<ProductModel>.CreateAsync(productMs, 1, 2));
            
        }

        //Open edit dialog / modal
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
            //Should get a new one because an admin updates data realtime
            ProductDetailModel editProduct = new ProductDetailModel(await _productRepository.GetById(id));
            if (editProduct != null)
            {
                return View(editProduct);
            }
            return RedirectToAction("Index");
        }

        [HttpPut]
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

        [HttpDelete]
        public async Task<IActionResult> Detele(ProductDetailModel productModel)
        {
            //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

            //If order get null or Id null ( somehow ) => notfound
            if (productModel == null || productModel.Id == null) return NotFound();
            //Check if the order still exists
            var product = await _productRepository.GetById(productModel.Id);
            if (product != null)
            {
                //If order != null => we will detele this order by using directly ProductModel.Id
                //Check productModel for sure if losing some data
                var result = await _productRepository.RemoveById(productModel.Id);
                if (result == false)
                    return RedirectToAction($"Unable to remove {productModel.Id}");
                else
                {
                    //Detele successfully, we pull a new list of orders
                    List<ProductModel> productMs = await _stockServices.GetUpdatedProducts(_productRepository);

                    return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll
                }
            }
            return NotFound();
        }

        //Open an Create Dialog
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Set up default values for OrderPage

            ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

            /*Set up viewmodel
			 


			*/
            return View(); 
        }

        // Confirm and create an Order
        [HttpPost]
        public async Task<IActionResult> Create(ProductDetailModel productModel)
        {
            var result = await _stockServices.CreateProduct(productModel, _productRepository);
            if (result == true)
            {
                List<ProductModel> orders = await _stockServices.GetUpdatedProducts(_productRepository);
                return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll
            }
            return NotFound(); // Can be changed to Redirect
        }

        public async Task<IActionResult> Sort(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
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
                    productMs = (List<ProductModel>)productMs.Where(s => s.Name.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        productMs = (List<ProductModel>)productMs.OrderByDescending(s => s.Name);
                        break;
                    //case "Date":
                    //    productMs = (List<ProductModel>)productMs.OrderBy(s => s.);
                    //      break;
                    case "name_asc":
                        productMs = (List<ProductModel>)productMs.OrderBy(s => s.Name);
                        break;
                    default:
                        //productMs = productMs.OrderBy(s => s.LastName);
                        break;
                }
                int pageSize = 6;
                PaginatedList<ProductModel> objs = PaginatedList<ProductModel>.CreateAsync(productMs, pageNumber ?? 1, 2);
                return Json(new { isValid = true, 
                    htmlViewAll = Helper.RenderRazorViewToString(this, "_ViewAll", objs) ,
                    htmlPagination = Helper.RenderRazorViewToString(this, "_Pagination", objs)

                });
                //return PartialView("_ViewAll",PaginatedList<ProductModel>.CreateAsync(productMs, pageNumber ?? 1, pageSize));
            }
            return NotFound();  

        }

    }
}
