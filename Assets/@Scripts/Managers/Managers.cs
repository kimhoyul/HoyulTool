using System.Collections;
using UnityEngine;

public class Managers : MonoBehaviour
{
	static Managers s_instance;
	static bool s_init = false;

	#region Contents
	GameManager _game = new GameManager();
	ObjectManager _object = new ObjectManager();
	PoolManager _pool = new PoolManager();
	public static GameManager Game { get { return Instance._game; } }
	public static ObjectManager Object { get { return Instance._object; } }
	public static PoolManager Pool { get { return Instance._pool; } }
	#endregion

	#region Core
	DataManager _data = new DataManager();
	LoadingManager _loading = new LoadingManager();
	LogManager _log = new LogManager();
	ResourceManager _resource = new ResourceManager();
	SceneManagerEX _scene = new SceneManagerEX();
	SoundManager _sound = new SoundManager();
	public static DataManager Data { get { return Instance._data; } }
	public static LoadingManager Loading { get { return Instance._loading; } }
	public static LogManager Log { get { return Instance._log; } }
	public static ResourceManager Resource { get { return Instance._resource; } }
	public static SceneManagerEX Scene { get { return Instance._scene; } }
	public static SoundManager Sound { get { return Instance._sound; } }
	#endregion

	public static Managers Instance 
	{ 
		get 
		{ 
			if (s_init == false)
			{
				s_init = true;

				GameObject go = GameObject.Find("@Managers");
				if (go == null)
				{
					go = new GameObject { name = "@Managers" };
					go.AddComponent<Managers>();
				}

				DontDestroyOnLoad(go);
				s_instance = go.GetComponent<Managers>();

				s_instance._loading.Init();
			}
			
			return s_instance; 
		} 
	}

	private void OnDestroy()
	{
	}
}
