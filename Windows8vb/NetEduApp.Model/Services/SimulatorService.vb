Imports NetEduApp.Model.ViewModels

Namespace Services
	Public Class SimulatorService

		Public Shared Function Test(model As Laboratory) As Boolean
			Simulator.NetworkFactory.Clear()
			Dim devices = New List(Of Simulator.Abstract.INetDevice)()

			For Each dev In model.Devices
				If TypeOf dev Is ComputerViewModel Then
					Dim device = Simulator.NetworkFactory.CreateComputer()
					device.Name = dev.Name
					devices.Add(device)
				ElseIf TypeOf dev Is RouterViewModel Then
					Dim device = Simulator.NetworkFactory.CreateRouter()
					device.Name = dev.Name
					devices.Add(device)
				ElseIf TypeOf dev Is HubViewModel Then
					Dim device = Simulator.NetworkFactory.CreateHub()
					device.Name = dev.Name
					devices.Add(device)
				End If
			Next

			For Each link In model.Links
				If TypeOf link Is EthernetLinkViewModel Then
					Dim devA = devices.First(Function(x As Simulator.Abstract.INetDevice) As Boolean
												 Return x.Name.Equals(link.ItemA.Name)
											 End Function)
					Dim devB = devices.First(Function(x As Simulator.Abstract.INetDevice) As Boolean
												 Return x.Name.Equals(link.ItemB.Name)
											 End Function)
					Dim ifA = devA.Interfaces.First(Function(x As Simulator.Abstract.INetHwInterface) As Boolean
														Return x.Name.EndsWith(link.InterfA)
													End Function)
					Dim ifB = devB.Interfaces.First(Function(x As Simulator.Abstract.INetHwInterface) As Boolean
														Return x.Name.EndsWith(link.InterfB)
													End Function)

					Simulator.NetworkFactory.MakeLink(ifA, ifB)
				End If
			Next



			Return False
		End Function

	End Class
End Namespace
