using System;
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
		//System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
		//if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		//{
			//string folderPath = folderDialog.SelectedPath;
			//string initialFileName = Microsoft.VisualBasic.Interaction.InputBox("파일의 시작 번호를 입력하세요.:", "시작 번호", "1");
			//if (!int.TryParse(initialFileName, out int fileNameNumber))
			//{
			//	MessageBox.Show("이름이 숫자 형식이 아닙니다. 다시 입력해 주세요.");
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
			//	UpdateControlText($"다운로드 중...({i} / {imageUrls.Length})", lblStatus);
			//}

			//UpdateControlText("다운로드 완료", lblStatus);
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
