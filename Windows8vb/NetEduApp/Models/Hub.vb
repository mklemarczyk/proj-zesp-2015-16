Namespace Models
    Public Class Hub
        Inherits VisualLabElement

        Protected Shared Names As HashSet(Of String) = New HashSet(Of String)
        Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Hub.png"

        Protected Overrides Function GetNames() As ICollection(Of String)
            Return Names
        End Function
        Protected Overrides Function GetNamePattern() As String
            Return "Hub{0}"
        End Function
    End Class
End Namespace
