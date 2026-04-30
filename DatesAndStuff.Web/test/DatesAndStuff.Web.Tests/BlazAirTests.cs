using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazAirTests
{
    private IWebDriver driver;
    private const string BlazDemoURL = "https://blazedemo.com";

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void TeardownTest()
    {
        try
        {
            driver.Quit();
            driver.Dispose();
        }
        catch (Exception)
        {
            // Ignore errors if unable to close the browser
        }
    }

    [Test]
    public void FlightSearch_MexicoCityToDublin_ShouldReturnAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl(BlazDemoURL);
        
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        // Select departure city - Mexico City
        var departureDropdown = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("fromPort")));
        var departureSelect = new OpenQA.Selenium.Support.UI.SelectElement(departureDropdown);
        departureSelect.SelectByValue("Mexico City");

        // Select destination city - Dublin
        var destinationDropdown = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("toPort")));
        var destinationSelect = new OpenQA.Selenium.Support.UI.SelectElement(destinationDropdown);
        destinationSelect.SelectByValue("Dublin");

        // Act
        var findFlightsButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@value='Find Flights']")));
        findFlightsButton.Click();

        // Wait for flight results table to load
        var flightsTable = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("//table[@class='table']//tbody//tr")));

        // Assert
        flightsTable.Should().HaveCountGreaterThanOrEqualTo(3, "there should be at least 3 flights between Mexico City and Dublin");
    }
}
