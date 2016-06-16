Imports NetEduApp.Simulator
Imports NetEduApp.Model.ViewModels
Imports Windows.Data
Imports Windows.Data.Json
Imports Windows.Storage
Imports Windows.UI.Notifications

Namespace Services
	Public Class ExportService
		Public Shared File As Laboratory

		Public Shared Async Function PrepareLabFromFileActivated(fileEventArgs As FileActivatedEventArgs) As Task(Of Boolean)
			If fileEventArgs.Files.Count > 0 Then
				Try
					Dim jsonString = Await FileIO.ReadTextAsync(fileEventArgs.Files(0))
					Dim lab As Laboratory = Nothing
					InterprateJson(jsonString, lab)
					File = lab
					Return True
				Catch ex As Exception
                    NotificationService.ShowToast("Plik uszkodzony")
                End Try
			End If
			Return False
		End Function

		Public Shared Sub GenerateJson(model As Laboratory, ByRef json As String)
			Dim jsonObj As JsonObject = New JsonObject()

			jsonObj.SetNamedValue("Version", JsonValue.CreateStringValue("1.0"))

			Dim deviceArray As JsonArray = New JsonArray()
			For Each device In model.Devices
				Dim jsonDevice As JsonObject = New JsonObject()

				jsonDevice.SetNamedValue("Type", JsonValue.CreateStringValue(device.GetType().Name))
				jsonDevice.SetNamedValue("Name", JsonValue.CreateStringValue(device.Name))
				jsonDevice.SetNamedValue("DefaultGateway", JsonValue.CreateStringValue(device.DefaultGateway.ToString()))
				jsonDevice.SetNamedValue("PositionX", JsonValue.CreateNumberValue(device.Position.X))
				jsonDevice.SetNamedValue("PositionY", JsonValue.CreateNumberValue(device.Position.Y))

				Dim interfaceArray As JsonArray = New JsonArray()
				For Each interf In device.Interfaces
					Dim jsonInterface As JsonObject = New JsonObject()

					jsonInterface.SetNamedValue("Name", JsonValue.CreateStringValue(interf.Name))
					jsonInterface.SetNamedValue("MacAddress", JsonValue.CreateStringValue(interf.MacAddress.GetUlongRepresentation()))

					If interf.IpAddress IsNot Nothing Then
						jsonInterface.SetNamedValue("IpAddress", JsonValue.CreateStringValue(interf.IpAddress.Value.Address.GetUintRepresentation()))
						jsonInterface.SetNamedValue("IpNetmask", JsonValue.CreateStringValue(interf.IpAddress.Value.Netmask.GetUintRepresentation()))
						jsonInterface.SetNamedValue("IpBroadcast", JsonValue.CreateStringValue(interf.IpAddress.Value.Broadcast.GetUintRepresentation()))
					Else
						jsonInterface.SetNamedValue("IpAddress", JsonValue.CreateStringValue(String.Empty))
						jsonInterface.SetNamedValue("IpNetmask", JsonValue.CreateStringValue(String.Empty))
						jsonInterface.SetNamedValue("IpBroadcast", JsonValue.CreateStringValue(String.Empty))
					End If

					interfaceArray.Add(jsonInterface)
				Next
				jsonDevice.SetNamedValue("Interfaces", interfaceArray)

				Dim routeArray As JsonArray = New JsonArray()
				For Each route In device.Routes
					Dim jsonRoute As JsonObject = New JsonObject()

					jsonRoute.SetNamedValue("SubnetAddress", JsonValue.CreateStringValue(route.SubnetAddress.Address.GetUintRepresentation()))
					jsonRoute.SetNamedValue("SubnetNetmask", JsonValue.CreateStringValue(route.SubnetAddress.Netmask.GetUintRepresentation()))
					jsonRoute.SetNamedValue("TargetAddress", JsonValue.CreateStringValue(route.TargetAddress.GetUintRepresentation()))

					routeArray.Add(jsonRoute)
				Next
				jsonDevice.SetNamedValue("Routes", routeArray)

				deviceArray.Add(jsonDevice)
			Next
			jsonObj.SetNamedValue("Devices", deviceArray)

			Dim linksArray As JsonArray = New JsonArray()
			For Each link In model.Links
				Dim jsonLink As JsonObject = New JsonObject()

				jsonLink.SetNamedValue("Type", JsonValue.CreateStringValue(link.GetType().Name))
				jsonLink.SetNamedValue("DeviceNameA", JsonValue.CreateStringValue(link.ItemA.Name))
				jsonLink.SetNamedValue("DeviceNameB", JsonValue.CreateStringValue(link.ItemB.Name))
				jsonLink.SetNamedValue("InterfaceNameA", JsonValue.CreateStringValue(If(link.InterfA, String.Empty)))
				jsonLink.SetNamedValue("InterfaceNameB", JsonValue.CreateStringValue(If(link.InterfB, String.Empty)))

				linksArray.Add(jsonLink)
			Next
			jsonObj.SetNamedValue("Links", linksArray)

			json = jsonObj.Stringify()
		End Sub

		Public Shared Sub InterprateJson(json As String, ByRef model As Laboratory)
			Dim jsonObj As JsonObject = Nothing
			If model IsNot Nothing Then
				model.Dispose()
			End If

			model = New Laboratory()
			jsonObj = JsonObject.Parse(json)

			Dim deviceArray = jsonObj("Devices").GetArray()
			For Each deviceValue In deviceArray
				Dim deviceViewModel As DeviceViewModel = LoadDevice(deviceValue, model)

				If Not model.Devices.Contains(deviceViewModel) Then
					model.Devices.Add(deviceViewModel)
				End If
			Next

			Dim linkArray = jsonObj("Links").GetArray()
			For Each linkValue In linkArray
				model.Links.Add(LoadLink(linkValue, model.Devices))
			Next
		End Sub

		Private Shared Function LoadDevice(value As JsonValue, model As Laboratory) As DeviceViewModel
			Dim deviceObject As JsonObject = value.GetObject()
			Dim deviceTypeName = String.Format("{0}.{1}", GetType(DeviceViewModel).Namespace, deviceObject("Type").GetString())
			Dim deviceType = Type.GetType(deviceTypeName)
			Dim deviceInstance = Activator.CreateInstance(deviceType, model)
			Dim deviceViewModel As DeviceViewModel = deviceInstance

			deviceViewModel.Name = deviceObject("Name").GetString()
			deviceViewModel.Position = New Point(deviceObject("PositionX").GetNumber(), deviceObject("PositionY").GetNumber())
			Dim defaultGateway = deviceObject("DefaultGateway").GetString()

			If defaultGateway IsNot String.Empty Then
				Dim address As NetIpAddress
				NetIpAddress.TryParse(defaultGateway, address)
				deviceViewModel.DefaultGateway = address
			End If

			Dim interfaceArray = deviceObject("Interfaces").GetArray()
			If interfaceArray.Count > 0 Then
				deviceViewModel.Interfaces.Clear()
				For Each interfaceValue In interfaceArray
					deviceViewModel.Interfaces.Add(LoadInterface(interfaceValue))
				Next
			End If

			Dim routeArray = deviceObject("Routes").GetArray()
			For Each routeValue In routeArray
				deviceViewModel.Routes.Add(LoadRoute(routeValue))
			Next

			Return deviceViewModel
		End Function

		Private Shared Function LoadInterface(value As JsonValue) As InterfaceViewModel
			Dim interfaceObject As JsonObject = value.GetObject()
			Dim inerfaceViewModel = New InterfaceViewModel(interfaceObject("Name").GetString())

			Dim mac = interfaceObject("MacAddress").GetString()
			Dim ip = interfaceObject("IpAddress").GetString()
			Dim mask = interfaceObject("IpNetmask").GetString()
			Dim broadcast = interfaceObject("IpBroadcast").GetString()

			inerfaceViewModel.MacAddress = New NetMacAddress(UInt64.Parse(mac))
			If Not String.IsNullOrEmpty(ip) AndAlso Not String.IsNullOrEmpty(mask) AndAlso Not String.IsNullOrEmpty(broadcast) Then
				inerfaceViewModel.IpAddress = New NetAddress(New NetIpAddress(UInt32.Parse(ip)), New NetIpAddress(UInt32.Parse(mask)), New NetIpAddress(UInt32.Parse(broadcast)))
			End If

			Return inerfaceViewModel
		End Function

		Private Shared Function LoadRoute(value As JsonValue) As RouteViewModel
			Dim routeObject As JsonObject = value.GetObject()
			Dim routeViewModel = New RouteViewModel()

			Dim ip = routeObject("SubnetAddress").GetString()
			Dim mask = routeObject("SubnetNetmask").GetString()
			Dim target = routeObject("TargetAddress").GetString()

			routeViewModel.SubnetAddress = New NetAddress(New NetIpAddress(UInt32.Parse(ip)), New NetIpAddress(UInt32.Parse(mask)))
			routeViewModel.TargetAddress = New NetIpAddress(UInt32.Parse(target))

			Return routeViewModel
		End Function

		Private Shared Function LoadLink(value As JsonValue, devices As IEnumerable(Of DeviceViewModel)) As LinkViewModel
			Dim linkObject As JsonObject = value.GetObject()
			Dim linkTypeName = String.Format("{0}.{1}", GetType(LinkViewModel).Namespace, linkObject("Type").GetString())
			Dim linkType = Type.GetType(linkTypeName)
			Dim linkInstance = Activator.CreateInstance(linkType)
			Dim linkViewModel As LinkViewModel = linkInstance

			linkViewModel.ItemA = devices.First(Function(b As DeviceViewModel) linkObject("DeviceNameA").GetString().Equals(b.Name))
			linkViewModel.ItemB = devices.First(Function(b As DeviceViewModel) linkObject("DeviceNameB").GetString().Equals(b.Name))

			linkViewModel.InterfA = linkObject("InterfaceNameA").GetString()
			linkViewModel.InterfB = linkObject("InterfaceNameB").GetString()

			If linkViewModel.ItemA IsNot Nothing AndAlso linkViewModel.ItemA.VisibleInterfaces.Contains(linkViewModel.InterfA) Then
				linkViewModel.ItemA.VisibleInterfaces.Remove(linkViewModel.InterfA)
			End If

			If linkViewModel.ItemB IsNot Nothing AndAlso linkViewModel.ItemB.VisibleInterfaces.Contains(linkViewModel.InterfB) Then
				linkViewModel.ItemB.VisibleInterfaces.Remove(linkViewModel.InterfB)
			End If
			Return linkViewModel
		End Function

	End Class
End Namespace
