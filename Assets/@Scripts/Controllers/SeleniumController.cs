using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V121.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V121.Network;
using UnityEngine;
using System.IO;
using OpenQA.Selenium;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;

public class SeleniumController : MonoBehaviour
{
	[SerializeField] private Transform itemPanel;

	private ChromeOptions _options = new ChromeOptions();
	private ChromeDriver _driver = null;
	private IDevToolsSession _session = null;
	private DevToolsSessionDomains _domains = null;
	private ChromeDriverService _driverService = ChromeDriverService.CreateDefaultService(Path.Combine(Application.dataPath, "Plugins"));

	private const string script = @"
    var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    var totalHeight = document.documentElement.scrollHeight;
    var windowHeight = window.innerHeight;
    var scrollPercentage = (scrollTop / (totalHeight - windowHeight));
    if(scrollPercentage >= 0.9) {
        window.scrollTo(0, 0);
    }";
	
	private string _baseUrl = null;
	private string _parseUrl = null;
	private List<string> _imageUrls = new List<string>();
	private Dictionary<string, string> _imageDic = new Dictionary<string, string>();

	private bool init = false;

	public IEnumerator StartSelenium(string url, Action<WebtoonInfo> webtoonInfoCallback, Action<int> itemCountCallback, Action<WebtoonResource> complateCallback)
	{
		if (_baseUrl == null)
		{
			int lastIndex = url.LastIndexOf('/');
			if (lastIndex != -1)
			{
				_baseUrl = url.Substring(1, lastIndex);
				_parseUrl = url;
			}
			else
			{
				Debug.Log("URL 형식이 올바르지 않습니다.");
				yield break;
			}
		}
		else if (url.Contains(_baseUrl) == false)
		{
			_parseUrl = _baseUrl + url;
		}

		ClearLoadedResources();

		if (init == false)
		{
			Init();
		}

		yield return StartCoroutine(WaitForDriverReady(webtoonInfoCallback, itemCountCallback, complateCallback));
	}

	private void ClearLoadedResources()
	{
		_imageUrls.Clear();
		_imageDic.Clear();

		foreach (Transform child in itemPanel)
		{
			child.gameObject.SetActive(false);
		}
	}

	private void Init()
	{
		_driverService.HideCommandPromptWindow = true;
		_options.AddArgument("--ignore-certificate-errors");
		_options.AddArgument("--disable-gpu");
		//_options.AddArgument("--window-position=-32000,-32000"); // 창을 화면 밖으로 이동
		_options.PageLoadStrategy = PageLoadStrategy.None;

		Task.Run(() =>
		{
			_driver = new ChromeDriver(_driverService, _options);
			_session = _driver.GetDevToolsSession();
			_domains = _session.GetVersionSpecificDomains<DevToolsSessionDomains>();
			_domains.Network.Enable(new Network.EnableCommandSettings());
			_domains.Network.ResponseReceived += OnResponseReceived;
		});

		init = true;
	}

	private IEnumerator WaitForDriverReady(Action<WebtoonInfo> webtoonInfoCallback, Action<int> itemCountCallback, Action<WebtoonResource> complateCallback)
	{
		Managers.Loading.StartLoading(transform.parent);
		while (_driver == null || _session == null || _domains == null)
		{
			yield return new WaitForSeconds(0.1f);
		}

		_driver.Navigate().GoToUrl(_parseUrl);

		while (_driver.Url != _parseUrl)
		{
			yield return new WaitForSeconds(0.1f);
		}

		StartCoroutine(SetTitleTextCoroutine(webtoonInfoCallback));

        while (true)
		{
			_imageUrls.Clear();
			bool isPageLoadComplete = false;
			while (isPageLoadComplete == false)
			{
				var html_encoder_div = _driver.FindElements(By.Id("html_encoder_div"));
				if (html_encoder_div.Count == 0)
				{
					var view_padding = _driver.FindElements(By.ClassName("view-padding"));
					if (view_padding.Count == 0)
					{
						yield return new WaitForSeconds(0.1f);
					}
					else
					{
						isPageLoadComplete = true;
						yield return StartCoroutine(ParseWebtoon(By.XPath("//div[@class='view-img']/following-sibling::div")));
					}
				}
				else
				{
					isPageLoadComplete = true;
					yield return StartCoroutine(ParseWebtoon(By.Id("html_encoder_div")));
				}
			}

			if (_imageUrls.Count == _imageDic.Count)
			{
				itemCountCallback.Invoke(_imageUrls.Count);
				Debug.Log("웹툰 파싱이 완료되었습니다.");
				break;
			}

			_driver.Navigate().Refresh();
		}

		((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, 0);");
		yield return new WaitForSeconds(0.5f);

		WebtoonResource webtoonResource = new WebtoonResource();
		webtoonResource._imageUrls = _imageUrls;
		webtoonResource._imageDic = _imageDic;
		complateCallback.Invoke(webtoonResource);

		Managers.Loading.StopLoadng();
	}

    private IEnumerator SetTitleTextCoroutine(Action<WebtoonInfo> webtoonInfoCallback)
	{
		while (true)
		{
            var title = _driver.FindElements(By.XPath("//*[@id=\"at-main\"]/div[4]/section/article/div[1]/div/div[2]/div"));
            var pageIndicator = _driver.FindElements(By.XPath("//*[@id=\"at-main\"]/div[4]/section/article/div[1]/div/div[2]/div/span"));
            var selectElements = _driver.FindElements(By.Name("wr_id"));
            if (title.Count != 0 && pageIndicator.Count != 0 && selectElements.Count != 0)
            {
                WebtoonInfo webtoonInfo = new WebtoonInfo();
                webtoonInfo.title = title[0].GetAttribute("title");
                webtoonInfo.pageIndicator = pageIndicator[0].Text;

                var selectOptions = selectElements[0].FindElements(By.TagName("option"));
                foreach (var option in selectOptions)
                {
                    webtoonInfo.navItems.Add(option.Text, option.GetAttribute("value"));
                }

                webtoonInfoCallback.Invoke(webtoonInfo);
                yield break;
            }
        }
	}

	private IEnumerator ParseWebtoon(By findElement)
	{
		new Actions(_driver).ScrollByAmount(0, 150).Perform();

		var elements = _driver.FindElements(findElement);

		for (int i = 0; i < elements.Count; i++)
		{
			var element = elements[i].FindElements(By.TagName("img"));

			yield return WaitLoading(element);
		}
	}

	private IEnumerator WaitLoading(ReadOnlyCollection<IWebElement> element)
	{
		for (int searchingIndex = 0; searchingIndex < element.Count; searchingIndex++)
		{
			string src = element[searchingIndex].GetAttribute("src");
			if (src.Contains("loading"))
			{
				((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, document.body.scrollHeight * 0.02);");

				IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;

				jsExecutor.ExecuteScript(script);

				yield return new WaitForSeconds(0.1f);

				searchingIndex--;
				continue;
			}

			if (src.Contains("img1"))
				continue;

			_imageUrls.Add(src);
		}
	}

	private async void OnResponseReceived(object sender, Network.ResponseReceivedEventArgs e)
	{
		if (e.Response.Url.Contains("img2"))
		{
			var responseBody = await _domains.Network.GetResponseBody(new Network.GetResponseBodyCommandSettings
			{
				RequestId = e.RequestId
			});

			MainThreadDispatcher.Enqueue(() =>
			{
				_imageDic[e.Response.Url] = responseBody.Body;
			});
		}
	}

	private void Clear()
	{
		if (_driver != null)
		{
			_driver.Quit(); 
			_driver.Dispose(); 
			_driver = null;
		}

		_session = null;
		_domains = null;
	}

	private void OnDestroy()
	{
		Clear();
	}
}
