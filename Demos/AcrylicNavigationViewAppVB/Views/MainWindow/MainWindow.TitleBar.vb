Partial Class MainWindow
    Private Sub Rectangle_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        DragMove()
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles BtnClose.Click
        Close()
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
