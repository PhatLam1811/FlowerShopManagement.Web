using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Services;
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
builder.Services.AddSingleton<IMongoDbDAO, MongoDbDAO>();
builder.Services.AddSingleton<IMongoClient>(
    s => new MongoClient(builder.Configuration.GetValue<string>("CustomerDatabase:ConnectionString")));

// Add object CRUD operation services
builder.Services.AddScoped<ICartCRUD, CartCRUD>();
builder.Services.AddScoped<ICustomerCRUD, CustomerCRUD>();
builder.Services.AddScoped<IProductCRUD, ProductCRUD>();
builder.Services.AddScoped<ISupplierCRUD, SupplierCRUD>();

// Add application logic services
builder.Services.AddScoped<ICustomerServices, CustomerServices>();


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
