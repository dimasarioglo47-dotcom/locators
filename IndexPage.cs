using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace JobSearchAutomation.Tests;

public class IndexPage
{
    private string BaseUrl { get; }
    private readonly IWebDriver _driver;
    private const int DefaultWaitSeconds = 10;
    public  WebDriverWait Wait { get; }

    public IndexPage(IWebDriver driver)
    {
        _driver = driver;
        _driver.Manage().Window.Maximize();
        var config = new ConfigurationBuilder() .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        BaseUrl = config["BaseUrl"] ??
                  throw new InvalidOperationException("BaseUrl is not set");
        int waitSeconds = int.TryParse(config["Seconds"], out var result) ? result : DefaultWaitSeconds;
        Wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitSeconds));
    }

    public IndexPage Open()
    {
        _driver.Url = BaseUrl;
        return this;


    }
}