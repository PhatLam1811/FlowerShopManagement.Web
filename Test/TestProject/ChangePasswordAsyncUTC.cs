using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services.UserServices;
using FlowerShopManagement.Infrustructure.MongoDB.Implements;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

namespace TestProject;

public class ChangePasswordAsyncUTC
{
    private IMongoDBSettings _mongoDbSettings;
    private IMongoDBContext _mongoDBContext;

    private IUserRepository _userRepository;
    private ICartRepository _cartRepository;
    private UserService _userService;

    private string? Id;
    private string? OldPassword;
    private string? NewPassword;

    [SetUp]
    public void Setup()
    {
        _mongoDbSettings = new MongoDBSettings();
        _mongoDbSettings.ConnectionString = "mongodb+srv://phatlam1811:%21D212884553@ooad-se100-n11-sem1-202.uk4cshi.mongodb.net/?retryWrites=true&w=majority";
        _mongoDbSettings.DatabaseName = "Test-DB";

        _mongoDBContext = new MongoDBContext(_mongoDbSettings);

        _userRepository = new UserRepository(_mongoDBContext);
        _cartRepository = new CartRepository(_mongoDBContext);

        _userService = new UserService(_userRepository, _cartRepository);
    }

    [Test]
    public async Task F06UTCID01()
    {
        // Safe input
        Id = "48736c2c-89ff-4920-b8fa-8be5a2251af2";
        OldPassword = "";
        NewPassword = "Abc123";

        Assert.IsTrue(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID02()
    {
        // null - old password
        Id = "48736c2c-89ff-4920-b8fa-8be5a2251af2";
        OldPassword = null;
        NewPassword = "Abc123";

        Assert.IsFalse(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID03()
    {
        // Incorrect old password
        Id = "48736c2c-89ff-4920-b8fa-8be5a2251af2";
        OldPassword = "3242342123";
        NewPassword = "Abc123";

        Assert.IsFalse(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID04()
    {
        // Safe input
        Id = "4b0a4c53-dc98-4209-bb2b-588fa0c9ee48";
        OldPassword = "";
        NewPassword = "Abc123";

        Assert.IsTrue(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID05()
    {
        // Null - new password
        Id = "4b0a4c53-dc98-4209-bb2b-588fa0c9ee48";
        OldPassword = "Abc123";
        NewPassword = null;

        Assert.IsFalse(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID06()
    {
        // Invalid lenght - new password (length >= 6)
        Id = "4b0a4c53-dc98-4209-bb2b-588fa0c9ee48";
        OldPassword = "Abc123";
        NewPassword = "a23";

        Assert.IsFalse(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID07()
    {
        // Invalid format - new password
        Id = "4b0a4c53-dc98-4209-bb2b-588fa0c9ee48";
        OldPassword = "Abc123";
        NewPassword = "123456";

        Assert.IsFalse(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID08()
    {
        // Safe input
        Id = "4b0a4c53-dc98-4209-bb2b-588fa0c9ee48";
        OldPassword = "Abc123";
        NewPassword = "Password123";

        Assert.IsTrue(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID09()
    {
        // Safe input
        Id = "4b0a4c53-dc98-4209-bb2b-588fa0c9ee48";
        OldPassword = "Password123";
        NewPassword = "Asdhf1233";

        Assert.IsTrue(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }

    [Test]
    public async Task F06UTCID10()
    {
        // Safe input
        Id = "4b0a4c53-dc98-4209-bb2b-588fa0c9ee48";
        OldPassword = "Dsgdfsg324235";
        NewPassword = "123123";

        Assert.IsTrue(await _userService.ChangePasswordAsync(Id, OldPassword, NewPassword));
    }
}
