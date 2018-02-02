Public Class Win32ApiInformation

    Public Shared Function IsAeroGlassApiPresent() As Boolean
        Return IsProcedurePresent("dwmapi.dll", NameOf(DwmEnableBlurBehindWindow))
    End Function

    Public Shared Function IsWindowAcrylicApiPresent() As Boolean
        Return Aggregate mdl As ProcessModule In Process.GetCurrentProcess.Modules
               Where mdl.ModuleName.Equals("user32.dll", StringComparison.OrdinalIgnoreCase) AndAlso
                     mdl.FileVersionInfo.ProductMajorPart >= 10
               Into Any
    End Function

    Public Shared Function IsProcessDpiAwarenessApiPresent() As Boolean
        Return IsProcedurePresent("Shcore.dll", NameOf(SetProcessDpiAwareness))
    End Function

    Public Shared Function IsProcedurePresent(library As String, procedure As String) As Boolean
        If String.IsNullOrWhiteSpace(library) Then
            Return False
        End If

        If String.IsNullOrWhiteSpace(procedure) Then
            Return False
        End If

        Dim hModule = LoadLibrary(library)
        If hModule = IntPtr.Zero Then
            Return False
        End If

        Dim proc = GetProcAddress(hModule, procedure)
        Return proc <> IntPtr.Zero
    End Function

End Class
