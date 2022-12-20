using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Infrustructure.MongoDB.Implements;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

namespace TestProject;

public class SignInAsyncUTC
{
    private IMongoDBSettings _mongoDbSettings;
    private IMongoDBContext _mongoDBContext;

    private IUserRepository _userRepository;
    private ICartRepository _cartRepository;
    private AuthService _authService;

    private string? EmailOrPhoneNb;
    private string? Password;

    [SetUp]
    public void Setup()
    {
        _mongoDbSettings = new MongoDBSettings();
        _mongoDbSettings.ConnectionString = "mongodb+srv://phatlam1811:%21D212884553@ooad-se100-n11-sem1-202.uk4cshi.mongodb.net/?retryWrites=true&w=majority";
        _mongoDbSettings.DatabaseName = "Test-DB";

        _mongoDBContext = new MongoDBContext(_mongoDbSettings);

        _userRepository = new UserRepository(_mongoDBContext);
        _cartRepository = new CartRepository(_mongoDBContext);

        _authService = new AuthService(_userRepository, _cartRepository);
    }

    [Test]
    public async Task F02UTCID01()
    {
        // Safe input
        EmailOrPhoneNb = "phatlam1811@gmail.com";
        Password = "lamphat123";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNotNull(result);
        Assert.AreEqual(result._id, "69413ce5-74e5-4676-a208-d45d4b025e71");
    }

    [Test]
    public async Task F02UTCID02()
    {
        // Invalid password - null
        EmailOrPhoneNb = "phatlam1811@gmail.com";
        Password = null;

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F02UTCID03()
    {
        // Wrong password
        EmailOrPhoneNb = "phatlam1811@gmail.com";
        Password = "asfasfas";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F02UTCID04()
    {
        // Null email or phone number
        EmailOrPhoneNb = null;
        Password = "lamphat123";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F02UTCID05()
    {
        // Invalid email
        EmailOrPhoneNb = "asdfsda";
        Password = "lamphat123";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F02UTCID06()
    {
        // Unregistered email
        EmailOrPhoneNb = "z613z@gmail.com";
        Password = "lamphat123";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F02UTCID07()
    {
        // Safe input
        EmailOrPhoneNb = "0383001282";
        Password = "lamphat123";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNotNull(result);
        Assert.AreEqual(result._id, "69413ce5-74e5-4676-a208-d45d4b025e71");
    }

    [Test]
    public async Task F02UTCID08()
    {
        // Invalid password - null
        EmailOrPhoneNb = "0933766232";
        Password = null;

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F02UTCID09()
    {
        // Wrong password
        EmailOrPhoneNb = "phatlam1811@gmail.com";
        Password = "asdfsdafsda";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F02UTCID10()
    {
        // Invalid phone number
        EmailOrPhoneNb = "2138912739";
        Password = "lamphat123";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F02UTCID11()
    {
        // Unregistered phone number
        EmailOrPhoneNb = "0933766232";
        Password = "lamphat123";

        var result = await _authService.SignInAsync(EmailOrPhoneNb, Password);

        Assert.IsNull(result);
    }
}
