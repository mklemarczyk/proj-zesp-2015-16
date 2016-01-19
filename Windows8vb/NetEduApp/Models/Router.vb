Namespace Models
    Public Class Router
        Inherits VisualLabElement

        Protected Shared Names As HashSet(Of String) = New HashSet(Of String)
        Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Router.png"

        Protected Overrides Function GetNames() As ICollection(Of String)
            Return Names
        End Function
        Protected Overrides Function GetNamePattern() As String
            Return "Router{0}"
        End Function
    End Class
End Namespace
