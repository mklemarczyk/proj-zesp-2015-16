using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Packets {
	internal class ArpDiscoveryPacket : NetPacket {
		/// <summary>
		/// Address Resolution Protocol
		/// </summary>
		public ArpDiscoveryPacket(NetMacAddress sourceHardwareAddress, NetMacAddress destinationHardwareAddress, NetAddress sourceAddress, NetAddress destinationAddress)
			: base(sourceHardwareAddress, destinationHardwareAddress, sourceAddress, destinationAddress) {
		}
	}
}
