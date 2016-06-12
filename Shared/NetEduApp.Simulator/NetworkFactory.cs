using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Simulator.Abstract;
using NetEduApp.Simulator.Devices;

namespace NetEduApp.Simulator {
	public class NetworkFactory {
		private HashSet<INetDevice> devices;

		private NetworkFactory( ) {
			devices = new HashSet<INetDevice>( );
		}

		#region Static
		internal static NetworkFactory instance;
		private static int i;

		public static IHub CreateHub( ) {
			var name = "Hub{0}";
			var devName = string.Format(name, i++);
			var device = new Hub(devName);
			instance.devices.Add(device);
			return device;
		}

		public static IRouter CreateRouter( ) {
			var name = "Router{0}";
			var devName = string.Format(name, i++);
			var device = new Router(devName);
			instance.devices.Add(device);
			return device;
		}

		public static IComputer CreateComputer( ) {
			var name = "Computer{0}";
			var devName = string.Format(name, i++);
			var device = new Computer(devName);
			device.Name = devName;
			instance.devices.Add(device);
			return device;
		}

		public static void MakeLink(INetHwInterface a, INetHwInterface b) {
			Disconnect(a);
			Disconnect(b);
			Connect(a, b);
		}

		public static void UnLink(INetHwInterface a) {
			Disconnect(a);
		}

		public static void Clear( ) {
			foreach(var device in instance.devices) {
				device.Dispose( );
			}
			instance.devices.Clear( );
			i = 0;
		}

		private static void Connect(INetHwInterface a, INetHwInterface b) {
			a.Connect(b);
		}

		private static void Disconnect(INetHwInterface a) {
			a.Disconnect( );
		}

		static NetworkFactory( ) {
			instance = new NetworkFactory( );
		}
		#endregion
	}
}
