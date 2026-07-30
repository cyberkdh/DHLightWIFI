# DHWifiClient2Sample manual checklist

## 목적

- `DHWifiClient2Sample` 이 보여줘야 하는 주요 기능(feature)들을 실제 실행(run) 기준으로 빠르게 점검하기 위한 체크리스트(checklist)이다.
- 이 문서는 자동 테스트(automated test) 대체가 아니라, 샘플 동작(sample behavior) 확인용이다.

## 사전 조건

- Windows 환경
- Wi-Fi 어댑터(adapter) 사용 가능
- 필요 시 테스트용 AP(access point) 준비
  - open 또는 WPA2/WPA3-Personal
  - hidden network
  - Enterprise (`802.1X`) 환경이 있으면 추가 확인
- `802.1X` 확인 시 샘플 실행 파일(executable)은 `x86` 또는 `x64` 빌드 사용

## 기본 흐름

### 1. 초기 진입

- `DHWifiClient2Sample` 실행
- 기대 결과(expected result)
  - `Status: Click 'Check WiFi' to begin.`
  - 주요 버튼(button) 비활성화(disabled)

### 2. Wi-Fi 어댑터 확인

- `Check WiFi` 클릭
- 기대 결과
  - 어댑터 목록(adapter list) 로드
  - `WiFi Adapter: Found (...)`
  - `Status: Ready`
  - 기능 버튼 활성화(enabled)

### 3. 어댑터 전환

- 콤보박스(combo box)에서 다른 어댑터 선택
- 기대 결과
  - 현재 어댑터(current adapter) 전환
  - radio state 및 network list 갱신(refresh)

## 스캔 / 목록

### 4. 스캔

- `Scan` 클릭
- 기대 결과
  - `Status: Scanning...`
  - 완료 후 network list 갱신
  - 성공 시 `Status: Found N network(s)`
  - 실패 시 `Status: Scan failed`

### 5. 중복 항목 병합 옵션

- `Merge duplicate entries (same BSSID)` 체크/해제
- 기대 결과
  - 목록(row) 구성이 변경될 수 있음
  - 기능 오류 없이 즉시 refresh

### 6. 연결 상태 표시

- 이미 연결된 AP가 있는 상태에서 목록 확인
- 기대 결과
  - 해당 row 의 상태(status)에 `Connected` 표시
  - 저장 프로필(saved profile)이 있으면 `Profile saved` 표시 가능

## 연결 / 해제

### 7. 일반 연결

- visible network 선택 후 `Connect`
- 보안 네트워크(security-enabled network)면 비밀번호(password) 입력
- 기대 결과
  - `Status: Connecting to ...`
  - 성공 시 `Status: Connected to ...`
  - 목록 갱신 후 connected 상태 반영

### 8. 더블클릭 연결

- 목록 row 더블클릭
- 기대 결과
  - `Connect` 버튼과 동일한 흐름으로 연결 시도

### 9. 저장 프로필 재연결

- 저장 프로필(saved profile)이 있는 네트워크 선택 후 `Reconnect (Saved Profile)`
- 기대 결과
  - `Status: Connecting to ... (saved profile)...`
  - 성공 시 connected 상태 반영

### 10. 연결 해제

- `Disconnect` 클릭
- 기대 결과
  - `Status: Disconnected`
  - 목록 갱신 후 connected 표시 제거

## 숨김 네트워크 / Enterprise

### 11. Hidden network 연결

- `Hidden Network...` 클릭
- SSID / 보안 타입(security type) / 비밀번호 입력
- 기대 결과
  - open / WEP / WPA / WPA2 hidden network 흐름 동작
  - 성공 시 connected 상태 반영

### 12. Enterprise (802.1X) 연결

- `Connect Enterprise...` 클릭
- PEAP-MSCHAPv2 또는 EAP-TLS 정보 입력
- 기대 결과
  - 성공 시 connected 상태 반영
  - 실패 시 적절한 오류 메시지(message) 또는 실패 상태 표시

## 프로필 / 라디오

### 13. 프로필 삭제

- 저장 프로필(saved profile)이 있는 네트워크 선택 후 `Delete Profile`
- 기대 결과
  - 삭제 성공 메시지 대신 상태(status) 갱신
  - `Status: Deleted profile for ...`
  - 목록 refresh 후 profile saved 표시 제거 가능

### 14. 인증 실패 시 프로필 삭제 옵션

- `Delete profile on auth failure` 체크
- 의도적으로 잘못된 자격 증명(credentials)으로 연결 시도
- 기대 결과
  - 실패 시 상태(status)에 삭제 반영 가능
  - profile 삭제 후 목록 refresh

### 15. Radio ON/OFF

- `ON/OFF` 클릭
- 기대 결과
  - radio state label 변경
  - 목록 refresh
  - 환경에 따라 스캔 결과가 달라질 수 있음

## 로그 / 예외

### 16. 로그 출력

- 주요 동작 후 log textbox 확인
- 기대 결과
  - scan / connect / failure / delete profile 등 이벤트 기록

### 17. 파일 로그 옵션

- `File logging` 체크/해제
- 기대 결과
  - 로그 디렉터리(log directory) 출력 여부 변경

## 메모

- `NETSDK1138`, `NETSDK1201`, `CA1416` 는 현재 저장소 정책상 문서화 대상(documentation-only) 경고이며, 샘플 기능 자체의 실패를 뜻하지 않는다.
- `802.1X` 는 `AnyCPU` 실행보다 `x86` / `x64` 실행 파일을 권장한다.
