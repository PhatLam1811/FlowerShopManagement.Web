using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Driver;
using FlowerShopManagement.Infrustructure.DatabaseSettings;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Services;
using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using MongoDB.Bson.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("CustomerDatabase"));

/*builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);*/

// Add database access services
builder.Services.AddSingleton<IMongoDbDAO, MongoDbDAO>();
builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(builder.Configuration.GetValue<string>("CustomerDatabase:ConnectionString")));

// Add object CRUD operation services
builder.Services.AddScoped<ICartCRUD, CartCRUD>();
builder.Services.AddScoped<ICustomerCRUD, CustomerCRUD>();
builder.Services.AddScoped<IOrderCRUD, OrderCRUD>();
builder.Services.AddScoped<IProfileCRUD, ProfileCRUD>();

// Add application logic services
builder.Services.AddScoped<ICustomerManagementServices, CustomerManagementServices>();



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

app.UseAuthorization();

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
