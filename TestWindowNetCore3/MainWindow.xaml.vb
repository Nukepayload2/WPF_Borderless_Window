Imports System.Reflection
Imports Nukepayload2.UI.Win32

Class MainWindow
    Private Sub TitleBarDragElement_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles TitleBarDragElement.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub BtnAbout_Click(sender As Object, e As RoutedEventArgs) Handles BtnAbout.Click
        Dim version As String = GetType(Win32ApiInformation).Assembly.GetCustomAttribute(Of AssemblyFileVersionAttribute)?.Version
        MsgBox("Version: " & version, MsgBoxStyle.Information)
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles BtnClose.Click
        Close()
    End Sub

    Private Sub MainWindow_SourceInitialized(sender As Object, e As EventArgs) Handles Me.SourceInitialized
        Dim windowCompositionFactory As New WindowCompositionFactory
        If Win32ApiInformation.IsWindowAcrylicApiPresent OrElse Win32ApiInformation.IsAeroGlassApiPresent Then
            ' Enable blur effect
            Dim composition = windowCompositionFactory.TryCreateForCurrentView
            If composition?.TrySetBlur(Me, True) Then
                TitleBar.Background = New SolidColorBrush(Color.FromArgb(&H99, &HFF, &HFF, &HFF))
                ClientArea.Background = New SolidColorBrush(Color.FromArgb(&HCC, &HFF, &HFF, &HFF))
                Background = Brushes.Transparent
                ChkBlured.IsChecked = True
            End If
        End If
        If Win32ApiInformation.IsProcessDpiAwarenessApiPresent Then
            ' Check DPI awareness
            ChkDpiAware.IsChecked = DpiAwareness = ProcessDpiAwareness.PerMonitorDpiAware
        End If
    End Sub

    Private Sub ChkBlured_Click(sender As Object, e As RoutedEventArgs) Handles ChkBlured.Click
        Dim windowCompositionFactory As New WindowCompositionFactory
        Dim composition = windowCompositionFactory.TryCreateForCurrentView
        Dim blur = ChkBlured.IsChecked.GetValueOrDefault
        If composition?.TrySetBlur(Me, blur) Then

        End If
    End Sub

    Private Sub BtnMinimize_Click(sender As Object, e As RoutedEventArgs) Handles BtnMinimize.Click
        WindowState = WindowState.Minimized
    End Sub

    Private Sub BtnMaximize_Click(sender As Object, e As RoutedEventArgs) Handles BtnMaximize.Click
        If WindowState = WindowState.Maximized Then
            WindowState = WindowState.Normal
            BtnMaximize.ToolTip = "Maximize"
        Else
            WindowState = WindowState.Maximized
            BtnMaximize.ToolTip = "Restore"
        End If
    End Sub
End Class
