using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetPacket {
		INetHwInterface SourceInterface { get; }
		INetHwInterface DestinationInterface { get; }
		INetAddress SourceAddress { get; }
		INetAddress DestinationAddress { get; }
		int TTL { get; set; }
	}
}
