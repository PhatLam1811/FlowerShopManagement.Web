using FlowerShopManagement.Application.Interfaces.Temp;
using FlowerShopManagement.Application.Services.Temp;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.DatabaseSettings;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("CustomerDatabase"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(builder.Configuration.GetValue<string>("CustomerDatabase:ConnectionString")));

// Add object CRUD operation services
builder.Services.AddScoped<ICart, CartServices>();
builder.Services.AddScoped<ICustomer, FlowerShopManagement.Infrustructure.DatabaseSettings.CustomerServices>();
builder.Services.AddScoped<IProduct, ProductServices>();

// Add application logic services
builder.Services.AddScoped<ICustomerServices, FlowerShopManagement.Application.Services.Temp.CustomerServices>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
