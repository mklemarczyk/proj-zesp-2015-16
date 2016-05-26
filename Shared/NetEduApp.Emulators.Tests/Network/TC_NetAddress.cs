using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEduApp.Emulators.Network;

namespace NetEduApp.Emulators.Tests.Network {
    [TestClass]
    public class TC_NetAddress {

        #region Constructor

        [TestMethod]
        public void Constructor_SingleHost( ) {
            var netAddress = new NetAddress(new NetIpAddress(124, 102, 11, 7));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Address.bytes);
            CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, netAddress.Netmask.bytes);
            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Broadcast.bytes);
        }

        [TestMethod]
        public void Constructor_RandomAddress( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Address.bytes);
            CollectionAssert.AreEqual(new byte[] { 255, 255, 0, 0 }, netAddress.Netmask.bytes);
            CollectionAssert.AreEqual(new byte[] { 124, 102, 255, 255 }, netAddress.Broadcast.bytes);
        }

        [TestMethod]
        public void Constructor_FullRandomAddress( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.Address.bytes);
            CollectionAssert.AreEqual(new byte[] { 255, 255, 0, 0 }, netAddress.Netmask.bytes);
            CollectionAssert.AreEqual(new byte[] { 124, 102, 255, 255 }, netAddress.Broadcast.bytes);
        }

        #endregion

        #region Methods

        [TestMethod]
        public void IsValid1( ) {
            var netAddress = new NetAddress(new NetIpAddress(124, 102, 11, 7));
            Assert.AreEqual(true, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid2( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0));
            Assert.AreEqual(true, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid3( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));
            Assert.AreEqual(true, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid4( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 8));
            Assert.AreEqual(false, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid5( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 254));
            Assert.AreEqual(false, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsValid6( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 8),
                new NetIpAddress(124, 102, 255, 247));
            Assert.AreEqual(false, netAddress.IsValid( ));
        }

        [TestMethod]
        public void IsNetwork1( ) {
            var netAddress = new NetAddress(new NetIpAddress(124, 102, 11, 7));
            Assert.AreEqual(true, netAddress.IsNetwork( ));
        }

        [TestMethod]
        public void IsNetwork2( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 128, 0),
                new NetIpAddress(255, 255, 128, 0));
            Assert.AreEqual(true, netAddress.IsNetwork( ));
        }

        [TestMethod]
        public void IsNetwork3( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0));
            Assert.AreEqual(false, netAddress.IsNetwork( ));
        }

        [TestMethod]
        public void IsHost1( ) {
            var netAddress = new NetAddress(new NetIpAddress(124, 102, 11, 7));
            Assert.AreEqual(false, netAddress.IsHost( ));
        }

        [TestMethod]
        public void IsHost2( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 128, 0),
                new NetIpAddress(255, 255, 128, 0));
            Assert.AreEqual(false, netAddress.IsHost( ));
        }

        [TestMethod]
        public void IsHost3( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0));
            Assert.AreEqual(true, netAddress.IsHost( ));
        }

        [TestMethod]
        public void Contains( ) {
            var netAddressNet = new NetAddress(
                new NetIpAddress(124, 102, 128, 0),
                new NetIpAddress(255, 255, 128, 0));
            var netAddressHost = new NetAddress(
                new NetIpAddress(124, 102, 128, 7),
                new NetIpAddress(255, 255, 0, 0));

            Assert.AreEqual(true, netAddressNet.Contains(netAddressNet));
            Assert.AreEqual(true, netAddressNet.Contains(netAddressHost));
            Assert.AreEqual(false, netAddressHost.Contains(netAddressNet));
            Assert.AreEqual(true, netAddressHost.Contains(netAddressHost));
        }

        [TestMethod]
        public void GetNetwork1( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 255, 255));

            var network = netAddress.GetNetwork( );

            Assert.AreEqual(null, network);
        }

        [TestMethod]
        public void GetNetwork2( ) {
            var netAddress = new NetAddress(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(0, 0, 0, 0));
            var network = netAddress.GetNetwork( );

            Assert.AreEqual(netAddress.Netmask, network.Netmask);
            Assert.AreEqual(netAddress.Broadcast, network.Broadcast);
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, network.Address.bytes);
        }

        [TestMethod]
        public void GetNetwork3( ) {
            var netAddress = new NetAddress(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(255, 255, 254, 0));
            var network = netAddress.GetNetwork( );

            Assert.AreEqual(netAddress.Netmask, network.Netmask);
            Assert.AreEqual(netAddress.Broadcast, network.Broadcast);
            CollectionAssert.AreEqual(new byte[] { 124, 102, 10, 0 }, network.Address.bytes);
        }

        [TestMethod]
        public void GetNetwork4( ) {
            var netAddress = new NetAddress(
               new NetIpAddress(124, 101, 11, 7),
               new NetIpAddress(255, 254, 0, 0));
            var network = netAddress.GetNetwork( );

            Assert.AreEqual(netAddress.Netmask, network.Netmask);
            Assert.AreEqual(netAddress.Broadcast, network.Broadcast);
            CollectionAssert.AreEqual(new byte[] { 124, 100, 0, 0 }, network.Address.bytes);
        }

        [TestMethod]
        public void GetNetwork5( ) {
            var netAddress = new NetAddress(
                new NetIpAddress(125, 102, 11, 7),
                new NetIpAddress(254, 0, 0, 0));
            var network = netAddress.GetNetwork( );

            Assert.AreEqual(netAddress.Netmask, network.Netmask);
            Assert.AreEqual(netAddress.Broadcast, network.Broadcast);
            CollectionAssert.AreEqual(new byte[] { 124, 0, 0, 0 }, network.Address.bytes);
        }

        [TestMethod]
        public void Equals( ) {
            var netAddressA = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressB = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressC = new NetAddress(
                new NetIpAddress(124, 102, 1, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressD = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 254, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressE = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 100, 255, 255));

            var netAddressF = new NetAddress(
                new NetIpAddress(124, 102, 1, 7),
                new NetIpAddress(255, 254, 0, 0),
                new NetIpAddress(124, 102, 255, 255));

            var netAddressG = new NetAddress(
                new NetIpAddress(124, 102, 1, 7),
                new NetIpAddress(255, 255, 0, 0),
                new NetIpAddress(124, 100, 255, 255));

            var netAddressH = new NetAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 254, 0, 0),
                new NetIpAddress(124, 100, 255, 255));

            var netAddressI = new NetAddress(
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
            var result = NetAddress.NetmaskIsValid(new NetIpAddress(255, 255, 255, 255));
            Assert.AreEqual(true, result);

            result = NetAddress.NetmaskIsValid(new NetIpAddress(0, 0, 0, 0));
            Assert.AreEqual(true, result);

            result = NetAddress.NetmaskIsValid(new NetIpAddress(255, 255, 254, 0));
            Assert.AreEqual(true, result);

            result = NetAddress.NetmaskIsValid(new NetIpAddress(255, 254, 0, 0));
            Assert.AreEqual(true, result);

            result = NetAddress.NetmaskIsValid(new NetIpAddress(254, 0, 0, 0));
            Assert.AreEqual(true, result);

            result = NetAddress.NetmaskIsValid(new NetIpAddress(0, 0, 0, 0));
            Assert.AreEqual(true, result);

            result = NetAddress.NetmaskIsValid(new NetIpAddress(255, 255, 254, 255));
            Assert.AreEqual(false, result);

            result = NetAddress.NetmaskIsValid(new NetIpAddress(253, 0, 0, 0));
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ComputeBroadcast( ) {
            var netAddress = NetAddress.ComputeBroadcast(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 255, 255));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.bytes);

            netAddress = NetAddress.ComputeBroadcast(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(0, 0, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, netAddress.bytes);

            netAddress = NetAddress.ComputeBroadcast(
               new NetIpAddress(124, 102, 10, 7),
               new NetIpAddress(255, 255, 254, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 255 }, netAddress.bytes);

            netAddress = NetAddress.ComputeBroadcast(
               new NetIpAddress(124, 100, 11, 7),
               new NetIpAddress(255, 254, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 101, 255, 255 }, netAddress.bytes);

            netAddress = NetAddress.ComputeBroadcast(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(254, 0, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 125, 255, 255, 255 }, netAddress.bytes);
        }

        [TestMethod]
        public void ComputeNetworkAddress( ) {
            var netAddress = NetAddress.ComputeNetworkAddress(
                new NetIpAddress(124, 102, 11, 7),
                new NetIpAddress(255, 255, 255, 255));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 11, 7 }, netAddress.bytes);

            netAddress = NetAddress.ComputeNetworkAddress(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(0, 0, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, netAddress.bytes);

            netAddress = NetAddress.ComputeNetworkAddress(
               new NetIpAddress(124, 102, 11, 7),
               new NetIpAddress(255, 255, 254, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 102, 10, 0 }, netAddress.bytes);

            netAddress = NetAddress.ComputeNetworkAddress(
               new NetIpAddress(124, 101, 11, 7),
               new NetIpAddress(255, 254, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 100, 0, 0 }, netAddress.bytes);

            netAddress = NetAddress.ComputeNetworkAddress(
               new NetIpAddress(125, 102, 11, 7),
               new NetIpAddress(254, 0, 0, 0));

            CollectionAssert.AreEqual(new byte[] { 124, 0, 0, 0 }, netAddress.bytes);
        }

        #endregion

    }
}
