using System.Collections;
using UnityEngine;

public class LoadingManager
{
	public GameObject loadingUI;

	public void Init()
	{
		
	}

	public void StartLoading(Transform parent)
	{
		loadingUI = Managers.Resource.Instantiate("UI_Loading.prefab", parent);
	}

	public void StopLoadng()
	{
		Managers.Resource.Destroy(loadingUI);
	}
}
