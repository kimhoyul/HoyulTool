using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ToolButton : UI_ButtonBase
{
	private Image _toolButtonBG;
	private Image _toolButtonIcon;
	private TMP_Text _toolButtonText;

	private bool _isClicked = false;

	private void Start()
	{
		_toolButtonBG = gameObject.GetComponent<Image>();
		_toolButtonIcon = Utils.FindChild<Image>(gameObject, "ToolIcon", true);
		_toolButtonText = Utils.FindChild<TMP_Text>(gameObject, "ToolText", true);

		gameObject.GetComponent<Button>().onClick.AddListener(() =>
		{
			_toolButtonBG.color = Utils.HexToColor("#6C5DCFFF");

			SetColor("#FFFFFF");

			_isClicked = true;

			Managers.Game.ToolState = Define.ToolState.Downloader;
		});
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		SetColor("#FFFFFF");
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (_isClicked)
			return;

		SetColor("#7A8191");
	}

	private void SetColor(string hexColor)
	{
		_toolButtonIcon.color = Utils.HexToColor(hexColor);
		_toolButtonText.color = Utils.HexToColor(hexColor);
	}
}