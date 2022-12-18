using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BlackBoxTestProject
{
	public class Tests
	{
		IWebDriver driver;

		[OneTimeSetUp]
		public void StartChrome()
		{
			
			driver = new ChromeDriver(".");
		}

		[Test]
		public void Test1()
		{
			driver.Navigate().GoToUrl("https://localhost:7120/");


			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("btn-gotoapp"));
			webElement.Click();
		}

	}
}