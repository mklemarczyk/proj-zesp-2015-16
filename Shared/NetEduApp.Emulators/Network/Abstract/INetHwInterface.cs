using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetHwInterface {
		INetEmulator Emulator { get; }
		INetDevice Parent { get; }
		string Name { get; }

		void ReciveData(INetPacket data);
		void SendData(INetPacket data);
	}
}
