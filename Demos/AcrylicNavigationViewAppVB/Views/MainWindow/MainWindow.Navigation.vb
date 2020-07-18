Imports System.Windows.Media.Animation

Partial Class MainWindow

    Private ReadOnly _selectedIndexes As New Stack(Of Integer)
    Private ReadOnly _scaleAnimRev As New DoubleAnimation(1.03, 1, TimeSpan.FromMilliseconds(120))
    Private ReadOnly _scaleAnim As New DoubleAnimation(0.93, 1, TimeSpan.FromMilliseconds(120))
    Private ReadOnly _renderScale As New ScaleTransform

    Private _suppressAutoNavigate As Boolean

    Private Sub LstLeftBar_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles LstLeftBar.SelectionChanged
        If LstLeftBar.SelectedItem IsNot Nothing AndAlso Not _suppressAutoNavigate Then
            Dim sel = DirectCast(LstLeftBar.SelectedItem, LeftBarModel)
            Dim assocPage = sel.AssocPage
            Navigate(assocPage)
        End If
    End Sub

    Private Sub Navigate(assocPage As Page)
        _renderScale.BeginAnimation(ScaleTransform.ScaleXProperty, _scaleAnim)
        _renderScale.BeginAnimation(ScaleTransform.ScaleYProperty, _scaleAnim)
        MainFrame.Navigate(assocPage)
    End Sub

    Private Sub BtnToggleWidth_Click(sender As Object, e As RoutedEventArgs) Handles BtnToggleWidth.Click
        If GrdSplit.ColumnDefinitions(0).ActualWidth <> 48 Then
            GrdSplit.ColumnDefinitions(0).Width = New GridLength(48)
        Else
            GrdSplit.ColumnDefinitions(0).Width = New GridLength(1, GridUnitType.Auto)
        End If
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        LstLeftBar.SelectedIndex = 0
        MainFrame.RenderTransform = _renderScale
    End Sub

    Private Sub Backward(sender As Object, e As RoutedEventArgs)
        If MainFrame.CanGoBack Then
            _renderScale.BeginAnimation(ScaleTransform.ScaleXProperty, _scaleAnimRev)
            _renderScale.BeginAnimation(ScaleTransform.ScaleYProperty, _scaleAnimRev)
            MainFrame.GoBack()
        End If
    End Sub

    Private Sub BtnSetting_Click(sender As Object, e As RoutedEventArgs)
        If LstLeftBar.SelectedIndex = -1 Then
            Return
        End If
        Navigate(My.Application.SettingsPage)
        LstLeftBar.SelectedIndex = -1
    End Sub

    Private Sub MainFrame_Navigating(sender As Object, e As NavigatingCancelEventArgs) Handles MainFrame.Navigating
        rectSettingSelected.Visibility = If(TypeOf e.Content Is SettingsPage, Visibility.Visible, Visibility.Collapsed)
        If e.NavigationMode = NavigationMode.New Then
            _selectedIndexes.Push(LstLeftBar.SelectedIndex)
        Else
            _suppressAutoNavigate = True
            If _selectedIndexes.Count > 0 Then
                _selectedIndexes.Pop()
                LstLeftBar.SelectedIndex = If(TypeOf e.Content Is SettingsPage, -1, _selectedIndexes.Peek)
            End If
            _suppressAutoNavigate = False
        End If
    End Sub
End Class
