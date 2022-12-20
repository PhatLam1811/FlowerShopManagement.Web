using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BlackBoxTestProject
{
	public class TestImport
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
			IWebElement userNAV = driver.FindElement(By.Id("import-nav"));
            userNAV.Click();


		}



		[Test]
		public void TestUserCreate()
		{


			IWebElement btnCreate = driver.FindElement(By.Id("f6f4196f-0a70-4b34-9b50-52defa621166"));
			btnCreate.SendKeys("3");
		


			//cus info
			IWebElement ipCusName = driver.FindElement(By.Id("1"));
			ipCusName.Submit();

			IWebElement submit = driver.FindElement(By.Id("btn-submit"));
			submit.Click();

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