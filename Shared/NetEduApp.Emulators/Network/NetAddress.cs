using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
    public struct NetAddress : INetAddress {
        private INetIpAddress address;
        private INetIpAddress netmask;
        private INetIpAddress broadcast;

        public NetAddress(INetIpAddress address) {
            this.address = address;
            this.netmask = NetIpAddress.MaxAddress;
            this.broadcast = ComputeBroadcast(this.address, this.netmask);
        }

        public NetAddress(INetIpAddress address, INetIpAddress netmask) {
            this.address = address;
            this.netmask = netmask;
            this.broadcast = ComputeBroadcast(this.address, this.netmask);
        }

        public NetAddress(INetIpAddress address, INetIpAddress netmask, INetIpAddress broadcast) {
            this.address = address;
            this.netmask = netmask;
            this.broadcast = broadcast;
        }

        public INetIpAddress Address { get { return address; } }
        public INetIpAddress Netmask { get { return netmask; } }
        public INetIpAddress Broadcast { get { return broadcast; } }

        public bool IsValid( ) { return NetmaskIsValid(netmask) && broadcast == ComputeBroadcast(this.Address, this.Netmask); }
        public bool IsNetwork( ) { return address == ComputeNetworkAddress(this.Address, this.Netmask); }
        public bool IsHost( ) { return address != ComputeNetworkAddress(this.Address, this.Netmask); }
        public bool Contains(INetAddress hostAddress) {
            return (this.IsNetwork( ) && this.Address == ComputeNetworkAddress(hostAddress.Address, this.Netmask))
                || this.Address == hostAddress.Address;
        }
        public INetAddress GetNetwork( ) {
            if (this.Netmask != NetIpAddress.MaxAddress) {
                var networkAddress = ComputeNetworkAddress(this.Address, this.Netmask);
                return new NetAddress(networkAddress, this.Netmask, this.Broadcast);
            }
            return null;
        }

        public bool Equals(INetAddress other) {
            /*return this.address == other.Address
				&& this.netmask == other.Netmask
				&& this.broadcast == other.Broadcast;*/
            if (this.address == other.Address)
                if (this.netmask == other.Netmask)
                    if (this.broadcast == other.Broadcast)
                        return true;
                    else
                        return false;
                else
                    return false;
            else
                return false;
        }

        #region Helpers
        public static bool NetmaskIsValid(INetIpAddress netmask) {
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
        public static INetIpAddress ComputeBroadcast(INetIpAddress address, INetIpAddress netmask) {
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
        public static INetIpAddress ComputeNetworkAddress(INetIpAddress address, INetIpAddress netmask) {
            var addr = address.GetUintRepresentation( );
            var netm = netmask.GetUintRepresentation( );
            var neta = addr & netm;
            return new NetIpAddress(neta);
        }
        #endregion
    }
}
