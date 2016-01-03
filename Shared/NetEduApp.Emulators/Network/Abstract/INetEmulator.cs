using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetEmulator {
		INetHwInterface GetDestination(INetHwInterface netHwInterface);
		void TransferData(INetPacket netPacket);
	}
}
