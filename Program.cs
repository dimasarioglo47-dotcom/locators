using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
[TestFixtureAttribute]
class Test
{

    private IWebDriver driver;
    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
    
        driver.Url="https://www.epam.com/"; 
        //temporary
    }
    
    [Test]
    public void Tester()
    {
        driver.Manage().Window.Maximize();
        driver.FindElement(By.Id("onetrust-accept-btn-handler")).Click();
         Console.ReadLine();
    //    driver.FindElement(By.LinkText()); 
    }



    // [TearDown]
    // public void TearDown()
    // {
    //     driver.Dispose();
        
    // }
    

}