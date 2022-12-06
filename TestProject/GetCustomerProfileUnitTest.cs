using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Services;
using FlowerShopManagement.Infrustructure.DatabaseSettings;
using FlowerShopManagement.Infrustructure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Web.Mvc;
using Assert = NUnit.Framework.Assert;

namespace TestProject
{
    [TestClass]
    public class GetCustomerProfileTest
    {
        private MongoDbDAO _mongoDbDAO = new MongoDbDAO(new MongoClient("mongodb+srv://phatlam1811:%21D212884553@ooad-se100-n11-sem1-202.uk4cshi.mongodb.net/?retryWrites=true&w=majority"));
        private CustomerManagementServices _customerManagementServices;

        [SetUp]
        [TestInitialize]
        public void Setup()
        {
            _customerManagementServices = new CustomerManagementServices(new CartCRUD(_mongoDbDAO), new CustomerCRUD(_mongoDbDAO), new ProfileCRUD(_mongoDbDAO));

        }

        [Test]
        public  void GCP01()
        {
            var obj1 = _customerManagementServices.GetProfile("").Result;
            Assert.That(obj1,Is.EqualTo(null));
        }

        [Test]
        public void GCP02()
        {
            var obj1 = _customerManagementServices.GetProfile("632e2341ab01fc3d6e2a9741").Result;

            Assert.That(obj1, Is.TypeOf<Profile>());
        }

     
    }
}