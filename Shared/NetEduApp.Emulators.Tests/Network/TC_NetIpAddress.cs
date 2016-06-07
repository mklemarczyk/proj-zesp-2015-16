using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEduApp.Emulators.Network;
using NetEduApp.Emulators.Network.Abstract;
using SUTest = NetEduApp.Emulators.Network.NetIpAddress;

namespace NetEduApp.Emulators.Tests.Network {
    [TestClass]
    public class TC_NetIpAddress {

        #region Constructor

        [TestMethod]
        public void Constructor_Empty( ) {
            var ipAddress = new SUTest( );
            CollectionAssert.AreEqual(null, ipAddress.Bytes);
        }

        [TestMethod]
        public void Constructor_UintEmpty( ) {
            var ipAddress = new SUTest(uint.MinValue);
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, ipAddress.Bytes);

            ipAddress = new SUTest(uint.MaxValue);
            CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, ipAddress.Bytes);

            ipAddress = new SUTest(4286595104u);
            CollectionAssert.AreEqual(new byte[] { 255, 128, 64, 32 }, ipAddress.Bytes);
        }

        [TestMethod]
        public void Constructor_ByteArray( ) {
            var ipAddress = new SUTest(new byte[] { 247, 30, 63, 161 });
            CollectionAssert.AreEqual(new byte[] { 247, 30, 63, 161 }, ipAddress.Bytes);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_ByteArray_LessParams( ) {
            new SUTest(new byte[] { 247, 30, 63 });
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_ByteArray_MoreParams( ) {
            new SUTest(new byte[] { 247, 30, 63, 161, 142 });
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ByteArray_Null( ) {
            new SUTest(null);
        }

        #endregion

        #region Static properties

        [TestMethod]
        public void ZeroAddress( ) {
            var ipAddress = SUTest.Zero;
            Assert.AreEqual(uint.MinValue, ipAddress.GetUintRepresentation( ));
        }

        [TestMethod]
        public void MaxAddress( ) {
            var ipAddress = SUTest.MaxAddress;
            Assert.AreEqual(uint.MaxValue, ipAddress.GetUintRepresentation( ));
        }

        #endregion

        #region Methods

        [TestMethod]
        public void GetUintRepresentation_FromUint( ) {
            var ipAddress = new SUTest(uint.MinValue);
            Assert.AreEqual(uint.MinValue, ipAddress.GetUintRepresentation( ));

            ipAddress = new SUTest(uint.MaxValue);
            Assert.AreEqual(uint.MaxValue, ipAddress.GetUintRepresentation( ));

            ipAddress = new SUTest(4286595104u);
            Assert.AreEqual(4286595104u, ipAddress.GetUintRepresentation( ));
        }

        [TestMethod]
        public void GetUintRepresentation_FromByteArray( ) {
            var ipAddress = new SUTest(new byte[] { 0, 0, 0, 0 });
            Assert.AreEqual(uint.MinValue, ipAddress.GetUintRepresentation( ));

            ipAddress = new SUTest(new byte[] { 255, 255, 255, 255 });
            Assert.AreEqual(uint.MaxValue, ipAddress.GetUintRepresentation( ));

            ipAddress = new SUTest(new byte[] { 255, 128, 64, 32 });
            Assert.AreEqual(4286595104u, ipAddress.GetUintRepresentation( ));

            ipAddress = new SUTest(new byte[] { 0, 128, 64, 32 });
            Assert.AreEqual(8405024u, ipAddress.GetUintRepresentation( ));

            ipAddress = new SUTest(new byte[] { 255, 0, 64, 32 });
            Assert.AreEqual(4278206496u, ipAddress.GetUintRepresentation( ));

            ipAddress = new SUTest(new byte[] { 255, 128, 0, 32 });
            Assert.AreEqual(4286578720u, ipAddress.GetUintRepresentation( ));

            ipAddress = new SUTest(new byte[] { 255, 128, 64, 0 });
            Assert.AreEqual(4286595072u, ipAddress.GetUintRepresentation( ));
        }

        [TestMethod]
        public void Equals( ) {
            var ipAddressByte = new SUTest(new byte[] { 0, 0, 0, 0 });
            var ipAddressUint = new SUTest(uint.MinValue);
            Assert.AreEqual(true, ipAddressByte.Equals(ipAddressByte));
            Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

            ipAddressByte = new SUTest(new byte[] { 255, 255, 255, 255 });
            Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

            ipAddressUint = new SUTest(uint.MaxValue);
            Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

            ipAddressByte = new SUTest(new byte[] { 255, 128, 64, 32 });
            Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

            ipAddressUint = new SUTest(4286595104u);
            Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

            ipAddressByte = new SUTest(new byte[] { 0, 128, 64, 32 });
            Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

            ipAddressUint = new SUTest(8405024u);
            Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

            ipAddressByte = new SUTest(new byte[] { 255, 0, 64, 32 });
            Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

            ipAddressUint = new SUTest(4278206496u);
            Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

            ipAddressByte = new SUTest(new byte[] { 255, 128, 0, 32 });
            Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

            ipAddressUint = new SUTest(4286578720u);
            Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

            ipAddressByte = new SUTest(new byte[] { 255, 128, 64, 0 });
            Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

            ipAddressUint = new SUTest(4286595072u);
            Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
            Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));
        }

        [TestMethod]
        public void ToString_ValidInput( ) {
            var ipAddress = new SUTest(new byte[] { 0, 0, 0, 0 });
            Assert.AreEqual("0.0.0.0", ipAddress.ToString( ));

            ipAddress = new SUTest(new byte[] { 255, 255, 255, 255 });
            Assert.AreEqual("255.255.255.255", ipAddress.ToString( ));

            ipAddress = new SUTest(new byte[] { 255, 128, 64, 32 });
            Assert.AreEqual("255.128.64.32", ipAddress.ToString( ));

            ipAddress = new SUTest(new byte[] { 0, 128, 64, 32 });
            Assert.AreEqual("0.128.64.32", ipAddress.ToString( ));

            ipAddress = new SUTest(new byte[] { 255, 0, 64, 32 });
            Assert.AreEqual("255.0.64.32", ipAddress.ToString( ));

            ipAddress = new SUTest(new byte[] { 255, 128, 0, 32 });
            Assert.AreEqual("255.128.0.32", ipAddress.ToString( ));

            ipAddress = new SUTest(new byte[] { 255, 128, 64, 0 });
            Assert.AreEqual("255.128.64.0", ipAddress.ToString( ));
        }

        #endregion

        #region Static methods

        [TestMethod]
        public void TryParse_Valid( ) {
            SUTest ipAddress;
            bool result;

            result = SUTest.TryParse("0.0.0.0", out ipAddress);
            Assert.AreEqual(true, result);
            CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, ipAddress.Bytes);

            result = SUTest.TryParse("255.255.255.255", out ipAddress);
            Assert.AreEqual(true, result);
            CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, ipAddress.Bytes);

            result = SUTest.TryParse("0.128.64.32", out ipAddress);
            Assert.AreEqual(true, result);
            CollectionAssert.AreEqual(new byte[] { 0, 128, 64, 32 }, ipAddress.Bytes);

            result = SUTest.TryParse("255.128.64.32", out ipAddress);
            Assert.AreEqual(true, result);
            CollectionAssert.AreEqual(new byte[] { 255, 128, 64, 32 }, ipAddress.Bytes);

            result = SUTest.TryParse("255.0.64.32", out ipAddress);
            Assert.AreEqual(true, result);
            CollectionAssert.AreEqual(new byte[] { 255, 0, 64, 32 }, ipAddress.Bytes);

            result = SUTest.TryParse("255.128.0.32", out ipAddress);
            Assert.AreEqual(true, result);
            CollectionAssert.AreEqual(new byte[] { 255, 128, 0, 32 }, ipAddress.Bytes);

            result = SUTest.TryParse("255.128.64.0", out ipAddress);
            Assert.AreEqual(true, result);
            CollectionAssert.AreEqual(new byte[] { 255, 128, 64, 0 }, ipAddress.Bytes);
        }

        [TestMethod]
        public void TryParse_NullString( ) {
            SUTest ipAddress;
            bool result;

            result = SUTest.TryParse(null, out ipAddress);

            Assert.AreEqual(false, result);
            Assert.AreEqual(null, ipAddress.Bytes);
        }

        [TestMethod]
        public void TryParse_EmptyString( ) {
            SUTest ipAddress;
            bool result;

            result = SUTest.TryParse(string.Empty, out ipAddress);

            Assert.AreEqual(false, result);
            Assert.AreEqual(null, ipAddress.Bytes);
        }

        [TestMethod]
        public void TryParse_MissingByte( ) {
            SUTest ipAddress;
            bool result;

            result = SUTest.TryParse("121.64.147", out ipAddress);

            Assert.AreEqual(false, result);
            Assert.AreEqual(null, ipAddress.Bytes);
        }

        [TestMethod]
        public void TryParse_InvalidByte( ) {
            SUTest ipAddress;
            bool result;

            result = SUTest.TryParse("121.1255.64.147", out ipAddress);

            Assert.AreEqual(false, result);
            Assert.AreEqual(null, ipAddress.Bytes);
        }

        [TestMethod]
        public void TryParse_AdditionalByte( ) {
            SUTest ipAddress;
            bool result;

            result = SUTest.TryParse("121.64.147.21.231", out ipAddress);

            Assert.AreEqual(false, result);
            Assert.AreEqual(null, ipAddress.Bytes);
        }

        #endregion

    }
}
