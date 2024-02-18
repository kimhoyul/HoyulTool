using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
        {
            Debug.Log($"�ε���... {key} ({count}/{totalCount})");

            if (count == totalCount)
            {
				Debug.Log($"�ε�Ϸ�");

                StartLoaded();
			}
		});
    }

    void StartLoaded()
    {
        GameObject titleBar = Managers.Resource.Instantiate("UI_TitleBar.prefab");
        titleBar.name = "@TitleBar";
	}


}
