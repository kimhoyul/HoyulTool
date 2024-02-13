using System;
using UnityEngine;

public class UI_Content : MonoBehaviour
{
    void Start()
    {
        Managers.Game.OnToolStateChanged += SetContentPanelItem;
    }

    void SetContentPanelItem(Define.ToolState menubarState)
    {
        switch (menubarState)
        {
            case Define.ToolState.Unkown:
				break;
            case Define.ToolState.Downloader:
                ShowDownLoaderContent();
                break;
            case Define.ToolState.Setting:
                break;
        }
    }

	private void ShowDownLoaderContent()
	{
        Utils.FindChild(gameObject, "UI_SearchBar", true).SetActive(true);
	}

	private void OnDestroy()
	{
		Managers.Game.OnToolStateChanged -= SetContentPanelItem;
	}
}
