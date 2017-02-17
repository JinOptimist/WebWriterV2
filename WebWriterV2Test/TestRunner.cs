using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;


namespace WebWriterV2Test
{
    public class TestRunner
    {
        public static void RunTest(Action<IWebDriver> test, TargetBrowser target)
        {
            var drivers = GenerateDrivers(target);
            drivers.ForEach(x => test(x.Value));
        }

        public static List<Lazy<IWebDriver>> GenerateDrivers(TargetBrowser target)
        {
            var drivers = new List<Lazy<IWebDriver>>();

            if (target == TargetBrowser.ChromeOnly
                || target == TargetBrowser.ChromePlusIe
                || target == TargetBrowser.All)
            {
                drivers.Add(new Lazy<IWebDriver>(() => new ChromeDriver()));
            }

            if (target == TargetBrowser.IeOnly
                || target == TargetBrowser.ChromePlusIe
                || target == TargetBrowser.All)
            {
                drivers.Add(new Lazy<IWebDriver>(() => new InternetExplorerDriver()));
            }

            if (target == TargetBrowser.FireFoxOnly
                || target == TargetBrowser.All)
            {
                drivers.Add(new Lazy<IWebDriver>(() => new FirefoxDriver()));
            }

            return drivers;
        }
    }

    public enum TargetBrowser
    {
        ChromeOnly = 1,
        IeOnly = 2,
        FireFoxOnly = 3,
        All = 4,
        ChromePlusIe = 5
    }
}
