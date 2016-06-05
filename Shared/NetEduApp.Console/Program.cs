using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetEduApp.Emulators.Network;

namespace NetEduApp.Console {
	class Program {
		static void Main(string[] args) {
			var pc1 = NetworkFactory.CreateComputer( );
			var pc2 = NetworkFactory.CreateComputer( );
			var pc3 = NetworkFactory.CreateComputer( );

			var r1 = NetworkFactory.CreateRouter( );
			var r2 = NetworkFactory.CreateRouter( );
			var r3 = NetworkFactory.CreateRouter( );

			var h1 = NetworkFactory.CreateHub( );
			var h2 = NetworkFactory.CreateHub( );
			var h3 = NetworkFactory.CreateHub( );

			var s1 = NetworkFactory.CreateSwitch( );
			var s2 = NetworkFactory.CreateSwitch( );
			var s3 = NetworkFactory.CreateSwitch( );
			var s4 = NetworkFactory.CreateSwitch( );

			NetworkFactory.MakeLink(pc1.Interfaces[0], h1.Interfaces[0]);
			NetworkFactory.MakeLink(pc2.Interfaces[0], h2.Interfaces[0]);
			NetworkFactory.MakeLink(pc3.Interfaces[0], h3.Interfaces[0]);

			NetworkFactory.MakeLink(h1.Interfaces[1], s1.Interfaces[0]);
			NetworkFactory.MakeLink(h2.Interfaces[1], s2.Interfaces[0]);
			NetworkFactory.MakeLink(h3.Interfaces[1], s3.Interfaces[0]);

			NetworkFactory.MakeLink(s1.Interfaces[1], r1.Interfaces[0]);
			NetworkFactory.MakeLink(s2.Interfaces[1], r2.Interfaces[0]);
			NetworkFactory.MakeLink(s3.Interfaces[1], r3.Interfaces[0]);

			NetworkFactory.MakeLink(r1.Interfaces[1], s4.Interfaces[1]);
			NetworkFactory.MakeLink(r2.Interfaces[1], s4.Interfaces[2]);
			NetworkFactory.MakeLink(r3.Interfaces[1], s4.Interfaces[2]);

			pc1.Interfaces[0].Address = new NetAddress(IPAddressParse("192.168.1.1"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.1.255"));
			pc2.Interfaces[0].Address = new NetAddress(IPAddressParse("192.168.2.1"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.2.255"));
			pc3.Interfaces[0].Address = new NetAddress(IPAddressParse("192.168.3.1"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.3.255"));

			r1.Interfaces[0].Address = new NetAddress(IPAddressParse("192.168.1.100"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.1.255"));
			r1.Interfaces[1].Address = new NetAddress(IPAddressParse("192.168.4.1"), IPAddressParse("255.255.255.240"), IPAddressParse("192.168.4.255"));

			r2.Interfaces[0].Address = new NetAddress(IPAddressParse("192.168.2.100"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.2.255"));
			r2.Interfaces[1].Address = new NetAddress(IPAddressParse("192.168.4.2"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.4.255"));
			r2.Interfaces[2].Address = new NetAddress(IPAddressParse("192.168.5.2"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.5.255"));

			r3.Interfaces[0].Address = new NetAddress(IPAddressParse("192.168.3.100"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.3.255"));
			r3.Interfaces[1].Address = new NetAddress(IPAddressParse("192.168.5.1"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.5.255"));

			pc1.DefaultRoute = new NetRoute(IPAddressParse("192.168.1.100"));
			pc2.DefaultRoute = new NetRoute(IPAddressParse("192.168.2.100"));
			pc3.DefaultRoute = new NetRoute(IPAddressParse("192.168.3.100"));

			r1.Routes.Add(new NetRoute(IPAddressParse("192.168.2.0"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.4.2")));
			r1.Routes.Add(new NetRoute(IPAddressParse("192.168.3.0"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.4.2")));

			r2.Routes.Add(new NetRoute(IPAddressParse("192.168.1.0"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.4.1")));
			r2.Routes.Add(new NetRoute(IPAddressParse("192.168.3.0"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.5.1")));

			r3.Routes.Add(new NetRoute(IPAddressParse("192.168.1.0"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.5.2")));
			r3.Routes.Add(new NetRoute(IPAddressParse("192.168.2.0"), IPAddressParse("255.255.255.0"), IPAddressParse("192.168.5.2")));

			pc1.SendPing(IPAddressParse("192.168.3.1"));

			System.Console.WriteLine("EOS");
			System.Console.ReadKey( );
		}

		static NetIpAddress IPAddressParse(string ipAddress) {
			return new NetIpAddress(IPAddress.Parse(ipAddress).GetAddressBytes( ));
		}
	}
}
