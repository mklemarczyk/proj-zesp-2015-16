using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Abstract {
    public interface INetIpAddress : IEquatable<INetIpAddress> {
        byte[] Bytes { get; }

        uint GetUintRepresentation( );
    }
}
