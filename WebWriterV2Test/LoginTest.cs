using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace WebWriterV2Test
{
    [TestFixture]
    public class LoginTest
    {
        public static string RpgBaseUrl = Properties.Settings.Default.BaseAppUrl;
        public static Random Random = new Random();

        private static string RegisterLinkId = "register-lnk";
        private static string LoginLinkId = "login-lnk";
        private static string UsernameTextboxId = "username";
        private static string PasswordTextboxId = "password";
        private static string RegisterButtonId = "register-btn";
        private static string LoginButtonId = "login-btn";
        private static string ProfileLinkClass = "profile-lnk";
        private static string RemoveMyAccountLinkId = "removeMyAccount";

        [Test]
        public void User()
        {
            TestRunner.RunTest(CreateUser, TargetBrowser.ChromePlusIe);
        }

        private void CreateUser(IWebDriver driver)
        {
            var testUserName = $"test-username{Random.Next()}";
            var password = "1";

            /* Register new user */
            driver.FindElement(By.Id(RegisterLinkId)).Click();
            driver.FindElement(By.Id(UsernameTextboxId)).SendKeys(testUserName);
            driver.FindElement(By.Id(PasswordTextboxId)).SendKeys(password);
            driver.FindElement(By.Id(RegisterButtonId)).Click();

            /* go to profile and delete user */
            var profileLink = driver.FindElement(By.ClassName(ProfileLinkClass));
            Assert.NotNull(profileLink);
            profileLink.Click();
            driver.FindElement(By.Id(RemoveMyAccountLinkId)).Click();
            driver.SwitchTo().Alert().Accept();

            var registerLink = driver.FindElement(By.Id(RegisterLinkId));
            Assert.NotNull(registerLink);
            Assert.False(driver.PageSource.Contains(ProfileLinkClass));
        }
    }
}
