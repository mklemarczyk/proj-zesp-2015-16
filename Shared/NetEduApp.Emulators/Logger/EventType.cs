using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Logger {
    public enum EventType {
        #region Connection
        Connected,
        Disconnected,
        NotConnected,
        #endregion

        #region Packet transmission
        PacketSend,
        PacketRecived,
        #endregion
    }
}
