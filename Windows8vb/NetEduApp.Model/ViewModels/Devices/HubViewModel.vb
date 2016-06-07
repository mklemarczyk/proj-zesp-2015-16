Namespace ViewModels
    Public Class HubViewModel
        Inherits DeviceViewModel

        Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Hub.png"

		Public Sub New(lab As Laboratory)
			MyBase.New(lab)

			Interfaces.Add(New InterfaceViewModel("eth0"))
			Interfaces.Add(New InterfaceViewModel("eth1"))
			Interfaces.Add(New InterfaceViewModel("eth2"))
			Interfaces.Add(New InterfaceViewModel("eth3"))
			VisibleInterfaces.Add("eth0")
			VisibleInterfaces.Add("eth1")
			VisibleInterfaces.Add("eth2")
			VisibleInterfaces.Add("eth3")
		End Sub

        Protected Overrides Function GetNamePattern() As String
            Return "Hub{0}"
        End Function
    End Class
End Namespace
