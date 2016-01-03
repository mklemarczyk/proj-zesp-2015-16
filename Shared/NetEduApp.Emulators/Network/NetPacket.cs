using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
    internal class NetPacket : INetPacket {
        INetHwInterface sourceInterface, destinationInterface;
        INetAddress sourceAddress, destinationAddress;

        internal NetPacket(INetHwInterface sourceInterface, INetHwInterface destinationInterface, INetAddress sourceAddress, INetAddress destinationAddress) {
            this.sourceInterface = sourceInterface;
            this.destinationInterface = destinationInterface;
            this.sourceAddress = sourceAddress;
            this.destinationAddress = destinationAddress;
            TTL = 30;
        }
        public INetHwInterface SourceInterface { get { return sourceInterface; } }
        public INetHwInterface DestinationInterface { get { return destinationInterface; } }
        public INetAddress SourceAddress { get { return sourceAddress; } }
        public INetAddress DestinationAddress { get { return destinationAddress; } }

        public int TTL { get; set; }
    }
}
