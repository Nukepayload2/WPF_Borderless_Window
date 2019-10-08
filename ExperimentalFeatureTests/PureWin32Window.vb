Imports System.ComponentModel
Imports System.Runtime.InteropServices

Class PureWin32Window
    Implements IDisposable

    Private Delegate Function WndProc(hWnd As IntPtr, msg As UInteger, wParam As IntPtr, lParam As IntPtr) As IntPtr

    Private Structure WindowClass
        Public Style As UInteger
        Public WndProc As WndProc
        Public ClassExtra As Integer
        Public WindowExtra As Integer
        Public Instance As IntPtr
        Public Icon As IntPtr
        Public Cursor As IntPtr
        Public BackgroundBrush As IntPtr
        <MarshalAs(UnmanagedType.LPWStr)>
        Public MenuName As String
        <MarshalAs(UnmanagedType.LPWStr)>
        Public ClassName As String
    End Structure

    Private Declare Unicode Function RegisterClass Lib "user32.dll" Alias "RegisterClassW" (
        <[In]> ByRef lpWndClass As WindowClass
    ) As UShort

    Private Declare Unicode Function CreateWindowEx Lib "user32.dll" Alias "CreateWindowExW" (
        dwExStyle As Integer,
        <MarshalAs(UnmanagedType.LPWStr)> lpClassName As String,
        <MarshalAs(UnmanagedType.LPWStr)> lpWindowName As String,
        dwStyle As Integer,
        x As Integer,
        y As Integer,
        nWidth As Integer,
        nHeight As Integer,
        hWndParent As IntPtr,
        hMenu As IntPtr,
        hInstance As IntPtr,
        lpParam As IntPtr
    ) As IntPtr

    Private Declare Unicode Function DefWindowProc Lib "user32.dll" Alias "DefWindowProcW" (
        hWnd As IntPtr,
        msg As UInteger,
        wParam As IntPtr,
        lParam As IntPtr
    ) As IntPtr

    Private Declare Function DestroyWindow Lib "user32.dll" (
        hWnd As IntPtr
    ) As Boolean

    Private Declare Function SetParent Lib "user32" (hWndChild As IntPtr, hWndNewParent As IntPtr) As IntPtr

    Private Const ERROR_CLASS_ALREADY_EXISTS As Integer = 1410

    Private _disposed As Boolean

    Private _hwnd As IntPtr

    Public ReadOnly Property Handle As IntPtr
        Get
            Return _hwnd
        End Get
    End Property

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Private Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
            End If

            If _hwnd <> IntPtr.Zero Then
                DestroyWindow(_hwnd)
                _hwnd = IntPtr.Zero
            End If
        End If
    End Sub

    Public Sub New(className As String, style As Integer, extendStyle As Integer, Optional parent As PureWin32Window = Nothing)
        If String.IsNullOrEmpty(className) Then
            Throw New ArgumentException(NameOf(className))
        End If

        _wndProc = AddressOf CustomWndProc
        Dim wind_class As New WindowClass With {
            .ClassName = className,
            .WndProc = _wndProc
        }
        Dim class_atom As UInt16 = RegisterClass(wind_class)
        Dim last_error As Integer = Marshal.GetLastWin32Error()
        If class_atom = 0 AndAlso last_error <> ERROR_CLASS_ALREADY_EXISTS Then
            Throw New Exception("Could not register window class")
        End If

        _hwnd = CreateWindowEx(extendStyle, className, String.Empty, style,
                               Integer.MinValue, 0, Integer.MinValue, 0,
                               (parent?.Handle).GetValueOrDefault, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
        If _hwnd = Nothing Then
            Throw New Win32Exception
        End If
    End Sub

    Private Shared Function CustomWndProc(hWnd As IntPtr, msg As UInteger, wParam As IntPtr, lParam As IntPtr) As IntPtr
        Return DefWindowProc(hWnd, msg, wParam, lParam)
    End Function

    Public Sub SetParent(parent As PureWin32Window)
        Dim hwnd = SetParent(Handle, parent.Handle)
        If hwnd = Nothing Then
            Throw New Win32Exception
        End If
    End Sub

    Public Sub SetChild(child As Window)
        Dim hwnd = SetParent(New Interop.WindowInteropHelper(child).Handle, Handle)
        If hwnd = Nothing Then
            Throw New Win32Exception
        End If
    End Sub

    Private _wndProc As WndProc
End Class
