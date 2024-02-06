using UnityEngine;
public class Managers : MonoBehaviour
{
	static Managers s_instance;
	static Managers Instance { get { Init(); return s_instance; } }

	//#region Contents
	//GameManagerEX _game = new GameManagerEX();

	//public static GameManagerEX Game { get { return Instance._game; } }
	//#endregion

	#region Core

	LogManager _log = new LogManager();
	TitleBarManager _titlebar = new TitleBarManager();
	//DataManager _data = new DataManager();
	//InputManager _input = new InputManager();
	//PoolManager _pool = new PoolManager();
	//ResourceManager _resource = new ResourceManager();
	//SceneManagerEX _scene = new SceneManagerEX();
	//SoundManager _sound = new SoundManager();
	//UIManager _ui = new UIManager();

	public static LogManager Log { get { return Instance._log; } }
	public static TitleBarManager Titlebar { get { return Instance._titlebar; } }
	//public static DataManager Data { get { return Instance._data; } }
	//public static InputManager Input { get { return Instance._input; } }
	//public static PoolManager Pool { get { return Instance._pool; } }
	//public static ResourceManager Resource { get { return Instance._resource; } }
	//public static SceneManagerEX Scene { get { return Instance._scene; } }
	//public static SoundManager Sound { get { return Instance._sound; } }
	//public static UIManager UI { get { return Instance._ui; } }
	
	#endregion

	void Start()
	{
		Init();
	}

	private void Update()
	{
		//_input.OnUpdate();
	}

	static void Init()
	{
		if (s_instance == null)
		{
			GameObject go = GameObject.Find("@Managers");
			if (go == null)
			{
				go = new GameObject { name = "@Managers" };
				go.AddComponent<Managers>();
			}

			DontDestroyOnLoad(go);
			s_instance = go.GetComponent<Managers>();

			s_instance._log.Init();
			s_instance._titlebar.Init();
			//s_instance._data.Init();
			//s_instance._pool.Init();
			//s_instance._sound.Init();
		}
	}

	public static void Clear()
	{
		//Input.Clear();
		//Scene.Clear();
		//Sound.Clear();
		//UI.Clear();

		//Pool.Clear();
	}
}
