Namespace ViewModels
    Public Class SwitchViewModel
        Inherits DeviceViewModel

        Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Switch.png"

		Public Sub New(lab As Laboratory)
			MyBase.New(lab)

			Interfaces.Add("eth0")
			Interfaces.Add("eth1")
			Interfaces.Add("eth2")
			Interfaces.Add("eth3")
			Interfaces.Add("eth4")
			Interfaces.Add("eth5")
			Interfaces.Add("eth6")
			Interfaces.Add("eth7")
		End Sub

        Protected Overrides Function GetNamePattern() As String
            Return "Switch{0}"
        End Function
    End Class
End Namespace
