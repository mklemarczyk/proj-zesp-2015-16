Namespace ViewModels
    Public Class RouterViewModel
        Inherits DeviceViewModel

		Public Overrides ReadOnly Property ImagePath As String = "ms-appx:///Assets/Lab/Router.png"

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

			Dim route As RouteViewModel = New RouteViewModel()
			route.SubnetAddress = New Emulators.Network.NetAddress(New Emulators.Network.NetIpAddress(192, 168, 3, 0), New Emulators.Network.NetIpAddress(255, 255, 255, 0))
			route.TargetAddress = New Emulators.Network.NetIpAddress(192, 168, 0, 1)
			Routes.Add(route)
		End Sub

        Protected Overrides Function GetNamePattern() As String
            Return "Router{0}"
        End Function
    End Class
End Namespace
