using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Core.Entities;
using Moq;

namespace TestProject;

public class Tests
{
    private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
    private readonly Mock<ICartRepository> _cartRepository = new Mock<ICartRepository>();
    private AuthService _authService;

    private string? Email;
    private string? PhoneNumber;
    private string? Password;

    [SetUp]
    public void Setup()
    {
;       _authService = new AuthService(_userRepository.Object, _cartRepository.Object);
    }

    [Test]
    public async Task F01UTCID01()
    {
        // Safe input
        Email = "phatlam1811@gmail.com";
        PhoneNumber = "0344236212";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsInstanceOf<User>(result);
    }

    [Test]
    public async Task F01UTCID02()
    {
        // Invalid email - null
        Email = null;
        PhoneNumber = "0344236212";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID03()
    {
        // Invalid email - Invalid format
        Email = "asdasdas";
        PhoneNumber = "0344236212";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID04()
    {
        // Invalid email - existed
        Email = "hoangle.q3@gmail.com";
        PhoneNumber = "0344236212";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID05()
    {
        // Invalid phone number - null
        Email = "phatlam1811@gmail.com";
        PhoneNumber = null;
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID06()
    {
        // Invalid phone number - Invalid format
        Email = "phatlam1811@gmail.com";
        PhoneNumber = "dfdsjf324";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID07()
    {
        // Invalid phone number - Invalid format
        Email = "phatlam1811@gmail.com";
        PhoneNumber = "12321213";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID08()
    {
        // Invalid phone number - Existed
        Email = "phatlam1811@gmail.com";
        PhoneNumber = "0932826231";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID09()
    {
        // Invalid password - null
        Email = "phatlam1811@gmail.com";
        PhoneNumber = "0344236212";
        Password = null;

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID10()
    {
        // Invalid password - minLenght < 6
        Email = "phatlam1811@gmail.com";
        PhoneNumber = "0344236212";
        Password = "a23";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID11()
    {
        // Invalid password - must consists of letters & numbers
        Email = "phatlam1811@gmail.com";
        PhoneNumber = "0344236212";
        Password = "123456";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsNull(result);
    }

    [Test]
    public async Task F01UTCID12()
    {
        // Safe input
        Email = "lamphat613@gmail.com";
        PhoneNumber = "0344235212";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsInstanceOf<User>(result);
    }

    [Test]
    public async Task F01UTCID13()
    {
        // Safe input
        Email = "huyen123@gmai.com";
        PhoneNumber = "0344238212";
        Password = "abc123";

        var result = await _authService.RegisterAsync(Email, PhoneNumber, Password);

        Assert.IsInstanceOf<User>(result);
    }
}