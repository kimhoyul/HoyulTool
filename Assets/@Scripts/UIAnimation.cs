using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
	public enum EasingType
	{
		EasyIn,
		EasyOut,
		EasyInOut
	}

	public bool autoPlay;
	public float duration;
	public Vector3 targetPos;
	public EasingType easingType;
	public bool changeColor;
	[HideInInspector] public Color targetColor;

	private void Start()
	{
		if (autoPlay)
		{
			StartCoroutine(MoveUI(gameObject.GetComponent<RectTransform>()));
		}
	}

	IEnumerator MoveUI(RectTransform target)
	{
		Vector3 startPosition = target.localPosition;
		float duration = this.duration;
		float elapsedTime = 0;

		Image image = GetComponent<Image>();
		Color startColor = image != null ? image.color : Color.white;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / duration;

			float easedT = CalculateEasedT(easingType, t);
			target.localPosition = Vector3.Lerp(startPosition, targetPos, easedT);

			if (changeColor && image) UpdateColor(image, startColor, targetColor, easedT);

			yield return null;
		}

		target.localPosition = targetPos;
	}

	float CalculateEasedT(EasingType easingType, float t)
	{
		switch (easingType)
		{
			case EasingType.EasyOut:
				return Mathf.Sin(t * Mathf.PI * 0.5f);
			case EasingType.EasyIn:
				return 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
			case EasingType.EasyInOut:
				return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
			default:
				return t;
		}
	}

	void UpdateColor(Image image, Color startColor, Color endColor, float t)
	{
		image.color = Color.Lerp(startColor, endColor, t);
	}
}
