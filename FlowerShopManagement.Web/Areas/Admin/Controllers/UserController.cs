using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FlowerShopManagement.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
[Authorize(Policy = "StaffOnly")]
public class UserController : Controller
{
    private readonly IAuthService _authService;
    private readonly IPersonalService _personalService;
    private readonly IAdminService _adminService;
    private readonly IStaffService _staffService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserController(
        IAuthService authService,
        IAdminService adminService,
        IStaffService staffService,
        IPersonalService personalService,
        IWebHostEnvironment webHostEnvironment)
    {
        ViewBag.User = true;

        _authService = authService;
        _adminService = adminService;
        _staffService = staffService;
        _personalService = personalService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Route("Index")]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        ViewBag.User = true;
        try
        {
            ViewData["Role"] = Enum.GetNames(typeof(Role)).Where(s => s != "Admin" && s != "Passenger").ToList();
            var users = await _staffService.GetUsersAsync() ?? new List<UserModel>();
            int pageSize = 8;
            return View(PaginatedList<UserModel>.CreateAsync(users ?? new List<UserModel>(), 1, pageSize));

        }
        catch
        {
            return NotFound(); // Notify failed to retrieve the list of users for some reasons!
        }
    }

    [Route("Sort")]
    [HttpPost]
    public async Task<IActionResult> Sort(string sortOrder, int? pageNumber, string? currentCategory, string? searchString)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["CurrentCategory"] = currentCategory;

        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

        var users = await _staffService.GetUsersAsync() ?? new List<UserModel>();

        if (users != null)
        {

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Name).ToList();
                    break;
                //case "Date":
                //    productMs = (List<ProductModel>)productMs.OrderBy(s => s.);
                //      break;
                case "date_asc":
                    users = users.OrderBy(s => s.CreatedDate).ToList();
                    break;
                default:
                    //case filter
                    break;
            }

            switch (currentCategory)
            {
                case "Customer":
                    users = users.Where(s => s.Role == Role.Customer).ToList();
                    break;
                case "Staff":
                    users = users.Where(s => s.Role == Role.Staff).ToList();
                    break;

                default:
                    //All
                    break;
            }

            if (searchString != null && searchString != "")
            {
                users = users.Where(i => i.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
                ViewData["CurrentFilter"] = searchString;
            }

            int pageSize = 8;
            PaginatedList<UserModel> objs = PaginatedList<UserModel>.CreateAsync(users, pageNumber ?? 1, pageSize);
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

    [Route("Create")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["Roles"] = Enum.GetValues(typeof(Role))
            .Cast<Role>()
            .Where(i => i.ToString() != "Passenger" && i.ToString() != "Admin" && i.ToString() != "All")
            .ToList();
        ViewData["Genders"] = Enum.GetValues(typeof(Gender))
            .Cast<Gender>()
            .ToList();
        var list = await _adminService.GetAddresses();
        ViewData["Addresses"] = JsonConvert.SerializeObject(list, Formatting.Indented);

        return View(new UserCreateVM());
    }

    [Route("FindDistricts")]
    [HttpPost]
    public async Task<IActionResult> FindDistricts(string city)
    {

        var list = await _adminService.GetAddresses();
        List<string> districts = list.AsParallel().Where(i => i._city == city).GroupBy(i => i._district).Select(i => i.Key).ToList();
        List<string> wards = list.AsParallel().Where(i => i._city == city && i._district == districts.FirstOrDefault()).GroupBy(i => i._commune).Select(i => i.Key).ToList();
        ViewData["Districts"] = districts;
        ViewData["Wards"] = wards;

        return Json(new
        {
            districts = districts,
            wards = wards

        });
    }
    [Route("FindWards")]
    [HttpPost]
    public async Task<List<string>> FindWards(string city, string district)
    {

        var list = await _adminService.GetAddresses();
        List<string> wards = list.Where(i => i._city == city && i._district == district).GroupBy(i => i._commune).Select(i => i.Key).ToList();
        ViewData["Wards"] = wards;

        return wards;
    }

    [Route("Create")]
    [HttpPost]
    public async Task<IActionResult> Create(UserCreateVM userCreateVM)
    {
        // Create
        bool result = false;
        if (userCreateVM.city == null && userCreateVM.district == null
            && userCreateVM.ward == null && userCreateVM.detailAddress == null) return NotFound();
        var infoAddress = new InforDeliveryModel()
        {
            FullName = userCreateVM.userModel.Name,
            IsDefault = true,
            PhoneNumber = userCreateVM.userModel.PhoneNumber,
            Address = userCreateVM.detailAddress,
            Commune = userCreateVM.ward,
            District = userCreateVM.district,
            City = userCreateVM.city
        };
        userCreateVM.userModel.InforDelivery.Add(infoAddress);
        try
        {
            if (userCreateVM.userModel.Role == Role.Customer)
                result = await _adminService.AddCustomerAsync(userCreateVM.userModel);
            else if (userCreateVM.userModel.Role == Role.Staff)
            {
                result = await _adminService.AddStaffAsync(userCreateVM.userModel, Role.Staff);
            }
            if (result == true)
                return RedirectToAction("Index");
            else
                return NotFound();
        }
        catch
        {
            return NotFound();
        }
    }

    [Route("Edit")]
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        // Get user
        try
        {
            var user = await _adminService.GetUserByPhone(id);
            TempData["editUser"] = JsonConvert.SerializeObject(user, Formatting.Indented);
            return View(user);
        }
        catch { return NotFound(); }

    }

    [Route("Edit")]
    [HttpPost]
    public async Task<IActionResult> Edit(UserModel model)
    {
        string s = TempData["editUser"] as string ?? "";
        if (string.IsNullOrEmpty(s)) return NoContent();
        var editUser = JsonConvert.DeserializeObject<UserModel>(s);
        if (editUser == null) return NoContent();
        try
        {
            var result = await _adminService.EditUserAsync(model);
            if (result == false) return NotFound();
            return RedirectToAction("Index"); // return the List of Models or attach it to the view model

        }
        catch
        {
            return NotFound(); // Notify failed to retrieve the list of users for some reasons!
        }
    }

    [Route("Delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(string id, string role)
    {
        try
        {
            var result = await _adminService.RemoveAccountAsync(id, role);
            if (result == true)
                return RedirectToAction("Index");
            return NotFound();
        }
        catch
        {
            return NotFound();
        }
    }

    // ========================= ADMIN ========================= //

    public async Task AddStaffAsync(UserModel newStaffModel)
    {
        try
        {
            await _adminService.AddStaffAsync(newStaffModel, Role.Staff);

            return; // Notify successfully added a new staff
        }
        catch
        {
            return; // Notify failed to add a new staff for some reasons!
        }
    }

    public async Task EditUserRoleAsync(UserModel userModel, Role newRole)
    {
        try
        {
            await _adminService.EditUserRoleAsync(userModel, newRole);

            return; // Notify successfully editted the role of a user!
        }
        catch
        {
            return; // Notify failed to editt the role of a user for some reasons!
        }
    }

    // ========================= STAFF ========================= //

    public async Task ResetUserPassword(UserModel userModel)
    {
        try
        {
            await _staffService.ResetPasswordAsync(userModel);

            return; // Notify successfully reset password!
        }
        catch
        {
            return; // Notify failed to reset the password for some reasons!
        }
    }

    public async Task RemoveUserAccountAsync(UserModel userModel)
    {
        // Should a staff able to remove other staff accounts?
        //if (userModel.Role == Role.Staff)
        //    return;

        try
        {
            await _staffService.RemoveUserAsync(userModel);

            return; // Notify successfully removed the selected user's account 
        }
        catch
        {
            return; // Notify failed to remove the account for some reasons!
        }
    }

    public async Task AddCustomerAsync(UserModel newCustomerModel)
    {
        try
        {
            await _staffService.AddCustomerAsync(newCustomerModel);

            return; // Notify successfully added a new customer
        }
        catch
        {
            return; // Notify failed to add a new customer for some reasons!
        }
    }

    public async Task GetAllUsersAsync()
    {
        try
        {
            var users = new List<UserModel>();

            // Get all users registered (both customers & staffs)
            users = await _staffService.GetUsersAsync();

            return; // return the List of Models or attach it to the view model
        }
        catch
        {
            return; // Notify failed to retrieve the list of users for some reasons!
        }
    }

    // ========================= PERSONAL ========================= //

    public async Task RemovePersonalAccountAsync()
    {
        // ====== Should have password, email or phone number verification here!!!!! ======

        try
        {
            // Dont need to get the entire user
            // Might need if want to do verification in earlier steps
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserId == null || currentUserRole == null)
                return; // Notify the current user not found!

            await _personalService.RemoveAccountAsync(currentUserId, currentUserRole);

            // Notify successfully removed current user's account
            // Might redirect to the homepage without signin
            return;
        }
        catch
        {
            return; // Notify failed to remove the account for some reasons!
        }
    }

    public async Task EditPersonalInfoAsync(UserModel userModel)
    {
        try
        {
            // Change & save the editted info from front-end to database
            await _personalService.EditInfoAsync(userModel);

            return; // Notify succesfully editted current user info
        }
        catch
        {
            return; // Notify failed to edit current user info for some reasons!
        }
    }

    public async Task ResetPersonalPasswordAsync()
    {
        // ====== Should have email or phone number verification here!!!!! ======

        try
        {
            //var currentUser = await GetCurrentUser();

            //await _personalService.ResetPasswordAsync(currentUser);

            return; // Notify successfully reset password!
        }
        catch
        {
            return; // Notify failed to reset the password for some reasons!
        }
    }

    public async Task ChangePersonalPasswordAsync(string oldPassword, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
            return; // Notify the passwords dont match!

        if (oldPassword == newPassword)
            return; // Notify new password is the same as old password!

        try
        {
            var currentId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _authService.GetAuthenticatedUserAsync(currentId);

            // Verify old password
            var encryptedPass = Validator.MD5Hash(oldPassword);
            //if (!currentUser.IsPasswordMatched(encryptedPass))
            //    return; // Old password didnt match! 

            //await _personalService.ChangePasswordAsync(currentUser, newPassword);

            return; // Notify successfully changed password!
        }
        catch
        {
            return; // Notify failed to change the password for some reasons!
        }
    }
}
