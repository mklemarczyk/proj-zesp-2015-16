using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Logger;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Devices {
	internal class Hub : IHub {
		private List<INetHwInterface> interfaces;
		private int? busyInterface;

		internal Hub(string name) {
			this.Name = name;
			this.interfaces = new List<INetHwInterface>( );
			this.interfaces.Add(new NetHwInterface(this, name + "/eth0"));
			this.interfaces.Add(new NetHwInterface(this, name + "/eth1"));
			this.interfaces.Add(new NetHwInterface(this, name + "/eth2"));
			this.interfaces.Add(new NetHwInterface(this, name + "/eth3"));
		}

		public IReadOnlyList<INetHwInterface> Interfaces { get { return interfaces; } }
		public int PortCount { get { return interfaces.Count; } }
		public string Type { get { return "Hub"; } }

		public string Name { get; set; }

		public void ReceiveData(INetPacket data, INetHwInterface iface) {
			if (busyInterface != null) {
				EmulatorLogger.Log(LogLevel.Error, EventType.HubPacketColision, this.Name);
				return;
			} else {
				int ifaceNo = this.interfaces.FindIndex(x => ReferenceEquals(x, iface));
				if (ifaceNo >= 0) {
					busyInterface = ifaceNo;
					if (data.TTL > 0) {
						data.TTL--;
						this.SendData(data);
					}
					busyInterface = null;
				}
			}
		}

		public void SendData(INetPacket data) {
			if (busyInterface != null) {
				for (int i = 0; i < this.interfaces.Count; i++) {
					if (busyInterface != i) {
						var ipInterface = this.interfaces[i];
						Task.Run(( ) => {
							var transmitData = data.Clone( );
							ipInterface.SendData(transmitData);
						});
					}
				}
			}
		}
	}
}
