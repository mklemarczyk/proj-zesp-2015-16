using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
    public struct NetIpAddress {
        private readonly byte[] bytes;
        public byte[] Bytes => bytes;

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
            if (bytes == null) {
                return 0;
            } else {
                uint bitNetmask = bytes[0];
                bitNetmask <<= 8;
                bitNetmask |= bytes[1];
                bitNetmask <<= 8;
                bitNetmask |= bytes[2];
                bitNetmask <<= 8;
                bitNetmask |= bytes[3];
                return bitNetmask;
            }
        }

        public bool Equals(NetIpAddress other) {
            var thisBytes = Bytes;
            var otherBytes = other.Bytes;
            if (thisBytes == null) thisBytes = Zero.Bytes;
            if (otherBytes == null) otherBytes = Zero.Bytes;
            return thisBytes[0] == otherBytes[0]
                && thisBytes[1] == otherBytes[1]
                && thisBytes[2] == otherBytes[2]
                && thisBytes[3] == otherBytes[3];
        }

        public override bool Equals(object obj) {
            if (obj is NetIpAddress) {
                return this.Equals((NetIpAddress)obj);
            }
            return false;
        }

        public override int GetHashCode( ) {
            return (int)this.GetUintRepresentation( );
        }

        public override string ToString( ) {
            if (bytes == null)
                return Zero.ToString();
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
