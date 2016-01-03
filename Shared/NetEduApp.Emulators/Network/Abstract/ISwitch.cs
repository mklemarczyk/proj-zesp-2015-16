using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface ISwitch : INetDevice {
		int PortCount { get; }
		IReadOnlyCollection<INetHwInterface> Interfaces { get; } // 8, 16, 24, 48
	}
}
