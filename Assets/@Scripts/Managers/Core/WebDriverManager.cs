using System;
using System.Collections;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using UnityEngine;

public class WebDriverManager : MonoBehaviour
{
	private const int SLEEP_TIME = 100;

	private ChromeDriverService _driverService;
	private ChromeOptions _options = new ChromeOptions();
	private ChromeDriver _driver;
	private WebDriverWait _wait;

	private const string script = @"
                            var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
                            var totalHeight = document.documentElement.scrollHeight;
                            var windowHeight = window.innerHeight;
                            var scrollPercentage = (scrollTop / (totalHeight - windowHeight));
                            if(scrollPercentage >= 0.9) {
                                window.scrollTo(0, 0);
                            }
                            return scrollPercentage;
                        ";

	private bool TryWaitForElementVisible(By by, out IWebElement element)
	{
		try
		{
			_wait.Until(ExpectedConditions.ElementIsVisible(by));
			element = _driver.FindElement(by);
			return true;
		}
		catch (WebDriverTimeoutException)
		{
			element = null;
			return false;
		}
	}

	public IEnumerator LoadSearchResults(string url, Action<string> onLoaded, Action<int> onCount, Action onComplate)
	{
		string driverPath = Path.Combine(Application.dataPath, "Plugins");
		_driverService = ChromeDriverService.CreateDefaultService(driverPath);
		_driverService.HideCommandPromptWindow = true;

		_options.AddArgument("--ignore-certificate-errors");
		_options.AddArgument("--disable-gpu");
		_options.AddArgument("--window-position=-32000,-32000"); // 창을 화면 밖으로 이동
		_options.PageLoadStrategy = PageLoadStrategy.None;

		yield return _driver = new ChromeDriver(_driverService, _options);
		yield return _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));

		_driver.Navigate().GoToUrl(url);

		IWebElement element;
		if (TryWaitForElementVisible(By.Id("html_encoder_div"), out element))
		{
			Debug.Log($"[WebDriverManager::LoadSearchResults]ParseHtmlEncoderDiv");
			yield return StartCoroutine(ParseHtmlEncoderDiv(onLoaded, onCount, onComplate));
		}
		else if (TryWaitForElementVisible(By.ClassName("view-padding"), out element))
		{
			Debug.Log($"[WebDriverManager::LoadSearchResults]ParseViewPadding");
			yield return StartCoroutine(ParseViewPadding(onLoaded, onCount, onComplate));
		}
		else
		{
			Debug.Log($"[WebDriverManager::LoadSearchResults]view-padding Time Out");
			yield break;
		}
	}

	private IEnumerator ParseHtmlEncoderDiv(Action<string> onLoaded, Action<int> onCount, Action onComplate)
	{
		((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, 0);");

		var divElement = _driver.FindElements(By.Id("html_encoder_div"));

		var imgElements = divElement[0].FindElements(By.TagName("img"));

		if (imgElements.Count == 0)
		{
			Debug.Log("이미지가 없습니다.");
			_driver.Quit();
			_driver = null;
		}

		int count = 0;
		for (int searchingIndex = 0; searchingIndex < imgElements.Count; ++searchingIndex)
		{
			string src = imgElements[searchingIndex].GetAttribute("src").ToString();
			if (src.Contains("loading"))
			{
				((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, document.body.scrollHeight * 0.1);");

				IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;

				double scrollPercentage = (double)jsExecutor.ExecuteScript(script);

				yield return new WaitForSeconds(0.1f);
				searchingIndex--;

				continue;
			}

			if (src.Contains("img1"))
				continue;

			onCount?.Invoke(++count);
			Debug.Log($"[WebDriverManager::ParseViewPadding] {src} : {count}");
			onLoaded?.Invoke(src);
		}

		Debug.Log("웹툰 파싱이 완료되었습니다.");
		onComplate?.Invoke();
		Clear();
	}

	private IEnumerator ParseViewPadding(Action<string> onLoaded, Action<int> onCount, Action onComplate)
	{
		((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, 0);");

		var followingDivs = _driver.FindElements(By.XPath("//div[@class='view-img']/following-sibling::div"));
		
		for (int i = 0; i < followingDivs.Count; i++)
		{
			var ele = followingDivs[i].FindElements(By.TagName("img"));

			int count = 0;
			for (int searchingIndex = 0; searchingIndex < ele.Count; searchingIndex++)
			{
				string src = ele[searchingIndex].GetAttribute("src");
				if (src.Contains("loading"))
				{
					((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, document.body.scrollHeight * 0.02);");

					IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;

					double scrollPercentage = (double)jsExecutor.ExecuteScript(script);

					yield return new WaitForSeconds(0.1f);

					searchingIndex--;
					continue;
				}

				if (src.Contains("img1"))
					continue;

				onCount?.Invoke(++count);
				Debug.Log($"[WebDriverManager::ParseViewPadding] {src} : {count}");
				onLoaded?.Invoke(src);
			}
		}

		Debug.Log("웹툰 파싱이 완료되었습니다.");
		onComplate?.Invoke();
		Clear();
	}

	public void Clear()
	{
		_driver?.Quit();
		_driver?.Dispose();
	}
}
