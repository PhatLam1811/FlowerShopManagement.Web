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
    public class GetCustomerCartTest
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
        public  void GCC01()
        {
            var obj1 = _customerManagementServices.GetCustomerCart("").Result;
            Assert.That(obj1,Is.EqualTo(null));
        }

        [Test]
        public void GCC02()
        {
            var obj1 = _customerManagementServices.GetCustomerCart("634bccee40515633add09be6").Result;

            Assert.That(obj1, Is.TypeOf<Cart>());
        }
        [Test]
        public void GCC03()
        {
            var obj1 = _customerManagementServices._customerCRUD.GetCustomerById("634bccee40515633add09be6").Result;

            Assert.That(obj1, Is.TypeOf<Customer>());
        }

        [Test]
        public void GCC04()
        {
            var ok = new Customer();
            ok._fullName = "mlem";
            var obj1 = _customerManagementServices._customerCRUD.AddNewCustomer(ok);

            Assert.That(obj1.Result, Is.EqualTo(true));
        }

    }
}