﻿Option Strict Off
Imports System.Windows.Interop

''' <summary>
''' 通过<see cref="ResizeThickness"/>和<see cref="AngleWidth"/>设置大小调整的无默认样式边框窗体
''' </summary>
''' <remarks></remarks>
Public Class NoBorderWindow
    Inherits Window
    Public ReadOnly Property CurrentDPIScale As Vector
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
                           GetType(Integer), GetType(NoBorderWindow),
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
                           GetType(Thickness), GetType(NoBorderWindow),
                           New PropertyMetadata(New Thickness(2)))

    Protected Overridable Function WndProc(hwnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr, ByRef handled As Boolean) As IntPtr
        Const WM_NCHITTEST As Integer = &H84
        Dim mousePoint As New Point()
        Select Case msg
            Case WM_NCHITTEST
                If WindowState = WindowState.Maximized Then Return IntPtr.Zero
                mousePoint.X = (lParam.ToInt32() And &HFFFF) * 96 / CurrentDPIScale.X
                mousePoint.Y = (lParam.ToInt32() >> 16) * 96 / CurrentDPIScale.Y
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
        End Select
        Return IntPtr.Zero
    End Function

    Public Sub UpdateDPIScale()
        Dim dc = GetDC(IntPtr.Zero)
        _CurrentDPIScale.X = GetDeviceCaps(dc, LOGPIXELSX)
        _CurrentDPIScale.Y = GetDeviceCaps(dc, LOGPIXELSY)
        ReleaseDC(IntPtr.Zero, dc)
    End Sub

    Protected Overrides Sub OnSourceInitialized(e As EventArgs)
        MyBase.OnSourceInitialized(e)
        Dim hwndSource As HwndSource = DirectCast(PresentationSource.FromVisual(Me), HwndSource)
        If hwndSource IsNot Nothing Then
            UpdateDPIScale()
            hwndSource.AddHook(New HwndSourceHook(AddressOf WndProc))
        End If
    End Sub

    Dim hWnd As IntPtr
    Public ReadOnly Property Handle As IntPtr = hWnd

    Private Sub NoBorderWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        hWnd = New WindowInteropHelper(Me).Handle
        SetWindowLong(New WindowInteropHelper(Me).Handle, -16, 369295360)
    End Sub
End Class