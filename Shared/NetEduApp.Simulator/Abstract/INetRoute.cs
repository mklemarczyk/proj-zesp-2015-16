using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Simulator.Abstract {
	public interface INetRoute {
		bool IsDefault { get; }
		NetAddress Address { get; }
		NetAddress Target { get; }

		bool IsValid( );
		bool IsMatch(NetAddress destinationAddress);
	}
}
