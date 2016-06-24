Imports NetEduApp.Simulator
Imports NetEduApp.Model.Common
Imports Windows.UI

Namespace ViewModels
	Public Class InterfaceViewModel
		Inherits ViewModelBase

		Private _Name As String = String.Empty
		Private _MacAddress As NetMacAddress
		Private _IpAddress As NetAddress?

		Public Sub New(Name As String)
			Me.Name = Name
		End Sub

		Public Property Name As String
			Get
				Return _Name
			End Get
			Set(value As String)
				_Name = value
			End Set
		End Property

		Public Property MacAddress As NetMacAddress
			Get
				Return _MacAddress
			End Get
			Set(value As NetMacAddress)
				_MacAddress = value
			End Set
		End Property

		Public Property IpAddress As NetAddress?
			Get
				Return _IpAddress
			End Get
			Set(value As NetAddress?)
				_IpAddress = value
			End Set
		End Property

	End Class
End Namespace
