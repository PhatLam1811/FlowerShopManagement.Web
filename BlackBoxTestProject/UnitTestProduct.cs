using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BlackBoxTestProject
{
	public class Test1s
	{
		IWebDriver driver;

		[OneTimeSetUp]
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
			IWebElement webElement = driver.FindElement(By.Id("a97e30fe-53d9-4895-9dda-79655d0421c4"));
			webElement.Click();
			IWebElement cancel = driver.FindElement(By.Id("cancel-a97e30fe-53d9-4895-9dda-79655d0421c4"));
			cancel.Click();
			Assert.Pass();
		}


	}
}