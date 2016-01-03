using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetRoute {
		bool IsDefault { get; }
		INetAddress Address { get; }
		INetAddress Target { get; }

		bool IsMatch(INetAddress destinationAddress);
	}
}
