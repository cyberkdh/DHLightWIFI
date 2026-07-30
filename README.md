# DHLightWIFI (DHWifiClient.NET)

Windows Native WiFi (`wlanapi.dll`) client library for .NET. Scan nearby networks, connect using
Open/WEP/WPA/WPA2/WPA3-Personal or WPA2-Enterprise (PEAP-MSCHAPv2 / EAP-TLS), manage saved
profiles, and observe real-time connection events — all through a small, dependency-free API.

## Requirements

- Windows Vista or later (uses the Native Wifi API, `wlanapi.dll`)
- Target frameworks: `net46`, `net48`, `net6.0-windows`, `net8.0-windows`
- Platform:
  - The library itself can remain `AnyCPU`.
  - For `802.1X` (`PEAP-MSCHAPv2` / `EAP-TLS`) scenarios, the consuming executable should target an explicit architecture: `x86` or `x64` (not `AnyCPU`).
  - The WinForms sample defaults to `x64` and also provides an `x86` configuration.
  - This recommendation is based on real validation plus a matching Microsoft Q&A symptom report for `WlanSetProfileEapXmlUserData` with `AnyCPU` executables. The Learn API reference does not currently document this constraint explicitly.

## Install

```
dotnet add package DHWifiClient.NET
```

## Quick start

Recommended entry point for new code: `DHWifiClient2`

- Recommended pattern for new code: prefer the `...AndWait(...)` helpers over `WaitForConnectionResult(...)` when possible, because they subscribe before issuing the connect request and avoid notification timing races.
- `ScanAndGetAvailableNetworks(...)` is available as a convenience helper, but it still returns immediately after requesting the asynchronous scan. Use `ScanAndWait(...)`, `WaitForScanComplete(...)`, or the `Notification` event if you need a confirmed refreshed list.

### Basic connect

```csharp
using DHWifiClient.NET;

using (var client = new DHWifiClient2())
{
    client.ScanAndWait();

    var homeWifi = client.GetAvailableNetwork("MyHomeWifi", mergeDuplicateBssids: true);
    if (homeWifi == null)
    {
        return;
    }

    var result = client.ConnectAndWait(
        homeWifi,
        password: "MyPassword123",
        mergeDuplicateBssids: true);

    if (!result.IsSuccess)
    {
        throw new InvalidOperationException(result.Message);
    }
}
```

### Direct SSID-first connect

```csharp
using DHWifiClient.NET;

using (var client = new DHWifiClient2())
{
    var result = client.ConnectPersonalAndWait(
        "MyHomeWifi",
        "MyPassword123",
        millisecondsTimeout: 15000,
        mergeDuplicateBssids: true);
}
```

### Saved profile reconnect

```csharp
using DHWifiClient.NET;

using (var client = new DHWifiClient2())
{
    if (client.HasSavedProfile("OfficeWifi"))
    {
        var result = client.ConnectSavedProfileAndWait(
            "OfficeWifi",
            millisecondsTimeout: 15000,
            mergeDuplicateBssids: true);
    }
}
```

### Hidden network

```csharp
using DHWifiClient.NET;
using DHWifiClient.NET.module;

using (var client = new DHWifiClient2())
{
    var result = client.ConnectHiddenPersonalAndWait(
        "HiddenWifi",
        "MyPassword123",
        WifiPskProtocol.WPA2,
        WifiCipher.AES,
        millisecondsTimeout: 15000,
        mergeDuplicateBssids: true);
}
```

### Enterprise (802.1X)

```csharp
using DHWifiClient.NET;

using (var client = new DHWifiClient2())
{
    var result = client.ConnectEnterpriseAndWait(
        "CorpWifi",
        username: "user1",
        password: "password1",
        domain: "CONTOSO",
        millisecondsTimeout: 15000,
        mergeDuplicateBssids: true);
}
```

### EAP-TLS

```csharp
using DHWifiClient.NET;

using (var client = new DHWifiClient2())
{
    var result = client.ConnectEnterpriseEapTlsAndWait(
        "CorpWifi",
        clientCertThumbprint: "00112233445566778899AABBCCDDEEFF00112233",
        millisecondsTimeout: 15000,
        mergeDuplicateBssids: true);
}
```

### When you need lower-level control

- `ScanAndWait(...)`, `WaitForScanComplete(...)`
- `ConnectAndWait(...)`, `ConnectSavedProfileAndWait(...)`
- `WaitForConnectionResult(...)`
- `Notification` event

Classic low-level entry point:

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
- `GetAvailableNetworks()` / `GetAvailableNetwork(ssid)` / `GetCurrentConnection()` — inspect visible and current networks
- `GetSavedProfiles()` / `HasSavedProfile(name)` / `DeleteSavedProfile(name)` — manage saved profiles
- `GetRadioState()` / `SetRadioState(bool)` — query/toggle the software radio switch
- `ScanAndWait()` / `ConnectAndWait()` — wait-helper APIs for common scan/connect flows
- `ConnectOpenAndWait()` / `ConnectPersonalAndWait()` / `ConnectEnterpriseAndWait()` — direct wait-helper APIs for SSID-first flows
- `GetProfiles()` / `HasProfile()` / `DeleteProfile()` remain for backward compatibility, but new code should prefer the `SavedProfile`-named APIs for clearer intent
- `Notification` event — scan/connect/disconnect lifecycle notifications (raised on a native callback thread; marshal to the UI thread yourself before touching UI controls)

## Sample application

- `DHWifiClientSol\sample\DHWifiClientSample`
  - Classic sample that uses the original `DHWifiClient` entry point.
- `DHWifiClientSol\sample\DHWifiClient2Sample`
  - WinForms feature sample that uses the `DHWifiClient2` facade entry point and demonstrates scan, connect, saved-profile reconnect, hidden-network connect, Enterprise (`802.1X`) connect, radio toggle, and profile deletion.
- `DHWifiClientSol\sample\DHWifiClient2ConsoleSample`
  - Minimal console sample for `scan -> select -> connect -> disconnect` flow with the `DHWifiClient2` facade entry point.
- Manual verification checklist for the WinForms sample is maintained in `doc/discuss_v1.0.1/0021_20260730_205500_dhwificlient2sample_manual_checklist.md`.

### Console sample quick run

```powershell
dotnet run --project .\DHWifiClientSol\sample\DHWifiClient2ConsoleSample\DHWifiClient2ConsoleSample.csproj -c Debug -p:Platform=x64
```

- Optional first argument: Wi-Fi interface name

```powershell
dotnet run --project .\DHWifiClientSol\sample\DHWifiClient2ConsoleSample\DHWifiClient2ConsoleSample.csproj -c Debug -p:Platform=x64 -- "Wi-Fi"
```

## Sample platform notes

- The sample applications are Windows-only (`WinForms` / `Native Wifi`).
- On modern .NET targets (`net6.0-windows`, `net8.0-windows`), `CA1416` warnings can appear because the analyzers see `WinForms` control access and other Windows-only APIs.
- In this repository, those `CA1416` warnings in the sample applications do not indicate a portability goal regression; they reflect the intentional Windows-only design of the samples.
- `NETSDK1201` can appear after adding `RuntimeIdentifier` / `RuntimeIdentifiers` for explicit `x86` / `x64` sample builds. In this repository, that warning is kept as documentation-only because the samples are intended to remain framework-dependent, not self-contained.
- `NETSDK1138` can appear because `net6.0-windows` is out of support as of July 30, 2026. In this repository, that warning is also kept as documentation-only while `net6.0-windows` remains in the multi-target matrix for compatibility.

## License

MIT — see [LICENSE](LICENSE).
