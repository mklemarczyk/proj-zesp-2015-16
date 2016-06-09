using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Abstract;

namespace NetEduApp.Simulator {
	public struct NetAddress {
		private NetIpAddress address;
		private NetIpAddress netmask;
		private NetIpAddress broadcast;

		public NetAddress(NetIpAddress address) {
			this.address = address;
			this.netmask = NetIpAddress.MaxAddress;
			this.broadcast = ComputeBroadcast(this.address, this.netmask);
		}

		public NetAddress(NetIpAddress address, NetIpAddress netmask) {
			this.address = address;
			this.netmask = netmask;
			this.broadcast = ComputeBroadcast(this.address, this.netmask);
		}

		public NetAddress(NetIpAddress address, NetIpAddress netmask, NetIpAddress broadcast) {
			this.address = address;
			this.netmask = netmask;
			this.broadcast = broadcast;
		}

		public NetIpAddress Address { get { return address; } }
		public NetIpAddress Netmask { get { return netmask; } }
		public NetIpAddress Broadcast { get { return broadcast; } }

		public bool IsValid( ) { return NetmaskIsValid(netmask) && broadcast == ComputeBroadcast(this.Address, this.Netmask); }
		public bool IsNetwork( ) { return address == ComputeNetworkAddress(this.Address, this.Netmask); }
		public bool IsHost( ) { return address != ComputeNetworkAddress(this.Address, this.Netmask); }
		public bool Contains(NetAddress hostAddress) {
			return (this.IsNetwork( ) && this.Address == ComputeNetworkAddress(hostAddress.Address, this.Netmask))
				|| this.Address == hostAddress.Address;
		}
		public NetAddress GetNetwork( ) {
			if (this.Netmask != NetIpAddress.MaxAddress) {
				var networkAddress = ComputeNetworkAddress(this.Address, this.Netmask);
				return new NetAddress(networkAddress, this.Netmask, this.Broadcast);
			}
			return this;
		}

		public bool Equals(NetAddress other) {
			return this.address == other.Address
				&& this.netmask == other.Netmask
				&& this.broadcast == other.Broadcast;
		}

		public override bool Equals(object obj) {
			if (obj is NetAddress) {
				return this.Equals((NetAddress)obj);
			}
			return false;
		}

		public override int GetHashCode( ) {
			return (int)this.address.GetUintRepresentation( ) ^ (int)this.netmask.GetUintRepresentation( );
		}

		#region Helpers
		public static bool NetmaskIsValid(NetIpAddress netmask) {
			if (netmask != null) {
				var bitNetmask = netmask.GetUintRepresentation( );
				bool hostPart = false;
				for (int i = 0; i < 32; i++) {
					if (bitNetmask % 2 == 1) {
						hostPart = true;
					} else if (hostPart) {
						return false;
					}
					bitNetmask >>= 1;
				}
			}
			return true;
		}
		public static NetIpAddress ComputeBroadcast(NetIpAddress address, NetIpAddress netmask) {
			var addr = address.Bytes;
			var netm = netmask.Bytes;
			var n = Math.Min(addr.Length, netm.Length);
			var broad = new byte[n];
			for (int i = 0; i < n; i++) {
				broad[i] = addr[i];
				broad[i] &= netm[i];
				byte second = byte.MaxValue;
				second ^= netm[i];
				broad[i] += second;
			}
			return new NetIpAddress(broad);
		}
		public static NetIpAddress ComputeNetworkAddress(NetIpAddress address, NetIpAddress netmask) {
			var addr = address.GetUintRepresentation( );
			var netm = netmask.GetUintRepresentation( );
			var neta = addr & netm;
			return new NetIpAddress(neta);
		}
		#endregion

		public static bool operator ==(NetAddress a, NetAddress b) {
			return a.Equals(b);
		}

		public static bool operator !=(NetAddress a, NetAddress b) {
			return !a.Equals(b);
		}
	}
}
