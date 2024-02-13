using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;
using System.Collections.Generic;

public class ResourceManager
{
	Dictionary<string, Object> _resouces = new Dictionary<string, Object>();

	// 리소스 로드
	public T Load<T>(string key) where T : Object
	{
		// 이미 로드된 리소스가 있다면 리턴
		if (_resouces.TryGetValue(key, out Object resouce))
			return resouce as T;

		return null;
	}

	// 리소스 인스턴스화
	public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
	{
		GameObject prefab = Load<GameObject>($"{key}");
		if (prefab == null)
		{
			Debug.LogError($"Failed to prefab : {key}");
			return null;
		}

		GameObject go = Object.Instantiate(prefab, parent);
		go.name = prefab.name;
		return go;
	}

	// 리소스 삭제
	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

		Object.Destroy(go);
	}

	#region Addressables
	// Addressables name 기준으로 리소스 로드
	public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object
	{
		// 이미 로드된 리소스가 있다면 콜백 호출 후 리턴
		if (_resouces.TryGetValue(key, out Object resouce))
		{
			callback?.Invoke(resouce as T);	
			return;
		}
		
		// 리소스 비동기 로드
		AsyncOperationHandle<T> asyncOperation = Addressables.LoadAssetAsync<T>(key);
		asyncOperation.Completed += (op) =>
		{
			_resouces.Add(key, op.Result);
			callback?.Invoke(op.Result);
		};
	}

	// Addressables Label 기준으로 전체 리소스 로드
	public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : Object
	{
		// Addressables Label 에 갖고있는 Addressables name 을 비동기로 로드
		var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
		opHandle.Completed += (op) =>
		{
			int loadCount = 0;
			int totalCount = op.Result.Count;

			// 가져온 Addressables name 순회 
			foreach (var result in op.Result)
			{
				// 리소스 비동기 로드
				LoadAsync<T>(result.PrimaryKey, (obj) =>
				{
					loadCount++;
					callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
				});
			}
		};
	}
	#endregion
}