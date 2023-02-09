using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BlackBoxTestProject
{
	public class TestProducts
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
        }

		[TearDown]
		public void EndChrome()
		{
			driver.Close();
		}
		[Test]
		public void TestGoToProductPage()
		{
			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("hihi"));
            Assert.That(webElement.Displayed, Is.True);
		}


        [Test]
        public void Test2Search()
        {
            IWebElement inputSearch = driver.FindElement(By.Id("ip-search"));
            inputSearch.SendKeys("TP1");

            IWebElement btnSearch = driver.FindElement(By.Id("btn-search"));
            btnSearch.Click();

            IWebElement webElement = driver.FindElement(By.Id("hihi"));
            Assert.That(webElement.Displayed, Is.True);
        }

        [Test]
        public void Test3Create()
        {
            IWebElement btnCreate = driver.FindElement(By.Id("btn-create"));
            btnCreate.Click();
            IWebElement inputName = driver.FindElement(By.Id("ip-productname"));
            inputName.SendKeys("Test Product");
            IWebElement inputColor = driver.FindElement(By.Id("ip-color"));
			inputColor.SendKeys("Test Color");
			IWebElement inputSize = driver.FindElement(By.Id("ip-size"));
			inputSize.SendKeys("test size ");
			IWebElement inputMaterial = driver.FindElement(By.Id("ip-material"));
			inputMaterial.SendKeys("Test Material");
			IWebElement inputAmount = driver.FindElement(By.Id("ip-amount"));
			inputAmount.SendKeys("20");
			IWebElement inputMaintainment = driver.FindElement(By.Id("ip-maintainment"));
			inputMaintainment.SendKeys("Test maintainment");
			IWebElement inputDes = driver.FindElement(By.Id("ip-description"));
			inputDes.SendKeys("Test des");

			IWebElement submit = driver.FindElement(By.Id("btn-submit"));
			submit.Click();
			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("hihi"));
			Assert.That(webElement.Displayed, Is.True);


        }

		[Test]
		public void TestEdit()
		{
			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("f6f4196f-0a70-4b34-9b50-52defa621166"));
			webElement.Click();
			IWebElement price = driver.FindElement(By.Id("ip-price"));
			price.SendKeys("10");
			IWebElement btnSearch = driver.FindElement(By.Id("btn-submit"));
			btnSearch.Click();
			Assert.Pass();
		}
		[Test]
		public void TestDelete()
		{
			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("f6f4196f-0a70-4b34-9b50-52defa621166"));

			webElement.Click();
			IWebElement btnD = driver.FindElement(By.Id("btn-delete"));
			btnD.Click();
			//Submit login
			IWebElement home = driver.FindElement(By.Id("hihi"));
			Assert.That(home.Displayed, Is.True);
		}

	}
}