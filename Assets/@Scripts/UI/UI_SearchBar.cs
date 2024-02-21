using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class UI_SearchBar : MonoBehaviour
{
	[SerializeField] private Button searchIconButton;
	[SerializeField] private TMP_InputField searchInputField;
	[SerializeField] private Animator searchBarAnimator;

	private void Start()
	{
		SetupEventListeners();
	}

	private void SetupEventListeners()
	{
		searchIconButton.onClick.AddListener(() => searchInputField.ActivateInputField());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if (searchInputField.text != string.Empty)
			{
				HandleSearchSubmit(searchInputField.text);
			}
		}
	}

	private void HandleSearchSubmit(string searchText)
	{
		if (!string.IsNullOrEmpty(searchText))
		{
			searchBarAnimator.Play("Move");
			StartCoroutine(FadeInAfterAnimation(searchText));
		}
	}

	IEnumerator FadeInAfterAnimation(string url)
	{
		yield return new WaitUntil(() => searchBarAnimator.GetCurrentAnimatorStateInfo(0).IsName("Move"));
		yield return new WaitUntil(() => searchBarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !searchBarAnimator.IsInTransition(0));

		SeleniumController sc = transform.GetComponentInChildren<SeleniumController>();
		sc.ConfigureDriverOptions(url, HandleImageLoaded);
	}

	private void HandleImageLoaded(int imageCount)
	{
		GameObject go = Utils.FindChild(transform.parent.gameObject, "UI_SearchedItem", true);

		if (go.activeSelf == false)
			go.SetActive(true);
		else
			go.GetComponent<UI_SearchedItem>().SetSourceUrls();
	}
}
