using FlowerShopManagement.Core.Entities;
using MongoDB.Bson.Serialization;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Infrustructure.MongoDB.Implements;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using FlowerShopManagement.Infrustructure.Mail;
using FlowerShopManagement.Application.Interfaces.UserSerivices;
using FlowerShopManagement.Application.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region //============= MongoDb Configurations =============//
//-- Database Configurations --//
builder.Services.Configure<MongoDBSettings>(mongoDBSettings => {
    mongoDBSettings.ConnectionString = builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value;
    mongoDBSettings.DatabaseName = builder.Configuration.GetSection("DatabaseSettings:DatabaseName").Value;});

builder.Services.AddSingleton<IMongoDBSettings>(_ => _.GetRequiredService<IOptions<MongoDBSettings>>().Value);

//-- Repository Services (CRUD) --//
builder.Services.AddScoped<IMongoDBContext, MongoDBContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();

//-- Entities Mapping --//
BsonClassMap.RegisterClassMap<User>(cm =>
{
    cm.MapIdField(c => c._id);
    cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.String));
    cm.AutoMap();
});

BsonClassMap.RegisterClassMap<Cart>(cm =>
{
    cm.MapIdField(c => c._id);
    cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.String));
    cm.AutoMap();
});

BsonClassMap.RegisterClassMap<Product>(cm =>
{
    cm.MapIdField(c => c._id);
    cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.String));
    cm.AutoMap();
});

BsonClassMap.RegisterClassMap<Review>(cm =>
{
    cm.MapIdField(c => c._id);
    cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.String));
    cm.AutoMap();
});

BsonClassMap.RegisterClassMap<Order>(cm =>
{
    cm.MapIdField(c => c._id);
    cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.String));
    cm.AutoMap();
});
#endregion

// Add application logic services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IStockService, StockServices>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPersonalService, UserService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<MailKitService>();

// HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

// Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    //app.MapRazorPages();
});

app.Run();
