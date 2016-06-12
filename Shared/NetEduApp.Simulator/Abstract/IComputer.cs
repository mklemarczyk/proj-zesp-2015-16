using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Simulator.Abstract {
	public interface IComputer : INetDevice {
		int PortCount { get; }
		new IReadOnlyList<INetLgInterface> Interfaces { get; }
		IList<INetRoute> Routes { get; }
		INetRoute DefaultRoute { get; set; }

		void SendPing(NetIpAddress iPAddress);
	}
}
