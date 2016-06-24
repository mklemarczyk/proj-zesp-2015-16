Imports NetEduApp.Simulator
Imports NetEduApp.Model.Common
Imports Windows.UI

Namespace ViewModels
	Public Class RouteViewModel
		Inherits ViewModelBase

		Private _SubnetAddress As NetAddress
		Private _TargetAddress As NetIpAddress

		Public Property SubnetAddress As NetAddress
			Get
				Return _SubnetAddress
			End Get
			Set(value As NetAddress)
				_SubnetAddress = value
				RaisePropertyChanged()
			End Set
		End Property

		Public Property TargetAddress As NetIpAddress
			Get
				Return _TargetAddress
			End Get
			Set(value As NetIpAddress)
				_TargetAddress = value
				RaisePropertyChanged()
			End Set
		End Property

	End Class
End Namespace
