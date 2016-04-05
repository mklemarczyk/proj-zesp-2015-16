using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
	public struct NetAddress : INetAddress {
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
		public bool IsNetwork( ) { return address == ComputeNetworkAddress(this.Address, this.Address); }
		public bool IsHost( ) { return address != ComputeNetworkAddress(this.Address, this.Address); }
		public bool Contains(INetAddress hostAddress) {
			return this.Address == ComputeNetworkAddress(hostAddress.Address, this.Netmask);
		}
		public INetAddress GetNetwork( ) {
			if (this.Netmask != NetIpAddress.MaxAddress) {
				var networkAddress = ComputeNetworkAddress(this.Address, this.Netmask);
				return new NetAddress(networkAddress, this.Netmask, this.Broadcast);
			}
			return null;
		}

		public bool Equals(INetAddress other) {
			return this.address == other.Address
				&& this.netmask == other.Netmask
				&& this.broadcast == other.Broadcast;
		}

		#region Helpers
		public static bool NetmaskIsValid(NetIpAddress? netmask) {
			if (netmask.HasValue) {
				var bitNetmask = netmask.Value.GetUintRepresentation( );
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
			var addr = address.bytes;
			var netm = netmask.bytes;
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
	}

	public struct NetIpAddress {
		public readonly byte[] bytes;

		public static NetIpAddress MaxAddress { get { return new NetIpAddress(255, 255, 255, 255); } }

		public static NetIpAddress Zero { get { return new NetIpAddress(0, 0, 0, 0); } }

		public NetIpAddress(params byte[] bytes)
			: this( ) {
			if (bytes == null)
				throw new ArgumentNullException("bytes");
			else if (bytes.Length != 4)
				throw new ArgumentOutOfRangeException("bytes");
			else
				this.bytes = bytes;
		}

		public NetIpAddress(uint bitAddress)
			: this( ) {
			bytes = new byte[4];
			bytes[3] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[2] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[1] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[0] = (byte)bitAddress;
		}

		public uint GetUintRepresentation( ) {
			uint bitNetmask = bytes[0];
			bitNetmask <<= 8;
			bitNetmask |= bytes[1];
			bitNetmask <<= 8;
			bitNetmask |= bytes[2];
			bitNetmask <<= 8;
			bitNetmask |= bytes[3];
			return bitNetmask;
		}

		public override bool Equals(object obj) {
			if (obj is NetIpAddress) {
				var other = (NetIpAddress)obj;
				return bytes[0] == other.bytes[0] && bytes[1] == other.bytes[1] && bytes[2] == other.bytes[2] && bytes[3] == other.bytes[3];
			}
			return false;
		}

		public override int GetHashCode( ) {
			return (int)this.GetUintRepresentation( );
		}

		public override string ToString( ) {
			return string.Format("{0}.{1}.{2}.{3}", bytes[0], bytes[1], bytes[2], bytes[3]);
		}

		public static bool TryParse(string address, out NetIpAddress validAddress) {
			validAddress = default(NetIpAddress);
			if (address == null)
				return false;
			if (address == string.Empty)
				return false;
			var strBytes = address.Split('.');
			byte[] numBytes = new byte[4];
			if (strBytes.Length != 4)
				return false;
			for (int i = 0; i < 4; i++) {
				byte num;
				if (!byte.TryParse(strBytes[i], out num))
					return false;
				if (num < 0)
					return false;
				if (num > 255)
					return false;
				numBytes[i] = num;
			}
			validAddress = new NetIpAddress(numBytes);
			return true;
		}

		public static bool operator ==(NetIpAddress a, NetIpAddress b) {
			return a.Equals(b);
		}

		public static bool operator !=(NetIpAddress a, NetIpAddress b) {
			return !a.Equals(b);
		}
	}
}
