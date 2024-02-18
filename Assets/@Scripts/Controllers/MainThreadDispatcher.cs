using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
	private static MainThreadDispatcher _instance;
	private static readonly Queue<Action> _actions = new Queue<Action>();

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	public static void Enqueue(Action action)
	{
		if (_instance == null)
		{
			Debug.LogWarning("MainThreadDispatcher not present in the scene.");
			return;
		}

		lock (_actions)
		{
			_actions.Enqueue(action);
		}
	}

	void Update()
	{
		while (_actions.Count > 0)
		{
			Action action = null;

			lock (_actions)
			{
				if (_actions.Count > 0)
				{
					action = _actions.Dequeue();
				}
			}

			action?.Invoke();
		}
	}
}
