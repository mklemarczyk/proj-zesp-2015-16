using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetEduApp.Emulators.Network;
using NetEduApp.Emulators.Network.Abstract;
using SUTest = NetEduApp.Emulators.Network.NetMacAddress;

namespace NetEduApp.Emulators.Tests.Network {
	[TestClass]
	public class TC_NetMacAddress {

		#region Constructor

		[TestMethod]
		public void Constructor_Empty( ) {
			var ipAddress = new SUTest( );
			CollectionAssert.AreEqual(null, ipAddress.Bytes);
		}

		[TestMethod]
		public void Constructor_UintEmpty( ) {
			var ipAddress = new SUTest(ulong.MinValue);
			CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, ipAddress.Bytes);

			ipAddress = new SUTest(ulong.MaxValue);
			CollectionAssert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }, ipAddress.Bytes);

			ipAddress = new SUTest(4286595104u);
			CollectionAssert.AreEqual(new byte[] { 0, 0, 0, 0, 255, 128, 64, 32 }, ipAddress.Bytes);
		}

		[TestMethod]
		public void Constructor_ByteArray( ) {
			var ipAddress = new SUTest(new byte[] { 247, 30, 63, 161, 142, 231, 21, 32 });
			CollectionAssert.AreEqual(new byte[] { 247, 30, 63, 161, 142, 231, 21, 32 }, ipAddress.Bytes);
		}

		[TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Constructor_ByteArray_LessParams( ) {
			new SUTest(new byte[] { 247, 30, 63 });
		}

		[TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Constructor_ByteArray_MoreParams( ) {
			new SUTest(new byte[] { 247, 30, 63, 161, 142, 231, 21, 32, 121 });
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
			Assert.AreEqual(ulong.MinValue, ipAddress.GetUlongRepresentation( ));
		}

		[TestMethod]
		public void MaxAddress( ) {
			var ipAddress = SUTest.MaxAddress;
			Assert.AreEqual(ulong.MaxValue, ipAddress.GetUlongRepresentation( ));
		}

		#endregion

		#region Methods

		[TestMethod]
		public void GetUlongRepresentation_FromUint( ) {
			var ipAddress = new SUTest(uint.MinValue);
			Assert.AreEqual(uint.MinValue, ipAddress.GetUlongRepresentation( ));

			ipAddress = new SUTest(uint.MaxValue);
			Assert.AreEqual(uint.MaxValue, ipAddress.GetUlongRepresentation( ));

			ipAddress = new SUTest(4286595104u);
			Assert.AreEqual(4286595104u, ipAddress.GetUlongRepresentation( ));
		}

		[TestMethod]
		public void GetUlongRepresentation_FromByteArray( ) {
			var ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
			Assert.AreEqual(uint.MinValue, ipAddress.GetUlongRepresentation( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 255, 255, 255 });
			Assert.AreEqual(uint.MaxValue, ipAddress.GetUlongRepresentation( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 64, 32 });
			Assert.AreEqual(4286595104u, ipAddress.GetUlongRepresentation( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 0, 128, 64, 32 });
			Assert.AreEqual(8405024u, ipAddress.GetUlongRepresentation( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 0, 64, 32 });
			Assert.AreEqual(4278206496u, ipAddress.GetUlongRepresentation( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 0, 32 });
			Assert.AreEqual(4286578720u, ipAddress.GetUlongRepresentation( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 64, 0 });
			Assert.AreEqual(4286595072u, ipAddress.GetUlongRepresentation( ));
		}

		[TestMethod]
		public void Equals( ) {
			var ipAddressByte = new SUTest(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
			var ipAddressUint = new SUTest(ulong.MinValue);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressByte));
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new SUTest(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new SUTest(ulong.MaxValue);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 64, 32 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new SUTest(4286595104u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new SUTest(new byte[] { 0, 0, 0, 0, 0, 128, 64, 32 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new SUTest(8405024u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new SUTest(new byte[] { 0, 0, 0, 0, 255, 0, 64, 32 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new SUTest(4278206496u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 0, 32 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new SUTest(4286578720u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));

			ipAddressByte = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 64, 0 });
			Assert.AreEqual(false, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(false, ipAddressUint.Equals(ipAddressByte));

			ipAddressUint = new SUTest(4286595072u);
			Assert.AreEqual(true, ipAddressByte.Equals(ipAddressUint));
			Assert.AreEqual(true, ipAddressUint.Equals(ipAddressByte));
		}

		[TestMethod]
		public void ToString_ValidInput( ) {
			var ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
			Assert.AreEqual("00-00-00-00-00-00-00-00", ipAddress.ToString( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 255, 255, 255 });
			Assert.AreEqual("00-00-00-00-FF-FF-FF-FF", ipAddress.ToString( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 64, 32 });
			Assert.AreEqual("00-00-00-00-FF-80-40-20", ipAddress.ToString( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 0, 128, 64, 32 });
			Assert.AreEqual("00-00-00-00-00-80-40-20", ipAddress.ToString( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 0, 64, 32 });
			Assert.AreEqual("00-00-00-00-FF-00-40-20", ipAddress.ToString( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 0, 32 });
			Assert.AreEqual("00-00-00-00-FF-80-00-20", ipAddress.ToString( ));

			ipAddress = new SUTest(new byte[] { 0, 0, 0, 0, 255, 128, 64, 0 });
			Assert.AreEqual("00-00-00-00-FF-80-40-00", ipAddress.ToString( ));
		}

		#endregion

	}
}
