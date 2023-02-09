using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BlackBoxTestProject
{
	public class TestSales
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
			IWebElement orderNAV = driver.FindElement(By.Id("order-nav"));
			orderNAV.Click();


		}

		[Test]
		public void TestFilterCanceledOrder()
		{
			//Submit login
			IWebElement webElement = driver.FindElement(By.Id("filter-canceled"));
            webElement.Click();
            Assert.Pass();
		}

        [Test]
        public void TestCreate()
        {
           
			//open create order page
            IWebElement btnCreate = driver.FindElement(By.Id("btn-create"));
            btnCreate.Click();
			//set up data for create an order


			IWebElement btnItemDialog = driver.FindElement(By.Id("btn-pickitemdialog"));
			btnItemDialog.Click();

            IWebElement dialog = driver.FindElement(By.Id("accordion666"));
            
            IWebElement input = dialog.FindElement(By.Id("b5beee9b-64b5-4b58-9830-8d580c16e401"));
			input.SendKeys("3");
			IWebElement submitItem = driver.FindElement(By.Id("btn-item"));
			submitItem.Click();
			//cus info
			IWebElement ipCusName = driver.FindElement(By.Id("ip-cusname"));
			ipCusName.SendKeys("TEst cus");

			IWebElement ipCusPhone = driver.FindElement(By.Id("ip-cusphone"));
			ipCusPhone.SendKeys("0123321123");

			IWebElement ipCusEmail = driver.FindElement(By.Id("ip-email"));
			ipCusEmail.SendKeys("test@gmail.com");

			IWebElement ipCusaddress = driver.FindElement(By.Id("ip-cusaddress"));
			ipCusaddress.SendKeys("test adress");

			IWebElement submit = driver.FindElement(By.Id("btn-submitorder"));
			submit.Click();



			//Submit success
			IWebElement webElement = driver.FindElement(By.Id("hihi"));
			Assert.That(webElement.Displayed, Is.True);


        }

        


    }
}