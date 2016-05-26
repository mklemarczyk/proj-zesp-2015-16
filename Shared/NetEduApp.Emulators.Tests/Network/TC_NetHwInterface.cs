using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEduApp.Emulators.Network.Abstract;
using NetEduApp.Emulators.Tests.Network;
using SUTest = NetEduApp.Emulators.Network.NetHwInterface;

namespace NetEduApp.Emulators.Tests.Network {
    [TestClass]
    public class TC_NetHwInterface {

        #region Init

        INetDevice fakeDevice;

        [TestInitialize]
        public void Setup( ) {
            fakeDevice = new Emulators.Network.Abstract.Fakes.StubINetDevice( );
        }

        [TestCleanup]
        public void Cleanup( ) {
            fakeDevice = null;
        }

        #endregion

        #region Constructor

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDevice_NullName( ) {
            new SUTest(null, null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDevice_EmptyName( ) {
            new SUTest(null, string.Empty);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDevice_ValidName( ) {
            new SUTest(null, "eth0");
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ValidDevice_NullName( ) {
            new SUTest(fakeDevice, null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Constructor_ValidDevice_EmptyName( ) {
            new SUTest(fakeDevice, string.Empty);
        }

        [TestMethod]
        public void Constructor_ExistingIfaceName( ) {
            var iface = new SUTest(fakeDevice, "eth0");
            new SUTest(fakeDevice, "eth0");
        }

        [TestMethod]
        public void Constructor_Valid( ) {
            var iface = new SUTest(fakeDevice, "eth0");
            Assert.AreEqual(null, iface);
        }

        #endregion

        #region Methods

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Connect_Null( ) {
            var iface = new SUTest(fakeDevice, "eth1");
            iface.Connect(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Connect_TheSame( ) {
            var iface = new SUTest(fakeDevice, "eth1");
            iface.Connect(iface);
        }

        [TestMethod]
        public void Connect_Other( ) {
            var iface = new SUTest(fakeDevice, "eth1");
            var iface2 = new SUTest(fakeDevice, "eth2");
            iface.Connect(iface2);

            Assert.AreEqual(iface2, iface.GetOtherInterface( ));
            Assert.AreEqual(iface, iface2.GetOtherInterface( ));
        }

        [TestMethod]
        public void Connect_OtherFromOtherDevice_TheSameName( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            var otherFakeDevice = new Emulators.Network.Abstract.Fakes.StubINetDevice( );
            var iface2 = new SUTest(otherFakeDevice, "eth1");

            iface.Connect(iface2);

            Assert.AreEqual(iface2, iface.GetOtherInterface( ));
            Assert.AreEqual(iface, iface2.GetOtherInterface( ));
        }

        [TestMethod]
        public void Connect_OtherFromOtherDevice_OtherName( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            var otherFakeDevice = new Emulators.Network.Abstract.Fakes.StubINetDevice( );
            var iface2 = new SUTest(otherFakeDevice, "eth2");

            iface.Connect(iface2);

            Assert.AreEqual(iface2, iface.GetOtherInterface( ));
            Assert.AreEqual(iface, iface2.GetOtherInterface( ));
        }

        [TestMethod]
        public void Connect_Connected( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            var otherFakeDevice = new Emulators.Network.Abstract.Fakes.StubINetDevice( );
            var iface2 = new SUTest(otherFakeDevice, "eth1");

            iface.Connect(iface2);
            iface.Connect(iface2);
        }

        [TestMethod]
        public void Disconnect_NotConnected( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            iface.Disconnect( );
        }

        [TestMethod]
        public void Disconnect_Connected( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            #region Connect
            var otherFakeDevice = new Emulators.Network.Abstract.Fakes.StubINetDevice( );
            var iface2 = new SUTest(otherFakeDevice, "eth1");
            iface.Connect(iface2);
            #endregion

            iface.Disconnect( );

            Assert.AreEqual(null, iface.GetOtherInterface( ));
            Assert.AreEqual(null, iface2.GetOtherInterface( ));
        }

        [TestMethod]
        public void Connect_ReConnected( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            #region Disconnect
            var otherFakeDevice = new Emulators.Network.Abstract.Fakes.StubINetDevice( );
            var iface2 = new SUTest(otherFakeDevice, "eth1");
            iface.Connect(iface2);
            iface.Disconnect( );
            #endregion

            iface.Connect(iface2);

            Assert.AreEqual(iface2, iface.GetOtherInterface( ));
            Assert.AreEqual(iface, iface2.GetOtherInterface( ));
        }

        [TestMethod]
        public void Disconnect_ReConnected( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            #region ReConnect
            var otherFakeDevice = new Emulators.Network.Abstract.Fakes.StubINetDevice( );
            var iface2 = new SUTest(otherFakeDevice, "eth1");
            iface.Connect(iface2);
            iface.Disconnect( );
            iface.Connect(iface2);
            #endregion

            iface.Disconnect( );

            Assert.AreEqual(null, iface.GetOtherInterface( ));
            Assert.AreEqual(null, iface2.GetOtherInterface( ));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ReceiveData_Null( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            iface.ReceiveData(null);
        }

        [TestMethod]
        public void ReceiveData_WrongTarget( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            var received = false;
            var fakePacket = new Emulators.Network.Abstract.Fakes.StubINetPacket( );
            ((Emulators.Network.Abstract.Fakes.StubINetDevice)fakeDevice).ReceiveDataINetPacket = (INetPacket iNetPacket) => received = fakePacket == iNetPacket;

            var otherIface = new SUTest(fakeDevice, "eth2");
            fakePacket.DestinationInterfaceGet = ( ) => otherIface;
            
            iface.ReceiveData(fakePacket);

            Assert.IsFalse(received);
        }

        [TestMethod]
        public void ReceiveData_ValidTarget( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            var received = false;
            var fakePacket = new Emulators.Network.Abstract.Fakes.StubINetPacket( );
            ((Emulators.Network.Abstract.Fakes.StubINetDevice)fakeDevice).ReceiveDataINetPacket = (INetPacket iNetPacket) => received = fakePacket == iNetPacket;

            fakePacket.DestinationInterfaceGet = ( ) => iface;

            iface.ReceiveData(fakePacket);

            Assert.IsTrue(received);
        }

        [TestMethod]
        public void SendData( ) {
            var iface = new SUTest(fakeDevice, "eth1");

            var sended = false;
            var fakePacket = new Emulators.Network.Abstract.Fakes.StubINetPacket( );
            var otherIface = new Emulators.Network.Fakes.ShimNetHwInterface( );
            otherIface.ReceiveDataINetPacket = (INetPacket iNetPacket) => sended = fakePacket == iNetPacket;
            fakePacket.SourceInterfaceGet = ( ) => (INetHwInterface)otherIface;

            iface.SendData(fakePacket);

            Assert.IsTrue(sended);
        }

        #endregion

    }

    #region Helpers

    internal static class SUTestExtension {
        internal static object GetOtherInterface(this SUTest obj) {
            return typeof(SUTest).GetField("otherInterface").GetValue(obj);
        }
    }

    #endregion
}
