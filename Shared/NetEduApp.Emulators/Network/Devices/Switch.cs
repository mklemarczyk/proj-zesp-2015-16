using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Logger;
using NetEduApp.Emulators.Network.Abstract;
using NetEduApp.Emulators.Network.Modules;

namespace NetEduApp.Emulators.Network.Devices {
	internal class Switch : ISwitch {
		private List<INetHwInterface> interfaces;
		private LldpTableModule macTable;
		private Queue<KeyValuePair<int, INetPacket>> packetsQueue;

		internal Switch(string name) {
			this.Name = name;

			this.interfaces = new List<INetHwInterface>( );
			this.interfaces.Add(new NetHwInterface(this, name + "/eth0"));
			this.interfaces.Add(new NetHwInterface(this, name + "/eth1"));
			this.interfaces.Add(new NetHwInterface(this, name + "/eth2"));
			this.interfaces.Add(new NetHwInterface(this, name + "/eth3"));

			this.macTable = new LldpTableModule( );
			this.packetsQueue = new Queue<KeyValuePair<int, INetPacket>>( );
		}

		public IReadOnlyList<INetHwInterface> Interfaces { get { return interfaces; } }
		public int PortCount { get { return interfaces.Count; } }
		public string Type { get { return "Switch"; } }

		public string Name { get; set; }

		public void ReceiveData(INetPacket data, INetHwInterface iface) {
			int ifaceNo = this.interfaces.FindIndex(x => ReferenceEquals(x, iface));
			foreach (var ipInterface in interfaces) {
				if (ipInterface.HardwareAddress != default(NetMacAddress) && ipInterface.HardwareAddress == data.DestinationHardwareAddress) {
					if (data is Packets.LldpResponsePacket) {
						if (macTable.Register(ifaceNo, data.SourceHardwareAddress)) {
							EmulatorLogger.Log(LogLevel.Info, EventType.LldpEntryAdded, this.Name);
						} else {
							EmulatorLogger.Log(LogLevel.Info, EventType.LldpEntryExists, this.Name);
						}
						this.SendData(null);
					}
					return;
				}
			}
			if (data.TTL > 0) {
				data.TTL--;
				this.packetsQueue.Enqueue(new KeyValuePair<int, INetPacket>(ifaceNo, data));
				this.SendData(data);
			}
		}

		public void SendData(INetPacket data) {
			if (data == null) {
				if (packetsQueue.Count > 0) {
					var queued = packetsQueue.Dequeue( );
					data = queued.Value;
					var targetPort = macTable.GetPort(data.DestinationHardwareAddress);
					if (targetPort != null) {
						interfaces[targetPort.Value].SendData(data);
					} else {
						data.TTL--;
						packetsQueue.Enqueue(queued);
						#region Handle missing Mac
						SendDiscovery(queued.Key);
						#endregion
					}
				}
			} else {
				var targetPort = macTable.GetPort(data.DestinationHardwareAddress);
				if (targetPort != null) {
					interfaces[targetPort.Value].SendData(data);
				} else {
					var queued = new KeyValuePair<int, INetPacket>(-1, data);
					packetsQueue.Enqueue(queued);
					#region Handle missing Mac
					SendDiscovery(queued.Key);
					#endregion
				}
			}
		}

		private void SendDiscovery(int key) {
			foreach (var iface in interfaces) {
				iface.SendData(new Packets.LldpDiscoveryPacket(iface.HardwareAddress, NetMacAddress.MaxAddress));
			}
		}
	}
}
