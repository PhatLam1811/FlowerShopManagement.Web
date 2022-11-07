using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Driver;
using FlowerShopManagement.Infrustructure.DatabaseSettings;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Entities;
using MongoDB.Bson.Serialization;
using FlowerShopManagement.Application.Services.Temp;
using FlowerShopManagement.Application.Interfaces.Temp;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using FlowerShopManagement.Infrustructure.MongoDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("CustomerDatabase"));

/*builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);*/

// Add database access services
/* builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(builder.Configuration.GetValue<string>("CustomerDatabase:ConnectionString"))); */
builder.Services.AddSingleton<IMongoDBServices, MongoDBServices>(
    s => new MongoDBServices(
        builder.Configuration.GetValue<string>("Database:ConnectionString"),
        builder.Configuration.GetValue<string>("Database:Name")));

// Add object CRUD operation services
builder.Services.AddScoped<ICart, CartServices>();
builder.Services.AddScoped<ICustomer, FlowerShopManagement.Infrustructure.DatabaseSettings.CustomerServices>();

// Add application logic services
builder.Services.AddScoped<ICustomerServices, FlowerShopManagement.Application.Services.Temp.CustomerServices>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
