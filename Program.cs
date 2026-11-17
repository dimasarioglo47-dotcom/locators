using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V157.IndexedDB;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
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
        [TestCase("Ukraine")]
    public void Tester(string param)
    {
        driver.Manage().Window.Maximize();
        var wait = new WebDriverWait(driver,TimeSpan.FromSeconds(10));
        wait.Until(driver=>driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Click();
         
       wait.Until(driver=>driver.FindElement(By.LinkText("Careers"))).Click(); 
       wait.Until(driver=>driver.FindElement(By.ClassName("button-body"))).Click();
       wait.Until(driver=>driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Click();
       wait.Until(driver =>
               driver.FindElement(
                   By.XPath("//div[@data-testid='country-dropdown']//div[contains(@class,'dropdown__control')]")))
           .Click();
       var input = wait.Until(driver =>
           {
               var el =driver.FindElement(By.CssSelector("input[aria-label='Choose your country']"));
               return el.GetAttribute("aria-expanded") == "true" ? el : null;
               
           }
           );
      
       input.SendKeys(param);
       input.SendKeys(Keys.Enter);
       wait.Until(driver => driver.FindElement(By.XPath("//label[contains(.,'Remote')]"))).Click();

    }



    // [TearDown]
    // public void TearDown()
    // {
    //     driver.Dispose();
        
    // }
    

}