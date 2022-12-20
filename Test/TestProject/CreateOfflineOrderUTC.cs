using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Infrustructure.MongoDB.Implements;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

namespace TestProject;

public class CreateOfflineOrderUTC
{
    private IMongoDBSettings _mongoDbSettings;
    private IMongoDBContext _mongoDBContext;

    private IOrderRepository _orderRepository;
    private IUserRepository _userRepository;
    private IProductRepository _productRepository;

    private ISaleService _saleService;

    private List<ProductModel?> _products = new List<ProductModel>(); 
    private OrderModel? Order;
    private UserModel? User;

    [SetUp]
    public async Task Setup()
    {
        _mongoDbSettings = new MongoDBSettings();
        _mongoDbSettings.ConnectionString = "mongodb+srv://phatlam1811:%21D212884553@ooad-se100-n11-sem1-202.uk4cshi.mongodb.net/?retryWrites=true&w=majority";
        _mongoDbSettings.DatabaseName = "Test-DB";

        _mongoDBContext = new MongoDBContext(_mongoDbSettings);

        _userRepository = new UserRepository(_mongoDBContext);
        _orderRepository = new OrderRepository(_mongoDBContext);
        _productRepository = new ProductRepository(_mongoDBContext);

        var result = await _productRepository.GetAll();

        foreach (var item in result)
        {
            var model = new ProductModel(item);
            _products.Add(model);
        }

        _saleService = new SaleService();
    }

    [Test] // Defect null product list
    public async Task F03UTCID01()
    {
        // Null product list
        Order = new OrderModel();
        Order.Products = null;

        User = new UserModel();
        User.Name = "Lam Tan Phat";
        User.PhoneNumber = "0344221322";
        User.Email = "lamphatz613@gmail.com";

        Assert.IsFalse(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test] // Defect null user
    public async Task F03UTCID02()
    {
        // Null user
        var size = 1;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = null;

        Assert.IsFalse(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID03()
    {
        // Safe input
        var size = 1;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Le Hoang Quy";
        User.PhoneNumber = "0344221322";
        User.Email = "hoangle49@gmail.com";

        Assert.IsTrue(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID04()
    {
        // Safe input
        var size = 5;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Nguyen Khanh Huyen";
        User.PhoneNumber = "0348979654";
        User.Email = "huyen0709@gmail.com";

        Assert.IsTrue(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID05()
    {
        // Safe input
        var size = 5;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Lam Tan Phat";
        User.PhoneNumber = "0344221322";
        User.Email = "lamphatz613@gmail.com";

        Assert.IsTrue(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID06()
    {
        // Safe input
        var size = 10;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Le Hoang Quy";
        User.PhoneNumber = "0344221322";
        User.Email = "hoangle49@gmail.com";

        Assert.IsTrue(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID07()
    {
        // Safe input
        var size = 10;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Nguyen Khanh Huyen";
        User.PhoneNumber = "0348979654";
        User.Email = "huyen0709@gmail.com";

        Assert.IsTrue(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID08()
    {
        // Safe input
        var size = 20;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Lam Tan Phat";
        User.PhoneNumber = "0344221322";
        User.Email = "lamphatz613@gmail.com";

        Assert.IsTrue(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID09()
    {
        // Safe input
        var size = 20;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Le Hoang Quy";
        User.PhoneNumber = "0344221322";
        User.Email = "hoangle49@gmail.com";

        Assert.IsTrue(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID10()
    {
        // Overcome max diferent products per order (0 < size <= 20) 
        var size = 21;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Nguyen Khanh Huyen";
        User.PhoneNumber = "0348979654";
        User.Email = "huyen0709@gmail.com";

        Assert.IsFalse(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID11()
    {
        // Overcome max diferent products per order (0 < size <= 20)
        var size = 21;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Lam Tan Phat";
        User.PhoneNumber = "0344221322";
        User.Email = "lamphatz613@gmail.com";

        Assert.IsFalse(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID12()
    {
        // Overcome max diferent products per order (0 < size <= 20)
        var size = 25;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Le Hoang Quy";
        User.PhoneNumber = "0344221322";
        User.Email = "hoangle49@gmail.com";

        Assert.IsFalse(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID13()
    {
        // Overcome max diferent products per order (0 < size <= 20)
        var size = 25;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();

        User = new UserModel();
        User.Name = "Nguyen Khanh Huyen";
        User.PhoneNumber = "0348979654";
        User.Email = "huyen0709@gmail.com";

        Assert.IsFalse(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID14()
    {
        // Insufficient amount (out of stock)
        var size = 10;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();
        Order.Products[2].Amount = 100;
        Order.Products[4].Amount = 120;
        Order.Products[6].Amount = 140;

        User = new UserModel();
        User.Name = "Lam Tan Phat";
        User.PhoneNumber = "0344221322";
        User.Email = "lamphatz613@gmail.com";

        Assert.IsFalse(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }

    [Test]
    public async Task F03UTCID15()
    {
        // Safe input
        var size = 10;
        Order = new OrderModel();
        Order.Products = _products.Take<ProductModel>(size).ToList<ProductModel>();
        Order.Products[3].Amount = 0;
        Order.Products[5].Amount = 12;
        Order.Products[7].Amount = 14;

        User = new UserModel();
        User.Name = "Le Hoang Quy";
        User.PhoneNumber = "0344221322";
        User.Email = "hoangle49@gmail.com";

        Assert.IsFalse(await _saleService.CreateOfflineOrder(Order, User, _orderRepository, _userRepository, _productRepository));
    }
}
