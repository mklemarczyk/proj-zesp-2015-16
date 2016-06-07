Imports NetEduApp.Model.Common
Imports NetEduApp.Emulators.Network

Namespace ViewModels.Config
	Public Class GeneralConfigViewModel
		Inherits ConfigViewModelBase

		Private _Name As String = String.Empty

		Public Sub New(navigationHelper As NavigationHelper)
			MyBase.New(navigationHelper)
		End Sub

		Protected Overrides Sub OnDeviceChanged()
			If Device IsNot Nothing Then
				Me.Name = Me.Device.Name
			End If
		End Sub

		Protected Overrides Sub SaveAction()
			Me.Device.Name = Me._Name
			MyBase.SaveAction()
		End Sub

		Protected Overrides Function CanSave() As Boolean
			Return Not String.IsNullOrWhiteSpace(Me._Name)
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
	End Class
End Namespace
