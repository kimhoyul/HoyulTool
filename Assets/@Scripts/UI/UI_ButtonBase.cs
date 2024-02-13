using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public virtual void OnPointerEnter(PointerEventData eventData) { }
	public virtual void OnPointerExit(PointerEventData eventData) { }
}