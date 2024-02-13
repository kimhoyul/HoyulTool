using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
		searchInputField.onEndEdit.AddListener(HandleSearchSubmit);
	}

	private void HandleSearchSubmit(string searchText)
	{
		if (!string.IsNullOrEmpty(searchText))
		{
			searchBarAnimator.Play("Move");
			StartCoroutine(FadeInAfterAnimation());
		}
	}

	IEnumerator FadeInAfterAnimation()
	{
		yield return new WaitUntil(() => searchBarAnimator.GetCurrentAnimatorStateInfo(0).IsName("Move"));
		yield return new WaitUntil(() => searchBarAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !searchBarAnimator.IsInTransition(0));

		GameObject go = Utils.FindChild(transform.parent.gameObject, "UI_SearchedItem", true);
		go.SetActive(true);
		UI_SearchedItem _searchedItem = go.GetComponent<UI_SearchedItem>();
		
		Debug.Log("UI_SearchedItem");
		GameObject go2 = new GameObject();
		WebDriverManager web = go2.AddComponent<WebDriverManager>();
		StartCoroutine(web.LoadSearchResults(searchInputField.text, _searchedItem.OnImageLoaded, _searchedItem.SetItemCountText, _searchedItem.OnComplate));
	}
}
