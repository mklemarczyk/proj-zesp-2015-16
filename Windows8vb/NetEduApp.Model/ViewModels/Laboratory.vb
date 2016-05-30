Namespace ViewModels
	Public Class Laboratory
		Protected Names As HashSet(Of String) = New HashSet(Of String)
		Public Property Devices As ObservableCollection(Of DeviceViewModel)
		Public Property Links As ObservableCollection(Of LinkViewModel)

		Public Sub New()
			Devices = New ObservableCollection(Of DeviceViewModel)
			Links = New ObservableCollection(Of LinkViewModel)

			Dim r1 = New RouterViewModel(Me) With {
				.Position = New Point(231, 284)
			}
			Dim r2 = New RouterViewModel(Me) With {
				.Position = New Point(12, 271)
			}
			Dim l1 = New EthernetLinkViewModel With {
				.ItemA = r1,
				.ItemB = r2
			}

			Links.Add(l1)
		End Sub

		Public Sub NewAccessPoint()
			Dim device = New AccessPointViewModel(Me)
			If Not Devices.Contains(device) Then
				Devices.Add(device)
			End If
		End Sub

		Public Sub NewBridge()
			Dim device = New BridgeViewModel(Me)
			If Not Devices.Contains(device) Then
				Devices.Add(device)
			End If
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

		Public Sub NewNotebook()
			Dim device = New NotebookViewModel(Me)
			If Not Devices.Contains(device) Then
				Devices.Add(device)
			End If
		End Sub

		Public Sub NewRepeater()
			Dim device = New RepeaterViewModel(Me)
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

		Public Sub NewSwitch()
			Dim device = New SwitchViewModel(Me)
			If Not Devices.Contains(device) Then
				Devices.Add(device)
			End If
		End Sub

		Public Sub NewEthernetLink()
			Links.Add(New EthernetLinkViewModel)
		End Sub

		Public Sub NewEthernetCrossoverLink()
			Links.Add(New EthernetCrossoverLinkViewModel)
		End Sub

		Public Sub NewCoaxialLink()
			Links.Add(New CoaxialLinkViewModel)
		End Sub

		Public Sub NewOpticalFiberLink()
			Links.Add(New OpticalFiberLinkViewModel)
		End Sub

		Public Sub NewSerialLink()
			Links.Add(New SerialLinkViewModel)
		End Sub

		Public Sub RemoveDevice(Device As DeviceViewModel)
			Dim RelatedLinks = Links.Where(
				Function(x)
					Return x.ItemA Is Device Or x.ItemB Is Device
				End Function).ToArray()
			For Each x In RelatedLinks
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
	End Class
End Namespace
