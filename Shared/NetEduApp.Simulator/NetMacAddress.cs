using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Abstract;

namespace NetEduApp.Simulator {
	public struct NetMacAddress {
		private readonly byte[] bytes;
		public byte[] Bytes => bytes;

		public static NetMacAddress MaxAddress { get { return new NetMacAddress(255, 255, 255, 255, 255, 255, 255, 255); } }

		public static NetMacAddress Zero { get { return new NetMacAddress(0, 0, 0, 0, 0, 0, 0, 0); } }

		public NetMacAddress(params byte[] bytes)
			: this( ) {
			if (bytes == null)
				throw new ArgumentNullException("bytes");
			else if (bytes.Length != 8)
				throw new ArgumentOutOfRangeException("bytes");
			else
				this.bytes = bytes;
		}

		public NetMacAddress(ulong bitAddress)
			: this( ) {
			bytes = new byte[8];
			bytes[7] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[6] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[5] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[4] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[3] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[2] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[1] = (byte)bitAddress;
			bitAddress >>= 8;
			bytes[0] = (byte)bitAddress;
		}

		public ulong GetUlongRepresentation( ) {
			if (bytes == null) {
				return 0;
			} else {
				ulong bitNetmask = bytes[0];
				bitNetmask <<= 8;
				bitNetmask |= bytes[1];
				bitNetmask <<= 8;
				bitNetmask |= bytes[2];
				bitNetmask <<= 8;
				bitNetmask |= bytes[3];
				bitNetmask <<= 8;
				bitNetmask |= bytes[4];
				bitNetmask <<= 8;
				bitNetmask |= bytes[5];
				bitNetmask <<= 8;
				bitNetmask |= bytes[6];
				bitNetmask <<= 8;
				bitNetmask |= bytes[7];
				return bitNetmask;
			}
		}

		public bool Equals(NetMacAddress other) {
			var thisBytes = Bytes;
			var otherBytes = other.Bytes;
			if (thisBytes == null) thisBytes = Zero.Bytes;
			if (otherBytes == null) otherBytes = Zero.Bytes;
			return thisBytes.Length == otherBytes.Length
				&& thisBytes.SequenceEqual(otherBytes);
		}

		public override bool Equals(object obj) {
			if (obj is NetMacAddress) {
				return this.Equals((NetMacAddress)obj);
			}
			return false;
		}

		public override int GetHashCode( ) {
			return (int)this.GetUlongRepresentation( );
		}

		public override string ToString( ) {
			if (bytes == null)
				return Zero.ToString( );
			return string.Join("-", bytes.Select(x => string.Format("{0:X2}", x)));
		}

		public static bool operator ==(NetMacAddress a, NetMacAddress b) {
			return a.Equals(b);
		}

		public static bool operator !=(NetMacAddress a, NetMacAddress b) {
			return !a.Equals(b);
		}
	}
}
