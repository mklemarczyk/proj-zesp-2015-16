﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Logger;
using NetEduApp.Simulator.Abstract;

namespace NetEduApp.Simulator {
	internal class NetLgInterface : NetHwInterface, INetLgInterface {

		internal NetLgInterface(INetDevice parent, string name) : base(parent, name) { }

		public NetAddress? Address { get; set; }

		public override void ReceiveData(INetPacket data) {
			base.ReceiveData(data);
		}

		public override void SendData(INetPacket data) {
			if (otherInterface != null) {
				EmulatorLogger.Log(LogLevel.Info, EventType.PacketSend, this.Name);
				otherInterface.ReceiveData(new NetPacket(this.HardwareAddress, otherInterface.HardwareAddress, data.SourceAddress ?? this.Address, data.DestinationAddress));
			} else {
				EmulatorLogger.Log(LogLevel.Error, EventType.NotConnected, "There are no connected devie: " + this.Name);
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected override void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					this.parent = null;
					this.otherInterface = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}
		#endregion
	}
}
