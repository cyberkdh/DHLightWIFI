//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiInterfaceHelperTests
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Text;
using DHWifiClient.NET.module;
using DHWifiClient.NET.win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DHWifiClient.NET.Tests
{
    [TestClass]
    public class WifiInterfaceHelperTests
    {
        private static Dot11Ssid MakeSsid(string value)
        {
            var arrBytes = new byte[32];
            var arrEncoded = Encoding.UTF8.GetBytes(value ?? string.Empty);
            Array.Copy(arrEncoded, arrBytes, arrEncoded.Length);
            return new Dot11Ssid { SsidBytes = arrBytes, SsidLength = (uint)arrEncoded.Length };
        }

        [TestMethod]
        public void DecodeSsid_NormalSsid_ReturnsDecodedString()
        {
            string strResult = WifiInterface.DecodeSsid(MakeSsid("MyHomeWifi"));
            Assert.AreEqual("MyHomeWifi", strResult);
        }

        [TestMethod]
        public void DecodeSsid_ZeroLength_ReturnsEmptyString()
        {
            string strResult = WifiInterface.DecodeSsid(MakeSsid(string.Empty));
            Assert.AreEqual(string.Empty, strResult);
        }

        [TestMethod]
        public void DecodeSsid_NullBytes_ReturnsEmptyString()
        {
            string strResult = WifiInterface.DecodeSsid(new Dot11Ssid { SsidBytes = null, SsidLength = 0 });
            Assert.AreEqual(string.Empty, strResult);
        }

        [TestMethod]
        public void DecodeSsid_LengthExceedsBuffer_ClampsToBufferSize()
        {
            var oSsid = MakeSsid("ABC");
            oSsid.SsidLength = 100;
            string strResult = WifiInterface.DecodeSsid(oSsid);
            Assert.AreEqual(32, strResult.Length);
        }

        [TestMethod]
        public void FormatBssid_SixBytes_ReturnsColonSeparatedUppercaseHex()
        {
            string strResult = WifiInterface.FormatBssid(new byte[] { 0x00, 0x1A, 0x2B, 0x3C, 0x4D, 0xFF });
            Assert.AreEqual("00:1A:2B:3C:4D:FF", strResult);
        }

        [TestMethod]
        public void FormatBssid_NullOrWrongLength_ReturnsNull()
        {
            Assert.IsNull(WifiInterface.FormatBssid(null));
            Assert.IsNull(WifiInterface.FormatBssid(new byte[] { 0x01, 0x02 }));
        }

        [TestMethod]
        public void ValidatePskPassphrase_LengthWithinRange_DoesNotThrow()
        {
            WifiInterface.ValidatePskPassphrase(new string('a', 8));
            WifiInterface.ValidatePskPassphrase(new string('a', 63));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidatePskPassphrase_TooShort_Throws()
        {
            WifiInterface.ValidatePskPassphrase(new string('a', 7));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidatePskPassphrase_TooLong_Throws()
        {
            WifiInterface.ValidatePskPassphrase(new string('a', 64));
        }
    }
}
