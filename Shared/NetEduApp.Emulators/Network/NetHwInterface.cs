using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
    internal class NetHwInterface : INetHwInterface {
        private INetEmulator emulator;
        private INetDevice parent;
        private string name;

        internal NetHwInterface(INetEmulator emulator, INetDevice parent, string name) {
            this.emulator = emulator;
            this.parent = parent;
            this.name = name;
        }

        public INetEmulator Emulator { get { return this.emulator; } }
        public INetDevice Parent { get { return this.parent; } }
        public string Name { get { return this.name; } }

        public virtual void ReceiveData(INetPacket data) {
            if (data.DestinationInterface == this)
                parent.ReceiveData(data);
        }

        public virtual void SendData(INetPacket data) {
            var destination = emulator.GetDestination(this);
            var source = this;
            emulator.TransferData(new NetPacket(source, destination, data.SourceAddress, data.DestinationAddress));
        }
    }
}
