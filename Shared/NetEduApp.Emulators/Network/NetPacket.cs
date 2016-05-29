using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
	internal class NetPacket : INetPacket {
		NetMacAddress sourceMacAddress, destinationMacAddress;
		NetAddress? sourceAddress, destinationAddress;

		internal NetPacket(INetHwInterface sourceInterface, INetHwInterface destinationInterface, NetAddress? sourceAddress, NetAddress? destinationAddress) {
			this.sourceMacAddress = sourceInterface?.HardwareAddress ?? NetMacAddress.Zero;
			this.destinationMacAddress = destinationInterface?.HardwareAddress ?? NetMacAddress.Zero;
			this.sourceAddress = sourceAddress;
			this.destinationAddress = destinationAddress;
			TTL = 30;
		}

		public NetMacAddress SourceHardwareAddress { get { return sourceMacAddress; } }
		public NetMacAddress DestinationHardwareAddress { get { return destinationMacAddress; } }

		public NetAddress? SourceAddress { get { return sourceAddress; } }
		public NetAddress? DestinationAddress { get { return destinationAddress; } }

		public int TTL { get; set; }

		public INetPacket Clone( ) {
			return this.MemberwiseClone( ) as INetPacket;
		}
	}
}
