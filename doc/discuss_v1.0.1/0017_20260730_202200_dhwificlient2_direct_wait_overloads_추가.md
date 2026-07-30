# DHWifiClient2 direct wait overloads 추가

## 배경

- 앞 단계에서 `ConnectAndWait(WifiNetwork ...)` 와 `ConnectSavedProfileAndWait(...)` 를 추가했지만,
  실제 사용 코드에서는 여전히 `SSID` 중심 호출이 더 흔하다.
- 따라서 `DHWifiClient2` 의 "쉬운 진입점(easy entry point)" 성격을 더 살리기 위해
  주요 `Connect...` 계열에 대응하는 `...AndWait(...)` 오버로드(overload)를 채운다.

## 이번 추가 API

- `ConnectOpenAndWait(...)`
- `ConnectPersonalAndWait(...)`
- `ConnectWepAndWait(...)`
- `ConnectHiddenOpenAndWait(...)`
- `ConnectHiddenPersonalAndWait(...)`
- `ConnectHiddenWepAndWait(...)`
- `ConnectEnterpriseAndWait(...)`
- `ConnectEnterpriseEapTlsAndWait(...)`

## 의도

- 새 코드(new code)에서는 `Notification` 이벤트(event) 직접 처리 없이도
  대부분의 연결(connect) 시나리오를 한 호출(call)로 마무리할 수 있게 한다.
- 기존 `Connect...` 메서드(method)는 그대로 유지해 이전 버전 호환성(backward compatibility)을 보존한다.
