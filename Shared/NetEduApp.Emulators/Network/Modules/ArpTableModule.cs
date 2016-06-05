using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Modules {
	/// <summary>
	/// Address Resolution Protocol
	/// </summary>
	internal class ArpTableModule {
		private HashSet<Entry> table;

		public ArpTableModule( ) {
			table = new HashSet<Entry>( );
		}

		public bool Register(NetIpAddress ipAddress, NetMacAddress hardwareAddress) {
			Entry item;
			item.hardwareAddress = hardwareAddress;
			item.ipAddress = ipAddress;
			if (!table.Contains(item)) {
				table.Add(item);
				return true;
			}
			return false;
		}

		public int Unregister(NetMacAddress hardwareAddress) {
			return table.RemoveWhere(x => x.hardwareAddress == hardwareAddress);
		}

		public NetMacAddress? GetAddress(NetIpAddress ipAddress) {
			return table.Where(x => x.ipAddress == ipAddress).Select(x => x.hardwareAddress).FirstOrDefault( );
		}

		private struct Entry {
			public NetMacAddress hardwareAddress;
			public NetIpAddress ipAddress;
		}
	}
}
