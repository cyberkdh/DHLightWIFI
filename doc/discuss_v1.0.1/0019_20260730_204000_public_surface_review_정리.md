# DHWifiClient2 public surface review 정리

## 검토 관점

- 이름 일관성(name consistency)
- 새 wait helper API 의 권장 사용 패턴(recommended usage pattern)
- 이전 버전 호환성(backward compatibility) 유지 여부

## 결론

- `DHWifiClient2` 의 메서드 이름(method name) 체계는 현재 상태로도 충분히 일관적이다.
- `Connect...` / `Connect...AndWait(...)` 쌍(pair)은 사용자가 이해하기 쉽다.
- `GetProfiles()` / `HasProfile()` / `DeleteProfile()` 는 약간 덜 명시적이지만,
  기존 사용자(existing user) 호환성 때문에 유지하는 편이 맞다.

## 이번 정리 방향

- `Obsolete` 는 이번 버전에서 넣지 않는다.
  - 이유: 경고(warning) 추가가 실제 사용자 경험(user experience)에 주는 영향이 아직 불필요하게 크다.
- 대신 문서 주석(XML documentation)과 `README` 에서 아래를 분명히 한다.
  - 새 코드(new code)는 `GetSavedProfiles()` / `HasSavedProfile()` / `DeleteSavedProfile()` 권장
  - 새 코드(new code)는 가능하면 `...AndWait(...)` 계열 권장
  - `WaitForConnectionResult(...)` 는 분리 호출 시 타이밍(timing) 민감성이 있다는 점 안내
