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
- Multi-targeting: `net46`, `net6.0-windows`, `net8.0-windows`.
- WinForms sample application demonstrating all supported connection flows.
