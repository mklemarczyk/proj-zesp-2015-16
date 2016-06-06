Namespace ViewModels
    Public Class RouterViewModel
        Inherits DeviceViewModel

		Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Router.png"

		Public Sub New(lab As Laboratory)
			MyBase.New(lab)

			Interfaces.Add("eth0")
			Interfaces.Add("eth1")
			Interfaces.Add("eth2")
			Interfaces.Add("eth3")
			VisibleInterfaces.Add("eth0")
			VisibleInterfaces.Add("eth1")
			VisibleInterfaces.Add("eth2")
			VisibleInterfaces.Add("eth3")
		End Sub

        Protected Overrides Function GetNamePattern() As String
            Return "Router{0}"
        End Function
    End Class
End Namespace
