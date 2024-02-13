using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
        {
            Debug.Log($"로드중... {key} ({count}/{totalCount})");

            if (count == totalCount)
            {
				Debug.Log($"로드완료");

                StartLoaded();
			}
		});
	}

    void StartLoaded()
    {
        GameObject go = Managers.Resource.Instantiate("UI_TitleBar.prefab");
        go.name = "UI_TitleBar";
    }


}
