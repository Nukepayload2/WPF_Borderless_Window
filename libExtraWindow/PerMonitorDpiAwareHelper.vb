Imports System.ComponentModel

Friend Class PerMonitorDpiAwareHelper

    Public Property DpiAwareness As ProcessDpiAwareness
        Get
            Dim hProc = Process.GetCurrentProcess.Handle
            Dim aware As ProcessDpiAwareness
            Dim result = GetProcessDpiAwareness(hProc, aware)
            If result <> 0 Then
                Throw New Win32Exception
            End If
            Return aware
        End Get
        Set(value As ProcessDpiAwareness)
            Dim result = SetProcessDpiAwareness(value)
            If result <> 0 Then
                Throw New Win32Exception
            End If
        End Set
    End Property

    Public Function GetWindowDpi(hWnd As IntPtr) As Vector
        Dim hMonitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST)
        Dim dpiX, dpiY As UInteger
        If GetDpiForMonitor(hMonitor, MonitorDpiType.MDT_EFFECTIVE_DPI, dpiX, dpiY) < 0 Then
            dpiX = 96UI
            dpiY = 96UI
        End If
        Return New Vector(dpiX, dpiY)
    End Function

End Class

