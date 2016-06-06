using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEduApp.Emulators.Network.Modules {
	/// <summary>
	/// Link Layer Discovery Protocol
	/// </summary>
	internal class LldpTableModule {
		private HashSet<Entry> table;

		public LldpTableModule( ) {
			table = new HashSet<Entry>( );
		}

		public bool Register(int portNo, NetMacAddress hardwareAddress) {
			Entry item;
			item.portNo = portNo;
			item.hardwareAddress = hardwareAddress;
			if (!table.Contains(item)) {
				table.Add(item);
				return true;
			}
			return false;
		}

		public int Unregister(int portNo) {
			return table.RemoveWhere(x => x.portNo == portNo);
		}

		public int? GetPort(NetMacAddress hardwareAddress) {
			return table.Where(x => x.hardwareAddress == hardwareAddress).Select(x => (int?)x.portNo).FirstOrDefault( );
		}

		private struct Entry {
			public int portNo;
			public NetMacAddress hardwareAddress;
		}
	}
}
