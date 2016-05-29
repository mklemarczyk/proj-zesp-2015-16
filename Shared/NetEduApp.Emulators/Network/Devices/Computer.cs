using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Logger;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Devices {
	internal class Computer : IComputer {
		private List<INetLgInterface> interfaces;
		private List<INetRoute> routes;

		internal Computer(string name) {
			this.Name = name;
			this.interfaces = new List<INetLgInterface>( );
			this.routes = new List<INetRoute>( );
			this.interfaces.Add(new NetLgInterface(this, name));
		}

		public IReadOnlyList<INetLgInterface> Interfaces { get { return interfaces; } }
		public int PortCount { get { return interfaces.Count; } }
		public IList<INetRoute> Routes { get { return routes; } }
		public string Type { get { return "Computer"; } }

		public INetRoute DefaultRoute { get; set; }

		public string Name { get; set; }

		public void ReceiveData(INetPacket data, INetHwInterface iface) {
#if DEBUG
			System.Diagnostics.Debug.WriteLine("{0} recived {1}", this.Name, data);
#endif
		}

		public void SendData(INetPacket data) {
			if (data.DestinationAddress != null) {
				var targetIf = Modules.InterfaceModule.GetTargetInterface(data.DestinationAddress, interfaces);
				if (targetIf != null) {
					targetIf.SendData(data);
					return;
				} else {
					var routeTarget = Modules.RouteTableModule.GetRoute(data.DestinationAddress, interfaces, routes, DefaultRoute);
					if (routeTarget != null) {
						targetIf = Modules.InterfaceModule.GetTargetInterface(routeTarget, interfaces);
						if (targetIf != null) {
							targetIf.SendData(data);
							return;
						}
					}
				}
				EmulatorLogger.Log(LogLevel.Info, EventType.RouteNotFound, string.Empty);
			}
		}

		public void SendPing(NetIpAddress ipAddress) {
			SendData(new NetPacket(null, null, null, new NetAddress(ipAddress)));
		}
	}
}
