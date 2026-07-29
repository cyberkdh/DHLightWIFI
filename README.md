# DHLightWIFI (DHWifiClient.NET)

Windows Native WiFi (`wlanapi.dll`) client library for .NET. Scan nearby networks, connect using
Open/WEP/WPA/WPA2/WPA3-Personal or WPA2-Enterprise (PEAP-MSCHAPv2 / EAP-TLS), manage saved
profiles, and observe real-time connection events — all through a small, dependency-free API.

## Requirements

- Windows Vista or later (uses the Native Wifi API, `wlanapi.dll`)
- Target frameworks: `net46`, `net6.0-windows`, `net8.0-windows`
- Platform: `x64` (the sample app targets x64; `wlanapi.dll` marshaling has been validated on x64 only)

## Install

```
dotnet add package DHWifiClient.NET
```

## Quick start

```csharp
using DHWifiClient.NET;

using (var client = new DHWifiClient())
{
    var wifiInterface = client.GetInterfaces().First();

    wifiInterface.Scan();
    var networks = wifiInterface.GetAvailableNetworks();

    // Auto-detects Open/WEP/WPA/WPA2-Personal from the scan result.
    wifiInterface.Connect(networks.First(n => n.Ssid == "MyHomeWifi"), password: "MyPassword123");
}
```

## Supported authentication methods

| Method | API |
|---|---|
| Open | `Connect(ssid)` |
| WEP | `ConnectWep(ssid, wepKey, authentication, keyIndex)` |
| WPA/WPA2-Personal (PSK) | `Connect(ssid, password)` / `Connect(ssid, password, protocol, cipher)` |
| Saved profile reconnect | `ConnectSavedProfile(ssid)` |
| WPA2-Enterprise (PEAP-MSCHAPv2) | `ConnectEnterprise(ssid, username, password, domain)` |
| WPA2-Enterprise (EAP-TLS, client certificate) | `ConnectEnterpriseEapTls(ssid, clientCertThumbprint)` |
| Auto-detect from scan result | `Connect(WifiNetwork network, string password)` |

WPA3-SAE and ad-hoc (IBSS) networks are intentionally not supported; the API throws
`NotSupportedException` for these cases with an explanatory message.

## Other capabilities

- `GetInterfaces()` — enumerate WiFi adapters
- `GetProfiles()` / `DeleteProfile(name)` — manage saved profiles
- `GetRadioState()` / `SetRadioState(bool)` — query/toggle the software radio switch
- `Notification` event — scan/connect/disconnect lifecycle notifications (raised on a native
  callback thread; marshal to the UI thread yourself before touching UI controls)

## Sample application

A WinForms sample (`DHWifiClientSol\sample\DHWifiClientSample`) demonstrates scanning, connecting
(including Enterprise credential/certificate dialogs and hidden-network entry), and viewing saved
profiles.

## License

MIT — see [LICENSE](LICENSE).
