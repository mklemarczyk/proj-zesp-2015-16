Imports Windows.UI

Namespace ViewModels
	Public Class InterfaceViewModel

		Public Sub New(Name As String)
			Me.Name = Name
		End Sub

		Public Property Name As String
		Public Property HardwareAddress As String
		Public Property IpAddress As String
		Public Property IpMask As String

	End Class
End Namespace
