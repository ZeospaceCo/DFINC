# 엑셀 업로드형 조직도 설치

새 화면: `/DeskPlusEIP/Account/Organization/Sheet.aspx`

기존 조직도와 독립된 페이지입니다. 새 메뉴의 URL을 위 주소로 등록하거나 직접 접속합니다.

1. `SQLConnSolution`에 지정된 DB에서 `Sql/Create-OrganizationSheet.sql`을 한 번 실행합니다. 새 조직도 저장 테이블만 추가하며 기존 행은 변경하지 않습니다.
2. Account 프로젝트를 빌드하고 `Zeospace.DeskPlus.Account.BE.dll`, `Zeospace.DeskPlus.Account.DAL.dll`, `Zeospace.DeskPlus.Account.Web.dll`을 상위 웹의 `bin`에 배포합니다.
3. 다음 파일을 같은 상대 경로로 배포합니다.
   - `Account/Organization/Sheet.aspx`
   - `Account/Organization/Upload.ashx`
   - `Account/Js/Organization/SheetModel.js`
   - `Account/Js/Organization/Sheet.js`
   - `Account/Resource/OrganizationSheet.css`
   - `Account/App_Data/OrganizationSheetTemplate.json`
4. 기본 양식 JSON의 `companyID`는 현재 확인한 회사 ID인 `1`입니다. 대상 회사 ID가 다르면 배포 전에 변경합니다. 기본 양식의 이름/이미지는 지정한 회사에서만 제공됩니다. 이미 저장된 회사별 데이터가 있으면 DB 값을 우선합니다.
5. 이전 셀 편집 버전을 설치했다면 `Account/Ws/Organization/Sheet.asmx`를 제거합니다. 새 업로드 버전은 이 API를 사용하지 않습니다. 기존 `Organization/Default.aspx`는 변경하지 않습니다.
6. 관리자 계정으로 `엑셀 업로드`를 눌러 `.xlsx`를 등록하고, 일반 계정에는 업로드 버튼이 보이지 않으며 갱신된 조직도를 볼 수 있는지 확인합니다.

이미 `DP_ACC_OrganizationSheet` 테이블을 설치했다면 SQL을 다시 실행할 필요가 없습니다. 업로드도 기존 저장 테이블을 사용합니다. 저장된 조직도는 다음 업로드 전까지 그대로 표시됩니다.

업로드 권한은 기존 ACCOUNT 관리자 화면과 동일하게 시스템 관리자 또는 `admin`, `account`, `accountExp`, `Employee` 역할을 사용합니다. 업로드 API도 동일한 권한, 로그인 회사 ID, 세션 토큰을 확인합니다. 회사마다 하나의 조직도를 저장하며, 다른 관리자가 먼저 갱신했다면 덮어쓰기를 거부합니다.

## 사용 방법

1. 엑셀에서 조직도를 수정하고 계산을 완료한 뒤 `.xlsx`로 저장합니다.
2. `조직도`라는 표시 시트를 우선 읽습니다. 해당 시트가 없으면 현재 선택된 표시 시트, 그것도 없으면 첫 표시 시트를 읽습니다. 숨긴 시트는 표시하지 않습니다.
3. 해당 시트의 인쇄 영역을 조직도 전체가 들어가는 하나의 사각형으로 지정합니다. 인쇄 영역이 없으면 사용 범위를 읽습니다.
4. 화면의 `엑셀 업로드`에서 파일을 선택합니다. 검증과 저장이 완료되면 즉시 현재 화면이 갱신되고 다른 사용자는 새로고침 시 확인할 수 있습니다. 별도 저장 버튼은 없습니다.

파일은 10MB, 표시 영역은 200행·100열 이내입니다. 셀 값·병합·글자 크기·정렬·색상·테두리·행 높이·열 너비·삽입된 PNG/JPEG 이미지·직선 연결선을 표시합니다. 이미지 한 개는 1MB, 이미지 합계는 2MB 이내입니다. 숨긴 행/열이 인쇄 영역에 있거나 지원하지 않는 텍스트 상자·차트·도형이 있으면 수정 안내를 표시합니다. 수식은 엑셀에 저장된 계산 결과를 표시하므로 업로드 전에 엑셀에서 계산 후 저장해야 합니다. 잘못된 파일이나 저장 실패 시 기존 조직도는 유지됩니다.

서버에 Microsoft Excel이나 Python을 설치할 필요는 없습니다. 업로드 파일은 서버에서 표시용 데이터로 변환해 회사별 DB에 저장하며 원본 엑셀 파일은 보관하지 않습니다. 인사 원장의 사용자/부서 정보는 변경하지 않습니다.

원본의 `조직도` 시트만 읽어 기본 양식을 다시 만들려면 Python으로 다음 명령을 실행합니다. 원본 엑셀은 변경하지 않습니다.

```powershell
python .\Tools\Import-OrganizationSheet.py '조직도.xlsx' .\DeskPlusEIP\Account\App_Data\OrganizationSheetTemplate.json --company-id 1
```

이 개발용 명령으로 만든 기본 양식은 기존 DB 저장본을 덮어쓰지 않습니다. 일상적인 갱신에는 화면의 업로드 버튼을 사용합니다. `App_Data`는 외부 직접 다운로드가 차단된 상태로 유지합니다.
