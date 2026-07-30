# Changelog

All notable changes to this project are documented in this file.
The format follows [Keep a Changelog](https://keepachangelog.com/), and this project adheres to
[Semantic Versioning](https://semver.org/).

## [1.0.0] - Unreleased

### Added
- Native WiFi (`wlanapi.dll`) client: interface enumeration, scanning, and available-network listing.
- Connect support for Open, WEP, WPA/WPA2/WPA3-Personal (PSK) networks.
- Connect support for WPA2-Enterprise via PEAP-MSCHAPv2 (username/password) and EAP-TLS (client certificate).
- Saved WLAN profile management: list, reconnect, delete.
- Radio state query/toggle (software switch).
- Real-time connection lifecycle notifications (scan/connect/disconnect events).
- Multi-targeting: `net46`, `net48`, `net6.0-windows`, `net8.0-windows`.
- `DHWifiClient2` facade entry point for simpler Wi-Fi usage from new code.
- `DHWifiClient2` wait-helper APIs: `ScanAndWait`, `WaitForScanComplete`, `ConnectAndWait`, `ConnectSavedProfileAndWait`, and `WaitForConnectionResult`.
- `DHWifiClient2` direct connection wait-helper overloads for Open, WPA/WPA2-Personal, WEP, hidden variants, PEAP-MSCHAPv2, and EAP-TLS flows.
- Split samples by role:
  - `DHWifiClientSample` keeps the legacy `DHWifiClient` usage for backward compatibility.
  - `DHWifiClient2Sample` is a minimal WinForms sample for the new `DHWifiClient2` facade.
  - `DHWifiClient2ConsoleSample` is a minimal console sample for quick Wi-Fi testing.
- Documentation note: for `802.1X` (`PEAP-MSCHAPv2` / `EAP-TLS`) scenarios, the consuming executable should target an explicit architecture (`x86` or `x64`) instead of `AnyCPU`. The library itself can remain `AnyCPU`; this guidance matches real validation and a corresponding Microsoft Q&A symptom report, even though the Learn API reference does not call out the constraint directly.
- Documentation note: `NETSDK1201` and `NETSDK1138` are currently tracked as documentation-only warnings while the compatibility target matrix remains unchanged.
