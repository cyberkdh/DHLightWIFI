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
        public static string CreateOpen(string ssid, bool isHidden = false)
        {
            return
$@"<?xml version=""1.0""?>
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <name>{SecurityElement.Escape(ssid)}</name>
    <SSIDConfig>
        <SSID>
            <name>{SecurityElement.Escape(ssid)}</name>
        </SSID>
        <nonBroadcast>{(isHidden ? "true" : "false")}</nonBroadcast>
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

        public static string CreateWpa2Psk(string ssid, string passphrase, bool isHidden = false)
        {
            return CreatePsk(ssid, passphrase, "WPA2PSK", "AES", isHidden);
        }

        /// <summary>
        /// Creates a WPA/WPA2-Personal (PSK) profile with an explicit authentication/encryption combination.
        /// <paramref name="authentication"/> must be "WPAPSK" or "WPA2PSK"; <paramref name="encryption"/> must be "AES" or "TKIP".
        /// </summary>
        public static string CreatePsk(string ssid, string passphrase, string authentication, string encryption, bool isHidden = false)
        {
            return
$@"<?xml version=""1.0""?>
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <name>{SecurityElement.Escape(ssid)}</name>
    <SSIDConfig>
        <SSID>
            <name>{SecurityElement.Escape(ssid)}</name>
        </SSID>
        <nonBroadcast>{(isHidden ? "true" : "false")}</nonBroadcast>
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
        public static string CreateWep(string ssid, string keyMaterialHex, string authentication, int keyIndex, bool isHidden = false)
        {
            return
$@"<?xml version=""1.0""?>
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <name>{SecurityElement.Escape(ssid)}</name>
    <SSIDConfig>
        <SSID>
            <name>{SecurityElement.Escape(ssid)}</name>
        </SSID>
        <nonBroadcast>{(isHidden ? "true" : "false")}</nonBroadcast>
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
        /// <param name="ssid">Target network SSID.</param>
        /// <param name="serverNames">Optional semicolon-separated list of expected RADIUS server certificate subject names (ServerNames element). Empty means no restriction.</param>
        /// <param name="trustedRootCaThumbprint">Optional SHA1 thumbprint (hex, no separators) of the trusted Root CA certificate that issued the RADIUS server certificate. When set, Windows can validate the server certificate without prompting the user.</param>
        /// <param name="disableUserPromptForServerValidation">When true, Windows will not show the "Continue connecting?" server-certificate-trust prompt. Only set this to true when <paramref name="trustedRootCaThumbprint"/> is also provided, otherwise the connection has no way to establish trust.</param>
        public static string CreateWpa2EnterprisePeap(string ssid, string serverNames = "", string trustedRootCaThumbprint = null, bool disableUserPromptForServerValidation = false)
        {
            string strServerValidationBlock =
$@"                                    <ServerValidation>
                                        <DisableUserPromptForServerValidation>{(disableUserPromptForServerValidation ? "true" : "false")}</DisableUserPromptForServerValidation>
                                        <ServerNames>{SecurityElement.Escape(serverNames ?? string.Empty)}</ServerNames>";

            if (!string.IsNullOrEmpty(trustedRootCaThumbprint))
            {
                strServerValidationBlock += $@"
                                        <TrustedRootCA>{trustedRootCaThumbprint}</TrustedRootCA>";
            }

            strServerValidationBlock += @"
                                    </ServerValidation>";

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
{strServerValidationBlock}
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
                                    <PeapExtensions></PeapExtensions>
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
        /// Creates a WPA2-Enterprise (802.1X) profile using EAP-TLS (EAP type 13), authenticating with a
        /// client certificate instead of a username/password. The client certificate is selected from the
        /// Windows certificate store, either automatically (SimpleCertSelection) or pinned to a specific
        /// certificate via <see cref="CreateEapTlsUserData"/> and WlanSetProfileEapXmlUserData.
        /// </summary>
        /// <param name="ssid">Target network SSID.</param>
        /// <param name="serverNames">Optional semicolon-separated list of expected RADIUS server certificate subject names (ServerNames element). Empty means no restriction.</param>
        /// <param name="trustedRootCaThumbprint">Optional SHA1 thumbprint (hex, no separators) of the trusted Root CA certificate that issued the RADIUS server certificate. When set, Windows can validate the server certificate without prompting the user.</param>
        /// <param name="disableUserPromptForServerValidation">When true, Windows will not show the "Continue connecting?" server-certificate-trust prompt. Only set this to true when <paramref name="trustedRootCaThumbprint"/> is also provided, otherwise the connection has no way to establish trust.</param>
        public static string CreateWpa2EnterpriseEapTls(string ssid, string serverNames = "", string trustedRootCaThumbprint = null, bool disableUserPromptForServerValidation = false)
        {
            string strServerValidationBlock =
$@"                                    <ServerValidation>
                                        <DisableUserPromptForServerValidation>{(disableUserPromptForServerValidation ? "true" : "false")}</DisableUserPromptForServerValidation>
                                        <ServerNames>{SecurityElement.Escape(serverNames ?? string.Empty)}</ServerNames>";

            if (!string.IsNullOrEmpty(trustedRootCaThumbprint))
            {
                strServerValidationBlock += $@"
                                        <TrustedRootCA>{trustedRootCaThumbprint}</TrustedRootCA>";
            }

            strServerValidationBlock += @"
                                    </ServerValidation>";

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
                <authMode>user</authMode>
                <EAPConfig>
                    <EapHostConfig xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
                        <EapMethod>
                            <Type xmlns=""http://www.microsoft.com/provisioning/EapCommon"">13</Type>
                            <VendorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorId>
                            <VendorType xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorType>
                            <AuthorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</AuthorId>
                        </EapMethod>
                        <Config xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
                            <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                                <Type>13</Type>
                                <EapType xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV1"">
                                    <CredentialsSource>
                                        <CertificateStore>
                                            <SimpleCertSelection>true</SimpleCertSelection>
                                        </CertificateStore>
                                    </CredentialsSource>
{strServerValidationBlock}
                                    <DifferentUsername>false</DifferentUsername>
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
        /// Creates the EapHostUserCredentials XML for EAP-TLS (EAP type 13), pinning a specific client
        /// certificate by its SHA1 thumbprint (hex, no separators), consumed by WlanSetProfileEapXmlUserData.
        /// When this is not applied, Windows falls back to automatic certificate selection
        /// (SimpleCertSelection in the profile) or an interactive certificate chooser.
        /// </summary>
        public static string CreateEapTlsUserData(string clientCertThumbprint)
        {
            return
$@"<?xml version=""1.0""?>
<EapHostUserCredentials xmlns=""http://www.microsoft.com/provisioning/EapHostUserCredentials"" xmlns:eapCommon=""http://www.microsoft.com/provisioning/EapCommon"" xmlns:baseEap=""http://www.microsoft.com/provisioning/BaseEapMethodUserCredentials"">
    <EapMethod>
        <eapCommon:Type>13</eapCommon:Type>
        <eapCommon:AuthorId>0</eapCommon:AuthorId>
    </EapMethod>
    <Credentials xmlns:eapUser=""http://www.microsoft.com/provisioning/EapUserPropertiesV1"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:baseEap=""http://www.microsoft.com/provisioning/BaseEapUserPropertiesV1"" xmlns:eapTls=""http://www.microsoft.com/provisioning/EapTlsUserPropertiesV1"">
        <baseEap:Eap>
            <baseEap:Type>13</baseEap:Type>
            <eapTls:EapType>
                <eapTls:UserCert>{clientCertThumbprint}</eapTls:UserCert>
            </eapTls:EapType>
        </baseEap:Eap>
    </Credentials>
</EapHostUserCredentials>";
        }

        /// <summary>
        /// Creates the EapHostUserCredentials XML for PEAP-MSCHAPv2 (username/password, optional logon domain),
        /// consumed by WlanSetProfileEapXmlUserData. Never persisted or logged by this library.
        /// </summary>
        public static string CreatePeapMsChapV2UserData(string username, string password, string domain)
        {
            return
$@"<?xml version=""1.0""?>
<EapHostUserCredentials xmlns=""http://www.microsoft.com/provisioning/EapHostUserCredentials"" xmlns:eapCommon=""http://www.microsoft.com/provisioning/EapCommon"" xmlns:baseEap=""http://www.microsoft.com/provisioning/BaseEapMethodUserCredentials"">
    <EapMethod>
        <eapCommon:Type>25</eapCommon:Type>
        <eapCommon:AuthorId>0</eapCommon:AuthorId>
    </EapMethod>
    <Credentials xmlns:eapUser=""http://www.microsoft.com/provisioning/EapUserPropertiesV1"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:baseEap=""http://www.microsoft.com/provisioning/BaseEapUserPropertiesV1"" xmlns:MsPeap=""http://www.microsoft.com/provisioning/MsPeapUserPropertiesV1"" xmlns:MsChapV2=""http://www.microsoft.com/provisioning/MsChapV2UserPropertiesV1"">
        <baseEap:Eap>
            <baseEap:Type>25</baseEap:Type>
            <MsPeap:EapType>
                <MsPeap:RoutingIdentity>{SecurityElement.Escape(username)}</MsPeap:RoutingIdentity>
                <baseEap:Eap>
                    <baseEap:Type>26</baseEap:Type>
                    <MsChapV2:EapType>
                        <MsChapV2:Username>{SecurityElement.Escape(username)}</MsChapV2:Username>
                        <MsChapV2:Password>{SecurityElement.Escape(password)}</MsChapV2:Password>
                        <MsChapV2:LogonDomain>{SecurityElement.Escape(domain ?? string.Empty)}</MsChapV2:LogonDomain>
                    </MsChapV2:EapType>
                </baseEap:Eap>
            </MsPeap:EapType>
        </baseEap:Eap>
    </Credentials>
</EapHostUserCredentials>";
        }
    }
}
