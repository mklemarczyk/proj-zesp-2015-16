using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEduApp.Simulator;
using SUTest = NetEduApp.Simulator.NetRoute;

namespace NetEduApp.Simulator.Tests.Network {
	[TestClass]
	public class TC_NetRoute {

		#region Constructor

		[TestMethod]
		public void Constructor_DefaultRoute( ) {
			var netRoute = new SUTest(new NetIpAddress(124, 102, 11, 7));

			Assert.AreEqual(true, netRoute.IsDefault);
			CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netRoute.Target.Address.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.MaxAddress.Bytes, netRoute.Target.Netmask.Bytes);
			CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netRoute.Target.Broadcast.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.Zero.Bytes, netRoute.Address.Address.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.Zero.Bytes, netRoute.Address.Netmask.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.MaxAddress.Bytes, netRoute.Address.Broadcast.Bytes);
		}

		[TestMethod]
		public void Constructor_CustomRoute_Default( ) {
			var netRoute = new SUTest(
				NetIpAddress.Zero,
				NetIpAddress.Zero,
				new NetIpAddress(172, 144, 11, 201));

			Assert.AreEqual(true, netRoute.IsDefault);
			CollectionAssert.AreEqual(new byte[] { 172, 144, 11, 201 }, netRoute.Target.Address.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.MaxAddress.Bytes, netRoute.Target.Netmask.Bytes);
			CollectionAssert.AreEqual(new byte[] { 172, 144, 11, 201 }, netRoute.Target.Broadcast.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.Zero.Bytes, netRoute.Address.Address.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.Zero.Bytes, netRoute.Address.Netmask.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.MaxAddress.Bytes, netRoute.Address.Broadcast.Bytes);
		}

		[TestMethod]
		public void Constructor_CustomRoute_RandomValid( ) {
			var netRoute = new SUTest(
				new NetIpAddress(124, 102, 0, 0),
				new NetIpAddress(255, 255, 0, 0),
				new NetIpAddress(172, 144, 11, 201));

			Assert.AreEqual(false, netRoute.IsDefault);
			CollectionAssert.AreEqual(new byte[] { 172, 144, 11, 201 }, netRoute.Target.Address.Bytes);
			CollectionAssert.AreEqual(NetIpAddress.MaxAddress.Bytes, netRoute.Target.Netmask.Bytes);
			CollectionAssert.AreEqual(new byte[] { 172, 144, 11, 201 }, netRoute.Target.Broadcast.Bytes);
			CollectionAssert.AreEqual(new byte[] { 124, 102, 0, 0 }, netRoute.Address.Address.Bytes);
			CollectionAssert.AreEqual(new byte[] { 255, 255, 0, 0 }, netRoute.Address.Netmask.Bytes);
			CollectionAssert.AreEqual(new byte[] { 124, 102, 255, 255 }, netRoute.Address.Broadcast.Bytes);
		}

		#endregion

		#region Methods

		[TestMethod]
		public void IsValid_DefaultRoute( ) {
			var netRoute = new SUTest(new NetIpAddress(124, 102, 11, 7));

			Assert.AreEqual(true, netRoute.IsValid( ));
		}

		[TestMethod]
		public void IsValid_CustomRoute_Default( ) {
			var netRoute = new SUTest(
				NetIpAddress.Zero,
				NetIpAddress.Zero,
				new NetIpAddress(172, 144, 11, 201));

			Assert.AreEqual(true, netRoute.IsValid( ));
		}

		[TestMethod]
		public void IsValid_CustomRoute_RandomInvalidNetwork( ) {
			var netRoute = new SUTest(
				 new NetIpAddress(124, 102, 0, 1),
				 new NetIpAddress(255, 255, 0, 0),
				 new NetIpAddress(172, 144, 11, 201));

			Assert.AreEqual(false, netRoute.IsValid( ));
		}

		[TestMethod]
		public void IsValid_CustomRoute_RandomInvalidMask( ) {
			var netRoute = new SUTest(
				  new NetIpAddress(124, 102, 0, 0),
				  new NetIpAddress(255, 255, 0, 1),
				  new NetIpAddress(172, 144, 11, 201));

			Assert.AreEqual(false, netRoute.IsValid( ));
		}

		[TestMethod]
		public void IsValid_CustomRoute_RandomValid( ) {
			var netRoute = new SUTest(
				new NetIpAddress(124, 102, 0, 0),
				new NetIpAddress(255, 255, 0, 0),
				new NetIpAddress(172, 144, 11, 201));

			Assert.AreEqual(true, netRoute.IsValid( ));
		}

		[TestMethod]
		public void IsMatch_OutsideBefore( ) {
			var netRoute = new SUTest(
				new NetIpAddress(124, 102, 0, 0),
				new NetIpAddress(255, 255, 0, 0),
				new NetIpAddress(172, 144, 11, 201));

			var netIpAddressOutsideBefore = new NetAddress(
				new NetIpAddress(124, 101, 255, 254),
				new NetIpAddress(255, 255, 0, 0));

			Assert.AreEqual(false, netRoute.IsMatch(netIpAddressOutsideBefore));
		}

		[TestMethod]
		public void IsMatch_InsideAfterOutside( ) {
			var netRoute = new SUTest(
				new NetIpAddress(124, 102, 0, 0),
				new NetIpAddress(255, 255, 0, 0),
				new NetIpAddress(172, 144, 11, 201));

			var netIpAddressInsideAfterOutside = new NetAddress(
				new NetIpAddress(124, 102, 0, 0),
				new NetIpAddress(255, 255, 0, 0));

			Assert.AreEqual(true, netRoute.IsMatch(netIpAddressInsideAfterOutside));
		}

		[TestMethod]
		public void IsMatch_Inside( ) {
			var netRoute = new SUTest(
				new NetIpAddress(124, 102, 0, 0),
				new NetIpAddress(255, 255, 0, 0),
				new NetIpAddress(172, 144, 11, 201));

			var netIpAddressInside = new NetAddress(
				new NetIpAddress(124, 102, 11, 7),
				new NetIpAddress(255, 255, 0, 0));

			Assert.AreEqual(true, netRoute.IsMatch(netIpAddressInside));
		}

		[TestMethod]
		public void IsMatch_InsideBeforeOutside( ) {
			var netRoute = new SUTest(
				new NetIpAddress(124, 102, 0, 0),
				new NetIpAddress(255, 255, 0, 0),
				new NetIpAddress(172, 144, 11, 201));

			var netIpAddressInsideBeforeOutside = new NetAddress(
				new NetIpAddress(124, 102, 255, 255),
				new NetIpAddress(255, 255, 0, 0));

			Assert.AreEqual(true, netRoute.IsMatch(netIpAddressInsideBeforeOutside));
		}

		[TestMethod]
		public void IsMatch_OutsideAfter( ) {
			var netRoute = new SUTest(
				new NetIpAddress(124, 102, 0, 0),
				new NetIpAddress(255, 255, 0, 0),
				new NetIpAddress(172, 144, 11, 201));

			var netIpAddressOutsideAfter = new NetAddress(
				new NetIpAddress(124, 103, 0, 1),
				new NetIpAddress(255, 255, 0, 0));

			Assert.AreEqual(false, netRoute.IsMatch(netIpAddressOutsideAfter));
		}

		#endregion

	}
}
