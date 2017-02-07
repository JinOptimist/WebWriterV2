using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;

namespace ConsoleApplication1
{
    class Program
    {
        static string rpgBaseUrl = "http://martin-s.by/AngularRoute/listQuest";

        //static string secondBefor = "stateType-60072-Value";
        static void Main(string[] args)
        {
            //var towerId = "quest-32";
            //var sevenMinutesId = "quest-33";
            //var kgbId = "quest-34";

            var list = new List<string> { "quest-80026", "quest-80027",  "quest-100030" };

            var driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            //driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));

            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(rpgBaseUrl);
            var questBlockIds = driver.FindElementsByCssSelector(".quest-block").ToList()
                .Select(x=> x.GetAttribute("id")).ToList();

            questBlockIds.ForEach(x => MyTest(driver, x));

            Thread.Sleep(1 * 1000);
            driver.Quit();
        }

        static void MyTest(IWebDriver driver, string questId)
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
