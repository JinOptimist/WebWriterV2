using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace WebWriterV2Test
{
    [TestFixture]
    public class QuestTest
    {
        public static string rpgBaseUrl = "http://martin-s.by/AngularRoute/listQuest";

        [Test]
        public void Test()
        {
            //IWebDriver driver = new ChromeDriver();
            //IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            //string title = (string)js.ExecuteScript("return document.title");

            var drivers = new List<IWebDriver>();
            drivers.Add(new ChromeDriver());
            drivers.Add(new FirefoxDriver());
            drivers.Add(new InternetExplorerDriver());

            drivers.ForEach(RunTestWithOneDriver);

            Assert.AreEqual(10, 10);
        }

        private void RunTestWithOneDriver(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(rpgBaseUrl);
            var questBlockIds = driver.FindElements(By.ClassName(".quest-block")).ToList()
                .Select(x => x.GetAttribute("id")).ToList();

            questBlockIds.ForEach(x => CheckQuest(driver, x));

            driver.Quit();
        }

        private void CheckQuest(IWebDriver driver, string questId)
        {
            driver.Navigate().GoToUrl(rpgBaseUrl);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            Thread.Sleep(500);
            driver.FindElement(By.Id(questId)).Click();

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
            var liTags = driver.FindElements(By.CssSelector("li.wayLink"));
            while (liTags.Count > 0)
            {
                Thread.Sleep(100);
                liTags[0].Click();
                liTags = driver.FindElements(By.CssSelector("li.wayLink"));
            }
        }
    }
}
