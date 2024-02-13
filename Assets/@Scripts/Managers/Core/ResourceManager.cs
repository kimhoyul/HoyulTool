using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;
using System.Collections.Generic;

public class ResourceManager
{
	Dictionary<string, Object> _resouces = new Dictionary<string, Object>();

	// ���ҽ� �ε�
	public T Load<T>(string key) where T : Object
	{
		// �̹� �ε�� ���ҽ��� �ִٸ� ����
		if (_resouces.TryGetValue(key, out Object resouce))
			return resouce as T;

		return null;
	}

	// ���ҽ� �ν��Ͻ�ȭ
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

	// ���ҽ� ����
	public void Destroy(GameObject go)
	{
		if (go == null)
			return;

		Object.Destroy(go);
	}

	#region Addressables
	// Addressables name �������� ���ҽ� �ε�
	public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object
	{
		// �̹� �ε�� ���ҽ��� �ִٸ� �ݹ� ȣ�� �� ����
		if (_resouces.TryGetValue(key, out Object resouce))
		{
			callback?.Invoke(resouce as T);	
			return;
		}
		
		// ���ҽ� �񵿱� �ε�
		AsyncOperationHandle<T> asyncOperation = Addressables.LoadAssetAsync<T>(key);
		asyncOperation.Completed += (op) =>
		{
			_resouces.Add(key, op.Result);
			callback?.Invoke(op.Result);
		};
	}

	// Addressables Label �������� ��ü ���ҽ� �ε�
	public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : Object
	{
		// Addressables Label �� �����ִ� Addressables name �� �񵿱�� �ε�
		var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
		opHandle.Completed += (op) =>
		{
			int loadCount = 0;
			int totalCount = op.Result.Count;

			// ������ Addressables name ��ȸ 
			foreach (var result in op.Result)
			{
				// ���ҽ� �񵿱� �ε�
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