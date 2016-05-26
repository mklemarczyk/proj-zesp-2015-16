using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetRoute {
		bool IsDefault { get; }
		NetAddress Address { get; }
		NetAddress Target { get; }

		bool IsMatch(NetAddress destinationAddress);
	}
}
