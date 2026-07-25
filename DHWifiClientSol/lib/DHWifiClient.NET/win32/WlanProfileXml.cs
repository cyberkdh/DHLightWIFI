//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WlanProfileXml
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System.Security;

namespace DHWifiClient.NET.win32
{
    /// <summary>Generates WLAN profile XML.</summary>
    internal static class WlanProfileXml
    {
        public static string CreateOpen(string ssid)
        {
            return
$@"<?xml version=""1.0""?>
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <name>{SecurityElement.Escape(ssid)}</name>
    <SSIDConfig>
        <SSID>
            <name>{SecurityElement.Escape(ssid)}</name>
        </SSID>
    </SSIDConfig>
    <connectionType>ESS</connectionType>
    <connectionMode>manual</connectionMode>
    <MSM>
        <security>
            <authEncryption>
                <authentication>open</authentication>
                <encryption>none</encryption>
                <useOneX>false</useOneX>
            </authEncryption>
        </security>
    </MSM>
</WLANProfile>";
        }

        public static string CreateWpa2Psk(string ssid, string passphrase)
        {
            return CreatePsk(ssid, passphrase, "WPA2PSK", "AES");
        }

        /// <summary>
        /// Creates a WPA/WPA2-Personal (PSK) profile with an explicit authentication/encryption combination.
        /// <paramref name="authentication"/> must be "WPAPSK" or "WPA2PSK"; <paramref name="encryption"/> must be "AES" or "TKIP".
        /// </summary>
        public static string CreatePsk(string ssid, string passphrase, string authentication, string encryption)
        {
            return
$@"<?xml version=""1.0""?>
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <name>{SecurityElement.Escape(ssid)}</name>
    <SSIDConfig>
        <SSID>
            <name>{SecurityElement.Escape(ssid)}</name>
        </SSID>
    </SSIDConfig>
    <connectionType>ESS</connectionType>
    <connectionMode>manual</connectionMode>
    <MSM>
        <security>
            <authEncryption>
                <authentication>{authentication}</authentication>
                <encryption>{encryption}</encryption>
                <useOneX>false</useOneX>
            </authEncryption>
            <sharedKey>
                <keyType>passPhrase</keyType>
                <protected>false</protected>
                <keyMaterial>{SecurityElement.Escape(passphrase)}</keyMaterial>
            </sharedKey>
        </security>
    </MSM>
</WLANProfile>";
        }

        /// <summary>
        /// Creates a legacy WEP profile. <paramref name="keyMaterialHex"/> must already be a 10 or 26-digit hex string.
        /// WEP is deprecated and trivially breakable; use only for compatibility with legacy hardware.
        /// </summary>
        public static string CreateWep(string ssid, string keyMaterialHex, string authentication, int keyIndex)
        {
            return
$@"<?xml version=""1.0""?>
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <name>{SecurityElement.Escape(ssid)}</name>
    <SSIDConfig>
        <SSID>
            <name>{SecurityElement.Escape(ssid)}</name>
        </SSID>
    </SSIDConfig>
    <connectionType>ESS</connectionType>
    <connectionMode>manual</connectionMode>
    <MSM>
        <security>
            <authEncryption>
                <authentication>{authentication}</authentication>
                <encryption>WEP</encryption>
                <useOneX>false</useOneX>
            </authEncryption>
            <sharedKey>
                <keyType>networkKey</keyType>
                <protected>false</protected>
                <keyMaterial>{keyMaterialHex}</keyMaterial>
            </sharedKey>
            <keyIndex>{keyIndex}</keyIndex>
        </security>
    </MSM>
</WLANProfile>";
        }

        /// <summary>
        /// Creates a WPA2-Enterprise (802.1X) profile using PEAP-MSCHAPv2 (EAP type 25, inner EAP type 26).
        /// The server certificate is validated against the system trust store; no client certificate is required.
        /// Credentials are supplied separately via <see cref="CreatePeapMsChapV2UserData"/> and WlanSetProfileEapXmlUserData.
        /// </summary>
        public static string CreateWpa2EnterprisePeap(string ssid)
        {
            return
$@"<?xml version=""1.0""?>
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <name>{SecurityElement.Escape(ssid)}</name>
    <SSIDConfig>
        <SSID>
            <name>{SecurityElement.Escape(ssid)}</name>
        </SSID>
    </SSIDConfig>
    <connectionType>ESS</connectionType>
    <connectionMode>manual</connectionMode>
    <MSM>
        <security>
            <authEncryption>
                <authentication>WPA2</authentication>
                <encryption>AES</encryption>
                <useOneX>true</useOneX>
            </authEncryption>
            <OneX xmlns=""http://www.microsoft.com/networking/OneX/v1"">
                <cacheUserData>true</cacheUserData>
                <authMode>user</authMode>
                <EAPConfig>
                    <EapHostConfig xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
                        <EapMethod>
                            <Type xmlns=""http://www.microsoft.com/provisioning/EapCommon"">25</Type>
                            <VendorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorId>
                            <VendorType xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorType>
                            <AuthorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</AuthorId>
                        </EapMethod>
                        <Config xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
                            <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                                <Type>25</Type>
                                <EapType xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV1"">
                                    <ServerValidation>
                                        <DisableUserPromptForServerValidation>false</DisableUserPromptForServerValidation>
                                        <ServerNames></ServerNames>
                                    </ServerValidation>
                                    <FastReconnect>true</FastReconnect>
                                    <InnerEapOptional>false</InnerEapOptional>
                                    <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                                        <Type>26</Type>
                                        <EapType xmlns=""http://www.microsoft.com/provisioning/MsChapV2ConnectionPropertiesV1"">
                                            <UseWinLogonCredentials>false</UseWinLogonCredentials>
                                        </EapType>
                                    </Eap>
                                    <EnableQuarantineChecks>false</EnableQuarantineChecks>
                                    <RequireCryptoBinding>false</RequireCryptoBinding>
                                    <PeapExtensions>
                                        <PerformServerValidation xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2"">true</PerformServerValidation>
                                        <AcceptServerName xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2"">false</AcceptServerName>
                                    </PeapExtensions>
                                </EapType>
                            </Eap>
                        </Config>
                    </EapHostConfig>
                </EAPConfig>
            </OneX>
        </security>
    </MSM>
</WLANProfile>";
        }

        /// <summary>
        /// Creates the EapHostUserCredentials XML for PEAP-MSCHAPv2 (username/password, optional logon domain),
        /// consumed by WlanSetProfileEapXmlUserData. Never persisted or logged by this library.
        /// </summary>
        public static string CreatePeapMsChapV2UserData(string username, string password, string domain)
        {
            string strDomainElement = string.IsNullOrEmpty(domain)
                ? string.Empty
                : $"<LogonDomain>{SecurityElement.Escape(domain)}</LogonDomain>";

            return
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<EapHostUserCredentials xmlns=""http://www.microsoft.com/provisioning/EapHostUserCredentials"" xmlns:eapCommon=""http://www.microsoft.com/provisioning/EapCommon"" xmlns:baseEap=""http://www.microsoft.com/provisioning/BaseEapMethodUserCredentials"">
    <EapMethod>
        <eapCommon:Type>25</eapCommon:Type>
        <eapCommon:AuthorId>0</eapCommon:AuthorId>
    </EapMethod>
    <Credentials xmlns:eapUser=""http://www.microsoft.com/provisioning/EapUserPropertiesV1"" xmlns:baseEap=""http://www.microsoft.com/provisioning/BaseEapUserPropertiesV1"" xmlns=""http://www.microsoft.com/provisioning/MsPeapUserPropertiesV1"">
        <baseEap:Eap>
            <baseEap:Type>26</baseEap:Type>
            <baseEap:EapType xmlns:eapUser=""http://www.microsoft.com/provisioning/EapUserPropertiesV1"" xmlns=""http://www.microsoft.com/provisioning/MsChapV2UserPropertiesV1"">
                <Username>{SecurityElement.Escape(username)}</Username>
                <Password>{SecurityElement.Escape(password)}</Password>
                {strDomainElement}
            </baseEap:EapType>
        </baseEap:Eap>
    </Credentials>
</EapHostUserCredentials>";
        }
    }
}
