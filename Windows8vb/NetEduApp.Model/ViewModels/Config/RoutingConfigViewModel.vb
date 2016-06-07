Imports NetEduApp.Model.Common
Imports NetEduApp.Emulators.Network

Namespace ViewModels.Config
	Public Class RoutingConfigViewModel
		Inherits ConfigViewModelBase

		Private _Routes As ObservableCollection(Of RouteViewModel)
		Private _SelectedRoute As RouteViewModel

		Private _IpAddress As String = String.Empty
		Private _IpSubnetMask As String = String.Empty
		Private _TargetAddress As String = String.Empty

		Public Sub New(navigationHelper As NavigationHelper)
			MyBase.New(navigationHelper)
		End Sub

		Protected Overrides Sub OnDeviceChanged()
			If Device IsNot Nothing Then
				Routes = New ObservableCollection(Of RouteViewModel)(Me.Device.Routes)
			Else
				Routes = Nothing
			End If
			SelectedRoute = Nothing

			MyBase.OnDeviceChanged()
		End Sub

		Protected Overridable Sub OnSelectedRouteChanged()
			If SelectedRoute IsNot Nothing Then
				Me.IpAddress = SelectedRoute.SubnetAddress.Address.ToString()
				Me.IpSubnetMask = SelectedRoute.SubnetAddress.Netmask.ToString()
				Me.TargetAddress = SelectedRoute.TargetAddress.ToString()
			End If

			Me.SaveCommand.RaiseCanExecuteChanged()
			Me.RaisePropertyChanged(NameOf(SelectedRoute))
		End Sub

		Protected Overrides Sub SaveAction()
			Me.Device.Name = Me._TargetAddress
			MyBase.SaveAction()
		End Sub

		Protected Overrides Function CanSave() As Boolean
			Return Not String.IsNullOrWhiteSpace(Me._TargetAddress)
		End Function

		Public Property Routes As ObservableCollection(Of RouteViewModel)
			Get
				Return Me._Routes
			End Get
			Set(value As ObservableCollection(Of RouteViewModel))
				Me._Routes = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(Routes))
			End Set
		End Property

		Public Property SelectedRoute As RouteViewModel
			Get
				Return Me._SelectedRoute
			End Get
			Set(value As RouteViewModel)
				Me._SelectedRoute = value
				Me.OnSelectedRouteChanged()
			End Set
		End Property

		Public Property IpAddress As String
			Get
				Return Me._IpAddress
			End Get
			Set(value As String)
				Me._IpAddress = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(IpAddress))
			End Set
		End Property

		Public Property IpSubnetMask As String
			Get
				Return Me._IpSubnetMask
			End Get
			Set(value As String)
				Me._IpSubnetMask = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(IpSubnetMask))
			End Set
		End Property

		Public Property TargetAddress As String
			Get
				Return Me._TargetAddress
			End Get
			Set(value As String)
				Me._TargetAddress = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(TargetAddress))
			End Set
		End Property

	End Class
End Namespace
