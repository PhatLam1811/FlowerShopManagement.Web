using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValueType = FlowerShopManagement.Core.Enums.ValueType;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
[Authorize(Policy = "StaffOnly")]
public class VoucherController : Controller
{
    //Services
    IStockService _stockServices;
    //Repositories
    IVoucherRepository _voucherRepository;

    static List<string> listCategories = new List<string>();
    static List<string> listStatus = new List<string>();
    static List<string> listValueTypes = new List<string>();

    public VoucherController(IVoucherRepository voucherRepository, IStockService stockServices)
    {
        _stockServices = stockServices;
        _voucherRepository = voucherRepository;

        if (listStatus.Count <= 0 && listValueTypes.Count <= 0 && listCategories.Count <= 0)
        {

            listCategories = Enum.GetNames(typeof(VoucherCategories)).ToList();
            listValueTypes = Enum.GetNames(typeof(ValueType)).ToList();
            listStatus = Enum.GetNames(typeof(VoucherStatus)).ToList();

            listCategories.Insert(0, "All");
            listStatus.Insert(0, "All");
            listValueTypes.Insert(0, "All");
        }
    }

    [Route("Index")]
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        //Set up default values for ProductPage

        ViewData["VoucherCategories"] = listCategories;
        ViewData["ValueType"] = listValueTypes;
        ViewData["VoucherStatus"] = listStatus;
        List<VoucherDetailModel> productMs = await _stockServices.GetUpdatedVouchers();

        productMs = productMs.OrderBy(i => i.ExpiredDate).ToList();
        int pageSize = 8;
        
        return View(PaginatedList<VoucherDetailModel>.CreateAsync(productMs,1,pageSize));
    }

    [Route("Sort")]
    [HttpPost]
    public async Task<IActionResult> Sort(string sortOrder, int? pageNumber, string? currentCategory, 
        string? currentStatus, string? currentValueType, string? searchString)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["CurrentCategory"] = currentCategory;

        var vouchers = await _stockServices.GetUpdatedVouchers() ?? new List<VoucherDetailModel>();

        if (vouchers != null)
        {

            switch (sortOrder)
            {
                case "name_desc":
                    vouchers = vouchers.OrderByDescending(s => s.Code).ToList();
                    break;
                //case "Date":
                //    productMs = (List<ProductModel>)productMs.OrderBy(s => s.);
                //      break;
                case "date_asc":
                    vouchers = vouchers.OrderBy(s => s.CreatedDate).ToList();
                    break;
                default:
                    //case filter
                    break;
            }
            if (currentCategory != null && currentCategory != "" && currentCategory != "All")
            {
                vouchers = vouchers.Where(s => s.Categories.ToString() == currentCategory).ToList();
            }
            if (currentValueType != null && currentValueType != "" && currentValueType != "All")
            {
                vouchers = vouchers.Where(s => s.ValueType.ToString() == currentValueType).ToList();
            }

            if (currentStatus != null && currentStatus != "" && currentStatus != "All")
            {
                var str = string.Join("", currentStatus.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

                vouchers = vouchers.Where(s => s.State.ToString() == str).ToList();
            }

            if (searchString != null && searchString != "")
            {
                vouchers = vouchers.Where(i => i.Code.ToUpper().Contains(searchString.ToUpper())).ToList();
                ViewData["CurrentFilter"] = searchString;
            }

            int pageSize = 8;
            PaginatedList<VoucherDetailModel> objs = PaginatedList<VoucherDetailModel>.CreateAsync(vouchers, pageNumber ?? 1, pageSize);
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

    //Open edit dialog / modal
    [HttpGet]
    [Route("Edit")]

    public async Task<IActionResult> Edit(string id)
    {
        ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
        //Should get a new one because an admin updates data realtime
        VoucherDetailModel editProduct = await _stockServices.GetADetailVoucher(id);
        if (editProduct != null)
        {
            return View(editProduct);
        }
        return RedirectToAction("Index");
    }

    [Route("Update")]
    [HttpPost]
    public async Task<IActionResult> Update(VoucherDetailModel voucherM)
    {
        //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

        //If product get null or Id null ( somehow ) => notfound
        if (voucherM == null || voucherM.Code == null) return NotFound();

        //Check if the product still exists
        var voucher = await _voucherRepository.GetById(voucherM.Code); // this may be eleminated
        if (voucher != null)
        {
            //If product != null => we will update this order by using directly orderModel.Id
            //Check ProductModel for sure if losing some data

            var result = await _voucherRepository.UpdateById(voucherM.Code, voucherM.ToEntity());
            if (result != false)
            {
                //Update successfully, we pull new list of products
                return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll

            }
        }
        return RedirectToAction("Index");
    }

    [Route("Delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(string Code = "")
    {
        //ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();

        //If order get null or Id null ( somehow ) => notfound
        if (Code == "") return NotFound();
        //Check if the order still exists

        var result = await _voucherRepository.RemoveById(Code);
        if (result == false)
            return RedirectToAction($"Unable to remove {Code}");
        else
        {
            //Detele successfully, we pull a new list of orders


            return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll
        }

    }
    [Route("Activate")]
    [HttpPost]
    public async Task<IActionResult> Activate(string Code = "")
    {
        if (Code == "") return NotFound();

        var result = await _stockServices.ActivateVoucher(Code);
        if (result == false)
            return RedirectToAction($"Unable to activate {Code}");
        else
        {
            return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll
        }

    }

    //Open an Create Dialog
    [HttpGet]
    [Route("Create")]

    public IActionResult Create()
    {
        //Set up default values for OrderPage

        ViewData["Categories"] = Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList();
        return View(new VoucherDetailModel());
    }

    // Confirm and create an Order
    [HttpPost]
    [Route("Create")]

    public async Task<IActionResult> Create(VoucherDetailModel voucherDetailModel)
    {
        var result = await _stockServices.CreateVoucher(voucherDetailModel, _voucherRepository);
        if (result == true)
        {
            List<VoucherDetailModel> orders = await _stockServices.GetUpdatedVouchers();
            return RedirectToAction("Index"/*Coult be a ViewModel in here*/); // A updated _ViewAll
        }
        return NotFound(); // Can be changed to Redirect
    }
}
