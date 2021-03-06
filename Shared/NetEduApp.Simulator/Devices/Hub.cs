﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Logger;
using NetEduApp.Simulator.Abstract;

namespace NetEduApp.Simulator.Devices {
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

		~Hub( ) {
			Dispose(false);
		}

		public void Dispose( ) {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
