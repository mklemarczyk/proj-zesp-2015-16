Namespace Models
	Public Class Laboratory
		Protected Names As HashSet(Of String) = New HashSet(Of String)
		Public Property Devices As ObservableCollection(Of VisualLabElement)
		Public Property Links As ObservableCollection(Of VisualLabLink)

		Public Sub New()
			Devices = New ObservableCollection(Of VisualLabElement)
			Links = New ObservableCollection(Of VisualLabLink)

			Dim r1 = New Router(Me) With {
				.Position = New Point(231, 284)
			}
			Dim r2 = New Router(Me) With {
				.Position = New Point(12, 271)
			}
			Dim l1 = New EthernetLink With {
				.ItemA = r1,
				.ItemB = r2
			}

			Devices.Add(r1)
			Devices.Add(r2)
			Links.Add(l1)
		End Sub

		Public Sub NewHub()
			Devices.Add(New Hub(Me))
		End Sub

		Public Sub NewSwitch()
			Devices.Add(New Switch(Me))
		End Sub

		Public Sub NewRouter()
			Devices.Add(New Router(Me))
		End Sub

		Public Sub NewComputer()
			Devices.Add(New Computer(Me))
		End Sub

		Public Sub NewEthernetLink()
			Links.Add(New EthernetLink)
		End Sub

		Public Sub RemoveDevice(Device As VisualLabElement)
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

		Public Sub RegisterMyName(visualLabElement As VisualLabElement, oldName As String)
			If Devices.Contains(visualLabElement) Then
				If oldName IsNot Nothing AndAlso Names.Contains(oldName) Then
					Names.Remove(oldName)
				End If
				Names.Add(visualLabElement.Name)
			End If
		End Sub
	End Class
End Namespace

