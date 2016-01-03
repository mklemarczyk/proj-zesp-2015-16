using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
	internal class NetLgInterface : NetHwInterface, INetLgInterface {
		internal NetLgInterface(INetEmulator emulator, INetDevice parent, string name) : base(emulator, parent, name) { }

		public INetAddress Address { get; set; }

		public override void SendData(INetPacket data) {
			var destination = this.Emulator.GetDestination(this);
			var source = this;
			Emulator.TransferData(new NetPacket(source, destination, data.SourceAddress ?? this.Address, data.DestinationAddress));
		}
	}
}
