using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Driver;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Entities;
using MongoDB.Bson.Serialization;
using FlowerShopManagement.Application.Services.Temp;
using FlowerShopManagement.Application.Interfaces.Temp;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using FlowerShopManagement.Application.Interfaces.UseCases;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Application.Interfaces.Application;
using FlowerShopManagement.Infrustructure.MongoDB.Implements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<MongoDBSettings>(
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
builder.Services.AddSingleton<IApplicationUserServices, ApplicationUserServices>();

// Add object CRUD operation services
builder.Services.AddScoped<IUserDAOServices, UserDAOServices>();
builder.Services.AddScoped<ICartDAOServices, CartDAOServices>();

// Add application logic services
builder.Services.AddScoped<ICustomerServices, FlowerShopManagement.Application.Services.Temp.CustomerServices>();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();

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
