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
		public void Test1JUSTDOIT()
		{
			driver.Navigate().GoToUrl("https://localhost:7120/");


			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("btn-gotoapp"));
			webElement.Click();
		}


        [Test]
        public void Test2DungTKMK()
        {
            driver.Navigate().GoToUrl("https://localhost:7120/");


            //Submit login
            IWebElement account = driver.FindElement(By.Id("ip-account"));
            IWebElement password = driver.FindElement(By.Id("ip-password"));
            IWebElement webElement = driver.FindElement(By.Id("btn-gotoapp"));
			account.SendKeys("jah@gmail.com");
            password.SendKeys("123123");
            webElement.Click();
        }

        [Test]
        public void Test3SaiTKMK()
        {
            driver.Navigate().GoToUrl("https://localhost:7120/");


            //Submit login
            IWebElement account = driver.FindElement(By.Id("ip-account"));
            IWebElement password = driver.FindElement(By.Id("ip-password"));
            IWebElement webElement = driver.FindElement(By.Id("btn-gotoapp"));
            account.SendKeys("jah@gmail.com");
            password.SendKeys("1231233");
            webElement.Click();
        }
    }
}