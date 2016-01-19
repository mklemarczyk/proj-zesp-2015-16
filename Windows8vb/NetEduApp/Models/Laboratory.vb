Namespace Models
    Public Class Laboratory
        Public Property Devices As ObservableCollection(Of VisualLabElement)
        Public Property Links As ObservableCollection(Of VisualLabLink)

        Public Sub New()
            Devices = New ObservableCollection(Of VisualLabElement)
            Links = New ObservableCollection(Of VisualLabLink)

            Dim r1 = New Router With {
                .Position = New Point(231, 284)
            }
            Dim r2 = New Router With {
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
            Devices.Add(New Hub)
        End Sub

        Public Sub NewSwitch()
            Devices.Add(New Switch)
        End Sub

        Public Sub NewRouter()
            Devices.Add(New Router)
        End Sub

        Public Sub NewComputer()
            Devices.Add(New Computer)
        End Sub

        Public Sub NewEthernetLink()
            Links.Add(New EthernetLink)
        End Sub

        Friend Sub RemoveDevice(Device As VisualLabElement)
            Dim RelatedLinks = Links.Where(
                Function(x)
                    Return x.ItemA Is Device Or x.ItemB Is Device
                End Function).ToArray()
            For Each x In RelatedLinks
                Links.Remove(x)
            Next
            Devices.Remove(Device)
            Device.Dispose()
        End Sub
    End Class
End Namespace

