using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetDevice {
		INetEmulator Emulator { get; }
		string Name { get; set; }
		string Type { get; }

		void ReceiveData(INetPacket data, INetHwInterface iface);
		void SendData(INetPacket data);
	}
}
