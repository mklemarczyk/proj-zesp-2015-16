using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network.Abstract;
using NetEduApp.Emulators.Network.Devices;

namespace NetEduApp.Emulators.Network {
	public class NetworkFactory {
		private NetworkEmulator emulator;
		private HashSet<string> names;
		private HashSet<INetDevice> devices;

		private NetworkFactory( ) {
			emulator = new NetworkEmulator( );
			names = new HashSet<string>( );
			devices = new HashSet<INetDevice>( );
		}

		#region Static
		internal static NetworkFactory instance;
		private static int i;

		public static IHub CreateHub( ) {
			var name = "Hub{0}";
			var device = new Hub( );
			var devName = string.Format(name, i++);
			device.Name = devName;
			return device;
		}

		public static void MakeLink(INetHwInterface a, INetHwInterface b) {
			instance.emulator.MakeLink(a, b);
		}

		public static ISwitch CreateSwitch( ) {
			var name = "Switch{0}";
			var device = new Switch( );
			var devName = string.Format(name, i++);
			device.Name = devName;
			return device;
		}
		public static IRouter CreateRouter( ) {
			var name = "Router{0}";
			var devName = string.Format(name, i++);
			var device = new Router(instance.emulator, devName);
			return device;
		}
		public static IComputer CreateComputer( ) {
			var name = "Computer{0}";
			var devName = string.Format(name, i++);
			var device = new Computer(instance.emulator, devName);
			device.Name = devName;
			return device;
		}

		static NetworkFactory( ) {
			instance = new NetworkFactory( );
		}
		#endregion
	}
}
