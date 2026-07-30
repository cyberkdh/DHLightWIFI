# DHWifiClient2 wait helper API 추가

## 배경

- `DHWifiClient2` 는 사용성이 좋아졌지만, `Scan()` 과 `Connect(...)` 는 본질적으로 비동기(async) 요청이라 호출자마다 직접 `Notification` 이벤트(event)와 대기(wait) 코드를 작성해야 했다.
- 특히 연결(connect) 직후 결과를 기다릴 때는 이벤트 구독(subscription)과 요청(request) 순서에 따라 경쟁 조건(race condition)이 생길 수 있다.

## 이번 추가 범위

- `WifiWaitStatus`
  - `Success`
  - `Failed`
  - `TimedOut`
- `WifiConnectionResult`
  - 최종 상태(status)
  - 종료 알림(notification type)
  - 기대 SSID(expected SSID)
  - 확인된 연결 네트워크(connected network)
  - 사용자 메시지(message)
- `DHWifiClient2.ScanAndWait(...)`
- `DHWifiClient2.WaitForScanComplete(...)`
- `DHWifiClient2.ConnectAndWait(...)`
- `DHWifiClient2.ConnectSavedProfileAndWait(...)`
- `DHWifiClient2.WaitForConnectionResult(...)`

## 의도

- 새로운 코드(new code)는 `Notification` 직접 처리 없이도 기본적인 scan/connect 결과를 바로 다룰 수 있게 한다.
- 기존 API 호환성(backward compatibility)은 유지한다.
- `WaitForConnectionResult(...)` 는 분리 호출 시 여전히 호출 타이밍(timing)에 민감할 수 있으므로, 실사용에서는 `ConnectAndWait(...)` 계열을 우선 권장한다.
