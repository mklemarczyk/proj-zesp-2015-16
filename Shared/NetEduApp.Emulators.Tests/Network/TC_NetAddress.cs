using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEduApp.Emulators.Network;
using SUTest = NetEduApp.Emulators.Network.NetAddress;

namespace NetEduApp.Emulators.Tests.Network {
    [TestClass]
    public class TC_NetAddress {

        #region Constructor

        [TestMethod]
        public void Constructor_SingleHost( ) {
            var netAddress = new SUTest(new NetIpAddress(124, 102, 11, 7));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Address.Bytes);
            CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, netAddress.Netmask.Bytes);
            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Broadcast.Bytes);
        }

        [TestMethod]
        public void Constructor_RandomAddress( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Address.Bytes);
            CollectionAssert.AreEqual(new byte[] { 255, 255, 0, 0 }, netAddress.Netmask.Bytes);
            CollectionAssert.AreEqual(new byte[] { 124, 102, 255, 255 }, netAddress.Broadcast.Bytes);
        }

        [TestMethod]
        public void Constructor_FullRandomAddress( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Address.Bytes);
            CollectionAssert.AreEqual(new byte[] { 255, 255, 0, 0 }, netAddress.Netmask.Bytes);
            CollectionAssert.AreEqual(new byte[] { 124, 102, 255, 255 }, netAddress.Broadcast.Bytes);
        }

        #endregion

        #region Methods

        [TestMethod]
        public void IsValid1( ) {
            var netAddress = new SUTest(new NetIpAddress(124, 102, 11, 7));
            Assert.AreEqual(true, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid2( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0));
            Assert.AreEqual(true, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid3( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));
            Assert.AreEqual(true, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid4( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 8));
            Assert.AreEqual(false, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid5( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 254));
            Assert.AreEqual(false, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid6( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 8),
                new NetIpAddress(124, 102, 255, 247));
            Assert.AreEqual(false, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsNetwork1( ) {
            var netAddress = new SUTest(new NetIpAddress(124, 102, 11, 7));
            Assert.AreEqual(true, netAddress.IsNetwork( ));
        }

        [TestMethod]
        public void IsNetwork2( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 128, 0),
                new NetIpAddress(255, 255, 128, 0));
            Assert.AreEqual(true, netAddress.IsNetwork( ));
        }

        [TestMethod]
        public void IsNetwork3( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0));
            Assert.AreEqual(false, netAddress.IsNetwork( ));
        }

        [TestMethod]
        public void IsHost1( ) {
            var netAddress = new SUTest(new NetIpAddress(124, 102, 11, 7));
            Assert.AreEqual(false, netAddress.IsHost( ));
        }

        [TestMethod]
        public void IsHost2( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 128, 0),
                new NetIpAddress(255, 255, 128, 0));
            Assert.AreEqual(false, netAddress.IsHost( ));
        }

        [TestMethod]
        public void IsHost3( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0));
            Assert.AreEqual(true, netAddress.IsHost( ));
        }

        [TestMethod]
        public void Contains( ) {
            var netAddressNet = new SUTest(
                new NetIpAddress(124, 102, 128, 0),
                new NetIpAddress(255, 255, 128, 0));
            var netAddressHost = new SUTest(
                new NetIpAddress(124, 102, 128, 7),
                new NetIpAddress(255, 255, 0, 0));

            Assert.AreEqual(true, netAddressNet.Contains(netAddressNet));
            Assert.AreEqual(true, netAddressNet.Contains(netAddressHost));
            Assert.AreEqual(false, netAddressHost.Contains(netAddressNet));
            Assert.AreEqual(true, netAddressHost.Contains(netAddressHost));
        }

        [TestMethod]
        public void GetNetwork1( ) {
            var netAddress = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 255, 255));

            var network = netAddress.GetNetwork( );

            Assert.AreEqual(null, network);
        }

        [TestMethod]
        public void GetNetwork2( ) {
            var netAddress = new SUTest(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(0, 0, 0, 0));
            var network = netAddress.GetNetwork( );

            Assert.AreEqual(netAddress.Netmask, network.Netmask);
            Assert.AreEqual(netAddress.Broadcast, network.Broadcast);
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, network.Address.Bytes);
        }

        [TestMethod]
        public void GetNetwork3( ) {
            var netAddress = new SUTest(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(255, 255, 254, 0));
            var network = netAddress.GetNetwork( );

            Assert.AreEqual(netAddress.Netmask, network.Netmask);
            Assert.AreEqual(netAddress.Broadcast, network.Broadcast);
            CollectionAssert.AreEqual(new byte[] { 124, 102, 10, 0 }, network.Address.Bytes);
        }

        [TestMethod]
        public void GetNetwork4( ) {
            var netAddress = new SUTest(
               new NetIpAddress(124, 101, 11, 7),
               new NetIpAddress(255, 254, 0, 0));
            var network = netAddress.GetNetwork( );

            Assert.AreEqual(netAddress.Netmask, network.Netmask);
            Assert.AreEqual(netAddress.Broadcast, network.Broadcast);
            CollectionAssert.AreEqual(new byte[] { 124, 100, 0, 0 }, network.Address.Bytes);
        }

        [TestMethod]
        public void GetNetwork5( ) {
            var netAddress = new SUTest(
                new NetIpAddress(125, 102, 11, 7),
                new NetIpAddress(254, 0, 0, 0));
            var network = netAddress.GetNetwork( );

            Assert.AreEqual(netAddress.Netmask, network.Netmask);
            Assert.AreEqual(netAddress.Broadcast, network.Broadcast);
            CollectionAssert.AreEqual(new byte[] { 124, 0, 0, 0 }, network.Address.Bytes);
        }

        [TestMethod]
        public void Equals( ) {
            var netAddressA = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressB = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressC = new SUTest(
                new NetIpAddress(124, 102, 1, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressD = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 254, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressE = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 100, 255, 255));

            var netAddressF = new SUTest(
                new NetIpAddress(124, 102, 1, 7),
                new NetIpAddress(255, 254, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressG = new SUTest(
                new NetIpAddress(124, 102, 1, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 100, 255, 255));

            var netAddressH = new SUTest(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 254, 0, 0),
                new NetIpAddress(124, 100, 255, 255));

            var netAddressI = new SUTest(
                new NetIpAddress(124, 102, 1, 7),
                new NetIpAddress(255, 254, 0, 0),
                new NetIpAddress(124, 100, 255, 255));

            Assert.AreEqual(true, netAddressA.Equals(netAddressA));
            Assert.AreEqual(true, netAddressA.Equals(netAddressB));
            Assert.AreEqual(true, netAddressB.Equals(netAddressA));

            Assert.AreEqual(false, netAddressA.Equals(netAddressC));
            Assert.AreEqual(false, netAddressA.Equals(netAddressD));
            Assert.AreEqual(false, netAddressA.Equals(netAddressE));
            Assert.AreEqual(false, netAddressA.Equals(netAddressF));
            Assert.AreEqual(false, netAddressA.Equals(netAddressG));
            Assert.AreEqual(false, netAddressA.Equals(netAddressH));
            Assert.AreEqual(false, netAddressA.Equals(netAddressI));
        }

        #endregion

        #region Static methods

        [TestMethod]
        public void NetmaskIsValid( ) {
            var result = SUTest.NetmaskIsValid(new NetIpAddress(255, 255, 255, 255));
            Assert.AreEqual(true, result);

            result = SUTest.NetmaskIsValid(new NetIpAddress(0, 0, 0, 0));
            Assert.AreEqual(true, result);

            result = SUTest.NetmaskIsValid(new NetIpAddress(255, 255, 254, 0));
            Assert.AreEqual(true, result);

            result = SUTest.NetmaskIsValid(new NetIpAddress(255, 254, 0, 0));
            Assert.AreEqual(true, result);

            result = SUTest.NetmaskIsValid(new NetIpAddress(254, 0, 0, 0));
            Assert.AreEqual(true, result);

            result = SUTest.NetmaskIsValid(new NetIpAddress(0, 0, 0, 0));
            Assert.AreEqual(true, result);

            result = SUTest.NetmaskIsValid(new NetIpAddress(255, 255, 254, 255));
            Assert.AreEqual(false, result);

            result = SUTest.NetmaskIsValid(new NetIpAddress(253, 0, 0, 0));
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ComputeBroadcast( ) {
            var netAddress = SUTest.ComputeBroadcast(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 255, 255));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Bytes);

            netAddress = SUTest.ComputeBroadcast(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(0, 0, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, netAddress.Bytes);

            netAddress = SUTest.ComputeBroadcast(
               new NetIpAddress(124, 102, 10, 7),
               new NetIpAddress(255, 255, 254, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 255 }, netAddress.Bytes);

            netAddress = SUTest.ComputeBroadcast(
               new NetIpAddress(124, 100, 11, 7),
               new NetIpAddress(255, 254, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 101, 255, 255 }, netAddress.Bytes);

            netAddress = SUTest.ComputeBroadcast(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(254, 0, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 125, 255, 255, 255 }, netAddress.Bytes);
        }

        [TestMethod]
        public void ComputeNetworkAddress( ) {
            var netAddress = SUTest.ComputeNetworkAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 255, 255));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Bytes);

            netAddress = SUTest.ComputeNetworkAddress(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(0, 0, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, netAddress.Bytes);

            netAddress = SUTest.ComputeNetworkAddress(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(255, 255, 254, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 10, 0 }, netAddress.Bytes);

            netAddress = SUTest.ComputeNetworkAddress(
               new NetIpAddress(124, 101, 11, 7),
               new NetIpAddress(255, 254, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 100, 0, 0 }, netAddress.Bytes);

            netAddress = SUTest.ComputeNetworkAddress(
               new NetIpAddress(125, 102, 11, 7),
               new NetIpAddress(254, 0, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 0, 0, 0 }, netAddress.Bytes);
        }

        #endregion

    }
}
