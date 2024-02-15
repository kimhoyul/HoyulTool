<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
using SFB;
=======
=======
using NUnit.Framework;
>>>>>>> parent of 46896cb (download Î∞è FileBrowser Ï†úÏûë ÏãúÏûë)
using System;
>>>>>>> parent of 99e2f5b (sfb ÏÇ¨Ïö©)
=======
>>>>>>> parent of 1a1fac3 (download Ï†úÏûëÏ§ë)
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.HostingServices;
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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
		// TODO : ∆ƒ¿œ ¿Ã∏ß ¡ﬂ∫π √º≈©
		// TODO : ∆˙¥ı∏∏ º±≈√«“¡ˆ, ∆˙¥ıøÕ ∆ƒ¿œ∏Ì ¡ˆ¡§«œ∞‘ «“¡ˆ º±≈√«œ±‚
		
		var path = StandaloneFileBrowser.SaveFilePanel("¿˙¿Â«“ ¿Âº“∏¶ º±≈√»ƒ ¿˙¿Â«“ ∆ƒ¿œ¿« Ω√¿€ π¯»£∏¶ ¿‘∑¬«ÿ ¡÷ººø‰", "", "1", "jpg");

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
=======
		//System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
		//if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		//{
			//string folderPath = folderDialog.SelectedPath;
			//string initialFileName = Microsoft.VisualBasic.Interaction.InputBox("∆ƒ¿œ¿« Ω√¿€ π¯»£∏¶ ¿‘∑¬«œººø‰.:", "Ω√¿€ π¯»£", "1");
			//if (!int.TryParse(initialFileName, out int fileNameNumber))
			//{
			//	MessageBox.Show("¿Ã∏ß¿Ã º˝¿⁄ «¸Ωƒ¿Ã æ∆¥’¥œ¥Ÿ. ¥ŸΩ√ ¿‘∑¬«ÿ ¡÷ººø‰.");
			//	return;
			//}

			//ResetProgressBar(imageUrls.Length);

			//UpdateControlText("0%", lblPercent);


			//for (int i = 0; i < imageUrls.Length; i++)
			//{
			//	string fileName = $"{fileNameNumber++}{System.IO.Path.GetExtension(imageUrls[i])}";
			//	fileName = fileName.Replace("\r", "");
			//	await DownloadImageAsync(imageUrls[i], folderPath, fileName);
			//	progressBar.Value++;
			//	UpdateControlText($"{(int)((double)progressBar.Value / progressBar.Maximum * 100)}%", lblPercent);
			//	UpdateControlText($"¥ŸøÓ∑ŒµÂ ¡ﬂ...({i} / {imageUrls.Length})", lblStatus);
			//}

			//UpdateControlText("¥ŸøÓ∑ŒµÂ øœ∑·", lblStatus);
			//MessageBox.Show("Download completed!");
			//System.Diagnostics.Process.Start("explorer.exe", folderPath);
	}
}

	//private async Task DownloadImageAsync(string imageUrl, string folderPath, string fileName)
	//{
	//	try
	//	{
	//		var response = await client.GetAsync(imageUrl);
	//		response.EnsureSuccessStatusCode();
	//		var imageBytes = await response.Content.ReadAsByteArrayAsync();
	//		string filePath = System.IO.Path.Combine(folderPath, fileName);
	//		System.IO.File.WriteAllBytes(filePath, imageBytes);
	//	}
	//	catch (Exception ex)
	//	{
	//		MessageBox.Show($"Error downloading {imageUrl}: {ex.Message}");
	//	}
	//}
>>>>>>> parent of 99e2f5b (sfb ÏÇ¨Ïö©)
=======
      
    }
}
>>>>>>> parent of 1a1fac3 (download Ï†úÏûëÏ§ë)
=======
		
	}
}
>>>>>>> parent of 46896cb (download Î∞è FileBrowser Ï†úÏûë ÏãúÏûë)
