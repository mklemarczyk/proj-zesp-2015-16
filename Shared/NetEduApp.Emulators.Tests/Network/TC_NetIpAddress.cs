using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEduApp.Emulators.Network;

namespace NetEduApp.Emulators.Tests.Network {
    [TestClass]
	public class TC_NetIpAddress {

		#region Constructor

		[TestMethod]
		public void Constructor_Empty( ) {
			var ipAddress = new Network.NetIpAddress( );
			CollectionAssert.AreEqual(ipAddress.bytes, null);
		}

		[TestMethod]
		public void Constructor_UintEmpty( ) {
			var ipAddress = new Network.NetIpAddress(uint.MinValue);
			CollectionAssert.AreEqual(ipAddress.bytes, new byte[] { 0, 0, 0, 0 });

			ipAddress = new Network.NetIpAddress(uint.MaxValue);
			CollectionAssert.AreEqual(ipAddress.bytes, new byte[] { 255, 255, 255, 255 });

			ipAddress = new Network.NetIpAddress(4286595104u);
			CollectionAssert.AreEqual(ipAddress.bytes, new byte[] { 255, 128, 64, 32 });
		}

		[TestMethod]
		public void Constructor_ByteArray( ) {
			var ipAddress = new Network.NetIpAddress(new byte[] { 247, 30, 63, 161 });
			CollectionAssert.AreEqual(ipAddress.bytes, new byte[] { 247, 30, 63, 161 });
		}

		[TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Constructor_ByteArray_LessParams( ) {
			new Network.NetIpAddress(new byte[] { 247, 30, 63 });
		}

		[TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Constructor_ByteArray_MoreParams( ) {
			new Network.NetIpAddress(new byte[] { 247, 30, 63, 161, 142 });
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ByteArray_Null( ) {
			new Network.NetIpAddress(null);
		}

		#endregion

		#region Static properties

		[TestMethod]
		public void ZeroAddress( ) {
			var ipAddress = NetIpAddress.Zero;
			Assert.AreEqual(uint.MinValue, ipAddress.GetUintRepresentation( ));
		}

		[TestMethod]
		public void MaxAddress( ) {
			var ipAddress = NetIpAddress.MaxAddress;
			Assert.AreEqual(uint.MaxValue, ipAddress.GetUintRepresentation( ));
		}

		#endregion

		#region Methods

		[TestMethod]
		public void GetUintRepresentation_FromUint( ) {
			var ipAddress = new Network.NetIpAddress(uint.MinValue);
			Assert.AreEqual(uint.MinValue, ipAddress.GetUintRepresentation( ));

			ipAddress = new Network.NetIpAddress(uint.MaxValue);
			Assert.AreEqual(uint.MaxValue, ipAddress.GetUintRepresentation( ));

			ipAddress = new Network.NetIpAddress(4286595104u);
			Assert.AreEqual(4286595104u, ipAddress.GetUintRepresentation( ));
		}

		[TestMethod]
		public void GetUintRepresentation_FromByteArray( ) {
			var ipAddress = new Network.NetIpAddress(new byte[] { 0, 0, 0, 0 });
			Assert.AreEqual(uint.MinValue, ipAddress.GetUintRepresentation( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 255, 255, 255 });
			Assert.AreEqual(uint.MaxValue, ipAddress.GetUintRepresentation( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 128, 64, 32 });
			Assert.AreEqual(4286595104u, ipAddress.GetUintRepresentation( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 0, 128, 64, 32 });
			Assert.AreEqual(8405024u, ipAddress.GetUintRepresentation( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 0, 64, 32 });
			Assert.AreEqual(4278206496u, ipAddress.GetUintRepresentation( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 128, 0, 32 });
			Assert.AreEqual(4286578720u, ipAddress.GetUintRepresentation( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 128, 64, 0 });
			Assert.AreEqual(4286595072u, ipAddress.GetUintRepresentation( ));
		}

		[TestMethod]
		public void Equals( ) {
			var ipAddressByte = new Network.NetIpAddress(new byte[] { 0, 0, 0, 0 });
			var ipAddressUint = new Network.NetIpAddress(uint.MinValue);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressByte));
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new Network.NetIpAddress(new byte[] { 255, 255, 255, 255 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new Network.NetIpAddress(uint.MaxValue);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new Network.NetIpAddress(new byte[] { 255, 128, 64, 32 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new Network.NetIpAddress(4286595104u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new Network.NetIpAddress(new byte[] { 0, 128, 64, 32 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new Network.NetIpAddress(8405024u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new Network.NetIpAddress(new byte[] { 255, 0, 64, 32 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new Network.NetIpAddress(4278206496u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new Network.NetIpAddress(new byte[] { 255, 128, 0, 32 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new Network.NetIpAddress(4286578720u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new Network.NetIpAddress(new byte[] { 255, 128, 64, 0 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new Network.NetIpAddress(4286595072u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));
		}

		[TestMethod]
		public void ToString_ValidInput( ) {
			var ipAddress = new Network.NetIpAddress(new byte[] { 0, 0, 0, 0 });
			Assert.AreEqual("0.0.0.0", ipAddress.ToString( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 255, 255, 255 });
			Assert.AreEqual("255.255.255.255", ipAddress.ToString( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 128, 64, 32 });
			Assert.AreEqual("255.128.64.32", ipAddress.ToString( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 0, 128, 64, 32 });
			Assert.AreEqual("0.128.64.32", ipAddress.ToString( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 0, 64, 32 });
			Assert.AreEqual("255.0.64.32", ipAddress.ToString( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 128, 0, 32 });
			Assert.AreEqual("255.128.0.32", ipAddress.ToString( ));

			ipAddress = new Network.NetIpAddress(new byte[] { 255, 128, 64, 0 });
			Assert.AreEqual("255.128.64.0", ipAddress.ToString( ));
		}

		#endregion

		#region Static methods

		[TestMethod]
		public void TryParse_Valid( ) {
			Network.NetIpAddress ipAddress;
			bool result;

			result = NetIpAddress.TryParse("0.0.0.0", out ipAddress);
			Assert.AreEqual(true, result);
			CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0 }, ipAddress.bytes);

			result = NetIpAddress.TryParse("255.255.255.255", out ipAddress);
			Assert.AreEqual(true, result);
			CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255 }, ipAddress.bytes);

			result = NetIpAddress.TryParse("0.128.64.32", out ipAddress);
			Assert.AreEqual(true, result);
			CollectionAssert.AreEqual(new byte[] { 0, 128, 64, 32 }, ipAddress.bytes);

			result = NetIpAddress.TryParse("255.128.64.32", out ipAddress);
			Assert.AreEqual(true, result);
			CollectionAssert.AreEqual(new byte[] { 255, 128, 64, 32 }, ipAddress.bytes);

			result = NetIpAddress.TryParse("255.0.64.32", out ipAddress);
			Assert.AreEqual(true, result);
			CollectionAssert.AreEqual(new byte[] { 255, 0, 64, 32 }, ipAddress.bytes);

			result = NetIpAddress.TryParse("255.128.0.32", out ipAddress);
			Assert.AreEqual(true, result);
			CollectionAssert.AreEqual(new byte[] { 255, 128, 0, 32 }, ipAddress.bytes);

			result = NetIpAddress.TryParse("255.128.64.0", out ipAddress);
			Assert.AreEqual(true, result);
			CollectionAssert.AreEqual(new byte[] { 255, 128, 64, 0 }, ipAddress.bytes);
		}

		[TestMethod]
		public void TryParse_NullString( ) {
			Network.NetIpAddress ipAddress;
			bool result;

			result = NetIpAddress.TryParse(null, out ipAddress);

			Assert.AreEqual(false, result);
			Assert.AreEqual(null, ipAddress.bytes);
		}

		[TestMethod]
		public void TryParse_EmptyString( ) {
			Network.NetIpAddress ipAddress;
			bool result;

			result = NetIpAddress.TryParse(string.Empty, out ipAddress);

			Assert.AreEqual(false, result);
			Assert.AreEqual(null, ipAddress.bytes);
		}

		[TestMethod]
		public void TryParse_MissingByte( ) {
			Network.NetIpAddress ipAddress;
			bool result;

			result = NetIpAddress.TryParse("121.64.147", out ipAddress);

			Assert.AreEqual(false, result);
			Assert.AreEqual(null, ipAddress.bytes);
		}

		[TestMethod]
		public void TryParse_InvalidByte( ) {
			Network.NetIpAddress ipAddress;
			bool result;

			result = NetIpAddress.TryParse("121.1255.64.147", out ipAddress);

			Assert.AreEqual(false, result);
			Assert.AreEqual(null, ipAddress.bytes);
		}

		[TestMethod]
		public void TryParse_AdditionalByte( ) {
			Network.NetIpAddress ipAddress;
			bool result;

			result = NetIpAddress.TryParse("121.64.147.21.231", out ipAddress);

			Assert.AreEqual(false, result);
			Assert.AreEqual(null, ipAddress.bytes);
		}

		#endregion

	}
}
