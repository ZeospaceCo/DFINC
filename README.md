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

실제 DB 연결 정보, 인증 설정과 개인 인증서는 Git에서 제외합니다. 처음 체크아웃한 환경에서는
각 폴더의 `*.config.example` 파일을 `.example` 확장자를 뺀 이름으로 복사한 뒤
실제 환경에 맞게 설정합니다.

| 예제 파일 | 복사할 파일 이름 |
| --- | --- |
| `DeskPlusSolution.root/DeskPlusSolution/DeskPlusEIP/WebConnectionString.config.example` | `WebConnectionString.config` |
| `DeskPlusSolution.root/DeskPlusSolution/DeskPlusEIP/Web.config.example` | `Web.config` |
| `DeskPlusSolution.root/DeskPlusSolution/DeskPlusEIP/WebAppSettings.config.example` | `WebAppSettings.config` |
| `DeskPlusSolutionWS.root/DeskPlusSolutionWS/DeskPlusWS/WebConnectionString.config.example` | `WebConnectionString.config` |
| `DeskPlusSolutionWS.root/DeskPlusSolutionWS/DeskPlusWS/Web.config.example` | `Web.config` |
| `DeskPlusSolutionWS.root/DeskPlusSolutionWS/DeskPlusWS/WebAppSettings.config.example` | `WebAppSettings.config` |
| `DeskPlusMobileSolution(Deep).root/DeskPlusMobileSolution/DeskPlusMobile/Web.config.example` | `Web.config` |
| `OTP/DeskPlusOTP/DeskPlusOTP/Web.config.example` | `Web.config` |
| `DeskPlusSolution.root/DeskPlusSolution/DeskPlusEIP/App_Data/PIM.xml.example` | `PIM.xml` |

Google Calendar 연동에 필요한 개인 인증서와 `*_secret.json` 서비스 계정 키는 별도로 준비합니다. 기존 개발 PC의
실제 설정 파일과 인증서는 로컬에 보존합니다.

`.vs`, `bin`, `obj`, NuGet 복원 패키지, 사용자별 설정과 Visual SourceSafe 메타데이터는
버전 관리에서 제외합니다. OTP도 이 저장소에서 일반 소스 폴더로 함께 관리합니다.
