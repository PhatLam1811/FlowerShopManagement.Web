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
			driver.Url = "http://www.google.com.vn";
			Assert.Pass();
		}
		[OneTimeTearDown]
		public void CloseTest()
		{

			driver.Close();
		}
	}
}