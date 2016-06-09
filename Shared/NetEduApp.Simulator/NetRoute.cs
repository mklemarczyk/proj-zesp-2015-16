using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Abstract;

namespace NetEduApp.Simulator {
	public class NetRoute : INetRoute {
		private NetAddress address;
		private NetAddress target;

		public NetRoute(NetIpAddress target) {
			this.address = new NetAddress(NetIpAddress.Zero, NetIpAddress.Zero);
			this.target = new NetAddress(target);
		}

		public NetRoute(NetIpAddress address, NetIpAddress netmask, NetIpAddress target) {
			this.address = new NetAddress(address, netmask);
			this.target = new NetAddress(target);
		}

		public bool IsDefault {
			get {
				return NetIpAddress.Zero.Equals(this.address.Address)
					&& NetIpAddress.Zero.Equals(this.address.Netmask);
			}
		}
		public NetAddress Address { get { return this.address; } }
		public NetAddress Target { get { return this.target; } }

		public bool IsValid( ) {
			return this.address.IsValid( )
				&& this.address.IsNetwork( )
				&& this.target.IsValid( );
		}

		public bool IsMatch(NetAddress destinationAddress) {
			if (Address.IsNetwork( ))
				return Address.Contains(destinationAddress);
			return false;
		}
	}
}
