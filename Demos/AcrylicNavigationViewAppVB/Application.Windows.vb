Partial Class Application

    ' Singleton windows and pages

#Region "MainWindow single instance"
    Private _MainWindow As MainWindow

    Public Shadows ReadOnly Property MainWindow As MainWindow
        Get
            If _MainWindow Is Nothing Then
                _MainWindow = New MainWindow
                AddHandler _MainWindow.Closed, AddressOf MainWindowClosed
            End If

            Return _MainWindow
        End Get
    End Property

    Private Sub MainWindowClosed(sender As Object, e As EventArgs)
        RemoveHandler _MainWindow.Closed, AddressOf MainWindowClosed
        _MainWindow = Nothing
    End Sub
#End Region

#Region "MainPage single instance"
    Private _MainPage As MainPage

    Public ReadOnly Property MainPage As MainPage
        Get
            If _MainPage Is Nothing Then
                _MainPage = New MainPage
            End If

            Return _MainPage
        End Get
    End Property

#End Region

#Region "SettingsPage single instance"
    Private _SettingsPage As SettingsPage

    Public ReadOnly Property SettingsPage As SettingsPage
        Get
            If _SettingsPage Is Nothing Then
                _SettingsPage = New SettingsPage
            End If

            Return _SettingsPage
        End Get
    End Property
#End Region

End Class
