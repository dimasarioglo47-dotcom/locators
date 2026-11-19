using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Microsoft.Extensions.Configuration;
namespace JobSearchAutomation.Tests;
[TestFixture]

class RemoteJobSearchTests
{
    private int ExplicitWaitSeconds => int.TryParse(_jsonWaitSeconds,out var result) ? result : _waitSeconds;

    private IWebDriver _driver = null!;
    private  string  _baseUrl=null!;
    private WebDriverWait _wait = null!;
    private string? _jsonWaitSeconds;
    private int _waitSeconds;
    

    [SetUp]
    public void SetUp()
    {
        _waitSeconds = 10;
        var config = new ConfigurationBuilder() .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        _baseUrl = config["BaseUrl"] ??
                   throw new InvalidOperationException("BaseUrl is not set");
         _jsonWaitSeconds = config["Seconds"];

        _driver = new ChromeDriver();
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        _driver.Url=_baseUrl;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero; 
        _driver.Manage().Window.Maximize();
        _wait = new WebDriverWait(_driver,TimeSpan.FromSeconds(ExplicitWaitSeconds));
        _wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
    

    }
    
    [Test]
    [TestCase("C#", "All Locations")]
    [TestCase("Python", "Ukraine")]
    public void RemoteJobSearch_ByCountryAndLanguage_LeadsToApplyForm(string programLanguage,string country)
    {
        
        CloseCookieBanner();
        WaitForDisplayed(By.LinkText("Careers")).Click(); 
        WaitForDisplayed(By.PartialLinkText("START YOUR SEARCH")).Click();
       CloseCookieBanner();
       SelectCountry(country);
      
        var remote =_wait.Until(webDriver => webDriver.FindElement(By.XPath("//label[contains(.,'Remote')]"))); 
       remote.Click();
       _wait.Until(webDriver=>webDriver.FindElement(By.CssSelector("[data-testid='search-input']"))).SendKeys(programLanguage);
       WaitForDisplayed(By.Name("submit_search_box_button")).Click();
       var link = WaitForDisplayed(By.XPath("(//a[@data-testid='job-card-link'])[last()]"));
       new Actions(_driver).MoveToElement(link).Click().Perform();
       var apply = _wait.Until(d => d.FindElements(By.TagName("button"))
           .FirstOrDefault(b=>b.
               Text.Contains("apply",StringComparison.OrdinalIgnoreCase) && b is { Displayed: true, Enabled: true }));
       apply.Click();
      bool IsOnthePage = _wait.Until(d=>d.FindElement(By.TagName("body"))
      .Text.Contains(programLanguage,StringComparison.OrdinalIgnoreCase));
      Assert.That(IsOnthePage,Is.True);
    }
    [TearDown]
    public void TearDown()
    {
        _driver.Dispose();
    }


    private void CloseCookieBanner()
    {
        WaitForDisplayed(By.Id("onetrust-accept-btn-handler")).Click();
        _wait.Until(d => !d.FindElements(By.Id("onetrust-banner-sdk")).Any(e => e.Displayed));
        
    }

    private void SelectCountry(string country)
    {
        var countryDropdown = WaitForDisplayed(By.CssSelector("[data-testid='country-dropdown']"));
        var control = countryDropdown.FindElement(By.ClassName("dropdown__control"));
        control.Click();
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

    private IWebElement WaitForDisplayed(By by)
    {
        var el =_wait.Until(d => d.FindElements(by).FirstOrDefault(e=>e is { Displayed: true, Enabled: true }));
        return el ?? throw new WebDriverTimeoutException($"Element '{by}' was not displayed within {ExplicitWaitSeconds} seconds.");
    }
    

}