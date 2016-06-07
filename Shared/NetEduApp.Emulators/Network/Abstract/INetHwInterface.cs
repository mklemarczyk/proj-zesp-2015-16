using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
	public interface INetHwInterface {
		INetDevice Parent { get; }
		string Name { get; }
		NetMacAddress HardwareAddress { get; }

		void Connect(INetHwInterface other);
		void Disconnect( );
		void ReceiveData(INetPacket data);
		void SendData(INetPacket data);
	}
}
