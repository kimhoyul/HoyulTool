using SFB;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SearchedItem : MonoBehaviour
{
	[SerializeField] private Button SelectAllButton;
	[SerializeField] private Button DownloadButton;

	private bool isActive = false;
	private List<int> _selectedItem = new List<int>();
	private Dictionary<int, GameObject> _imageSourceUrls = new Dictionary<int, GameObject>();

	private void Start()
	{
		//SelectAllButton.onClick.AddListener(() => ChangeSelectStateAll());
		//DownloadButton.onClick.AddListener(() => Download());
	}

	public void SetSourceUrls()
	{
		_imageSourceUrls.Clear();
		_selectedItem.Clear();

		for (int i = 0; i < transform.childCount; i++)
		{
			int index = i;
			_imageSourceUrls.Add(i, transform.GetChild(i).gameObject);
			transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
			transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => SelectOne(index));
		}
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

    private void ChangeSelectStateAll()
	{
		isActive = !isActive;

		_selectedItem.Clear();

		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.activeSelf == false)
				continue;

			Utils.FindChild(transform.GetChild(i).gameObject, "Check", true).SetActive(isActive);
            if (isActive == true)
            {
				_selectedItem.Add(i);
			}
		}
	}

	private void Download()
	{
		var path = StandaloneFileBrowser.OpenFolderPanel("", "", false);

		if (path.Length == 0)
			return;

		int addCount = 1;
		for (int i = 0; i < _selectedItem.Count; i++)
        {
			string fullPath = path[0] + $"\\{i + addCount}.jpg";

			while(true)
			{
				if (File.Exists(fullPath))
				{
					++addCount;
					fullPath = path[0] + $"\\{i + addCount}.jpg";
				}
				else
				{
					break;
				}
			}

			GameObject go = _imageSourceUrls[_selectedItem[i]];
			Texture2D texture2D = go.GetComponent<RawImage>().texture as Texture2D;

			byte[] _textureBytes;
			_textureBytes = texture2D.EncodeToJPG();
            File.WriteAllBytes(fullPath, _textureBytes);
        }

		ChangeSelectStateAll();
		Debug.Log("다운로드가 완료 되었습니다.");
	}
}