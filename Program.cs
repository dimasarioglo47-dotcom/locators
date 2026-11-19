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

    private IWebDriver? _driver;
    private  string?  _baseUrl;
    private WebDriverWait wait;
    

    [SetUp]
    public void SetUp()
    {

        _baseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["BaseUrl"];

        _driver = new ChromeDriver();
    

    }
    
    [Test]
        [TestCase("C#","Ukraine")]
        [TestCase("Python","Argentina")]
    public void RemoteJobSearch_ByCountryAndLanguage_LeadsToApplyForm(string programLanguage,string country)
    {
       
        if (_baseUrl!=null)_driver.Url=_baseUrl;
        _driver.Manage().Window.Maximize();
        wait = new WebDriverWait(_driver,TimeSpan.FromSeconds(15));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        CloseCookieBanner();
         
       wait.Until(webDriver=>webDriver.FindElement(By.LinkText("Careers"))).Click(); 
       wait.Until(webDriver=>webDriver.FindElement(By.ClassName("button-body"))).Click();
       CloseCookieBanner();
       wait.Until(webDriver =>
               webDriver.FindElement(
                   By.XPath("//div[@data-testid='country-dropdown']/descendant::div[contains(@class,'dropdown__control')]"))) 
           .Click();
       var input = wait.Until(webDriver =>
           {
               var el =webDriver.FindElement(By.CssSelector("input[aria-label='Choose your country']:not([disabled])"));
               return el.GetAttribute("aria-expanded") == "true" ? el : null;
               
           }
           );
      
       input.SendKeys(country);
       input.SendKeys(Keys.Enter);
        var remote =wait.Until(webDriver => webDriver.FindElement(By.XPath("//label[contains(.,'Remote')]"))); 
       remote.Click();
       wait.Until(webDriver=>webDriver.FindElement(By.CssSelector("[data-testid='search-input']"))).SendKeys(programLanguage);
       wait.Until(d=>d.FindElement(By.Name("submit_search_box_button"))).Click();
       var link = wait.Until(d=>d.FindElement(By.XPath("(//a[@data-testid='job-card-link'])[last()]")));
       new Actions(_driver).MoveToElement(link).Click().Perform();
       var apply = wait.Until(d => d.FindElements(By.TagName("button")).FirstOrDefault(b=>b.Text.Contains("apply",StringComparison.OrdinalIgnoreCase) && b is { Displayed: true, Enabled: true }));
       apply.Click();
    }
    [TearDown]
    public void TearDown()
    {
        
     if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed
        && _driver is ITakesScreenshot shot)
    {
        shot.GetScreenshot().SaveAsFile($"{TestContext.CurrentContext.Test.Name}.png");
    }

    _driver?.Dispose();
   
        
    }


    private void CloseCookieBanner()
    {
        wait.Until(d => d.FindElements(By.Id("onetrust-accept-btn-handler"))
            .FirstOrDefault(e => e is { Displayed: true, Enabled: true })).Click();
        wait.Until(d => !d.FindElements(By.Id("onetrust-banner-sdk")).Any(e => e.Displayed));
        
    }
    

}