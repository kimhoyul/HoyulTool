using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TitleBar : MonoBehaviour, IPointerEnterHandler
{
    #region Windows API Constants and Structures

    // 윈도우 스타일 및 위치 조정에 사용되는 상수
    const int SWP_SHOWWINDOW = 0x40;
    const int SWP_NOMOVE = 0x0002;
    const int SWP_NOSIZE = 0x0001;
    const uint WS_SIZEBOX = 0x00040000;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 0x00800000;
    const int WS_DLGFRAME = 0x00400000;
    const int WS_CAPTION = WS_BORDER | WS_DLGFRAME;

    // 윈도우 핸들 및 위치 조정을 위한 기본 핸들
    static System.IntPtr hWnd;
    static System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);

    // 윈도우 정보 구조체
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    #endregion

    #region Windows API Functions

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    #endregion

    public GameObject _trigger;

    private float reducedHeight;
    private const float _titleBarHeight = 30.0f;

    public void Start()
    {
		reducedHeight = gameObject.GetComponent<RectTransform>().rect.height - _titleBarHeight;
		//ToggleTitleBar(true);
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rectTransform = _trigger.GetComponent<RectTransform>();

        switch (Managers.Game.TitleBarState)
        {
            case Define.TitleBarState.Hide:
                //ToggleTitleBar(true);

                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, - _titleBarHeight);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
				Managers.Game.TitleBarState = Define.TitleBarState.Show;

				break;
            case Define.TitleBarState.Show:
				//ToggleTitleBar(false);

				rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0);
				rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, reducedHeight);
				Managers.Game.TitleBarState = Define.TitleBarState.Hide;

				break;
        }
    }

    public static void ToggleTitleBar(bool showBorders)
    {
        // 현재 활성화된 윈도우 핸들 가져오기
        hWnd = GetActiveWindow();

        // 유니티 에디터에서는 실행하지 않음
        if (Application.isEditor) return;

        // 현재 윈도우 스타일 가져오기
        int currentStyle = GetWindowLong(hWnd, GWL_STYLE).ToInt32();

        // 타이틀 바 및 사이즈 박스 표시 여부에 따라 스타일 조정
        uint newStyle;
        if (showBorders)
        {
            // 타이틀 바와 사이즈 박스를 추가
            newStyle = (uint)(currentStyle | WS_CAPTION | WS_SIZEBOX);
        }
        else
        {
            // 타이틀 바와 사이즈 박스를 제거
            newStyle = (uint)(currentStyle & ~(WS_CAPTION | WS_SIZEBOX));
        }

        // 조정된 스타일 적용
        SetWindowLong(hWnd, GWL_STYLE, newStyle);

        // 윈도우의 스타일 변경을 적용하기 위해 윈도우 위치 및 크기 갱신
        // 윈도우 크기는 변경하지 않고, 최상위로 설정하여 화면에 표시
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
    }
}
