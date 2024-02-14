using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI_SearchedItem : MonoBehaviour
{
    public TMP_Text ItemCountText;
	public Transform ItemPanel;
	public GameObject Content;
	public RectTransform SearchedItemBG;

	public Button SelectAllButton;
	public Button DownloadButton;

	private List<string> _selectedItem = new List<string>();
	private Dictionary<int, KeyValuePair<string, GameObject>> _imageSourceUrls = new Dictionary<int, KeyValuePair<string, GameObject>>();
	private int index = 0;

	private void Start()
	{
		SelectAllButton.onClick.AddListener(() => SelectAll());
		DownloadButton.onClick.AddListener(() => Download());
	}

	public void SetItemCountText(int count)
    {
		ItemCountText.text = $"{count} item";
	}

	private void SetContentPanelSize(float value)
	{
		Vector2 contentSizeDelta = Content.GetComponent<RectTransform>().sizeDelta;

		Content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSizeDelta.x, contentSizeDelta.y + value);
		LayoutRebuilder.ForceRebuildLayoutImmediate(SearchedItemBG);
	}

	public void OnImageLoaded(string sourceUrl)
	{
		int i = index;

		GameObject go = Managers.Resource.Instantiate("UI_LoadedImage.prefab", ItemPanel);
		go.GetComponent<Button>().onClick.AddListener(() => { SelectOne(i); });

		KeyValuePair<string, GameObject> pair = new KeyValuePair<string, GameObject>(sourceUrl, go);
		_imageSourceUrls.Add(i, pair);

		index++;

		if (ItemPanel.childCount % 4 == 1)
		{
			SetContentPanelSize(220);
		}
		RawImage rawImage = go.GetComponent<RawImage>();
		StartCoroutine(DownloadImage(sourceUrl, rawImage));
	}

	private void SelectOne(int index)
	{
		if (_selectedItem.Contains(_imageSourceUrls[index].Key))
		{
			Utils.FindChild(_imageSourceUrls[index].Value, "Check", true).SetActive(false);
			_selectedItem.Remove(_imageSourceUrls[index].Key);
			return;
		}

		Utils.FindChild(_imageSourceUrls[index].Value, "Check", true).SetActive(true);
		_selectedItem.Add(_imageSourceUrls[index].Key);
	}

	public void OnComplate()
	{
		SetContentPanelSize(80);
	}

	IEnumerator DownloadImage(string MediaUrl, RawImage rawImage)
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
		yield return request.SendWebRequest();

		if (request.result == UnityWebRequest.Result.ConnectionError)
			Debug.Log(request.error);
		else
		{
			rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
		}
	}

    private void SelectAll()
	{
		_selectedItem.Clear();

		for (int i = 0; i < ItemPanel.childCount; i++)
		{
			Utils.FindChild(ItemPanel.GetChild(i).gameObject, "Check", true).SetActive(true);
			_selectedItem.Add(_imageSourceUrls[i].Key);
		}
	}



	private void Download()
	{
      
    }
}