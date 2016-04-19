Namespace Models
    Public Class AccessPointViewModel
        Inherits DeviceViewModel

        Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Switch.png"

        Public Sub New(lab As Laboratory)
            MyBase.New(lab)
        End Sub

        Protected Overrides Function GetNamePattern() As String
            Return "AccessPoint{0}"
        End Function
    End Class
End Namespace
