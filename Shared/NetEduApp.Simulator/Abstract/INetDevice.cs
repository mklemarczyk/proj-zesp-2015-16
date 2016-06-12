using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Simulator.Abstract {
	public interface INetDevice : IDisposable {
		string Name { get; set; }
		string Type { get; }

		IReadOnlyList<INetHwInterface> Interfaces { get; }
		int PortCount { get; }

		void ReceiveData(INetPacket data, INetHwInterface iface);
		void SendData(INetPacket data);
	}
}
