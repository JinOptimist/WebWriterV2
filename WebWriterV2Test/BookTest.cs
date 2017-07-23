using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;

namespace WebWriterV2Test
{
    [TestFixture]
    public class BookTest
    {
        public static string RpgBaseUrl = Properties.Settings.Default.BaseAppUrl;
        public static Random Random = new Random();

        [Test]
        public void BookOwner()
        {
            //TestRunner.RunTest(CheckAllBooks, TargetBrowser.ChromePlusIe);
        }

        [Test]
        public void BookThrough()
        {
            TestRunner.RunTest(CheckAllBooks, TargetBrowser.ChromePlusIe);
        }

        private void CheckAllBooks(IWebDriver driver)
        {
            var bookBlockIds = driver.FindElements(By.ClassName("book-block"))
                .Select(x => x.GetAttribute("id")).ToList();

            Assert.Greater(bookBlockIds.Count, 0, "Not found any book. I suppose it's bad signs");

            bookBlockIds.ForEach(x => CheckBook(driver, x));
        }

        private void CheckBook(IWebDriver driver, string bookId)
        {
            
            driver.Navigate().GoToUrl(RpgBaseUrl);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            Thread.Sleep(500);
            var book = driver.FindElement(By.Id(bookId));

            Assert.IsNotNull(book, $"Somebody try check book which does not exist. {nameof(bookId)} - {bookId}");
            Assert.IsTrue(book.Displayed, $"I can't see book {bookId}. There is exist, but not visible for user");

            book.Click();

            var counter = 0;
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));
            var liTags = driver.FindElements(By.CssSelector("li.wayLink"));
            while (liTags.Count > 0)
            {
                //Thread.Sleep(100);
                var wayNumber = Random.Next(liTags.Count);
                liTags[wayNumber].Click();
                liTags = driver.FindElements(By.CssSelector("li.wayLink"));
                counter++;
                if (counter > 100)
                {
                    Assert.Warn($"Possible infinity loop. We did {counter} choose and still can't find exit. qId - {bookId}");
                    return;
                }
            }

            var endButton = driver.FindElement(By.Id("end"));
            Assert.IsNotNull(endButton, $"List of ways empty and we haven't end button. qId - {bookId}");
            Assert.IsTrue(endButton.Displayed, $"List of ways empty and we have, but doesn't see end button. qId - {bookId}");
            endButton.Click();
        }
    }
}
