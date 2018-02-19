Imports System.Reflection
Imports Nukepayload2.UI.Win32

Class App
    Inherits Application

    Dim wnd As New InteropWindow With {
        .Content = New Frame
    }

    '''<summary>
    '''Application Entry Point.
    '''</summary>
    <STAThread>
    Public Shared Sub Main()
        Dim app As New App
        app.Run()
    End Sub

    Private Sub App_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        Dim rootFrame = DirectCast(wnd.Content, Frame)
        rootFrame.Navigate(New Uri("pack://application:,,,/Views/MainPage.xaml"))
        wnd.SetBlurBehind(True)
    End Sub

    Private Sub App_NavigationFailed(sender As Object, e As NavigationFailedEventArgs) Handles Me.NavigationFailed
        Throw New Exception("Failed to load Page " + e.Uri.ToString)
    End Sub
End Class
