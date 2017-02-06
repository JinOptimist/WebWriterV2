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

        static string secondBefor = "stateType-60072-Value";
        static void Main(string[] args)
        {
            var towerId = "quest-32";
            var sevenMinutesId = "quest-33";
            var kgbId = "quest-34";
            var driver = new ChromeDriver();

            MyTest(driver, towerId);
            driver.Manage().Window.Maximize();
            MyTest(driver, sevenMinutesId);

            //MyTest(driver, kgbId);

            Thread.Sleep(1 * 1000);
            driver.Quit();
        }

        static void MyTest(IWebDriver driver, string questName)
        {
            driver.Navigate().GoToUrl(rpgBaseUrl);


            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));

            //driver.FindElements(By.CssSelector("." + questName + " div"))[0].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id(questName)).Click();
            //var sec = driver.FindElement(By.Id(secondBefor)).Text;

            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(200);
                driver.FindElements(By.TagName("li"))[0].Click();
            }
        }
    }
}
