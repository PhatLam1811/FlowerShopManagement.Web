using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Interfaces.MongoDB;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ValueType = FlowerShopManagement.Core.Enums.ValueType;
using FlowerShopManagement.Core.Entities;
using System.Web.WebPages;
using Microsoft.AspNetCore.Authorization;
using FlowerShopManagement.Application.Services;

namespace FlowerShopManagement.Web.Controllers;

//--------------------------------------Customer Order Controller--------------------------------------------------

[Authorize(Policy = "CustomerOnly")]
public class OrderController : Controller
{
	//Services
	ISaleService _saleServices;
	IReportService _reportServices;
	//Repositories
	IOrderRepository _orderRepository;
	IStockService _stockService;
	IProductRepository _productRepository;
	IUserRepository _userRepository;
	ICustomerfService _customerService;
	IAuthService _authService;


	public OrderController(ISaleService saleServices, IOrderRepository orderRepository, IProductRepository productRepository,
		IUserRepository userRepository, ICustomerfService customerfService, IAuthService authService, IStockService stockService, IReportService reportServices)
	{
		_orderRepository = orderRepository;
		_saleServices = saleServices;
		_productRepository = productRepository;
		_userRepository = userRepository;
		_customerService = customerfService;
		_authService = authService;
		_stockService = stockService;
		_reportServices = reportServices;
	}

	[HttpGet]
	public async Task<IActionResult> Index(string filter = "")
	{
		ViewBag.Order = true;

		//Set up default values for OrderPage

		//ViewData["Categories"] = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

		string? userId;
		List<OrderDetailModel>? orderMs = new List<OrderDetailModel>();

		if (this.HttpContext != null)
		{
			userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{
				orderMs = await _customerService.GetOrdersOfUserAsync(userId, _orderRepository);
				if (orderMs != null)
				{
					if (filter != "" && filter != "All") orderMs = orderMs.Where(i => i.Status.ToString().Equals(filter)).ToList();
					orderMs = orderMs
						.AsParallel().OrderByDescending(i => i.Date)
						.ThenByDescending(i => i.Status == Status.Delivering)
						.ThenByDescending(i => i.Status == Status.Waiting).ToList();

					// Report for customer
					var totalAmount = 0;
					var success = 0;
					var waiting = 0;
					double totalPrice = 0;

					foreach(var i in orderMs)
					{
						totalAmount++;
						totalPrice += i.Total;

						if (i.Status == Status.Delivered)
						{
							success++;
						}

						if (i.Status == Status.Waiting)
						{
							waiting++;
						}
					}

					ViewData["TotalAmount"] = totalAmount;
					ViewData["TotalPrice"] = totalPrice;
					ViewData["Waiting"] = waiting;
					ViewData["Success"] = success;
                }
			}
		}

		return View(orderMs);
	}

	[HttpGet]
	public async Task<IActionResult> OrderDetail(string id)
	{
		if (id == null)
		{
			return NotFound();
		}
		else
		{
			string? userId = "";
			List<OrderDetailModel>? orderMs = new List<OrderDetailModel>();

			if (this.HttpContext != null)
			{
				userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (userId != null)
				{
					orderMs = await _customerService.GetOrdersOfUserAsync(userId, _orderRepository);
				}
				else
				{
					return NotFound("Not Found! This order can't show!");
				}
			}

			if (orderMs?.Count > 0)
			{
				OrderDetailModel orderM = orderMs.Where(o => o.Id == id).First();
				return View(orderM);
			}
		}

		return NotFound();
	}

	[HttpGet]
	public async Task<IActionResult> Create()
	{
		// get cart of current user
		string? userId;

		if (this.HttpContext != null)
		{
			userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{
				List<CartItemModel>? cartItems = _customerService.GetCartOfUserAsync(userId).Result?.Items;
				var orderM = new OrderDetailModel();

				// update order items
				if (cartItems?.Count > 0)
				{
					orderM.Products = new List<ProductDetailModel>();
					foreach (var i in cartItems)
					{
						if (i.isSelected)
						{
							ProductDetailModel item = i.items;
							item.Amount = i.amount;
							orderM.Products.Add(item);
							orderM.Amount += i.amount;
							orderM.Total += i.items.UniPrice * i.amount;
						}
					}
				}

				// update user's information
				// find delivery information of user, if default info is different from initial info
				// get info delivery
				// check which info is default
				// attach to orderM
				UserModel? user = await _authService.GetAuthenticatedUserAsync(userId);
				if (user != null)
				{
					if (user.InforDelivery != null && user.InforDelivery.Count > 0)
					{
						var info = user.InforDelivery.Where(o => o.IsDefault = true).First();
						orderM.FullName = info.FullName;
						orderM.PhoneNumber = info.PhoneNumber;
						orderM.Address = info.Address + ", " + info.Commune + ", " + info.District+ ", " + info.City;
					}
					else
					{
						orderM.FullName = user.Name;
						orderM.PhoneNumber = user.PhoneNumber;
						orderM.Address = "";
					}
				}

				return View(orderM);
			}
		}
		return NotFound();
	}

	[HttpGet("Order/BuyNow/{id?}/{amount}")]
	public async Task<IActionResult> BuyNow(string id, int amount)
	{
		// get cart of current user
		string? userId;

		if (this.HttpContext != null)
		{
			userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{
				var orderM = new OrderDetailModel();
				// update order products
				var product = await _productRepository.GetById(id);
				if (product != null)
				{
					var productM = new ProductDetailModel(product);
					productM.Amount = amount;

					orderM.Products.Add(productM);
					orderM.Amount = amount;
					orderM.Total = amount * productM.UniPrice;
				}

				// update user's information
				// find delivery information of user, if default info is different from initial info
				// get info delivery
				// check which info is default
				// attach to orderM
				UserModel? user = await _authService.GetAuthenticatedUserAsync(userId);
				if (user != null)
				{
					if (user.InforDelivery != null && user.InforDelivery.Count > 0)
					{
						var info = user.InforDelivery.Where(o => o.IsDefault = true).First();
						orderM.FullName = info.FullName;
						orderM.PhoneNumber = info.PhoneNumber;
						orderM.Address = info.Address;
					}
					else
					{
						orderM.FullName = user.Name;
						orderM.PhoneNumber = user.PhoneNumber;
						orderM.Address = "";
					}
				}

				return View(orderM);
			}
		}
		return NotFound();
	}

	[HttpPost]
	public async Task<IActionResult> Create(OrderDetailModel orderM, string discounte)
	{
		discounte = discounte.Remove(0, 1);
		if (!discounte.IsEmpty())
		{
			orderM.Discount = Double.Parse(discounte);
		}

		// check if model is valid?
		if (ModelState.IsValid)
		{
			string? userId;

			if (this.HttpContext != null)
			{
				userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (userId != null)
				{
					List<CartItemModel>? cartItems = _customerService.GetCartOfUserAsync(userId).Result?.Items;

					// update order items
					if (cartItems?.Count > 0)
					{
						//orderM.Products = new List<ProductDetailModel>();
						foreach (var i in cartItems)
						{
							if (i.isSelected)
							{
								// check if product is remain in stock && voucher date is in using time
								var product = await _productRepository.GetById(i._productId);
								if (product != null && i.amount <= product._amount)
								{
									ProductDetailModel item = i.items;
									item.Amount = i.amount;
									orderM.Products.Add(item);
									orderM.Amount += i.amount;
									orderM.Total += i.items.UniPrice * i.amount;
								}
								else
								{
									//notify that
								}
							}
						}
					}
					else
					{
						// have no any products
						// notify that
					}

					// update customer's info
					orderM.AccountID = userId;

					// all check pass
					// create new order
					var result = await _orderRepository.Add(orderM.ToEntity());
					if (result)
					{
						// update amount of bought products
						// remove products out of cart
						foreach (var i in orderM.Products)
						{
							Product? product = await _productRepository.GetById(i.Id);
							if (product != null && product._amount >= i.Amount)
							{
								product._amount -= i.Amount;
							}
							var isOk = await _productRepository.UpdateById(i.Id, product);
							if (isOk == false)
							{
								// notify
							}

							_customerService.RemoveItemToCart(userId, i.Id);
						}
					}
					else
					{

					}
				}

			}
			return RedirectToAction("Index", "Order");
		}

		return NotFound();
	}

	[HttpPost]
	public async Task<IActionResult> CreateBuyNow(OrderDetailModel orderM)
	{
		// check if model is valid?
		if (ModelState.IsValid)
		{
			string? userId;

			if (this.HttpContext != null)
			{
				userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (userId != null)
				{
					// update order products
					var productId = orderM.Products[0].Id;
					if (productId != null)
					{
						var product = await _productRepository.GetById(productId);
						if (product != null)
						{
							var productM = new ProductDetailModel(product);
							productM.Amount = orderM.Amount;
							orderM.Products.Clear();
							orderM.Products.Add(productM);
						}
					}

					// update customer's info
					orderM.AccountID = userId;

					// all check pass
					// create new order
					var result = await _orderRepository.Add(orderM.ToEntity());
					if (result)
					{
						// update amount of bought products
						// remove products out of cart
						foreach (var i in orderM.Products)
						{
							Product? product = await _productRepository.GetById(i.Id);
							if (product != null && product._amount >= i.Amount)
							{
								product._amount -= i.Amount;
							}
							var isOk = await _productRepository.UpdateById(i.Id, product);
							if (isOk == false)
							{
								// notify
							}
						}
					}
					else
					{

					}
				}

			}
			return RedirectToAction("Index", "Order");
		}

		return NotFound();
	}

	[HttpGet]
	public async Task<IActionResult> HandleStatus(string? id)
	{
		// check what order's status is
		// if waiting, paying, purchased -> cancel
		// if delivering -> delivered
		Order? order = await _orderRepository.GetById(id);
		if (order != null)
		{
			if (order._status == Status.Waiting || order._status == Status.Paying || order._status == Status.Purchased)
			{
				order._status = Status.Canceled;
			}
			else if (order._status == Status.Delivering)
			{
				order._status = Status.Delivered;
			}

			// update db
			var result = await _orderRepository.UpdateById(order._id, order);
			if (result)
			{
				return PartialView("_ViewStatus", new OrderDetailModel(order));
			}
		}

		return NotFound();
	}

	[HttpGet]
	public async Task<IActionResult> ChooseAddress()
	{
		// get cart of current user
		string? userId;

		if (this.HttpContext != null)
		{
			userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{

				UserModel? user = await _authService.GetAuthenticatedUserAsync(userId);
				if (user == null) return NotFound();
				return PartialView("_ChooseAddress", user.InforDelivery);
			}
		}
		return NotFound();
	}

	[HttpPost]
	public IActionResult ChooseAddress(InforDeliveryModel inforDeliveryModel)
	{
		if (!ModelState.IsValid) return NotFound();

		return PartialView("_ViewInfoCus", inforDeliveryModel);
	}

	[HttpPost]
	public async Task<IActionResult> CheckVoucher(string id, string value)
	{
		string message = "Invalid";
		bool isValid = false;
		float finalValue = 0;
		string discount = "$0";
		if (!float.TryParse(value, out finalValue))
		{
			return Json(new { isValid = isValid, message = message, value = value });
		}

		// Handle if having voucher code

		VoucherDetailModel voucher = await _stockService.GetADetailVoucher(id);

		if (voucher != null)
		{
			// Handle voucher's type
			if (voucher.State == VoucherStatus.ComingSoon)
			{
				message = "Unavaiable! The voucher is coming soon!";
			}
			else
			if (finalValue < voucher.ConditionValue)
			{
				message = "Buy " + (voucher.ConditionValue - finalValue) + "$ more to apply this voucher";
			}
			else
			// Handle if expired
			if (voucher.ExpiredDate > DateTime.Now)
			{
				// Annouce that voucer has been expired
				message = "Expired!";
			}
			else
			if (voucher.Amount <= 0)
			{
				// Annouce that voucer has been expired
				message = "Out of stock!";
			}
			else
			{
				switch (voucher.ValueType)
				{
					case ValueType.RealValue:
						{
							finalValue -= voucher.Discount;
							discount = voucher.Discount.ToString();
							break;
						}
					case ValueType.Percent:
						{
							finalValue -= finalValue * voucher.Discount / 100;
							discount = "$" + (finalValue * voucher.Discount / 100).ToString();

							break;
						}
				}
				isValid = true;
				message = "Voucher valid!";
			}
			// Handle voucher value type

		}

		return Json(new { isValid = isValid, message = message, value = "$" + finalValue.ToString(), discount = discount });

	}
}
