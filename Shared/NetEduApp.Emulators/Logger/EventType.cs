using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Logger {
	public enum EventType {
		#region Connection
		Connected,
		Disconnected,
		NotConnected,
		#endregion

		#region Routing
		RouteFoundConnected,
		RouteFound,
		RouteDefaultUsed,
		RouteNotFound,
		#endregion

		#region Packet transmission
		PacketSend,
		PacketRecived,
		PacketRouted,
		HubPacketColision,
		RouteInvalidDestinationAddress,
		#endregion
	}
}
