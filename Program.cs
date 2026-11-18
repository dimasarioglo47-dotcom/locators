using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V157.IndexedDB;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
[TestFixtureAttribute]
class Test
{

    private IWebDriver driver;
    private  string  baseUrl;
    [SetUp]
    public void SetUp()
    {

        baseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["BaseUrl"];

        driver = new ChromeDriver();
    

    }
    
    [Test]
        [TestCase("C#","Ukraine")]
        [TestCase("Python","Argentina")]
    public void RemoteJobSearch_ByCountryAndLanguage_LeadsToApplyForm(string ProgramLanguage,string Country)
    {
        driver.Url=baseUrl;
        driver.Manage().Window.Maximize();
        var wait = new WebDriverWait(driver,TimeSpan.FromSeconds(15));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        wait.Until(driver=>{
           var btn= driver.FindElements(By.Id("onetrust-accept-btn-handler")).FirstOrDefault(e=>e.Displayed&& e.Enabled);
           if (btn == null) return false;
           btn.Click();
           return true;
           });
         
       wait.Until(driver=>driver.FindElement(By.LinkText("Careers"))).Click(); 
       wait.Until(driver=>driver.FindElement(By.ClassName("button-body"))).Click();
       wait.Until(d => d.FindElements(By.Id("onetrust-accept-btn-handler"))
                 .FirstOrDefault(e => e.Displayed && e.Enabled)).Click();
       wait.Until(d => !d.FindElements(By.Id("onetrust-banner-sdk")).Any(e => e.Displayed));
       wait.Until(driver =>
               driver.FindElement(
                   By.XPath("//div[@data-testid='country-dropdown']/descendant::div[contains(@class,'dropdown__control')]"))) 
           .Click();
       var input = wait.Until(driver =>
           {
               var el =driver.FindElement(By.CssSelector("input[aria-label='Choose your country']:not([disabled])"));
               return el.GetAttribute("aria-expanded") == "true" ? el : null;
               
           }
           );
      
       input.SendKeys(Country);
       input.SendKeys(Keys.Enter);
        var remote =wait.Until(driver => driver.FindElement(By.XPath("//label[contains(.,'Remote')]"))); 
       remote.Click();
       wait.Until(driver=>driver.FindElement(By.CssSelector("[data-testid='search-input']"))).SendKeys(ProgramLanguage);
       wait.Until(d=>d.FindElement(By.Name("submit_search_box_button"))).Click();
       var link = wait.Until(d=>d.FindElement(By.XPath("(//a[@data-testid='job-card-link'])[last()]")));
       new Actions(driver).MoveToElement(link).Click().Perform();
       var apply = wait.Until(d => d.FindElements(By.TagName("button")).FirstOrDefault(b=>b.Text.Contains("apply",StringComparison.OrdinalIgnoreCase) && b.Displayed && b.Enabled));
       apply.Click();
    }
    [TearDown]
    public void TearDown()
    {
        
     if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed
        && driver is ITakesScreenshot shot)
    {
        shot.GetScreenshot().SaveAsFile($"{TestContext.CurrentContext.Test.Name}.png");
    }

    driver?.Dispose();
   
        
    }
    

}