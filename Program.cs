using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Interfaces;
[TestFixture]
class Test
{

    private IWebDriver _driver = null!;
    private  string  _baseUrl=null!;
    private WebDriverWait _wait = null!;
    

    [SetUp]
    public void SetUp()
    {

        _baseUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["BaseUrl"] ??
                   throw new InvalidOperationException("BaseUrl is not set");

        _driver = new ChromeDriver();
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _driver.Url=_baseUrl;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero; 
        _driver.Manage().Window.Maximize();
        _wait = new WebDriverWait(_driver,TimeSpan.FromSeconds(15));
        _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
    

    }
    
    [Test]
    [TestCase("C#", "All Locations")]
    [TestCase("Python", "Ukraine")]
    public void RemoteJobSearch_ByCountryAndLanguage_LeadsToApplyForm(string programLanguage,string country)
    {
        
        CloseCookieBanner();
        _wait.Until(webDriver=>webDriver.FindElement(By.LinkText("Careers"))).Click(); 
       _wait.Until(webDriver=>webDriver.FindElement(By.ClassName("button-body"))).Click();
       CloseCookieBanner();
       SelectCountry(country);
      
        var remote =_wait.Until(webDriver => webDriver.FindElement(By.XPath("//label[contains(.,'Remote')]"))); 
       remote.Click();
       _wait.Until(webDriver=>webDriver.FindElement(By.CssSelector("[data-testid='search-input']"))).SendKeys(programLanguage);
       _wait.Until(d=>d.FindElement(By.Name("submit_search_box_button"))).Click();
       var link = _wait.Until(d=>d.FindElement(By.XPath("(//a[@data-testid='job-card-link'])[last()]")));
       new Actions(_driver).MoveToElement(link).Click().Perform();
       var apply = _wait.Until(d => d.FindElements(By.TagName("button")).FirstOrDefault(b=>b.Text.Contains("apply",StringComparison.OrdinalIgnoreCase) && b is { Displayed: true, Enabled: true }));
       apply.Click();
       var pageText = _wait.Until(d =>
       {
           var text = d.FindElement(By.TagName("body")).Text;
           return text.Contains(programLanguage, StringComparison.OrdinalIgnoreCase) ? text : null;
       });

       Assert.That(pageText, Does.Contain(programLanguage).IgnoreCase,
           $"'{programLanguage}' not found on the job page");
    }
    [TearDown]
    public void TearDown()
    {
        
     if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed
        && _driver is ITakesScreenshot shot)
    {
        shot.GetScreenshot().SaveAsFile($"{TestContext.CurrentContext.Test.Name}.png");
    }

    _driver.Dispose();
   
        
    }


    private void CloseCookieBanner()
    {
        _wait.Until(d => d.FindElements(By.Id("onetrust-accept-btn-handler"))
            .FirstOrDefault(e => e is { Displayed: true, Enabled: true })).Click();
        _wait.Until(d => !d.FindElements(By.Id("onetrust-banner-sdk")).Any(e => e.Displayed));
        
    }

    private void SelectCountry(string country)
    {
        _wait.Until(webDriver =>
                webDriver.FindElement(
                    By.XPath("//div[@data-testid='country-dropdown']/descendant::div[contains(@class,'dropdown__control')]"))) 
            .Click();
        var input = _wait.Until(webDriver =>
            {
                var el =webDriver.FindElement(By.CssSelector("input[aria-label='Choose your country']:not([disabled])"));
                return el.GetAttribute("aria-expanded") == "true" ? el : null;
               
            }
        );
        input.Clear();
        input.SendKeys(country);
        input.SendKeys(Keys.Enter);
    }

    // public IWebElement WaitForDisplayed(By by)
    // {
    //     var el =_wait.Until(d => d.FindElements(by).FirstOrDefault(e=>e is { Displayed: true, Enabled: true }));
    //     return el ?? throw new WebDriverTimeoutException($"Element '{by}' was not displayed within {ExplicitWaitSeconds} seconds.");
    // }
    

}