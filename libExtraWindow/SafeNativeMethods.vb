' 此文件使用 GPLv3 许可协议。协议内容：https://github.com/Nukepayload2/WPF_Borderless_Window/blob/master/libExtraWindow/License.txt
' 额外权限授权申请方式：在百度贴吧, GitHub, 新浪微博 @Nukepayload2 或者发邮件到 1939357182@qq.com

Friend Module Win32APIConstants
    Friend Const LOGPIXELSX = 88
    Friend Const LOGPIXELSY = 90
    Friend Const MONITOR_DEFAULTTONEAREST = 2
    Friend Const SWP_NOZORDER = 4
    Friend Const SWP_NOOWNERZORDER = &H200
    Friend Const SWP_NOACTIVATE = &H10
End Module

Friend Module NativeMethods
    Declare Function GetDC Lib "user32" (hwnd As IntPtr) As IntPtr
    Declare Function ReleaseDC Lib "user32" (hwnd As IntPtr, hdc As IntPtr) As Boolean
    Declare Function GetDeviceCaps Lib "gdi32" (hdc As IntPtr, nIndex As Integer) As Integer
    Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (hwnd As IntPtr, nIndex As Integer, dwNewLong As Integer) As Integer
    Declare Function GetProcessDpiAwareness Lib "Shcore.dll" (hProcess As IntPtr, ByRef dpiAwareness As ProcessDpiAwareness) As Integer
    Declare Function SetProcessDpiAwareness Lib "Shcore.dll" (dpiAwareness As ProcessDpiAwareness) As Integer
    Declare Function MonitorFromWindow Lib "user32" (hWnd As IntPtr, flags As Integer) As IntPtr
    Declare Function GetDpiForMonitor Lib "Shcore.dll" (hMonitor As IntPtr, dpiType As MonitorDpiType, ByRef dpiX As UInteger, ByRef dpiY As UInteger) As Integer
    Declare Function SetWindowPos Lib "user32.dll" (hWnd As IntPtr, hWndInsertAfter As IntPtr, X As Integer, Y As Integer, cx As Integer, cy As Integer, uFlags As Integer) As Boolean
End Module

Friend Enum MonitorDpiType
    MDT_EFFECTIVE_DPI
    MDT_ANGULAR_DPI
    MDT_RAW_DPI
    MDT_DEFAULT = MDT_EFFECTIVE_DPI
End Enum

Public Enum ProcessDpiAwareness
    Unaware
    SystemDpiAware
    PerMonitorDpiAware
End Enum

Friend Enum HitTest As Integer
    HTERROR = -2
    HTTRANSPARENT = -1
    HTNOWHERE = 0
    HTCLIENT = 1
    HTCAPTION = 2
    HTSYSMENU = 3
    HTGROWBOX = 4
    HTSIZE = HTGROWBOX
    HTMENU = 5
    HTHSCROLL = 6
    HTVSCROLL = 7
    HTMINBUTTON = 8
    HTMAXBUTTON = 9
    HTLEFT = 10
    HTRIGHT = 11
    HTTOP = 12
    HTTOPLEFT = 13
    HTTOPRIGHT = 14
    HTBOTTOM = 15
    HTBOTTOMLEFT = 16
    HTBOTTOMRIGHT = 17
    HTBORDER = 18
    HTREDUCE = HTMINBUTTON
    HTZOOM = HTMAXBUTTON
    HTSIZEFIRST = HTLEFT
    HTSIZELAST = HTBOTTOMRIGHT
    HTOBJECT = 19
    HTCLOSE = 20
    HTHELP = 21
End Enum

Friend Structure RECT
    Dim Left, Top, Right, Bottom As Integer
End Structure
