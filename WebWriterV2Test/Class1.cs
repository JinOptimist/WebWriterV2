using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebWriterV2Test
{
    public class Class1
    {
        public void Test()
        {
            IWebDriver driver = new ChromeDriver();
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            string title = (string)js.ExecuteScript("return document.title");
        }
    }
}
