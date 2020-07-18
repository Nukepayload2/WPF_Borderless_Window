Public Class LeftBarModel
    Public ReadOnly Property Symbol$
    Public ReadOnly Property Text$
    Public ReadOnly Property AssocPage As Page

    Sub New(Symbol$, Text$, AssocPage As Page)
        Me.Symbol = Symbol
        Me.Text = Text
        Me.AssocPage = AssocPage
    End Sub
End Class
