Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports Nukepayload2.UI.Win32

Public Class InteropWindow

    ' 这是 CoreWindow 。作为 CoreApplicationWindow 的一部分，要让它发挥作用还需要一个标题栏和一个输入层。
    ' 暂时只用最外面的应用窗口。
    'Dim coreWindow As New HwndSource(New HwndSourceParameters(String.Empty, 640, 480) With {
    '    .ExtendedWindowStyle = &H280000, .WindowStyle = &H14000000, .ParentWindow = containerWindow.Handle,
    '    .RestoreFocusMode = RestoreFocusMode.None, .TreatAncestorsAsNonClientArea = True, .TreatAsInputRoot = True
    '})

    Dim containerWindow As New HwndSource(New HwndSourceParameters(My.Application.Info.ProductName,
                                                                   SystemParameters.PrimaryScreenWidth / 2,
                                                                   SystemParameters.PrimaryScreenHeight / 2) With {
        .ExtendedWindowStyle = ExtendedWindowStyle.NoRedirectionBitmap Or ExtendedWindowStyle.WindowEdge,
        .WindowStyle = NativeWindowStyle.Popup Or NativeWindowStyle.ThickFrame Or NativeWindowStyle.Visible
    })

    Sub New()
        containerWindow.AddHook(AddressOf WndProc)
    End Sub

    Public ReadOnly Property HwndSource As HwndSource
        Get
            Return containerWindow
        End Get
    End Property

    Private Declare Function GetSystemMetrics Lib "user32.dll" (index As SM) As Integer

    Private Structure RECT
        Dim Left, Top, Right, Bottom As Integer
    End Structure

    Private Structure NCCALCSIZE_PARAMS
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)>
        Dim rgrc As RECT()
        Dim pWindowPos As IntPtr
    End Structure

    Protected Overridable Function WndProc(hwnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr, ByRef handled As Boolean) As IntPtr
        Const WM_CLOSE = &H10
        Const WM_NCCALCSIZE = &H83
        Select Case msg
            Case WM_CLOSE
                App.Current.Shutdown()
            Case WM_NCCALCSIZE
                ' 在非 Windows 10 需要测试这样做是否合适。
                'Dim xborder = GetSystemMetrics(SM.CXSIZEFRAME)
                'Dim yborder = GetSystemMetrics(SM.CYSIZEFRAME)
                Dim lpncsp = Marshal.PtrToStructure(Of NCCALCSIZE_PARAMS)(lParam)
                With lpncsp.rgrc(0)
                    .Top += 1
                    .Bottom -= 1
                    .Left += 1
                    .Right -= 1
                End With
                Marshal.StructureToPtr(lpncsp, lParam, False)
                handled = True
        End Select
    End Function

    Public Sub SetBlurBehind(enable As Boolean)
        Dim comp As New WindowComposition10
        comp.TrySetBlur(HwndSource.Handle, enable, True)
    End Sub

    Public Property Content As Visual
        Get
            Return HwndSource.RootVisual
        End Get
        Set(value As Visual)
            HwndSource.RootVisual = value
        End Set
    End Property
End Class
