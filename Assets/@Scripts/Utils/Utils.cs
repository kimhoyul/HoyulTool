using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Utils
{
	public static T GetOrAddComponent<T>(GameObject go) where T : Component
	{
		T component = go.GetComponent<T>();
		if (component == null)
		{
			component = go.AddComponent<T>();
		}
		return component;
	}

	public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
	{
		Transform transform = FindChild<Transform>(go, name, recursive);
		if (transform == null)
		{
			return null;
		}
		return transform.gameObject;
	}

	public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
	{
		if (go == null)
			return null;

		if (recursive == false)
		{
			for (int i = 0; i < go.transform.childCount; i++)
			{
				Transform transform = go.transform.GetChild(i);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					T component = transform.GetComponent<T>();
					if (component != null)
						return component;
				}
			}
		}
		else
		{
			foreach (T component in go.GetComponentsInChildren<T>(true))
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
					return component;
			}
		}

		return null;
	}

	public static Color HexToColor(string hexCode)
	{
		if (ColorUtility.TryParseHtmlString(hexCode, out Color color))
		{
			return color;
		}
		else
		{
			Debug.LogWarning($"[Utils::HexToColor]Invalid Hex Code - {hexCode}");
			return Color.white; 
		}
	}

	public static List<string> ExtractNumbersFromString(string name)
	{
		string pattern = @"\d+";
		List<string> strings = new List<string>();

		MatchCollection matches = Regex.Matches(name, pattern);

		foreach (Match match in matches)
		{
			strings.Add(match.Value);
		}

		return strings;
	}
}
	