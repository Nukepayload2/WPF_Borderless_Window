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

Friend Enum HitTest
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

Public Enum NativeWindowStyle
    Overlapped = &H0
    Popup = &H80000000
    Child = &H40000000
    Minimize = &H20000000
    Visible = &H10000000
    Disabled = &H8000000
    ClipSiblings = &H4000000
    ClipChildren = &H2000000
    Maximize = &H1000000
    Caption = Border Or DlgFrame
    Border = &H800000
    DlgFrame = &H400000
    VScroll = &H200000
    HScroll = &H100000
    SysMenu = &H80000
    ThickFrame = &H40000
    Group = &H20000
    TabStop = &H10000
    MinimizeBox = &H20000
    MaximizeBox = &H10000
    Tiled = Overlapped
    Iconic = Minimize
    SizeBox = ThickFrame
    TileWindow = OverlappedWindow
    OverlappedWindow = (Overlapped Or
                        Caption Or
                        SysMenu Or
                        ThickFrame Or
                        MinimizeBox Or
                        MaximizeBox)
    PopupWindow = (Popup Or
                   Border Or
                   SysMenu)
    ChildWindow = Child
End Enum

<Flags>
Public Enum ExtendedWindowStyle
    ' EX
    DlgModalFrame = &H1
    NoParentNotify = &H4
    TopMost = &H8
    AcceptFiles = &H10
    Transparent = &H20
    ' Windows NT 4.0
    MdiChild = &H40
    ToolWindow = &H80
    WindowEdge = &H100
    ClientEdge = &H200
    ContextHelp = &H400
    Right = &H1000
    Left = &H0
    RtlReading = &H2000
    LtrReading = &H0
    LeftScrollBar = &H4000
    RightScrollBar = &H0
    ControlParent = &H10000
    StaticEdge = &H20000
    AppWindow = &H40000
    OverlappedWindow = (WindowEdge Or ClientEdge)
    PaletteWindow = (WindowEdge Or ToolWindow Or TopMost)
    ' Windows 2000
    Layered = &H80000
    NoInheritLayout = &H100000
    LayoutRtl = &H400000
    NoActive = &H8000000
    ' Windows XP
    Composited = &H2000000
    ' Windows 8
    NoRedirectionBitmap = &H200000
End Enum