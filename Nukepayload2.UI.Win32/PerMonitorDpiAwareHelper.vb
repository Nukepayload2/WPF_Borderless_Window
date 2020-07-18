Imports System.ComponentModel

Public Class PerMonitorDpiAwareHelper

    Public Property DpiAwareness As ProcessDpiAwareness?
        Get
            Dim hProc = Process.GetCurrentProcess.Handle
            Dim aware As ProcessDpiAwareness
            Try
                Dim result = GetProcessDpiAwareness(hProc, aware)
                If result <> 0 Then
                    Throw New Win32Exception
                End If
            Catch ex As DllNotFoundException
                Return Nothing
            End Try
            Return aware
        End Get
        Set(value As ProcessDpiAwareness?)
            If value Is Nothing Then
                Return
            End If
            Dim result = SetProcessDpiAwareness(value.Value)
            If result <> 0 Then
                Throw New Win32Exception
            End If
        End Set
    End Property

    Public Function GetWindowDpi(hWnd As IntPtr) As Vector?
        Dim hMonitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST)
        Dim dpiX, dpiY As UInteger
        Try
            If GetDpiForMonitor(hMonitor, MonitorDpiType.MDT_EFFECTIVE_DPI, dpiX, dpiY) < 0 Then
                dpiX = 96UI
                dpiY = 96UI
            End If
        Catch ex As DllNotFoundException
            Return Nothing
        End Try
        Return New Vector(dpiX, dpiY)
    End Function

End Class
