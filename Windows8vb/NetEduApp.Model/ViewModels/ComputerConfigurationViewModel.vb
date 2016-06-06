Imports NetEduApp.Model.Common
Imports NetEduApp.Emulators.Network

Namespace ViewModels
	Public Class ComputerConfigurationViewModel
		Inherits ViewModelBase

		Private computer As DeviceViewModel

		Private _Name As String = String.Empty
		Private _Address As String = String.Empty
		Private _Netmask As String = String.Empty
		Private _DefaultGateway As String = String.Empty

		Public Sub New(computer As DeviceViewModel)
			Me.computer = computer
			Me.ApplySettingsCommand = New RelayCommand(AddressOf ApplySettingsAction, AddressOf CanApplySettings)

			Me._Name = Me.computer.Name
			If Me.computer.Settings IsNot Nothing Then
				Me._Address = Me.computer.Settings("Address")
				Me._Netmask = Me.computer.Settings("Netmask")
				Me._DefaultGateway = Me.computer.Settings("DefaultGateway")
			End If
		End Sub

		Private Sub ApplySettingsAction()
			Me.computer.Name = Me._Name
			If Me.computer.Settings Is Nothing Then
				Me.computer.Settings = New Dictionary(Of String, String)
			End If
			Me.computer.Settings("Address") = Me._Address
			Me.computer.Settings("Netmask") = Me._Netmask
			Me.computer.Settings("DefaultGateway") = Me._DefaultGateway
		End Sub

		Private Function CanApplySettings() As Boolean
			Dim ValidAddress As NetIpAddress = Nothing
			Dim ValidNetmask As NetIpAddress = Nothing
			Dim ValidDefaultGateway As NetIpAddress = Nothing
			If Not String.IsNullOrWhiteSpace(Name) Then
				If NetIpAddress.TryParse(Address, ValidAddress) AndAlso NetIpAddress.TryParse(Netmask, ValidNetmask) AndAlso NetIpAddress.TryParse(DefaultGateway, ValidDefaultGateway) Then
					Return True
				End If
			End If
			Return False
		End Function

		Public Property ApplySettingsCommand As RelayCommand

		Public Property Name As String
			Get
				Return Me._Name
			End Get
			Set(value As String)
				Me._Name = value
				Me.ApplySettingsCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(Name))
			End Set
		End Property
		Public Property Address As String
			Get
				Return Me._Address
			End Get
			Set(value As String)
				Me._Address = value
				Me.ApplySettingsCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(Address))
			End Set
		End Property
		Public Property Netmask As String
			Get
				Return Me._Netmask
			End Get
			Set(value As String)
				Me._Netmask = value
				Me.ApplySettingsCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(Netmask))
			End Set
		End Property
		Public Property DefaultGateway As String
			Get
				Return Me._DefaultGateway
			End Get
			Set(value As String)
				Me._DefaultGateway = value
				Me.ApplySettingsCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(DefaultGateway))
			End Set
		End Property
	End Class
End Namespace
