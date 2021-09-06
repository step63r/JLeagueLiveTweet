using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Services
{
    /// <summary>
    /// Webブラウザ列挙型
    /// </summary>
    public enum BrowserName
    {
        None,
        Chrome,
        Firefox,
        InternetExplorer,
        Edge,
        Safari
    }

    /// <summary>
    /// 各ブラウザーのウェブドライバーを作成するクラス
    /// </summary>
    public class WebDriverFactory
    {
        /// <summary>
        /// ウェブドライバーのインスタンスを生成する
        /// </summary>
        /// <param name="browserName">Webブラウザ</param>
        /// <returns>ウェブドライバー インスタンス</returns>
        public static IWebDriver CreateInstance(BrowserName browserName)
        {
            switch (browserName)
            {
                case BrowserName.None:
                    throw new ArgumentException(string.Format("Not Definition. BrowserName:{0}", browserName));

                case BrowserName.Chrome:
                    var chromeDriverService = ChromeDriverService.CreateDefaultService();
                    // コマンドプロンプトを非表示にする
                    chromeDriverService.HideCommandPromptWindow = true;
                    // ブラウザを非表示にする
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments(new List<string>() { "headless", "lang=ja" });
                    //chromeOptions.AddArguments(new List<string>() { "lang=ja" });
                    return new ChromeDriver(chromeDriverService, chromeOptions);

                case BrowserName.Firefox:
                    var driverService = FirefoxDriverService.CreateDefaultService();
                    driverService.FirefoxBinaryPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
                    driverService.HideCommandPromptWindow = true;
                    driverService.SuppressInitialDiagnosticInformation = true;
                    return new FirefoxDriver(driverService);

                case BrowserName.InternetExplorer:
                    return new InternetExplorerDriver();

                case BrowserName.Edge:
                    return new EdgeDriver();

                case BrowserName.Safari:
                    return new SafariDriver();

                default:
                    throw new ArgumentException(string.Format("Not Definition. BrowserName:{0}", browserName));
            }
        }
    }
}
