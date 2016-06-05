using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network.Packets {
	/// <summary>
	/// Link Layer Discovery Protocol
	/// </summary>
	internal class LldpResponsePacket : NetPacket {
		public LldpResponsePacket(NetMacAddress sourceHardwareAddress, NetMacAddress destinationHardwareAddress)
			: base(sourceHardwareAddress, destinationHardwareAddress) {
		}
	}
}
