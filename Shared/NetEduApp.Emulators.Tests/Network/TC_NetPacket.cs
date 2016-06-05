using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEduApp.Emulators.Network;
using NetEduApp.Emulators.Network.Abstract;
using SUTest = NetEduApp.Emulators.Network.NetPacket;

namespace NetEduApp.Emulators.Tests.Network {
	[TestClass]
	public class TC_NetPacket {

		#region Constructor

		[TestMethod]
		public void Constructor( ) {
			var srcMac = new NetMacAddress(1LU);

			var dstMac = new NetMacAddress(2LU);

			var srcInterf = new Emulators.Network.Abstract.Fakes.StubINetHwInterface( );
			srcInterf.HardwareAddressGet = ( ) => srcMac;

			var dstInterf = new Emulators.Network.Abstract.Fakes.StubINetHwInterface( );
			dstInterf.HardwareAddressGet = ( ) => dstMac;

			var srcAddress = new NetAddress(
				new NetIpAddress(182, 95, 34, 147),
				new NetIpAddress(255, 254, 11, 201),
				new NetIpAddress(182, 94, 0, 0));

			var dstAddress = new NetAddress(
				new NetIpAddress(214, 21, 157, 193),
				new NetIpAddress(255, 255, 192, 0),
				new NetIpAddress(214, 21, 0, 0));

			var netPacket = new SUTest(
				((INetHwInterface)srcInterf).HardwareAddress,
				((INetHwInterface)dstInterf).HardwareAddress,
				srcAddress,
				dstAddress);

			Assert.AreEqual(srcMac, netPacket.SourceHardwareAddress);
			Assert.AreEqual(dstMac, netPacket.DestinationHardwareAddress);
			Assert.AreEqual(srcAddress, netPacket.SourceAddress);
			Assert.AreEqual(dstAddress, netPacket.DestinationAddress);
			Assert.AreEqual(30, netPacket.TTL);
		}

		[TestMethod]
		public void Constructor_NullAddress( ) {
			var srcMac = new NetMacAddress(1LU);

			var dstMac = new NetMacAddress(2LU);

			var srcInterf = new Emulators.Network.Abstract.Fakes.StubINetHwInterface( );
			srcInterf.HardwareAddressGet = ( ) => srcMac;

			var dstInterf = new Emulators.Network.Abstract.Fakes.StubINetHwInterface( );
			dstInterf.HardwareAddressGet = ( ) => dstMac;

			var netPacket = new SUTest(
				((INetHwInterface)srcInterf).HardwareAddress,
				((INetHwInterface)dstInterf).HardwareAddress,
				null,
				null);

			Assert.AreEqual(srcMac, netPacket.SourceHardwareAddress);
			Assert.AreEqual(dstMac, netPacket.DestinationHardwareAddress);
			Assert.AreEqual(null, netPacket.SourceAddress);
			Assert.AreEqual(null, netPacket.DestinationAddress);
			Assert.AreEqual(30, netPacket.TTL);
		}

		#endregion

		#region Methods

		[TestMethod]
		public void Clone( ) {
			var srcMac = new NetMacAddress(1LU);

			var dstMac = new NetMacAddress(2LU);

			var srcInterf = new Emulators.Network.Abstract.Fakes.StubINetHwInterface( );
			srcInterf.HardwareAddressGet = ( ) => srcMac;

			var dstInterf = new Emulators.Network.Abstract.Fakes.StubINetHwInterface( );
			dstInterf.HardwareAddressGet = ( ) => dstMac;

			var srcAddress = new NetAddress(
				new NetIpAddress(182, 95, 34, 147),
				new NetIpAddress(255, 254, 11, 201),
				new NetIpAddress(182, 94, 0, 0));

			var dstAddress = new NetAddress(
				new NetIpAddress(214, 21, 157, 193),
				new NetIpAddress(255, 255, 192, 0),
				new NetIpAddress(214, 21, 0, 0));

			var netPacket = new SUTest(
				((INetHwInterface)srcInterf).HardwareAddress,
				((INetHwInterface)dstInterf).HardwareAddress,
				srcAddress,
				dstAddress);
			netPacket.TTL = 20;

			var cloned = netPacket.Clone( );

			Assert.AreEqual(netPacket.SourceHardwareAddress, cloned.SourceHardwareAddress);
			Assert.AreEqual(netPacket.DestinationHardwareAddress, cloned.DestinationHardwareAddress);
			Assert.AreEqual(netPacket.SourceAddress, cloned.SourceAddress);
			Assert.AreEqual(netPacket.DestinationAddress, cloned.DestinationAddress);
			Assert.AreEqual(netPacket.TTL, cloned.TTL);
		}

		#endregion

	}
}
