using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BlackBoxTestProject
{
	public class TestUsers
	{
		IWebDriver driver;

		[SetUp]
		public void StartChrome()
		{
			
			driver = new ChromeDriver(".");
            driver.Navigate().GoToUrl("https://localhost:7120/");
            IWebElement account = driver.FindElement(By.Id("ip-account"));
            IWebElement password = driver.FindElement(By.Id("ip-password"));
            IWebElement webElement = driver.FindElement(By.Id("btn-gotoapp"));
            account.SendKeys("jah@gmail.com");
            password.SendKeys("123123");
            webElement.Click();
			IWebElement userNAV = driver.FindElement(By.Id("user-nav"));
            userNAV.Click();


		}

		[Test]
		public void TestFilterCustomer()
		{
			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("filter-cus"));
            webElement.Click();
            Assert.Pass();
		}

        [Test]
        public void TestUserCreate()
        {
           
			//open create order page
            IWebElement btnCreate = driver.FindElement(By.Id("btn-create"));
            btnCreate.Click();
			//set up data for create an order


		
			//cus info
			IWebElement ipCusName = driver.FindElement(By.Id("ip-name"));
			ipCusName.SendKeys("TEst cus");

			IWebElement ipCusPhone = driver.FindElement(By.Id("ip-phone"));
			ipCusPhone.SendKeys("0123321133");

			IWebElement ipCusEmail = driver.FindElement(By.Id("ip-email"));
			ipCusEmail.SendKeys("test11@gmail.com");

			IWebElement ipCusaddress = driver.FindElement(By.Id("ip-address"));
			ipCusaddress.SendKeys("test adress");

			IWebElement submit = driver.FindElement(By.Id("btn-submit"));
			submit.Click();



			//Submit success
			IWebElement webElement = driver.FindElement(By.Id("hihi"));
			Assert.That(webElement.Displayed, Is.True);


        }

		[Test]
		public void TestUserEdit()
		{
			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("jahdkd@gmail.com"));
			webElement.Click();	
			IWebElement address = driver.FindElement(By.Id("ip-address"));
			address.SendKeys("test");
			IWebElement submit = driver.FindElement(By.Id("btn-submit"));
			submit.Click();
			//Submit success
			IWebElement webElement1 = driver.FindElement(By.Id("hihi"));
			Assert.That(webElement1.Displayed, Is.True);
		}


	}
}