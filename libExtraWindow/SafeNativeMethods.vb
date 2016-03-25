Module Win32APIConstants
    Friend Const WS_CAPTION = &HC0000
    Friend Const LOGPIXELSX = 88
    Friend Const LOGPIXELSY = 90
End Module
Module SafeNativeMethods
    Declare Function GetDC Lib "user32" (hwnd As IntPtr) As IntPtr
    Declare Function ReleaseDC Lib "user32" (hwnd As IntPtr, hdc As IntPtr) As Boolean
    Declare Function GetDeviceCaps Lib "gdi32" (hdc As IntPtr, nIndex As Integer) As Integer
    Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (hwnd As IntPtr, nIndex As Integer) As Integer
    Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (hwnd As IntPtr, nIndex As Integer, dwNewLong As Integer) As Integer
End Module
Enum HitTest As Integer
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