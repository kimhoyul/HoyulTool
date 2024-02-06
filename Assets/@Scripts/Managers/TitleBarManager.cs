using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleBarManager
{
	EventTrigger[] _eventTriggers = new EventTrigger[(int)Define.TitleBarTrigger.MaxCount];

	public void Init()
	{
		GameObject root = GameObject.Find("@TitleBarTrigger");
		if (root == null)
		{
			root = new GameObject("@TitleBarTrigger", typeof(GraphicRaycaster));
			root.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			GameObject.DontDestroyOnLoad(root);

			for (int i = 0; i < _eventTriggers.Length; i++)
			{
				var triggerName = ((Define.TitleBarTrigger)i).ToString();
				var go = new GameObject(triggerName, typeof(RectTransform), typeof(Image), typeof(EventTrigger));
				_eventTriggers[i] = go.GetComponent<EventTrigger>();

				var entry = new EventTrigger.Entry
				{
					eventID = EventTriggerType.PointerEnter
				};
				
				int index = i;
				entry.callback.AddListener((data) => OnPointerEnterDelegate((PointerEventData)data, (Define.TitleBarTrigger)index));

				_eventTriggers[i].triggers.Add(entry);

				go.transform.SetParent(root.transform, false);
			}
		}
	}

	private void OnPointerEnterDelegate(PointerEventData data, Define.TitleBarTrigger titleBarTrigger)
	{
		switch (titleBarTrigger)
		{
			case Define.TitleBarTrigger.Hide:
				Debug.Log("Hide");
				// HideTitleBar.ToggleTitleBar(true);
				break;
			case Define.TitleBarTrigger.Show:
				Debug.Log("Show");
				// HideTitleBar.ToggleTitleBar(false);
				break;
		}
	}
}
