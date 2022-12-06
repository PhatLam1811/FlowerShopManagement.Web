using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Application.MongoDB.Interfaces;
using FlowerShopManagement.Application.Services;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Infrustructure.Google.Implementations;
using FlowerShopManagement.Infrustructure.Google.Interfaces;
using FlowerShopManagement.Infrustructure.MongoDB.Implements;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using Google.Apis.Gmail.v1;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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

#region //================== App Services ==================//
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddScoped<IGmailServices ,GmailServices>();
#endregion

#region Commentted
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
//builder.Services.AddSingleton<IMongoClient>(
//   s => new MongoClient(builder.Configuration.GetValue<string>("CustomerDatabase:ConnectionString")));
// builder.Services.AddSingleton<IMongoDBServices, MongoDBServices>(
//    s => new MongoDBServices(
//        builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString"),
//        builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName")));


// Add object CRUD operation services
//builder.Services.AddScoped<ICartDAOServices, CartServices>();
//builder.Services.AddScoped<ICustomer, FlowerShopManagement.Infrustructure.DatabaseSettings.CustomerServices>();
//builder.Services.AddScoped<IProduct, ProductServices>();

// Add application logic services
//builder.Services.AddScoped<ICustomerServices, FlowerShopManagement.Application.Services.Temp.CustomerServices>();

// Authentication logic services
// builder.Services.AddScoped<IApplicationUserServices, ApplicationUserServices>();
// builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();

// builder.Services.AddScoped<IUserDAOServices, UserDAOServices>();
// builder.Services.AddScoped<ICartDAOServices, CartDAOServices>();
// builder.Services.AddScoped<ISecurityServices, SecurityServices>();
#endregion

// HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//// Swagger
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
