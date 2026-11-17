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
        var wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
        wait.Until(driver=>driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Click();
         
       wait.Until(driver=>driver.FindElement(By.LinkText("Careers"))).Click(); 
       wait.Until(driver=>driver.FindElement(By.ClassName("button-body"))).Click();
       Console.ReadLine();
    }



    // [TearDown]
    // public void TearDown()
    // {
    //     driver.Dispose();
        
    // }
    

}