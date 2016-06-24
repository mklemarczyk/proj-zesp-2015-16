using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Abstract;

namespace NetEduApp.Simulator {
	internal class NetPacket : INetPacket {
		NetMacAddress sourceMacAddress, destinationMacAddress;
		NetAddress? sourceAddress, destinationAddress;

		internal NetPacket(NetMacAddress sourceHardwareAddress, NetMacAddress destinationHardwareAddress) {
			this.sourceMacAddress = sourceHardwareAddress;
			this.destinationMacAddress = destinationHardwareAddress;
			TTL = 30;
		}

		internal NetPacket(NetMacAddress sourceHardwareAddress, NetMacAddress destinationHardwareAddress, NetAddress? sourceAddress, NetAddress? destinationAddress)
			: this(sourceHardwareAddress, destinationHardwareAddress) {
			this.sourceAddress = sourceAddress;
			this.destinationAddress = destinationAddress;
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
