Public Class RelayCommand
    Implements ICommand

    Public Property Exec As Action(Of Object)
    Public Property CanExec As Func(Of Object, Boolean)

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Exec?.Invoke(parameter)
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return If(CanExec?.Invoke(parameter), True)
    End Function

    Public Sub CanExecChanged()
        RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
    End Sub
End Class
