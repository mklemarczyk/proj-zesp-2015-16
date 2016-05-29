using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Logger;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Modules {
	internal class RouteTableModule {

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
