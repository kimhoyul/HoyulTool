using OpenQA.Selenium;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Downloder : MonoBehaviour
{
	[SerializeField] private Button _searchBarIcon;
	[SerializeField] private TMP_InputField _searchInputField;
	[SerializeField] private TMP_Text _titleText;
	[SerializeField] private TMP_Text _pageIndicatorText;
	[SerializeField] private TMP_Text _itemCountText;
	[SerializeField] private Transform _itemPanel;

	private SeleniumController _seleniumController;

	private void Start()
	{
		_searchBarIcon.onClick.AddListener(() => _searchInputField.ActivateInputField());
		_searchInputField.onSubmit.AddListener((inputValue) => HandleSearchSubmit(inputValue));

		_seleniumController = transform.GetComponent<SeleniumController>();
	}

	private void HandleSearchSubmit(string searchText)
	{
		if (!string.IsNullOrEmpty(searchText))
		{
			StartCoroutine(_seleniumController.StartSelenium(searchText, SetWebtoonInfo, SetItemCount, SetConvertAndDisplayImage));
		}
	}

	private void SetWebtoonInfo(WebtoonInfo webtoonInfo)
	{
		_titleText.text = webtoonInfo.title;
		_pageIndicatorText.text = $"{webtoonInfo.pageIndicator}";
	}

	private void SetItemCount(int count)
	{
		_itemCountText.text = $"{count} item";
	}

	private void SetConvertAndDisplayImage(WebtoonResource webtoonResource)
	{
		StartCoroutine(ConvertAndDisplayImageCoroutine(webtoonResource));
	}

	private IEnumerator ConvertAndDisplayImageCoroutine(WebtoonResource webtoonResource)
	{
		GameObject loadedImage;

		for (int index = 0; index < webtoonResource._imageUrls.Count; index++)
		{
			if (index < _itemPanel.childCount)
			{
				loadedImage = _itemPanel.GetChild(index).gameObject;
				loadedImage.SetActive(true);
			}
			else
			{
				loadedImage = Managers.Resource.Instantiate("UI_LoadedImage.prefab", _itemPanel);
			}

			yield return new WaitForSeconds(0.1f);

			RawImage raw = loadedImage.GetComponent<RawImage>();

			Texture2D texture = new Texture2D(1, 1);
			byte[] imageBytes;
			imageBytes = Convert.FromBase64String(webtoonResource._imageDic[webtoonResource._imageUrls[index]]);

			if (texture.LoadImage(imageBytes))
			{
				raw.texture = texture;
			}
			else
			{
				Debug.Log("Failed to load image");
			}
		}

		_itemPanel.gameObject.GetComponent<UI_SearchedItem>().SetSourceUrls();
	}
}
