Namespace Models
    Public Class Hub
        Inherits VisualLabElement

		Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Hub.png"

		Public Sub New(lab As Laboratory)
			MyBase.New(lab)
		End Sub

		Protected Overrides Function GetNamePattern() As String
            Return "Hub{0}"
        End Function
    End Class
End Namespace
