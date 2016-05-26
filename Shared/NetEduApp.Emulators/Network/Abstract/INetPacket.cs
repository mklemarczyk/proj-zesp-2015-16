using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetPacket {
		INetHwInterface SourceInterface { get; }
		INetHwInterface DestinationInterface { get; }
		NetAddress? SourceAddress { get; }
		NetAddress? DestinationAddress { get; }
		int TTL { get; set; }

		INetPacket Clone( );
	}
}
