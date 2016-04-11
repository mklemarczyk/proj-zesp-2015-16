Imports Windows.UI

Namespace Models
    Public Class EthernetCrossoverLink
        Inherits VisualLabLink
        Public Sub New()
            Color = New SolidColorBrush(Colors.Black)
            LineStyle = New DoubleCollection()
            LineStyle.Add(7)
            LineStyle.Add(3)
        End Sub
    End Class
End Namespace
