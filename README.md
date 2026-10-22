# DFINC

DFINC 웹, 모바일, 웹서비스 및 OTP 프로젝트를 함께 관리하는 저장소입니다.
기존 DFINCOTP 저장소의 커밋 이력을 이어서 사용합니다.

| 구성 | 솔루션 파일 |
| --- | --- |
| 웹 | `DeskPlusSolution.root/DeskPlusSolution/DeskPlusSolution.sln` |
| 모바일 | `DeskPlusMobileSolution(Deep).root/DeskPlusMobileSolution/DeskPlusMobileSolution.sln` |
| 웹서비스 | `DeskPlusSolutionWS.root/DeskPlusSolutionWS/DeskPlusWS.sln` |
| OTP | `OTP/DeskPlusOTP/DeskPlusOTP.sln` |

## 개발 환경

- Visual Studio의 ASP.NET 및 웹 개발 도구
- .NET Framework 4.8 개발 도구
- 실행 환경에 맞는 IIS 및 SQL Server 연결 설정

솔루션을 연 뒤 NuGet 패키지를 복원하고 필요한 프로젝트를 빌드합니다.
기존 프로젝트가 직접 참조하는 `Component` 폴더 등의 외부 DLL은 포함합니다.
웹 프로젝트가 `bin` 경로에서 직접 참조하는 `ComponentArt.Web.UI.dll`도 포함합니다.

## 로컬 설정

`Web.config`, `WebAppSettings.config`, `WebConnectionString.config`는 실제 설정 파일을 Git에서 함께 관리합니다.
웹, 웹서비스, 모바일, OTP의 `Web.config`와 웹·웹서비스·모바일의 `WebAppSettings.config`가 포함됩니다.
웹과 웹서비스의 `WebConnectionString.config`도 포함됩니다.

Google Calendar 설정은 다음 예제 파일을 `.example` 확장자를 뺀 이름으로
복사한 뒤 실제 환경에 맞게 설정합니다.

| 예제 파일 | 복사할 파일 이름 |
| --- | --- |
| `DeskPlusSolution.root/DeskPlusSolution/DeskPlusEIP/App_Data/PIM.xml.example` | `PIM.xml` |

Google Calendar 연동에 필요한 개인 인증서와 `*_secret.json` 서비스 계정 키는 별도로 준비합니다.

`.vs`, `bin`, `obj`, NuGet 복원 패키지, 사용자별 설정과 Visual SourceSafe 메타데이터는
버전 관리에서 제외합니다. OTP도 이 저장소에서 일반 소스 폴더로 함께 관리합니다.

모바일의 모든 메신저 알람에 대한 Teams 연동 설정은
[모바일 Teams 설정 안내](DeskPlusMobileSolution%28Deep%29.root/DeskPlusMobileSolution/Tools/TeamsChat-Setup.txt)를 참고합니다.

## DeskPlusSolution의 IIS 구성

`DeskPlusEIP`만 IIS 응용프로그램으로 등록하고 `Account`, `Approval`, `Board` 등
하위 웹 프로젝트는 같은 응용프로그램의 일반 폴더로 사용합니다. 각 하위 프로젝트의
응용프로그램 루트 URL은 `http://localhost/DeskPlusEIP`로 지정되어 있으며,
Debug와 Release 모두 상위 `DeskPlusEIP/bin` 폴더로 빌드합니다.

이미 하위 폴더가 IIS 응용프로그램으로 만들어졌다면 관리자 PowerShell에서 다음을 실행합니다.
`-Apply`를 생략하면 변경 대상만 확인할 수 있습니다.

```powershell
& '.\DeskPlusSolution.root\DeskPlusSolution\Tools\Restore-DeskPlusIisFolders.ps1' -Apply
```

이 스크립트는 현재 체크아웃의 `DeskPlusEIP`를 참조하는 사이트만 처리하고,
IIS 설정을 백업한 뒤 하위 응용프로그램 등록만 해제합니다.
실제 폴더, 상위 응용프로그램과 `Design`, `FileData` 연결은 유지합니다.

구성 방식은 [Microsoft의 IIS 하위 프로젝트 안내](https://devblogs.microsoft.com/dotnet/part-1-of-3-creating-sub-projects-in-iis-with-web-application-projects/)를 참고합니다.
