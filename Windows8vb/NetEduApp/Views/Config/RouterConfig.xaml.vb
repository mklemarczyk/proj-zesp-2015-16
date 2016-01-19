' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class RouterConfig
    Inherits Page

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.IsEnabled = False
    End Sub
End Class
