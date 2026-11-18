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
        [TestCase("C#","Ukraine")]
    public void Tester(string lab,string param)
    {
        driver.Manage().Window.Maximize();
        var wait = new WebDriverWait(driver,TimeSpan.FromSeconds(10));
        wait.Until(driver=>driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Click();
         
       wait.Until(driver=>driver.FindElement(By.LinkText("Careers"))).Click(); 
       wait.Until(driver=>driver.FindElement(By.ClassName("button-body"))).Click();
       wait.Until(driver=>driver.FindElement(By.Id("onetrust-accept-btn-handler"))).Click();
       wait.Until(d => !d.FindElements(By.Id("onetrust-banner-sdk")).Any(e => e.Displayed));
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
        var remote =wait.Until(driver => driver.FindElement(By.XPath("//label[contains(.,'Remote')]"))); 
       remote.Click();
       wait.Until(driver=>driver.FindElement(By.CssSelector("[data-testid='search-input']"))).SendKeys(lab);
       wait.Until(d=>d.FindElement(By.CssSelector("[name='submit_search_box_button']"))).Click();
       var link = wait.Until(d=>d.FindElement(By.XPath("(//a[@data-testid='job-card-link'])[last()]")));
       new Actions(driver).MoveToElement(link).Click().Perform();
       var apply = wait.Until(d => d.FindElements(By.XPath("//button[@type='submit'][contains(.,'apply')]"))
       .FirstOrDefault(b => b.Displayed && b.Enabled));
       apply.Click();
    }



    // [TearDown]
    // public void TearDown()
    // {
    //     driver.Dispose();
        
    // }
    

}