using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Logger;
using NetEduApp.Simulator.Abstract;
using NetEduApp.Simulator.Modules;

namespace NetEduApp.Simulator.Devices {
	internal class Router : IRouter {
		private List<INetLgInterface> interfaces;
		private List<INetRoute> routes;

		internal Router(string name) {
			this.Name = name;
			this.interfaces = new List<INetLgInterface>( );
			this.routes = new List<INetRoute>( );
			this.interfaces.Add(new NetLgInterface(this, name + "/eth0"));
			this.interfaces.Add(new NetLgInterface(this, name + "/eth1"));
			this.interfaces.Add(new NetLgInterface(this, name + "/eth2"));
			this.interfaces.Add(new NetLgInterface(this, name + "/eth3"));
		}

		IReadOnlyList<INetHwInterface> INetDevice.Interfaces { get { return interfaces; } }
		public IReadOnlyList<INetLgInterface> Interfaces { get { return interfaces; } }
		public int PortCount { get { return interfaces.Count; } }
		public IList<INetRoute> Routes { get { return routes; } }
		public string Type { get { return "Computer"; } }

		public INetRoute DefaultRoute { get; set; }

		public string Name { get; set; }

		public void ReceiveData(INetPacket data, INetHwInterface iface) {
			int ifaceNo = this.interfaces.FindIndex(x => x.Equals(iface));
			foreach (var ipInterface in interfaces) {
				if (ipInterface.Address != null && ipInterface.Address.Value.Address == data.DestinationAddress?.Address) {
					return;
				}
			}
			if (data.TTL > 0) {
				data.TTL--;
				this.SendData(data);
			}
		}

		public void SendData(INetPacket data) {
			if (data.DestinationAddress != null) {
				var targetIf = RouteTableModule.GetTargetInterface(data.DestinationAddress, interfaces);
				if (targetIf != null) {
					targetIf.SendData(data);
					return;
				} else {
					var routeTarget = RouteTableModule.GetRoute(data.DestinationAddress, interfaces, routes, DefaultRoute);
					if (routeTarget != null) {
						targetIf = RouteTableModule.GetTargetInterface(routeTarget, interfaces);
						if (targetIf != null) {
							targetIf.SendData(data);
							return;
						}
					}
				}
				EmulatorLogger.Log(LogLevel.Info, EventType.RouteNotFound, string.Empty);
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					foreach (var iface in interfaces) {
						iface.Dispose( );
					}
					interfaces.Clear( );
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		~Router( ) {
			Dispose(false);
		}

		public void Dispose( ) {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
