using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TitleBar : MonoBehaviour, IPointerEnterHandler
{
    #region Windows API Constants and Structures

    // ������ ��Ÿ�� �� ��ġ ������ ���Ǵ� ���
    const int SWP_SHOWWINDOW = 0x40;
    const int SWP_NOMOVE = 0x0002;
    const int SWP_NOSIZE = 0x0001;
    const uint WS_SIZEBOX = 0x00040000;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 0x00800000;
    const int WS_DLGFRAME = 0x00400000;
    const int WS_CAPTION = WS_BORDER | WS_DLGFRAME;

    // ������ �ڵ� �� ��ġ ������ ���� �⺻ �ڵ�
    static System.IntPtr hWnd;
    static System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);

    // ������ ���� ����ü
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

    private Define.TitleBarTriggerState _triggerState = Define.TitleBarTriggerState.Hide;

    public GameObject trigger;

    public float maxHeight;
    float reducedHeight;

    public void Init()
    {
        maxHeight = gameObject.GetComponent<RectTransform>().rect.height;
        reducedHeight = maxHeight - (maxHeight / 10);
        //ToggleTitleBar(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rectTransform = trigger.GetComponent<RectTransform>();

        switch (_triggerState)
        {
            case Define.TitleBarTriggerState.Hide:
                Debug.Log("Hide");
                //ToggleTitleBar(true);
                _triggerState = Define.TitleBarTriggerState.Show;

                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
                break;
            case Define.TitleBarTriggerState.Show:
                Debug.Log("Show");
                //ToggleTitleBar(false);
                _triggerState = Define.TitleBarTriggerState.Hide;

                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, reducedHeight);
                break;
        }
    }

    public static void ToggleTitleBar(bool showBorders)
    {
        // ���� Ȱ��ȭ�� ������ �ڵ� ��������
        hWnd = GetActiveWindow();

        // ����Ƽ �����Ϳ����� �������� ����
        if (Application.isEditor) return;

        // ���� ������ ��Ÿ�� ��������
        int currentStyle = GetWindowLong(hWnd, GWL_STYLE).ToInt32();

        // Ÿ��Ʋ �� �� ������ �ڽ� ǥ�� ���ο� ���� ��Ÿ�� ����
        uint newStyle;
        if (showBorders)
        {
            // Ÿ��Ʋ �ٿ� ������ �ڽ��� �߰�
            newStyle = (uint)(currentStyle | WS_CAPTION | WS_SIZEBOX);
        }
        else
        {
            // Ÿ��Ʋ �ٿ� ������ �ڽ��� ����
            newStyle = (uint)(currentStyle & ~(WS_CAPTION | WS_SIZEBOX));
        }

        // ������ ��Ÿ�� ����
        SetWindowLong(hWnd, GWL_STYLE, newStyle);

        // �������� ��Ÿ�� ������ �����ϱ� ���� ������ ��ġ �� ũ�� ����
        // ������ ũ��� �������� �ʰ�, �ֻ����� �����Ͽ� ȭ�鿡 ǥ��
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
    }
}