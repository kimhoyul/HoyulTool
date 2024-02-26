using UnityEngine;
using UnityEngine.UI;

public class ButtonEX : Button
{
	[SerializeField] public Graphic[] subGraphics = new Graphic[0];
	[SerializeField] private Color subNormalColor = Color.white;

	protected override void Start()
	{
		base.Start();
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		if (subGraphics == null || subGraphics.Length == 0)
			return;

		Color targetColor = SetColorByState(state);

		foreach (var graphic in subGraphics)
			graphic.CrossFadeColor(targetColor, instant ? 0 : colors.fadeDuration, true, true);

		targetColor = state == SelectionState.Normal ? colors.normalColor : targetColor;
		targetGraphic.CrossFadeColor(targetColor, instant ? 0 : colors.fadeDuration, true, true);
	}

	protected Color SetColorByState(SelectionState state)
	{
		Color targetColor = Color.white;

		switch (state)
		{
			case SelectionState.Disabled:
				targetColor = colors.disabledColor;
				break;
			case SelectionState.Highlighted:
				targetColor = colors.highlightedColor;
				break;
			case SelectionState.Normal:
				targetColor = subNormalColor;
				break;
			case SelectionState.Pressed:
				targetColor = colors.pressedColor;
				break;
			case SelectionState.Selected:
				targetColor = colors.selectedColor;
				break;
		}

		return targetColor;
	}
}