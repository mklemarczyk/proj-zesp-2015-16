﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Logger;
using NetEduApp.Simulator.Abstract;

namespace NetEduApp.Simulator {
	internal class NetHwInterface : INetHwInterface {
		private static byte n = 0;
		protected INetDevice parent;
		protected INetHwInterface otherInterface;
		protected string name;
		protected NetMacAddress hardwareAddress;

		internal NetHwInterface(INetDevice parent, string name) {
			unchecked { n++; }
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (name == null)
				throw new ArgumentNullException("name");
			if (name == string.Empty)
				throw new ArgumentException("name can not be empty");
			this.parent = parent;
			this.name = name;
			this.hardwareAddress = new NetMacAddress(((ulong)DateTime.Now.Ticks << 8) + n);
		}

		public INetDevice Parent { get { return this.parent; } }
		public string Name { get { return this.name; } }
		public NetMacAddress HardwareAddress { get { return this.hardwareAddress; } }

		public virtual void Connect(INetHwInterface other) {
			if (other == null)
				throw new ArgumentNullException("other");
			if (ReferenceEquals(this, other))
				throw new ArgumentException("Can not connect the same interface instances");
			if (ReferenceEquals(this.otherInterface, other))
				throw new ArgumentException("That connection was made already");
			if (other is NetHwInterface) {
				this.Connect((NetHwInterface)other);
			} else {
				throw new ArgumentException("Invalid other type, it require [NetHwInterface] object");
			}
		}

		public virtual void Disconnect( ) {
			var other = otherInterface;
			if (other != null) {
				otherInterface = null;
				EmulatorLogger.Log(LogLevel.Info, EventType.Disconnected, this.Name);
				other.Disconnect( );
			}
		}

		private void Connect(NetHwInterface other) {
			if (otherInterface != null) {
				otherInterface.Disconnect( );
			}
			if (other != null) {
				other.Disconnect( );
				other.otherInterface = this;
				EmulatorLogger.Log(LogLevel.Info, EventType.Connected, other.Name);
			}
			otherInterface = other;
			EmulatorLogger.Log(LogLevel.Info, EventType.Connected, this.Name);
		}

		public virtual void ReceiveData(INetPacket data) {
			EmulatorLogger.Log(LogLevel.Info, EventType.PacketRecived, this.Name);
			if (data == null)
				throw new ArgumentNullException("data");
			if (data.DestinationHardwareAddress == this.HardwareAddress)
				parent.ReceiveData(data, this);
		}

		public virtual void SendData(INetPacket data) {
			if (otherInterface != null) {
				EmulatorLogger.Log(LogLevel.Info, EventType.PacketSend, this.Name);
				otherInterface.ReceiveData(new NetPacket(this.HardwareAddress, otherInterface.HardwareAddress, data.SourceAddress, data.DestinationAddress));
			} else {
				EmulatorLogger.Log(LogLevel.Info, EventType.NotConnected, this.Name);
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing) {
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

		~NetHwInterface( ) {
			Dispose(false);
		}

		public void Dispose( ) {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
