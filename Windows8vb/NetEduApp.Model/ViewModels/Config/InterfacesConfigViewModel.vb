﻿Imports NetEduApp.Model.Common
Imports NetEduApp.Simulator

Namespace ViewModels.Config
	Public Class InterfacesConfigViewModel
		Inherits ConfigViewModelBase

		Private _Interfaces As ObservableCollection(Of InterfaceViewModel)
		Private _SelectedInterface As InterfaceViewModel

		Private _Name As String = String.Empty
		Private _MacAddress As String = String.Empty
		Private _IpAddress As String = String.Empty
		Private _IpSubnetMask As String = String.Empty
		Private _IpBroadcast As String = String.Empty

		Public Sub New(navigationHelper As NavigationHelper)
			MyBase.New(navigationHelper)
		End Sub

		Protected Overrides Sub OnDeviceChanged()
			If Device IsNot Nothing Then
				Interfaces = New ObservableCollection(Of InterfaceViewModel)(Me.Device.Interfaces)
			Else
				Interfaces = Nothing
			End If
			SelectedInterface = Nothing

			MyBase.OnDeviceChanged()
		End Sub

		Protected Overridable Sub OnSelectedInterfaceChanged()
			If SelectedInterface IsNot Nothing Then
				Me.Name = SelectedInterface.Name
				Me.MacAddress = SelectedInterface.MacAddress.ToString()
				If SelectedInterface.IpAddress.HasValue Then
					Me.IpAddress = SelectedInterface.IpAddress.Value.Address.ToString()
					Me.IpSubnetMask = SelectedInterface.IpAddress.Value.Netmask.ToString()
					Me.IpBroadcast = SelectedInterface.IpAddress.Value.Broadcast.ToString()
				Else
					Me.IpAddress = String.Empty
					Me.IpSubnetMask = String.Empty
					Me.IpBroadcast = String.Empty
				End If
			Else
				Me.Name = String.Empty
				Me.MacAddress = String.Empty
				Me.IpAddress = String.Empty
				Me.IpSubnetMask = String.Empty
				Me.IpBroadcast = String.Empty
			End If

			Me.SaveCommand.RaiseCanExecuteChanged()
			Me.RaisePropertyChanged(NameOf(SelectedInterface))
		End Sub

		Protected Overrides Sub SaveAction()
			If String.IsNullOrEmpty(Me.IpAddress) Then
				SelectedInterface.IpAddress = Nothing
			Else
				Dim ipAddress, ipSubnetMask, ipBroadcast As NetIpAddress
				NetIpAddress.TryParse(Me.IpAddress, ipAddress)
				NetIpAddress.TryParse(Me.IpSubnetMask, ipSubnetMask)
				NetIpAddress.TryParse(Me.IpBroadcast, ipBroadcast)
				SelectedInterface.IpAddress = New NetAddress(ipAddress, ipSubnetMask, ipBroadcast)
			End If
			SelectedInterface = Nothing
		End Sub

		Protected Overrides Function CanSave() As Boolean
			Dim ipAddress, ipSubnetMask, ipBroadcast As NetIpAddress
			If SelectedInterface IsNot Nothing Then
				If (String.IsNullOrEmpty(Me.IpAddress) And
					String.IsNullOrEmpty(Me.IpSubnetMask) And
					String.IsNullOrEmpty(Me.IpBroadcast)) Then
					Return True
				ElseIf (NetIpAddress.TryParse(Me.IpAddress, ipAddress) AndAlso
						NetIpAddress.TryParse(Me.IpSubnetMask, ipSubnetMask) AndAlso
						NetIpAddress.TryParse(Me.IpBroadcast, ipBroadcast) AndAlso
						(New NetAddress(ipAddress, ipSubnetMask, ipBroadcast)).IsValid() AndAlso
						(New NetAddress(ipAddress, ipSubnetMask, ipBroadcast)).IsHost()) Then
					Return True
				End If
			End If
			Return False
		End Function

		Protected Overrides Sub CancelAction()
			Me.SelectedInterface = Nothing
		End Sub

		Public Property Interfaces As ObservableCollection(Of InterfaceViewModel)
			Get
				Return Me._Interfaces
			End Get
			Set(value As ObservableCollection(Of InterfaceViewModel))
				Me._Interfaces = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(Interfaces))
			End Set
		End Property

		Public Property SelectedInterface As InterfaceViewModel
			Get
				Return Me._SelectedInterface
			End Get
			Set(value As InterfaceViewModel)
				Me._SelectedInterface = value
				Me.OnSelectedInterfaceChanged()
			End Set
		End Property

		Public Property Name As String
			Get
				Return Me._Name
			End Get
			Protected Set(value As String)
				Me._Name = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(Name))
			End Set
		End Property

		Public Property MacAddress As String
			Get
				Return Me._MacAddress
			End Get
			Protected Set(value As String)
				Me._MacAddress = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(MacAddress))
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

		Public Property IpBroadcast As String
			Get
				Return Me._IpBroadcast
			End Get
			Set(value As String)
				Me._IpBroadcast = value
				Me.SaveCommand.RaiseCanExecuteChanged()
				Me.RaisePropertyChanged(NameOf(IpBroadcast))
			End Set
		End Property
	End Class
End Namespace
