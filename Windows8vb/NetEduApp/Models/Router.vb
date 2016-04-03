Namespace Models
    Public Class Router
        Inherits VisualLabElement

		Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Router.png"

		Public Sub New(lab As Laboratory)
			MyBase.New(lab)
		End Sub

		Protected Overrides Function GetNamePattern() As String
            Return "Router{0}"
        End Function
    End Class
End Namespace
