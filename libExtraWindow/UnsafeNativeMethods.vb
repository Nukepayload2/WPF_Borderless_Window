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

    Declare Function DwmExtendFrameIntoClientArea Lib "dwmapi.dll" (hwnd As IntPtr, ByRef pMarInset As DwmMargins) As Integer
    Declare Function DwmIsCompositionEnabled Lib "dwmapi.dll" (ByRef enabledptr As Boolean) As Integer
    Declare Function DwmEnableBlurBehindWindow Lib "dwmapi.dll" (Hwnd As IntPtr, ByRef Blur As DwmBlurBehind) As Integer

    Declare Function SetWindowCompositionAttribute Lib "user32.dll" (hwnd As IntPtr, ByRef data As WindowCompositionAttributeData) As Boolean
    Declare Function GetWindowCompositionAttribute Lib "user32.dll" (hwnd As IntPtr, ByRef data As WindowCompositionAttributeData) As Boolean

    Declare Unicode Function LoadLibrary Lib "Kernel32.dll" Alias "LoadLibraryW" (fileName As String) As IntPtr
    Declare Function GetProcAddress Lib "Kernel32.dll" (hModule As IntPtr, procName As String) As IntPtr

End Module

Friend Enum AccentState
    Disabled
    EnableGradient
    EnableTransparentGradient
    EnableBlurBehind
    Invalid
End Enum

Friend Structure AccentPolicy
    Dim AccentState As AccentState
    Dim AccentFlags As Integer
    Dim GradientColor As Integer
    Dim AnimationId As Integer
End Structure

Friend Structure WindowCompositionAttributeData
    Dim Attribute As WindowCompositionAttribute
    Dim Data As IntPtr
    Dim SizeOfData As Integer
End Structure

Friend Enum WindowCompositionAttribute
    ' ...
    WCA_ACCENT_POLICY = 19
    ' ...
End Enum

Friend Structure DwmMargins
    Public LeftWidth As Integer
    Public RightWidth As Integer
    Public TopHeight As Integer
    Public BottomHeight As Integer
End Structure

Friend Structure DwmBlurBehind
    Public Flags As Integer ' 3
    Public Enable As Boolean ' 1
    Public RgnBlur As IntPtr
    Public Transition As Boolean '0
    Public Shared Function Create(Optional flags As Integer = 3, Optional enable As Integer = 1, Optional rgnBlur As Integer = 0, Optional transition As Integer = 0) As DwmBlurBehind
        Dim blu As New DwmBlurBehind With {
            .Flags = flags,
            .Enable = enable,
            .RgnBlur = rgnBlur,
            .Transition = transition
        }
        Return blu
    End Function
End Structure

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
