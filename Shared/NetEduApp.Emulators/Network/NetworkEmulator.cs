using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;

namespace NetEduApp.Emulators.Network {
	internal class NetworkEmulator : INetEmulator {
		internal void MakeLink(INetHwInterface a, INetHwInterface b) {
			Disconnect(a);
			Disconnect(b);
			Connect(a, b);
		}

		internal void UnLink(INetHwInterface a) {
			Disconnect(a);
		}

		#region Private methods
		private void Connect(INetHwInterface a, INetHwInterface b) {
            a.Connect(b);
		}

		private void Disconnect(INetHwInterface a) {
            a.Disconnect( );
		}
		#endregion
	}
}
