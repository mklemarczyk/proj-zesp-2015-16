Imports NetEduApp.Model.Common

Namespace ViewModels
	Public MustInherit Class LinkViewModel
		Inherits ViewModelBase

		Private _ItemA As DeviceViewModel
		Private _ItemB As DeviceViewModel

		Public Property Color As Brush
		Public Property LineStyle As DoubleCollection
		Public Property InterfA As String
		Public Property InterfB As String

		Public Property ItemA As DeviceViewModel
			Get
				Return _ItemA
			End Get
			Set(value As DeviceViewModel)
				If value IsNot _ItemA Then
					_ItemA = value
					RaisePropertyChanged("ItemA")
				End If
			End Set
		End Property
		Public Property ItemB As DeviceViewModel
			Get
				Return _ItemB
			End Get
			Set(value As DeviceViewModel)
				If value IsNot _ItemB Then
					_ItemB = value
					RaisePropertyChanged("ItemB")
				End If
			End Set
		End Property

#Region "IDisposable Support"
		Private disposedValue As Boolean ' To detect redundant calls

		' IDisposable
		Protected Overrides Sub Dispose(disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					_ItemA = Nothing
					_ItemB = Nothing
					Color = Nothing
					LineStyle = Nothing
					InterfA = Nothing
					InterfB = Nothing
				End If

				' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
				' TODO: set large fields to null.
			End If
			Me.disposedValue = True
			MyBase.Dispose(disposing)
		End Sub
#End Region
	End Class
End Namespace
