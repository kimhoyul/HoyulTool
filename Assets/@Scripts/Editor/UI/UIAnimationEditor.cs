using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIAnimation))]
public class UIAnimationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		UIAnimation script = (UIAnimation)target;

		DrawDefaultInspector();

		if (script.changeColor)
		{
			script.targetColor = EditorGUILayout.ColorField("TargetColor", script.targetColor);
		}
	}
}
