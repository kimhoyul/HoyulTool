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

        // ����� ���ø����̼ǿ����� WebDriver ���
        var driverPath = Path.Combine(Application.dataPath, "Plugins"); // Windows ����
                                                                        // macOS �Ǵ� �ٸ� �÷����� ��� ��ΰ� �޶��� �� ����

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
