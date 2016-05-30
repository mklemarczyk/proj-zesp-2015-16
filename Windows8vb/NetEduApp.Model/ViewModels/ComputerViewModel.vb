Namespace ViewModels
    Public Class ComputerViewModel
        Inherits DeviceViewModel

        Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Computer.png"

		Public ReadOnly Property Interfaces As List(Of String) = New List(Of String)

		Public Sub New(lab As Laboratory)
			MyBase.New(lab)

			Interfaces.Add("eth0")
			Interfaces.Add("eth1")
			Interfaces.Add("eth2")
			Interfaces.Add("eth3")
		End Sub

        Protected Overrides Function GetNamePattern() As String
            Return "Computer{0}"
        End Function
    End Class
End Namespace
