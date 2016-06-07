Imports NetEduApp.Model.Common
Imports NetEduApp.Emulators.Network

Namespace ViewModels.Config
	Public MustInherit Class ConfigViewModelBase
		Inherits ViewModelBase

		Private _Device As DeviceViewModel
		Private _NavigationHelper As NavigationHelper

		Public ReadOnly Property SaveCommand As RelayCommand
		Public ReadOnly Property CancelCommand As RelayCommand

		Public Sub New(navigationHelper As NavigationHelper)
			Me._NavigationHelper = navigationHelper
			Me.SaveCommand = New RelayCommand(AddressOf SaveAction, AddressOf CanSave)
			Me.CancelCommand = New RelayCommand(AddressOf CancelAction)
		End Sub

		Protected Overridable Sub SaveAction()
			NavigationHelper.GoBack()
		End Sub

		Protected Overridable Function CanSave() As Boolean
			Return True
		End Function

		Protected Overridable Sub CancelAction()
			OnDeviceChanged()
			NavigationHelper.GoBack()
		End Sub

		Protected Overridable Sub OnDeviceChanged()
			SaveCommand.RaiseCanExecuteChanged()
		End Sub

		Public Property Device As DeviceViewModel
			Get
				Return _Device
			End Get
			Set(value As DeviceViewModel)
				If value IsNot _Device Then
					Me._Device = value
					OnDeviceChanged()
				End If
			End Set
		End Property

		Public ReadOnly Property NavigationHelper As NavigationHelper
			Get
				Return _NavigationHelper
			End Get
		End Property

	End Class
End Namespace
