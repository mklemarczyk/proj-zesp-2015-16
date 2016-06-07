Imports NetEduApp.Model.Common
Imports NetEduApp.Emulators.Network

Namespace ViewModels.Config
	Public Class GeneralConfigViewModel
		Inherits ConfigViewModelBase

		Private _Name As String = String.Empty
		Private _DefaultGatewayAddress As String = String.Empty

		Public Sub New(navigationHelper As NavigationHelper)
			MyBase.New(navigationHelper)
		End Sub

		Protected Overrides Sub OnDeviceChanged()
			If Device IsNot Nothing Then
				Me.Name = Me.Device.Name
				Me.DefaultGatewayAddress = Me.Device.DefaultGateway.ToString()
			Else
				Me.Name = String.Empty
				Me.DefaultGatewayAddress = String.Empty
			End If

			MyBase.OnDeviceChanged()
		End Sub

		Protected Overrides Sub SaveAction()
			Me.Device.Name = Me._Name
			If Not String.IsNullOrEmpty(Me.DefaultGatewayAddress) Then
				Dim ipAddress As NetIpAddress
				NetIpAddress.TryParse(Me.DefaultGatewayAddress, ipAddress)
				Me.Device.DefaultGateway = ipAddress
			Else
				Me.Device.DefaultGateway = Nothing
			End If

			MyBase.SaveAction()
		End Sub

		Protected Overrides Function CanSave() As Boolean
			Dim ipAddress As NetIpAddress
			Return Not String.IsNullOrWhiteSpace(Me._Name) AndAlso (String.IsNullOrEmpty(Me.DefaultGatewayAddress) OrElse NetIpAddress.TryParse(Me.DefaultGatewayAddress, ipAddress))
		End Function

		Public Property Name As String
			Get
				Return Me._Name
			End Get
			Set(value As String)
				Me._Name = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(Name))
			End Set
		End Property

		Public Property DefaultGatewayAddress() As String
			Get
				Return Me._DefaultGatewayAddress
			End Get
			Set(ByVal value As String)
				Me._DefaultGatewayAddress = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged()
			End Set
		End Property

	End Class
End Namespace
