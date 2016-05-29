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
				NetAddress? target = null;
				foreach (var ipInterface in interfaces) {
					if (ipInterface.Address != null && ipInterface.Address.Value.GetNetwork( ).Contains(data.DestinationAddress.Value) == true) {
						EmulatorLogger.Log(LogLevel.Info, EventType.RouteFoundConnected, string.Empty);
						ipInterface.SendData(data);
						return;
					}
				}
				if (target == null) {
					foreach (var route in this.Routes) {
						if (route.IsMatch(data.DestinationAddress.Value)) {
							EmulatorLogger.Log(LogLevel.Info, EventType.RouteFound, string.Empty);
							target = route.Target;
						}
					}
				}
				if (target == null && this.DefaultRoute != null) {
					EmulatorLogger.Log(LogLevel.Info, EventType.RouteDefaultUsed, string.Empty);
					target = this.DefaultRoute.Target;
				}
				if (target != null) {
					foreach (var ipInterface in interfaces) {
						if (ipInterface.Address != null && ipInterface.Address.Value.GetNetwork( ).Contains(target.Value) == true) {
							EmulatorLogger.Log(LogLevel.Info, EventType.PacketRouted, string.Empty);
							ipInterface.SendData(data);
							return;
						}
					}
				} else {
					EmulatorLogger.Log(LogLevel.Info, EventType.RouteNotFound, string.Empty);
				}
			}
		}

		public void SendPing(NetIpAddress ipAddress) {
			SendData(new NetPacket(null, null, null, new NetAddress(ipAddress)));
		}
	}
}
