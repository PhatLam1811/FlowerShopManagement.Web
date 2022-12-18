using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IPersonalService _personalService;
        private readonly IAdminService _adminService;
        private readonly IStaffService _staffService;
        public SupplierController(
        IAuthService authService,
        IAdminService adminService,
        IStaffService staffService,
        IPersonalService personalService)
        {
            _authService = authService;
            _adminService = adminService;
            _staffService = staffService;
            _personalService = personalService;
        }
        public IActionResult Index()
        {
            ViewBag.Supplier = true;
            return View(new List<SupplierDetailModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new SupplierDetailModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(SupplierDetailModel model)
        {
            // Create
            bool result = false;
            try
            {
                result = await _adminService.AddSupplierAsync(model);
                if(result)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    return NotFound();
                }

            }
            catch { return NotFound(); }
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(new SupplierDetailModel());
        }
    }
}
