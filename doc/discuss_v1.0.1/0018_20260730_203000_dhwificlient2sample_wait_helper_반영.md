# DHWifiClient2Sample wait helper 반영

## 배경

- `DHWifiClient2` 에 `ScanAndWait(...)`, `ConnectAndWait(...)`, `ConnectEnterpriseAndWait(...)` 계열이 추가되었으므로,
  WinForms 샘플(sample)도 직접 이벤트(event)와 타이머(timer)에 크게 의존하지 않도록 정리할 필요가 있었다.

## 적용 내용

- `btnScan_Click`
  - 기존 `Scan()` + `Timer` fallback 조합을 제거
  - `Task.Run(() => m_oClient.ScanAndWait(...))` 패턴으로 변경
- `btnConnect_Click`
  - `Connect(...)` 대신 `ConnectAndWait(...)` 사용
- `btnReconnectSaved_Click`
  - `ConnectSavedProfile(...)` 대신 `ConnectSavedProfileAndWait(...)` 사용
- `btnHiddenNetwork_Click`
  - hidden network 보안 타입별 `...AndWait(...)` 사용
- `btnConnectEnterprise_Click`
  - `ConnectEnterpriseAndWait(...)`
  - `ConnectEnterpriseEapTlsAndWait(...)`

## 의도

- UI 구조(UI structure)는 유지한다.
- 샘플 코드(sample code)는 더 단순하게 만든다.
- 연결 성공/실패 판정(result handling)은 `WifiConnectionResult` 중심으로 정리한다.
- 외부에서 발생한 상태 변화(external state change)는 기존 `Notification` 이벤트로 계속 반영하되,
  샘플이 직접 요청한 작업 중에는 중복 UI 처리(duplicate UI handling)를 줄인다.
