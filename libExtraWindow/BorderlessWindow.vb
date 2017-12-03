Option Strict Off
' 此文件使用 GPLv3 许可协议。协议内容：https://github.com/Nukepayload2/WPF_Borderless_Window/blob/master/libExtraWindow/License.txt
' 额外权限授权申请方式：在百度贴吧, GitHub, 新浪微博 @Nukepayload2 或者发邮件到 1939357182@qq.com

Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports Nukepayload2.UI.Win32

Namespace Global.Nukepayload2.UI.Xaml

    ''' <summary>
    ''' 无默认样式边框窗体
    ''' </summary>
    Public Class BorderlessWindow
        Inherits Window

        ''' <summary>
        ''' 为非 Windows Runtime 环境提供 DisplayInformation.LogicalDpi 数据。
        ''' </summary>
        Public ReadOnly Property SystemDPI As Vector

        ''' <summary> 窗体角宽，用于调整大小 </summary>
        Public Property AngleWidth As Integer
            Get
                Return GetValue(AngleWidthProperty)
            End Get
            Set
                SetValue(AngleWidthProperty, Value)
            End Set
        End Property
        Public Shared ReadOnly AngleWidthProperty As DependencyProperty =
                               DependencyProperty.Register(NameOf(AngleWidth),
                               GetType(Integer), GetType(BorderlessWindow),
                               New PropertyMetadata(8))

        ''' <summary>框宽</summary>
        Public Property ResizeThickness As Thickness
            Get
                Return GetValue(ResizeThicknessProperty)
            End Get
            Set
                SetValue(ResizeThicknessProperty, Value)
            End Set
        End Property
        Public Shared ReadOnly ResizeThicknessProperty As DependencyProperty =
                               DependencyProperty.Register(NameOf(ResizeThickness),
                               GetType(Thickness), GetType(BorderlessWindow),
                               New PropertyMetadata(New Thickness(2)))

        Protected Overridable Function WndProc(hwnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr, ByRef handled As Boolean) As IntPtr
            Const WM_NCHITTEST As Integer = &H84
            Const WM_DPICHANGED = &H2E0
            Dim mousePoint As New Point()
            Select Case msg
                Case WM_NCHITTEST
                    If WindowState = WindowState.Maximized Then Return IntPtr.Zero
                    mousePoint.X = (lParam.ToInt32() And &HFFFF) * 96 / SystemDPI.X
                    mousePoint.Y = (lParam.ToInt32() >> 16) * 96 / SystemDPI.Y
                    ' 左上  
                    If mousePoint.Y - Top <= AngleWidth AndAlso mousePoint.X - Left <= AngleWidth Then
                        handled = True
                        Return New IntPtr(HitTest.HTTOPLEFT)
                        ' 左下      
                    ElseIf ActualHeight + Top - mousePoint.Y <= AngleWidth AndAlso mousePoint.X - Left <= AngleWidth Then
                        handled = True
                        Return New IntPtr(HitTest.HTBOTTOMLEFT)
                        ' 右上  
                    ElseIf mousePoint.Y - Top <= AngleWidth AndAlso ActualWidth + Left - mousePoint.X <= AngleWidth Then
                        handled = True
                        Return New IntPtr(HitTest.HTTOPRIGHT)
                        ' 右下  
                    ElseIf ActualWidth + Left - mousePoint.X <= AngleWidth AndAlso ActualHeight + Top - mousePoint.Y <= AngleWidth Then
                        handled = True
                        Return New IntPtr(HitTest.HTBOTTOMRIGHT)
                        ' 左  
                    ElseIf mousePoint.X - Left <= ResizeThickness.Left Then
                        handled = True
                        Return New IntPtr(HitTest.HTLEFT)
                        ' 右  
                    ElseIf ActualWidth + Left - mousePoint.X <= ResizeThickness.Right Then
                        handled = True
                        Return New IntPtr(HitTest.HTRIGHT)
                        ' 上  
                    ElseIf mousePoint.Y - Top <= ResizeThickness.Top Then
                        handled = True
                        Return New IntPtr(HitTest.HTTOP)
                        ' 下  
                    ElseIf ActualHeight + Top - mousePoint.Y <= ResizeThickness.Bottom Then
                        handled = True
                        Return New IntPtr(HitTest.HTBOTTOM)
                    Else
                        handled = False
                    End If
                Case WM_DPICHANGED
                    Dim newDpi As Integer = wParam.ToInt64 And &HFFFF
                    Dim dpi = newDpi
                    Dim newRect As RECT
                    newRect = Marshal.PtrToStructure(lParam, GetType(RECT))
                    SetScaleTransform(dpi)
                    SetWindowPos(hwnd, 0, newRect.Left, newRect.Top, newRect.Right - newRect.Left,
                                 newRect.Bottom - newRect.Top, SWP_NOZORDER Or SWP_NOOWNERZORDER Or SWP_NOACTIVATE)
            End Select
            Return IntPtr.Zero
        End Function

        Private Sub SetScaleTransform(dpi As Integer)
            Dim rootElement = TryCast(Content, FrameworkElement)
            If rootElement IsNot Nothing Then
                Dim transform = TryCast(rootElement.LayoutTransform, ScaleTransform)
                If transform Is Nothing Then
                    transform = New ScaleTransform
                    rootElement.SetValue(Window.LayoutTransformProperty, transform)
                End If
                transform.ScaleX = dpi / SystemDPI.X
                transform.ScaleY = dpi / SystemDPI.Y
            End If
        End Sub

        Public Sub UpdateSystemDPIValue()
            Dim dc = GetDC(IntPtr.Zero)
            _SystemDPI.X = GetDeviceCaps(dc, LOGPIXELSX)
            _SystemDPI.Y = GetDeviceCaps(dc, LOGPIXELSY)
            ReleaseDC(IntPtr.Zero, dc)
        End Sub

        Protected Overrides Sub OnSourceInitialized(e As EventArgs)
            MyBase.OnSourceInitialized(e)
            Dim hwndSource As HwndSource = DirectCast(PresentationSource.FromVisual(Me), HwndSource)
            If hwndSource IsNot Nothing Then
                UpdateSystemDPIValue()
                hwndSource.AddHook(New HwndSourceHook(AddressOf WndProc))
            End If
        End Sub

        Public Property DpiAwareness As ProcessDpiAwareness
            Get
                Return GetValue(DpiAwarenessProperty)
            End Get
            Set
                SetValue(DpiAwarenessProperty, Value)
            End Set
        End Property
        Public Shared ReadOnly DpiAwarenessProperty As DependencyProperty =
                               DependencyProperty.Register(NameOf(DpiAwareness),
                               GetType(ProcessDpiAwareness), GetType(BorderlessWindow),
                               New PropertyMetadata(ProcessDpiAwareness.SystemDpiAware,
                                                    Sub(s, e)
                                                        Dim this = DirectCast(s, BorderlessWindow)
                                                        this.perMonDPIHelper.DpiAwareness = e.NewValue
                                                    End Sub))

        Dim hWnd As IntPtr
        Public ReadOnly Property Handle As IntPtr = hWnd
        Dim perMonDPIHelper As New PerMonitorDpiAwareHelper
        Dim _minWidth, _minHeight As Double

        Private Sub NoBorderWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            hWnd = New WindowInteropHelper(Me).Handle
            Dim dpi = perMonDPIHelper.GetWindowDpi(hWnd)
            SetScaleTransform(dpi.X)
            SetWindowLong(New WindowInteropHelper(Me).Handle, -16, 369295360)
        End Sub
    End Class

End Namespace
