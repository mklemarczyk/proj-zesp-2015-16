using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Logger;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Modules {
	internal class InterfaceModule {

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

	}
}
