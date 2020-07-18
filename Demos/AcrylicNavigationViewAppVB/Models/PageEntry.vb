Public Class PageEntry
    Sub New(title As String, description As String, page As Page)
        Me.Title = title
        Me.Description = description
        Me.Page = page
    End Sub
    Public ReadOnly Property Title As String
    Public ReadOnly Property Description As String
    Public ReadOnly Property Page As Page

    Private Shared ReadOnly s_navigateCommand As New NavigateToPageCommand
    Public ReadOnly Property Navigate As ICommand
        Get
            Return s_navigateCommand
        End Get
    End Property
End Class
