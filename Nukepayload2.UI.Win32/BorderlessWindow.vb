﻿Option Strict Off
' 此文件使用 LGPLv3 许可协议。协议内容：https://github.com/Nukepayload2/WPF_Borderless_Window/blob/master/libExtraWindow/License.txt
' 额外权限授权申请方式：在百度贴吧, GitHub, 新浪微博 @Nukepayload2 或者发邮件到 1939357182@qq.com

Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports Nukepayload2.UI.Win32

Namespace Global.Nukepayload2.UI.Xaml

    Public Class BorderlessWindow
        Inherits Window

        ''' <summary>
        ''' Ported from WinRT API: DisplayInformation.LogicalDpi
        ''' </summary>
        Public ReadOnly Property SystemDPI As Vector

        ''' <summary>Corner size for resizing</summary>
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

        ''' <summary>Border width for resizing</summary>
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

        Const WM_DPICHANGED = &H2E0

        Protected Overridable Function WndProc(hwnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr, ByRef handled As Boolean) As IntPtr
            Const WM_NCHITTEST As Integer = &H84
            Dim mousePoint As New Point()
            Select Case msg
                Case WM_NCHITTEST
                    If WindowState = WindowState.Maximized Then Return IntPtr.Zero
                    Dim rawPoint As Integer = lParam.ToInt32()
                    Dim pointLowPart As Integer = CShort(rawPoint And &HFFFF)
                    Dim pointHighPart As Integer = CShort(rawPoint >> 16)

                    If enableLegacyPointScale OrElse
                        DpiAwarenessIsNotPM() OrElse
                        Not IsNet461CompatibleMode Then

                        mousePoint.X = pointLowPart * 96 / SystemDPI.X
                        mousePoint.Y = pointHighPart * 96 / SystemDPI.Y
                    Else
                        mousePoint.X = pointLowPart
                        mousePoint.Y = pointHighPart
                    End If

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
                    If DpiAwarenessIsNotPM() Then
                        ' Let the .NET Framework to handle this case.
                        Return IntPtr.Zero
                    End If

                    Dim newDpi As Integer = wParam.ToInt32 And &HFFFF
                    Dim dpi = newDpi
                    Dim newRect As RECT
                    newRect = Marshal.PtrToStructure(lParam, GetType(RECT))

                    Dim wpfWidth = ActualWidth
                    Dim wpfHeight = ActualHeight

                    Dim width = newRect.Right - newRect.Left
                    Dim height = newRect.Bottom - newRect.Top

                    If IsNet461CompatibleMode Then
                        SetScaleTransform(dpi)
                        SetWindowPos(hwnd, 0, newRect.Left, newRect.Top, width,
                                 height, SWP_NOZORDER Or SWP_NOOWNERZORDER Or SWP_NOACTIVATE)
                    Else
                        _SystemDPI = New Vector(newDpi, newDpi)
                        NotifyWpfSizing()
                    End If
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
                transform.ScaleX = dpi / 96
                transform.ScaleY = dpi / 96
            End If
        End Sub

        Private Sub RemoveScaleTransform()
            Dim rootElement = TryCast(Content, FrameworkElement)
            If rootElement IsNot Nothing Then
                Dim transform = TryCast(rootElement.LayoutTransform, ScaleTransform)
                If transform IsNot Nothing Then
                    rootElement.LayoutTransform = Nothing
                End If
            End If
        End Sub

        Protected Overrides Sub OnSourceInitialized(e As EventArgs)
            MyBase.OnSourceInitialized(e)
            Dim hwndSource As HwndSource = DirectCast(PresentationSource.FromVisual(Me), HwndSource)
            If hwndSource IsNot Nothing Then
                hwndSource.AddHook(New HwndSourceHook(AddressOf WndProc))
            End If
        End Sub

        Public Property DpiAwareness As ProcessDpiAwareness?
            Get
                Return GetValue(DpiAwarenessProperty)
            End Get
            Set
                SetValue(DpiAwarenessProperty, Value)
            End Set
        End Property
        Public Shared ReadOnly DpiAwarenessProperty As DependencyProperty =
                               DependencyProperty.Register(NameOf(DpiAwareness),
                               GetType(ProcessDpiAwareness?), GetType(BorderlessWindow),
                               New PropertyMetadata(New PerMonitorDpiAwareHelper().DpiAwareness,
                                                    Sub(s, e)
                                                        Dim this = DirectCast(s, BorderlessWindow)
                                                        If e.NewValue IsNot Nothing Then
                                                            this.perMonDPIHelper.DpiAwareness = e.NewValue
                                                        End If
                                                    End Sub))

        Dim perMonDPIHelper As New PerMonitorDpiAwareHelper
        Dim enableLegacyPointScale As Boolean

        Private Function DpiAwarenessIsNotPM() As Boolean
            Return DpiAwareness <> ProcessDpiAwareness.PerMonitorDpiAware
        End Function

        Private Function DpiAwarenessIsUnset() As Boolean
            Dim lc = ReadLocalValue(DpiAwarenessProperty)
            Return lc Is DependencyProperty.UnsetValue
        End Function

        Public Property IsNet461CompatibleMode As Boolean

        Private Sub NoBorderWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            Dim hWnd = New WindowInteropHelper(Me).Handle
            SetWindowLong(hWnd, -16, &H16030000)
            SetWindowLong(hWnd, -20, &H40000)
            Dim dpi = perMonDPIHelper.GetWindowDpi(hWnd)
            If dpi IsNot Nothing Then
                _SystemDPI = dpi.Value
                If Not DpiAwarenessIsUnset() Then
                    If IsNet461CompatibleMode Then
                        SetScaleTransform(dpi.Value.X)
                    Else
                        NotifyWpfDpiChanged(hWnd, dpi.Value.X)
                    End If
                    Width *= SystemDPI.X / 96
                    Height *= SystemDPI.Y / 96
                End If
            Else
                Dim dc = GetDC(IntPtr.Zero)
                _SystemDPI.X = GetDeviceCaps(dc, LOGPIXELSX)
                _SystemDPI.Y = GetDeviceCaps(dc, LOGPIXELSY)
                ReleaseDC(IntPtr.Zero, dc)
            End If
            enableLegacyPointScale = Not Win32ApiInformation.IsWindowAcrylicApiPresent
        End Sub

        Private Sub NotifyWpfDpiChanged(hWnd As IntPtr, dpi As Integer)
            Dispatcher.BeginInvoke(
            Sub()
                Dim newRect As New RECT With {
                    .Left = Left,
                    .Top = Top,
                    .Right = Left + Width,
                    .Bottom = Top + Height
                }
                Dim dpiNative As New IntPtr(dpi Or (dpi << 16))
                SendMessage(hWnd, WM_DPICHANGED, dpiNative, newRect)
                NotifyWpfSizing()
            End Sub)
        End Sub

        Private Sub NotifyWpfSizing()
            Dispatcher.BeginInvoke(
            Sub()
                Top -= 1
                Top += 1
            End Sub)
        End Sub
    End Class

End Namespace
