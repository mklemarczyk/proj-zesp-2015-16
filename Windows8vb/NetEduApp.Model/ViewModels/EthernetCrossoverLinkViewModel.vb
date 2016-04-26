Imports Windows.UI

Namespace ViewModels
    Public Class EthernetCrossoverLinkViewModel
        Inherits LinkViewModel
        Public Sub New()
            Color = New SolidColorBrush(Colors.Black)
            LineStyle = New DoubleCollection()
            LineStyle.Add(7)
            LineStyle.Add(3)
        End Sub
    End Class
End Namespace
