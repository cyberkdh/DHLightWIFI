//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WlanProfileXmlTests
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System.Xml.Linq;
using DHWifiClient.NET.win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DHWifiClient.NET.Tests
{
    [TestClass]
    public class WlanProfileXmlTests
    {
        private static readonly XNamespace Ns = "http://www.microsoft.com/networking/WLAN/profile/v1";

        [TestMethod]
        public void CreateOpen_NotHidden_SetsNonBroadcastFalseAndOpenAuth()
        {
            var oDoc = XDocument.Parse(WlanProfileXml.CreateOpen("MySsid", isHidden: false));
            var oRoot = oDoc.Element(Ns + "WLANProfile");

            Assert.AreEqual("false", oRoot.Element(Ns + "SSIDConfig").Element(Ns + "nonBroadcast").Value);
            var oAuthEncryption = oRoot.Element(Ns + "MSM").Element(Ns + "security").Element(Ns + "authEncryption");
            Assert.AreEqual("open", oAuthEncryption.Element(Ns + "authentication").Value);
            Assert.AreEqual("none", oAuthEncryption.Element(Ns + "encryption").Value);
        }

        [TestMethod]
        public void CreateOpen_Hidden_SetsNonBroadcastTrue()
        {
            var oDoc = XDocument.Parse(WlanProfileXml.CreateOpen("MySsid", isHidden: true));
            var oRoot = oDoc.Element(Ns + "WLANProfile");

            Assert.AreEqual("true", oRoot.Element(Ns + "SSIDConfig").Element(Ns + "nonBroadcast").Value);
        }

        [TestMethod]
        public void CreateWpa2Psk_UsesWpa2PskAndAes()
        {
            var oDoc = XDocument.Parse(WlanProfileXml.CreateWpa2Psk("MySsid", "passphrase1"));
            var oAuthEncryption = oDoc.Element(Ns + "WLANProfile").Element(Ns + "MSM").Element(Ns + "security").Element(Ns + "authEncryption");

            Assert.AreEqual("WPA2PSK", oAuthEncryption.Element(Ns + "authentication").Value);
            Assert.AreEqual("AES", oAuthEncryption.Element(Ns + "encryption").Value);
        }

        [TestMethod]
        public void CreatePsk_WpaTkip_UsesGivenAuthenticationAndEncryption()
        {
            var oDoc = XDocument.Parse(WlanProfileXml.CreatePsk("MySsid", "passphrase1", "WPAPSK", "TKIP"));
            var oSecurity = oDoc.Element(Ns + "WLANProfile").Element(Ns + "MSM").Element(Ns + "security");
            var oAuthEncryption = oSecurity.Element(Ns + "authEncryption");

            Assert.AreEqual("WPAPSK", oAuthEncryption.Element(Ns + "authentication").Value);
            Assert.AreEqual("TKIP", oAuthEncryption.Element(Ns + "encryption").Value);
            Assert.AreEqual("passphrase1", oSecurity.Element(Ns + "sharedKey").Element(Ns + "keyMaterial").Value);
        }

        [TestMethod]
        public void CreateWep_SetsWepAuthenticationEncryptionAndKeyIndex()
        {
            var oDoc = XDocument.Parse(WlanProfileXml.CreateWep("MySsid", "0102030405", "shared", 2, isHidden: true));
            var oRoot = oDoc.Element(Ns + "WLANProfile");
            var oSecurity = oRoot.Element(Ns + "MSM").Element(Ns + "security");

            Assert.AreEqual("true", oRoot.Element(Ns + "SSIDConfig").Element(Ns + "nonBroadcast").Value);
            Assert.AreEqual("shared", oSecurity.Element(Ns + "authEncryption").Element(Ns + "authentication").Value);
            Assert.AreEqual("WEP", oSecurity.Element(Ns + "authEncryption").Element(Ns + "encryption").Value);
            Assert.AreEqual("0102030405", oSecurity.Element(Ns + "sharedKey").Element(Ns + "keyMaterial").Value);
            Assert.AreEqual("2", oSecurity.Element(Ns + "keyIndex").Value);
        }
    }
}
