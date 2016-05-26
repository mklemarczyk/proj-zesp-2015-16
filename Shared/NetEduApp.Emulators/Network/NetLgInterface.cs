using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Logger;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
    internal class NetLgInterface : NetHwInterface, INetLgInterface {
        private INetLgInterface otherInterface;

        internal NetLgInterface(INetDevice parent, string name) : base(parent, name) { }

        public NetAddress? Address { get; set; }

        public override void Connect(INetHwInterface other) {
            if (other is NetLgInterface) {
                this.Connect((NetLgInterface)other);
            } else {
                throw new ArgumentException("Invalid other type, it require NetLgInterface object");
            }
        }

        public override void Disconnect( ) {
            var other = otherInterface;
            if (other != null) {
                otherInterface = null;
                EmulatorLogger.Log(LogLevel.Info, EventType.Disconnected, this.Name);
                other.Disconnect( );
            }
        }

        private void Connect(NetLgInterface other) {
            if (otherInterface != null) {
                otherInterface.Disconnect( );
            }
            if (other != null) {
                other.Disconnect( );
                other.otherInterface = this;
            }
            otherInterface = other;
            EmulatorLogger.Log(LogLevel.Info, EventType.Connected, this.Name);
        }

        public override void ReceiveData(INetPacket data) {
            base.ReceiveData(data);
        }

        public override void SendData(INetPacket data) {
            if (otherInterface != null) {
                EmulatorLogger.Log(LogLevel.Info, EventType.PacketSend, this.Name);
                otherInterface.ReceiveData(new NetPacket(this, otherInterface, data.SourceAddress ?? this.Address, data.DestinationAddress));
            }else {
                EmulatorLogger.Log(LogLevel.Error, EventType.NotConnected, "There are no connected devie: " + this.Name);
            }
        }
    }
}
