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
    public class CreateAnOrderTest
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
            var obj1 = _saleServices.CreateAnOrder("","",null).Result;
            Assert.That(obj1,Is.EqualTo(false));
        }

        [Test]
        public void CAO02()
        {
            var obj1 = _saleServices.CreateAnOrder("","0123456789",null).Result;

            Assert.That(obj1, Is.EqualTo(true));
        }

        [Test]
        public void CAO03()
        {
            var obj1 = _saleServices.CreateAnOrder("", "123", null).Result;

            Assert.That(obj1, Is.EqualTo(true));
        }

        [Test]
        public void CAO04()
        {
            var obj1 = _saleServices.CreateAnOrder("customer@gmail.com", "", null).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }
        [Test]
        public void CAO05()
        {
            var obj1 = _saleServices.CreateAnOrder("customer@gmail.com", "0123456789", null).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }

        [Test]
        public void CAO06()
        {
            var obj1 = _saleServices.CreateAnOrder("customer@gmail.com", "123", null).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }

        [Test]
        public void CAO07()
        {
            var obj1 = _saleServices.CreateAnOrder(It.IsAny<string>(), "", null).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }

        [Test]
        public void CAO08()
        {
            var obj1 = _saleServices.CreateAnOrder(It.IsAny<string>(), "0123456789", null).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }
        [Test]
        public void CAO09()
        {
            var obj1 = _saleServices.CreateAnOrder(It.IsAny<string>(), "123", null).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }

        [Test]
        public void CAO010()
        {
            var obj1 = _saleServices.CreateAnOrder("", "", It.IsAny<Order>()).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }

        [Test]
        public void CAO11()
        {
            var obj1 = _saleServices.CreateAnOrder("", "0123456789", It.IsAny<Order>()).Result;

            Assert.That(obj1, Is.EqualTo(true));
        }

        [Test]
        public void CAO12()
        {
            var obj1 = _saleServices.CreateAnOrder("", "123", It.IsAny<Order>()).Result;

            Assert.That(obj1, Is.EqualTo(false));
        }

        [Test]
        public void CAO13()
        {
            var obj1 = _saleServices.CreateAnOrder("customer@gmail.com", "", It.IsAny<Order>()).Result;
            Assert.That(obj1, Is.EqualTo(true));
        }
        [Test]
        public void CAO14()
        {
            var obj1 = _saleServices.CreateAnOrder("customer@gmail.com", "0123456789", It.IsAny<Order>()).Result;
            Assert.That(obj1, Is.EqualTo(true));
        }
        [Test]
        public void CAO15()
        {
            var obj1 = _saleServices.CreateAnOrder("customer@gmail.com", "123", It.IsAny<Order>()).Result;
            Assert.That(obj1, Is.EqualTo(true));
        }

        [Test]
        public void CAO16()
        {
            var obj1 = _saleServices.CreateAnOrder(It.IsAny<string>(), "123", null).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }

        [Test]
        public void CAO17()
        {
            var obj1 = _saleServices.CreateAnOrder(It.IsAny<string>(), "", null).Result;
            Assert.That(obj1, Is.EqualTo(true));
        }

        [Test]
        public void CAO18()
        {
            var obj1 = _saleServices.CreateAnOrder(It.IsAny<string>(), "0123456789", null).Result;
            Assert.That(obj1, Is.EqualTo(false));
        }
       
    }
}