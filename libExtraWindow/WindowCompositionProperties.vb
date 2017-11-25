Imports Nukepayload2.UI.Win32

Namespace Global.Nukepayload2.UI.Xaml
    Public Class WindowCompositionProperties
        Inherits DependencyObject

        Public Shared Function GetIsBlurred(ByVal element As DependencyObject) As Boolean
            If element Is Nothing Then
                Throw New ArgumentNullException(NameOf(element))
            End If
            Return CBool(element.GetValue(IsBlurredProperty))
        End Function

        Public Shared Sub SetIsBlurred(ByVal element As DependencyObject, ByVal value As Boolean)
            If element Is Nothing Then
                Throw New ArgumentNullException(NameOf(element))
            End If
            element.SetValue(IsBlurredProperty, value)
        End Sub

        Public Shared ReadOnly IsBlurredProperty As _
                           DependencyProperty = DependencyProperty.RegisterAttached("IsBlurred",
                           GetType(Boolean), GetType(WindowCompositionProperties),
                           New PropertyMetadata(False,
                                                Sub(s, e)
                                                    Dim wnd = TryCast(s, Window)
                                                    If wnd Is Nothing Then
                                                        Return
                                                    End If
                                                    Dim composition As New WindowComposition
                                                    composition.SetBlur(wnd, CBool(e.NewValue))
                                                End Sub,
                                                Function(s, v)
                                                    Dim wnd = TryCast(s, Window)
                                                    If wnd Is Nothing Then
                                                        Return v
                                                    End If
                                                    Dim composition As New WindowComposition
                                                    Dim blurred = composition.IsBlurred(wnd)
                                                    Dim setBlurred = CBool(v)
                                                    If wnd.IsLoaded Then
                                                        composition.SetBlur(wnd, setBlurred)
                                                    Else
                                                        Dim blur As RoutedEventHandler =
                                                                Sub()
                                                                    composition.SetBlur(wnd, setBlurred)
                                                                    RemoveHandler wnd.Loaded, blur
                                                                End Sub
                                                        AddHandler wnd.Loaded, blur
                                                    End If
                                                    Return blurred
                                                End Function))

    End Class
End Namespace