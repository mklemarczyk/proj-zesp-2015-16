using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface IHub : INetDevice {
		int PortCount { get; }
		IReadOnlyList<INetHwInterface> Interfaces { get; } // 4, 8, 16
	}
}
