<Assembly: DisableDpiAwareness>

Class MainWindow
    Private Sub TitleBarDragElement_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles TitleBarDragElement.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub BtnAbout_Click(sender As Object, e As RoutedEventArgs) Handles BtnAbout.Click
        MsgBox("Version: " + My.Application.Info.Version.ToString, MsgBoxStyle.Information)
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub
End Class
