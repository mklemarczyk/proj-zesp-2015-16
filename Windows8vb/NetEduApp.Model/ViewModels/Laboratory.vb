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
            Devices.Add(New AccessPointViewModel(Me))
        End Sub

        Public Sub NewBridge()
            Devices.Add(New BridgeViewModel(Me))
        End Sub

        Public Sub NewComputer()
            Devices.Add(New ComputerViewModel(Me))
        End Sub

        Public Sub NewHub()
            Devices.Add(New HubViewModel(Me))
        End Sub

        Public Sub NewNotebook()
            Devices.Add(New NotebookViewModel(Me))
        End Sub

        Public Sub NewRepeater()
            Devices.Add(New RepeaterViewModel(Me))
        End Sub

        Public Sub NewRouter()
            Devices.Add(New RouterViewModel(Me))
        End Sub

        Public Sub NewSwitch()
            Devices.Add(New SwitchViewModel(Me))
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
