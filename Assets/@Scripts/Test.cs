using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private ChromeDriverService _driverService = ChromeDriverService.CreateDefaultService();
    private ChromeOptions _options = new ChromeOptions();
#nullable enable
    private ChromeDriver? _driver = null;
#nullable disable
    public Button button;

    private void Start()
    {
        Debug.Log("Start");
        //fullScreenMode = FullScreenMode.Windowed;
        //refreshRate.numerator = 60;
        //Screen.SetResolution(300, 400, fullScreenMode, refreshRate);
        //button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        InitializeWebDriver();
    }

    private void InitializeWebDriver()
    {
        if (_driver != null)
            return;

        // 빌드된 애플리케이션에서의 WebDriver 경로
        var driverPath = Path.Combine(Application.dataPath, "Plugins"); // Windows 예시
                                                                        // macOS 또는 다른 플랫폼의 경우 경로가 달라질 수 있음

        _driverService = ChromeDriverService.CreateDefaultService(driverPath);
        _driverService.HideCommandPromptWindow = true;
        _options.AddArgument("--ignore-certificate-errors");
        _options.AddArgument("--disable-gpu");
        _driver = new ChromeDriver(_driverService, _options);
        _driver.Navigate().GoToUrl("https://www.google.com");
        ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, document.body.scrollHeight * 0.5);");
    }

    private void OnDestroy()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
