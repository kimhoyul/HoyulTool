using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DownloadButton : UI_ButtonBase
{
	public GameObject shadow;

	public override void OnPointerEnter(PointerEventData eventData) 
	{
		shadow.SetActive(true);
	}

	public override void OnPointerExit(PointerEventData eventData) 
	{
		shadow.SetActive(false);
	}
}
