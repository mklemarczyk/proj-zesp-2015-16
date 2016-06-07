Namespace ViewModels
	Public Class Laboratory
		Implements IDisposable

		Protected Names As HashSet(Of String) = New HashSet(Of String)
		Public Property Devices As ObservableCollection(Of DeviceViewModel)
		Public Property Links As ObservableCollection(Of LinkViewModel)

		Public Sub New()
			Devices = New ObservableCollection(Of DeviceViewModel)
			Links = New ObservableCollection(Of LinkViewModel)
		End Sub

		Public Sub NewComputer()
			Dim device = New ComputerViewModel(Me)
			If Not Devices.Contains(device) Then
				Devices.Add(device)
			End If
		End Sub

		Public Sub NewHub()
			Dim device = New HubViewModel(Me)
			If Not Devices.Contains(device) Then
				Devices.Add(device)
			End If
		End Sub

		Public Sub NewRouter()
			Dim device = New RouterViewModel(Me)
			If Not Devices.Contains(device) Then
				Devices.Add(device)
			End If
		End Sub

		Public Sub NewEthernetLink()
			Links.Add(New EthernetLinkViewModel)
		End Sub

		Public Sub RemoveDevice(Device As DeviceViewModel)
			Dim RelatedLinks = Links.Where(
				Function(x)
					Return x.ItemA Is Device Or x.ItemB Is Device
				End Function).ToArray()
			For Each x In RelatedLinks
				x.Dispose()
				Links.Remove(x)
			Next
			If Device.Name IsNot Nothing AndAlso Names.Contains(Device.Name) Then
				Names.Remove(Device.Name)
			End If
			Devices.Remove(Device)
			Device.Dispose()
		End Sub

		Public Function IsNameExist(value As String) As Boolean
			Return Names.Contains(value)
		End Function

		Public Sub RegisterMyName(visualLabElement As DeviceViewModel, oldName As String)
			If Devices.Contains(visualLabElement) Then
				If oldName IsNot Nothing AndAlso Names.Contains(oldName) Then
					Names.Remove(oldName)
				End If
			Else
				Devices.Add(visualLabElement)
			End If
			If Not Names.Contains(visualLabElement.Name) Then
				Names.Add(visualLabElement.Name)
			End If
		End Sub

#Region "IDisposable Support"
		Private disposedValue As Boolean ' To detect redundant calls

		' IDisposable
		Protected Overridable Sub Dispose(disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					Dim deviceArray = Devices.ToArray()
					For Each device In deviceArray
						RemoveDevice(device)
					Next
					Names = Nothing
					Devices = Nothing
					Links = Nothing
				End If

				' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
				' TODO: set large fields to null.
			End If
			Me.disposedValue = True
		End Sub

		' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
		Protected Overrides Sub Finalize()
#If DEBUG Then
			Debug.WriteLine("Laboratory is shutting down with Sub Finalize.")
#End If
			Dispose(False)
			MyBase.Finalize()
		End Sub

		' This code added by Visual Basic to correctly implement the disposable pattern.
		Public Sub Dispose() Implements IDisposable.Dispose
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region
	End Class
End Namespace
