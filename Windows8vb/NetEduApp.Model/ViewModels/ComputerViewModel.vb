Namespace ViewModels
    Public Class ComputerViewModel
        Inherits DeviceViewModel

        Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Computer.png"

        Public Sub New(lab As Laboratory)
            MyBase.New(lab)
        End Sub

        Protected Overrides Function GetNamePattern() As String
            Return "Computer{0}"
        End Function
    End Class
End Namespace
