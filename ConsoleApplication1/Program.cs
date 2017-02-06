using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var rpgBaseUrl = "http://martin-s.by/AngularRoute/listQuest";
            var sevenMinutesId = "quest-33";
            var secondBefor = "stateType-60072-Value";
            IWebDriver driver = new ChromeDriver();

            try
            {
                driver.Navigate().GoToUrl(rpgBaseUrl);
                driver.Manage().Window.Maximize();

                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));

                driver.FindElement(By.Id(sevenMinutesId)).Click();
                //var sec = driver.FindElement(By.Id(secondBefor)).Text;

                for (int i = 0; i < 11; i++)
                {
                    Thread.Sleep(200);
                    driver.FindElements(By.TagName("li"))[0].Click();
                }
            }
            finally
            {
                Thread.Sleep(1 * 1000);
                driver.Quit();
            }
        }
    }
}
