﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Logger;
using NetEduApp.Simulator.Abstract;

namespace NetEduApp.Simulator.Modules {
	internal class RouteTableModule {

		public static INetLgInterface GetTargetInterface(NetAddress? destinationAddress, IEnumerable<INetLgInterface> ipInterfaces) {
			if (destinationAddress != null) {
				foreach (var ipInterface in ipInterfaces) {
					if (ipInterface.Address != null && ipInterface.Address.Value.GetNetwork( ).Contains(destinationAddress.Value) == true) {
						EmulatorLogger.Log(LogLevel.Info, EventType.RouteFoundConnected, string.Empty);
						return ipInterface;
					}
				}
			}
			return null;
		}

		public static NetAddress? GetRoute(NetAddress? destinationAddress, IEnumerable<INetLgInterface> ipInterfaces, IEnumerable<INetRoute> routes, INetRoute defaultRoute) {
			if (destinationAddress != null) {
				foreach (var route in routes) {
					if (route.IsMatch(destinationAddress.Value)) {
						EmulatorLogger.Log(LogLevel.Info, EventType.RouteFound, string.Empty);
						return route.Target;
					}
				}
				if (defaultRoute != null) {
					EmulatorLogger.Log(LogLevel.Info, EventType.RouteDefaultUsed, string.Empty);
					return defaultRoute.Target;
				}
				EmulatorLogger.Log(LogLevel.Info, EventType.RouteNotFound, string.Empty);
			}else {
				EmulatorLogger.Log(LogLevel.Warning, EventType.RouteInvalidDestinationAddress, string.Empty);
			}
			return null;
		}

	}
}
