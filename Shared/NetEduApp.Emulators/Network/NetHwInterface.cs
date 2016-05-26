using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Logger;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
    internal class NetHwInterface : INetHwInterface {
        private INetDevice parent;
        private INetHwInterface otherInterface;
        private string name;

        internal NetHwInterface(INetDevice parent, string name) {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (name == null)
                throw new ArgumentNullException("name");
            this.parent = parent;
            this.name = name;
        }

        public INetDevice Parent { get { return this.parent; } }
        public string Name { get { return this.name; } }

        public virtual void Connect(INetHwInterface other) {
            if(other is NetHwInterface) {
                this.Connect((NetHwInterface)other);
            } else {
                throw new ArgumentException("Invalid other type, it require NetHwInterface object");
            }
        }

        public virtual void Disconnect() {
            var other = otherInterface;
            otherInterface.Disconnect( );
            if (other != null) {
                other.Disconnect( );
            }
            EmulatorLogger.Log(LogLevel.Info, EventType.Disconnected, string.Empty);
        }

        private void Connect(NetHwInterface other) {
            if (otherInterface != null) {
                otherInterface.Disconnect( );
            }
            if (other != null) {
                other.Disconnect( );
                other.otherInterface = this;
            }
            otherInterface = other;
            EmulatorLogger.Log(LogLevel.Info, EventType.Connected, string.Empty);
        }

        public virtual void ReceiveData(INetPacket data) {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.DestinationInterface == this)
                parent.ReceiveData(data);
        }

        public virtual void SendData(INetPacket data) {
            otherInterface.ReceiveData(new NetPacket(this, otherInterface, data.SourceAddress, data.DestinationAddress));
        }
    }
}
