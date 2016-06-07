using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetPacket {
		NetMacAddress SourceHardwareAddress { get; }
		NetMacAddress DestinationHardwareAddress { get; }
		NetAddress? SourceAddress { get; }
		NetAddress? DestinationAddress { get; }
		int TTL { get; set; }

		INetPacket Clone( );
	}
}
