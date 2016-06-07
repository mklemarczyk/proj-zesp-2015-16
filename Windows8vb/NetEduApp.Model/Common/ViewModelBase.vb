Namespace Common
    Public MustInherit Class ViewModelBase
		Implements INotifyPropertyChanged
		Implements IDisposable

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Protected Sub RaisePropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End Sub

#Region "IDisposable Support"
		Private disposedValue As Boolean ' To detect redundant calls

		' IDisposable
		Protected Overridable Sub Dispose(disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then

				End If

				' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
				' TODO: set large fields to null.
			End If
			Me.disposedValue = True
		End Sub

		Protected Overrides Sub Finalize()
#If DEBUG Then
			Debug.WriteLine("ViewModelBase is shutting down with Sub Finalize.")
#End If
			Dispose(False)
			MyBase.Finalize()
		End Sub

		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region
	End Class
End Namespace
