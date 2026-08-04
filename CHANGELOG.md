# Changelog

All notable changes to this project are documented in this file.
The format follows [Keep a Changelog](https://keepachangelog.com/), and this project adheres to
[Semantic Versioning](https://semver.org/).

## [1.0.1] - Unreleased

### User-facing improvements
- New recommended entry point: `DHWifiClient2` for simpler Wi-Fi workflows in new code.
- Easier scan/connect flow with wait-helper APIs so callers can handle completion and failure with less event plumbing.
- Direct SSID-first connect helpers for Open, WPA/WPA2-Personal, WEP, hidden-network, PEAP-MSCHAPv2, and EAP-TLS scenarios.
- Clearer saved-profile APIs: `GetSavedProfiles`, `HasSavedProfile`, and `DeleteSavedProfile`.
- Expanded sample coverage:
  - `DHWifiClientSample` keeps the legacy `DHWifiClient` usage for backward compatibility.
  - `DHWifiClient2Sample` demonstrates the new WinForms-centered workflow.
  - `DHWifiClient2ConsoleSample` provides a minimal console-based test path.

### Developer-facing API changes
- Added `DHWifiClient2` facade over the existing `DHWifiClient` / `WifiInterface` engine.
- Added wait-helper APIs:
  - `ScanAndWait`
  - `WaitForScanComplete`
  - `ConnectAndWait`
  - `ConnectSavedProfileAndWait`
  - `WaitForConnectionResult`
- Added direct wait-helper overloads:
  - `ConnectOpenAndWait`
  - `ConnectPersonalAndWait`
  - `ConnectWepAndWait`
  - Hidden-network `...AndWait` overloads
  - `ConnectEnterpriseAndWait`
  - `ConnectEnterpriseEapTlsAndWait`
- Added structured wait/connect result types to make connection outcomes easier to inspect in application code.
- Preserved the original `DHWifiClient` path for backward compatibility while steering new code toward `DHWifiClient2`.

### Packaging and compatibility
- Multi-targeting updated to `net46`, `net48`, `net6.0-windows`, and `net8.0-windows`.
- Package assemblies are now strong-name signed to improve compatibility with consumers that require signed dependencies.
- Sample executables are configured for explicit `x86` / `x64` builds.
- Documentation now calls out the practical `802.1X` guidance that consuming executables should target `x86` or `x64` instead of `AnyCPU`.
- `NETSDK1201` and `NETSDK1138` remain tracked as documentation-level warnings while the compatibility target matrix stays unchanged.

### NuGet release note draft
- Adds the new `DHWifiClient2` facade for easier Wi-Fi workflows in .NET applications.
- Adds wait-helper APIs for scan/connect completion handling and clearer connection result inspection.
- Adds dedicated WinForms and console samples for the new workflow.
- Expands target framework coverage with `net48` while retaining Windows-focused multi-target support.
- Strong-name signs the package assemblies for better compatibility with signed consumer projects.
