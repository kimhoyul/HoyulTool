using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Maintools : MonoBehaviour
{
	[Header("Downloader")]
    [SerializeField] private Button _downloaderButton;
	[SerializeField] private GameObject _downloaderPanel;

	void Start()
    {
		_downloaderButton.onClick.AddListener(ShowDownloaderPanel);
	}

	private void ShowDownloaderPanel()
	{
		CanvasGroup cv = _downloaderPanel.GetComponent<CanvasGroup>();
		StartCoroutine(LerpAlpha(cv, 3f));
	}

	protected IEnumerator LerpAlpha(CanvasGroup cv, float duration)
	{
		for (float f = 0; f <= duration; f = f + Time.deltaTime)
		{
			cv.alpha = Mathf.Lerp(0, duration, f / duration);
			yield return null;
		}
	}
}
