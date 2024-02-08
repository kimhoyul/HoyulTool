using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameManager
{
	Define.TitleBarState _titleBarState = Define.TitleBarState.Hide;

	public event Action<Define.TitleBarState> OnTitleBarStateChanged;

	public Define.TitleBarState TitleBarState
	{
		get { return _titleBarState; }
		set
		{
			_titleBarState = value;
			OnTitleBarStateChanged?.Invoke(_titleBarState);
		}
	}
}