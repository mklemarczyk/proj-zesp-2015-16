using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetAddress : IEquatable<INetAddress> {
		NetIpAddress Address { get; }
		NetIpAddress Netmask { get; }
		NetIpAddress Broadcast { get; }

		bool IsValid( );
		bool IsNetwork( );
		bool IsHost( );
		bool Contains(INetAddress hostAddress);
		INetAddress GetNetwork( );
	}
}
