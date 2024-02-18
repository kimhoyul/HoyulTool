using SFB;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SearchedItem : MonoBehaviour
{
	[SerializeField] private TMP_Text ItemCountText;
	[SerializeField] private Transform ItemPanel;
	[SerializeField] private RectTransform SearchedItemBG;
	[SerializeField] private Button SelectAllButton;
	[SerializeField] private Button DownloadButton;

	private List<int> _selectedItem = new List<int>();
	private Dictionary<int, GameObject> _imageSourceUrls = new Dictionary<int, GameObject>();

	private void Start()
	{
		SelectAllButton.onClick.AddListener(() => SelectAll());
		DownloadButton.onClick.AddListener(() => Download());
				
		for ( int i = 0; i < ItemPanel.childCount; i++)
		{
			int index = i;
			_imageSourceUrls.Add(i, ItemPanel.GetChild(i).gameObject);
			ItemPanel.GetChild(i).GetComponent<Button>().onClick.AddListener(() => SelectOne(index));
		}

		SetItemCountText(ItemPanel.childCount);
	}

	public void SetItemCountText(int count)
	{
		ItemCountText.text = $"{count} item";
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
		var path = StandaloneFileBrowser.OpenFolderPanel("", "", false);

		if (string.IsNullOrEmpty(path[0]))
			return;

        for(int i = 0; i < _selectedItem.Count; i++)
        {
			string fullPath = path[0] + $"\\{i + 1}.jpg";
			GameObject go = _imageSourceUrls[_selectedItem[i]];
			Texture2D texture2D = go.GetComponent<RawImage>().texture as Texture2D;

			byte[] _textureBytes;
			_textureBytes = texture2D.EncodeToJPG();
            File.WriteAllBytes(fullPath, _textureBytes);
        }
	}
}