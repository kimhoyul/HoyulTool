using System;

public class GameManager
{
	#region TitleBar
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
	#endregion

	#region Tool
	Define.ToolState _toolState = Define.ToolState.Unkown;

	public event Action<Define.ToolState> OnToolStateChanged;

	public Define.ToolState ToolState
	{
		get { return _toolState; }
		set
		{
			_toolState = value;
			OnToolStateChanged?.Invoke(_toolState);
		}
	}
	#endregion
}