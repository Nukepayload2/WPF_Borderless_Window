Public Class LeftBarViewModel
    Public ReadOnly Property Data As New List(Of LeftBarModel) From {
        New LeftBarModel(ChrW(&HE18A), "Main", My.Application?.MainPage)
    }
End Class
