using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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

	private List<int> _selectedItem = new List<int>();
	private Dictionary<int, GameObject> _imageSourceUrls = new Dictionary<int, GameObject>();
	private int index = 0;

	List<Texture2D> _downloadImages = new List<Texture2D>();

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

		_imageSourceUrls.Add(i, go);

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
		if (_selectedItem.Contains(index))
		{
			Utils.FindChild(_imageSourceUrls[index], "Check", true).SetActive(false);
			_selectedItem.Remove(index);
			return;
		}

		Utils.FindChild(_imageSourceUrls[index], "Check", true).SetActive(true);
		_selectedItem.Add(index);
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
			_downloadImages.Add(((DownloadHandlerTexture)request.downloadHandler).texture);
		}
	}

	private void SelectAll()
	{
		_selectedItem.Clear();

		for (int i = 0; i < ItemPanel.childCount; i++)
		{
			Utils.FindChild(ItemPanel.GetChild(i).gameObject, "Check", true).SetActive(true);
			_selectedItem.Add(i);
		}
	}

	private void Download()
	{
		// TODO : 파일 이름 중복 체크
		// TODO : 폴더만 선택할지, 폴더와 파일명 지정하게 할지 선택하기
		
		var path = StandaloneFileBrowser.SaveFilePanel("저장할 장소를 선택후 저장할 파일의 시작 번호를 입력해 주세요", "", "1", "jpg");

		switch (IsValidName(path))
		{
			case Define.NameValidationStatus.Empty:
				break;
			case Define.NameValidationStatus.MultipleNumbers:
				Download();
				return;
			case Define.NameValidationStatus.NoneNumber:

				break;
			case Define.NameValidationStatus.Valid:
				foreach (int index in _selectedItem)
				{
					byte[] _textureBytes;
					_textureBytes = _downloadImages[index].EncodeToPNG();
					File.WriteAllBytes(path, _textureBytes);
				}
				break;
		}
		//if (IsValidName(path))
		//{
		//	foreach (int index in _selectedItem)
		//	{
		//		byte[] _textureBytes;
		//		_textureBytes = _downloadImages[index].EncodeToPNG();
		//		File.WriteAllBytes(path, _textureBytes);
		//	}
		//}
		//else
		//{

		//}
	}

	private Define.NameValidationStatus IsValidName(string path)
	{
		if (string.IsNullOrEmpty(path))
			return Define.NameValidationStatus.Empty;

		string fileName = Path.GetFileNameWithoutExtension(path);

		if (Utils.ExtractNumbersFromString(fileName).Count > 1)
			return Define.NameValidationStatus.MultipleNumbers;

		else if (Utils.ExtractNumbersFromString(fileName).Count == 0)
			return Define.NameValidationStatus.Valid;

		else
			return Define.NameValidationStatus.NoneNumber;
	}

	
}