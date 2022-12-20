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
		public void TestShowLoginInPage()
		{
			driver.Navigate().GoToUrl("https://localhost:7120/");


			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("btn-gotoapp"));
			webElement.Click();
		}


        [Test]
        public void Test2RightPassword()
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
        public void Test3WrongPassword()
        {
            driver.Navigate().GoToUrl("https://localhost:7120/");
            IWebElement account = driver.FindElement(By.Id("ip-account"));
            IWebElement password = driver.FindElement(By.Id("ip-password"));
            IWebElement webElement = driver.FindElement(By.Id("btn-gotoapp"));
            //Submit login

            account.SendKeys("jah@gmail.com");
            password.SendKeys("1231233");
            webElement.Click();
        }
        [Test]
        public void Test4Register()
        {
            driver.Navigate().GoToUrl("https://localhost:7120/");
           //chuyeern trang
            IWebElement webElement = driver.FindElement(By.Id("btn-register"));
            webElement.Click();

            //Submit login
            IWebElement email = driver.FindElement(By.Id("ip-email"));
            IWebElement phone = driver.FindElement(By.Id("ip-phone"));
            IWebElement password = driver.FindElement(By.Id("ip-password"));
            IWebElement cpassword = driver.FindElement(By.Id("ip-confirmpassword"));
            email.SendKeys("phatmuontest@gmail.com");
            phone.SendKeys("0123321321");

            password.SendKeys("asd1231233");
            cpassword.SendKeys("asd1231233");
            IWebElement webElement1 = driver.FindElement(By.Id("btn-submit"));
            webElement1.Click();

        }

    }
}