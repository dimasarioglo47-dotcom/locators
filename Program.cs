using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using var driver = new ChromeDriver();
[TestFixtureAttribute]
class Test
{

    private IWebDriver driver;
    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
        driver.Url="https://www.epam.com/"; 
        Console.ReadLine();
    }
    
    [Test]
    public void Tester()
    {
    //    driver.FindElement(By.LinkText()); 
    }



    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
        
    }
    

}