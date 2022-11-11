using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Services;
using FlowerShopManagement.Infrustructure.DatabaseSettings;
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
    public class CancelOrderTest
    {
        private MongoDbDAO _mongoDbDAO = new MongoDbDAO(new MongoClient("mongodb+srv://phatlam1811:%21D212884553@ooad-se100-n11-sem1-202.uk4cshi.mongodb.net/?retryWrites=true&w=majority"));
        private SaleServices _saleServices;

        [SetUp]
        [TestInitialize]
        public void Setup()
        {
            _saleServices = new SaleServices(new CartCRUD(_mongoDbDAO), new CustomerCRUD(_mongoDbDAO),new OrderCRUD(_mongoDbDAO), new ProfileCRUD(_mongoDbDAO));

        }

        [Test]
        public  void CAO01()
        {
            var obj1 = _saleServices.CancelOrder("632e2341ab01fc3d6e2a9741", null).Result;
            Assert.That(obj1,Is.EqualTo(false));
        }

        [Test]
        public void CAO02()
        {
            var obj1 = _saleServices.CancelOrder("632e2341ab01fc3d6e2a9741",new Order()).Result;

            Assert.That(obj1, Is.TypeOf<Cart>());
        }

        [Test]
        public void CAO03()
        {
            var obj1 = _saleServices.CancelOrder("", null).Result;

            Assert.That(obj1, Is.TypeOf<Cart>());
        }

        [Test]
        public void CAO04()
        {
            var obj1 = _saleServices.CancelOrder("632e2341ab01fc3d6e2a9741", new Order()).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }
       
    }
}