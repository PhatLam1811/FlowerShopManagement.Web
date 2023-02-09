using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Models;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Infrustructure.Mail;
using FlowerShopManagement.Infrustructure.MongoDB.Implements;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

namespace TestProject;

public class CreateRequestFormUTC
{
    private IMongoDBSettings _mongoDbSettings;
    private IMongoDBContext _mongoDBContext;

    private IMailService _mailService;
    private ISupplierRepository _supplierRepository;
    private IProductRepository _productRepository;

    private IImportService _importService;

    private List<ProductDetailModel>? _products = new List<ProductDetailModel>();
    private List<SupplierModel>? _suppliers = new List<SupplierModel>();

    private List<ProductDetailModel>? SelectedProducts = new List<ProductDetailModel>();
    private List<int>? Amounts;
    private List<SupplierModel>? SelectedSuppliers = new List<SupplierModel>();

    [SetUp]
    public async Task Setup()
    {
        _mongoDbSettings = new MongoDBSettings();
        _mongoDbSettings.ConnectionString = "mongodb+srv://phatlam1811:%21D212884553@ooad-se100-n11-sem1-202.uk4cshi.mongodb.net/?retryWrites=true&w=majority";
        _mongoDbSettings.DatabaseName = "Test-DB";

        _mongoDBContext = new MongoDBContext(_mongoDbSettings);

        _mailService = new MailKitService();
        _supplierRepository = new SupplierRepository(_mongoDBContext);
        _productRepository = new ProductRepository(_mongoDBContext);

        Amounts = new List<int>();

        var result = await _productRepository.GetAll();

        foreach (var item in result)
        {
            var model = new ProductDetailModel(item);
            _products.Add(model);
        }

        var result1 = await _supplierRepository.GetAll();

        foreach (var item in result1)
        {
            var model = new SupplierModel(item);
            _suppliers.Add(model);
        }

        _importService = new ImportService(_mailService);
    }

    [Test]
    public void F04UTCID01()
    {
        // Null products list
        var productSize = 1;
        var supplierSize = 1;
        SelectedProducts = null;
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts.Add(40);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNull(result);
    }

    [Test]
    public void F04UTCID02()
    {
        // Null amount list
        var productSize = 1;
        var supplierSize = 1;
        SelectedProducts = _products.Take<ProductDetailModel>(productSize).ToList<ProductDetailModel>();
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts = null;

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNull(result);
    }

    [Test]
    public void F04UTCID03()
    {
        // Insufficient request amount (40 <= amount <= 100)
        var productSize = 1;
        var supplierSize = 1;
        SelectedProducts = _products.Take<ProductDetailModel>(productSize).ToList<ProductDetailModel>();
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts.Add(0);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNull(result);
    }

    [Test]
    public void F04UTCID04()
    {
        // Safe input
        var productSize = 1;
        var supplierSize = 1;
        SelectedProducts = _products.Take<ProductDetailModel>(productSize).ToList<ProductDetailModel>();
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts.Add(40);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNotNull(result);
        foreach (var item in result.Products)
        {
            Assert.AreEqual(result.Products, SelectedProducts);
            Assert.AreEqual(result.Amounts, Amounts);
        }
        for (var i = 0; i < result.To.Count; i++)
            Assert.AreEqual(result.To[i], SelectedSuppliers[i].Email);
    }

    [Test]
    public void F04UTCID05()
    {
        // Null supplier list
        var productSize = 1;
        var supplierSize = 1;
        SelectedProducts = _products.Take<ProductDetailModel>(productSize).ToList<ProductDetailModel>();
        SelectedSuppliers = null;
        Amounts.Add(40);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNull(result);
    }

    [Test]
    public void F04UTCID06()
    {
        // Safe input
        var productSize = 1;
        var supplierSize = 2;
        SelectedProducts = _products.Take<ProductDetailModel>(productSize).ToList<ProductDetailModel>();
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts.Add(65);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);
        
        Assert.IsNotNull(result);
        foreach (var item in result.Products)
        {
            Assert.AreEqual(result.Products, SelectedProducts);
            Assert.AreEqual(result.Amounts, Amounts);
        }
        for (var i = 0; i < result.To.Count; i++)
            Assert.AreEqual(result.To[i], SelectedSuppliers[i].Email);
    }

    [Test]
    public void F04UTCID07()
    {
        // Safe input
        var productSize = 1;
        var supplierSize = 3;
        SelectedProducts = _products.Take<ProductDetailModel>(productSize).ToList<ProductDetailModel>();
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts.Add(83);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNotNull(result);
        foreach (var item in result.Products)
        {
            Assert.AreEqual(result.Products, SelectedProducts);
            Assert.AreEqual(result.Amounts, Amounts);
        }
        for (var i = 0; i < result.To.Count; i++)
            Assert.AreEqual(result.To[i], SelectedSuppliers[i].Email);
    }

    [Test]
    public void F04UTCID08()
    {
        // // Insufficient request amount (40 <= amount <= 100)
        var productSize = 1;
        var supplierSize = 1;
        SelectedProducts = null;
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts.Add(120);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNull(result);
    }

    [Test]
    public void F04UTCID09()
    {
        // Safe input
        var productSize = 3;
        var supplierSize = 1;
        SelectedProducts = _products.Take<ProductDetailModel>(productSize).ToList<ProductDetailModel>();
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts.Add(50);
        Amounts.Add(100);
        Amounts.Add(60);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNotNull(result);
        foreach (var item in result.Products)
        {
            Assert.AreEqual(result.Products, SelectedProducts);
            Assert.AreEqual(result.Amounts, Amounts);
        }
        for (var i = 0; i < result.To.Count; i++)
            Assert.AreEqual(result.To[i], SelectedSuppliers[i].Email);
    }

    [Test]
    public void F04UTCID010()
    {
        // Null product list
        var productSize = 5;
        var supplierSize = 1;
        SelectedProducts = _products.Take<ProductDetailModel>(productSize).ToList<ProductDetailModel>();
        SelectedSuppliers = _suppliers.Take<SupplierModel>(supplierSize).ToList<SupplierModel>();
        Amounts.Add(60);
        Amounts.Add(110);
        Amounts.Add(60);
        Amounts.Add(70);
        Amounts.Add(80);

        var result = _importService.CreateSupplyForm(SelectedProducts, Amounts, SelectedSuppliers);

        Assert.IsNull(result);
    }
}
