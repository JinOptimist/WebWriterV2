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
    public class LoginTest
    {
        public static string rpgBaseUrl = Properties.Settings.Default.BaseAppUrl;

        public static Random random = new Random();

        [Test]
        public void User()
        {
            
        }

        private void CreateUser()
        {

        }
    }
}
