### DeskPlusSP v4.6 (2017.2.24) #########################################################
: 2026-09-18 by ghtjd7794 -----------------------------------------------------------
[전자결재] 승인팝업에서 결재의견 입력창 사이즈 수정
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
[포틀릿] 축하합니다. 이미지 잘림 수정
	\DeskPlusEIP\Portlet\Package\Congratulation.ascx
	\DeskPlusEIP\Portlet\Jt\Package\JTCongratulation.htm


: 2026-09-18 by shpark1325 -------------------------------------------------------------
[포틀릿]결재문서 포틀릿의 상신중인문서, 결재대상문서에 erp문서 나오도록 처리
	\DeskPlusEIP\Portlet\Ws\Package\Approval\Tab.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	
	* DB
	DP_APP_DocProcess_SelPageCntProcessInErp
	DP_APP_DocProcess_SelPageListProcessInErp
	DP_APP_DocProcess_SelPageListApproveInErp
	DP_APP_DocProcess_SelPageCntApproveInErp


: 2026-09-17 by kkzz52 -------------------------------------------------------------
[포틀릿] MyInfo 및 오늘의 일정 목록에 공유일정 표시

	* 추가/수정 파일
	\DeskPlusEIP\Portlet\Ws\Package\MyInfo.asmx.cs 수정

	* DB
	- QRY_PORTLET_ToSch_SelCnt
	- DP_PT_ToSch_SelPageList

	* 기능 변경
	- MyInfo 오늘일정 건수는 기존 할일 건수를 포함하면서 조회 가능한 공유일정도 함께 집계
	- ToSchList 일정 건수와 목록에 개인·부서·공유·전체 및 개별 공유 일정 조회 권한 적용
	- 조회일에 걸쳐 있는 기간 일정도 목록과 건수에 포함
	- 여러 공유 조건에 동시에 해당하는 일정은 중복 없이 표시 및 집계

: 2026-09-15 by kkzz52 -------------------------------------------------------------
[일정관리] 전체일정 및 개별 공유 일정 조회 조건 보정

	* DB
	- DP_PT_TodoPopup_PimScheduleListCnt
	- DP_PT_TodoPopup_PimScheduleList
	- DP_PIM_Schedule_PersonStatic

	* 기능 변경
	- 전체일정(OpenType=2)은 공유대상 선택 여부와 관계없이 전 사용자에게 표시 및 카운트
	- 공유일정(OpenType=1)은 공유대상 사용자/부서에게 표시 및 카운트
	- 부서일정(OpenType=3)은 같은 부서 및 하위 부서 사용자에게 표시 및 카운트
	- DP_PIM_ScheShareList에 등록된 일정은 공개종류와 관계없이 공유대상 사용자/부서에게 표시 및 카운트
	- 조회 가능한 타인 일정도 공유일정으로 일괄 변환하지 않고 원래 공개종류별로 집계

: 2026-09-14 by kkzz52 -------------------------------------------------------------
[포틀릿] TO-DO 팝업 및 일정 포틀릿에 공유일정 표시

	* 추가/수정 파일
	\DeskPlusEIP\Portlet\Ws\TodoPopup\TodoList.asmx.cs 수정
	\DeskPlusEIP\Portlet\Ws\Package\PimList.asmx.cs 수정
	\DeskPlusEIP\Pim\Ws\Menu\PimMenu.asmx.cs 수정
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\TodoPopup.cs 수정
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\MyPim.cs 수정
	\Projects\PimProject\Zeospace.DeskPlus.Pim.DAL\Schedule.cs 수정

	* DB
	- DP_PT_TodoPopup_PimScheduleListCnt
	- DP_PT_TodoPopup_PimScheduleList
	- DP_PIM_Schedule_PersonStatic

	* 기능 변경
	- TO-DO 팝업 일정관리-일정(PIM-S)과 일정 포틀릿에서 내 일정과 공유일정을 함께 조회
	- 사용자·부서·개별 공유일정 및 전체 공유일정 조회 권한을 동일하게 적용
	- 일정관리 좌측 메뉴의 공유일정 카운트에도 동일한 조회 권한 적용
	- 내 일정과 공유일정이 중복되는 경우 동일 일정을 한 번만 표시
	- 건수와 목록에 동일한 공유일정 조회 조건 적용

: 2026-09-02 by ghtjd7794 -----------------------------------------------------------
[전자결재] 상신문서함에서 1인결재 가능하도록 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js

: 2026-08-26 by maniaysw -----------------------------------------------------------
[일정관리] 알람 기간 추가 4~7일 

	\DeskPlusEIP\Pim\Schedule\Add.aspx.cs
	\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs
	
	\DeskPlusEIP\Pim\Schedule\ListDay.aspx.cs
	\DeskPlusEIP\Pim\Schedule\ListMonth.aspx.cs
	\DeskPlusEIP\Pim\Schedule\ListWeek.aspx.cs
	
	
	\DeskPlusEIP\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Resource\GlobalString\string.ja.resx


: 2026-08-07 by kkzz52 -------------------------------------------------------------
[게시판] 메신저 알람 팝업에서 삭제된 게시물 안내 표시

	* 추가/수정 파일
	\DeskPlusEIP\Board\Content\ViewPopup.aspx 수정
	\DeskPlusEIP\Board\Content\ViewPopup.aspx.cs 수정
	
	리소스 추가

	* 기능 변경
	- 메신저 알람 링크의 게시물이 삭제된 경우 본문에 "삭제된 게시물입니다." 표시
	- 삭제된 게시물에서는 조회·평가·인쇄·댓글·첨부파일 기능을 표시하지 않음
	- 삭제 안내 문구를 게시판 다국어 리소스로 관리

: 2026-08-06 by kkzz52 -------------------------------------------------------------
[게시판] 일반 공지 팝업에 "다시 띄우지 않기" 기능 추가

	* 추가/수정 파일
	\DeskPlusEIP\Portlet\BoardPopup\BoardPopUp.aspx 수정
	\DeskPlusEIP\Portlet\Js\BoardPopup\BoardPopUp.js 수정
	\DeskPlusEIP\Board\Content\View.aspx 수정
	\DeskPlusEIP\Board\Content\View.aspx.cs 수정
	\DeskPlusEIP\Board\Js\Content\View.js 수정

	* 기능 변경
	- "다시 띄우지 않기" 클릭 시 공지 ID(popid) 기준 쿠키를 3650일간 저장
	- 팝업 게시물 상세 화면에서 팝업 허용/오늘 하루/일주일/다시 띄우지 않기를 개인 설정 가능
	- 팝업 사용 기간이 지난 게시물에는 개인 팝업 설정 영역을 표시하지 않음

: 2026-08-05 by shpark1325 ----------------------------------------------------------
[전자결재] 협조 최종결재지정여부 체크해제
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js

: 2026-08-05 by kkzz52 ----------------------------------------------------------
[전자결재] 메신저 결재 바로가기 새로고침 기본 화면 이동 보정

	* 추가/수정 파일
	\DeskPlusEIP\EIP\Js\Package\NMenu.js 수정
	\DeskPlusEIP\EIP\Package\NMenu.aspx 수정

	* 기능 변경
		- 메신저 결재 바로가기 최초 진입 후 직접호출 파라미터를 주소에서 제거하도록 보정
		- 이후 F5 새로고침 시 결재 직접호출을 반복하지 않고 일반 로그인과 동일하게 홈 화면으로 이동

: 2026-08-05 by kkzz52 ----------------------------------------------------------
[전자결재] 신메뉴 반려(결재) 카운트 기준 일치 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Common\Status\FolderStatusAllTreeScript.ascx.cs 수정

	* 기능 변경
		- 신메뉴 반려(결재) 트리 카운트를 ListRejectAll 목록과 동일한 All 목록 집계 기준으로 조회하도록 보정

: 2026-08-05 by kkzz52 ----------------------------------------------------------
[전자결재] 신메뉴 문서함 목록 문맥 유지 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Js\Folder\Private\ListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListRejectAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\ListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ListReceiveAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReceiptListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Dept\ListAll.js 수정

	* 기능 변경
		- 신메뉴 문서함 All 목록의 페이징 및 상세 처리 후 복귀 시 folderStatusID를 유지해 진입한 메뉴 타이틀을 유지하도록 보정
		- 결재현황 신메뉴 목록은 동일 문맥 누락이 없는 것을 확인

: 2026-08-04 by kkzz52 ----------------------------------------------------------
[전자결재] 수발신함/개인문서함 페이징 목록 문맥 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ListReceiveAll.js 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\ListReceiveAll.aspx 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardListReceiveAll.js 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListReceiveAll.aspx 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ListAll.js 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\ListAll.aspx 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardListAll.js 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListAll.aspx 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendListAll.js 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirSendListAll.aspx 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReceiptListAll.js 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirReceiptListAll.aspx 수정

	* 기능 변경
		- 수발신함 참조문서의 페이징/검색/페이지 크기 변경 시 개인문서함이 아닌 현재 수발신함 목록을 유지하도록 보정
		- 동일한 이동 오류가 있는 수발신함 전달문서 목록도 함께 보정
		- 개인문서함의 참조/전달/보낸회람/받은회람도 페이징 시 현재 All 목록을 유지하도록 보정

: 2026-08-04 by kkzz52 ----------------------------------------------------------
[전자결재] 메신저 진입 후 상단 상황조회 구메뉴 이동 보정

	* 추가/수정 파일
	\DeskPlusEIP\EIP\Package\NMenu.aspx.cs 수정
	\DeskPlusEIP\EIP\Package\NMenu.aspx 수정
	\DeskPlusEIP\EIP\Js\Package\NMenu.js 수정

	* 기능 변경
		- 메신저 전자결재 바로가기 최초 진입은 기존처럼 개인 설정에 따라 신메뉴/구메뉴로 이동
		- 메신저로 진입한 후 상단 상황조회 메뉴를 직접 클릭하면 원래 구메뉴 URL로 이동하도록 보정

: 2026-08-04 by kkzz52 ----------------------------------------------------------
[전자결재] 수발신함 참조문서 확인 후 트리 카운트 갱신 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Js\Status\Menu\TreeAll.js 수정
	\DeskPlusEIP\Approval\Status\Menu\TreeAll.aspx 수정

	* 기능 변경
		- 수발신함 참조문서 상세 화면에서 다른 트리 폴더 클릭 시 카운트를 직접 재조회한 후 목록으로 이동하도록 보정
		- 상세 화면 URL을 소문자로 정규화하여 대소문자와 관계없이 카운트 갱신 대상을 판별하도록 보정
		- 일반 목록 간 트리 이동은 기존처럼 카운트 재조회를 생략하여 불필요한 갱신 방지

: 2026-08-04 by kkzz52 ----------------------------------------------------------
[전자결재] 개인문서함/완결결재문서함 목록 조회 성능 보정

	* 추가/수정 파일
	\Patch_DP_APP_Folder_List_Performance_20260804.sql 추가

	* DB 추가/변경
		- 동일 키 인덱스가 없는 경우 추가: dbo.DP_APP_AppDoc (CompanyID, UserID, FolderID, DocumentID)
		- 동일 키 인덱스가 없는 경우 추가: dbo.DP_APP_PrivateDoc (CompanyID, UserID, FolderID, DocumentID)
		- 동일 키 인덱스가 없는 경우 추가: dbo.DP_APP_ImpoUser (CompanyID, DocumentID, UserID)
		- 프로시저 수정: dbo.DP_APP_Doc_SelPageListPrivate, dbo.DP_APP_Doc_SelPageCntPrivate
		- 프로시저 수정: dbo.DP_APP_Doc_SelPageListApp, dbo.DP_APP_Doc_SelPageCntApp
		  사용자 문서 전체의 표시용 데이터를 조회하지 않고 페이지 대상 문서 키를 먼저 추출한 후 현재 페이지 데이터만 조회하도록 변경
		  목록 건수는 표시용 문서 뷰를 거치지 않고 기본 문서 테이블에서 조회하도록 변경
		  정렬값이 같은 문서는 문서함 고유 ID로 순서를 고정하여 페이지 이동 시 중복/누락 방지

: 2026-08-04 by kkzz52 ----------------------------------------------------------
[전자결재] 결재 홈 완료 탭 조회 성능 보정

	* 추가/수정 파일
	\Patch_DP_APP_Home_ListComplete_Performance_20260804.sql 추가

	* DB 추가/변경
		- 인덱스 추가: dbo.DP_APP_AppDoc (CompanyID, UserID, DocumentID)
		- 인덱스 추가: dbo.DP_APP_PrivateDoc (CompanyID, UserID, DocumentID)
		- 프로시저 수정: dbo.DP_APP_Home_ListComplete
		  사용자 결재/개인 문서 ID를 먼저 조회한 후 완료 문서와 조인하도록 변경
		  기존과 동일하게 완료/반려 문서 중 완료일 기준 최근 4건을 반환하며, 완료일이 같으면 DocumentID 내림차순 적용

: 2026-08-04 by kkzz52 ----------------------------------------------------------
[전자결재] 구메뉴/신메뉴 이동 문맥 추가 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js 수정
	\DeskPlusEIP\Approval\Js\Status\ListProcessAll.js 수정
	\DeskPlusEIP\Approval\Js\Status\ListRejectAll.js 수정
	\DeskPlusEIP\Approval\Js\Status\ListReportDelayAll.js 수정
	\DeskPlusEIP\Approval\Js\Home\Content.js 수정
	\DeskPlusEIP\Portlet\Package\MyAppCount.ascx 수정

	* 기능 변경
		- 구메뉴/신메뉴 문서 목록에서 진입한 본문 수정 및 수신문서 상신/수정은 부모 FormFlow 프레임 기준으로 기존 메뉴에 복귀하도록 보정
		- 결재 화면에서 본문 수정 진입 시 현재 문서 목록의 구메뉴/신메뉴 기준으로 FormFlow 프레임을 선택하도록 보정
		- 메뉴 문맥이 없는 직접 호출은 개인별 신메뉴/구메뉴 설정 기준으로 이동
		- 신메뉴 상신/반려/보고지연 문서함의 수정 후 재상신 진입 시 신메뉴 FormFlow 프레임을 유지하도록 보정
		- 결재 홈 본문 탭의 상신문서/완결문서/참조문서 더보기 링크가 개인별 신메뉴/구메뉴 설정을 따르도록 보정
		- 메인 건수 포틀릿의 결재참조/받은회람 링크가 개인 설정에 따라 신메뉴 문서함으로 이동하도록 보정(시행직인은 기존 구메뉴 유지)

: 2026-07-31 by ghtjd7794 ----------------------------------------------------------
[게시판] 메인팝업 겹치지 않도록 수정(고영선 요청으로 원복)
	\DeskPlusEIP\EIP\Package\NMain.aspx.cs

: 2026-07-28 by ghtjd7794 ----------------------------------------------------------
[게시판] 메인팝업 겹치지 않도록 수정
	\DeskPlusEIP\EIP\Package\NMain.aspx.cs

: 2026-07-23 by kkzz52 ----------------------------------------------------------
[전자결재] 신메뉴 처리 후 트리 전체 카운트 갱신 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Status\DefaultAll.aspx 수정
	\DeskPlusEIP\Approval\Js\Status\Menu\TreeAll.js 수정
	\DeskPlusEIP\Approval\Common\Status\FolderStatusAllTreeScript.ascx.cs 수정
	\DeskPlusEIP\Approval\Ws\Status\Common\SetDefaultAll.asmx.cs 수정
	\DeskPlusEIP\Approval\Js\Folder\Private\ListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListRejectAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\ListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReceiptListAll.js 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendListAll.js 수정

	* 기능 변경
		- 신메뉴에서 문서보기/결재/상신 화면에서 리스트로 복귀하는 경우 Ajax로 왼쪽 트리 카운트만 갱신하도록 변경
		- 승인/수신문서 상신/참조/전달 확인 및 이관/협조/보류/관계/후열/진행/반려 등 상태 변경 후 리스트 복귀 시 트리 카운트가 갱신되도록 보정
		- 협조 문서함 카운트가 초기 트리 생성 및 Ajax 카운트 갱신 시 모두 표시되도록 보정
		- 개인문서함 계열 문서함(완결/반려/전결/참조/전달/회람)에서 삭제 후 리스트에 머무르는 경우에도 트리 카운트가 갱신되도록 보정
		- 트리 프레임 전체 리로드 방식은 제거하고 노드 카운트 텍스트만 갱신하도록 처리

: 2026-07-23 by kkzz52 ----------------------------------------------------------
[전자결재] 전달/회람 알림 발송 누락 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Js\Folder\Dept\ForwardMemo.js 수정
	\DeskPlusEIP\Approval\Folder\Dept\ForwardMemo.aspx.cs 수정
	\DeskPlusEIP\Approval\Js\Folder\Dept\CirMemo.js 수정
	\DeskPlusEIP\Approval\Folder\Dept\CirMemo.aspx.cs 수정

	* 기능 변경
		- 전달/회람 저장 성공 콜백에서 팝업을 먼저 닫아 알림 포스트백이 실행되지 않을 수 있는 흐름 보정
		- 전달/회람 알림 발송 포스트백 완료 후 처리 완료 메시지와 팝업 닫기가 실행되도록 변경

: 2026-07-22 by kkzz52 ----------------------------------------------------------
[전자결재] 수신문서 연동문서 미지정 양식 Legacy 경로 호출 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs 수정

	* 기능 변경
		- 수신문서 작성/수정 화면에서 연동문서가 지정되지 않은 양식은 CustomFilePath가 빈값으로 내려가도록 보정
		- 연동파일명이 없는 상태에서 /DeskPlusEIP/Legacy/ 루트 경로가 호출되어 403 오류가 발생하는 현상 보정

: 2026-07-22 by kkzz52 ----------------------------------------------------------
[전자결재] 신메뉴 초기메뉴 설정 일부 문서함 미반영 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Js\Status\ListAfterwardAll.js 수정
	\DeskPlusEIP\Approval\Js\Status\ListDelayAll.js 수정
	\DeskPlusEIP\Approval\Js\Status\ListProcessAll.js 수정
	\DeskPlusEIP\Approval\Js\Status\ListRejectAll.js 수정
	\DeskPlusEIP\Approval\Js\Status\ListRelatedAll.js 수정
	\DeskPlusEIP\Approval\Js\Status\ListReportDelayAll.js 수정

	* 기능 변경
		- 신메뉴 상태조회 계열 초기메뉴 설정이 구메뉴 저장 API를 호출하던 부분을 신메뉴 저장 API로 보정
		- 진행/보류/반려/참조/보고지연/후열 문서함 초기메뉴 설정이 통합 결재함 첫 화면에 반영되도록 변경


: 2026-07-22 by kkzz52 ----------------------------------------------------------
[전자결재] 메신저 전자결재 알람 링크 개인별 신메뉴/구메뉴 이동 적용

	* 추가/수정 파일
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs 수정
	\DeskPlusEIP\Approval\Status\Default.aspx.cs 수정
	\DeskPlusEIP\Site\Auth\LoginOutSide.aspx.cs 수정

	* 기능 변경
		- 메신저 전자결재 알람 중 리스트/함으로 이동하는 링크가 개인 설정에 따라 구메뉴/신메뉴로 이동하도록 변경
		- 신메뉴 설정 시 수신결재/반려/결재/이관 알람은 통합 결재함으로 이동하도록 변경
		- 신메뉴 설정 시 회람/전달 알람은 통합 수발신함의 해당 문서함으로 이동하도록 변경
		- 신메뉴 설정 시 전달 알림은 수발신함의 받은 전달함(CarboncopyForwardReceiveAll)으로 이동하도록 보정
		- Carboncopy_Forward 알림 매핑을 CarboncopyForwardReceiveAll로 변경
		- 상신자 반려 알람은 기안함 반려(Reject)로, 결재자 반려 알람은 결재함 결재대상(Approve)으로 이동하도록 구분
		- 분기 이전 메신저 알람 링크도 신메뉴 사용자는 통합 결재함으로 이동하도록 보정
		- 암호화 p 파라미터를 포함한 분기 이전 외부 로그인 알람도 신메뉴 상태값으로 변환하도록 보정
		- 외부 로그인 알람은 클릭 시점의 메뉴 설정에 따라 기존 신메뉴 링크도 구메뉴로 되돌려 이동하도록 보정
		- 구메뉴 전환 시 결재자 반려(ApproveRejectAll)와 통합 수발신함 상태값을 구메뉴 상태값으로 다시 암호화하도록 보정
		- 문서보기/결재팝업 계열 알람과 메일 알람 링크는 기존 동작 유지

: 2026-07-22 by kkzz52 ----------------------------------------------------------
[전자결재] 메신저 전자결재 바로가기 버튼 개인별 신메뉴/구메뉴 이동 적용

	* 추가/수정 파일
	\DeskPlusEIP\EIP\Package\NMenu.aspx.cs 수정

	* 기능 변경
		- 메신저 전자결재 좌측 큰 버튼(dcparm=approvalStatus)이 개인 설정에 따라 구메뉴/신메뉴로 이동하도록 변경
		- 메신저 미결문서/보류문서 버튼의 folderStatusID 파라미터를 유지하면서 신메뉴 설정 시 통합 결재함으로 이동하도록 변경
		- 메신저 참조문서/전달문서 버튼의 cabinetID 파라미터를 신메뉴 folderStatusID로 변환하도록 변경
		  Carboncopy_Doc -> CarboncopyAll
		  Carboncopy_Forward -> CarboncopyForwardAll
		- 일반 전자결재 상황조회 메뉴 클릭은 기존 메뉴 URL을 유지하도록 보정
		- 메신저/외부 바로가기처럼 d=approvalStatus 또는 d=approvalComplete로 직접 진입한 경우에만 개인 설정에 따른 신메뉴 이동 적용

: 2026-07-21 by kkzz52 ----------------------------------------------------------
[전자결재] 개인별 결재대상 신메뉴/구메뉴 이동 설정 추가

	* 추가/수정 파일
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserEnvironmentInfo.cs 수정
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironment.cs 수정
	\DeskPlusEIP\Approval\Preference\Setting\List.aspx 수정
	\DeskPlusEIP\Approval\Preference\Setting\List.aspx.cs 수정
	\DeskPlusEIP\Approval\Preference\Setting\List.aspx.designer.cs 수정
	\DeskPlusEIP\Approval\Js\Preference\Setting\List.js 수정
	\DeskPlusEIP\Approval\Ws\Preference\Setting\List.asmx.cs 수정
	\DeskPlusEIP\Approval\Home\Menu.aspx 수정
	\DeskPlusEIP\Approval\Home\Menu.aspx.cs 수정
	\DeskPlusEIP\Approval\Js\Home\Menu.js 수정
	\DeskPlusEIP\Portlet\Package\MyInfo.ascx 수정
	\DeskPlusEIP\Portlet\Package\MyInfo.ascx.cs 수정
	\DeskPlusEIP\Portlet\Js\Package\MyInfo.js 수정
	\DeskPlusEIP\Portlet\Package\MyInfoNoCnt.ascx 수정
	\DeskPlusEIP\Portlet\Package\MyInfoNoCnt.ascx.cs 수정
	\DeskPlusEIP\Portlet\Js\Package\MyInfoNoCnt.js 수정
	\DeskPlusEIP\Portlet\Package\Approval\Tab.ascx 수정
	\DeskPlusEIP\Portlet\Package\Approval\Tab.ascx.cs 수정
	\DeskPlusEIP\Portlet\Package\MyAppCount.ascx 수정
	\DeskPlusEIP\Portlet\Package\MyAppCount.ascx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx 수정
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx 수정
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx 수정
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx 수정
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx 수정
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx 수정
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx 수정
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs 수정
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx 수정
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx.cs 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js 수정
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx 수정
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs 수정
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js 수정

	* 기능 변경
		- 개인 설정 화면에 전자결재 메뉴 선택값 추가: 구메뉴(OLD), 신메뉴(NEW)
		- 개인 설정 저장 시 전자결재 메뉴 선택값 저장 처리 추가
		- 전자결재 홈 결재대상 링크가 개인 설정에 따라 이동하도록 변경
		  구메뉴: /DeskPlusEIP/Approval/Status/Default.aspx
		  신메뉴: /DeskPlusEIP/Approval/Status/DefaultAll.aspx
		- 전자결재 홈 참조받은문서 링크도 신메뉴 설정 시 통합 참조받은문서로 이동하도록 변경
		- 전자결재 홈 개인별문서함/완결결재문서함/반려문서함 링크도 신메뉴 설정 시 통합 메뉴로 이동하도록 변경
		- 전자결재 탭 포틀릿 참조문서/전달문서/받은회람 링크도 신메뉴 설정 시 통합 메뉴로 이동하도록 변경
		- 기본 포틀릿/메신저 전자결재 결재대상 이동도 개인 설정을 따르도록 변경
		- 문서 상신/본문수정/수신문서 상신 후 상태조회 이동을 화면 프레임 기준이 아닌 개인별 신메뉴/구메뉴 설정 기준으로 보정
		- FuncCall 없이 직접 열린 결재 팝업의 결재 처리 후 상태조회 이동도 개인별 신메뉴/구메뉴 설정 기준으로 보정
		- 개인 설정이 신메뉴(NEW)인 경우 DefaultAll.aspx로, 구메뉴(OLD)인 경우 Default.aspx로 이동하도록 변경

	* DB 추가/변경
		- 테이블 수정: dbo.DP_ACC_UserEnv
		  ApprovalStatusMenuType varchar(3) NOT NULL DEFAULT('OLD') 컬럼 추가
		  OLD: 구메뉴, NEW: 신메뉴
		- 프로시저 추가: dbo.DP_ACC_UserEnv_UpdApprovalStatusMenuType
		  개인별 ApprovalStatusMenuType 저장
		- 실서버 DB 패치 파일: \Patch_DP_ACC_UserEnv_ApprovalStatusMenuType_20260721.sql

: 2026-07-14 by kkzz52 ----------------------------------------------------------
[전자결재] 통합 결재함/수발신함/반려문서함 보정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeReceiveAll.js 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js 수정
	\DeskPlusEIP\Approval\Folder\Approve\ListReject.aspx 수정
	\DeskPlusEIP\Approval\Folder\Approve\ListReject.aspx.cs 수정
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListReject.js 수정
	\DeskPlusEIP\Approval\Folder\Approve\ListRejectAll.aspx 수정
	\DeskPlusEIP\Approval\Folder\Approve\ListRejectAll.aspx.cs 수정
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListRejectAll.js 수정

	* 삭제 파일
	\DeskPlusEIP\Approval\FormFlow\Receive\AddAll.aspx 삭제
	\DeskPlusEIP\Approval\FormFlow\Receive\AddAll.aspx.cs 삭제
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\AddAll.js 삭제

	* 기능 변경
		- 통합 결재함 수신 문서 처리 화면을 구메뉴와 동일하게 보정
		- 수발신함 참조/전달 문서보기 읽음확인 및 이관 버튼 노출 보정
		- 개인문서함 반려(결재), 구메뉴 반려문서함 삭제 기능 추가
	
: 2026-07-13 by maniaysw --------------------------------------------------------
[메뉴] 권한 부여해도 메뉴 적용 안되는 문제 
	\DeskPlusEIP\EIP\Js\Admin\Auth\List.js
	\DeskPlusEIP\EIP\Js\Admin\Auth\Mod.js

: 2026-07-08 by kkzz52 ----------------------------------------------------------
[전자결재] 통합 트리 결재함 이관 폴더 추가

	* 추가/수정 파일
	\Patch_DP_APP_FolderStatusAll_TransferAll_20260708.sql 추가
	\Create_DP_APP_FolderStatusAllDefault.sql 수정
	\DeskPlusEIP\Approval\Status\ListTransferAll.aspx 추가
	\DeskPlusEIP\Approval\Status\ListTransferAll.aspx.cs 추가
	\DeskPlusEIP\Approval\Status\ListTransferAll.aspx.designer.cs 추가
	\DeskPlusEIP\Approval\Js\Status\ListTransferAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListTransferAll.asmx 추가
	\DeskPlusEIP\Approval\Ws\Status\ListTransferAll.asmx.cs 추가
	\DeskPlusEIP\Approval\Js\Status\Menu\TreeAll.js 수정
	\DeskPlusEIP\Approval\Status\DefaultAll.aspx.cs 수정
	\DeskPlusEIP\Approval\Approval.csproj 수정

	* 기능 변경
		- 이관 문서 클릭 시 수신문서 상신 결재망 페이지로 이동하도록 파라미터 보정

	* DB 테이블
		- DP_APP_FolderStatusAll

	* DB 추가/변경
	DP_APP_FolderStatusAll 메뉴 데이터 추가/갱신
		- Transfer / 이관 / ApprovalBox 하위, Receive 다음 순서로 추가
		- Cooperate, Delay, Related, Afterward 순서 조정
		- 이관 폴더 대상 페이지를 ListTransferAll.aspx로 연결

: 2026-07-08 by kkzz52 ----------------------------------------------------------
[전자결재] 통합 트리 수발신함 분리

	* 추가/수정 파일
	\Patch_DP_APP_FolderStatusAll_ReceiveSendBox_20260708.sql 추가
	\Create_DP_APP_FolderStatusAllDefault.sql 수정
	\DeskPlusEIP\Approval\Js\Status\Menu\TreeAll.js 수정
	\DeskPlusEIP\Approval\Status\DefaultAll.aspx.cs 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\ListReceiveAll.aspx 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListReceiveAll.aspx 수정

	* DB 테이블
		- DP_APP_FolderStatusAll

	* DB 추가/변경
	DP_APP_FolderStatusAll 메뉴 데이터 추가/갱신
		- ReceiveSendBox / 수발신함
		- CarboncopyReceiveAll / 참조 / ReceiveSendBox 하위로 이동
		- CarboncopyForwardReceiveAll / 전달 / ReceiveSendBox 하위로 이동

: 2026-07-06 by kkzz52 ----------------------------------------------------------
[전자결재] 후열 문서확인 시각 표시 수정

	* 추가/수정 파일
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\SequenceBackUp.cs 수정
	
	* DB 추가/변경
	DP_APP_SeqBackup_UpdAfterwardConfirm 추가
		- 후열 문서확인 시 완료 결재선(DP_APP_SeqBackup)의 ApprovalDate/ApprovalDateYMD/ViewingTime 갱신
		- ApprovalType = 'L' 후열 표시 유지

: 2026-07-01 by kkzz52 ----------------------------------------------------------
[전자결재] 결재함 참조/전달 확인 전 문서함 추가 및 개인 문서함 확인 완료 분리

	* 추가/수정 파일
	\Patch_DP_APP_FolderStatusAll_CarboncopyReceiveAll_20260701.sql 추가
	
	\DeskPlusEIP\Approval\Folder\Carboncopy\ListReceiveAll.aspx 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\ListReceiveAll.aspx.cs 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\ListReceiveAll.aspx.designer.cs 추가
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ListReceiveAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ListReceiveAll.asmx 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ListReceiveAll.asmx.cs 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListReceiveAll.aspx 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListReceiveAll.aspx.cs 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListReceiveAll.aspx.designer.cs 추가
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardListReceiveAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ForwardListReceiveAll.asmx 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ForwardListReceiveAll.asmx.cs 추가
	\DeskPlusEIP\Approval\Js\Status\Menu\TreeAll.js 수정
	\DeskPlusEIP\Approval\Status\DefaultAll.aspx.cs 수정
	\DeskPlusEIP\Approval\Common\Status\FolderStatusAllTreeScript.ascx.cs 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\ListAll.aspx.cs 수정
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListAll.aspx.cs 수정
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ListAll.asmx.cs 수정
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ForwardListAll.asmx.cs 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CarboncopyAllData.cs 추가

	* DB 추가/변경
	DP_APP_FolderStatusAll 메뉴 데이터 추가/갱신
		- CarboncopyReceiveAll / 참조 / \DeskPlusEIP\Approval\Folder\Carboncopy\ListReceiveAll.aspx
		- CarboncopyForwardReceiveAll / 전달 / \DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListReceiveAll.aspx
	DP_APP_Doc_SelPageListCarbonAll 추가
	DP_APP_Doc_SelPageCntCarbonAll 추가
	DP_APP_Doc_SelPageListForwardAll 추가
	DP_APP_Doc_SelPageCntForwardAll 추가

: 2026-07-01 by kkzz52 ----------------------------------------------------------
[전자결재] 결재상황조회 통합 트리 및 All 전용 목록 수정

	* 추가/수정 파일
	\DeskPlusEIP\Approval\Folder\Approve\ListRejectAll.aspx.cs 수정
	\DeskPlusEIP\Approval\Ws\Folder\Approve\ListRejectAll.asmx.cs 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcessAll.cs 수정

	* DB 변경
	DP_APP_DocProcess_SelPageListApproveAll 수정
	DP_APP_DocProcess_SelPageCntApproveAll 수정
	DP_APP_Doc_SelPageListRejectAll 추가
	DP_APP_Doc_SelPageCntRejectAll 추가
	DP_APP_ShortReply_Ins 수정
	DP_APP_DocProcess_SelPageListReportDelayAll 수정
	DP_APP_DocProcess_SelPageCntReportDelayAll 수정

: 2026-06-19 by kkzz52 ----------------------------------------------------------
[전자결재] 결재상황조회 통합 트리 및 All 전용 목록 추가

	* 추가/수정 파일
	\Create_DP_APP_FolderStatusAllDefault.sql 추가
	\DeskPlusEIP\Approval\Status\DefaultAll.aspx 추가
	\DeskPlusEIP\Approval\Status\Menu\TreeAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\Menu\TreeAll.js 추가
	\DeskPlusEIP\Approval\Common\Status\FolderStatusAllTreeScript.ascx 추가
	\DeskPlusEIP\Approval\Status\ListApproveAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListApproveAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListApproveAll.asmx 추가
	\DeskPlusEIP\Approval\Status\ListReceiveAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListReceiveAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListReceiveAll.asmx 추가
	\DeskPlusEIP\Approval\FormFlow\DefaultAll.aspx 추가
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveAll.aspx 추가
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeReceiveAll.js 추가
	\DeskPlusEIP\Approval\FormFlow\Receive\AddAll.aspx 추가
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\AddAll.js 추가
	\DeskPlusEIP\Approval\Status\ListCooperateAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListCooperateAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListCooperateAll.asmx 추가
	\DeskPlusEIP\Approval\Status\ListDelayAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListDelayAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListDelayAll.asmx 추가
	\DeskPlusEIP\Approval\Status\ListRelatedAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListRelatedAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListRelatedAll.asmx 추가
	\DeskPlusEIP\Approval\Status\ListAfterwardAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListAfterwardAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListAfterwardAll.asmx 추가
	\DeskPlusEIP\Approval\Status\ListProcessAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListProcessAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListProcessAll.asmx 추가
	\DeskPlusEIP\Approval\Status\ListRejectAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListRejectAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListRejectAll.asmx 추가
	\DeskPlusEIP\Approval\Status\ListReportDelayAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Status\ListReportDelayAll.js 추가
	\DeskPlusEIP\Approval\Ws\Status\ListReportDelayAll.asmx 추가
	\DeskPlusEIP\Approval\Ws\Status\Common\SetDefaultAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Private\ListAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Private\ListAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Private\ListAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Approve\ListAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Approve\ListAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Approve\ListRejectAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListRejectAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Approve\ListRejectAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Arbitrary\ListAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\ListAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Arbitrary\ListAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\ListAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ListAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ListAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardListAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ForwardListAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirReceiptListAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReceiptListAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\CirReceiptListAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirSendListAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendListAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\CirSendListAll.asmx 추가
	\DeskPlusEIP\Approval\Folder\Dept\ListAll.aspx 추가
	\DeskPlusEIP\Approval\Js\Folder\Dept\ListAll.js 추가
	\DeskPlusEIP\Approval\Ws\Folder\Dept\ListAll.asmx 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FolderStatusAll.cs 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcessAll.cs 추가

	Web.config
		- zeoAppFolderStatusAllTree 추가

	* DB 추가/변경
	DP_APP_FolderStatusAll 테이블 추가
	DP_APP_FolderStatusAll 메뉴 데이터 추가/갱신
	DP_APP_FolderStatusAllDefault 테이블 추가(All 통합 트리 초기메뉴 저장)
		- ApprovalBox / 결재함
		- DraftBox / 기안함
		- Approve / 결재 / \DeskPlusEIP\Approval\Status\ListApproveAll.aspx
		- Receive / 수신 / \DeskPlusEIP\Approval\Status\ListReceiveAll.aspx
		- Cooperate / 협조 / \DeskPlusEIP\Approval\Status\ListCooperateAll.aspx
		- Delay / 보류 / \DeskPlusEIP\Approval\Status\ListDelayAll.aspx
		- Related / 관계 / \DeskPlusEIP\Approval\Status\ListRelatedAll.aspx
		- Afterward / 후열 / \DeskPlusEIP\Approval\Status\ListAfterwardAll.aspx
		- Process / 진행 / \DeskPlusEIP\Approval\Status\ListProcessAll.aspx
		- Reject / 반려 / \DeskPlusEIP\Approval\Status\ListRejectAll.aspx
		- ReportDelay / 보류 / \DeskPlusEIP\Approval\Status\ListReportDelayAll.aspx
		- PersonalBox / 개인 문서함
		- PrivateCompleteAll / 완결(상신) / \DeskPlusEIP\Approval\Folder\Private\ListAll.aspx
		- ApproveCompleteAll / 완결(결재) / \DeskPlusEIP\Approval\Folder\Approve\ListAll.aspx
		- ApproveRejectAll / 반려(결재) / \DeskPlusEIP\Approval\Folder\Approve\ListRejectAll.aspx
		- ArbitraryAll / 전결 / \DeskPlusEIP\Approval\Folder\Arbitrary\ListAll.aspx
		- CarboncopyAll / 참조 / \DeskPlusEIP\Approval\Folder\Carboncopy\ListAll.aspx
		- CarboncopyForwardAll / 전달 / \DeskPlusEIP\Approval\Folder\Carboncopy\ForwardListAll.aspx
		- CarboncopyCirReceiptAll / 받은회람 / \DeskPlusEIP\Approval\Folder\Carboncopy\CirReceiptListAll.aspx
		- CarboncopyCirSendAll / 보낸회람 / \DeskPlusEIP\Approval\Folder\Carboncopy\CirSendListAll.aspx
		- DeptBox / 부서 문서함
		- DeptCompleteAll / 완결 / \DeskPlusEIP\Approval\Folder\Dept\ListAll.aspx
	DP_APP_DocProcess_SelPageListApproveAll 추가
	DP_APP_DocProcess_SelPageCntApproveAll 추가
	DP_APP_DocProcess_SelPageListCooperateAll 추가
	DP_APP_DocProcess_SelPageCntCooperateAll 추가
	DP_APP_DocProcess_SelPageListProcessAll 추가
	DP_APP_DocProcess_SelPageCntProcessAll 추가
	DP_APP_DocProcess_SelPageListDelayAll 추가
	DP_APP_DocProcess_SelPageCntDelayAll 추가
	DP_APP_DocProcess_SelPageListReceiveAll 추가
	DP_APP_DocProcess_SelPageCntReceiveAll 추가
	DP_APP_DocProcess_SelPageListRelatedAll 추가
	DP_APP_DocProcess_SelPageCntRelatedAll 추가
	DP_APP_DocProcess_SelPageListAfterwardAll 추가
	DP_APP_DocProcess_SelPageCntAfterwardAll 추가
	DP_APP_DocProcess_SelPageListRejectAll 추가
	DP_APP_DocProcess_SelPageCntRejectAll 추가
	DP_APP_DocProcess_SelPageListReportDelayAll 추가
	DP_APP_DocProcess_SelPageCntReportDelayAll 추가

: 2026-04-21 by maniaysw ------------------------------------------------------------
[게시판] 메인팝업 체크 못하도록 요청 
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Content\AddN.aspx

: 2026-04-17 by freeice122 ----------------------------------------------------------
[로그인] 인증시 Spool폴더 삭제 기준 변경(쿠키 살아 있으면 안지게 변경)
	\DeskPlusEIP\Site\Auth\Default.aspx.cs
	\DeskPlusEIP\Site\Auth\LegacyLogin.aspx.cs
	\DeskPlusEIP\Site\Auth\LoginOutSide.aspx.cs


: 2026-04-15 by maniaysw ----------------------------------------------------------
[일정관리] 할일 등록 후 수정 시 일정반영 오류(공유일정 조회대상선택버튼 제거)
	\DeskPlusEIP\Pim\ToDo\Add.aspx.cs
	\DeskPlusEIP\Pim\Schedule\AllSchedule.aspx

: 2026-03-18 by shpark1325 --------------------------------------------------------
[인사] 인사관리 권한 추가 (Employee)
	\DeskPlusEIP\Account\Base\BasePageAccountAdmin.cs

: 2026-03-16 by maniaysw --------------------------------------------------------
[다운로드] 버튼 명칭 변경 
	\DeskPlusEIP\Upload\JSUpload\Main.aspx

	\DeskPlusEIP\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-tw.resx


: 2026-03-06 by kkzz52 ----------------------------------------------------------
[전자결재] 수신회수 시 결재의견 삭제되는 오류 수정
	DP_APP_CounterPartUser_UpdCancel

: 2026-03-05 by kkzz52 ----------------------------------------------------------
[전자결재] 결재의견 버튼에 의견 카운트 표시
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
    \DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
    \DeskPlusEIP\Approval\Js\Common\Popup\View.js
    \DeskPlusEIP\Approval\Js\Common\Popup\ViewCir.js
    \DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
    \DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
    \DeskPlusEIP\Approval\Js\Folder\Common\View.js
    \DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
    \DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
    \DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
    \DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
    \DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
    \DeskPlusEIP\Approval\Js\Process\Confirm\Approve.js
    \DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.js
    \DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusEIP\Approval\Ws\Common\Popup\OpinionView.asmx.cs

: 2026-02-11 by shpark1325 ----------------------------------------------------------
[전자결재] 결재-상황조회-ERP문서-반려문서 리스트에서 문서 삭제 시 폐기처리 요청
	\DeskPlusEIP\Approval\Js\Status\ListRejectSlip.js
	\DeskPlusEIP\Approval\Ws\Status\ListReject.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs

	- 프로시저
	DP_APP_Doc_UpdIsConfirmSlipR

: 2026-02-10 by kkzz52 ----------------------------------------------------------
[전자결재] 수신문서 2개 이상 생기는 오류 방지
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs

: 2026-01-08 by kkzz52 ----------------------------------------------------------
[전자결재] 결재의견 잘리는 현상 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

	DP_APP_Seq_UpdDecide (@Opinion		nvarchar(1000))

	ALTER TABLE DP_APP_Seq ALTER COLUMN Opinion nvarchar(1000);
	ALTER TABLE DP_APP_SeqBackup ALTER COLUMN Opinion nvarchar(1000);

: 2025-12-01 by ghtjd7794 ----------------------------------------------------------
[전자결재] 개인결재선 지정시 더블클릭 방지
	\DeskPlusEIP\Approval\Js\FormFlow\Common\GetFavList.js

: 2025-11-17 by kkzz52 ----------------------------------------------------------
[전자결재] 수신 결재망 자동 추가
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeReceive.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ConfirmReject.js
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDeptUser.js

: 2025-10-28 by kkzz52 ----------------------------------------------------------
[전자결재] 수신결재망에서 개인결재망 사용 가능하게 추가
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs

: 2025-09-25 by ghtjd7794 ----------------------------------------------------------
[패스워드] 변경주기를 코드관리에서 설정
	\DeskPlusEIP\Ws\Site\Auth\Default.asmx.cs

: 2025-09-25 by kkzz52 ----------------------------------------------------------
[전자결재] 부서수신 페이지 추가 (ERP 연동문서), 코드관리로 지정하는 부서수신 로직은 삭제
	\DeskPlusEIP\Approval\Js\Status\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\Status\ListReceiveDept.js 추가
	\DeskPlusEIP\Approval\Js\Status\ListApprove.js
	\DeskPlusEIP\Approval\Js\Status\ListApproveSlip.js
	\DeskPlusEIP\Approval\Js\Status\ListProxy.js
	\DeskPlusEIP\Approval\Status\Default.aspx.cs
	\DeskPlusEIP\Approval\Status\ListApprove.aspx
	\DeskPlusEIP\Approval\Status\ListApprove.aspx.cs
	\DeskPlusEIP\Approval\Status\ListApproveSlip.aspx
	\DeskPlusEIP\Approval\Status\ListApproveSlip.aspx.cs
	\DeskPlusEIP\Approval\Status\ListProxy.aspx
	\DeskPlusEIP\Approval\Status\ListProxy.aspx.cs
	\DeskPlusEIP\Approval\Status\ListReceive.aspx.cs
	\DeskPlusEIP\Approval\Status\ListReceiveDept.aspx 추가
	\DeskPlusEIP\Approval\FormFlow\Default.aspx
	\DeskPlusEIP\Approval\Common\Status\FolderStatusTreeScript.ascx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListReceive.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListReceiveDept.asmx 추가
	\DeskPlusEIP\Approval\Ws\Home\Menu.asmx.cs
	\DeskPlusEIP\Portlet\Package\MyAppCount.ascx.cs
	\DeskPlusEIP\Portlet\Ws\Package\MyInfo.asmx.cs	
	\DeskPlusEIP\Portlet\Ws\Custom\MyInfo_Zeus.asmx.cs
	\DeskPlusEIP\Portlet\Ws\Custom\MyInfo_Zeus_New.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Home.cs
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\MyInfo.cs
	
	VW_APP_DocTot

	DP_APP_DocProcess_SelPageListReceiveDept 추가
	DP_APP_DocProcess_SelPageCntReceiveDept 추가
	DP_APP_DocProcess_SelPageListReceive
	DP_APP_DocProcess_SelPageCntReceive
	DP_APP_Home_TodayCnt
	DP_PT_MyInfo_SelUserID

	DP_APP_FolderStatus (ReceiveDept 추가)
	 
: 2025-09-11 by kkzz52 ----------------------------------------------------------
[전자결재] 부서수신 로직 추가
	부서수신 조직으로 설정된 수신자 지정시  수신자의 부서원이 모두 조회 가능하며 결재선 지정 가능
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\Status\ListApprove.aspx.cs
	\DeskPlusEIP\Approval\Status\ListApproveSlip.aspx.cs
	\DeskPlusEIP\Approval\Status\ListProxy.aspx.cs
	\DeskPlusEIP\Approval\Status\ListReceive.aspx.cs
	\DeskPlusEIP\Approval\Common\Status\FolderStatusTreeScript.ascx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListReceive.asmx.cs
	\DeskPlusEIP\Approval\Ws\Home\Menu.asmx.cs
	\DeskPlusEIP\Portlet\Package\MyAppCount.ascx.cs
	\DeskPlusEIP\Portlet\Ws\Package\MyInfo.asmx.cs	
	\DeskPlusEIP\Portlet\Ws\Custom\MyInfo_Zeus.asmx.cs
	\DeskPlusEIP\Portlet\Ws\Custom\MyInfo_Zeus_New.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Home.cs
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\MyInfo.cs

	DP_APP_DocProcess_SelPageListReceive 수정
	DP_APP_DocProcess_SelPageCntReceive 수정
	DP_APP_CounterPartUser_UpdDecide 수정
	DP_APP_Home_TodayCnt 수정
	DP_PT_MyInfo_SelUserID 수정

	- 부서수신결재대상 코드 추가
	SusinDeptApproval

: 2025-06-25 by maniaysw ----------------------------------------------------------
[전자결재] 홈화면 바로가기 링크 추가 
	\DeskPlusEIP\Approval\Home\Menu.aspx
	\DeskPlusEIP\Approval\Js\Home\Menu.js
	리소스 D:\SiteProject2017\DI5.1\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx 등

: 2025-06-25 by ghtjd7794 ----------------------------------------------------------
[전자결재] 결재메인화면에서 승인시 결재의견 입력창 수정
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js

: 2025-06-23 by kkzz52 ----------------------------------------------------------
[전자결재] 부서별문서함 발신부서 필드 추가
	\DeskPlusEIP\Approval\Js\Folder\Dept\List.js
	\DeskPlusEIP\Approval\Folder\Dept\List.aspx
	\DeskPlusEIP\Approval\Ws\Folder\Dept\List.asmx.cs
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTList.htm

	DP_APP_Doc_SelPageListDept

: 2025-06-10 by kkzz52 ----------------------------------------------------------
[전자결재] 결재의견 각 줄 앞에 존재하는 공백 적용되도록 처리
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js

: 2025-04-29 By famo23 ------------------------------------------------------------
[로그인이력] 1. 기간 필터 : 시작일 기준 AND 조건으로 구성(Default날짜는 오늘로 셋팅)
2. IP 유사 검색 : Like 검색 
3. 검색필터에서 시작일, 종료일 제외
	DP_COM_AccessLog_SelPageCnt
	DP_COM_AccessLog_SelPageList
	\DeskPlusEIP\EIP\Admin\Static\LogIn\ExcelDown.aspx
	\DeskPlusEIP\EIP\Admin\Static\LogIn\List.aspx
	\DeskPlusEIP\EIP\Js\Admin\Static\LogIn\List.js
	\DeskPlusEIP\EIP\Ws\Admin\Static\LogIn\List.asmx	

: 2025-04-03 by kkzz52 ----------------------------------------------------------
[전자결재] 대결 시 원결재자의 이름 및 인장으로 표시, 결재의견은 대결자의 이름으로 저장
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs

	DP_APP_Seq_UpdDecide

: 2025-03-18 by shpark1325 ----------------------------------------------------------
[전자결재] 결재선저장된 양식의 협의 칸 결재자 지정하지 않았을 경우 알림창 요청
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js

: 2025-03-13 by shpark1325 ----------------------------------------------------------
[전자결재] 임시저장, 상신문서함 문서 상신 시 상신일자 변경되도록 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js

: 2025-03-10 by shpark1325 ----------------------------------------------------------
[전자결재] 임시저장 여러번 저장 시 첨부파일, 근거문서 누락 수정
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx

[전자결재] 임시저장 팝업창, 임시보관함 제목 특수문자 표기 요청
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs
	\DeskPlusEIP\Approval\Ws\FormFlow\Temporary\List.asmx.cs

: 2025-03-07 by maniaysw ------------------------------------------------------------
[통계] IP검사 숫자로 like 검색 (.제외) 
	\DeskPlusEIP\EIP\Ws\Admin\Static\LogIn\List.asmx.cs

: 2025-02-24 by shpark1325 ----------------------------------------------------------
[전자결재] 임시저장 팝업창 두번째 안뜨는 문제
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx

: 2024-11-26 by shpark1325 ----------------------------------------------------------
[전자결재] 협의자 전결버튼 숨김 요청
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js

: 2024-11-21 by shpark1325 ----------------------------------------------------------
[전자결재] 협의 칸 결재자 지정하지 않았을 경우 알림창 요청
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js

: 2024-11-12 by maniaysw ------------------------------------------------------------
[포틀릿] 공지, 게시 구분 하드코딩 다국어 
	\DeskPlusEIP\Portlet\Jt\Package\Board\JTList.htm
	\DeskPlusEIP\Portlet\Package\Board\Tab.ascx

: 2024-10-29 by shpark1325 ----------------------------------------------------------
[사용자정보] 기본언어 변경 시 오류 수정
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs

: 2024-08-07 by maniaysw ----------------------------------------------------------
[SLO] 로직 변경 기존 mailid ->  [InterFace].[dbo].[vw_DP_User_All] LoginID_ENC 필드 값으로 호출하도록 수정
LoginID_ENC 값이 96자 이므로 본적 주소 필드 이용 BirthAddress1 

	VW_UserInfo --로컬은 최세영부장 값 하드코딩, 실서버와 다름 주의
	
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\Authenticator.cs
	\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserInfo.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BLL\User.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs
	\DeskPlusEIP\EIP\Package\NMain.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	
	실서버
	\DeskPlusEIP\Portlet\Package\QuickLink.ascx

: 2024-08-06 By famo23 ------------------------------------------------------------
[구글 OTP] 구글 otp 2차 인증 개발 
	DP_ACC_User
	VW_UserInfo

	DP_ACC_User_Ins
	DP_ACC_User_DelOTP
	DP_ACC_User_SelOTP
	DP_ACC_User_SelGetPwdDate
	DP_ACC_User_UpdOTP
	DP_ACC_User_Upd

	\DeskPlusEIP\Account\Admin\User\Add.aspx
	\DeskPlusEIP\Account\Admin\User\Mod.aspx
	\DeskPlusEIP\Account\Js\Admin\User\Mod.js
	\DeskPlusEIP\Account\Ws\Admin\User\Mod.asmx
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserInfo.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs

	\DeskPlusEIP\Js\Site\Auth\Default.js
	\DeskPlusEIP\Site\Auth\Default.aspx
	\DeskPlusEIP\Ws\Site\Auth\Default.asmx	

: 2024-07-31 By shpark1325 ------------------------------------------------------------
[로그인] 패스워드 변경 팝업창 문구 변경
	\DeskPlusEIP\Account\Admin\User\Password\Change.aspx
	\DeskPlusEIP\Account\MyInfo\Password\Change.aspx
	\DeskPlusEIP\Site\Auth\ModPassword.aspx

: 2024-07-30 by kkzz52 ----------------------------------------------------------
[전자결재] 결재망추가, 결재망삭제, 자주지정하는결재자 팝업에서 좌우 끝에 클릭하면 이벤트 발생 안되는 현상 수정
	DeskPlusEIPBase.css (.popup td 수정)
	/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js

: 2024-07-29 By freeice122 ------------------------------------------------------------
[보안처리] 임직원조회 검색 인젝션 처리
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs

[보안처리] 다운로드 경로 보완	
	\DeskPlusEIP\Common\DownLoad\DownLoad.aspx.cs


: 2024-07-08 By shpark1325 ------------------------------------------------------------
[자원예약]부로 지정된 담당자에게도 알람 발송 요청
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs
	QRY_RM_ListUser_Sel

[자원예약]예약시 구분값 기본 노랑색으로 요청
	\DeskPlusEIP\RM\Content\Add.aspx.cs

[자원예약]예약, 수정, 조회 시 구분 값 보이지 않도록 요청
	\DeskPlusEIP\RM\Admin\Reserve\ModN.aspx
	\DeskPlusEIP\RM\Admin\Reserve\View.aspx
	\DeskPlusEIP\RM\Content\AddN.aspx
	\DeskPlusEIP\RM\Content\ModN.aspx
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Content\ViewTotal.aspx

: 2024-06-12 By shpark1325 ------------------------------------------------------------
[자원예약]문구 추가요청 
	\DeskPlusEIP\RM\Js\Content\Add.js

: 2024-05-14 by shpark1325 ----------------------------------------------------------
[게시판]게시판 폴더 옵션의 [외부 다운로드 금지] 체크 시 작성 페이지에 외부다운로드금지 항목나타나도록 요청
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Content\Mod.aspx.cs

: 2024-05-10 by kkzz52 ----------------------------------------------------------
[전자결재] ERP문서 상신 시 중복 값 체크
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs

	VW_APP_DocTotAll 수정

	DP_APP_DocTot_SelByETC 추가

: 2024-05-08 by shpark1325 ----------------------------------------------------------
[포틀릿]게시판 게시체크 된 게시글만 공지, 이외는 게시로 표시  
	\DeskPlusEIP\Portlet\Jt\Package\Board\JTList.htm

: 2024-04-23 by shpark1325 ----------------------------------------------------------
[전자결재] 제목에 , 들어갈 경우 zip다운로드 불가 수정
	\DeskPlusEIP\Common\Zip\CreateZipFileSelect.aspx.cs

: 2024-04-11 by shpark1325 ----------------------------------------------------------
[전자결재] 결재문서리스트, 폐기문서함 검색조건 DocID 추가
	\DeskPlusEIP\Approval\Disuse\List.aspx.cs
	DeskPlusEIP\Approval\Admin\DocumentEtc\JList.aspx.cs

[전자결재]결재문서리스트 삭제 버튼명 변경
	\DeskPlusEIP\Approval\Admin\DocumentEtc\JList.aspx.cs

	-리소스
	Btn_DisuseDel

[전자결재]결재문서리스트 진행중인 문서 삭제 시 알림창 추가 요청
	\DeskPlusEIP\Approval\Admin\DocumentEtc\JList.aspx
	\DeskPlusEIP\Approval\Js\Admin\DocumentEtc\JListCom.js

	-리소스
	MSG_DisuseDel

: 2024-03-08 by kkzz52 --------------------------------------------------------
[전자결재] 수신부서가 있는 문서를 메일 발송 시, 첨부파일이 메일로 전달되지 않는 오류 수정
	\DeskPlusEIP\Approval\Ws\Folder\Dept\SendMail.asmx.cs

: 2024-02-22 by shpark1325 ----------------------------------------------------------
[나의정보] 패스워드 정책 추가
	코드관리-PwdFirst 코드명에 ,로 구분
	\DeskPlusEIP\Js\Site\Auth\ModPassword.js
	\DeskPlusEIP\Account\Js\MyInfo\Password\Change.js

: 2024-02-16 by maniaysw ----------------------------------------------------------
[메신저] 그룹웨어 이동시 패스워드 변경 주기 체크 
	\DeskPlusEIP\Site\Auth\ModPassword.aspx.cs
	\DeskPlusEIP\Js\Site\Auth\ModPassword.js
	\Projects\CommonProject\Zeospace.DeskPlus.Common.DAL\UCMessenger.cs
	\DeskPlusEIP\Site\Auth\LoginOutSide.aspx.cs


: 2024-01-12 by freeice122 --------------------------------------------------------
[전자결재] 문서폐기 사용자 로그 기록
	\DeskPlusEIP\Approval\Ws\Admin\Document\ViewDoc.asmx.cs
	\DeskPlusEIP\Approval\Ws\Disuse\ViewDoc.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Document.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs

: 2023-11-08 by shpark1325 --------------------------------------------------------
[전자결재] 인장없는 사용자만 서명버튼 보이도록 처리
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPasswordBundle.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPasswordBundle.aspx.cs
	
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js

: 2023-10-27 by shpark1325 --------------------------------------------------------
[전자결재] 전자결재 중요표시 리스트에서 결재문서 보이지 않음 수정
	\DeskPlusEIP\Approval\Js\Folder\Impo\List.js

: 2023-10-23 by shpark1325 --------------------------------------------------------
[전자결재] 양식명조건으로 괄호 포함하여 검색 시 검색되도록 수정
	-프로시저
		DP_APP_Doc_SelPageCntDept
		DP_APP_Doc_SelPageListDept
		DP_APP_Doc_SelPageCntApp
		DP_APP_Doc_SelPageListApp
		DP_APP_Doc_SelPageCntPrivate
		DP_APP_Doc_SelPageListPrivate
		DP_APP_Doc_SelPageListReject
		DP_APP_Doc_SelPageCntReject
		DP_APP_Doc_SelPageListArb
		DP_APP_Doc_SelPageCntArb
		DP_APP_Doc_SelPageListCarBon
		DP_APP_Doc_SelPageCntCarBon
		DP_APP_Doc_SelPageListSend
		DP_APP_Doc_SelPageCntSend
		DP_APP_Doc_SelPageListRecv
		DP_APP_Doc_SelPageCntRecv

: 2023-10-12 by kkzz52 --------------------------------------------------------
[전자결재] 첨부파일 팝업 JModal로 변경
	\DeskPlusEIP\Html\Js\JModal\DeskPlusModal2.js 추가
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Sumit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx

: 2023-10-11 by shpark1325 --------------------------------------------------------
[전자결재] 진행중인문서 제목에 괄호가 포함된 문서를 근거문서 첨부시 근거문서리스트에서 정상적으로 나오도록 요청
	\DeskPlusEIP\Approval\Ws\Process\Basic\Approve.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\Common\View.asmx.cs
	\DeskPlusEIP\Approval\Ws\FormFlow\Common\Common.asmx.cs

: 2023-10-10 by shpark1325 --------------------------------------------------------
[전자결재] 결재진행문서-결재대상문서, 일괄결재대상문서-ERP문서-결재대상문서 정렬 수정 요청
	DP_APP_DocProcess_SelPageListApprove
	DP_APP_DocProcess_SelPageListApproveSlip

: 2023-09-27 by ghtjd7794 --------------------------------------------------------
[전자결재] 결재칸 직급+이름 폰트 8pt로 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Common\CommonFlow.js

: 2023-09-07 by kkzz52 --------------------------------------------------------
[전자결재] 임시저장 첨부파일 빠지는 오류 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx

: 2023-09-06 by ghtjd7794 --------------------------------------------------------
[전자결재]결재의견 글자수 제한 변경 요청(200자->500자)
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeModReceive.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceive.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveEdit.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPasswordBundle.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx
	\DeskPlusEIP\Approval\Process\Opinion\ConfirmOpinion.aspx	
	
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx.cs	
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx.cs	
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx.cs	
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeModReceive.aspx.cs	
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceive.aspx.cs	
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveEdit.aspx.cs	
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx.cs	
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPasswordBundle.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx.cs	
	\DeskPlusEIP\Approval\Process\Opinion\ConfirmOpinion.aspx.cs

	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	
	- 다국어 수정
	Msg_ErrOpinion

[전자결재]결재의견 입력창 가로크기 수정 (1.5배)
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js

: 2023-09-05 by kkzz52 --------------------------------------------------------
[전자결재] 결재 시 의견이 있을 경우 의견 메일발송 (오류 수정)
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

	DP_APP_Seq_SelAlarmOpinionAddAll

: 2023-07-31 by shpark1325-----------------------------------------------------------
[전자결재]수신부서 결재자 결재취소 후 상태값 진행으로 나오도록 수정
	\DeskPlusEIP\Approval\Ws\Status\ListRelated.asmx.cs

: 2023-06-09 by ghtjd7794-----------------------------------------------------------
[전자결재] 결재진행상황 팝업창 크기 조절
	\DeskPlusEIP\Approval\Status\Common\GetDocFlow.aspx
	\DeskPlusEIP\Approval\Js\Home\Content.js
	\DeskPlusEIP\Approval\Js\Home\Search.js
	\DeskPlusEIP\Approval\Js\Status\ListAfterward.js
	\DeskPlusEIP\Approval\Js\Status\ListApprove.js
	\DeskPlusEIP\Approval\Js\Status\ListApproveSlip.js
	\DeskPlusEIP\Approval\Js\Status\ListComplete.js
	\DeskPlusEIP\Approval\Js\Status\ListCompleteSlip.js
	\DeskPlusEIP\Approval\Js\Status\ListConfirm.js
	\DeskPlusEIP\Approval\Js\Status\ListDelay.js
	\DeskPlusEIP\Approval\Js\Status\ListDelaySlip.js
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\Approval\Js\Status\ListProcessSlip.js
	\DeskPlusEIP\Approval\Js\Status\ListProxy.js
	\DeskPlusEIP\Approval\Js\Status\ListReceive.js
	\DeskPlusEIP\Approval\Js\Status\ListReceiveReject.js
	\DeskPlusEIP\Approval\Js\Status\ListReject.js
	\DeskPlusEIP\Approval\Js\Status\ListRejectSlip.js
	\DeskPlusEIP\Approval\Js\Status\ListRelated.js
	\DeskPlusEIP\Approval\Js\Status\ListRelatedSlip.js
	\DeskPlusEIP\Approval\Js\Status\ListTransfer.js

: 2023-05-23 by shpark1325-----------------------------------------------------------
[전자결재]반려 시 반려대상자 초기 지정되지 않도록 처리 + 반려대상 선택안했을 경우 알림창
	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx.cs
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js

: 2023-05-09 by shpark1325-----------------------------------------------------------
[나의정보] 패스워드 정책 추가(초기패스워드)
	\DeskPlusEIP\Account\MyInfo\Password\Change.aspx
	\DeskPlusEIP\Account\MyInfo\Password\Change.aspx.cs
	\DeskPlusEIP\Account\Js\MyInfo\Password\Change.js

[로그인]초기패스워드 + 문자 입력 시 변경되지 않도록 요청
	\DeskPlusEIP\Js\Site\Auth\ModPassword.js

: 2023-05-08 by shpark1325-----------------------------------------------------------
[로그인] 패스워드 변경 팝업창 문구 변경, 패스워드 정책 추가(초기패스워드)
	\DeskPlusEIP\Site\Auth\ModPassword.aspx
	\DeskPlusEIP\Site\Auth\ModPassword.aspx.cs
	\DeskPlusEIP\Js\Site\Auth\ModPassword.js

: 2023-03-20 by ghtjd7794-----------------------------------------------------------
[전자결재] 결재칸 추가 후 결재자 지정없이 상신안되도록 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

: 2023-02-20 by shpark1325-----------------------------------------------------------
[로그인]패스워드 변경 팝업 창 크기 변경요청
	\DeskPlusEIP\Site\Auth\ModPassword.aspx
	\DeskPlusEIP\Js\Site\Auth\Default.js

: 2023-02-17 by famo23 --------------------------------------------------------
[전자결재] 최종결재자는 전결버튼이 안 나오게 처리
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs

[전자결재] 홈화면에서 상신문서카운트를 진행중인문서로 변경
	DP_APP_Home_TodayCnt
	\DeskPlusEIP\Approval\Js\Home\Menu.js

[전자결재] 홈화면에서 결재차례가 오지 않은 문서 보이지 않게 처리
	DP_APP_Home_ListApprove
	DP_APP_Home_ListTotal

: 2023-02-16 by shpark1325-----------------------------------------------------------
[포탈]화면 문구 수정
	\DeskPlusEIP\Site\Auth\Default.aspx

: 2023-02-15 by shpark1325-----------------------------------------------------------
[포탈]화면 문구 수정
	\DeskPlusEIP\Site\Auth\Default.aspx

: 2023-01-27 by ghtjd7794-----------------------------------------------------------
[전자결재] 구매요청서 수신자가 본문/결재선 수정시 결재의견 필수값 체크 해제요청
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js

: 2023-01-26 by shpark1325 --------------------------------------------------------
[전자결재]임시저장 첨부파일명 &기호 오류 수정
	DP_APP_DraftFile_Ins

: 2022-12-28 by shpark1325-----------------------------------------------------------
[전자결재] 협조자가 본문/결재선 수정시 결재의견 필수값 체크 해제요청
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js

: 2022-12-22 by ghtjd7794-----------------------------------------------------------
[전자결재] 후열 처리 시 원결재자 정보로 표시되도록 수정(직급오류로 인한 재수정)
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx.cs

: 2022-11-17 by kkzz52 --------------------------------------------------------
[전자결재] 후열 처리 시 원래 결재자 이름으로 표시
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js

: 2022-11-16 by kkzz52 ------------------------------------------------------
[전자결재] 폐기문서함 리스트에 체크박스 추가
	\DeskPlusEIP\Approval\Js\Disuse\List.js
	\DeskPlusEIP\Approval\Disuse\List.aspx
	\DeskPlusEIP\Approval\Disuse\List.aspx.cs
	\DeskPlusEIP\Approval\Jt\Disuse\JTList.htm
	\DeskPlusEIP\Approval\Ws\Disuse\View.asmx.cs

: 2022-11-03 by freeice122 ------------------------------------------------------
[전자결재] 관리자 문서리스트에 documentID추가
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Admin\Document\List.aspx
	\DeskPlusEIP\Approval\Admin\Document\ListCom.aspx
	\DeskPlusEIP\Approval\Admin\DocumentEtc\JList.aspx
	\DeskPlusEIP\Approval\Disuse\List.aspx

	\DeskPlusEIP\Approval\Js\Admin\Document\JListCom.js
	\DeskPlusEIP\Approval\Js\Admin\Document\JList.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ListCom.js
	\DeskPlusEIP\Approval\Js\Admin\Document\JListCom.js
	\DeskPlusEIP\Approval\Js\Disuse\List.js

	\DeskPlusEIP\Approval\Jt\Admin\Document\JList.htm
	\DeskPlusEIP\Approval\Jt\Admin\Document\JListProcess.htm
	\DeskPlusEIP\Approval\Jt\Admin\DocumentEtc\JList.htm
	\DeskPlusEIP\Approval\Jt\Admin\DocumentEtc\JListProcess.htm
	\DeskPlusEIP\Approval\Jt\Disuse\JList.htm
	
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx

[전자결재] 임시저장/수신/상신문서에 결재선추가/삭제 버튼 추가
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs

	\DeskPlusEIP\Approval\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.js

: 2022-11-01 by kkzz52 ------------------------------------------------------
[전자결재] 에디터에 결재 css 추가되는 오류 수정 (에디터 말머리 깨짐)
	\DeskPlusEIP\Approval\FormFlow\Basic\EditorNonActiveX.aspx

: 2022-10-31 by kkzz52 ---------------------------------------------------------
[보안취약 수정] CSRF 대응 조치
	쿠키값을 이용한 CSRF (교차 사이트 요청 위조) 공격 방지 코드 추가
	\DeskPlusEIP\Upload\JSUpload\js\main.js
	\DeskPlusEIP\Upload\JSUpload\Main.aspx
	\DeskPlusEIP\Upload\JSUpload\Main.aspx.cs
	\DeskPlusEIP\Upload\JSUpload\Default.aspx
	\DeskPlusEIP\Upload\JSUpload\Default.aspx.cs
	\DeskPlusEIP\EIP\Js\Package\NMain.js
	\DeskPlusEIP\EIP\Package\NMain.aspx
	\DeskPlusEIP\EIP\Package\NMain.aspx.cs
	\DeskPlusEIP\Js\Site\Auth\Default.js
	\DeskPlusEIP\Site\Auth\Default.aspx
	\DeskPlusEIP\Site\Auth\Default.aspx.cs

: 2022-10-24 by freeice122 ------------------------------------------------------
[전자결재] 열람등급 ()에 대해 ReversXSS적용
	\DeskPlusEIP\Approval\Admin\AccessGrade\List.aspx.cs
	\DeskPlusEIP\Approval\Admin\AccessGrade\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceive.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveEdit.aspx.cs
	\DeskPlusEIP\Approval\Ws\Admin\AccessGrade\List.asmx.cs
	\DeskPlusEIP\Approval\Ws\Admin\AccessGrade\Mod.asmx.cs


: 2022-10-21 by kkzz52 ------------------------------------------------------
[전자결재] 결재망 추가 후 수신에서 내용 수정 가능하게 처리
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

: 2022-10-20 by kkzz52 ------------------------------------------------------
[전자결재] 결재자선택 검색창으로 커서 이동 시 자주 지정하는 결재자 리스트 팝업창 사라지도록 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js

: 2022-10-20 by kkzz52 ------------------------------------------------------
[전자결재] 상신취소 알람 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

	DP_APP_Seq_SelAlarmCancel 추가

: 2022-10-18 by kkzz52 ------------------------------------------------------
[전자결재] 결재 팝업에서 승인 시 결재상황조회로 이동되도록 수정
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx

: 2022-10-18 by kkzz52 ------------------------------------------------------
[전자결재] 상신취소 시 알람 링크에서 오류 페이지 안가도록 수정
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx.cs

: 2022-10-17 by kkzz52 ------------------------------------------------------
[전자결재] 폐기문서 영구삭제 버튼 추가
	\DeskPlusEIP\Approval\Js\Disuse\View.js
	\DeskPlusEIP\Approval\Disuse\View.aspx
	\DeskPlusEIP\Approval\Disuse\View.aspx.cs
	\DeskPlusEIP\Approval\Ws\Disuse\View.asmx.cs

: 2022-10-12 by kkzz52 --------------------------------------------------------
[전자결재] 다이렉트 호출 시 세부메뉴 호출 가능하게 수정
	\DeskPlusEIP\EIP\Js\Package\NMenu.js
	\DeskPlusEIP\Approval\Folder\Default.aspx.cs
	\DeskPlusEIP\Approval\Status\Default.aspx.cs

: 2022-10-12 by freeice122 --------------------------------------------------------
[전자결재] 양식관리자 권한 추가(formadmin)
	\DeskPlusEIP\Approval\Base\BasePageApprovalAdmin.cs
	\DeskPlusEIP\Approval\Base\BaseWSApproval.cs


: 2022-10-07 by kkzz52 --------------------------------------------------------
[전자결재] 오류수정 (수신 문서 결재자 보류 처리, 보류 해제 시 상신자 원안 품의 삭제 가능)
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs

: 2022-10-07 by kkzz52 --------------------------------------------------------
[전자결재] 결재 시 의견이 있을 경우 의견 메일발송
	\DeskPlusEIP\Approval\App_Data\Confirm\ko~.xml
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

	DP_APP_Seq_SelAlarmOpinionAddAll

: 2022-10-06 by kkzz52 -----------------------------------------------------------
[전자결재] 수신문서 상신보류 추가
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\Approval\Js\Status\ListProcessSlip.js

	DP_APP_DocProcess_UpdAppState

: 2022-10-05 by kkzz52 --------------------------------------------------------
[인사] 패스워드 변경 시 ERP 연동
	\DeskPlusEIP\App_Data\ACCOUNT.xml (ERPUse 추가)
	\DeskPlusEIP\Account\Ws\Admin\User\Password\Change.asmx.cs
	\DeskPlusEIP\Account\Ws\MyInfo\Password\Change.asmx.cs
	\DeskPlusEIP\Site\Auth\ModPassword.aspx.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BLL\SLOPwdChange.cs 추가
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\SLOPwdChange.cs 추가

: 2022-09-29 by kkzz52 --------------------------------------------------------
[게시판] 권한이 없는 카테고리는 안보이게 처리
	\DeskPlusEIP\Board\Common\BoardTreeScript.ascx.cs
	\DeskPlusEIP\Board\Common\GetBoardTreeScript.aspx.cs

: 2022-09-28 by kkzz52 -----------------------------------------------------------
[전자결재] 이관문서는 현재 수신문서 순서 체크 안하도록 처리
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

: 2022-09-28 by kkzz52 -----------------------------------------------------------
[전자결재] 수신문서 순서 체크 오류
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

: 2022-09-26 by kkzz52 -----------------------------------------------------------
[전자결재] 구 마이그레이션 문서는 보기 페이지 구분되도록 처리
	\DeskPlusEIP\Approval\Js\Admin\Document33\ViewDoc.js 추가
	\DeskPlusEIP\Approval\Js\Admin\Document33\ViewDocProc.js 추가
	\DeskPlusEIP\Approval\Js\Common\Popup33\ViewTot.js 추가
	\DeskPlusEIP\Approval\Js\Folder33\Common\ViewBaseDoc.js 추가
	\DeskPlusEIP\Approval\Admin\Document33\ViewDoc.aspx 추가
	\DeskPlusEIP\Approval\Admin\Document33\ViewDocProc.aspx 추가
	\DeskPlusEIP\Approval\Common\Popup33\ViewTot.aspx 추가
	\DeskPlusEIP\Approval\Folder33\Common\ViewBaseDoc.aspx 추가
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTAttachFile_Old.htm 추가
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTAttachFilePopup_Old.htm 추가
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTBaseDoc_Old.htm 추가
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTBaseDocPopup_Old.htm 추가
	\DeskPlusEIP\Approval\Jt\Status\Common\JTAttachFile_Old.htm 추가
	\DeskPlusEIP\Approval\Jt\Status\Common\JTBaseDoc_Old.htm 추가

	\DeskPlusEIP\Approval\Js\Common\Folder\ForwardList.js
	\DeskPlusEIP\Approval\Js\Folder\Common\DocUserSeq.js
	\DeskPlusEIP\Approval\Js\Folder\Common\HistoryDoc.js

	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx.cs

	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Js\Disuse\View.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js

	\DeskPlusEIP\Approval\Js\Admin\Document\JListCom.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\List.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardList.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\List.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\List.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\ReceiptList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\SendList.js
	\DeskPlusEIP\Approval\Js\Folder\Impo\List.js
	\DeskPlusEIP\Approval\Js\Folder\Private\List.js
	\DeskPlusEIP\Approval\Js\Folder\Private\ListFolder.js
	\DeskPlusEIP\Approval\Js\Home\Content.js
	\DeskPlusEIP\Approval\Js\Home\Search.js
	\DeskPlusEIP\Approval\Js\Status\ListComplete.js
	\DeskPlusEIP\Approval\Js\Status\ListCompleteSlip.js
	\DeskPlusEIP\Approval\Ws\Admin\Document\List.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Approve\List.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Approve\ListFolder.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ForwardList.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\List.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Dept\List.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Dept\ReceiptList.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Dept\SendList.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Impo\List.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Private\List.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Private\ListFolder.asmx.cs
	\DeskPlusEIP\Approval\Ws\Home\Content.asmx.cs
	\DeskPlusEIP\Approval\Ws\Home\Search.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListComplete.asmx.cs
	\DeskPlusEIP\Approval\Jt\Admin\Document\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\JTListFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTForwardList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTReceiptList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTSendList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Impo\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\JTListFolder.htm
	\DeskPlusEIP\Approval\Jt\Home\JTListDoc.htm
	\DeskPlusEIP\Approval\Jt\Home\JTListTab.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListComplete.htm

	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\WebComponent\ClientSmartButtonOld.cs 추가

	\Design\Site\Busangas\C~\Css\DeskPlusEIPBase_Old.css 추가

	DP_APP_DocProcess_SelPageListComplete
	DP_APP_DocProcess_SelPageListCompleteSlip
	DP_APP_HOME_SelPageListDocAll
	DP_EIP_Search_Scheduler

: 2022-09-05 by kkzz52 ----------------------------------------------------------
[공통] ViewStateUserKey 고정값으로 수정
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Base\BasePage.cs

: 2022-08-31 by kkzz52 --------------------------------------------------------
[전자결재] 진행중인문서를 근거문서로 첨부 가능하게 처리 (개인별 상신문서)
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\GetBaseDoc.js
	\DeskPlusEIP\Approval\Folder\Common\ViewBaseDoc.aspx
	\DeskPlusEIP\Approval\Folder\Common\ViewBaseDoc.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\GetBaseDoc.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\Ws\FormFlow\Basic\GetBaseDoc.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\PrivateDocument.cs

	-Approval 리소스 추가
	Msg_ErrBaseSameDoc

	VW_APP_DocTotAll 추가

	DP_APP_DocProcess_SelPageListPrivateAll 추가
	DP_APP_DocProcess_SelPageCntPrivateAll 추가
	DP_APP_BaseDoc_SelDocID
	DP_APP_DocProcess_UpdConclude
	DP_APP_DraftBaseDoc_Sel

: 2022-08-18 by kkzz52 ------------------------------------------------------
[전자결재] 문서보기 시 iframe -> xiframe 변경하는 부분 삭제 (연동문서에서 iframe 사용)
	\Projects\ApprovalProject\Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper\UtilHelper.cs

: 2022-08-18 by kkzz52 ------------------------------------------------------
[전자결재] 일괄결재 추가 (NickName = 'ERP_H')
	\DeskPlusEIP\Approval\Js\Status\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\Status\ListApprove.js
	\DeskPlusEIP\Approval\Js\Status\ListProxy.js
	\DeskPlusEIP\Approval\Js\Status\ListApproveSlip.js 추가
	\DeskPlusEIP\Approval\Js\Status\ListCompleteSlip.js 추가
	\DeskPlusEIP\Approval\Js\Status\ListDelaySlip.js 추가
	\DeskPlusEIP\Approval\Js\Status\ListProcessSlip.js 추가
	\DeskPlusEIP\Approval\Js\Status\ListRejectSlip.js 추가
	\DeskPlusEIP\Approval\Js\Status\ListRelatedSlip.js 추가
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPasswordBundle.aspx 추가
	\DeskPlusEIP\Approval\Status\ListApprove.aspx
	\DeskPlusEIP\Approval\Status\ListApprove.aspx.cs
	\DeskPlusEIP\Approval\Status\ListProxy.aspx
	\DeskPlusEIP\Approval\Status\ListProxy.aspx.cs
	\DeskPlusEIP\Approval\Status\ListApproveSlip.aspx 추가
	\DeskPlusEIP\Approval\Status\ListCompleteSlip.aspx 추가
	\DeskPlusEIP\Approval\Status\ListDelaySlip.aspx 추가
	\DeskPlusEIP\Approval\Status\ListProcessSlip.aspx 추가
	\DeskPlusEIP\Approval\Status\ListRejectSlip.aspx 추가
	\DeskPlusEIP\Approval\Status\ListRelatedSlip.aspx 추가
	\DeskPlusEIP\Approval\Common\Status\FolderStatusTreeScript.ascx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListApprove.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListComplete.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListDelay.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListProcess.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListReject.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\ListRelated.asmx.cs
	\DeskPlusEIP\Approval\Jt\Status\JTListApproveSlip.htm 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\BundleInfo.cs 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Bundle.cs 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs

	-리소스 추가
	MSG_SelBundle

	APP_Bundle 테이블 추가

	DP_APP_FolderStatus 일괄결재 리스트 정보 추가
	
	DP_APP_Bundle_Ins 추가
	DP_APP_DocProcess_SelPageCntProcessSlip 추가
	DP_APP_DocProcess_SelPageListProcessSlip 추가
	DP_APP_DocProcess_SelPageCntApproveSlip 추가
	DP_APP_DocProcess_SelPageListApproveSlip 추가
	DP_APP_DocProcess_SelPageCntRelatedSlip 추가
	DP_APP_DocProcess_SelPageListRelatedSlip 추가
	DP_APP_DocProcess_SelPageCntDelaySlip 추가
	DP_APP_DocProcess_SelPageListDelaySlip 추가
	DP_APP_DocProcess_SelPageCntCompleteSlip 추가
	DP_APP_DocProcess_SelPageListCompleteSlip 추가
	DP_APP_DocProcess_SelPageCntRejectSlip 추가
	DP_APP_DocProcess_SelPageListRejectSlip 추가

	DP_APP_DocProcess_SelPageCntProcess
	DP_APP_DocProcess_SelPageListProcess
	DP_APP_DocProcess_SelPageCntApprove
	DP_APP_DocProcess_SelPageListApprove
	DP_APP_DocProcess_SelPageCntRelated
	DP_APP_DocProcess_SelPageListRelated
	DP_APP_DocProcess_SelPageCntDelay
	DP_APP_DocProcess_SelPageListDelay
	DP_APP_DocProcess_SelPageCntComplete
	DP_APP_DocProcess_SelPageListComplete
	DP_APP_DocProcess_SelPageCntReject
	DP_APP_DocProcess_SelPageListReject	

: 2022-08-17 by kkzz52 ------------------------------------------------------
[전자결재] 특정사용자를 위한 결재문서리스트 추가
	- 문서보기는 없음
	- 진행중인문서 : 삭제
	- 완료문서 : 일반문서는 폐기
	\DeskPlusEIP\Approval\Js\Admin\DocumentEtc\JListCom.js 추가
	\DeskPlusEIP\Approval\Admin\DocumentEtc\JList.aspx 추가
	\DeskPlusEIP\Approval\Ws\Admin\DocumentEtc\List.asmx 추가
	\DeskPlusEIP\Approval\Jt\Admin\DocumentEtc\JTList.htm 추가
	\DeskPlusEIP\Approval\Jt\Admin\DocumentEtc\JTListProcess.htm 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Document.cs

	-권한관리 추가
	결재문서리스트관리자 approvalMng

: 2022-06-17 by famo23 ------------------------------------------------------
[전자결재] 결재의견추가시 알람버그 (프로시저에 CompanyID 값이 1로 하드코딩되어 있었음)
	DP_APP_Seq_SelAlarmOpinionAddAll

: 2022-06-14 by kkzz52 --------------------------------------------------------
[공통] 공휴일이 일요일일때 대체휴무 오류 수정
	\DeskPlusEIP\Common\Calendar\Calendar.ascx.cs
	\DeskPlusEIP\Common\Calendar\CalendarDialog.aspx.cs
	\DeskPlusEIP\Common\Calendar\CalendarDualDialog.aspx.cs
	\DeskPlusEIP\Common\Calendar\CalendarDualDialogTime.aspx.cs
	\DeskPlusEIP\Pim\Menu\PimMenu.aspx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx.cs
	\DeskPlusEIP\RM\Home\Main.aspx.cs
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx.cs
	\DeskPlusEIP\Community\Ws\Site\Apps\Schedule\List.asmx.cs
	\DeskPlusEIP\Portlet\Package\HR\Attendance.ascx.cs
	\DeskPlusEIP\Portlet\Package\MyPim.ascx.cs
	\DeskPlusEIP\Portlet\Package\PimList.ascx.cs
	\DeskPlusEIP\Portlet\Ws\Package\HR\Attendance.asmx.cs
	\DeskPlusEIP\PMS\Schedule\List.aspx.cs
	\DeskPlusEIP\ETC\Ws\Attendance\UserStatus.asmx.cs
	\DeskPlusEIP\Visit\Content\Tree.aspx.cs
	\DeskPlusEIP\Visit\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\HR\Ws\Content\ListPrivate.asmx.cs
	\DeskPlusEIP\HR\Ws\Content\ListPrivateMod.asmx.cs

: 2022-06-10 by kkzz52 ------------------------------------------------------------
[전자결재] ZQ수신에서 발신자에게 반려할 경우 결재홈에서 권한없음 오류 수정
	DP_APP_DocProcess_UpdConcludeR

: 2022-06-07 by kkzz52 ------------------------------------------------
[공통] JSUpload 순서변경(sortable) 되도록 적용
       JSUpload 업로드 시 동시업로드에서 순차적으로 업로드되도록 처리 (첨부파일이 랜덤으로 업로드되는 문제 때문)
	\DeskPlusEIP\Upload\JSUpload\js\jquery.fileupload.js
	\DeskPlusEIP\Upload\JSUpload\js\main.js
	\DeskPlusEIP\Upload\JSUpload\Main.aspx

: 2022-05-23 by kkzz52 ------------------------------------------------
[전자결재] 전자결재 문서 작성시 팝업 open
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeLegacy.js
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx.cs

	-코드 추가
	PopupApproval 전자결재-팝업작성

: 2022-05-17 by kkzz52 ------------------------------------------------
[Home] TodoList 팝업 삭제 및 Home 화면에 적용
	\DeskPlusEIP\EIP\Js\Package\NMain.js
	\DeskPlusEIP\EIP\Package\NMain.aspx
	\DeskPlusEIP\EIP\Package\NMain.aspx.cs
	\DeskPlusEIP\Account\Admin\User\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Portlet\Ws\TodoPopup\TodoList.asmx.cs
	\DeskPlusEIP\Portlet\Package\Clock\css\jClocksGMT.css
	\C\Css\DStyle\style.css

	-EIP 리소스 추가
	TodoAlert01
	TodoTitle
	TodoPopupMsg
	TodoItem01Sett
	TodoItem02Sett
	TodoItem03Sett
	TodoItem04Sett
	TodoItem05Sett
	TodoItem06Sett
	TodoItem07Sett
	TodoItem08Sett
	TodaySchedule
	CompleteDoc
	Task

	DP_PT_TodoPopup_PimScheduleList
	DP_PT_TodoPopup_PimTodoList

[근태] 이전부서 선택 시 부서이동 후 비활성화한 부서의 조회기간 조회 시 오류 수정
	\DeskPlusEIP\HR\Content\ListDept.aspx.cs
	\DeskPlusEIP\HR\Content\ListPrivate.aspx.cs
	\DeskPlusEIP\HR\Content\ListPrivateMod.aspx.cs

: 2022-05-16 by kkzz52 ---------------------------------------------------------
[전자결재] 결재자 없이 바로 완결 시 참조자에게 알람안가는 오류 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

: 2022-05-13 by famo23 ------------------------------------------------------
[포틀릿] 관리자 권한 없는 사용자한테 포틀릿삭제 버튼 안 나오게
	\DeskPlusEIP\Portlet\Js\Package\MyInfo.js      
	\DeskPlusEIP\Portlet\Js\Package\MyMemo.js       
	\DeskPlusEIP\Portlet\Js\Package\MyPim.js       
	\DeskPlusEIP\Portlet\Js\Package\Opinion.js 
	\DeskPlusEIP\Portlet\Js\Package\PimList.js      
	\DeskPlusEIP\Portlet\Js\Package\ResourceList.js  
	\DeskPlusEIP\Portlet\Js\Package\STA\PerformMon.js
	\DeskPlusEIP\Portlet\Js\Package\STA\PerformMonDisk.js
	\DeskPlusEIP\Portlet\Package\Approval\ListA.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListC.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListCir.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListF.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListR.ascx
	\DeskPlusEIP\Portlet\Package\Approval\Tab.ascx
	\DeskPlusEIP\Portlet\Package\Attendance.ascx    
	\DeskPlusEIP\Portlet\Package\Board\Album.ascx		
	\DeskPlusEIP\Portlet\Package\Board\Blog.ascx   
	\DeskPlusEIP\Portlet\Package\Board\CountTab.ascx
	\DeskPlusEIP\Portlet\Package\Board\GMWords.ascx
	\DeskPlusEIP\Portlet\Package\Board\List.ascx
	\DeskPlusEIP\Portlet\Package\Board\NoticeImage.ascx
	\DeskPlusEIP\Portlet\Package\Board\Tab.ascx    
	\DeskPlusEIP\Portlet\Package\Board\TopReader.ascx
	\DeskPlusEIP\Portlet\Package\Board\TopRegister.ascx
	\DeskPlusEIP\Portlet\Package\Congratulation.ascx 
	\DeskPlusEIP\Portlet\Package\HR\Attendance.ascx
	\DeskPlusEIP\Portlet\Package\Mail\List.ascx     
	\DeskPlusEIP\Portlet\Package\MyComment.ascx 
	\DeskPlusEIP\Portlet\Package\RM\RMManager.ascx 
	\DeskPlusEIP\Portlet\Package\RM\Tab.ascx   
	\DeskPlusEIP\Portlet\Package\SMSNew\SMS.ascx    
	\DeskPlusEIP\Portlet\Package\TabMain.ascx      
	\DeskPlusEIP\Portlet\Package\TabOpinion.ascx   
	\DeskPlusEIP\Portlet\Package\TabTaskMain.ascx   
	\DeskPlusEIP\Portlet\Package\Talk\Tab.ascx      
	\DeskPlusEIP\Portlet\Package\TodoList.ascx     
	\DeskPlusEIP\Portlet\Package\ToSchList.ascx    
	\DeskPlusEIP\Portlet\Package\Weather.ascx       

: 2022-05-13 by kkzz52 --------------------------------------------------------
[전자결재] 진행중인 ZP문서에 의견추가 시 다른 ZP문서까지 업데이트 되는 오류 수정
	DP_APP_DocProcess_UpdContent

: 2022-05-12 by famo23 ------------------------------------------------------
[전자결재] 회람 ZIP선택다운로드 안되는 버그 수정 
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTAttachFile.htm

[전자결재] 회람 디자인 수정
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTCirReadList.htm

: 2022-05-11 by famo23 ------------------------------------------------------
[포틀릿] 축하합니다 (날짜검색가능포틀릿)
	\DeskPlusEIP\Portlet\Js\Package\BirthDay.js
	\DeskPlusEIP\Portlet\Package\BirthDay.ascx

: 2022-05-10 by kkzz52 --------------------------------------------------------
[Home] 포틀릿 수정
	-바로가기 포틀릿 디자인 수정	
	-공지사항 탭포틀릿 디자인 수정
	-업무협업 포틀릿 디자인 수정
	-포토앨범 포틀릿 수정
	\DeskPlusEIP\Portlet\Package\QuickLink.ascx
	\DeskPlusEIP\Portlet\Package\Board\NoticeImage.ascx
	\DeskPlusEIP\Portlet\Package\Board\NoticeImage.ascx.cs	
	\DeskPlusEIP\Portlet\Jt\Package\Board\JTList.htm
	\DeskPlusEIP\Portlet\Jt\Package\Board\JTNoticeImage.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTForward.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTReceive.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTReference.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTRequest.htm
	\DeskPlusEIP\Portlet\Ws\Package\Board\Album.asmx.cs

	-Portlet 리소스
	Report 수정

	\C\Css\DStyle\style.css (portlet wslide 수정)

[게시판] 블로그, 갤러리 형태일 경우 ddlSearchTerm 초기값 오류 수정
	\DeskPlusEIP\Board\Content\BlogList.aspx.cs
	\DeskPlusEIP\Board\Content\BlogListImpo.aspx.cs
	\DeskPlusEIP\Board\Content\BlogListInterested.aspx.cs
	\DeskPlusEIP\Board\Content\GalleryList.aspx.cs
	\DeskPlusEIP\Board\Content\GalleryListImpo.aspx.cs
	\DeskPlusEIP\Board\Content\GalleryListInterested.aspx.cs
	\DeskPlusEIP\Board\Content\ListImpo.aspx.cs
	\DeskPlusEIP\Board\Content\ListInterested.aspx.cs

: 2022-05-02 by famo23 ------------------------------------------------------
[포틀릿] 일정포틀릿에서 주말,공휴일 빨간색 표시 버그수정
	\C\Css\DStyle\style.css

: 2022-04-28 by famo23 ------------------------------------------------------
[업무일지] 디자인깨지는 부분 수정
	\DeskPlusEIP\Report\Content\FolderAdd.aspx.cs

[업무일지] 팝업창에서 alert창이 짤려서 가로사이즈 넓히기처리 (최소 500)
	\DeskPlusEIP\Report\Js\Content\FolderList.js

: 2022-04-22 by kkzz52 --------------------------------------------------------
[전자결재] 크롬에서 beforeunload Viewing 처리
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Basic\SetViewing.aspx 추가

[전자결재] 승인 시 현재 결재 순서인지 체크되도록 적용 (중복 결재방지) 
    현재 순서가 아니면 보기 페이지로 이동
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Ws\Process\Basic\Approve.asmx.cs

	-Approval 리소스 추가
	MyTurnAppState

[전자결재] 메신저 알람에서 결재할때 상신보류 상태인지 체크되도록 수정
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Ws\Process\Basic\Approve.asmx.cs

	-Approval 리소스 추가
	DelayAppState

[전자결재] 수신 결재순서가 아니면 리스트 페이지로 이동
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\Ws\FormFlow\Receive\Add.asmx 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CounterPartUser.cs

	-Approval 리소스 추가
	MyTurnSusin

	DP_APP_CounterPartUser_SelMyTurn 추가	

[설문조사] 익명사용 추가, 응답자공개 -> 참여자공개로 변경
	: 결과공개 - 설문결과 노출
	: 참여자공개 - 사용자: 공개 설정시에만 조회 / 관리자는 설정과 관계 없이 응답자 조회가능
	: 익명사용 - 설정시 사용자 및 관리자 모두 응답자 조회 불가 / 익명사용시 응답자 공개 선택 불가 ( 신규 추가 옵션 )
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesAdd.js
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesMod.js
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesResult.js
	\DeskPlusEIP\Opinion\Js\Content\FavList.js
	\DeskPlusEIP\Opinion\Js\Content\List.js
	\DeskPlusEIP\Opinion\Js\Content\Result.js
	\DeskPlusEIP\Opinion\Admin\Content\QuesAdd.aspx
	\DeskPlusEIP\Opinion\Admin\Content\QuesAddN.aspx
	\DeskPlusEIP\Opinion\Admin\Content\QuesAdd.cs
	\DeskPlusEIP\Opinion\Admin\Content\QuesMod.aspx
	\DeskPlusEIP\Opinion\Admin\Content\QuesModN.aspx
	\DeskPlusEIP\Opinion\Admin\Content\QuesMod.cs
	\DeskPlusEIP\Opinion\Admin\Content\QuesResult.aspx
	\DeskPlusEIP\Opinion\Admin\Content\QuesResult.aspx.cs
	\DeskPlusEIP\Opinion\Admin\Content\QuesView.aspx
	\DeskPlusEIP\Opinion\Admin\Content\QuesView.aspx.cs
	\DeskPlusEIP\Opinion\Content\FavList.aspx
	\DeskPlusEIP\Opinion\Content\List.aspx
	\DeskPlusEIP\Opinion\Content\Result.aspx
	\DeskPlusEIP\Opinion\Content\Result.aspx.cs
	\DeskPlusEIP\Opinion\Content\ResultPopup.aspx
	\DeskPlusEIP\Opinion\Content\ResultPopup.aspx.cs
	\DeskPlusEIP\Opinion\Content\Vote.aspx
	\DeskPlusEIP\Opinion\Content\Vote.cs
	\DeskPlusEIP\Opinion\Content\VotePopup.aspx
	\DeskPlusEIP\Opinion\Content\VotePopup.cs
	\DeskPlusEIP\Opinion\Ws\Content\FavList.asmx.cs
	\DeskPlusEIP\Opinion\Ws\Content\List.asmx.cs
	\DeskPlusEIP\Opinion\Jt\Content\JTFavList.htm
	\DeskPlusEIP\Opinion\Jt\Content\JTFavListBlog.htm
	\DeskPlusEIP\Opinion\Jt\Content\JTList.htm
	\DeskPlusEIP\Opinion\Jt\Content\JTListBlog.htm
	\Projects\OpinionProject\Zeospace.DeskPlus.Opinion.BE\QuesInfo.cs
	\Projects\OpinionProject\Zeospace.DeskPlus.Opinion.DAL\Ques.cs

	-Opinion 리소스 추가
	VoteUser
	Secret
	TitSecret

	DP_OPI_Ques 테이블 IsSecret 필드 추가

	DP_OPI_Ques_Ins
	DP_OPI_Ques_Sel
	DP_OPI_Ques_Upd
	DP_OPI_Ques_SelUserPageList
	DP_OPI_Fav_SelPage

: 2022-04-22 by famo23 ---------------------------------------------------------
[공통] 대체휴일사용여부 패키지 DB 기본값변경
	DP_PIM_HoliDay

: 2022-04-08 by kkzz52 ------------------------------------------------------------
[설문조사] 관리자 설문결과에서 항목별 투표자 명단 볼 수 있도록 추가
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesResult.js
	\DeskPlusEIP\Opinion\Js\Admin\Content\VoteList.js 추가
	\DeskPlusEIP\Opinion\Admin\Content\VoteList.aspx 추가
	\DeskPlusEIP\Opinion\Ws\Admin\Content\VoteList.asmx 추가
	\DeskPlusEIP\Opinion\Jt\Admin\Content\JtVoteList.htm 추가
	\Projects\OpinionProject\Zeospace.DeskPlus.Opinion.DAL\Vote.cs

	DP_OPI_Vote_SelVoteListAdmin 추가
	DP_OPI_Vote_SelVoteCntAdmin 추가
	DP_OPI_Vote_SelVoteListAdminC 추가
	DP_OPI_Vote_SelVoteCntAdminC 추가
	DP_OPI_Vote_SelVoteListAdminD 추가
	DP_OPI_Vote_SelVoteCntAdminD 추가

: 2022-04-07 by kkzz52 --------------------------------------------------------
[공통] 크롬에서 인쇄 시 배경그래픽 기본 적용되도록 처리
	\DeskPlusEIP\Util\Print\Print.css 추가

	\DeskPlusEIP\Approval\Js\Common\Popup\Print.js
	\DeskPlusEIP\Approval\Common\Popup\Print.aspx

	\DeskPlusEIP\Board\Js\Admin\Content\View.js
	\DeskPlusEIP\Board\Js\Content\View.js
	\DeskPlusEIP\Board\Js\Content\ViewImpo.js
	\DeskPlusEIP\Board\Js\Content\ViewInterested.js
	\DeskPlusEIP\Board\Js\Temp\View.js
	\DeskPlusEIP\Board\Admin\Content\View.aspx
	\DeskPlusEIP\Board\Content\View.aspx
	\DeskPlusEIP\Board\Content\ViewImpo.aspx
	\DeskPlusEIP\Board\Content\ViewInterested.aspx
	\DeskPlusEIP\Board\Content\ViewPopup.aspx
	\DeskPlusEIP\Board\Content\ViewPopupImpo.aspx
	\DeskPlusEIP\Board\Content\ViewPopupMain.aspx
	\DeskPlusEIP\Board\Temp\View.aspx

	\DeskPlusEIP\Task\Js\Admin\Content\View.js
	\DeskPlusEIP\Task\Js\Common\ViewPopup.js
	\DeskPlusEIP\Task\Js\Common\ViewPopupTotal.js
	\DeskPlusEIP\Task\Js\Content\Forward\View.js
	\DeskPlusEIP\Task\Js\Content\Receive\View.js
	\DeskPlusEIP\Task\Js\Content\Reference\View.js
	\DeskPlusEIP\Task\Js\Content\Request\View.js
	\DeskPlusEIP\Task\Js\Content\Total\View.js
	\DeskPlusEIP\Task\Js\Home\View.js
	\DeskPlusEIP\Task\Admin\Content\View.aspx
	\DeskPlusEIP\Task\Common\ViewPopup.aspx
	\DeskPlusEIP\Task\Common\ViewPopupTotal.aspx
	\DeskPlusEIP\Task\Common\ViewPopupTotalN.aspx
	\DeskPlusEIP\Task\Content\Forward\View.aspx
	\DeskPlusEIP\Task\Content\Forward\ViewN.aspx
	\DeskPlusEIP\Task\Content\Receive\View.aspx
	\DeskPlusEIP\Task\Content\Receive\ViewN.aspx
	\DeskPlusEIP\Task\Content\Reference\View.aspx
	\DeskPlusEIP\Task\Content\Reference\ViewN.aspx
	\DeskPlusEIP\Task\Content\Request\View.aspx
	\DeskPlusEIP\Task\Content\Request\ViewN.aspx
	\DeskPlusEIP\Task\Content\Total\View.aspx
	\DeskPlusEIP\Task\Content\Total\ViewN.aspx
	\DeskPlusEIP\Task\Home\View.aspx
	\DeskPlusEIP\Task\Home\ViewN.aspx

	\DeskPlusEIP\Pim\Js\Anniversary\view.js
	\DeskPlusEIP\Pim\Js\Schedule\View.js
	\DeskPlusEIP\Pim\Js\ToDo\View.js
	\DeskPlusEIP\Pim\Anniversary\view.aspx
	\DeskPlusEIP\Pim\Schedule\View.aspx
	\DeskPlusEIP\Pim\Schedule\ViewPopup.aspx
	\DeskPlusEIP\Pim\ToDo\View.aspx
	\DeskPlusEIP\Pim\ToDo\ViewPopup.aspx

	\DeskPlusEIP\RM\Js\Admin\Content\ListQRCode.js
	\DeskPlusEIP\RM\Js\Admin\Content\View.js
	\DeskPlusEIP\RM\Js\Admin\Reserve\View.js
	\DeskPlusEIP\RM\Js\Content\ListMain.js
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Js\Content\ViewTotal.js
	\DeskPlusEIP\RM\Admin\Content\ListQRCode.aspx
	\DeskPlusEIP\RM\Admin\Content\View.aspx
	\DeskPlusEIP\RM\Admin\Reserve\View.aspx
	\DeskPlusEIP\RM\Content\ListMain.aspx
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Content\ViewTotal.aspx

	\DeskPlusEIP\PMS\Js\Board\View.js
	\DeskPlusEIP\PMS\Js\Unit\View.js
	\DeskPlusEIP\PMS\Board\View.aspx
	\DeskPlusEIP\PMS\Board\ViewPopup.aspx
	\DeskPlusEIP\PMS\Unit\View.aspx

	\DeskPlusEIP\Address\Js\Content\Print.js
	\DeskPlusEIP\Address\Content\Print.aspx

	\DeskPlusEIP\Community\Js\Site\Apps\Board\View.js
    \DeskPlusEIP\Community\Js\Site\Apps\Schedule\View.js
	\DeskPlusEIP\Community\Site\Apps\Board\View.aspx
	\DeskPlusEIP\Community\Site\Apps\Board\ViewPopup.aspx
    \DeskPlusEIP\Community\Site\Apps\Schedule\View.aspx
	\DeskPlusEIP\Community\Site\Apps\Schedule\ViewPopup.aspx

	\DeskPlusEIP\Report\Js\Content\View.js
	\DeskPlusEIP\Report\Js\Content\ViewPT.js
	\DeskPlusEIP\Report\Content\View.aspx
	\DeskPlusEIP\Report\Content\ViewPopup.aspx
	\DeskPlusEIP\Report\Content\ViewPT.aspx

	\DeskPlusEIP\Visit\Js\Content\ListMain.js
	\DeskPlusEIP\Visit\Js\Content\View.js
	\DeskPlusEIP\Visit\Content\ListMain.aspx
	\DeskPlusEIP\Visit\Content\View.aspx

	\DeskPlusEIP\Proposal\Js\Content\Admin\ResultView.js
	\DeskPlusEIP\Proposal\Js\Content\Judge\JudgeView.js
	\DeskPlusEIP\Proposal\Js\Content\View.js
	\DeskPlusEIP\Proposal\Js\Content\Write.js
	\DeskPlusEIP\Proposal\Content\Admin\ResultView.aspx
	\DeskPlusEIP\Proposal\Content\Judge\JudgeView.aspx
	\DeskPlusEIP\Proposal\Content\View.aspx
	\DeskPlusEIP\Proposal\Content\Write.aspx

: 2022-03-18 by kkzz52 --------------------------------------------------------
[전자결재] 수발자관리, 결재자관리에서 퇴사자도 나오게 처리 (삭제할 수 있도록)
	DP_APP_Account_SelList 수정
	DP_APP_Account_SelListCnt 수정

: 2022-03-17 by kkzz52 -----------------------------------------------------------
[전자결재] 반려문서 재상신 시 고정수신 태그 class ZQ로 변경 안되는 오류 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js

: 2022-03-17 by kkzz52 -----------------------------------------------------------
[전자결재] 문서작성(상신문서함) : 부서로 지정한 참조가 지정 안되는 오류 수정
	DP_APP_SeqBackup_SelByLine

: 2022-03-17 by kkzz52 -----------------------------------------------------------
[전자결재] 문서작성(상신문서함) : ZQ문서도 나오게 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs

	DP_APP_Doc_SelPageListPrivateSubmit
	DP_APP_Doc_SelPageCntPrivateSubmit

: 2022-03-16 by kkzz52 -----------------------------------------------------------
[근태] 부서현황정보, 근태정보수정에서 속도 개선
	\DeskPlusEIP\HR\Content\ListDept.aspx.cs
	\DeskPlusEIP\HR\Content\ListPrivateMod.aspx.cs

	DP_HR_Role_Sel
	DP_HR_AttendanceRole_Sel

: 2022-01-27 by famo23 ---------------------------------------------------------
[팝업관리] 수정이 안되는 버그 
	\DeskPlusEIP\ETC\PopUp\Mod.aspx.cs

: 2022-01-20 by kkzz52 -----------------------------------------------------------
[전자결재] 결재의견 보기 추가
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Common\Popup\OpinionView.js 추가
	\DeskPlusEIP\Approval\Js\Common\Popup\View.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewCir.aspx
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx.cs
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx.cs
	\DeskPlusEIP\Approval\Common\Popup\OpinionView.aspx 추가
	\DeskPlusEIP\Approval\Common\Popup\View.aspx
	\DeskPlusEIP\Approval\Common\Popup\View.aspx.cs
	\DeskPlusEIP\Approval\Common\Popup\ViewCir.aspx
	\DeskPlusEIP\Approval\Common\Popup\ViewCir.aspx.cs
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx.cs
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx.cs
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Folder\Common\ViewBaseDoc.aspx
	\DeskPlusEIP\Approval\Folder\Common\ViewBaseDoc.aspx.cs
	\DeskPlusEIP\Approval\Status\Common\View.aspx
	\DeskPlusEIP\Approval\Status\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs
	\DeskPlusEIP\Approval\Process\Confirm\Approve.aspx
	\DeskPlusEIP\Approval\Process\Confirm\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Confirm\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Process\Confirm\ApprovePopup.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\Ws\Common\Popup\OpinionView.asmx 추가
	\DeskPlusEIP\Approval\Jt\Common\Popup\JTOpinionView.htm 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ShortReply.cs

	- Web.config 비인증 추가
	Approval/Common/Popup/OpinionView.aspx
	Approval/Ws/Common/Popup/OpinionView.asmx

	DP_APP_ShortReply_Sel 추가
	DP_APP_ShortReply_Copy 추가

: 2022-01-12 by famo23 ---------------------------------------------------------
[근태관리] 엑셀업로드로 근태등록시 반차가 등록안되는 오류수정
	\DeskPlusEIP\ETC\Ws\Holiday\ListAdmin.asmx.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BE\HoliAdminInfo.cs

: 2022-01-07 by shpark1325 ------------------------------------------------------------
[전자결재]문서이관 관련 오류 수정
- 프로시저	
	DP_APP_AppDoc_InsTransfer
	DP_APP_ArbitraryDoc_InsTransfer
	DP_APP_CarbonCopyUser_InsTransfer
	DP_APP_CarbonCopyUser_InsTransferDept
	DP_APP_CirDoc_InsTransfer
	DP_APP_CirUser_InsTransfer
	DP_APP_ForwardUser_InsTransfer
	DP_APP_PrivateDoc_InsTransfer
	DP_APP_PublicDoc_InsTransfer

: 2022-01-03 by shpark1325 ------------------------------------------------------------
[전자결재]참조문서함 전체복사 시 오류 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CarbonCopyUser.cs

: 2021-12-22 by ghtjd7794 ---------------------------------------------------------
[전자결재] 개인보관함-상신문서함에서 임시저장 가능하도록 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx

: 2021-12-10 by kkzz52 ------------------------------------------------------------
[전자결재] 수신회수 기능 추가
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\Approval\Status\ListProcess.aspx
	\DeskPlusEIP\Approval\Ws\Status\ListProcess.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CounterPartUser.cs

	-Approval 리소스 추가
	CancelSusin
	ConfirmCancelSusin

	DP_APP_CounterPartUser_UpdCancel 추가

	DP_QUERY ( QRY_APP_CounterPartUser_SelByApprovalTag 추가 )

: 2021-12-10 by kkzz52 ------------------------------------------------------------
[전자결재] 근거문서 보기 시 결재정보 팝업에서 참조정보 안나오는 오류 수정
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js

: 2021-11-24 by freeice122 ----------------------------------------------------------
[메인화면]태블릿에서 화면 짤림 수정	
	\DeskPlusEIP\EIP\DeskPlusEPDefault.aspx

: 2021-11-23 by shpark1325 ----------------------------------------------------------
[자원예약]일간 List 누락 오류 수정요청
	\DeskPlusEIP\RM\Ws\Content\ListDay.asmx.cs

: 2021-11-22 by ghtjd7794------------------------------------------------------------
[전자결재] 관리자 통계-월별/기간별현황 크롬에서 엑셀 다운로드 오류 수정
	\DeskPlusEIP\Approval\Admin\Statistics\ListMonthExcel.aspx.cs

: 2021-11-22 by shpark1325 ------------------------------------------------------------
[전자결재] 참조문서함 복사이관 시 이관대상자에게 해당 문서가 있을경우 오류 수정
	DP_APP_CarbonCopyUser_InsTransfer

[전자결재]문서이관-사용자별 복사 이관 시 쿼리 수정
	DP_APP_PrivateDoc_InsTransfer

: 2021-11-19 by kkzz52 --------------------------------------------------------
[전자결재] 결재선저장한 정보를 불러올때 변경된 결재자 리스트 체크 시 수신정보가 체크 안되는 오류 수정
	\DeskPlusEIP\Approval\Ws\FormFlow\Basic\Add.asmx.cs

: 2021-11-17 by kkzz52 --------------------------------------------------------
[전자결재] 후열문서가 반려되었을 경우 결재정보 팝업 오류 수정
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js

: 2021-11-08 by kkzz52 ------------------------------------------------------------
[전자결재] EDMS 이관 시 상신자 정보 null 오류 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Transfer.cs

: 2021-11-05 by kkzz52 --------------------------------------------------------
[전자결재] 중간결재자에게 반려된 문서일때 결재정보조회가 안되는 오류 수정
	\DeskPlusEIP\Approval\Ws\Status\Common\GetDocFlow.asmx.cs

: 2021-10-05 by kkzz52 --------------------------------------------------------
[전자결재] 수발자관리에서 수발자 지정 시 겸직자일 경우 기본부서로 저장되는 오류 수정
	DP_APP_Account_Ins

: 2021-08-20 by famo23 ---------------------------------------------------------
[전자결재] 반려문서함에서 검색이 안되는 버그 수정
	DP_APP_DocProcess_SelPageCntReject
	DP_APP_DocProcess_SelPageListReject

[전자결재] 이관할때 이관부서에 이관받는 사람 부서정보가 들어간 버그 수정
	DP_APP_PassedDoc_InsByDoc

: 2021-08-05 by kkzz52 ---------------------------------------------------------
[전자결재] 메신저 알람 링크 오류 수정 (참조인데 수신 링크로 가는 문제)
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

: 2021-07-27 by famo23 ---------------------------------------------------------
[전자결재] 양식에 참조추가할때 부서검색해서는 추가 안되는 버그수정
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDeptUserSelect.js

[전자결재] 임시저장한 문서중에 부서변경되었을 경우 초기화 안되는 버그수정
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js

: 2021-07-14 by shpark1325 ------------------------------------------------------------
[전자결재]에디터 영역 수정가능하도록 처리
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx.cs

: 2021-07-14 by shpark1325 ------------------------------------------------------------
[에디터]엑스프리 하단 드래그 드롭 크기조절 설정변경
	\DeskPlusEIP\Data\Editor\XFE2\config\env.xml

[에디터]메뉴정리
	\DeskPlusEIP\Data\Editor\XFE2\config\ko_ver30.xml
	\DeskPlusEIP\Data\Editor\XFE2\config\en_ver30.xml
	\DeskPlusEIP\Data\Editor\XFE2\config\jp_ver30.xml
	\DeskPlusEIP\Data\Editor\XFE2\config\cn_t_ver30.xml
	\DeskPlusEIP\Data\Editor\XFE2\config\cn_s_ver30.xml

: 2021-07-12 by kkzz52 ---------------------------------------------------------
[전자결재] 열람권한 조회 시 수신완료 문서 체크 안되는 오류 수정
	DP_APP_Doc_SelAuthDoc

: 2021-07-07 by kkzz52 ---------------------------------------------------------
[게시판] 겸직 부서가 비활성화 되어있을 경우 게시판 권한 가져올때 오류 발생되는 현상 수정
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Category.cs

: 2021-07-01 by kkzz52 ---------------------------------------------------------
[전자결재] 수신문서 반려 시 패스워드 사용안함인데 패스워드 체크하는 오류 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ConfirmReject.js

: 2021-03-18 by kkzz52 ---------------------------------------------------------
[공통] 직급/직책관리에서 직책일 경우 열람등급 선택 안되도록 수정
	\DeskPlusEIP\Account\Js\Admin\Position\List.js
	\DeskPlusEIP\Account\Admin\Position\List.aspx
	\DeskPlusEIP\Account\Admin\Position\List.aspx.cs

: 2021-03-17 by famo23 ---------------------------------------------------------
[일정관리] 일정수정,삭제할때 알람도 같이 수정

	필드추가
	DP_ALARM_Common
	DPM_ALARM_Common
	DPM_ALARM_CommonIOS 
	DP_MDM_NotificationBox

	TRI_Alarm_Common_Insert

	SQL_DP_ALARM_COMMON_INSERT	
	SQL_DP_ALARM_Common_DelPim
	SQL_DP_ALARM_Common_DelPimAll
	SQL_DP_ALARM_Common_DelPimDate

	QRY_PIM_DISPLAY_SelID

	\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\AlarmInfo.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\Alarm.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.DAL\AlarmCommon.cs
	\DeskPlusEIP\Pim\Js\Schedule\ListDay.js
	\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js
	\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
	\DeskPlusEIP\Pim\Schedule\Add.aspx.cs
	\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\AddSimple.asmx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\Schedule.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.DAL\Alarm.cs

: 2021-03-02 by famo23 ---------------------------------------------------------
[공통] 팝업창에서 alert창이 짤려서 가로사이즈 넓히기처리 (최소 500)
	\DeskPlusEIP\EIP\Js\Admin\Portal\AddTab.js
	\DeskPlusEIP\EIP\Js\Admin\Portal\GetTab.js
	\DeskPlusEIP\EIP\Js\Package\NMenu.js
	\DeskPlusEIP\EIP\Js\Portal\AddTab.js
	\DeskPlusEIP\EIP\Js\Portal\GetTabUser.js
	\DeskPlusEIP\HR\Js\Admin\Content\WorkAdmin.js
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesAdd.js
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesMod.js
	\DeskPlusEIP\PMS\Js\Project\Menu.js
	
: 2021-02-26 by kkzz52 ---------------------------------------------------------
[전자결재] 특근보고서에서 특근내역 근거문서 자동추가 시 디자인 수정
	\DeskPlusEIP\ETC\Js\ETCLegacy\WorkList.js
	\DeskPlusEIP\ETC\ETCLegacy\WorkList.aspx
	\DeskPlusEIP\ETC\Ws\ETCLegacy\WorkList.asmx.cs

	- ETC 리소스 추가
	DocNumber
	FormName2
	Sort

: 2021-02-26 by famo23 ---------------------------------------------------------
[포틀릿] 날씨포틀릿 드롭다운박스 디자인 수정
	\DeskPlusEIP\Portlet\Package\Weather.ascx

[포탈] 메뉴바로가기일때 홈화면을 거치는 문제 수정
	\DeskPlusEIP\EIP\Package\NMenu.aspx.cs
	\DeskPlusEIP\EIP\DeskPlusEPDefault.aspx
	\DeskPlusEIP\EIP\DeskPlusEPDefault.aspx.cs

: 2021-02-18 by famo23 ---------------------------------------------------------
[포틀릿] 포토앨범포틀릿 가로사이즈 90%로 변경해서 페이지 사이즈 작아져도 반응할 수 있게처리
	\DeskPlusEIP\Portlet\Package\Board\NoticeImage.ascx

: 2021-01-14 by famo23 ---------------------------------------------------------
[자원관리] 자원명특수문자치환
	\DeskPlusEIP\RM\Admin\Reserve\View.aspx.cs
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\ViewTotal.aspx.cs

: 2021-01-07 by famo23 ---------------------------------------------------------
[주소록] 외부주소록 화면에서 이상한 박스 나오는 버그수정 (업로드화면에서 class를 추가했는데 사용하는 클래스명을 사용함)
	DeskPlusEIPBase.css
	\DeskPlusEIP\Upload\JSUpload\Main.aspx

: 2021-01-07 by kkzz52 --------------------------------------------------------
[공통] 관리자 -> GW관리 -> 코드관리에서 활성/비활성화 또는 삭제 시 캐쉬 삭제 되도록 처리
	 \DeskPlusEIP\EIP\Ws\Admin\Code\List.asmx.cs

: 2020-11-26 by maniaysw ---------------------------------------------------------
[X-Free 에디터]

- 에디터 높이 강제지정 해제
	var xfe = new XFE({
	   basePath : 'http://192.168.0.1/test',
	   width : '700px',
	   height : '700px',
	   ignoreMinHeight : true,      // 에디터에서 설정하는 최소 높이를 설정 되지 않게 한다.
	   ignoreMinWidth : true        // 에디터에서 설정하는 최소 너비를 설정 되지 않게 한다.
	});
  (결재) \DeskPlusEIP\Approval\FormFlow\Basic\EditorNonActiveX.aspx

- 하단 드래그 드롭 크기조절 추가(기본 false)
   \DeskPlusEIP\Data\Editor\XFE2\config\env.xml

- 작성영역 마우스 클릭 범위설정
   \DeskPlusEIP\Data\Editor\XFE2\CSS\xfe-content-base.css


: 2020-11-25 by kkzz52 -----------------------------------------------------------
[전자결재] 수발자의 대결자는 결재취소 불가 (수신문서를 대결자가 상신한 경우)
	ufn_IsCancelApproval

: 2020-11-23 by famo23 -----------------------------------------------
[업무협업] 리스트 정렬 Row 추가 	
	DP_TAS_List_SelCCPageList
	DP_TAS_List_SelForwardPageList
	DP_TAS_List_SelImpoPageList
	DP_TAS_List_SelRequestPageList
	DP_TAS_List_SelTOPageList
	DP_TAS_List_SelTotalPageList

: 2020-11-20 by famo23 -----------------------------------------------
[업무톡] 게시판 글 댓글 등록 안되는 버그	
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.DAL\BoardTalk.cs

: 2020-11-19 by shpark1325 -----------------------------------------------
[주소록] 직급주소록에서 사용자 이미지나오도록 수정
	\Projects\AddressProject\Zeospace.DeskPlus.Address.BLL\List.cs

[주소록] 직급주소록 하위 직급, 직책 클릭 시 페이지 처리 수정
	DP_ADD_SelPageCntWithSub

: 2020-11-18 by famo23 -----------------------------------------------
[에디터] 글씨폰트 기본 '맑은고딕'으로 설정 
	\DeskPlusSolution\DeskPlusEIP\Data\Editor\XFE2\config\env.xml
    \DeskPlusSolution\DeskPlusEIP\Data\Editor\env_ko.xml

: 2020-11-13 by famo23 -----------------------------------------------
[포틀릿] 관리자권한 있을 경우에만 전사메인탭 포틀릿 추가할 수 있게
	\DeskPlusEIP\EIP\Package\NMain.aspx

[포틀릿] 관리자권한 있을 경우에만 전사메인탭 포틀릿 삭제할 수 있게
	\DeskPlusEIP\EIP\Js\Package\NMain.js
	\DeskPlusEIP\EIP\Package\NMain.aspx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.zh-tw.resx

	\DeskPlusEIP\Portlet\Js\Package\MyInfo.js
	\DeskPlusEIP\Portlet\Js\Package\MyMemo.js
	\DeskPlusEIP\Portlet\Js\Package\MyPim.js
	\DeskPlusEIP\Portlet\Js\Package\Opinion.js
	\DeskPlusEIP\Portlet\Js\Package\PimList.js
	\DeskPlusEIP\Portlet\Js\Package\QuickLink.js
	\DeskPlusEIP\Portlet\Js\Package\ResourceList.js
	\DeskPlusEIP\Portlet\Js\Package\STA\PerformMon.js    
	\DeskPlusEIP\Portlet\Js\Package\STA\PerformMonDisk.js
	\DeskPlusEIP\Portlet\Package\Approval\ListA.ascx      
	\DeskPlusEIP\Portlet\Package\Approval\ListC.ascx      
	\DeskPlusEIP\Portlet\Package\Approval\ListCir.ascx    
	\DeskPlusEIP\Portlet\Package\Approval\ListF.ascx      
	\DeskPlusEIP\Portlet\Package\Approval\ListR.ascx      
	\DeskPlusEIP\Portlet\Package\Approval\Tab.ascx        
	\DeskPlusEIP\Portlet\Package\Attendance.ascx          
	\DeskPlusEIP\Portlet\Package\Board\Album.ascx         
	\DeskPlusEIP\Portlet\Package\Board\Blog.ascx         
	\DeskPlusEIP\Portlet\Package\Board\CountTab.ascx      
	\DeskPlusEIP\Portlet\Package\Board\GMWords.ascx       
	\DeskPlusEIP\Portlet\Package\Board\List.ascx          
	\DeskPlusEIP\Portlet\Package\Board\NoticeImage.ascx   
	\DeskPlusEIP\Portlet\Package\Board\Tab.ascx         
	\DeskPlusEIP\Portlet\Package\Board\TopReader.ascx     
	\DeskPlusEIP\Portlet\Package\Board\TopRegister.ascx   
	\DeskPlusEIP\Portlet\Package\Congratulation.ascx      
	\DeskPlusEIP\Portlet\Package\HR\Attendance.ascx       
	\DeskPlusEIP\Portlet\Package\Mail\List.ascx           
	\DeskPlusEIP\Portlet\Package\MyComment.ascx           
	\DeskPlusEIP\Portlet\Package\RM\RMManager.ascx        
	\DeskPlusEIP\Portlet\Package\RM\Tab.ascx
	\DeskPlusEIP\Portlet\Package\SMSNew\SMS.ascx        
	\DeskPlusEIP\Portlet\Package\TabMain.ascx
	\DeskPlusEIP\Portlet\Package\TabOpinion.ascx         
	\DeskPlusEIP\Portlet\Package\TabTaskMain.ascx         
	\DeskPlusEIP\Portlet\Package\Talk\Tab.ascx          
	\DeskPlusEIP\Portlet\Package\TodoList.ascx          
	\DeskPlusEIP\Portlet\Package\ToSchList.ascx           
	\DeskPlusEIP\Portlet\Package\UserSearch\DeptUserSearch.ascx
	\DeskPlusEIP\Portlet\Package\Weather.ascx
	\DeskPlusEIP\Portlet\Template\Album.ascx
	\DeskPlusEIP\Portlet\Template\Blog.ascx
	\DeskPlusEIP\Portlet\Template\List.ascx

: 2020-10-19 by kkzz52 -----------------------------------------------
[공통] .left-menu .accordion .menusub 스타일 수정 
	- accordion 메뉴 이동 시 스크롤이 생기지 않는 현상 수정 (overflow:auto !important;)	
	\DeskPlusEIP\Design\Dev2018\C\Css\DeskPlusEIPBase.css

: 2020-10-19 by ghtjd7794 -----------------------------------------------
[Report] 짧은댓글 날짜 순으로 정렬
	QRY_REPORT_ShortReply_Sel

: 2020-09-28 by famo23 -----------------------------------------------
[Account] 사용자추가 할때 인장삭제 안되는 버그 수정
	\DeskPlusEIP\Account\Js\Admin\User\Add.js

: 2020-09-17 by famo23 -----------------------------------------------
[커뮤니티] 메뉴추가 한 이후에 또 추가가 안되는 버그수정
	\DeskPlusEIP\Community\Js\Site\Admin\Function\Menu\Add.js

: 2020-09-15 by famo23 -----------------------------------------------
[방문예약] 자원예약의 보안/일반 설정 추가 
	\DeskPlusEIP\Visit\Content\Add.aspx.cs
	\DeskPlusEIP\Visit\Content\Mod.aspx.cs

[방문예약] 반복일정 수정할때 기존첨부파일 관련 수정
	DP_VIS_Attach_Del
	\Projects\VisitProject\Zeospace.DeskPlus.Visit.BLL\Attach.cs
	\Projects\VisitProject\Zeospace.DeskPlus.Visit.BLL\Reserve.cs
	\Projects\VisitProject\Zeospace.DeskPlus.Visit.DAL\Attach.cs

: 2020-09-10 by famo23 -----------------------------------------------
[커뮤니티] 앨범형 게시판에서 첨부파일이 이미지 파일이 아닌 경우 이미지보기 버그 수정
	\DeskPlusEIP\Community\Js\Site\Apps\Board\ListGallery.js
	\DeskPlusEIP\Community\Js\Site\Apps\Board\View.js
	\DeskPlusEIP\Community\Site\Apps\Board\GallerySubList.aspx.cs
	\DeskPlusEIP\Community\Site\Apps\Board\GallerySubListN.aspx.cs

[커뮤니티] 앨범형 게시판에서 첨부파일 올리기 버그수정
	\DeskPlusEIP\Community\Js\Site\Apps\Board\Add.js
	\DeskPlusEIP\Community\Js\Site\Apps\Board\Mod.js

: 2020-09-08 by famo23 -----------------------------------------------
[커뮤니티] 커뮤니티 이름, 소개의 특수문자 변환
	\DeskPlusEIP\Community\Ws\Admin\Site\ListClose.asmx.cs

: 2020-09-07 by famo23 -----------------------------------------------
[팝업관리] 로그인전에 팝업 창 나오는 부분이 로딩 안되는 버그 수정 
	\DeskPlusEIP\Preview.aspx.cs

[로그인] 패스워드 관련 화면 다국어 처리 안된 부분 수정 
	\DeskPlusEIP\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Site\Auth\ModPassword.aspx
	\DeskPlusEIP\Site\Auth\ModPassword.aspx.cs
	\DeskPlusEIP\Site\Auth\SendPassword.aspx
	\DeskPlusEIP\Site\Auth\SendPassword.aspx.cs

: 2020-08-31 by famo23 -----------------------------------------------
[일정관리] 할일 댓글수정할때 댓글공개 값을 가지고 오지 못하는 버그수정
	\Projects\PimProject\Zeospace.DeskPlus.Pim.DAL\ToDoReply.cs

: 2020-08-25 by famo23 -----------------------------------------------
[일정관리] 기념일 보기화면이 권한 없는 사용자들은 열리지 않는 버그수정
	\DeskPlusEIP\Pim\Admin\Holiday\View.aspx.cs

[일정관리] 기념일관리에서 검색어 입력 후 기간조회할때 검색이 안되는 버그수정
	\DeskPlusEIP\Pim\Js\Anniversary\List.js

[일정관리] 기념일관리에서 구분명칭 통일
	\DeskPlusEIP\Pim\Anniversary\Add.aspx.cs
	\DeskPlusEIP\Pim\Anniversary\List.aspx.cs
	\DeskPlusEIP\Pim\Anniversary\Mod.aspx.cs

: 2020-08-24 by famo23 -----------------------------------------------
[게시판] 메일발송 딥메일로 열리게 수정
	\DeskPlusEIP\Board\Js\Admin\Content\View.js
	\DeskPlusEIP\Board\Js\Content\ViewImpo.js
	\DeskPlusEIP\Board\Js\Content\ViewInterested.js

: 2020-08-21 by famo23 -----------------------------------------------
[자원관리] 첨부파일 카운트 버그 수정	
	DP_RM_Reserve_SelDeptList
	DP_RM_Reserve_SelPageList
	DP_RM_Reserve_SelMainList
	DP_RM_Reserve_SelMyList
	DP_RM_Reserve_SelUsablePageList

	\DeskPlusEIP\RM\Jt\Admin\Reserve\JTUsableList.htm
	\DeskPlusEIP\RM\Jt\Content\Manager\JTUsableList.htm
	\DeskPlusEIP\RM\Jt\Content\JTMyList.htm
	\DeskPlusEIP\RM\Jt\Home\JTDeptList.htm
	\DeskPlusEIP\RM\Jt\Home\JTMainList.htm
	\DeskPlusEIP\RM\Jt\Home\JTMyList.htm
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable0List.asmx.cs
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable1List.asmx.cs
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable2List.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\Manager\Usable0List.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\Manager\Usable1List.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\Manager\Usable2List.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\MyList.asmx.cs
	\DeskPlusEIP\RM\Ws\Home\Main.asmx.cs

[공통] 파일삭제 다국어처리
	\DeskPlusEIP\Common\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Common\Ws\SpoolFile.asmx.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\SpoolFileInfo.cs

: 2020-08-20 by famo23 -----------------------------------------------
[공통] 툴팁관련 추가
	\DeskPlusEIP\Html\Js\Design\popper.min.js

: 2020-08-19 by famo23 -----------------------------------------------
[자원관리] 자원폴더 트리에서 하위폴더가 권한 없는 사용자들은 열리지 않는 버그 수정
	\DeskPlusEIP\RM\Admin\Folder\GetFolderTree.aspx.cs

: 2020-08-18 by kkzz52 ------------------------------------------------
[근태] 부서현황정보에서 조회권한이 있는 부서의 상위부서가 변경될 경우 조회 불가능한 오류 수정
	\DeskPlusEIP\HR\Content\ListDept.aspx.cs
	\DeskPlusEIP\HR\Content\ListPrivateMod.aspx.cs

	DP_HR_Role_SelActive
	DP_HR_Role_Sel
	DP_HR_AttendanceRole_SelActive
	DP_HR_AttendanceRole_Sel

: 2020-08-13 by famo23 -----------------------------------------------
[공통] ClientSmartButton title 제거
	\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\WebComponent\ClientSmartButton.cs

: 2020-08-13 by kkzz52 -----------------------------------------------
[공통] .popup 스타일 수정 (z-index:999; 추가. 자주지정하는결재자 클릭 안되서 추가함) 
	\DeskPlusEIP\Design\Dev2018\C\Css\DeskPlusEIPBase.css

: 2020-08-11 by famo23 -----------------------------------------------
[게시판] 검색화면, 최신글, 최다댓글, 최다조회, 평가우수 화면에서 댓글화면 새창처리
	\DeskPlusEIP\Board\Js\Home\MoreList.js
	\DeskPlusEIP\Board\Js\Home\Search.js
	\DeskPlusEIP\Board\Jt\Home\JTMoreList.htm
	\DeskPlusEIP\Board\Jt\Home\JTSearch.htm

[게시판] 검색화면, 최신글, 최다댓글, 최다조회, 평가우수 화면에서 조회수화면 새창처리
	\DeskPlusEIP\Board\Js\Home\MoreList.js
	\DeskPlusEIP\Board\Js\Home\Search.js
	\DeskPlusEIP\Board\Jt\Home\JTMoreList.htm
	\DeskPlusEIP\Board\Jt\Home\JTSearch.htm

[게시판] 검색화면 등록자 안 나오는 버그수정
	\DeskPlusEIP\Board\Ws\Home\Search.asmx.cs

: 2020-08-10 by famo23 -----------------------------------------------
[게시판] 템플릿관리 번호 순서 버그수정
	\DeskPlusEIP\Board\Jt\Admin\Template\JTTemplate.htm
	\DeskPlusEIP\Board\Ws\Admin\Template\List.asmx.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\TemplatesInfo.cs

: 2020-08-07 by famo23 -----------------------------------------------
[게시판] 중요표시 리스트 쿼리 수정
	DP_BOA_ImpoPageList
	\DeskPlusEIP\Board\Content\BlogListImpo.aspx.cs
	\DeskPlusEIP\Board\Content\GalleryListImpo.aspx.cs
	\DeskPlusEIP\Board\Content\ListImpo.aspx.cs

[포틀릿] 게시판 댓글 리스트 페이지 변경
	\DeskPlusEIP\Portlet\Package\Board\List.ascx
	\DeskPlusEIP\Portlet\Package\Board\ListTot.ascx
	\DeskPlusEIP\Portlet\Package\Board\Tab.ascx
	\DeskPlusEIP\Portlet\Package\TabWorkMain.ascx

: 2020-08-06 by kkzz52 -----------------------------------------------
[공통] .layout_subbox 스타일 수정 (text-align: left; 추가)
	\DeskPlusEIP\Design\Dev2018\C\Css\DeskPlusEIPBase.css

: 2020-08-04 by famo23 -----------------------------------------------
[공통] 스크롤 처리할때 헤더 고정 
	\DeskPlusEIP\Html\Js\jQuery\jquery.common.js

: 2020-07-30 by famo23 -----------------------------------------------
[공통] ClientSmartButton 스타일 수정 
	\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\WebComponent\ClientSmartButton.cs

: 2020-07-21 by famo23 -----------------------------------------------
[커뮤니티] 바로가기주소복사 - 플래시제거
	\DeskPlusEIP\Community\Js\Site\Main\Menu.js
	\DeskPlusEIP\Community\Site\Main\Menu.aspx

: 2020-07-10 by kkzz52 -----------------------------------------------------------
[전자결재] 수발자관리에서 비활성화 유저도 보이도록 수정 (수발자가 퇴사했을경우 지정해제 가능하도록)
	DP_APP_Account_SelListCnt
	DP_APP_Account_SelList

: 2020-07-10 by kkzz52 -----------------------------------------------------------
[전자결재] 발신부서에서 수신(ZP) 문서의 현황을 볼 수 있도록 수정 (결재진행현황 팝업)
	\DeskPlusEIP\Approval\Js\Status\Common\GetDocFlow.js
	\DeskPlusEIP\Approval\Status\Common\GetDocFlow.aspx
	\DeskPlusEIP\Approval\Status\Common\GetDocFlow.aspx.cs
	\DeskPlusEIP\Approval\Ws\Status\Common\GetDocFlow.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs

	DP_APP_ZP_SelList 추가
	DP_APP_ZP_SelCount 추가
	ufn_GetDocPathSubtreeTot 추가

: 2020-07-08 by kkzz52 -----------------------------------------------------------
[전자결재] 결재 홈에서 검색 한 리스트에서 문서보기 시 문서조회 권한이 없다고 나오는 오류 수정
	DP_APP_Doc_SelAuthDoc (타부서문서 열람권한 추가)

: 2020-07-01 by kkzz52 ------------------------------------------------
[전자결재] 승인 시 ZA인장 이미지 새로고침 (합의불가->결재취소->합의 할 경우 인장이 바뀌지 않는 현상 때문)
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js

: 2020-06-26 by kkzz52 ------------------------------------------------
[전자결재] 크롬에서 양식 등록되도록 수정
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\List.js
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\Add.js
	\DeskPlusEIP\Approval\Admin\FormGeneration\AddN.aspx 추가

: 2020-06-23 by famo23 -----------------------------------------------
[알림발송기] 관리자가 모바일 Push 발송 할 수 있게  

	DPmanager 수정

	DP_COM_Env - Gubun : MDM , KeyID : ListCount 항목추가

	* 테이블 생성	
	DP_MDM_AppUser	
	DP_MDM_ListUser 
	DP_MDM_Receive
	DP_MDM_Send 

	* 필드 생성
	DP_ALARM_Common - MsgSendID,AlarmUrl
	DPM_ALARM_Common - AlarmUrl
	DPM_ALARM_CommonIOS - AlarmUrl
	DP_MDM_NotificationBox - MsgSendID

	TRI_Alarm_Common_Insert
	SQL_DP_ALARM_Common_Insert
	DPM_ALARM_Common_Sel
	DPM_ALARM_Common_SelByIOS

    DP_ACC_User_SelAuto
	DP_ACC_UserEtc_SelName
	DP_MDM_ListUser_InsUpd
	DP_MDM_AppUser_SelPageCnt
	DP_MDM_AppUser_SelPageList
	DP_MDM_Receive_Ins
	DP_MDM_Receive_InsAll
	DP_MDM_Receive_SelPageCnt
	DP_MDM_Receive_SelPageList
	DP_MDM_Send_Ins
	DP_MDM_Send_SelPageCnt
	DP_MDM_Send_SelPageList	

	QRY_MDM_ListUser_Sel
	QRY_MDM_ReceiveEtc_Sel
	QRY_MDM_Send_Sel

	mdmSend 권한생성

	\Design\C\Css\Func_MDM.css

	\DeskPlusEIP\App_Data\MDM.xml

	\DeskPlusEIP\Account\Js\MyInfo\Mobile.js
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx.cs
	\DeskPlusEIP\Account\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-tw.resx

	\CommonProject\Zeospace.DeskPlus.Common.BE\AlarmInfo.cs
	\CommonProject\Zeospace.DeskPlus.Common.BLL\Alarm.cs
	\CommonProject\Zeospace.DeskPlus.Common.DAL\AlarmCommon.cs

	\DeskPlusEIP\Html\Js\jQuery\jquery-ui.css
	\DeskPlusEIP\Html\Js\jQuery\jquery-ui.js
	\DeskPlusEIP\Html\Js\jQuery\jquery.autogrow-textarea.js

	\DeskPlusEIP\MDM\Base\BasePageMDM.cs
	\DeskPlusEIP\MDM\Config\Default.aspx
	\DeskPlusEIP\MDM\Config\Default.aspx.cs
	\DeskPlusEIP\MDM\Config\Setting.aspx
	\DeskPlusEIP\MDM\Config\Setting.aspx.cs
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Js\Config\Default.js
	\DeskPlusEIP\MDM\Js\Config\Setting.js
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js
	\DeskPlusEIP\MDM\Js\Send\Add.js
	\DeskPlusEIP\MDM\Js\Send\AppList.js
	\DeskPlusEIP\MDM\Js\Send\Content.js
	\DeskPlusEIP\MDM\Js\Send\GetUserDept.js
	\DeskPlusEIP\MDM\Js\Send\List.js
	\DeskPlusEIP\MDM\Js\Send\ReceiveList.js
	\DeskPlusEIP\MDM\Jt\Send\JTAppList.htm
	\DeskPlusEIP\MDM\Jt\Send\JTList.htm
	\DeskPlusEIP\MDM\Jt\Send\JTReceiveList.htm
	\DeskPlusEIP\MDM\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\MDM\Send\Add.aspx
	\DeskPlusEIP\MDM\Send\AppList.aspx
	\DeskPlusEIP\MDM\Send\Content.aspx
	\DeskPlusEIP\MDM\Send\GetUserDept.aspx
	\DeskPlusEIP\MDM\Send\List.aspx
	\DeskPlusEIP\MDM\Send\ReceiveList.aspx
	\DeskPlusEIP\MDM\Ws\Send\Add.asmx
	\DeskPlusEIP\MDM\Ws\Send\AppList.asmx
	\DeskPlusEIP\MDM\Ws\Send\List.asmx
	\DeskPlusEIP\MDM\Ws\Send\ReceiveList.asmx

	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\AppUserInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\ListUserInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\ReceiveInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\SendInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BLL\Send.cs
    \Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\AppUser.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\ListUser.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\Receive.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\Send.cs

: 2020-06-15 by famo23 -----------------------------------------------
[전자결재] 휴가원 커스텀파일 : 휴가사유 특수문자로 일정관리, 자원관리에 안 들어가는 버그로 사유등록 삭제
	\DeskPlusEIP\Legacy\HoliDay_Complete.aspx
	\DeskPlusEIP\Pim\Ws\Interface\Add.asmx.cs
	\DeskPlusEIP\RM\Ws\Interface\Add.asmx.cs

: 2020-06-10 by kkzz52 ------------------------------------------------
[전자결재] 고정수신망 태그일때 개인결재선, 결재선저장에서 수신자 로딩 안되는 현상 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\GetFavList.js

: 2020-06-09 by famo23 ----------------------------------------------------
[모바일관리] 모바일번호, 디바이스이름 안보이게 수정 
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx

[모바일관리] 알림함 보기 OFF 인 경우에는 Push를 수정할 수 없게 수정
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx.cs
	\Account\Resource\GlobalString\string.en.resx
	\Account\Resource\GlobalString\string.fr.resx
	\Account\Resource\GlobalString\string.ja.resx
	\Account\Resource\GlobalString\string.ko.resx
	\Account\Resource\GlobalString\string.zh-cn.resx
	\Account\Resource\GlobalString\string.zh-tw.resx

[모바일관리] 모바일관리 알람 설정 삭제
	\DeskPlusEIP\Account\Js\MyInfo\Mobile.js
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx.cs
	\DeskPlusEIP\MDM\Config\Default.aspx
	\DeskPlusEIP\MDM\Config\Default.aspx.cs
	\DeskPlusEIP\MDM\Config\Setting.aspx
	\DeskPlusEIP\MDM\Config\Setting.aspx.cs
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Js\Config\Default.js
	\DeskPlusEIP\MDM\Js\Config\Setting.js
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js

[모바일관리] 알림함 보기 관련해서 보완
	DP_MDM_List_UpdAlarm
    DP_MDM_List_UpdApprove

: 2020-05-25 by shpark1325 ------------------------------------------------
[전자결재] unload 할 경우 Viewing 체크 수정
	\DeskPlusEIP\Web.config
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.js

: 2020-05-22 by kkzz52 -----------------------------------------------------------
[근태] 휴가정보 팝업에서 퇴사자가 있을 경우 중복으로 나오는 오류 수정
	DP_HOLI_Used_SelHRPageList
	DP_HOLI_Used_SelHRPageCnt

: 2020-05-22 by kkzz52 -----------------------------------------------------------
[근태] 회사현황정보에서 변경내역 체크 후 조회할 경우 부서리스트 잘못 나오는 오류 수정
	DP_HR_Attendance_SelCompanyList

: 2020-05-22 by kkzz52 -----------------------------------------------------------
[근태] 부서현황정보, 근태정보수정에서 현재부서가 현재 조직도와 다르게(부서순서) 나오는 오류 수정
	\DeskPlusEIP\HR\Content\ListDept.aspx.cs
	\DeskPlusEIP\HR\Content\ListPrivateMod.aspx.cs
	\Projects\HRProject\Zeospace.DeskPlus.HR.DAL\Role.cs
	\Projects\HRProject\Zeospace.DeskPlus.HR.DAL\AttendanceRole.cs

	DP_HR_Role_SelActive 추가
	DP_HR_AttendanceRole_SelActive 추가

: 2020-04-09 by kkzz52 -----------------------------------------------------------
[전자결재] 수신 담당자 변경 시 복수로 선택되는 오류 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

: 2020-04-08 by kkzz52 -----------------------------------------------------------
[전자결재] 반려 알람 모두에게 체크 시 안가는 오류 수정
	DP_APP_Seq_SelAlarmRejectAll 수정

: 2020-04-08 by kkzz52 -----------------------------------------------------------
[일정관리] 개인이 기념일 수정 시 음력일 경우 DP_PIM_Display에 EndDate가 양력으로 입력되는 오류 수정
	\DeskPlusEIP\Pim\Anniversary\Mod.aspx.cs

: 2020-03-31 by kkzz52 -----------------------------------------------------------
[전자결재] 결재취소 시 병렬결재자일 경우 ApprovalState가 잘못 변경되는 오류 수정
	\DeskPlusEIP\Approval\Ws\Status\ListRelated.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs

	DP_APP_Doc_IsCancelZA 추가

: 2020-03-31 by kkzz52 -----------------------------------------------------------
[전자결재] 승인 클릭 시 최신 문서내용을 읽어온 후 승인 처리하도록 처리
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js	
	\DeskPlusEIP\Approval\Ws\Folder\Common\View.asmx.cs

: 2020-03-31 by kkzz52 -----------------------------------------------------------
[전자결재] 결재의견 추가 시 최신 문서내용 반영 안되는 오류 수정
	\DeskPlusEIP\Approval\Js\Process\Opinion\ConfirmOpinion.js
	\DeskPlusEIP\Approval\Ws\Folder\Common\View.asmx.cs

: 2020-03-24 by kkzz52 -----------------------------------------------------------
[전자결재] 문서번호 중복 오류 수정
	DP_APP_DocProcess_SelByDocNumber
	DPM_APP_DocProcess_SelByDocNumber

: 2020-03-20 by kkzz52 -----------------------------------------------------------
[전자결재] 수발자 DeptID 잘못 가져오는 오류 수정
	VW_APP_Account
	DP_APP_Account_SelGetTree

: 2020-03-18 by kkzz52 -----------------------------------------------------------
[전자결재] 수신문서를 회람한 후 문서삭제 또는 재기안 시 오류 수정
	DP_APP_CirDoc_DelByDocID

: 2020-03-16 by famo23 -----------------------------------------------
[모바일링크] pwd 조건 추가 
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\Alarm.cs

: 2020-02-26 by famo23 -----------------------------------------------
[팝업게시판] 레이아웃 스타일 변경 
	\DeskPlusEIP\ETC\PopUp\Add.aspx
	\DeskPlusEIP\ETC\PopUp\AddN.aspx
	\DeskPlusEIP\ETC\PopUp\List.aspx
	\DeskPlusEIP\ETC\PopUp\Mod.aspx
	\DeskPlusEIP\ETC\PopUp\ModN.aspx

: 2020-02-24 by famo23 -----------------------------------------------
[포틀릿] 메일포틀릿에서 읽지않은메일표시 구분 <B>처리
	QRY_PORTLET_Mail_SelZettaMail
	\DeskPlusEIP\Portlet\Jt\Package\Mail\JTList.htm
	\DeskPlusEIP\Portlet\Ws\Package\Mail\List.asmx.cs
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.BE\MailInfo.cs

: 2020-02-06 by famo23 -----------------------------------------------
[일정] 일간,주간,월간화면의 일정추가, 수정에 일정알람 추가 

	DP_PIM_Schedule_SimpleUPD

	\DeskPlusEIP\Pim\Js\Schedule\AddSimple.js
	\DeskPlusEIP\Pim\Js\Schedule\ListDay.js
	\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js
	\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
	\DeskPlusEIP\Pim\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Pim\Schedule\ListDay.aspx
	\DeskPlusEIP\Pim\Schedule\ListDay.aspx.cs
	\DeskPlusEIP\Pim\Schedule\ListMonth.aspx
	\DeskPlusEIP\Pim\Schedule\ListMonth.aspx.cs
	\DeskPlusEIP\Pim\Schedule\ListWeek.aspx
	\DeskPlusEIP\Pim\Schedule\ListWeek.aspx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\AddSimple.asmx
	\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx
	\Projects\PimProject\Zeospace.DeskPlus.Pim.DAL\Schedule.cs

[일정] 메일알람의 메일주소 필드 수정
	DP_PIM_Schedule 테이블 AlertTargetAddress 필드 데이터형식 변경 
	DP_PIM_Schedule_Ins
	DP_PIM_Schedule_UPD
	DP_PIM_Schedule_SimpleUPD
	\Projects\PimProject\Zeospace.DeskPlus.Pim.DAL\Schedule.cs

[일정] 일정추가에서 공유자 추가한 후에 삭제 안되는 버그수정
	\DeskPlusEIP\Pim\Js\Schedule\Add.js

: 2020-02-05 by famo23 -----------------------------------------------
[일정] '기본 알람으로 설정' 기능 추가 

	DP_PIM_Alarm 테이블 추가 
	DP_PIM_Alarm_Ins

	INSERT INTO [dbo].[DP_QUERY]
           ([Category],[qryid],[qryvalue],[owner],[memo],[regdate])
    VALUES
           ('PIM','QRY_PIM_Alarm_Sel'
           ,'SELECT * FROM DP_PIM_Alarm WITH(NOLOCK) WHERE CompanyID = @CompanyID AND UserID = @UserID'
           ,'famo23','기본 알람 설정가져오기','20200205')

	\DeskPlusEIP\Pim\Js\Schedule\Add.js
	\DeskPlusEIP\Pim\Js\Schedule\Mod.js
	\DeskPlusEIP\Pim\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Pim\Schedule\Add.aspx
	\DeskPlusEIP\Pim\Schedule\Add.aspx.cs
	\DeskPlusEIP\Pim\Schedule\AddN.aspx
	\DeskPlusEIP\Pim\Schedule\Mod.aspx
	\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs
	\DeskPlusEIP\Pim\Schedule\ModN.aspx
	\DeskPlusEIP\Pim\Ws\Schedule\Add.asmx
	\DeskPlusEIP\Pim\Ws\Schedule\Mod.asmx
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BE\UserAlarmInfo.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.DAL\Alarm.cs

[일정] 일정 수정할때 메신저 알림이 이미 설정되어 있을 시에 '전에알림' 문구가 안나오는 버그 수정 
	\DeskPlusEIP\Pim\Js\Schedule\Mod.js

[일정] 모바일 알림 추가 (코드관리 : 알람설정-일정관리에 MOBILE 추가)

	\DeskPlusEIP\Account\MyInfo\Alarm.aspx.cs

	\DeskPlusEIP\Pim\Js\Schedule\Add.js
	\DeskPlusEIP\Pim\Js\Schedule\Mod.js
	\DeskPlusEIP\Pim\Schedule\Add.aspx
	\DeskPlusEIP\Pim\Schedule\Add.aspx.cs
	\DeskPlusEIP\Pim\Schedule\AddN.aspx
	\DeskPlusEIP\Pim\Schedule\Mod.aspx
	\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs
	\DeskPlusEIP\Pim\Schedule\ModN.aspx
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\Schedule.cs

[일정] SMS 알람 관련 수정 (설정은 있는데 개발이 되어 있지 않아서 삭제처리)
	\DeskPlusEIP\Pim\Js\Schedule\Add.js
	\DeskPlusEIP\Pim\Js\Schedule\Mod.js
	\DeskPlusEIP\Pim\Schedule\Add.aspx.cs
	\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\Schedule.cs
	DP_COM_SYSCode 테이블에서 SMS 설정 삭제 

[일정] 일정추가, 수정팝업에서 일정분류 비활성화처리 수정
	\DeskPlusEIP\Pim\Js\Schedule\ListDay.js
	\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js
	\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js

: 2020-02-04 by famo23 -----------------------------------------------
[일정] 메일알림 관련 수정
	   (메일정규식체크, 구분자 설명추가, 공유자 설정시 입력한 메일주소는 초기화 되는 부분, 공유자 메일 알림 받는 지 체크,
	   _receiverID=0으로 메일 알림이 안오던 부분, 제목에 일정알림 + 일정날짜&시간 추가)

	QRY_PIM_NOTI_DEPT
	QRY_PIM_NOTI_USER
	\DeskPlusEIP\Pim\Js\Schedule\Add.js
	\DeskPlusEIP\Pim\Js\Schedule\Mod.js
	\DeskPlusEIP\Pim\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Pim\Schedule\Add.aspx
	\DeskPlusEIP\Pim\Schedule\Add.aspx.cs
	\DeskPlusEIP\Pim\Schedule\AddN.aspx
	\DeskPlusEIP\Pim\Schedule\Mod.aspx
	\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs
	\DeskPlusEIP\Pim\Schedule\ModN.aspx
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\Schedule.cs
	

: 2020-01-31 by shpark1325 -----------------------------------------------
[자원관리]자원 폴더, 자원리스트 순서변경

	\DeskPlusEIP\RM\Admin\Content\List.aspx
	\DeskPlusEIP\RM\Admin\Content\ListUpDown.aspx
	\DeskPlusEIP\RM\Admin\Content\SubFolderUpdown.aspx

	\DeskPlusEIP\RM\Admin\Content\View.aspx.cs
	\DeskPlusEIP\RM\Admin\Content\ListUpDown.aspx.cs
	\DeskPlusEIP\RM\Admin\Content\SubFolderUpdown.aspx.cs

	\DeskPlusEIP\RM\Js\Admin\Content\List.js
	\DeskPlusEIP\RM\Js\Admin\Content\ListUpDown.js
	\DeskPlusEIP\RM\Js\Admin\Content\SubFolderUpdown.js

	\DeskPlusEIP\RM\Jt\Admin\Content\JTList.htm
		
	\DeskPlusEIP\RM\Ws\Admin\Content\List.asmx.cs
	\DeskPlusEIP\RM\Ws\Admin\Content\ListUpDown.asmx.cs
	\DeskPlusEIP\RM\Ws\Admin\Content\SubFolderUpdown.asmx.cs

	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ListInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Category.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\List.cs
	
	\DeskPlusEIP\RM\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-tw.resx

	--테이블
	DP_RM_List	필드추가
	
	--뷰
	VW_RM_List	필드추가

	--프로시저
	DP_RM_List_UpdOrder			추가
	DP_RM_Category_UpdOrder		추가
	DP_RM_Reserve_SelDayList	수정

	--쿼리
	QRY_RM_List_Sel				수정
	QRY_RM_List_SelList			수정
	QRY_RM_Category_Sel			수정
	QRY_RM_Category_SelByID		추가
	QRY_RM_Category_SelByPID	수정
	
: 2020-01-28 by famo23 -----------------------------------------------
[포탈] 메뉴권한이 없는 사용자가 로그인 할때 null 체크
	\DeskPlusEIP\EIP\Package\NMenu.aspx.cs

[포탈] 메일ID, 도메인 없는 경우에 카운트 가져오지 않게 수정 
	\DeskPlusEIP\Portlet\Ws\Package\MyInfo.asmx.cs

: 2020-01-10 by famo23 -----------------------------------------------
[주소록] 직급, 직책 주소록에서 본인만 나오는 오류 수정
	\Projects\AddressProject\Zeospace.DeskPlus.Address.BLL\List.cs

: 2020-01-07 by famo23 -----------------------------------------------
[모바일관리] 알림함 보기 OFF 인 경우에는 Push를 수정할 수 없게 수정 
	QRY_MDM_UserEnv_Sel
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.zh-tw.resx
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\UserEnv.cs

[모바일관리] 모바일 승인시 기본설정값이 기존기기정보로 가져오게 수정
	DP_MDM_List_UpdApprove
	DP_MDM_List_Upd
	\DeskPlusEIP\MDM\Js\Auth\ListAdd.js
	\DeskPlusEIP\MDM\Js\Auth\ListReject.js
	\DeskPlusEIP\MDM\Ws\Auth\ListAdd.asmx.cs
	\DeskPlusEIP\MDM\Ws\Auth\ListReject.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\List.cs

: 2020-01-02 by famo23 -----------------------------------------------
[전자결재] 스타일 변경
	\DeskPlusEIP\Approval\Css\Func_ApprovalEdit.css

: 2019-12-23 by kkzz52 -----------------------------------------------------------
[전자결재] 결재문서관리 - 진행중인문서에서 ERP연동문서일 경우 삭제 안되는 오류 수정
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js

: 2019-12-20 by kkzz52 -----------------------------------------------------------
[전자결재] 후열문서 오류 처리 (반려문서일 경우)
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx.cs

: 2019-10-31 by famo23 -----------------------------------------------
[Home] 첫로그인시 하단 NMain.aspx 페이지 2번호출수정
	\DeskPlusEIP\EIP\Js\Package\NMenu.js
	\DeskPlusEIP\EIP\Package\NMenu.aspx.cs

[Home] 탭포틀릿정보 가져오는 부분 수정
	DP_EIP_TabPortlet_Sel
	DP_EIP_Portlet_SelGetMyRoleList
	DP_EIP_Portlet_SelGetSubRoleList
	\DeskPlusEIP\EIP\Package\NMain.aspx.cs
	\Projects\EIPProject\Zeospace.DeskPlus.EIP.BE\PortletInfo.cs
	\Projects\EIPProject\Zeospace.DeskPlus.EIP.BE\TabPortletInfo.cs
	\Projects\EIPProject\Zeospace.DeskPlus.EIP.DAL\Portlet.cs
	\Projects\EIPProject\Zeospace.DeskPlus.EIP.DAL\TabPortlet.cs
	\DeskPlusEIP\Portlet\Base\BaseControlPortlet.cs
	\DeskPlusEIP\Portlet\Package\_TabTaskMain.ascx.cs
	\DeskPlusEIP\Portlet\Package\Approval\Tab.ascx.cs  
	\DeskPlusEIP\Portlet\Package\Approval\TabAppMain.ascx.cs
	\DeskPlusEIP\Portlet\Package\Board\__Tab.ascx.cs   
	\DeskPlusEIP\Portlet\Package\Board\CountTab.ascx.cs
	\DeskPlusEIP\Portlet\Package\Board\Tab.ascx.cs     
	\DeskPlusEIP\Portlet\Package\Coop\TabCoop.ascx.cs  
	\DeskPlusEIP\Portlet\Package\Mail\Tab.ascx.cs      
	\DeskPlusEIP\Portlet\Package\RM\Tab.ascx.cs       
	\DeskPlusEIP\Portlet\Package\TabCoopMain.ascx.cs   
	\DeskPlusEIP\Portlet\Package\TabMain.ascx.cs       
	\DeskPlusEIP\Portlet\Package\TabOpinion.ascx.cs    
	\DeskPlusEIP\Portlet\Package\TabTaskMain.ascx.cs   
	\DeskPlusEIP\Portlet\Package\TabWorkMain.ascx.cs   
	\DeskPlusEIP\Portlet\Package\Talk\Tab.ascx.cs     
	\DeskPlusEIP\Portlet\Template\Tab.ascx.cs       

: 2019-10-22 by famo23 ----------------------------------------------------------
[전자결재] 수신담당자 변경 알림 버그 수정
	DP_APP_Counterpartuser_SelAlarm

: 2019-10-18 by shpark1325 ----------------------------------------------------------
[전자결재] 크롬 인쇄방식 수정
	\DeskPlusEIP\Approval\Js\Common\Popup\Print.js

: 2019-10-01 by famo23 ----------------------------------------------------------
[전자결재] 중간결재자한테 반려시 알림 안 가는 버그수정
	DP_APP_Seq_SelAlarmReject
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

[공통] 모바일 알림 읽기표시
	DP_MDM_NotificationBox 필드추가
	DP_ALARM_Common 필드추가
	TRI_Alarm_Common_Insert
	SQL_DP_ALARM_Common_Insert
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\AlarmInfo.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\Alarm.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.DAL\AlarmCommon.cs

: 2019-09-30 by famo23 ----------------------------------------------------------
[관리자] 조직도에서 최상위부서를 상위부서변경하면 부서정보들이 사라짐
	\DeskPlusEIP\Account\Admin\Dept\List.aspx.cs

: 2019-09-25 by famo23 ----------------------------------------------------------
[전자결재] 모바일 알림링크 추가 
	DP_APP_Seq_SelAlarmRejectAll
	\DeskPlusEIP\App_Data\APPROVAL.xml
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	
[전자결재] 회람,전달문서 알림링크 변경
	\DeskPlusEIP\App_Data\APPROVAL.xml
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

: 2019-09-18 by famo23 ----------------------------------------------------------
[자원관리] 모바일 알림링크 추가 
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\ShortReply.cs

: 2019-09-17 by famo23 ----------------------------------------------------------
[커뮤니티] 모바일 알림링크 추가 
	\Projects\CommunityProject\Zeospace.DeskPlus.Community.BLL\Alarm.cs

[일정관리] 할일 댓글 알림에서 모바일 알림이 메신저 알림으로 들어가는 버그수정
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\ToDoReply.cs

: 2019-09-11 by famo23 ----------------------------------------------------------
[일정관리] 모바일 알림링크 추가 
	\DeskPlusEIP\Pim\Js\Schedule\AllSchedule.js
	\DeskPlusEIP\Pim\Js\Schedule\ShortReplyList.js
	\DeskPlusEIP\Pim\Jt\Schedule\JTAllchedule.htm
	\DeskPlusEIP\Pim\Ws\Schedule\AllSchedule.asmx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\ShortReply.asmx.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BE\ScheduleReplyInfo.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\Schedule.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\ScheduleReply.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\ToDoReply.cs

: 2019-09-10 by famo23 ----------------------------------------------------------
[업무톡] 일정관리 - 모바일 알림링크 추가 
	\DeskPlusEIP\SNS\Js\Common\ScheduleTalk.js
	\DeskPlusEIP\SNS\Js\WorkTalk\Content\List.js
	\DeskPlusEIP\SNS\Jt\WorkTalk\Content\JTShortRecvList.htm
	\DeskPlusEIP\SNS\Jt\WorkTalk\Content\JTShortSendList.htm
	\DeskPlusEIP\SNS\Ws\Common\ScheduleTalk.asmx
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BE\ScheduleTalkInfo.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BE\WorkTalkInfo.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\ScheduleTalk.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\ToDoTalk.cs

[업무톡] 게시판 - 모바일 알림링크 추가 
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\BoardTalk.cs

: 2019-09-09 by famo23 ----------------------------------------------------------
[업무협업] 모바일 알림링크 추가 
	\Projects\TaskProject\Zeospace.DeskPlus.Task.BLL\Alarm.cs

[게시판] 모바일 알림링크 추가 
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Lists.cs

[공통] 모바일 알림링크 경로 
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\Alarm.cs

: 2019-09-04 by shpark1325 -----------------------------------------------------------
[업무일지] 내용이 길어질 경우 글자 겹침 발생
	\DeskPlusSolution\DeskPlusEIP\Report\Content\View.aspx
	\DeskPlusSolution\DeskPlusEIP\Report\Content\ViewPopup.aspx
	\DeskPlusSolution\DeskPlusEIP\Report\Content\ViewPT.aspx

: 2019-08-30 by shpark1325 -----------------------------------------------------------
[일정관리] 제목에 & 기호 들어갈 경우 일정수정 시 오류 수정
	\DeskPlusSolution\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
	\DeskPlusSolution\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js

: 2019-08-30 by famo23 ----------------------------------------------------------
[전자결재] 상신문서함에서 Nickname이 들어간 문서 안 나오는 버그 수정 
	DP_APP_Doc_SelPageCntPrivateSubmit
	DP_APP_Doc_SelPageListPrivateSubmit
	
[전자결재] 휴가원의 업무대행자 클릭시 ios 핸드폰에서 반응 없는 부분 수정 
	양식변경처리 	
	\DeskPlusEIP\Legacy\HoliDay.aspx

: 2019-08-28 by famo23 ----------------------------------------------------------
[전자결재] 열람권한이 1등급일때 안 보이는 버그 수정 
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx.cs
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx.cs
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs  
	\DeskPlusEIP\Approval\Status\Common\View.aspx.cs  

: 2019-08-27 by famo23 ----------------------------------------------------------
[전자결재] 특근신청서,특근보고서 날짜 입력칸 크기수정 (모바일에서 안 보임)
		   연장근무신청서 \표시 원으로 변경 (모바일에서 제대로 처리안됨)
    양식변경처리 

: 2019-08-30 by 이중호 ----------------------------------------------------------
[알람]
	\DeskPlusEIP\Common\BE\AlarmInfo.cs

	\DeskPlusEIP\Common\DAL\AlarmCommon.cs

	\DeskPlusEIP\Approval\BLL\DocumentProcess.cs

	모바일 도메인 정보 Config.xml 에 저장

: 2019-08-19 by famo23 ----------------------------------------------------------
[전자결재] 임시저장함, 상신문서함에서 겸직정보나 부서변경된 정보를 제대로 바인딩하지 못하는 버그수정
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js

[전자결재] 임시저장함, 상신문서함에서 EDM설정 없는 버그수정
	DP_APP_Draft_Sel
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Draft.cs

[전자결재] 임시저장, 상신문서함에서 Form 이름을 다국어 가지고 오지 않는 버그수정
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs

[전자결재] 임시저장, 상신문서함에서 문서스트리밍지원안된 버그수정
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs

[전자결재] 임시저장, 상신문서함에서 PDF파일 생성 안된 버그수정
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs

[전자결재] 상신문서함에서 참조열람옵션이 설정 안 되는 버그수정	
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js

: 2019-08-14 by famo23 ----------------------------------------------------------
[전자결재] 사용하지 않는 Form양식으로 문서가 상신되는 버그수정
	DP_APP_Draft_SelPageCnt
	DP_APP_Draft_SelPageList

: 2019-08-05 by famo23 ----------------------------------------------------------
[전자결재] 상신을 PC에서 했는 지, 모바일에서 했는지 구분값저장
	DP_APP_DocProcess_Ins
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs
	\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentProcessInfo.cs
	\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs

: 2019-07-26 by famo23 ----------------------------------------------------------
[전자결재] 임시저장을 PC에서 했는 지, 모바일에서 했는지 구분값저장
	DP_APP_Draft_Ins
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DraftInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Draft.cs

: 2019-07-23 by famo23 ----------------------------------------------------------
[전자결재] 근거문서에서 반려문서가 나오는 오류수정
	DP_APP_Doc_SelPageCntPrivateAll
	DP_APP_Doc_SelPageListPrivateAll
	DP_APP_Doc_SelPageCntAppAll

: 2019-07-11 by famo23 ----------------------------------------------------------
[전자결재] 상신문서함에서 사용하지 않는 양식은 나오지 않게 처리
	DP_APP_Doc_SelPageListPrivateSubmit
	DP_APP_Doc_SelPageCntPrivateSubmit

: 2019-07-10 by famo23 ----------------------------------------------------------
[전자결재] 모바일에서 문서작성하는 양식옵션지정
	DP_APP_Form IsMobile 필드추가
	DP_APP_Form_Sel
	DP_APP_Form_SelByFolderID
	DP_APP_Form_SelByNickNameInfo	
	DP_APP_Form_UpdEtc	
	\DeskPlusEIP\Approval\Admin\FormGeneration\ModSetting.aspx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\FormInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Form.cs

: 2019-06-26 by kkzz52 -----------------------------------------------------------
[전자결재] 결재선저장 시 공람 태그 저장 안되는 오류 수정
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js

: 2019-06-13 by famo23 ----------------------------------------------------------
[포틀릿] setTimeout 로딩 제거 
	\DeskPlusEIP\Portlet\Custom\HelpDesk.ascx    
	\DeskPlusEIP\Portlet\Custom\TabMainBoard.ascx  
	\DeskPlusEIP\Portlet\Js\Package\BirthDay.js    
	\DeskPlusEIP\Portlet\Js\Package\Chart\Bar.js
	\DeskPlusEIP\Portlet\Js\Package\Chart\Pie.js  
	\DeskPlusEIP\Portlet\Js\Package\MyInfo.js     
	\DeskPlusEIP\Portlet\Js\Package\MyInfoNoCnt.js 
	\DeskPlusEIP\Portlet\Js\Package\MySpace.js     
	\DeskPlusEIP\Portlet\Js\Package\Opinion.js
	\DeskPlusEIP\Portlet\Js\Package\ResourceList.js
	\DeskPlusEIP\Portlet\Js\Package\STA\PerformMon.js
	\DeskPlusEIP\Portlet\Js\Package\STA\PerformMonDisk.js
	\DeskPlusEIP\Portlet\Package\Approval\ListA.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListC.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListCir.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListF.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListR.ascx
	\DeskPlusEIP\Portlet\Package\Approval\Tab.ascx 
	\DeskPlusEIP\Portlet\Package\Approval\TabAppMain.ascx
	\DeskPlusEIP\Portlet\Package\Board\Album.ascx	
	\DeskPlusEIP\Portlet\Package\Board\Blog.ascx
	\DeskPlusEIP\Portlet\Package\Board\CountTab.ascx
	\DeskPlusEIP\Portlet\Package\Board\GMWords.ascx
	\DeskPlusEIP\Portlet\Package\Board\List.ascx  
	\DeskPlusEIP\Portlet\Package\Board\ListTot.ascx
	\DeskPlusEIP\Portlet\Package\Board\NoticeBanner.ascx
	\DeskPlusEIP\Portlet\Package\Board\NoticeImage.ascx
	\DeskPlusEIP\Portlet\Package\Board\Tab.ascx   
	\DeskPlusEIP\Portlet\Package\Board\TopReader.ascx
	\DeskPlusEIP\Portlet\Package\Board\TopRegister.ascx
	\DeskPlusEIP\Portlet\Package\Congratulation.ascx
	\DeskPlusEIP\Portlet\Package\Mail\List.ascx    
	\DeskPlusEIP\Portlet\Package\Mail\Tab.ascx		
	\DeskPlusEIP\Portlet\Package\MyComment.ascx    
	\DeskPlusEIP\Portlet\Package\RM\RMManager.ascx
	\DeskPlusEIP\Portlet\Package\RM\Tab.ascx     
	\DeskPlusEIP\Portlet\Package\TabIFrame.ascx    
	\DeskPlusEIP\Portlet\Package\TabMain.ascx     
	\DeskPlusEIP\Portlet\Package\TabOpinion.ascx  
	\DeskPlusEIP\Portlet\Package\TabTaskMain.ascx	
	\DeskPlusEIP\Portlet\Package\TabWorkMain.ascx  
	\DeskPlusEIP\Portlet\Package\Talk\Tab.ascx		
	\DeskPlusEIP\Portlet\Package\TodoList.ascx     
	\DeskPlusEIP\Portlet\Package\ToSchList.ascx   
	\DeskPlusEIP\Portlet\Template\Album.ascx       
	\DeskPlusEIP\Portlet\Template\Blog.ascx       
	\DeskPlusEIP\Portlet\Template\List.ascx        
	\DeskPlusEIP\Portlet\Template\Tab.ascx      


: 2019-06-04 by famo23 ----------------------------------------------------------
[전자결재] 결재순서에 ZA가 있을 경우에 결재취소 가능여부가 제대로 체크안되는 버그
	ufn_IsCancelApproval
	
[전자결재] 후열문서를 문서확인할때 등록한 첨언이 DB처리가 되어있지 않은 버그 
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

: 2019-02-19 by famo23 --------------------------------------------------------
[포틀릿] 업무협업 카운트관련 수정 
	 \DeskPlusEIP\Portlet\Ws\Package\Task\List.asmx.cs

: 2019-01-15 by famo23 --------------------------------------------------------
[관리자] 포틀릿관리에서 UI형식 특수문자깨지는 현상
	\DeskPlusEIP\EIP\Admin\Portlet\Add.aspx.cs
	\DeskPlusEIP\EIP\Admin\Portlet\Mod.aspx.cs

: 2018-12-12 by 이지원 --------------------------------------------------------
[전자결재] 결재선 감추기 양식
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Legacy\ShowHide.aspx

: 2018-12-06 by shpark1325 --------------------------------------------------------
[팝업관리] 다국어 수정
	\DeskPlusEIP\Preview.aspx
	\DeskPlusEIP\Preview.aspx.cs

	\DeskPlusEIP\ETC\PopUp\Add.aspx
	\DeskPlusEIP\ETC\PopUp\AddN.aspx
	\DeskPlusEIP\ETC\PopUp\Add.aspx.cs
	\DeskPlusEIP\ETC\PopUp\List.aspx
	\DeskPlusEIP\ETC\PopUp\List.aspx.cs
	\DeskPlusEIP\ETC\PopUp\Mod.aspx
	\DeskPlusEIP\ETC\PopUp\ModN.aspx
	\DeskPlusEIP\ETC\PopUp\Mod.aspx.cs

	\DeskPlusEIP\ETC\Js\PopUp\Add.js
	\DeskPlusEIP\ETC\Js\PopUp\List.js
	\DeskPlusEIP\ETC\Js\PopUp\Mod.js

	\DeskPlusEIP\ETC\Jt\PopUp\JTList.htm

-리소스 추가
	\DeskPlusEIP\ETC\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-tw.resx

: 2018-12-06 by kkzz52 --------------------------------------------------------
[근태] 근태관리 추가
	HRProject 추가

	\DeskPlusEIP\App_Data\HR.xml 추가
	\DeskPlusEIP\ETC\Holiday\ListAdmin.aspx.cs
	\DeskPlusEIP\Legacy\Basic.aspx 
	\DeskPlusEIP\Legacy\Basic_Complete.aspx 
	\DeskPlusEIP\Legacy\Work.aspx 
	\DeskPlusEIP\Legacy\Work_Complete.aspx 
	\DeskPlusEIP\Legacy\WorkReport.aspx 
	\DeskPlusEIP\Legacy\WorkReport_Complete.aspx 
	\DeskPlusEIP\Legacy\HoliDay.aspx
	\DeskPlusEIP\Legacy\HoliDay_Complete.aspx 
	\DeskPlusEIP\Portlet\Package\Clock\js\jClocksGMT.js
	\DeskPlusEIP\Portlet\Package\HR\Attendance.ascx 추가
	\DeskPlusEIP\Portlet\Ws\Package\HR\Attendance.asmx 추가
	\DeskPlusEIP\Account\Js\Admin\User\Add.js
	\DeskPlusEIP\Account\Js\Admin\User\Mod.js
	\DeskPlusEIP\Account\Js\Admin\User\List.js
	\DeskPlusEIP\Account\Admin\User\Add.aspx
	\DeskPlusEIP\Account\Admin\User\Add.aspx.cs
	\DeskPlusEIP\Account\Admin\User\Mod.aspx
	\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs
	\DeskPlusEIP\Account\Admin\User\List.aspx
	\DeskPlusEIP\Account\Ws\Admin\User\List.asmx.cs
	\DeskPlusEIP\EIP\Js\Admin\Auth\Mod.js
	\DeskPlusEIP\EIP\Admin\Auth\Mod.aspx.cs
	\DeskPlusEIP\Html\Js\Select\bootstrap.css 추가
	\DeskPlusEIP\Html\Js\Select\chosen-sprite.png 추가
	\DeskPlusEIP\Html\Js\Select\chosen.jquery.js 추가
	\DeskPlusEIP\ETC\Js\ETCLegacy\WorkList.js
	\DeskPlusEIP\ETC\Jt\Work\JTAdminManage.htm
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\HR.cs 추가
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\AppSettingHelper.cs
	
	\Design\Dev2018\C~\Css\Func_HR.css 추가
	\Design\Dev2018\C~\Css\Func_ETC.css (#layout_holi_body, #layout_work_body)
	Web.config (zeoHRFolderStatusTree, zeoHRDeptTree 추가)

	-Account 리소스 추가
	Admin_User_Input_RetireDate
	Admin_User_Input_JoinDate

	-ETC 리소스 추가 및 수정
	HoliAdmin

	-Portlet 리소스 추가
	OnDuty
	MSG_SUCCESS
	WeekOverTime
	WeekOverTimeMsg
	Hour
	Attendance_EarlyConfirm
	Attendance_EarlyRe
	Attendance_EarlyTime
	Attendance_EarlyTimeMsg
	Attendance_EarlyCancelConfirm
	Attendance_GotoWorkRe
	Beforework
	Cancel
	GotoWork_Success 
	OffWorkEarly_Success 
	OffWork_Success 
	WSTime 
	WCTime 
	Early_Time 
	Location 

	-권한추가
	HR
	HRDept
	HRAttendance

	-근태 메뉴추가
	-야근계 양식명 연장근무신청서로 변경
	-휴가원 양식 수정 (pathid 추가)
	-연장근무신청서 양식 수정 (pathid 추가)
	-특근보고서 양식 수정 (pathid 추가)	

	insert DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate,updatedate)
	values ('HR', 'QRY_HR_FolderStatus_Sel','SELECT * FROM dbo.DP_HR_FolderStatus WITH(NOLOCK) WHERE CompanyID = @CompanyID AND ActiveFolder = ''1'' ORDER BY ParentFolderStatusID ASC, Orders ASC'
	,'kkzz52', '근태현황 트리', '20181120', '20181120')

	insert DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate,updatedate)
	values ('HR', 'QRY_HR_FolderStatus_SelByID','SELECT * FROM DP_HR_FolderStatus WITH(NOLOCK) WHERE CompanyID = @CompanyID AND ActiveFolder in ({0}) AND FolderStatusID= @FolderStatusID'
	,'kkzz52', 'ID 별 근태현황 폴더명 가져오기', '20181120', '20181120')

	insert DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate,updatedate)
	values ('HR', 'QRY_HR_FolderUser_Sel','SELECT * FROM DP_HR_FolderUser  WHERE CompanyID = @CompanyID AND FolderID = @FolderID AND UserID = @UserID AND Types = @Types'
	,'kkzz52', 'HR 사용자 설정 페이지 사이즈 가져오기', '20181120', '20181120')

	insert DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate,updatedate)
	values ('HR', 'QRY_HR_WorkEnv_Sel','SELECT * FROM DP_HR_WorkEnv  WHERE CompanyID = @CompanyID'
	,'kkzz52', 'HR 기본근무시간 설정 가져오기', '20181129', '20181129')

	INSERT INTO DP_QUERY (Category, qryid, qryvalue, owner, memo, regdate)
    VALUES ('HR', 'QRY_HR_IP_Sel', 'SELECT * FROM DP_HR_IP WHERE CompanyID = @CompanyID', 'kkzz52', '사내 IP 가져오기', '20181218')

	insert DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate,updatedate)
	values ('HR', 'QRY_HR_Attendance_SelByUserID','SELECT COUNT(*) FROM DP_HR_Attendance WHERE CompanyID = @CompanyID AND UserID = @UserID AND WorkDate = @WorkDate','kkzz52', '사용자 일자별 출퇴근 정보 가져오기', '20190417', '20190417')

	DP_ACC_User_BackUp_Upd 추가
	DP_ACC_User_Upd
	DP_EIP_UserRole_Del
	DP_ACC_Dept_Ins  ChildDept 버그 수정

	VW_UserInfo 수정 (PathID 추가)

	DP_HOLI_Used 테이블 수정 (DeptID, PathID 추가)
	DP_PAY_Report 테이블 수정 (DeptID, PathID 추가)

	DP_HR_Closing 테이블 추가
	DP_ACC_Dept_BackUp 테이블 추가
	DP_ACC_User_BackUp 테이블 추가
	DP_ACC_UserCareer_BackUp 테이블 추가
	DP_ACC_UserDept_BackUp 테이블 추가
	DP_ACC_UserEdu_BackUp 테이블 추가
	DP_ACC_UserFamily_BackUp 테이블 추가
	DP_ACC_UserReward_BackUp 테이블 추가
	DP_ACC_UserScholar_BackUp 테이블 추가
	DP_COM_Code_BackUp 테이블 추가

	DP_HR_Attendance 테이블 추가
	DP_HR_Role 테이블 추가
	DP_HR_FolderStatus 테이블 추가
	DP_HR_FolderUser 테이블 추가
	DP_HR_WorkEnv 테이블 추가
	DP_HR_WorkEtc 테이블 추가
	DP_HR_Holiday 테이블 추가
	DP_HR_Attendance_History 테이블 추가
	DP_HR_AttendanceRole 테이블 추가
	DP_HR_IP 테이블 추가
	DP_HR_UserDept 테이블 추가
	DP_HR_UserDept_History 테이블 추가
	VW_HR_Attendance 추가
	VW_HR_WorkEtc 추가
	VW_HR_UserDept 추가

	DP_HOLI_Cat 내용 수정
	
	ufn_GetMonthWeek 추가
	ufn_GetSubTree_HR 추가
	ufn_GetSubTree_HR_Back 추가
	ufn_GetSubTree_HR_Back_In 추가
	VW_UserInfo_BackUp 추가
	VW_ACC_Dept_BackUp 추가

	DP_HR_Closing_Ins 추가
	DP_HR_Closing_MonthExec 추가
	DP_HR_Closing_WeekExec 추가
	DP_HR_Closing_Sel 추가
	DP_HR_Closing_SelByDeptID 추가
	DP_HR_Closing_SelByUserDeptID 추가
	DP_HR_Closing_SelUserInfo 추가
	DP_HR_Closing_SelBaseDeptInfo 추가
	DP_HR_Closing_SelDeptInfo 추가
	DP_HR_Closing_SelUserInfo_Max 추가
	DP_HR_Closing_SelByUserID_Max 추가

	DP_HR_IP_Ins 추가
	DP_HR_IP_Del 추가
	DP_HR_FolderUser_InsUpd 추가
	DP_HR_Attendance_SelPrivateList 추가
	DP_HR_Attendance_SelPrivateCnt 추가
	DP_HR_Attendance_SelPrivateSUM 추가
	DP_HR_Attendance_SelDeptList 추가
	DP_HR_Attendance_SelDeptCnt 추가
	DP_HR_Attendance_SelDeptSUM 추가
	DP_HR_Attendance_SelCompanyList 추가
	DP_HR_Attendance_SelCompanyCnt 추가
	DP_HR_Attendance_SelCompanySUM 추가
	DP_HR_Attendance_InsExec 추가
	DP_HR_Role_Sel 추가
	DP_HR_Role_SelRootDept 추가
	DP_HR_WorkEnv_Upd 추가
	DP_HR_WorkEtc_Sel 추가
	DP_HR_WorkEtc_Ins 추가
	DP_HR_WorkEtc_Del 추가
	DP_HR_Role_SelStatusRoleList 추가
	DP_HR_Role_SelStatusRoleCnt 추가
	DP_HR_Role_SelByUserID 추가
	DP_HR_Role_Ins 추가
	DP_HR_Role_Del 추가
	DP_HR_WorkEnv_Exec 추가
	DP_PT_HR_SelAttendanceData 추가
	DP_PT_HR_Attendance_GotoWork_Ins 추가
	DP_PT_HR_Attendance_OffWork_Ins 추가
	DP_PT_HR_Attendance_PreOffWork_Ins 추가
	DP_PT_HR_Attendance_OffWork_Del 추가
	DP_PT_HR_SelAttendanceTime 추가
	DP_HR_Holiday_Ins 추가
	DP_HR_Holiday_Del 추가
	DP_HR_Attendance_UpdWCTime 추가
	DP_HR_Attendance_SelAttendanceList 추가
	DP_HR_Attendance_SelAttendanceCnt 추가
	DP_HR_Attendance_Upd 추가
	DP_HR_Attendance_SelByID 추가
	DP_HR_Attendance_History_SelPageList 추가
	DP_HR_AttendanceRole_SelStatusRoleList 추가
	DP_HR_AttendanceRole_SelStatusRoleCnt 추가
	DP_HR_AttendanceRole_SelByUserID 추가
	DP_HR_AttendanceRole_Sel 추가
	DP_HR_AttendanceRole_Ins 추가
	DP_HR_Attendance_SelBasicOver 추가
	DP_HR_Attendance_UpdBasicAtt 추가
	DP_HR_Attendance_UpdHolidayAtt 추가
	DP_HR_Attendance_UpdWCTimeByHoliday 추가
	DP_HR_Attendance_SelDeptHolyInfo 추가
	DP_HR_Attendance_SelDeptHolyList 추가
	DP_HR_UserDept_SelByNoCnt 추가
	DP_HR_UserDept_SelPageList 추가
	DP_HR_UserDept_Ins 추가
	DP_HR_UserDept_SelDeptID 추가
	DP_APP_DocProcess_SelPageCntReject 수정
	DP_APP_DocProcess_SelPageListReject 수정	
	DP_HOLI_Used_SelHRInfo 추가
	DP_HOLI_Used_SelHRPageCnt 추가
	DP_HOLI_Used_SelHRPageList 추가
	DP_HOLI_Used_SelSrhPageList 수정
	DP_HOLI_Used_SelSrhPageCnt 수정	

	-SQL 에이전트 추가
	1. 그룹웨어_근태인사마감_월별 
             - exec DP_HR_Closing_MonthExec
             - 매 1개월마다 마지막 일 오후 11:00:00에 발생합니다.
	2. 그룹웨어_근태인사마감_주별
             - exec DP_HR_Closing_WeekExec
             - 매주 일요일 오후 11:00:00에 발생합니다.
	3. 그룹웨어_근태출퇴근정보등록
             - exec DP_HR_Attendance_InsExec 1
             - 매일 오전 5:00:00에 발생합니다. 
	4. 그룹웨어_근태퇴근시간자동등록
             - exec DP_HR_WorkEnv_Exec 1
             - 매일 1 분(오전 12:00:00에서 오후 11:59:59 사이) 간격으로 발생합니다.

: 2018-12-04 by 이지원 --------------------------------------------------------
[휴일관리] 대체휴일사용여부 공휴일/토 옵션 수정
	\DeskPlusEIP\Common\Calendar\Calendar.ascx.cs
	\DeskPlusEIP\Common\Calendar\CalendarDialog.aspx.cs
	\DeskPlusEIP\Common\Calendar\CalendarDualDialog.aspx.cs
	\DeskPlusEIP\Community\Ws\Site\Apps\Schedule\List.asmx.cs
	\DeskPlusEIP\ETC\Attendance\UserListView.aspx.cs
	\DeskPlusEIP\ETC\Ws\Attendance\UserStatus.asmx.cs
	\DeskPlusEIP\HR\Ws\Content\ListPrivate.asmx.cs
	\DeskPlusEIP\Portlet\Package\HR\Attendance.ascx.cs
	\DeskPlusEIP\Portlet\Package\MyPim.ascx.cs
	\DeskPlusEIP\Portlet\Package\PimList.ascx.cs
	\DeskPlusEIP\Portlet\Ws\Package\HR\Attendance.asmx.cs
	\DeskPlusEIP\Pim\Menu\PimMenu.aspx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx.cs
	\DeskPlusEIP\PMS\Schedule\List.aspx.cs
	\DeskPlusEIP\RM\Home\Main.aspx.cs
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx.cs
	\DeskPlusEIP\Visit\Content\Tree.aspx.cs
	\DeskPlusEIP\Visit\Ws\Content\ListMonth.asmx.cs


: 2018-12-03 by ellepooky --------------------------------------------------------
[휴가관리] 엑셀 업로드 기능
	\DeskPlusEIP\ETC\Holiday\ListAdmin.aspx
	\DeskPlusEIP\ETC\Js\Holiday\ListAdmin.js
	\DeskPlusEIP\ETC\Ws\Holiday\ListAdmin.asmx.cs

	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BE\HoliAdminInfo.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BE\Zeospace.DeskPlus.ETC.BE.csproj
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BE\HoliInfo.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BLL\HoliAdminFile.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BLL\Zeospace.DeskPlus.ETC.BLL.csproj
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.DAL\HoliAdmin.cs
	\Projects\ComponentProject\Zeospace.DeskPlus.Component.Excel\OleExcelReader.cs

	\DeskPlusEIP\ETC\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-tw.resx

	DP_HOLI_File 테이블 추가

	DP_HOLI_Admin_Add 추가

	sample경로 : /DeskPlusEIP/FileData/1/Etc/Holiday/HoliAdd.xlsx

	-아래 2가지 설정되어 있어야 엑셀 업로드 가능
	*서버에 설치 (26.2MB짜리 설치)
	http://www.microsoft.com/ko-kr/download/details.aspx?id=13255
	*IIS 풀에 32비트 응용 프로그램 사용 True로 설정

: 2018-10-02 by kkzz52 --------------------------------------------------------
[전자결재] 알람 시 상신자정보가 수신자 언어로 안가는 오류 수정
	\DeskPlusEIP\Approval\App_Data\Confirm\en.xml
	\DeskPlusEIP\Approval\Process\Basic\Approce.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprocePopup.aspx.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\DeptCodeInfo.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.DAL\DeptCode.cs

: 2018-09-20 By kkzz52  ----------------------------------------------------------
[게시판] 댓글의 댓글 기능 추가
	\DeskPlusEIP\Board\Content\View.aspx
	\DeskPlusEIP\Board\Content\View.aspx.cs
	\DeskPlusEIP\Board\Content\ViewImpo.aspx
	\DeskPlusEIP\Board\Content\ViewImpo.aspx.cs
	\DeskPlusEIP\Board\Content\ViewInterested.aspx
	\DeskPlusEIP\Board\Content\ViewInterested.aspx.cs
	\DeskPlusEIP\Board\Content\ViewPopup.aspx
	\DeskPlusEIP\Board\Content\ViewPopup.aspx.cs
	\DeskPlusEIP\Board\Content\ViewPopupImpo.aspx
	\DeskPlusEIP\Board\Content\ViewPopupImpo.aspx.cs
	\DeskPlusEIP\Board\Content\ViewShortReplyList.aspx 추가
	\DeskPlusEIP\Board\Content\ViewShortReplyList.aspx.cs 추가
	\DeskPlusEIP\Board\Admin\Content\View.aspx
	\DeskPlusEIP\Board\Admin\Content\View.aspx.cs
	\DeskPlusEIP\Board\Admin\Category\Mod.aspx
	\DeskPlusEIP\Board\Admin\Category\Mod.aspx.cs
	\DeskPlusEIP\Board\Js\Content\View.js
	\DeskPlusEIP\Board\Js\Content\ViewImpo.js
	\DeskPlusEIP\Board\Js\Content\ViewInterested.js
	\DeskPlusEIP\Board\Js\Content\ViewShortReplyList.js 추가
	\DeskPlusEIP\Board\Js\Content\ShortReply.js
	\DeskPlusEIP\Board\Js\Content\ShortReplyMod.js
	\DeskPlusEIP\Board\Js\Admin\Content\View.js
	\DeskPlusEIP\Board\Js\Admin\Content\ShortReply.js
	\DeskPlusEIP\Board\Js\Admin\Content\ShortReplyMod.js
	\DeskPlusEIP\Board\Js\Content\List.js
	\DeskPlusEIP\Board\Js\Content\ListImpo.js
	\DeskPlusEIP\Board\Js\Content\ListInterested.js
	\DeskPlusEIP\Board\Js\Content\BlogList.js
	\DeskPlusEIP\Board\Js\Content\BlogListImpo.js
	\DeskPlusEIP\Board\Js\Content\BlogListInterested.js
	\DeskPlusEIP\Board\Js\Admin\Content\List.js
	\DeskPlusEIP\Board\Ws\Content\View.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\ViewImpo.asmx.cs
	\DeskPlusEIP\Board\Ws\Admin\Content\View.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\ShortReply.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\ShortReplyMod.asmx.cs
	\DeskPlusEIP\Board\Ws\Admin\Content\ShortReply.asmx.cs
	\DeskPlusEIP\Board\Ws\Admin\Content\ShortReplyMod.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\ShortReplyGood.asmx 추가
	\DeskPlusEIP\Board\Jt\Content\JTReple.htm
	\DeskPlusEIP\Board\Ws\Admin\Statistic\View.asmx.cs
	\DeskPlusEIP\Board\Ws\SetBoard.asmx.cs
	\BoardProject\Zeospace.DeskPlus.Board.BE\ShortReplyInfo.cs
	\BoardProject\Zeospace.DeskPlus.Board.BE\CategoryInfo.cs
	\BoardProject\Zeospace.DeskPlus.Board.BE\ShortReplyGoodInfo.cs 추가
	\BoardProject\Zeospace.DeskPlus.Board.DAL\ShortReply.cs
	\BoardProject\Zeospace.DeskPlus.Board.DAL\ShortReplyGood.cs 추가
	\BoardProject\Zeospace.DeskPlus.Board.DAL\Category.cs


	DP_BOA_ShortReply 테이블 수정
	update DP_BOA_ShortReply set ParentID = 0, GroupID = ShortReplyID, ListOrder = 0, Depth = 0, Active = '1' where
	 GroupID is null

	DP_BOA_Category 테이블 수정
	update DP_BOA_Category set OptionShortReplyGood = 0, OptionShortReplyTop = 0

	DP_BOA_ShortReplyGood 테이블 추가
	DP_BOA_ShortReply_Ins 수정
	DP_BOA_ShortReply_DelByID 수정
	DP_BOA_ShortReply_SelParentCnt 추가
	DP_BOA_ShortReplyGood_Ins 추가
	DP_BOA_ShortReplyGood_SelCnt 추가
	DP_BOA_Category_Ins 수정
	DP_BOA_Category_Upd 수정

	update DP_QUERY set qryvalue = 'SELECT Lists.*, 
		 DC.DeptBase, DC.DeptKo, DC.DeptEn, DC.DeptJa, DC.DeptFr, DC.DeptZhcn, DC.DeptZhtw,   DC.TitleBase, DC.TitleKo, DC.TitleEn, DC.TitleJa, DC.TitleFr,
		 DC.TitleZhcn, DC.TitleZhtw,   DC.DutyBase, DC.DutyKo, DC.DutyEn, DC.DutyJa, DC.DutyFr, DC.DutyZhcn, DC.DutyZhtw,
		 EV.AlarmBoard, EV.AlarmSNS,
		 (SELECT COUNT(*) FROM DP_BOA_ShortReplyGood WITH(NOLOCK) WHERE CompanyID = Lists.CompanyID AND CategoryID = Lists.CategoryID AND ListID = Lists.ListID AND ShortReplyID = Lists.ShortReplyID AND Good = 1) AS GoodCount,
		 (SELECT COUNT(*) FROM DP_BOA_ShortReplyGood WITH(NOLOCK) WHERE CompanyID = Lists.CompanyID AND CategoryID = Lists.CategoryID AND ListID = Lists.ListID AND ShortReplyID = Lists.ShortReplyID AND Bad = 1) AS BadCount
	FROM  DP_BOA_ShortReply AS Lists WITH(NOLOCK)
		LEFT OUTER JOIN DP_COM_DeptCode AS DC WITH(NOLOCK) ON Lists.DeptCodeID = DC.DeptCodeID		
		LEFT OUTER JOIN DP_ACC_UserEnv EV WITH(NOLOCK) ON Lists.CompanyID = EV.CompanyID AND Lists.UserID = EV.UserID
	WHERE Lists.CategoryID = @CategoryID   
		AND Lists.ListID = @ListID
	ORDER BY Lists.GroupID, Lists.ListOrder, Lists.InputDate' where 
	 qryid = 'QRY_BOA_ShortReply_Sel'


	-리소스
	AddShortReply 추가
	ReplyAdd 추가
	DelReply 추가
	MyComment 추가
	AlreadyComment 추가
	Recommend 추가
	Decommendation 추가
	UseComments 추가
	ShowTop 추가

	-CSS
	\Design\C\Css\DeskPlusEIPBase.css

	-아이콘 추가
	\Design\C\Img\Board\

: 2018-07-26 by kkzz52 ----------------------------------------------------------
[게시판] 임시저장 글올리기 시 알람을 사용하는 게시판일때만 모바일알람이 가도록 수정 
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\MyTemplate.cs

: 2018-07-26 by kkzz52 ----------------------------------------------------------
[게시판] 임시저장 글올리기 시 알람 안가는 오류 수정
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Lists.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs

: 2018-07-11 by 이지원 ----------------------------------------------------------
[게시판] 알람타입 4번(Mobile) 추가

	\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Js\Admin\Content\Add.js
	\DeskPlusEIP\Board\Admin\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Js\Admin\Content\Mod.js
	\DeskPlusEIP\Board\Admin\Content\Reply.aspx.cs
	\DeskPlusEIP\Board\Js\Admin\Content\Reply.js
		
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusEIP\Board\Common\TransferAppoval.aspx.cs
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Mod.js
	\DeskPlusEIP\Board\Content\Reply.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Reply.js


: 2018-06-29 by kkzz52 ----------------------------------------------------------
[게시판] 상단 메뉴 클릭 시 리스트 타입 반영 안되는 오류 수정
	\DeskPlusEIP\Board\Default.aspx.cs

: 2018-06-22 by famo23 --------------------------------------------------------
[업무협업]미리 지정된 수신자, 참조자의 부서가 변경되었을때 변경전 부서로 등록되는 문제
	\DeskPlusEIP\Task\Content\Request\Add.aspx.cs
	\DeskPlusEIP\Task\Ws\Content\Request\Add.asmx.cs

: 2018-05-25 by kkzz52 ------------------------------------------------------------
[전자결재] 회람함 인쇄 오류 수정
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReceiptList.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendList.js
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirReceiptList.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirSendList.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx.cs
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTCirReceiptList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTCirSendList.htm

: 2018-05-10 By famo23 ----------------------------------------------------------
[포탈] admin 사용자 아닌 경우 상단 profile/setting.. 스타일 이상한 문제 
	Func_PackageMenu.css

[포틀릿] 권한없는 게시판 포틀릿이 메인화면에 있을 경우 오류 발생
	\DeskPlusEIP\Portlet\Package\Board\List.ascx.cs

[공통]PDF 일괄다운로드 안되는 버그 수정 
	\DeskPlusEIP\Common\PDF\PDFDownLoad.aspx.cs

: 2018-05-03 by famo23 ----------------------------------------------------------------
[모바일관리]승인거부 카운트 수정 
	\DeskPlusEIP\MDM\Js\Auth\ListReject.js

: 2018-04-25 by 이지원  ----------------------------------------------------------
[근태관리] 근태관리 포틀릿추가
	--소스
	\DeskPlusEIP\Portlet\Package\Attendance.ascx
	\DeskPlusEIP\Portlet\Package\Attendance.ascx.cs
	\DeskPlusEIP\Portlet\Ws\Package\Attendance.asmx.cs
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\Attendance.cs

	\DeskPlusEIP\Portlet\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.zh-tw.resx

	--프로시져
	DP_PT_Attendance_SelAttendanceData
	DP_PT_Attendance_GotoWork_Ins
	DP_PT_Attendance_OffWork_Ins

: 2018-04-20 by 이중호  ----------------------------------------------------------
[공통]문서 스트리밍 적용
	\DeskPlusEIP\App_Data\BOARD.xml
	\DeskPlusEIP\App_Data\COMMUNITY.xml
	\DeskPlusEIP\App_Data\PIM.xml
	\DeskPlusEIP\App_Data\SNS.xml
	\DeskPlusEIP\App_Data\TASK.xml
	\DeskPlusEIP\App_Data\VISIT.xml
	\DeskPlusEIP\App_Data\WEBHARD.xml
	\DeskPlusEIP\Base\ApplicationFrameWorks\Helper\WebComponent\DocStream.cs

	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Temp\Add.aspx.cs

	\DeskPlusEIP\PIM\Schedule\Add.aspx.cs

	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx.cs

	\DeskPlusEIP\Community\Site\Apps\Board\Add.aspx.cs
	\DeskPlusEIP\Community\Site\Apps\Board\Mod.aspx.cs

	\DeskPlusEIP\WebHard\Ws\Content\List.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListIcon.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListPreview.asmx.cs
	\DeskPlusEIP\WebHard\Zeospace.DeskPlus.WebHard.BLL\File.cs
	\DeskPlusEIP\WebHard\Zeospace.DeskPlus.WebHard.DAL\File.cs

	\DeskPlusEIP\SNS\Zeospace.DeskPlus.SNS.BLL\MyTalk.cs

	\DeskPlusEIP\Task\Content\Request\Add.aspx.cs
	\DeskPlusEIP\Task\Content\Request\Mod.aspx.cs
	\DeskPlusEIP\Task\Content\Draft\Add.aspx.cs

	--테이블
	DP_DS_ATTACH 추가

	--프로시저
	DP_DS_Attach_Del
	DP_DS_Attach_Ins
	DP_DS_Attach_Sel
	DP_DS_Attach_UpdateFileID

: 2018-04-20 by 이지원  ----------------------------------------------------------
[근태관리] 근태관리 기능추가
	--소스
	\DeskPlusEIP\ETC\Attendance\AdminStatusD.aspx
	\DeskPlusEIP\ETC\Attendance\AdminStatusD.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\AdminStatusD.js
	\DeskPlusEIP\ETC\Jt\Attendance\JTAdminStatusD.htm
	\DeskPlusEIP\ETC\Attendance\AdminStatusM.aspx
	\DeskPlusEIP\ETC\Attendance\AdminStatusM.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\AdminStatusM.js
	\DeskPlusEIP\ETC\Jt\Attendance\JTAdminStatusM.htm
	\DeskPlusEIP\ETC\Attendance\AdminStatusY.aspx
	\DeskPlusEIP\ETC\Attendance\AdminStatusY.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\AdminStatusY.js
	\DeskPlusEIP\ETC\Jt\Attendance\JTAdminStatusY.htm
	\DeskPlusEIP\ETC\Ws\Attendance\AdminStatus.asmx
	\DeskPlusEIP\ETC\Ws\Attendance\AdminStatus.asmx.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BE\AttendanceAdminStatusInfo.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.DAL\AttendanceAdminStatus.cs

	\DeskPlusEIP\ETC\Attendance\UserDefault.aspx
	\DeskPlusEIP\ETC\Attendance\UserDefault.aspx.cs
	\DeskPlusEIP\ETC\Attendance\UserTree.aspx
	\DeskPlusEIP\ETC\Attendance\UserTree.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\UserTree.js
	\DeskPlusEIP\ETC\Attendance\UserList.aspx
	\DeskPlusEIP\ETC\Attendance\UserList.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\UserList.js
	\DeskPlusEIP\ETC\Attendance\UserListView.aspx
	\DeskPlusEIP\ETC\Attendance\UserListView.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\UserListView.js
	\DeskPlusEIP\ETC\Ws\Attendance\UserStatus.asmx.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.DAL\AttendanceUserStatus.cs

	\DeskPlusEIP\ETC\Attendance\UserStatusD.aspx
	\DeskPlusEIP\ETC\Attendance\UserStatusD.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\UserStatusD.js
	\DeskPlusEIP\ETC\Jt\Attendance\JTUserStatusD.htm
	\DeskPlusEIP\ETC\Attendance\UserStatusM.aspx
	\DeskPlusEIP\ETC\Attendance\UserStatusM.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\UserStatusM.js
	\DeskPlusEIP\ETC\Jt\Attendance\JTUserStatusM.htm
	\DeskPlusEIP\ETC\Attendance\UserStatusY.aspx
	\DeskPlusEIP\ETC\Attendance\UserStatusY.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\UserStatusY.js
	\DeskPlusEIP\ETC\Jt\Attendance\JTUserStatusY.htm
	\DeskPlusEIP\ETC\Ws\Attendance\UserStatus.asmx
	\DeskPlusEIP\ETC\Ws\Attendance\UserStatus.asmx.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BE\AttendanceUserStatusInfo.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.DAL\AttendanceUserStatus.cs

	--테이블
	DP_ETC_Attendance

	--뷰
	VW_ETC_Attendance

	--프로시져
	DP_ETC_Attendance_SelUserCnt
	DP_ETC_Attendance_SelUserList
	DP_ETC_Attendance_SelUserListView

	DP_ETC_Attendance_SelStatusDCnt
	DP_ETC_Attendance_SelStatusDList
	DP_ETC_Attendance_SelStatusMCnt
	DP_ETC_Attendance_SelStatusMList
	DP_ETC_Attendance_SelStatusYCnt
	DP_ETC_Attendance_SelStatusYList

	DP_ETC_Attendance_SelStatusUDCnt
	DP_ETC_Attendance_SelStatusUDList
	DP_ETC_Attendance_SelStatusUMCnt
	DP_ETC_Attendance_SelStatusUMList
	DP_ETC_Attendance_SelStatusUYCnt
	DP_ETC_Attendance_SelStatusUYList


	--관리자
	--소스
	\DeskPlusEIP\ETC\Attendance\AdminDefault.aspx
	\DeskPlusEIP\ETC\Attendance\AdminDefault.aspx.cs
	\DeskPlusEIP\ETC\Attendance\AdminTree.aspx
	\DeskPlusEIP\ETC\Attendance\AdminTree.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\AdminTree.js

	\DeskPlusEIP\ETC\Attendance\AdminRole.aspx
	\DeskPlusEIP\ETC\Attendance\AdminRole.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\AdminRole.js
	\DeskPlusEIP\ETC\Jt\Attendance\JTAdminRole.htm
	\DeskPlusEIP\ETC\Attendance\AdminRoleAdd.aspx
	\DeskPlusEIP\ETC\Attendance\AdminRoleAdd.aspx.cs
	\DeskPlusEIP\ETC\Js\Attendance\AdminRoleAdd.js
	\DeskPlusEIP\ETC\Jt\Attendance\JTAdminRoleAdd.htm
	\DeskPlusEIP\ETC\Ws\Attendance\AdminRole.asmx
	\DeskPlusEIP\ETC\Ws\Attendance\AdminRole.asmx.cs

	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BE\AttendanceAdminRoleInfo.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.DAL\AttendanceAdminRole.cs

	--테이블
	DP_ETC_Attendance_Role

	--프로시져
	DP_ETC_Attendance_SelStatusRoleCnt
	DP_ETC_Attendance_SelStatusRoleList
	DP_ETC_Attendance_AdminRole_ByUserID
	DP_ETC_Attendance_AdminRole_Del
	DP_ETC_Attendance_AdminRole_Ins
	DP_ETC_Attendance_AdminRoleDept_ByUserID


: 2018-04-12 by kkzz52  ----------------------------------------------------------
[자원관리] 자원관리 종료 버튼 추가
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs
	리소스

: 2018-04-11 By ellepooky  ----------------------------------------------------------
[게시판] 기본 검색조건 1년 -> 전체로 변경
	\DeskPlusEIP\Board\Content\List.aspx.cs
	\DeskPlusEIP\Board\Temp\List.aspx.cs
	\DeskPlusEIP\Board\Admin\Content\List.aspx.cs

: 2018-02-20 By 이지원  ----------------------------------------------------------
[일정관리] 공휴일관리 대체휴일 구분 추가
	* 공휴일관리
	--소스
	\DeskPlusEIP\Pim\Admin\HolidayR\Add.aspx
	\DeskPlusEIP\Pim\Admin\HolidayR\Add.aspx.cs
	\DeskPlusEIP\Pim\Admin\HolidayR\Mod.aspx
	\DeskPlusEIP\Pim\Admin\HolidayR\Mod.aspx.cs
	\DeskPlusEIP\Pim\Admin\HolidayR\List.aspx
	\DeskPlusEIP\Pim\Js\Admin\HolidayR\List.js
	\DeskPlusEIP\Pim\Jt\Admin\HolidayR\JTList.htm

	*공휴일보기	
	--전자결재
	\DeskPlusEIP\Common\Calendar\Calendar.ascx.cs
	\DeskPlusEIP\Common\Calendar\CalendarDualDialog.aspx.cs

	--포틀릿
	\DeskPlusEIP\Portlet\Package\MyPim.ascx.cs
	\DeskPlusEIP\Portlet\Package\PimList.ascx.cs

	--일정관리
	\DeskPlusEIP\Pim\Menu\PimMenu.aspx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx.cs

	--자원관리
	\DeskPlusEIP\RM\Home\Main.aspx.cs
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx.cs

	--커뮤니티
	\DeskPlusEIP\Community\Ws\Site\Apps\Schedule\List.asmx.cs

	--방문예약
	\DeskPlusEIP\Visit\Content\Tree.aspx.cs
	\DeskPlusEIP\Visit\Ws\Content\ListMonth.asmx.cs

	--프로젝트관리
	\DeskPlusEIP\PMS\Schedule\List.aspx.cs

: 2017-01-04 by kkzz52 ----------------------------------------------------------
[전자결재] 수신자가 결재자 없이 상신할 경우 완결 알람이 안가는 오류 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs (AddZQ)

: 2017-12-20 by kkzz52 ----------------------------------------------------------
[전자결재] 발신부서 전결 시에 수신(ZQ)가 있는데 참조자에게 알람이 가는 오류 수정
	DP_APP_Seq_SelAlarmComplete 수정

: 2017-11-14 by withsuki ----------------------------------------------------------
[전자결재] 조직도/트리 부서 클릭 시, 하위폴더 자동열림
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDeptUser.js
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDept.js
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDeptUserSelect.js
	\DeskPlusEIP\Approval\Js\Folder\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeReceive.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\Preference\FavoriteForm\Add.js


: 2017-11-10 by kkzz52 ---------------------------------------------------------
[포틀릿] 나의정보, Today현황 포틀릿 추가
	--나의정보
	\DeskPlusEIP\Portlet\Js\Package\MyInfoNoCnt.js 추가
	\DeskPlusEIP\Portlet\Package\MyInfoNoCnt.ascx 추가

	\DeskPlusEIP\EIP\Package\Main.aspx 수정
	\DeskPlusEIP\Account\MyInfo\SLO.aspx.cs 수정

	--Today현황
	\DeskPlusEIP\Portlet\Package\MyAppCount.ascx 추가
	\DeskPlusEIP\Portlet\Package\Img 추가

	\DeskPlusEIP\Approval\Folder\Default.aspx.cs 수정

	DP_PT_MyInfo_SelUserID 수정

: 2017-11-09 by withsuki ----------------------------------------------------------
[전자결재] 수신(ZQ)문서가 있는경우, 발신에서 결재의견 추가 시, 수신문서에 반영안됨
	D:\01_Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Approval\Js\Status\Common\View.js
	D:\01_Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Approval\Status\Common\View.aspx.cs
	D:\01_Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Approval\Ws\Status\Common\View.asmx.cs
	D:\01_Source\DeskPlusEIP40\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	
	DP_APP_DocProcess_UpdContent

: 2017-11-08 by withsuki ----------------------------------------------------------
[전자결재] 임시저장문서를 재임시저장할때, 협조가 최종에 오면 저장이 안됨
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js

: 2017-11-08 by withsuki ----------------------------------------------------------
[전자결재] 수정후재상신(반려후) 및 결재자 본문내용 수정 시, 체크박스,라디오버튼등이 활성화 되지 않음
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js

[전자결재] ZP수신반려 중, 홈화면 문서보기 에러
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx.cs

[포탈] 겸직이 많은경우, 스크롤처리.
	\DeskPlusEIP\EIP\Package\Menu.aspx

: 2017-10-31 by kkzz52 ---------------------------------------------------------
[포틀릿] SMS, 바로가기(임직원정보,주소록,커뮤니티,ERP), 최신글 포틀릿 추가
	\DeskPlusEIP\Portlet\Js\Package\SMSNew\SendSMSP.js 추가
	\DeskPlusEIP\Portlet\Js\Package\SMSNew\SpecialChar.js 수정
	\DeskPlusEIP\Portlet\Js\Package\SMSNew\CharList.js 수정
	\DeskPlusEIP\Portlet\Js\Package\SMSNew\RecentNumber.js 수정

	\DeskPlusEIP\Portlet\Package\Link\AccountInfo.aspx 추가
	\DeskPlusEIP\Portlet\Package\Link\Community.aspx 추가
	\DeskPlusEIP\Portlet\Package\Link\Address.aspx 추가
	\DeskPlusEIP\Portlet\Package\Link\ERP.aspx 추가
	\DeskPlusEIP\Portlet\Package\Board\ListTot.aspx 추가
	\DeskPlusEIP\Portlet\Ws\Package\Board\List.asmx.cs 수정

	\DeskPlusEIP\Portlet\Package\SMSNew\SendSMSP.aspx 추가
	\DeskPlusEIP\Portlet\Package\SMSNew\Sample\simple2.css 추가
	\DeskPlusEIP\Portlet\Package\SMSNew\SMS.ascx 수정
	 
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\Board.cs 수정

	DP_PT_Boa_SelTotPageList 추가

	\DeskPlusEIP\Portlet\Package\SMSNew\img\common\bg_phone_simple_portlet.gif 추가
	\DeskPlusEIP\Portlet\Package\SMSNew\img\common\bg_checkbox_portlet.gif 추가
	\DeskPlusEIP\Portlet\Package\SMSNew\img\button\btn_reset_portlet.gif 추가
	\DeskPlusEIP\Portlet\Package\SMSNew\img\button\btn_send_portlet.gif 추가

: 2017-10-31 by 이지원 -------------------------------------------------------------
[커뮤니티] 운영자 변경 수정
	--소스
	\DeskPlusEIP\Community\Main\Content.aspx
	\DeskPlusEIP\Community\Admin\Site\ListClose.aspx

	--프로시져
	DP_CMN_UserGroupMem_Upd

: 2017-10-19 by withsuki ----------------------------------------------------------
[전자결재] 수신자 반려 시, 홈화면 문서보기에서 조회권한없음으로 나옴
	DP_APP_Doc_SelAuthDoc

: 2017-09-08 by famo23 -------------------------------------------------------------
[자원관리] 제목 특수기호 변환 버그 수정
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs

: 2017-08-31 by shpark1325 ---------------------------------------------------------
[전자결재] 야근 특근계 연동문서 개발
	\DeskPlusEIP\Legacy\WorkReport.aspx
	\DeskPlusEIP\Legacy\WorkReport_Complete.aspx	
	\DeskPlusEIP\Legacy\Work.aspx
	\DeskPlusEIP\Legacy\Work_Complete.aspx
	\DeskPlusEIP\Legacy\Basic.aspx
	\DeskPlusEIP\Legacy\Basic_Complete.aspx
	
	\DeskPlusEIP\ETC\ETCLegacy\WorkList.aspx
	\DeskPlusEIP\ETC\ETCLegacy\WorkList.aspx.cs
	\DeskPlusEIP\ETC\Js\ETCLegacy\WorkList.js
	\DeskPlusEIP\ETC\Ws\ETCLegacy\WorkList.asmx.cs
	\DeskPlusEIP\ETC\Jt\ETCLegacy\JTWorkList.htm
	
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.fr.resx

	\DeskPlusEIP\Common\Calendar\CalendarDialog.aspx
	\DeskPlusEIP\Common\Calendar\CalendarDialog.aspx.cs

	\Projects\ETCProject\Zeospace.DeskPlus.ETC.DAL\PayWork.cs

	-프로시저
	DP_PAY_Work_SelPageCnt
	DP_PAY_Work_SelPageList
	DP_PAY_Work_SelPageWorkList

: 2017-08-31 by ellepooky ---------------------------------------------------------
[전자결재] 야근 특근 사용자 및 관리자 리스트 페이지 개발.
	\DeskPlusEIP\ETC\Work\AdminManage.aspx
	\DeskPlusEIP\ETC\Work\AdminManage.aspx.cs
	\DeskPlusEIP\ETC\Work\ListUser.aspx
	\DeskPlusEIP\ETC\Work\ListUser.aspx.cs

	\DeskPlusEIP\ETC\Js\Work\AdminManage.js
	\DeskPlusEIP\ETC\Js\Work\ListUser.js
	\DeskPlusEIP\ETC\Jt\Work\JTAdminManage.htm
	\DeskPlusEIP\ETC\Jt\Work\JTListUser.htm

	\DeskPlusEIP\ETC\Ws\Work\AdminManage.asmx.cs
	\DeskPlusEIP\ETC\Ws\Work\ListUser.asmx.cs

	\Projects\ETCProject\Zeospace.DeskPlus.ETC.BE\WorkReportInfo.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.DAL\WorkReport.cs
	\Projects\ETCProject\Zeospace.DeskPlus.ETC.DAL\WorkCat.cs

	\DeskPlusEIP\ETC\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-tw.resx

	--테이블
	DP_PAY_Report
	DP_PAY_Work
	DP_PAY_WorkCat

	-프로시저
	DP_Pay_ReportAdmin_AmountSum
	DP_PAY_Report_SelPageList
	DP_PAY_Report_SelPageCnt
	DP_Pay_Report_SelAllList
	DP_Pay_Report_SelSrhPageCnt
	DP_Pay_ReportAdmin_SelSrhPageCnt
	DP_Pay_Report_SelSrhPageList
	DP_Pay_ReportAdmin_SelSrhPageList
	DP_Pay_WorkPay_Upd
	DP_Pay_WorkPayCancel_Upd

	--쿼리
	QRY_Work_Cat_Sel

: 2017-08-24 by kkzz52 ---------------------------------------------------------
[전자결재] 상신문서함에서 상신 시에 근거문서 오류 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js

: 2017-08-16 by famo23 -------------------------------------------------------------
[포틀릿] 임직원조회에서 퇴사한 사용자 검색되는 버그수정
	DP_ACC_User_SelSrhUser

: 2017-08-03 by famo23 -------------------------------------------------------------
[일정관리] 리스트 스타일 수정
	\DeskPlusEIP\Pim\Jt\Admin\HolidayD\JTList.htm

: 2017-08-02 by withsuki ----------------------------------------------------------
[회사관리] 주소부분에 특수문자 깨짐
	\DeskPlusEIP\EIP\Admin\Company\Add.aspx
	\DeskPlusEIP\EIP\Admin\Subsidiary\Add.aspx

: 2017-07-28 by ellepooky -------------------------------------------------------------
[전자결재] 결재의견 추가시 문서내용 가져오도록 수정 
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Process\Opinion\ConfirmOpinion.js
	\DeskPlusEIP\Approval\Ws\Folder\Common\View.asmx.cs 

: 2017-07-28 by famo23 -------------------------------------------------------------
[웹폴더] 모아보기에서 관리자 버튼 안 보이게 수정 
	\DeskPlusEIP\WebHard\Content\List.aspx.cs
	\DeskPlusEIP\WebHard\Content\ListIcon.aspx.cs
	\DeskPlusEIP\WebHard\Content\ListPreview.aspx.cs

: 2017-07-26 by 이지원 -------------------------------------------------------------
[전자결재] 관리자 문서보기 열람권한보기 추가
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx.cs
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js

	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx.cs
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js

: 2017-07-25 by famo23 -------------------------------------------------------------
[포틀릿] 일정 최소화 버그 수정
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_Package.css
	\DeskPlusEIP\Portlet\Package\PimList.ascx

[게시판,주소록,웹폴더] 중요표시 트리 선택후 다른 트리 선택시 한번 클릭하면 열리고 한번 클릭하면 닫히는 부분 수정 (영업요청)
	\DeskPlusEIP\Board\Js\Category\Tree.js	
	\DeskPlusEIP\Address\Js\Folder\Tree.js
	\DeskPlusEIP\WebHard\Js\Folder\Tree.js

: 2017-07-19 by kkzz52 -------------------------------------------------------------
[전자결재] 결재 홈화면에서 중간결재자에게 반려한 문서보기 시 오류 수정
	UPDATE DP_QUERY SET qryvalue ='SELECT UpPathID FROM VW_APP_DocTot WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID' WHERE qryid = 'QRY_APP_Doc_SelUpPath'

: 2017-07-19 by 이지원 -------------------------------------------------------------
[게시판] 템플릿관리 수정
	\DeskPlusEIP\Board\Js\Admin\Template\List.js

: 2017-07-19 by 이지원 -------------------------------------------------------------
[게시판] 통계수정
	\DeskPlusEIP\Board\Admin\Statistic\LineChart.aspx
	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx

: 2017-07-14 by 방영환 -------------------------------------------------------------
[게시판] PDF일괄 다운로드 오류 수정
	\DeskPlusEIP\Common\PDF\PDFCreate.aspx.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\CreatePDF.cs

: 2017-07-14 by famo23 -------------------------------------------------------------
[게시판] 게시글 삭제시 댓글은 삭제 안되는 버그 수정
	BOA_List_Del
	BOA_List_DelByUserID

[웹폴더] 중요표시 미리보기페이지에서 이미지 아이콘 선택시 권한없는데 다운되는 버그 수정
	\DeskPlusEIP\WebHard\Jt\Content\JTListImpoIcon.htm
	\DeskPlusEIP\WebHard\Jt\Content\JTListImpoPreview.htm

: 2017-07-12 by 방영환 -------------------------------------------------------------
[전자결재] 외부 문서 작성페이지 호출시 트리에 참조열람 추가
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx 
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx.cs

: 2017-07-12 by famo23 -------------------------------------------------------------
[웹폴더] 휴지통 파일형/앨범형 리스트에서 용량 크기 나오는 방식 버그 수정 
	\DeskPlusEIP\WebHard\Ws\Trash\ListIcon.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Trash\ListPreview.asmx.cs

: 2017-07-11 by famo23 -------------------------------------------------------------
[포틀릿] 영어버전일때 자원관리 리스트 버그 수정
	\DeskPlusEIP\Portlet\Ws\Package\Resource.asmx.cs

: 2017-07-11 by withsuki -------------------------------------------------------------
[게시판] 관리자 - 폴더관리 스타일변경 (항목간 GAP)
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Category\Mod.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Category\ModTot.aspx
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_Board.css


: 2017-07-11 by withsuki -------------------------------------------------------------
[전자결재] 버그수정 및 개선 
	- 열람권한 기본값지정
	- 버그수정 :열람권한 활성/비활성화 버그수정.
				열람권한이 변경되어, 셋팅값에 없는 경우. (기존에는 디폴트로 1등급으로 처리되어있음 => 기본설정값으로 반영되도록 수정)
	DP_APP_AccessGrade_UpdBase
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\AccessGrade.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\AccessGradeInfo.cs
	\DeskPlusEIP\Approval\Admin\AccessGrade\List.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeModReceive.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceive.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveEdit.aspx
	\DeskPlusEIP\Approval\Js\Admin\AccessGrade\List.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeEdit.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeLegacy.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeMod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeModReceive.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeReceiveEdit.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Jt\Admin\AccessGrade\JtList.htm
	\DeskPlusEIP\Approval\Resource\GlobalString\
	\DeskPlusEIP\Approval\Ws\Admin\AccessGrade\List.asmx


: 2017-07-10 by famo23 -------------------------------------------------------------
[포틀릿] '축하합니다' 날짜 짤려서 보이는 부분 수정
	\DeskPlusEIP\Portlet\Jt\Package\JTCongratulation.htm

: 2017-07-10 by withsuki -------------------------------------------------------------
[전자결재] 버그수정 - 홈화면 자주지정하는결재자, 부서클릭
	\DeskPlusEIP\Approval\Ws\Home\Content.asmx.cs
	\DeskPlusEIP\Approval\Jt\Home\JTFavApp.htm

: 2017-07-06 by famo23 -------------------------------------------------------------
[자원관리] 리스트 스타일 변경안된 것 수정
	\DeskPlusEIP\RM\Jt\Content\JTReject.htm

: 2017-07-06 by 이지원 -------------------------------------------------------------
[웹폴더] 메인화면 추가 (기획업무)
	\DeskPlusEIP\WebHard\Default.aspx
	\DeskPlusEIP\WebHard\Js\Folder\Tree.js

	\DeskPlusEIP\WebHard\Content\Home.aspx
	\DeskPlusEIP\WebHard\Content\Home.aspx.cs
	\DeskPlusEIP\WebHard\Js\Content\Home.js
	\DeskPlusEIP\WebHard\Jt\Content\JTListNewPublic.htm
	\DeskPlusEIP\WebHard\Jt\Content\JTListNewSend.htm
	\DeskPlusEIP\WebHard\Jt\Content\JTListNewShare.htm

	\DeskPlusEIP\WebHard\Content\Search.aspx
	\DeskPlusEIP\WebHard\Content\Search.aspx.cs
	\DeskPlusEIP\WebHard\Js\Content\Search.js
	\DeskPlusEIP\WebHard\Jt\Content\JTSearch.htm

	\DeskPlusEIP\WebHard\Ws\Content\List.asmx.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.DAL\File.cs

	DP_WHD_FileList_Public_SelNewList
	DP_WHD_FileList_Share_SelNewList
	DP_WHD_FileList_Send_SelNewList
	DP_WHD_File_SelSearchCount
	DP_WHD_File_SelSearchList

: 2017-07-05 by famo23 -------------------------------------------------------------
[커뮤니티] 새창 스크롤 디자인 수정
	\DeskPlusEIP\Community\Js\Site\Apps\Board\List.js
	\DeskPlusEIP\Community\Site\Apps\Board\ViewPopup.aspx

[자원관리] 삭제후 페이지 버그 수정
	\DeskPlusEIP\RM\Content\View.aspx.cs
	
: 2017-06-27 by withsuki -------------------------------------------------------
[인사정보] 요약정보 기능 수정 (기획업무)
	DP_ACC_User_SelSummaryAppCom
	DP_ACC_User_SelSummaryAppIng
	DP_ACC_User_SelSummaryBoaCategory
	DP_ACC_User_SelSummaryBoaList
	DP_ACC_User_SelSummaryCmnBoa
	DP_ACC_User_SelSummaryCmnMgr
	DP_ACC_User_SelSummaryTask

	\DeskPlusEIP\Account\Resource\GlobalString
	\DeskPlusEIP\Account\Admin\User\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Etc
	\DeskPlusEIP\Account\Js\MyInfo\Etc
	\DeskPlusEIP\Account\Js\MyInfo\Mod.js
	\DeskPlusEIP\Account\Jt\MyInfo\Etc\JTLoginInfo.htm
	\DeskPlusEIP\Account\Ws\MyInfo\Etc
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs


: 2017-06-23 by 이지원 -------------------------------------------------------------
[웹폴더] 공용폴더기능추가 (기획업무)
	--소스
	\DeskPlusEIP\WebHard\Folder\Tree.aspx
	\DeskPlusEIP\WebHard\Folder\Tree.aspx.cs
	\DeskPlusEIP\WebHard\Js\Folder\Tree.js
	\DeskPlusEIP\WebHard\Folder\TreeAdmin.aspx
	\DeskPlusEIP\WebHard\Folder\TreeAdmin.aspx.cs
	\DeskPlusEIP\WebHard\Js\Folder\TreeAdmin.js

	\DeskPlusEIP\WebHard\Public\Add.aspx.cs
	\DeskPlusEIP\WebHard\Js\Folder\Add.js

	\DeskPlusEIP\WebHard\Common\WebHardTree.ascx.cs
	\DeskPlusEIP\WebHard\Common\GetWebHardTree.aspx.cs

	\DeskPlusEIP\WebHard\Folder\GetPublicFolder.aspx
	\DeskPlusEIP\WebHard\Folder\GetPublicFolder.aspx.cs
	\DeskPlusEIP\WebHard\Folder\GetFolderTree.aspx.cs
	\DeskPlusEIP\WebHard\Js\Folder\GetPublicFolder.js
	\DeskPlusEIP\WebHard\Folder\GetFolderTree.aspx.cs

	\DeskPlusEIP\WebHard\Content\List.aspx
	\DeskPlusEIP\WebHard\Content\List.aspx.cs
	\DeskPlusEIP\WebHard\Js\Content\List.js
	\DeskPlusEIP\WebHard\Ws\Content\List.asmx.cs
	\DeskPlusEIP\WebHard\Content\ListIcon.aspx
	\DeskPlusEIP\WebHard\Content\ListIcon.aspx.cs
	\DeskPlusEIP\WebHard\Js\Content\ListIcon.js
	\DeskPlusEIP\WebHard\Ws\Content\ListIcon.asmx.cs
	\DeskPlusEIP\WebHard\Content\ListPreview.aspx
	\DeskPlusEIP\WebHard\Content\ListPreview.aspx.cs
	\DeskPlusEIP\WebHard\Js\Content\ListPreview.js
	\DeskPlusEIP\WebHard\Ws\Content\ListPreview.asmx.cs

	\DeskPlusEIP\WebHard\Public\Add.aspx
	\DeskPlusEIP\WebHard\Public\Add.aspx.cs
	\DeskPlusEIP\WebHard\Js\Public\Add.js
	\DeskPlusEIP\WebHard\Js\Content\FavoriteAdd.js
	\DeskPlusEIP\WebHard\Js\Content\FavoriteCopy.js

	\DeskPlusEIP\WebHard\Trash\List.aspx
	\DeskPlusEIP\WebHard\Trash\List.aspx.cs
	\DeskPlusEIP\WebHard\Js\Trash\List.js
	\DeskPlusEIP\WebHard\Trash\ListIcon.aspx
	\DeskPlusEIP\WebHard\Trash\ListIcon.aspx.cs
	\DeskPlusEIP\WebHard\Js\Trash\ListIcon.js
	\DeskPlusEIP\WebHard\Trash\ListPreview.aspx
	\DeskPlusEIP\WebHard\Trash\ListPreview.aspx.cs
	\DeskPlusEIP\WebHard\Js\Trash\ListPreview.js

	\DeskPlusEIP\WebHard\Folder\CopyMove.aspx
	\DeskPlusEIP\WebHard\Folder\CopyMove.aspx.cs
	\DeskPlusEIP\WebHard\Js\Folder\CopyMove.js

	\DeskPlusEIP\WebHard\Base\BasePageWebHard.cs	
	\DeskPlusEIP\Upload\DPUpload\Main.aspx.cs
	\DeskPlusEIP\Upload\JSUpload\Main.aspx.cs

	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.BLL\Folder.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.DAL\Folder.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.BE\PublicFolderInfo.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.BLL\PublicFolder.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.DAL\PublicFolder.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.DAL\File.cs

	\DeskPlusEIP\WebHard\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.zh-tw.resx

	--테이블
	DP_WHD_Folder
	DP_WHD_PublicFolder

	--프로시져
	DP_WHD_Folder_Ins
	DP_WHD_Folder_Upd
	DP_WHD_Folder_Del
	DP_WHD_PublicFolder_Sel
	DP_WHD_PublicFolder_SelByFID
	DP_WHD_PublicFolder_Ins
	DP_WHD_PublicFolder_Del
	DP_WHD_File_Del
	DP_WHD_File_SelTrashPageList
	DP_WHD_File_SelTrashPageCnt
	DP_WHD_File_SelByUserID
	DP_WHD_File_SelByFoldrID

	--쿼리
	QRY_WHD_Folder_Sel
	QRY_WHD_FolderU_Sel
	QRY_WHD_PublicFolder_Sel

: 2017-06-23 by famo23 -------------------------------------------------------------
[주소록] 페이징이나 검색했을 시에 겸직정보가 안 보이는 버그 수정
	DP_ACC_User_SelSrhUserPageCnt
	DP_ACC_User_SelSrhPageList

: 2017-06-22 by maniaysw ---------------------------------------------------------------
[게시판] 첨부파일 외부 다운로드 불가 
	\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Content\View.aspx
	\DeskPlusEIP\Board\Content\View.aspx.cs
	\DeskPlusEIP\Board\Js\Content\View.js
	\DeskPlusEIP\Board\Admin\Category\Mod.aspx
	\DeskPlusEIP\Board\Admin\Category\Mod.aspx.cs

	\DeskPlusEIP\Common\DownLoad\DownLoad.aspx.cs
	\DeskPlusEIP\Common\DownLoad\TotalDownLoad.aspx.cs

	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\CategoryInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\ListEtc.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\Category.cs

	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx

	\DeskPlusEIP\Common\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.zh-tw.resx

	[테이블]
	DP_BOA_Category

	[프로시저]
	DP_BOA_Category_Ins
	DP_BOA_Category_Upd

	[DPQuery]
	QRY_BOA_ListEtc_SelByListID

: 2017-06-22 by shpark1325 -------------------------------------------------------------
[전자결재]타부서문서열람시 수신문서함 문서열람 불가 오류수정
\DeskPlusEIP\Approval\Jt\Folder\OtherDept\JTReceiptList.htm

: 2017-06-21 by famo23 -------------------------------------------------------------
[업무협업]수신자,참조자 변경저장 버그수정
	\DeskPlusEIP\Task\Js\Content\Receive\View.js
	\DeskPlusEIP\Task\Js\Home\View.js


: 2017-06-21 by freeman -------------------------------------------------------
[공통] 표준 시간대 기본 정보 입력 (개발 방향 정해진 후 오픈)
	\DeskPlusSolution\DeskPlusEIP\Account\MyInfo\Mod.aspx (주석처리) 표준시간대 표시 않되도록
    \DeskPlusSolution\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs 

   \Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserInfo.cs (BaseTimeZone 추가)
   \Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs (BaseTimeZone 추가)

   \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\Authenticator.cs
   \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\IUserEnvironment.cs
   \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBaseControl.cs
   \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBasePage.cs
   \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentMasterPage.cs
   \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserInfo.cs

   \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Base\BasePage.cs

   \DeskPlusSolution\DeskPlusEIP\EIP\Package\Menu.aspx.cs (주석 처리함)

: 2017-06-20 by withsuki -------------------------------------------------------
[일정관리] 일정을 상세가 아닌 리스트에서 수정 시(심플입력), 모든 일정에 반영됨.
			-> 해당 내용을 alert처리 함.
           (상세보기에서 수정 시, 반영범위를 선택할 수 있음.)
Source>
	\DeskPlusEIP\Pim\Js\Schedule\ListDay.js
	\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
	\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js
	\DeskPlusEIP\Pim\Schedule\ListDay.aspx
	\DeskPlusEIP\Pim\Schedule\ListWeek.aspx
	\DeskPlusEIP\Pim\Schedule\ListMonth.aspx
	\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx.cs
	\DeskPlusEIP\Pim\Resource\GlobalString\


: 2017-06-20 by withsuki -------------------------------------------------------
[전자결재] 문서이관-관리자 기능 수정 (기획업무)

Design>
	Css\Func_Approval.css

Source>
	\DeskPlusEIP\Html\Js\jQuery\jquery.list.pager2.js
	\DeskPlusEIP\Account\Common\DeptTreeScript.ascx
	\DeskPlusEIP\Approval\Common\Popup\GetDept.aspx
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDept.js
	\DeskPlusEIP\Approval\Admin\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Admin\Transfer\
	\DeskPlusEIP\Approval\Jt\Admin\Transfer\
	\DeskPlusEIP\Approval\Ws\Admin\Transfer\List.asmx
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CarbonCopyUser.cs

DB>
	(추가)DP_APP_Doc_SelPageListCarbonAdminDept
	(추가)DP_APP_Doc_SelPageCntCarbonAdminDept
	(추가)DP_APP_CarbonCopyUser_UpdTransferDept
	(추가)DP_APP_CarbonCopyUser_InsTransferDept
	(변경)DP_APP_CarbonCopyUser_UpdTransfer
	(변경)DP_APP_CarbonCopyUser_InsTransfer


: 2017-06-14 by 이지원 -------------------------------------------------------
[전자결재] 열람등급관리 (기획업무)
	--열람등급관리
	--소스
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\Menu\Tree.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\Menu\Tree.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\Menu\Tree.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\AccessGrade\List.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\AccessGrade\List.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\AccessGrade\List.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Jt\Admin\AccessGrade\JtList.htm
	\DeskPlusSolution\DeskPlusEIP\Approval\Ws\Admin\AccessGrade\List.asmx.cs	
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\AccessGrade\ModSequence.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\AccessGrade\ModSequence.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\AccessGrade\ModSequence.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Ws\Admin\AccessGrade\ModSequence.asmx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\AccessGrade\Add.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\AccessGrade\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\AccessGrade\Add.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\AccessGrade\Mod.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Admin\AccessGrade\Mod.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\AccessGrade\Mod.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Jt\Admin\AccessGrade\JtTitleList.htm
	\DeskPlusSolution\DeskPlusEIP\Approval\Ws\Admin\AccessGrade\Mod.asmx.cs

	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\AccessGradeInfo.cs
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\AccessGrade.cs
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\AccessGrade.cs

	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx

	--테이블
	DP_APP_AccessGrade

	--프로시져
	DP_APP_AccessGrade_Del
	DP_APP_AccessGrade_Ins
	DP_APP_AccessGrade_IsAccessList
	DP_APP_AccessGrade_SelByAccessID
	DP_APP_AccessGrade_SelPageCnt
	DP_APP_AccessGrade_SelPageList
	DP_APP_AccessGrade_Title_Add
	DP_APP_AccessGrade_Title_Del
	DP_APP_AccessGrade_TitleSelByAccessID
	DP_APP_AccessGrade_Upd
	DP_APP_AccessGrade_UpdIsActive
	DP_APP_AccessGrade_UpdOrder
	DP_APP_AccessGrade_Check

	--직책등록
	--소스
	\DeskPlusSolution\DeskPlusEIP\Account\Admin\Position\List.aspx
	\DeskPlusSolution\DeskPlusEIP\Account\Admin\Position\List.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Account\Js\Admin\Position\List.js

	\DeskPlusSolution\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\CodeInfo.cs
	\DeskPlusSolution\Projects\CommonProject\Zeospace.DeskPlus.Common.DAL\Code.cs

	\DeskPlusSolution\DeskPlusEIP\Account\Resource\GlobalString\string.en.resx
	\DeskPlusSolution\DeskPlusEIP\Account\Resource\GlobalString\string.fr.resx
	\DeskPlusSolution\DeskPlusEIP\Account\Resource\GlobalString\string.ja.resx
	\DeskPlusSolution\DeskPlusEIP\Account\Resource\GlobalString\string.ko.resx
	\DeskPlusSolution\DeskPlusEIP\Account\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusSolution\DeskPlusEIP\Account\Resource\GlobalString\string.zh-tw.resx

	--프로시져
	DP_COM_Code_Ins
	DP_COM_Code_Upd

	--전자결재
	--소스
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceive.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceive.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveEdit.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveEdit.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Status\Common\View.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Folder\Common\AccessGrade.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Folder\Common\AccessGrade.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Folder\Common\AccessGrade.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Jt\Folder\Common\JTAccessGrade.htm

	--프로시져
	DP_APP_Doc_SelAuthDoc
	DP_APP_Doc_SelPageCntDept
	DP_APP_Doc_SelPageListDept
	DP_APP_Doc_SelPageCntDeptAll
	DP_APP_Doc_SelPageListDeptAll
	DP_APP_Doc_SelPageCntDeptFolder
	DP_APP_Doc_SelPageListDeptFolder
	DP_APP_Doc_SelPageCntRecv
	DP_APP_Doc_SelPageListRecv
	DP_APP_Doc_SelPageCntSend
	DP_APP_Doc_SelPageListSend
	DP_APP_Doc_SelTreeDept
	DP_APP_Doc_SelTreeDeptRecv

	DP_SEARCH_ApprovalUser_Schedular
	DPM_APP_Doc_SelPublicCnt
	DPM_APP_Doc_SelPublic
	DPM_APP_Doc_TopDown_Public
	
	--뷰
	VW_Search_APP_GetDocumentIDByUserID

: 2017-06-14 by freeman ----------------------------------------------------------
[공통] 마우스 클릭 위치에서 모달 팝업 생성
     - 일정관리 리스트가 길어 질때 클릭 위치에서 팝업 생성

	 \DeskPlusSolution\DeskPlusEIP\Html\Js\JModal\DeskPlusModal.js (FixModalDesign / centerPopupFix)
	 \DeskPlusSolution\DeskPlusEIP\Pim\Js\Schedule\ListDay.js  


: 2017-05-30 by 방영환 ----------------------------------------------------------
[자원관리] 리스트에서 엑셀다운로드시 데이터가 없을경우 에러
	리소스 추가)
	- SearchNoResultData
	\DeskPlusEIP\RM\Resource\GlobalString\string.en.resx	
	\DeskPlusEIP\RM\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-tw.resx

: 2017-05-29 by 이지원 ----------------------------------------------------------
[게시판] 다국어 수정
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Mod.js
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Reply.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Content\ReplyN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Reply.js

	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Category\Mod.aspx

	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Add.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\AddN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Add.js
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Mod.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\ModN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Mod.js
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Reply.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\ReplyN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Reply.js

: 2017-05-18 by 방영환 ----------------------------------------------------------
[일정관리, 업무협업, 업무일지] 개인알람 수정
	소스 수정)
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\Mod.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Pim\Anniversary\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Pim\Anniversary\Mod.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Pim\Schedule\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs

	\DeskPlusSolution\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\Schedule.cs
	\DeskPlusSolution\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\ScheduleReply.cs
	\DeskPlusSolution\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\ShoryReply.cs
	\DeskPlusSolution\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\ToDoReply.cs

	\DeskPlusSolution\Projects\PeportProject\Zeospace.DeskPlus.Report.BLL\List.cs
	\DeskPlusSolution\Projects\TaskProject\Zeospace.DeskPlus.Task.BLL\Alarm.cs

: 2017-05-18 by 이지원 ----------------------------------------------------------
[게시판] 개인알람설정 수정
	-- 소스
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Lists.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\MyTemplate.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\ShortReply.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\ListAlarm.cs

	--프로시져
	DP_BOA_ListAlarm_SelByID
	DP_BOA_ListAlarm_SelByCompanyID
	DP_BOA_ListAlarmSMS_SelByID

: 2017-05-16 by maniaysw ---------------------------------------------------------
[메뉴] 겸직 선택 팝업 띄우고 슬라이드 메뉴를 닫으면 겸직 선택 팝업이 닫히도록 요청 (영업팀)
	\DeskPlusEIP\EIP\Js\Package\Menu.js

: 2017-05-16 by freeman ----------------------------------------------------------
[게시판] 다국어로 변경시 홈화면에서작성자명이 다국어로 변경되지 않는 버그 수정 

     \DeskPlusSolution\DeskPlusEIP\Board\Home\Content.aspx.cs

	 \DeskPlusSolution\DeskPlusEIP\Board\Ws\Home\List.asmx.cs


: 2017-05-15 by freeman ----------------------------------------------------------
[주소록] 중요표시에서 블로그 모드일때 본인 사진으로 도배되는 버그

     \DeskPlusSolution\Projects\AddressProject\Zeospace.DeskPlus.Address.BLL\List.cs 

: 2017-05-15 by 이지원 ----------------------------------------------------------
[게시판] 답글 알림설정 및 링크 URL 추가
	\DeskPlusEIP\Board\Admin\Category\Mod.aspx

	\DeskPlusEIP\Board\Admin\Content\Reply.aspx
	\DeskPlusEIP\Board\Admin\Content\ReplyN.aspx
	\DeskPlusEIP\Board\Admin\Content\Reply.aspx.cs
	\DeskPlusEIP\Board\Js\Admin\Content\Reply.js

	\DeskPlusEIP\Board\Content\Reply.aspx
	\DeskPlusEIP\Board\Content\ReplyN.aspx
	\DeskPlusEIP\Board\Content\Reply.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Reply.js

	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx

: 2017-05-08 by maniaysw ----------------------------------------------------------
[사용자정보] 본인권한 표기 및 로그인 접속이력 버튼 추가
	\DeskPlusEIP\Account\MyInfo\AuthInfo.aspx
	\DeskPlusEIP\Account\MyInfo\AuthInfo.aspx.cs
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs
	\DeskPlusEIP\Account\Ws\MyInfo\LoginInfo.asmx
	\DeskPlusEIP\Account\Ws\MyInfo\LoginInfo.asmx.cs
	\DeskPlusEIP\Account\Js\MyInfo\AuthInfo.js
	\DeskPlusEIP\Account\Js\MyInfo\Mod.js
	\DeskPlusEIP\Account\Jt\MyInfo\JTLoginInfo.htm
	Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs

	\DeskPlusEIP\Account\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-tw.resx

	DP_ACC_User_SelAuth
	DP_ACC_SelLoginInfoCnt
	DP_ACC_SelLoginInfoList

: 2017-05-04 by 이지원 ----------------------------------------------------------
[게시판] 답글 알람오류 수정
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Reply.aspx.cs

: 2017-04-28 by 이지원 ----------------------------------------------------------
[커뮤니티,자원관리,사내SNS] 개인알람설정 수정
	--커뮤니티
	\DeskPlusSolution\Projects\CommunityProject\Zeospace.DeskPlus.Community.BLL\Alarm.cs
	\DeskPlusSolution\Projects\CommunityProject\Zeospace.DeskPlus.Community.BLL\Com.cs
	\DeskPlusSolution\Projects\CommunityProject\Zeospace.DeskPlus.Community.BLL\ComApply.cs
	\DeskPlusSolution\Projects\CommunityProject\Zeospace.DeskPlus.Community.BLL\Schedule.cs
	\DeskPlusSolution\Projects\CommunityProject\Zeospace.DeskPlus.Community.BLL\ShortReply.cs
	\DeskPlusSolution\Projects\CommunityProject\Zeospace.DeskPlus.Community.BLL\UserApply.cs

	QRY_CMN_Boa_SelByBoardID
	QRY_CMN_BoaShortReply_Sel
	QRY_CMN_ComApply_Sel
	QRY_CMN_User_SelByGrade
	QRY_CMN_User_SelByMenuID
	QRY_CMN_User_SelByUID
	QRY_CMN_UserApply_SelByUID
	QRY_EIP_UserRole_SelByRoleCode

	-- 자원관리
	\DeskPlusSolution\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs
	\DeskPlusSolution\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\ShortReply.cs

	QRY_RM_ListUser_SelByGubun
	QRY_RM_Reserve_Sel
	QRY_RM_ShareUser_Sel
	QRY_RM_ShortReply_Sel

	-사내SNS
	\DeskPlusSolution\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\ApprovalTalk.cs
	\DeskPlusSolution\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\MyTalk.cs
	\DeskPlusSolution\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\BoardTalk.cs
	\DeskPlusSolution\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\ScheduleTalk.cs

	SQL_DP_ALARM_Common_uc
	QRY_BOA_ShortReply_Sel
	QRY_PIM_Schedule_SelByScheduleID
	QRY_PIM_ToDo_SelectID

: 2017-04-28 by maniaysw ----------------------------------------------------------
[사용자정보] 본인권한 표기 및 로그인 접속이력 버튼 추가
	\DeskPlusEIP\Account\MyInfo\AuthInfo.aspx
	\DeskPlusEIP\Account\MyInfo\AuthInfo.aspx.cs
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs
	\DeskPlusEIP\Account\Ws\MyInfo\LoginInfo.asmx
	\DeskPlusEIP\Account\Ws\MyInfo\LoginInfo.asmx.cs
	\DeskPlusEIP\Account\Js\MyInfo\AuthInfo.js
	\DeskPlusEIP\Account\Js\MyInfo\Mod.js
	\DeskPlusEIP\Account\Jt\MyInfo\JTLoginInfo.htm
	Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs

	\DeskPlusEIP\Account\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-tw.resx

	DP_ACC_User_SelAuth
	DP_ACC_SelLoginInfoCnt
	DP_ACC_SelLoginInfoList

: 2017-04-28 by freeman -----------------------------------------------------------
[주소록] "한국어가 아닐 경우면 외부연동, 초성검색 금지 기능 제외" 기능 누락
     - \DeskPlusEIP\Address\Js\Content\ListBlog.js


: 2017-04-27 by kkzz52 -----------------------------------------------------------
[전자결재] 대결자 기간 적용 (휴가원 연동)
	\DeskPlusEIP\Approval\Base\BasePageApprovalAdmin.cs

	\DeskPlusEIP\Approval\Js\Preference\Proxy\List.js 
	\DeskPlusEIP\Approval\Preference\Proxy\List.aspx
	\DeskPlusEIP\Approval\Ws\Preference\Proxy\List.asmx.cs
	\DeskPlusEIP\Approval\Jt\Preference\Proxy\JTList.htm

	\DeskPlusEIP\Approval\Js\Preference\Proxy\Add.js 추가
	\DeskPlusEIP\Approval\Preference\Proxy\Add.aspx 추가
	\DeskPlusEIP\Approval\Preference\Proxy\Add.aspx.cs 추가
	\DeskPlusEIP\Approval\Ws\Preference\Proxy\Add.asmx.cs 추가

	\DeskPlusEIP\Approval\Js\Preference\Proxy\Mod.js 추가
	\DeskPlusEIP\Approval\Preference\Proxy\Mod.aspx 추가
	\DeskPlusEIP\Approval\Preference\Proxy\Mod.aspx.cs 추가
	\DeskPlusEIP\Approval\Ws\Preference\Proxy\Mod.asmx.cs 추가

	\DeskPlusEIP\Approval\Js\Preference\Proxy\View.js 추가
	\DeskPlusEIP\Approval\Preference\Proxy\View.aspx 추가
	\DeskPlusEIP\Approval\Preference\Proxy\View.aspx.cs 추가

	\DeskPlusEIP\Approval\Js\Home\Menu.js 
	\DeskPlusEIP\Approval\Ws\Home\Menu.asmx.cs
	\Design\Dev\C\Css\Func_Home.css ( #su_aprinfo .in_way span .in_date {font-size: 11px;line-height: 18px;padding-top: 5px;} )

	\DeskPlusEIP\Approval\Js\Admin\Proxy\List.js 
	\DeskPlusEIP\Approval\Admin\Proxy\List.aspx
	\DeskPlusEIP\Approval\Ws\Admin\Proxy\List.asmx.cs
	\DeskPlusEIP\Approval\Jt\Admin\Proxy\JTList.htm

	\DeskPlusEIP\Approval\Js\Admin\Proxy\Add.js 추가
	\DeskPlusEIP\Approval\Admin\Proxy\Add.aspx 추가
	\DeskPlusEIP\Approval\Admin\Proxy\Add.aspx.cs 추가
	\DeskPlusEIP\Approval\Ws\Admin\Proxy\Add.asmx.cs 추가

	\DeskPlusEIP\Approval\Js\Admin\Proxy\View.js 추가
	\DeskPlusEIP\Approval\Admin\Proxy\View.aspx 추가
	\DeskPlusEIP\Approval\Admin\Proxy\View.aspx.cs 추가

	\DeskPlusEIP\Approval\Js\Admin\Proxy\Mod.js 추가
	\DeskPlusEIP\Approval\Admin\Proxy\Mod.aspx 추가
	\DeskPlusEIP\Approval\Admin\Proxy\Mod.aspx.cs 추가
	\DeskPlusEIP\Approval\Ws\Admin\Proxy\Mod.asmx.cs 추가

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\ProxyInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Proxy.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Proxy.cs

	\DeskPlusEIP\Approval\App_Data\Proxy\ko~.xml 수정

	\DeskPlusEIP\Approval\Js\Admin\Menu\Tree.js
	\DeskPlusEIP\Approval\Admin\Menu\Tree.aspx

	\DeskPlusEIP\ETC\Js\Holiday\GetUserInfo.js
	\DeskPlusEIP\ETC\Holiday\GetUserInfo.aspx

	\DeskPlusEIP\Legacy\HoliDay.aspx
	\DeskPlusEIP\Legacy\HoliDay_Complete.aspx	
	
	\DeskPlusEIP\Approval\Resource

	휴가원 양식 수정

	DP_APP_Proxy 테이블 수정
	DP_APP_ProxyBackUp 테이블 수정
	DP_APP_Proxy_SelList 수정
	DP_APP_Proxy_SelListCnt 수정
	DP_APP_Proxy_Ins 수정
	DP_APP_Proxy_Upd 추가
	DP_APP_Proxy_SelByProxyUser 추가
	DP_APP_Proxy_SelByDate 추가
	DP_APP_Proxy_SelByProxyID 추가
	DP_APP_Proxy_Start 추가
	DP_APP_Proxy_SelListCntAdmin 추가
	DP_APP_Proxy_SelListAdmin 추가

	그룹웨어_대결자기간처리 SQL 에이전트 추가

: 2017-04-27 by 방영환 -----------------------------------------------------------
[전자결재] 전자결재 홈 검색버튼 다국어 적용
	소스수정)
	\DeskPlusSolution\DeskPlusEIP\Approval\Home\Content.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Home\Search.aspx

: 2017-04-27 by 이지원 -----------------------------------------------------------
[게시판, 전자결재] 검색어 입력 다국어 수정
	\DeskPlusSolution\DeskPlusEIP\Board\Home\Content.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Home\Content.js
	\DeskPlusSolution\DeskPlusEIP\Board\Home\Search.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Home\Search.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Home\Content.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Home\Content.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Home\Search.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Home\Search.js

	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusSolution\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx

: 2017-04-27 by 이지원 -----------------------------------------------------------
[게시판] 본문검색 전체 기간 제외
	\DeskPlusEIP\Board\Js\Admin\Content\List.js
	\DeskPlusEIP\Board\Js\Content\BlogList.js
	\DeskPlusEIP\Board\Js\Content\BlogListImpo.js
	\DeskPlusEIP\Board\Js\Content\BlogListInterested.js
	\DeskPlusEIP\Board\Js\Content\GalleryList.js
	\DeskPlusEIP\Board\Js\Content\GalleryListImpo.js
	\DeskPlusEIP\Board\Js\Content\GalleryListInterested.js
	\DeskPlusEIP\Board\Js\Content\List.js
	\DeskPlusEIP\Board\Js\Content\ListImpo.js
	\DeskPlusEIP\Board\Js\Content\ListInterested.js
	\DeskPlusEIP\Board\Js\Home\MoreList.js
	\DeskPlusEIP\Board\Js\Temp\GalleryList.js
	\DeskPlusEIP\Board\Js\Temp\List.js
	\DeskPlusEIP\Board\Js\Temp\BlogList.js
	\DeskPlusEIP\Board\Js\Temp\GalleryList.js

: 2017-04-25 by famo23 -----------------------------------------------------------
[모바일관리] 설정값 변경을 전체변경에서 개별변경으로 수정
	DP_MDM_List_UpdAlarm
	DP_MDM_List_UpdLogin
	DP_MDM_List_UpdAccess
	\DeskPlusEIP\MDM\Config\Setting.aspx
	\DeskPlusEIP\MDM\Config\Setting.aspx.cs
	\DeskPlusEIP\MDM\Js\Config\Setting.js
	\DeskPlusEIP\MDM\Ws\Config\Setting.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\List.cs

: 2017-04-20 by 이지원 -----------------------------------------------------------
[포틀릿] 바로가기 추가
	\DeskPlusEIP\Portlet\Package\QuickLink.ascx
	\DeskPlusEIP\Portlet\Package\QuickLink.ascx.cs
	\DeskPlusEIP\Portlet\Js\Package\QuickLink.js
	\DeskPlusEIP\Portlet\Base\BaseControlPortlet.cs
	\DeskPlusEIP\EIP\Package\Main.aspx.cs

	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_Package.css
	/*바로가기*/
	.wr_ptl
	{
	   padding-top:5px;
	   padding-bottom:5px;
	   width: 45px;
	   height:45px;
	   float:left;
	   text-align:center;
	}

	\DeskPlusEIP\Html\Js\livicons\livicons-1.4.min.js
	\DeskPlusEIP\Html\Js\livicons\raphael.min.js
	
: 2017-04-19 by withsuki -----------------------------------------------------------
[알람공통/전자결재] 개인알람설정 및 전자결재 개인알람수정

소스수정)
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserInfo.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironment.cs
	\DeskPlusEIP\Account\Js\Admin\User\Mod.js
	\DeskPlusEIP\Account\Js\MyInfo\Alarm.js
	\DeskPlusEIP\Account\Js\MyInfo\Mod.js
	\DeskPlusEIP\Account\Admin\User\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Alarm.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Account\Resource\GlobalString\
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\AlarmInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Proxy.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs
	\DeskPlusEIP\Approval\App_Data\Confirm\
	\DeskPlusEIP\Approval\App_Data\Proxy\
	\DeskPlusEIP\Approval\Ws\Preference\Proxy\List.asmx.cs

DB작업)
	- DP_ACC_UserEnv 필드추가
		AlarmApproval
		AlarmBoard
		AlarmPims
		AlarmTask
		AlarmWork
		AlarmCommunity
		AlarmRM
		AlarmSNS

	- 뷰/프로시저 수정
		VW_UserInfo 
		DP_ACC_UserEnv_UpdMyInfo
		DP_APP_Seq_SelAlarmApproveCNext
		DP_APP_Seq_SelAlarmApproveD
		DP_APP_Seq_SelAlarmApproveDC
		DP_APP_Seq_SelAlarmApproving
		DP_APP_Seq_SelAlarmCirculation
		DP_APP_Seq_SelAlarmComplete
		DP_APP_Seq_SelAlarmForward
		DP_APP_Seq_SelAlarmNext
		DP_APP_Seq_SelAlarmNextAll
		DP_APP_Seq_SelAlarmOpinion
		DP_APP_Seq_SelAlarmOpinionAddAll
		DP_APP_Seq_SelAlarmOpinionAll
		DP_APP_Seq_SelAlarmReject
		DP_APP_Seq_SelAlarmRejectAll

	- 추가
		DP_ACC_UserEnv_UpdAlarm

: 2017-04-18 by 방영환 -----------------------------------------------------------
[전자결재] 전자결재 리스트에서 보안문서, 긴급문서 아이콘 구분해서 표시
	소스수정)
	\DeskPlusEIP\Approval\Js\Admin\Document\JListCom.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\List.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListReject.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListRejectFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\List.js
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\ListFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReceiptList.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendList.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardList.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\List.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\CommonList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\List.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\ReceiptList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\SendList.js
	\DeskPlusEIP\Approval\Js\Folder\Impo\List.js
	\DeskPlusEIP\Approval\Js\Folder\Private\List.js
	\DeskPlusEIP\Approval\Js\Folder\Private\ListFolder.js
	\DeskPlusEIP\Approval\Js\Status\ListAfterward.js
	\DeskPlusEIP\Approval\Js\Status\ListApprove.js
	\DeskPlusEIP\Approval\Js\Status\ListComplete.js
	\DeskPlusEIP\Approval\Js\Status\ListDelay.js
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\Approval\Js\Status\ListProxy.js
	\DeskPlusEIP\Approval\Js\Status\ListReceive.js
	\DeskPlusEIP\Approval\Js\Status\ListReject.js
	\DeskPlusEIP\Approval\Js\Status\ListRelated.js
	\DeskPlusEIP\Approval\Js\Status\ListTransfer.js
	\DeskPlusEIP\Approval\Js\Home\Content.js
	\DeskPlusEIP\Approval\Js\Home\Search.js

	\DeskPlusEIP\Approval\Jt\Admin\Document\JTList.htm
	\DeskPlusEIP\Approval\Jt\Admin\Document\JTListProcess.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\JTListFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\JTListReject.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\JTListRejectFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Arbitrary\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Arbitrary\JTListFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTCirReceiptList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTCirSendList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTForwardList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTCommonList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTReceiptList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTSendList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Impo\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\JTListFolder.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListAfterward.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListApprove.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListComplete.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListDelay.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListProcess.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListProxy.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListReceive.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListReject.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListRelated.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListTransfer.htm
	\DeskPlusEIP\Approval\Jt\Home\JTListTab.htm
	\DeskPlusEIP\Approval\Jt\Home\JTReply.htm
	\DeskPlusEIP\Approval\Jt\Home\JTListDoc.htm

	\DeskPlusEIP\Ws\Home\Content.asmx.cs
	\DeskPlusEIP\Ws\Home\Search.asmx.cs

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\ShoryReplyInfo.cs

	프로시저수정)
	DP_APP_Home_SelShortReply
	DP_EIP_Search_Scheduler
	DP_APP_HOME_SelPageListDocAll

	이미지추가)
	\DeskPlusEIP\Design\C\Img\Icon\Security.gif
	\DeskPlusEIP\Design\C\Img\Icon\Emergency.gif

: 2017-04-11 by withsuki -----------------------------------------------------------
[포틀릿] 임직원조회 디자인변경
	\DeskPlusEIP\Portlet\Package\UserSearch\AccountInfo.aspx
	\DeskPlusEIP\Portlet\Package\UserSearch\DeptUserSearch.ascx
	\Design\C\Img\Etc
	\Design\C\Img\Package

: 2017-04-11 by withsuki -----------------------------------------------------------
[전자결재] 문서내용 길이체크

	\DeskPlusEIP\\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs
	\DeskPlusEIP\Approval\Resource\GlobalString\


: 2017-04-05 by maniaysw -------------------------------------------------------
[게시판] 팝업 미리보기기능 추가
	\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Content\Preview.aspx
	\DeskPlusEIP\Board\Content\Preview.aspx.cs

	\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusEIP\Board\Js\Content\Mod.js

	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx

: 2017-04-05 by ellepooky -------------------------------------------------------
[전자결재] 전달현황 기능 추가
	소스 추가)
	\DeskPlusEIP\Approval\Common\Folder\ForwardList.aspx
	\DeskPlusEIP\Approval\Js\Common\Folder\ForwardList.js
	\DeskPlusEIP\Approval\Jt\Common\Folder\JTForwardList.htm
	\DeskPlusEIP\Approval\Ws\Common\Folder\ForwardList.asmx
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\ListViewerInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ListViewer.cs

	소스 수정)
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js

	리소스 수정)
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx

	프로시저 추가)
	DP_APP_ListViewer_SelPageForwardList
	DP_APP_ListViewer_SelForwardCnt

: 2017-03-30 by hooper2 -------------------------------------------------------
[팝업관리] 모듈 추가
	소스 추가)
	\DeskPlusEIP\Preview.aspx
	\DeskPlusEIP\ETC\PopUp\Add.aspx
	\DeskPlusEIP\ETC\PopUp\AddN.aspx
	\DeskPlusEIP\ETC\PopUp\List.aspx
	\DeskPlusEIP\ETC\PopUp\Mod.aspx
	\DeskPlusEIP\ETC\PopUp\ModN.aspx

	\DeskPlusEIP\ETC\Js\PopUp\Add.js
	\DeskPlusEIP\ETC\Js\PopUp\List.js
	\DeskPlusEIP\ETC\Js\PopUp\Mod.js

	\DeskPlusEIP\ETC\Jt\PopUp\JTList.htm

	\DeskPlusEIP\ETC\Ws\PopUp\List.asmx

	Zeospace.DeskPlus.ETC.BE\ManagerInfo.cs

	Zeospace.DeskPlus.ETC.DAL\Manager.cs

	소스 수정)
	\DeskPlusEIP\DeskPlusEIP\Web.config		
	\DeskPlusEIP\EIP\DeskPlusEPDefault.aspx
	\DeskPlusEIP\Site\Auth\Default.aspx		Default1~10까지 수정

	테이블 추가)
	DP_POP_Manager

	프로시저 추가)
	DP_POP_Manager_SelPageCnt
	DP_POP_Manager_SelPageList
	DP_POP_Manager_Sel
	DP_POP_Manager_Ins
	DP_POP_Manager_Upd
	DP_POP_Manager_Del


: 2017-03-30 by famo23 -------------------------------------------------------
[모바일관리] 영어버전오류수정
	리소스수정

: 2017-03-29 by 방영환 -------------------------------------------------------
[전자결재] HOME 검색 개선 (기획업무)
	소스수정)
	\DeskPlusEIP\Approval\Home\Content.aspx
	\DeskPlusEIP\Approval\Home\Search.aspx.cs
	\DeskPlusEIP\Approval\Ws\Home\Search.asmx.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Approval.DAL\Home.cs

	프로시저 수정)
	DP_APP_HOME_SelPageListDocAll
	DP_APP_HOME_SelPageCntDocAll
	DP_EIP_Search_Scheduler

	프로시저 추가)
	DP_SEARCH_ApprovalUser_Schedular
	
	테이블 추가)
	DP_SEARCH_ApprovalUser

	테이블 수정)
	DP_EIP_Search_New 컬럼추가

	Etc6 NVARCHAR(500)
	Etc7 NVARCHAR(500)
	Etc8 NVARCHAR(500)
	Etc9 NVARCHAR(500)
	Etc10 NVARCHAR(500)

	리소스 수정)
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx

: 2017-03-29 by 이지원 -------------------------------------------------------
[게시판] 관심대상자 추가 (기획업무)
	--관심대상자 추가
	--소스
	\DeskPlusEIP\Board\Home\Content.aspx
	\DeskPlusEIP\Board\Js\Home\Content.js
	\DeskPlusEIP\Board\Content\List.aspx
	\DeskPlusEIP\Board\Content\List.aspx.cs
	\DeskPlusEIP\Board\Js\Content\List.js
	\DeskPlusEIP\Board\Content\BlogList.aspx
	\DeskPlusEIP\Board\Content\BlogList.aspx.cs
	\DeskPlusEIP\Board\Js\Content\BlogList.js
	\DeskPlusEIP\Board\Content\GalleryList.aspx
	\DeskPlusEIP\Board\Content\GalleryList.aspx.cs
	\DeskPlusEIP\Board\Js\Content\GalleryList.js
	\DeskPlusEIP\Board\Common\GetInterestedUser.aspx
	\DeskPlusEIP\Board\Common\GetInterestedUser.aspx.cs
	\DeskPlusEIP\Board\Js\Common\GetInterestedUser.js
	\DeskPlusEIP\Board\Ws\Common\InterestedUser.asmx.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\InterestedUserInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\InterestedUser.cs

	--DB
	DP_BOA_InterestedUser
	--프로시져
	DP_BOA_InterestedUser_Ins
	DP_BOA_InterestedUser_Del
	DP_BOA_InterestedUser_DelByUserID
	DP_BOA_InterestedUser_SelByID

	--관심대상자리스트
	--소스
	\DeskPlusEIP\Board\Category\Tree.aspx
	\DeskPlusEIP\Board\Category\Tree.aspx.cs
	\DeskPlusEIP\Board\Js\Category\Tree.js
	\DeskPlusEIP\Board\Common\BoardTreeScript.ascx.cs
	\DeskPlusEIP\Board\Content\ListInterested.aspx
	\DeskPlusEIP\Board\Content\ListInterested.aspx.cs
	\DeskPlusEIP\Board\Js\Content\ListInterested.js
	\DeskPlusEIP\Board\Jt\Content\JTListInterested.htm
	\DeskPlusEIP\Board\Ws\Content\ListInterested.asmx.cs
	\DeskPlusEIP\Board\Content\BlogListInterested.aspx
	\DeskPlusEIP\Board\Content\BlogListInterested.aspx.cs
	\DeskPlusEIP\Board\Js\Content\ListInterested.js
	\DeskPlusEIP\Board\Jt\Content\JTBlogListInterested.htm
	\DeskPlusEIP\Board\Content\GalleryListInterested.aspx
	\DeskPlusEIP\Board\Content\GalleryListInterested.aspx.cs
	\DeskPlusEIP\Board\Js\Content\GalleryListInterested.js
	\DeskPlusEIP\Board\Jt\Content\JTGalleryListInterested.htm
	\DeskPlusEIP\Board\Content\ViewInterested.aspx
	\DeskPlusEIP\Board\Content\ViewInterested.aspx.cs
	\DeskPlusEIP\Board\Js\Content\ViewInterested.js
	\DeskPlusEIP\SNS\Js\Point\Menu\Menu.js
	\DeskPlusEIP\Board\Js\Content\FavoriteAdd.js
	\DeskPlusEIP\Board\Js\Content\FavoriteCopy.js
	\DeskPlusEIP\Board\Js\Content\FavoriteOrder.js

	--프로시져
	DP_BOA_InterestedPageCnt
	DP_BOA_InterestedPageList

	--리소스
	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx	

: 2017-03-28 by withsuki -------------------------------------------------------
[팝업TO-DO] 로그인 후 TO-DO에 대한 수동적 정보조회 
	DP_ACC_UserEnv 필드추가
		IsTodo
		TodoMenu

	DP_ACC_UserEnv_UpdAppAlarm 삭제
	DP_ACC_UserEnv_UpdMyInfo 추가
	DP_PT_TodoPopup_RMIsMgr 추가
	DP_PT_TodoPopup_ApproveCnt
	DP_PT_TodoPopup_ApproveList
	DP_PT_TodoPopup_ApproveList_backup20170324
	DP_PT_TodoPopup_PimScheduleList
	DP_PT_TodoPopup_PimScheduleListCnt
	DP_PT_TodoPopup_PimTodoList
	DP_PT_TodoPopup_PimTodoListCnt
	DP_PT_TodoPopup_RMMgrUsable0List
	DP_PT_TodoPopup_RMMgrUsable0ListCnt
	DP_PT_TodoPopup_RMUsableList
	DP_PT_TodoPopup_RMUsableListCnt
	DP_PT_TodoPopup_SubmitCnt
	DP_PT_TodoPopup_SubmitList
	DP_PT_TodoPopup_TASCnt
	DP_PT_TodoPopup_TASList
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserEnvironmentInfo.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironment.cs
	\DeskPlusEIP\Account\Admin\User\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Account\Resource\
	\DeskPlusEIP\Portlet\Js\TodoPopup\TodoList.js
	\DeskPlusEIP\Portlet\Jt\TodoPopup\JTTodoList.htm
	\DeskPlusEIP\Portlet\TodoPopup\TodoList.aspx
	\DeskPlusEIP\Portlet\Ws\TodoPopup\TodoList.asmx
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\TodoList.cs
	\DeskPlusEIP\EIP\Js\Package\Main.js
	\DeskPlusEIP\EIP\Package\Main.aspx.cs

: 2017-03-27 by 방영환 -------------------------------------------------------
[전자결재] 자주지정하는결재자 부서변경시 리스트 회색처리, 결재자가 이미 지정된 문서를 작성하는 경우 변경내용 Alert처리
	소스 수정)
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Preference\Favorite\List.aspx

	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\Preference\Favorite\List.js

	\DeskPlusEIP\Approval\Jt\Preference\Favorite\JTList.htm

	\DeskPlusEIP\Approval\FormFlow\Basic\Add.asmx.cs
	\DeskPlusEIP\Approval\FormFlow\Common\Common.asmx.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.asmx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.asmx.cs
	\DeskPlusEIP\Approval\Preference\Favorite\List.asmx.cs

	\Projects\BoardProject\Zeospace.DeskPlus.Approval.BE\FavoriteApprovalInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Approval.DAL\DraftSequence.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Approval.DAL\FavoriteApprovalNet.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Approval.DAL\SequenceBackUp.cs

	리소스 추가)
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
	
	isModfy - 변경여부
	ModfyAppLine - 결재정보변경
	Msg_ConfirmAppLine - 결재라인을 확인하세요.

	프로시저 수정)
	DP_APP_FavoriteApp_SelList
	DP_APP_FavoriteApp_SelListCnt

	프로시저 추가)
	DP_APP_FavoriteAppNet_SelModfy
	DP_APP_DraftSequence_SelModfy
	DP_APP_SeqBackup_SelModfyByLine
	DP_APP_Seq_SelModfyByLine
	DP_APP_Seq_SelModfyByLineNonApp

: 2017-03-23 by famo23 -------------------------------------------------------
[자원관리] 알람에 자원명도 표기되게 수정 & 알람포맷 디자인변경
	\DeskPlusEIP\RM\App_Data\en.xml
	\DeskPlusEIP\RM\App_Data\fr.xml
	\DeskPlusEIP\RM\App_Data\ja.xml
	\DeskPlusEIP\RM\App_Data\ko.xml
	\DeskPlusEIP\RM\App_Data\zh-cn.xml
	\DeskPlusEIP\RM\App_Data\zh-tw.xml
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable0List.asmx
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable1List.asmx
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable2List.asmx
	\DeskPlusEIP\RM\Ws\Content\Manager\Usable0List.asmx
	\DeskPlusEIP\RM\Ws\Content\Manager\Usable1List.asmx
	\DeskPlusEIP\RM\Ws\Content\Manager\Usable2List.asmx
	\DeskPlusEIP\RM\Ws\Content\ListDay.asmx
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx
	\DeskPlusEIP\RM\Ws\Content\ListWeek.asmx
	\DeskPlusEIP\RM\Ws\Content\WholeDay.asmx
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs

: 2017-03-22 by famo23 -------------------------------------------------------
[사내sns - 모아보기] 각 모듈별 중요표시 & 즐겨찾기를 한군데서 모아보는 모듈 (기획업무)
	리소스추가
	\DeskPlusEIP\SNS\Base\BasePagePoint.cs
	\DeskPlusEIP\SNS\Js\Point\Menu\Menu.js
	\DeskPlusEIP\SNS\Point\Menu\Menu.aspx
	\DeskPlusEIP\SNS\Point\Default.aspx

	\DeskPlusEIP\Approval\Folder\Impo\List.aspx.cs

	\DeskPlusEIP\Board\Base\BasePageBoard.cs
	\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusEIP\Board\Content\BlogList.aspx
	\DeskPlusEIP\Board\Content\BlogList.aspx.cs
	\DeskPlusEIP\Board\Content\BlogListImpo.aspx
	\DeskPlusEIP\Board\Content\GalleryList.aspx
	\DeskPlusEIP\Board\Content\GalleryList.aspx.cs
	\DeskPlusEIP\Board\Content\GalleryListImpo.aspx
	\DeskPlusEIP\Board\Content\List.aspx
	\DeskPlusEIP\Board\Content\List.aspx.cs
	\DeskPlusEIP\Board\Content\ListImpo.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Content\Reply.aspx
	\DeskPlusEIP\Board\Content\Reply.aspx.cs
	\DeskPlusEIP\Board\Content\ReplyN.aspx
	\DeskPlusEIP\Board\Content\View.aspx
	\DeskPlusEIP\Board\Content\View.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusEIP\Board\Js\Content\BlogList.js
	\DeskPlusEIP\Board\Js\Content\GalleryList.js
	\DeskPlusEIP\Board\Js\Content\List.js
	\DeskPlusEIP\Board\Js\Content\Reply.js
	\DeskPlusEIP\Board\Js\Content\View.js

	\DeskPlusEIP\Task\ImpoUser\List.aspx

	\DeskPlusEIP\WebHard\Content\List.aspx
	\DeskPlusEIP\WebHard\Content\List.aspx.cs
	\DeskPlusEIP\WebHard\Content\ListIcon.aspx
	\DeskPlusEIP\WebHard\Content\ListIcon.aspx.cs
	\DeskPlusEIP\WebHard\Content\ListImpo.aspx
	\DeskPlusEIP\WebHard\Content\ListImpoIcon.aspx
	\DeskPlusEIP\WebHard\Content\ListImpoPreview.aspx
	\DeskPlusEIP\WebHard\Content\ListPreview.aspx
	\DeskPlusEIP\WebHard\Content\ListPreview.aspx.cs
	\DeskPlusEIP\WebHard\Js\Content\List.js
	\DeskPlusEIP\WebHard\Js\Content\ListIcon.js
	\DeskPlusEIP\WebHard\Js\Content\ListPreview.js

	\DeskPlusEIP\Address\Content\ListBlogImpo.aspx
	\DeskPlusEIP\Address\Content\ListBlogImpo.aspx.cs
	\DeskPlusEIP\Address\Content\ListImpo.aspx
	\DeskPlusEIP\Address\Content\ListImpo.aspx.cs

	\DeskPlusEIP\Opinion\Content\FavList.aspx
	\DeskPlusEIP\Opinion\Content\FavList.aspx.cs


: 2017-03-22 by freeman -------------------------------------------------------
[포탈] 포틀릿 추가 시 레이아웃 선택 하는 기능 추가 
	\DeskPlusEIP\EIP\Js\Package\Main.js
	\DeskPlusEIP\EIP\Js\Portal\GetPortlet.js

	\DeskPlusEIP\EIP\Jt\Portal\JTGetPortlet.htm

	\DeskPlusEIP\EIP\Portal\GetPortlet.aspx.*.*

	\Design\C\Img\Layout\layout1.png ~ layout7.png  (임시로 만들어서 디자인 필요)

: 2017-03-22 by 이지원 -------------------------------------------------------
[게시판] URL 링크 기능 추가 (기획업무)
	--관리자페이지
	--소스
	\DeskPlusEIP\Board\Admin\Category\Mod.aspx
	\DeskPlusEIP\Board\Admin\Category\Mod.aspx.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\CategoryInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\Category.cs
	\DeskPlusEIP\Board\Admin\Category\ModTot.aspx
	\DeskPlusEIP\Board\Admin\Category\ModTot.aspx.cs
	\DeskPlusEIP\Board\Admin\Content\Add.aspx
	\DeskPlusEIP\Board\Admin\Content\AddN.aspx
	\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Js\Admin\Content\Add.js
	\DeskPlusEIP\Board\Js\Admin\Content\View.js
	\DeskPlusEIP\Board\Admin\Content\Mod.aspx
	\DeskPlusEIP\Board\Admin\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Admin\Content\ModN.aspx
	\DeskPlusEIP\Board\Js\Admin\Content\Mod.js

	-테이블
	DP_BOA_Category	OptionLinkURL	char(1)

	-프로시져
	DP_BOA_Category_Ins
	DP_BOA_Category_Upd

	--게시판페이지
	--소스
	\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusEIP\Board\Content\LinkURL.aspx
	\DeskPlusEIP\Board\Content\LinkURL.aspx.cs
	\DeskPlusEIP\Board\Js\Content\LinkURL.js
	\DeskPlusEIP\Board\Jt\Content\JTAttachFile.htm
	\DeskPlusEIP\Board\Jt\Content\JTSpoolFileL.htm
	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Mod.js
	\DeskPlusEIP\Board\Ws\Content\Mod.asmx.cs
	\DeskPlusEIP\Board\Jt\Content\JTDelFile.htm
	\DeskPlusEIP\Board\Jt\Content\JTDelScreenFile.htm
	\DeskPlusEIP\Board\Js\Content\View.js
	\DeskPlusEIP\Board\Js\Content\ViewImpo.js
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\ListInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Lists.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\LinkURLInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\LinkURL.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\LinkURL.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\MyTemplate.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\MyLinkURLInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\MyLinkURL.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\MyLinkURL.cs

	--테이블
	DP_BOA_LinkURL

	--프로시져
	DP_BOA_LinkURL_Ins
	DP_BOA_LinkURL_DelByID
	DP_BOA_List_Del
	DP_BOA_List_DelByUserID
	DP_BOA_List_Move
	DP_BOA_List_SelPageList
	DP_BOA_List_SelPageCnt
	DP_BOA_ImpoPageList
	DP_BOA_ImpoPageCnt
	DP_BOA_HOME_SelSearchList
	DP_BOA_HOME_SelSearchCount
	DP_BOA_HOME_SelRecentWriteList
	DP_BOA_HOME_SelRecentWriteCount
	DP_BOA_HOME_SelMaxShortList
	DP_BOA_HOME_SelMaxShortCount
	DP_BOA_HOME_SelMaxReadList
	DP_BOA_HOME_SelMaxReadCount
	DP_BOA_HOME_SelMaxRateList
	DP_BOA_HOME_SelMaxRateCount
	DP_BOA_HOME_SelMyWriteList
	DP_BOA_HOME_SelMyWriteCount
	DP_BOA_HOME_SelMyListByShortList
	DP_BOA_HOME_SelMyListByShortCount

	--쿼리
	Board QRY_BOA_LinkURL_Sel

	--리소스
	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx

: 2017-03-21 by famo23 -------------------------------------------------------
[게시판] 임시저장 작성하기 / 수정 타이틀 적용
	\DeskPlusEIP\Board\Temp\Add.aspx
	\DeskPlusEIP\Board\Temp\AddN.aspx
	\DeskPlusEIP\Board\Temp\Mod.aspx
	\DeskPlusEIP\Board\Temp\ModN.aspx

: 2017-03-20 by maniaysw -------------------------------------------------------
[임직원조회] 포틀릿 추가 및 검색조건 변경 및 UI 변경(기획업무)
	\DeskPlusEIP\Account\Search\List.aspx
	\DeskPlusEIP\Account\Search\List.aspx.cs
	\DeskPlusEIP\Account\Ws\Search\List.asmx.cs
	\DeskPlusEIP\Account\Js\Search\List.js
	\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs

	\DeskPlusEIP\Portlet\Package\UserSearch\AccountInfo.aspx
	\DeskPlusEIP\Portlet\Package\UserSearch\AccountInfo.aspx.cs
	\DeskPlusEIP\Portlet\Package\UserSearch\DeptUserSearch.ascx
	\DeskPlusEIP\Portlet\Package\UserSearch\DeptUserSearch.ascx.cs
	\DeskPlusEIP\Portlet\Package\UserSearch\Search.aspx
	\DeskPlusEIP\Portlet\Package\UserSearch\Search.aspx.cs
	\DeskPlusEIP\Portlet\Js\Package\UserSearch\AccountInfo.js
	\DeskPlusEIP\Portlet\Js\Package\UserSearch\Search.js

	\DeskPlusEIP\Account\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-tw.resx

	\DeskPlusEIP\Portlet\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.zh-tw.resx
	
	\Design\Dev\C\Css\DeskPlusEIPBase.css 
	\Design\Dev\C\Css\Func_portal.css 
	\Design\Dev\C\Css\Func_Package.css 

	DP_ACC_User_SelSrhUserPageList
	DP_ACC_User_SelSrhUserPageCnt
	DP_ACC_User_SelSrhUser

: 2017-03-20 by famo23 -------------------------------------------------------
[게시판] 블로그형, 갤러리행 작성하기 버튼 스타일 수정
	\DeskPlusEIP\Board\Content\BlogList.aspx
	\DeskPlusEIP\Board\Content\GalleryList.aspx

[포탈] 왼쪽상단에 있던 로그아웃버튼 안 보이게 수정
	\DeskPlusEIP\EIP\Package\Menu.aspx

: 2017-03-20 by freeman -------------------------------------------------------
[일정관리 - 버그] Chrome 에서 전체공유일정보기 후 달력 클릭 이벤트 동작 않되는 문제 : Chrome에서 특정 frame 을 몾찾음
	\DeskPlusSolution\DeskPlusEIP\Pim\Js\Menu\PimMenu.js

: 2017-03-17 by freeman -------------------------------------------------------
[일정관리 - 버그] 일간에서 할일 리스트가 많을 경우 일정추가 팝업이 스크롤 아래로 내려가서 가운데 않보이는 문제 (해당 프레임 top 150에 고정함)
     \DeskPlusSolution\DeskPlusEIP\Pim\Js\Schedule\ListDay.js 

: 2017-03-16 by maniaysw -------------------------------------------------------
[주소록] 중요정보에서 상세보기 취소시 이상한 화면으로 이동문제 수정
	\DeskPlusEIP\Address\Js\Content\Add.js
	\DeskPlusEIP\Address\Js\Content\ListBlogImpo.js
	\DeskPlusEIP\Address\Js\Content\ListImpo.js
	\DeskPlusEIP\Address\Content\ViewCommon.aspx.cs
	\DeskPlusEIP\Address\Content\Add.aspx.cs

: 2017-03-16 by famo23 -------------------------------------------------------
[주소록] 중요표시 타이틀 다국어 처리안된 부분 수정
	\DeskPlusEIP\Address\Content\ListImpo.aspx
	\DeskPlusEIP\Address\Content\ListImpo.aspx.cs

[웹폴더] 즐겨찾기 폴더 형식 가져오는 부분 수정
	\DeskPlusEIP\WebHard\Js\Folder\Tree.js

: 2017-03-15 by 이지원 -------------------------------------------------------
[게시판] 검색 기능 개선 (기획업무)
	--홈리스트
	--소스
	\DeskPlusEIP\Board\Home\MoreList.aspx
	\DeskPlusEIP\Board\Home\MoreList.aspx.cs
	\DeskPlusEIP\Board\Js\Home\MoreList.js
	\DeskPlusEIP\Board\Ws\Home\MoreList.asmx.cs

	--프로시져
	DP_BOA_HOME_SelRecentWriteCount
	DP_BOA_HOME_SelRecentWriteList
	DP_BOA_HOME_SelMaxShortCount
	DP_BOA_HOME_SelMaxShortList
	DP_BOA_HOME_SelMaxReadCount
	DP_BOA_HOME_SelMaxReadList
	DP_BOA_HOME_SelMaxRateCount
	DP_BOA_HOME_SelMaxRateList
	DP_BOA_HOME_SelMyWriteCount
	DP_BOA_HOME_SelMyWriteList
	DP_BOA_HOME_SelMyListByShortCount
	DP_BOA_HOME_SelMyListByShortList
	DP_BOA_HOME_SelMyShortCount
	DP_BOA_HOME_SelMyShortList

	--리스트
	--소스
	\DeskPlusEIP\Board\Content\List.aspx
	\DeskPlusEIP\Board\Content\List.aspx.cs
	\DeskPlusEIP\Board\Js\Content\List.js
	\DeskPlusEIP\Board\Ws\Content\List.asmx.cs
	\DeskPlusEIP\Board\Content\BlogList.aspx
	\DeskPlusEIP\Board\Content\BlogList.aspx.cs
	\DeskPlusEIP\Board\Js\Content\BlogList.js
	\DeskPlusEIP\Board\Ws\Content\BlogList.asmx.cs
	\DeskPlusEIP\Board\Content\GalleryList.aspx
	\DeskPlusEIP\Board\Content\GalleryList.aspx.cs
	\DeskPlusEIP\Board\Js\Content\GalleryList.js
	\DeskPlusEIP\Board\Ws\Content\GalleryList.asmx.cs

	--프로시져
	DP_BOA_List_SelPageList
	DP_BOA_List_SelPageCnt

	--중요표시리스트
	--소스
	\DeskPlusEIP\Board\Content\ListImpo.aspx
	\DeskPlusEIP\Board\Content\ListImpo.aspx.cs
	\DeskPlusEIP\Board\Js\Content\ListImpo.js
	\DeskPlusEIP\Board\Ws\Content\ListImpo.asmx.cs
	\DeskPlusEIP\Board\Content\BlogListImpo.aspx
	\DeskPlusEIP\Board\Content\BlogListImpo.aspx.cs
	\DeskPlusEIP\Board\Js\Content\BlogListImpo.js
	\DeskPlusEIP\Board\Ws\Content\BlogListImpo.asmx.cs
	\DeskPlusEIP\Board\Content\GalleryListImpo.aspx
	\DeskPlusEIP\Board\Content\GalleryListImpo.aspx.cs
	\DeskPlusEIP\Board\Js\Content\GalleryListImpo.js
	\DeskPlusEIP\Board\Ws\Content\GalleryListImpo.asmx.cs

	--프로시져
	DP_BOA_ImpoPageList
	DP_BOA_ImpoPageCnt

	--임시리스트
	--소스
	\DeskPlusEIP\Board\Temp\List.aspx
	\DeskPlusEIP\Board\Temp\List.aspx.cs
	\DeskPlusEIP\Board\Js\Temp\List.js
	\DeskPlusEIP\Board\Ws\Temp\List.asmx.cs
	\DeskPlusEIP\Board\Temp\BlogList.aspx
	\DeskPlusEIP\Board\Temp\BlogList.aspx.cs
	\DeskPlusEIP\Board\Js\Temp\BlogList.js
	\DeskPlusEIP\Board\Ws\Temp\BlogList.asmx.cs
	\DeskPlusEIP\Board\Temp\GalleryList.aspx
	\DeskPlusEIP\Board\Temp\GalleryList.aspx.cs
	\DeskPlusEIP\Board\Js\Temp\GalleryList.js
	\DeskPlusEIP\Board\Ws\Temp\GalleryList.asmx.cs

	-프로시져
	DP_BOA_MyTemplate_SelPageList
	DP_BOA_MyTemplate_SelPageCnt

	--관리자
	--소스
	\DeskPlusEIP\Board\Admin\Content\List.aspx
	\DeskPlusEIP\Board\Admin\Content\List.aspx.cs
	\DeskPlusEIP\Board\Js\Admin\Content\List.js
	\DeskPlusEIP\Board\Ws\Admin\Content\List.asmx.cs

	--컴포넌트
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\WebComponent\DateHelper.cs

	--리소스
	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx

: 2017-03-15 by famo23 -------------------------------------------------------
[자원관리] 자원담당자에게 승인요청 알람 발송 기능 (기획업무)
	리소스추가
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_RM.css
	DP_RM_Reserve_SelWholeUsable
	\DeskPlusEIP\RM\Content\ListDay.aspx
	\DeskPlusEIP\RM\Content\ListWeek.aspx
	\DeskPlusEIP\RM\Content\WholeDay.aspx
	\DeskPlusEIP\RM\Content\WholeDay.aspx.cs
	\DeskPlusEIP\RM\Content\WholeWeek.aspx
	\DeskPlusEIP\RM\Content\WholeWeek.aspx.cs
	\DeskPlusEIP\RM\Js\Content\ListDay.js
	\DeskPlusEIP\RM\Js\Content\ListWeek.js
	\DeskPlusEIP\RM\Js\Content\WholeDay.js
	\DeskPlusEIP\RM\Js\Content\WholeWeek.js
	\DeskPlusEIP\RM\Jt\Content\JTListDay.htm
	\DeskPlusEIP\RM\Jt\Content\JTListWeek.htm
	\DeskPlusEIP\RM\Ws\Content\ListDay.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\ListWeek.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeDay.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Reserve.cs

: 2017-03-14 by famo23 -------------------------------------------------------
[포틀릿] 일정리스트 포틀릿 디자인 수정 (개선업무)
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_Package.css
	\DeskPlusEIP\Portlet\Js\Package\PimList.js
	\DeskPlusEIP\Portlet\Package\PimList.ascx

: 2017-03-14 by kkzz52 -----------------------------------------------------
[전자결재] 첨부파일 저장 시, 다중으로 선택하여 ZIP파일 저장 (기획업무)
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTAttachFile.htm

	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Folder\Common\ViewBaseDoc.aspx
	\DeskPlusEIP\Approval\Folder\Common\ViewBaseDoc.aspx.cs
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTAttachFilePopUp.htm

	\DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusEIP\Approval\Status\Common\View.aspx
	\DeskPlusEIP\Approval\Status\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Jt\Status\Common\JTAttachFile.htm

	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx.cs

	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx.cs

	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx.cs

	\DeskPlusEIP\Approval\Js\Disuse\View.js
	\DeskPlusEIP\Approval\Disuse\View.aspx
	\DeskPlusEIP\Approval\Disuse\View.aspx.cs
	\DeskPlusEIP\Approval\Jt\Disuse\JTAttachFile.htm

	\DeskPlusEIP\Approval\Js\Common\Popup\ViewCir.js
	\DeskPlusEIP\Approval\Common\Popup\ViewCir.aspx
	\DeskPlusEIP\Approval\Common\Popup\ViewCir.aspx.cs
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTAttachFile.htm

	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirDocUser.js
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirDocUser.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirDocUser.aspx.cs

	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx.cs

	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Jt\Process\Basic\JTAttachFile.htm

	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs

	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx.cs

	\DeskPlusEIP\Approval\Js\Process\Confirm\Approve.js
	\DeskPlusEIP\Approval\Process\Confirm\Approve.aspx
	\DeskPlusEIP\Approval\Process\Confirm\Approve.aspx.cs

	\DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Confirm\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Process\Confirm\ApprovePopup.aspx.cs

	\DeskPlusEIP\Common\Zip\Common\CreateZipFileSelect.aspx 추가
	\DeskPlusEIP\Common\Zip\Common\CreateZipFileSelect.aspx.cs 추가

	\DeskPlusEIP\Approval\Resource


: 2017-03-13 ~ by 방영환 -----------------------------------------------------
[전자결재] 긴급 or 보안문서일 경우 아이콘 표시 (기획업무)	
	소스수정)
	\DeskPlusEIP\EIP\Approval\Js\Status\ListAfterward.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListApprove.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListComplete.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListDelay.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListProxy.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListReceive.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListReject.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListRelated.js
	\DeskPlusEIP\EIP\Approval\Js\Status\ListTransfer.js

	\DeskPlusEIP\EIP\Approval\Js\Folder\Approve\List.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Approve\ListFolder.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Approve\ListReject.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Approve\ListRejectFolder.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Arbitrary\List.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Arbitrary\ListFolder.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Carboncopy\CirReceiptList.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Carboncopy\CirSendList.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Carboncopy\ForwardList.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Carboncopy\List.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Dept\CommonList.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Dept\List.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Dept\ReceiptList.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Dept\SendList.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Impo\List.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Private\List.js
	\DeskPlusEIP\EIP\Approval\Js\Folder\Private\ListFolder.js

	\DeskPlusEIP\EIP\Approval\Js\Admin\Document\JListCom.js

	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListAfterward.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListApprove.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListComplete.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListDelay.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListProcess.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListProxy.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListReceive.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListReject.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListRelated.htm
	\DeskPlusEIP\EIP\Approval\Jt\Status\JTListTransfer.htm

	\DeskPlusEIP\EIP\Approval\Jt\Folder\Approve\JTList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Approve\JTListFolder.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Approve\JTListReject.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Approve\JTListRejectFolder.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Arbitrary\JTList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Arbitrary\JTListFolder.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Carboncopy\JTCirReceiptList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Carboncopy\JTCirSendList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Carboncopy\JTForwardList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Carboncopy\JTList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Dept\JTCommonList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Dept\JTList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Dept\JTReceiptList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Dept\JTSendList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Impo\JTList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Private\JTList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Folder\Private\JTListFolder.htm

	\DeskPlusEIP\EIP\Approval\Jt\Admin\Document\JTList.htm
	\DeskPlusEIP\EIP\Approval\Jt\Admin\Document\JTListProcess.htm

	\DeskPlusEIP\EIP\Approval\Status\ListAfterward.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListApprove.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListComplete.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListDelay.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListProcess.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListProxy.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListReceive.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListReject.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListRelated.aspx
	\DeskPlusEIP\EIP\Approval\Status\ListTransfer.aspx

	\DeskPlusEIP\EIP\Approval\Folder\Approve\List.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Approve\ListFolder.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Approve\ListReject.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Approve\ListRejectFolder.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Arbitrary\List.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Arbitrary\ListFolder.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Carboncopy\CirReceiptList.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Carboncopy\CirSendList.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Carboncopy\ForwardList.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Carboncopy\List.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Dept\CommonList.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Dept\List.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Dept\ReceiptList.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Dept\SendList.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Impo\List.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Private\List.aspx
	\DeskPlusEIP\EIP\Approval\Folder\Private\ListFolder.aspx

	\DeskPlusEIP\EIP\Approval\Admin\Document\JList.aspx

	DeskPlusEIP\EIP\Approval\Ws\Status\ListAfterward.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListApprove.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListComplete.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListDelay.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListProcess.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListProxy.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListReceive.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListReject.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListRelated.asmx
	DeskPlusEIP\EIP\Approval\Ws\Status\ListTransfer.asmx

	\DeskPlusEIP\EIP\Approval\Ws\Folder\Approve\List.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Approve\ListFolder.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Approve\
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Approve\
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Arbitrary\List.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Arbitrary\ListFolder.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Carboncopy\CirReceiptList.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Carboncopy\CirSendList.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Carboncopy\ForwardList.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Carboncopy\List.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Dept\CommonList.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Dept\List.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Dept\ReceiptList.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Dept\SendList.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Impo\List.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Private\List.asmx
	\DeskPlusEIP\EIP\Approval\Ws\Folder\Private\ListFolder.asmx

	\DeskPlusEIP\EIP\Approval\Ws\Admin\Document\List.asmx


	프로시저수정)
	DP_APP_DocProcess_SelPageListProcess
	DP_APP_DocProcess_SelPageListComplete
	DP_APP_DocProcess_SelPageListReject
	DP_APP_DocProcess_SelPageListRelated
	DP_APP_DocProcess_SelPageListApprove
	DP_APP_DocProcess_SelPageListProxy
	DP_APP_DocProcess_SelPageListDelay
	DP_APP_DocProcess_SelPageListReceive
	DP_APP_DocProcess_SelPageListAfterward

	이미지추가)
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\Icon\IsGuard.gif

: 2017-03-13 by maniaysw -----------------------------------------------------
[포탈] > 를 선택한 슬라이드 방식의 설정 (기획업무)	
	\DeskPlusEIP\EIP\Package\Menu.aspx
	\DeskPlusEIP\EIP\Package\Menu.aspx.cs
	\DeskPlusEIP\EIP\Js\Package\Menu.js

	\Design\C\Css\Func_PackageMenu.css

: 2017-03-13 by famo23 -------------------------------------------------------
[자원관리] 범례구분 (기획업무)
	리소스추가
	DP_RM_Usable 테이블 생성
	DP_RM_Reserve_SelDayList
    DP_RM_Reserve_SelWeekList
	DP_RM_Reserve_SelMonthList
	DP_RM_Reserve_SelWholeList
	DP_RM_Usable_InsUpd
	QRY_RM_Usable_Sel
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_RM.css
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\RM\b1_w.gif
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\RM\b2_w.gif
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\RM\b3_w.gif
	\DeskPlusEIP\RM\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Content\ListDay.aspx
	\DeskPlusEIP\RM\Content\ListDay.aspx.cs
	\DeskPlusEIP\RM\Content\ListMain.aspx
	\DeskPlusEIP\RM\Content\ListMonth.aspx
	\DeskPlusEIP\RM\Content\ListMonth.aspx.cs
	\DeskPlusEIP\RM\Content\ListWeek.aspx
	\DeskPlusEIP\RM\Content\ListWeek.aspx.cs
	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\WholeDay.aspx
	\DeskPlusEIP\RM\Content\WholeDay.aspx.cs
	\DeskPlusEIP\RM\Content\WholeMonth.aspx
	\DeskPlusEIP\RM\Content\WholeMonth.aspx.cs
	\DeskPlusEIP\RM\Content\WholeWeek.aspx
	\DeskPlusEIP\RM\Content\WholeWeek.aspx.cs
	\DeskPlusEIP\RM\Js\Content\ListDay.js
	\DeskPlusEIP\RM\Js\Content\ListMain.js
	\DeskPlusEIP\RM\Js\Content\ListMonth.js
	\DeskPlusEIP\RM\Js\Content\ListWeek.js
	\DeskPlusEIP\RM\Js\Content\WholeDay.js
	\DeskPlusEIP\RM\Js\Content\WholeMonth.js
	\DeskPlusEIP\RM\Js\Content\WholeWeek.js
	\DeskPlusEIP\RM\Ws\Content\ListDay.asmx
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\ListWeek.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeDay.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\UsableInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Reserve.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Usable.cs

[자원관리] 타이틀 설명 추가
	리소스추가
	\DeskPlusEIP\RM\Content\ListMain.aspx

: 2017-03-10 by withsuki -----------------------------------------------------
[설문조사] 리스트 버그 사항 (페이징 - DP요청사항)
	\DeskPlusEIP\Opinion\Admin\Content\UnitAdd.aspx.cs
	\DeskPlusEIP\Opinion\Base\BasePageOpinionAdmin.cs

: 2017-03-10 by maniaysw -----------------------------------------------------
[게시판] 스크롤을 이용한 위치 이동에 대한 개선 (기획업무)
	\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusEIP\Board\Js\Content\Mod.js

: 2017-03-10 by famo23 -------------------------------------------------------
[자원관리] 리스트에서 처리내역(미승인/승인/거부) 글씨설명추가
	리소스추가
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_RM.css
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_Home.css
	\DeskPlusEIP\RM\Content\MyList.aspx
	\DeskPlusEIP\RM\Home\Main.aspx
	\DeskPlusEIP\RM\Js\Content\MyList.js
	\DeskPlusEIP\RM\Js\Home\Main.js
	\DeskPlusEIP\RM\Jt\Content\JTMyList.htm
	\DeskPlusEIP\RM\Jt\Home\JTDeptList.htm
	\DeskPlusEIP\RM\Jt\Home\JTMainList.htm
	\DeskPlusEIP\RM\Jt\Home\JTMyList.htm
	\DeskPlusEIP\RM\Ws\Content\MyList.asmx.cs
	\DeskPlusEIP\RM\Ws\Home\Main.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ReserveInfo.cs

: 2017-03-09 by kkzz52 -----------------------------------------------------------
[공통] Appsetting 의 cache 값이 없을 때 xml에서 직접 가져오기
    \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\AppSettingHelper.cs

: 2017-03-09 by maniaysw -------------------------------------------------------
[주소록] 개인주소록 등록 취소시 페이지 경로 오류, 공용주소록 세부보기 후 취소버튼 오동작 수정
	\DeskPlusEIP\Address\Content\Add.aspx
	\DeskPlusEIP\Address\Content\Add.aspx.cs
	\DeskPlusEIP\Address\Js\Content\Add.js
	\DeskPlusEIP\Address\Content\ViewCommon.aspx
	\DeskPlusEIP\Address\Content\ViewCommon.aspx.cs
	\DeskPlusEIP\Address\Js\Content\ViewCommon.js	

: 2017-03-09 by 이지원 ---------------------------------------------------------
[공통] 검색창 예시문구 추가 (기획업무)
	--메뉴, 메인
	\DeskPlusEIP\EIP\Package\Menu.aspx
	\DeskPlusEIP\EIP\Js\Menu\Menu.js
	\DeskPlusEIP\Approval\Home\Content.aspx
	\DeskPlusEIP\Approval\Js\Home\Content.js
	\DeskPlusEIP\Board\Home\Content.aspx
	\DeskPlusEIP\Board\Js\Home\Content.js
	\DeskPlusEIP\Task\Home\AdminMain.aspx
	\DeskPlusEIP\Task\Js\Home\AdminMain.js
	\DeskPlusEIP\Task\Home\Main.aspx
	\DeskPlusEIP\Task\Js\Home\Main.js

	--Account
	\DeskPlusEIP\Account\Admin\AuthEmail\List.aspx
	\DeskPlusEIP\Account\Js\Admin\AuthEmail\List.js
	\DeskPlusEIP\Account\Admin\Dept\List.aspx
	\DeskPlusEIP\Account\Js\Admin\Dept\List.js
	\DeskPlusEIP\Account\Admin\Position\List.aspx
	\DeskPlusEIP\Account\Js\Admin\Position\List.js
	\DeskPlusEIP\Account\Admin\User\List.aspx
	\DeskPlusEIP\Account\Js\Admin\User\List.js
	\DeskPlusEIP\Account\Admin\User\ListBlog.aspx
	\DeskPlusEIP\Account\Js\Admin\User\ListBlog.js
	\DeskPlusEIP\Account\Common\UserListByDept.aspx
	\DeskPlusEIP\Account\Js\Common\UserListByDept.js
	\DeskPlusEIP\Account\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-tw.resx

	--Address
	\DeskPlusEIP\Address\Admin\Content\ListBlog.aspx
	\DeskPlusEIP\Address\Js\Admin\Content\ListBlog.js
	\DeskPlusEIP\Address\Content\AddPrivate.aspx
	\DeskPlusEIP\Address\Js\Content\AddPrivate.js
	\DeskPlusEIP\Address\Content\List.aspx
	\DeskPlusEIP\Address\Js\Content\List.js
	\DeskPlusEIP\Address\Content\ListBlog.aspx
	\DeskPlusEIP\Address\Js\Content\ListBlog.js
	\DeskPlusEIP\Address\Content\ListBlogImpo.aspx
	\DeskPlusEIP\Address\Js\Content\ListBlogImpo.js
	\DeskPlusEIP\Address\Content\ListImpo.aspx
	\DeskPlusEIP\Address\Js\Content\ListImpo.js
	\DeskPlusEIP\Address\Mail\AddressSearch.aspx
	\DeskPlusEIP\Address\Js\Mail\AddressSearch.js
	\DeskPlusEIP\Address\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Address\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Address\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Address\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Address\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Address\Resource\GlobalString\string.zh-tw.resx

	--Approval
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Js\Admin\Document\JListCom.js
	\DeskPlusEIP\Approval\Admin\Document\List.aspx
	\DeskPlusEIP\Approval\Js\Admin\Document\List.js
	\DeskPlusEIP\Approval\Admin\Document\ListCom.aspx
	\DeskPlusEIP\Approval\Js\Admin\Document\ListCom.js
	\DeskPlusEIP\Approval\Admin\EnforceSeal\List.aspx
	\DeskPlusEIP\Approval\Js\Admin\EnforceSeal\List.js
	\DeskPlusEIP\Approval\Admin\FormGeneration\List.aspx
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\List.js
	\DeskPlusEIP\Approval\Admin\Statistics\ListMonth.aspx
	\DeskPlusEIP\Approval\Js\Admin\Statistics\ListMonth.js
	\DeskPlusEIP\Approval\Admin\Transfer\List.aspx
	\DeskPlusEIP\Approval\Js\Admin\Transfer\List.js
	\DeskPlusEIP\Approval\Common\Popup\ShortReplyList.aspx
	\DeskPlusEIP\Approval\Js\Common\Popup\ShortReplyList.js
	\DeskPlusEIP\Approval\Disuse\List.aspx
	\DeskPlusEIP\Approval\Js\Disuse\List.js
	\DeskPlusEIP\Approval\Folder\Approve\List.aspx
	\DeskPlusEIP\Approval\Js\Folder\Approve\List.js
	\DeskPlusEIP\Approval\Folder\Approve\ListFolder.aspx
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListFolder.js
	\DeskPlusEIP\Approval\Folder\Approve\ListReject.aspx
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListReject.js
	\DeskPlusEIP\Approval\Folder\Approve\ListRejectFolder.aspx
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListRejectFolder.js
	\DeskPlusEIP\Approval\Folder\Arbitrary\List.aspx
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\List.js
	\DeskPlusEIP\Approval\Folder\Arbitrary\ListFolder.aspx
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\ListFolder.js
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirReceiptList.aspx
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReceiptList.js
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirSendList.aspx
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendList.js
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardList.aspx
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardList.js
	\DeskPlusEIP\Approval\Folder\Carboncopy\List.aspx
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\List.js
	\DeskPlusEIP\Approval\Folder\Dept\CommonList.aspx
	\DeskPlusEIP\Approval\Js\Folder\Dept\CommonList.js
	\DeskPlusEIP\Approval\Folder\Dept\CompList.aspx
	\DeskPlusEIP\Approval\Js\Folder\Dept\CompList.js
	\DeskPlusEIP\Approval\Folder\Dept\List.aspx
	\DeskPlusEIP\Approval\Js\Folder\Dept\List.js
	\DeskPlusEIP\Approval\Folder\Dept\ReceiptList.aspx
	\DeskPlusEIP\Approval\Js\Folder\Dept\ReceiptList.js
	\DeskPlusEIP\Approval\Folder\Dept\SendList.aspx
	\DeskPlusEIP\Approval\Js\Folder\Dept\SendList.js
	\DeskPlusEIP\Approval\Folder\Enforce\List.aspx
	\DeskPlusEIP\Approval\Js\Folder\Enforce\List.js
	\DeskPlusEIP\Approval\Folder\Enforce\ListSend.aspx
	\DeskPlusEIP\Approval\Js\Folder\Enforce\ListSend.js
	\DeskPlusEIP\Approval\Folder\Enforce\ListSubmit.aspx
	\DeskPlusEIP\Approval\Js\Folder\Enforce\ListSubmit.js
	\DeskPlusEIP\Approval\Folder\EnforceAdmin\ListSend.aspx
	\DeskPlusEIP\Approval\Js\Folder\EnforceAdmin\ListSend.js
	\DeskPlusEIP\Approval\Folder\EnforceAdmin\ListSubmit.aspx
	\DeskPlusEIP\Approval\Js\Folder\EnforceAdmin\ListSubmit.js
	\DeskPlusEIP\Approval\Folder\Impo\List.aspx
	\DeskPlusEIP\Approval\Js\Folder\Impo\List.js
	\DeskPlusEIP\Approval\Folder\OtherDept\List.aspx
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\List.js
	\DeskPlusEIP\Approval\Folder\OtherDept\ReceiptList.aspx
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\ReceiptList.js
	\DeskPlusEIP\Approval\Folder\OtherDept\SendList.aspx
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\SendList.js
	\DeskPlusEIP\Approval\Folder\Private\List.aspx
	\DeskPlusEIP\Approval\Js\Folder\Private\List.js
	\DeskPlusEIP\Approval\Folder\Private\ListFolder.aspx
	\DeskPlusEIP\Approval\Js\Folder\Private\ListFolder.js
	\DeskPlusEIP\Approval\FormFlow\Basic\GetBaseDoc.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\GetBaseDoc.js
	\DeskPlusEIP\Approval\FormFlow\Submit\List.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\List.js
	\DeskPlusEIP\Approval\FormFlow\Temporary\List.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\List.js
	\DeskPlusEIP\Approval\Preference\FavoriteForm\List.aspx
	\DeskPlusEIP\Approval\Js\Preference\Favorite\List.js
	\DeskPlusEIP\Approval\Status\Batch\List.aspx
	\DeskPlusEIP\Approval\Status\ListAfterward.aspx
	\DeskPlusEIP\Approval\Js\Status\ListAfterward.js
	\DeskPlusEIP\Approval\Status\ListApprove.aspx
	\DeskPlusEIP\Approval\Js\Status\ListApprove.js
	\DeskPlusEIP\Approval\Status\ListComplete.aspx
	\DeskPlusEIP\Approval\Js\Status\ListComplete.js
	\DeskPlusEIP\Approval\Status\ListConfirm.aspx
	\DeskPlusEIP\Approval\Js\Status\ListConfirm.js
	\DeskPlusEIP\Approval\Status\ListDelay.aspx
	\DeskPlusEIP\Approval\Js\Status\ListDelay.js
	\DeskPlusEIP\Approval\Status\ListProcess.aspx
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\Approval\Status\ListProxy.aspx
	\DeskPlusEIP\Approval\Js\Status\ListProxy.js
	\DeskPlusEIP\Approval\Status\ListReceive.aspx
	\DeskPlusEIP\Approval\Js\Status\ListReceive.js
	\DeskPlusEIP\Approval\Status\ListReceiveReject.aspx
	\DeskPlusEIP\Approval\Js\Status\ListReceiveReject.js
	\DeskPlusEIP\Approval\Status\ListReject.aspx
	\DeskPlusEIP\Approval\Js\Status\ListReject.js
	\DeskPlusEIP\Approval\Status\ListRelated.aspx
	\DeskPlusEIP\Approval\Js\Status\ListRelated.js
	\DeskPlusEIP\Approval\Status\ListTransfer.aspx
	\DeskPlusEIP\Approval\Js\Status\ListTransfer.js
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx

	--Board
	\DeskPlusEIP\Board\Admin\Content\List.aspx
	\DeskPlusEIP\Board\Js\Admin\Content\List.js
	\DeskPlusEIP\Board\Admin\Template\List.aspx
	\DeskPlusEIP\Board\Js\Admin\Template\List.js
	\DeskPlusEIP\Board\Content\BlogList.aspx
	\DeskPlusEIP\Board\Js\Content\BlogList.js
	\DeskPlusEIP\Board\Content\BlogListImpo.aspx
	\DeskPlusEIP\Board\Js\Content\BlogListImpo.js
	\DeskPlusEIP\Board\Content\GalleryList.aspx
	\DeskPlusEIP\Board\Js\Content\GalleryList.js
	\DeskPlusEIP\Board\Content\GalleryListImpo.aspx
	\DeskPlusEIP\Board\Js\Content\GalleryListImpo.js
	\DeskPlusEIP\Board\Content\List.aspx
	\DeskPlusEIP\Board\Js\Content\List.js
	\DeskPlusEIP\Board\Content\ListImpo.aspx
	\DeskPlusEIP\Board\Js\Content\ListImpo.js
	\DeskPlusEIP\Board\Home\MoreList.aspx
	\DeskPlusEIP\Board\Js\Home\MoreList.js
	\DeskPlusEIP\Board\Temp\BlogList.aspx
	\DeskPlusEIP\Board\Js\Temp\BlogList.js
	\DeskPlusEIP\Board\Temp\GalleryList.aspx
	\DeskPlusEIP\Board\Js\Temp\GalleryList.js
	\DeskPlusEIP\Board\Temp\List.aspx
	\DeskPlusEIP\Board\Js\Temp\List.js
	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx

	--Common
	\DeskPlusEIP\Common\WebFax\Admin\List.aspx
	\DeskPlusEIP\Common\Js\WebFax\Admin\List.js
	\DeskPlusEIP\Common\WebFax\AddressBook.aspx
	\DeskPlusEIP\Common\Js\WebFax\AddressBook.js
	\DeskPlusEIP\Common\WebFax\RecvList.aspx
	\DeskPlusEIP\Common\Js\WebFax\RecvList.js
	\DeskPlusEIP\Common\WebFax\SendList.aspx
	\DeskPlusEIP\Common\Js\WebFax\SendList.js
	\DeskPlusEIP\Common\ZipCode\ZipCode.aspx
	\DeskPlusEIP\Common\Js\ZipCode\List.js
	\DeskPlusEIP\Common\ZipCode\ZipCodeNew.aspx
	\DeskPlusEIP\Common\Js\ZipCode\ListNew.js
	\DeskPlusEIP\Common\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Common\Resource\GlobalString\string.zh-tw.resx

	--Community
	\DeskPlusEIP\Community\Admin\Site\ListAdd.aspx
	\DeskPlusEIP\Community\Js\Admin\Site\ListAdd.js
	\DeskPlusEIP\Community\Admin\Site\ListClose.aspx
	\DeskPlusEIP\Community\Js\Admin\Site\ListClose.js
	\DeskPlusEIP\Community\Site\Admin\Member\ListAdd.aspx
	\DeskPlusEIP\Community\Js\Site\Admin\Member\ListAdd.js
	\DeskPlusEIP\Community\Site\Admin\Member\ListGood.aspx
	\DeskPlusEIP\Community\Js\Site\Admin\Member\ListGood.js
	\DeskPlusEIP\Community\Site\Admin\Member\ListMod.aspx
	\DeskPlusEIP\Community\Js\Site\Admin\Member\ListMod.js
	\DeskPlusEIP\Community\Site\Apps\Board\List.aspx
	\DeskPlusEIP\Community\Js\Site\Apps\Board\List.js
	\DeskPlusEIP\Community\Site\Apps\Board\ListBlog.aspx
	\DeskPlusEIP\Community\Site\Apps\Board\ListGallery.aspx
	\DeskPlusEIP\Community\Js\Site\Apps\Board\ListGallery.js
	\DeskPlusEIP\Community\Site\Main\Mail\Address\Default.aspx
	\DeskPlusEIP\Community\Js\Site\Main\Mail\Address\Default.js
	\DeskPlusEIP\Community\Site\Main\Search\List.aspx
	\DeskPlusEIP\Community\Js\Site\Main\Search\List.js
	\DeskPlusEIP\Community\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Community\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Community\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Community\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Community\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Community\Resource\GlobalString\string.zh-tw.resx

	--Coop
	\DeskPlusEIP\Coop\Admin\Content\List.aspx
	\DeskPlusEIP\Coop\Js\Admin\Content\List.js
	\DeskPlusEIP\Coop\Content\Forward\List.aspx
	\DeskPlusEIP\Coop\Js\Content\Forward\List.js
	\DeskPlusEIP\Coop\Content\Receive\List.aspx
	\DeskPlusEIP\Coop\Js\Content\Receive\List.js
	\DeskPlusEIP\Coop\Content\Reference\List.aspx
	\DeskPlusEIP\Coop\Js\Content\Reference\List.js
	\DeskPlusEIP\Coop\Content\Request\GetListBase.aspx
	\DeskPlusEIP\Coop\Js\Content\Request\GetListBase.js
	\DeskPlusEIP\Coop\Content\Request\List.aspx
	\DeskPlusEIP\Coop\Js\Content\Request\List.js
	\DeskPlusEIP\Coop\Content\Total\List.aspx
	\DeskPlusEIP\Coop\Js\Content\Total\List.js
	\DeskPlusEIP\Coop\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Coop\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Coop\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Coop\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Coop\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Coop\Resource\GlobalString\string.zh-tw.resx

	--EIP
	\DeskPlusEIP\EIP\Admin\Auth\List.aspx
	\DeskPlusEIP\EIP\Js\Admin\Auth\List.js
	\DeskPlusEIP\EIP\Admin\Code\List.aspx
	\DeskPlusEIP\EIP\Js\Admin\Code\List.js
	\DeskPlusEIP\EIP\Admin\Portlet\List.aspx
	\DeskPlusEIP\EIP\Js\Admin\Portlet\List.js
	\DeskPlusEIP\EIP\Admin\Static\Attach\List.aspx
	\DeskPlusEIP\EIP\Js\Admin\Static\Attach\List.js
	\DeskPlusEIP\EIP\Admin\Static\LogIn\List.aspx
	\DeskPlusEIP\EIP\Js\Admin\Static\LogIn\List.js
	\DeskPlusEIP\EIP\Admin\Static\Password\List.aspx
	\DeskPlusEIP\EIP\Js\Admin\Static\Password\List.js
	\DeskPlusEIP\EIP\Admin\Static\User\List.aspx
	\DeskPlusEIP\EIP\Js\Admin\Static\User\List.js
	\DeskPlusEIP\EIP\Search\List.aspx
	\DeskPlusEIP\EIP\Js\Search\List.js
	\DeskPlusEIP\EIP\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\EIP\Resource\GlobalString\string.zh-tw.resx

	--ETC
	\DeskPlusEIP\ETC\Customize\Admin\ListAdmin.aspx
	\DeskPlusEIP\ETC\Js\Customize\Admin\ListAdmin.js
	\DeskPlusEIP\ETC\Customize\Admin\ListFinancial.aspx
	\DeskPlusEIP\ETC\Js\Customize\Admin\ListFinancial.js
	\DeskPlusEIP\ETC\Customize\User\Popup\PopDepartMent.aspx
	\DeskPlusEIP\ETC\Js\Customize\User\Popup\PopDepartMent.js
	\DeskPlusEIP\ETC\Customize\User\Popup\PopEmployee.aspx
	\DeskPlusEIP\ETC\Js\Customize\User\Popup\PopEmployee.js
	\DeskPlusEIP\ETC\Customize\User\Popup\PopGLAccount.aspx
	\DeskPlusEIP\ETC\Js\Customize\User\Popup\PopGLAccount.js
	\DeskPlusEIP\ETC\Customize\User\Popup\PopProductGroup.aspx
	\DeskPlusEIP\ETC\Js\Customize\User\Popup\PopProductGroup.js
	\DeskPlusEIP\ETC\Holiday\ListAdmin.aspx
	\DeskPlusEIP\ETC\Js\Holiday\ListAdmin.js
	\DeskPlusEIP\ETC\Pay\List.aspx
	\DeskPlusEIP\ETC\Js\Pay\List.js
	\DeskPlusEIP\ETC\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\ETC\Resource\GlobalString\string.zh-tw.resx

	--MDM
	\DeskPlusEIP\MDM\Auth\ListAdd.aspx
	\DeskPlusEIP\MDM\Js\Auth\ListAdd.js
	\DeskPlusEIP\MDM\Auth\ListReject.aspx
	\DeskPlusEIP\MDM\Js\Auth\ListReject.js
	\DeskPlusEIP\MDM\Auth\ListUse.aspx
	\DeskPlusEIP\MDM\Js\Auth\ListUse.js
	\DeskPlusEIP\MDM\Dept\List.aspx
	\DeskPlusEIP\MDM\Js\Dept\List.js
	\DeskPlusEIP\MDM\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\MDM\Resource\GlobalString\string.zh-tw.resx

	--Opinion
	\DeskPlusEIP\Opinion\Admin\Content\QuesList.aspx
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesList.js
	\DeskPlusEIP\Opinion\Admin\Content\UnitList.aspx
	\DeskPlusEIP\Opinion\Js\Admin\Content\UnitList.js
	\DeskPlusEIP\Opinion\Content\FavList.aspx
	\DeskPlusEIP\Opinion\Js\Content\FavList.js
	\DeskPlusEIP\Opinion\Content\List.aspx
	\DeskPlusEIP\Opinion\Js\Content\List.js
	\DeskPlusEIP\Opinion\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Opinion\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Opinion\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Opinion\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Opinion\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Opinion\Resource\GlobalString\string.zh-tw.resx

	--Pim
	\DeskPlusEIP\Pim\Admin\HolidayD\List.aspx
	\DeskPlusEIP\Pim\Js\Admin\HolidayD\List.js
	\DeskPlusEIP\Pim\Admin\HolidayR\List.aspx
	\DeskPlusEIP\Pim\Js\Admin\HolidayR\List.js
	\DeskPlusEIP\Pim\Admin\ShareUser\List.aspx
	\DeskPlusEIP\Pim\Js\Admin\ShareUser\List.js
	\DeskPlusEIP\Pim\Anniversary\List.aspx
	\DeskPlusEIP\Pim\Js\Anniversary\List.js
	\DeskPlusEIP\Pim\Schedule\AllSchedule.aspx
	\DeskPlusEIP\Pim\Js\Schedule\AllSchedule.js
	\DeskPlusEIP\Pim\Schedule\ListSearch.aspx
	\DeskPlusEIP\Pim\Js\Schedule\ListSearch.js
	\DeskPlusEIP\Pim\Schedule\ScheduleMain.aspx
	\DeskPlusEIP\Pim\Js\Schedule\ScheduleMain.js
	\DeskPlusEIP\Pim\Schedule\ShortReplyList.aspx
	\DeskPlusEIP\Pim\Js\Schedule\ShortReplyList.js
	\DeskPlusEIP\Pim\ShortReply\ShortReplyGroup.aspx
	\DeskPlusEIP\Pim\Js\ShortReply\ShortReplyGroup.js
	\DeskPlusEIP\Pim\ShortReply\ShortReplyList.aspx
	\DeskPlusEIP\Pim\Js\ShortReply\ShortReplyList.js
	\DeskPlusEIP\Pim\ToDo\ShortReplyList.aspx
	\DeskPlusEIP\Pim\Js\ToDo\ShortReplyList.js
	\DeskPlusEIP\Pim\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Pim\Resource\GlobalString\string.zh-tw.resx

	--PMS
	\DeskPlusEIP\PMS\Admin\Content\JListClose.aspx
	\DeskPlusEIP\PMS\Js\Admin\Content\JListClose.js
	\DeskPlusEIP\PMS\Admin\Content\JListLinked.aspx
	\DeskPlusEIP\PMS\Js\Admin\Content\JListLinked.js
	\DeskPlusEIP\PMS\Admin\Content\JListOpen.aspx
	\DeskPlusEIP\PMS\Js\Admin\Content\JListOpen.js
	\DeskPlusEIP\PMS\Admin\Content\ListClose.aspx
	\DeskPlusEIP\PMS\Js\Admin\Content\ListClose.js
	\DeskPlusEIP\PMS\Admin\Content\ListLinked.aspx
	\DeskPlusEIP\PMS\Js\Admin\Content\ListLinked.js
	\DeskPlusEIP\PMS\Admin\Content\ListOpen.aspx
	\DeskPlusEIP\PMS\Js\Admin\Content\ListOpen.js
	\DeskPlusEIP\PMS\Admin\Member\JList.aspx
	\DeskPlusEIP\PMS\Js\Admin\Member\JList.js
	\DeskPlusEIP\PMS\Admin\Member\List.aspx
	\DeskPlusEIP\PMS\Js\Admin\Member\List.js
	\DeskPlusEIP\PMS\Board\JList.aspx
	\DeskPlusEIP\PMS\Js\Board\JList.js
	\DeskPlusEIP\PMS\Board\List.aspx
	\DeskPlusEIP\PMS\Js\Board\List.js
	\DeskPlusEIP\PMS\Member\JList.aspx
	\DeskPlusEIP\PMS\Js\Member\JList.js
	\DeskPlusEIP\PMS\Member\List.aspx
	\DeskPlusEIP\PMS\Js\Member\List.js
	\DeskPlusEIP\PMS\Project\JList.aspx
	\DeskPlusEIP\PMS\Js\Project\JList.js
	\DeskPlusEIP\PMS\Project\List.aspx
	\DeskPlusEIP\PMS\Js\Project\List.js
	\DeskPlusEIP\PMS\Related\JList.aspx
	\DeskPlusEIP\PMS\Js\Related\JList.js
	\DeskPlusEIP\PMS\Related\List.aspx
	\DeskPlusEIP\PMS\Js\Related\List.js
	\DeskPlusEIP\PMS\Related\ProjectList.aspx
	\DeskPlusEIP\PMS\Js\Related\ProjectList.js
	\DeskPlusEIP\PMS\Template\JList.aspx
	\DeskPlusEIP\PMS\Js\Template\JList.js
	\DeskPlusEIP\PMS\Template\List.aspx
	\DeskPlusEIP\PMS\Js\Template\List.js
	\DeskPlusEIP\PMS\Unit\JList.aspx
	\DeskPlusEIP\PMS\Js\Unit\JList.js
	\DeskPlusEIP\PMS\Unit\List.aspx
	\DeskPlusEIP\PMS\Js\Unit\List.js
	\DeskPlusEIP\PMS\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\PMS\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\PMS\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\PMS\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\PMS\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\PMS\Resource\GlobalString\string.zh-tw.resx

	--Portlet
	\DeskPlusEIP\Portlet\Package\SMS\AddressBook.aspx
	\DeskPlusEIP\Portlet\Js\Package\SMS\AddressBook.js
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.zh-tw.resx

	--Proposal
	\DeskPlusEIP\Proposal\Content\List.aspx
	\DeskPlusEIP\Proposal\Js\Content\List.js
	\DeskPlusEIP\Proposal\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Proposal\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Proposal\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Proposal\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Proposal\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Proposal\Resource\GlobalString\string.zh-tw.resx

	--Report
	\DeskPlusEIP\Report\Content\AdminList.aspx
	\DeskPlusEIP\Report\Js\Content\AdminList.js
	\DeskPlusEIP\Report\Content\ConfirmList.aspx
	\DeskPlusEIP\Report\Js\Content\ConfirmList.js
	\DeskPlusEIP\Report\Content\DeptList.aspx
	\DeskPlusEIP\Report\Js\Content\DeptList.js
	\DeskPlusEIP\Report\Content\InformList.aspx
	\DeskPlusEIP\Report\Js\Content\InformList.js
	\DeskPlusEIP\Report\Content\PrivateList.aspx
	\DeskPlusEIP\Report\Js\Content\PrivateList.js
	\DeskPlusEIP\Report\Content\ShareList.aspx
	\DeskPlusEIP\Report\Js\Content\ShareList.js
	\DeskPlusEIP\Report\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Report\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Report\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Report\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Report\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Report\Resource\GlobalString\string.zh-tw.resx

	--RM
	\DeskPlusEIP\RM\Admin\Content\List.aspx
	\DeskPlusEIP\RM\Js\Admin\Content\List.js
	\DeskPlusEIP\RM\Admin\Content\ListQRCode.aspx
	\DeskPlusEIP\RM\Js\Admin\Content\ListQRCode.js
	\DeskPlusEIP\RM\Content\ListMain.aspx
	\DeskPlusEIP\RM\Js\Content\ListMain.js
	\DeskPlusEIP\RM\Content\MyList.aspx
	\DeskPlusEIP\RM\Js\Content\MyList.js
	\DeskPlusEIP\RM\Admin\Reserve\Usable0List.aspx
	\DeskPlusEIP\RM\Js\Admin\Reserve\Usable0List.js
	\DeskPlusEIP\RM\Admin\Reserve\Usable1List.aspx
	\DeskPlusEIP\RM\Js\Admin\Reserve\Usable1List.js
	\DeskPlusEIP\RM\Admin\Reserve\Usable2List.aspx
	\DeskPlusEIP\RM\Js\Admin\Reserve\Usable2List.js
	\DeskPlusEIP\RM\Content\Manager\Usable0List.aspx
	\DeskPlusEIP\RM\Js\Content\Manager\Usable0List.js
	\DeskPlusEIP\RM\Content\Manager\Usable1List.aspx
	\DeskPlusEIP\RM\Js\Content\Manager\Usable1List.js
	\DeskPlusEIP\RM\Content\Manager\Usable2List.aspx
	\DeskPlusEIP\RM\Js\Content\Manager\Usable2List.js
	\DeskPlusEIP\RM\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-tw.resx

	--Search
	\DeskPlusEIP\Search\Admin\Index\List.aspx
	\DeskPlusEIP\Search\Js\Admin\Index\List.js
	\DeskPlusEIP\Search\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Search\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Search\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Search\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Search\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Search\Resource\GlobalString\string.zh-tw.resx

	--SNS
	\DeskPlusEIP\SNS\Common\ApprovalTalk.aspx
	\DeskPlusEIP\SNS\Js\Common\ApprovalTalk.js
	\DeskPlusEIP\SNS\Common\MyTalk.aspx
	\DeskPlusEIP\SNS\Js\Common\MyTalk.js
	\DeskPlusEIP\SNS\Common\MyTalkGroup.aspx
	\DeskPlusEIP\SNS\Js\Common\MyTalkGroup.js
	\DeskPlusEIP\SNS\Common\ScheduleTalk.aspx
	\DeskPlusEIP\SNS\Js\Common\ScheduleTalk.js
	\DeskPlusEIP\SNS\Common\ToDoTalk.aspx
	\DeskPlusEIP\SNS\Js\Common\ToDoTalk.js
	\DeskPlusEIP\SNS\MyTalk\Content\List.aspx
	\DeskPlusEIP\SNS\Js\MyTalk\Content\List.js
	\DeskPlusEIP\SNS\MyTalk\Content\ShareList.aspx
	\DeskPlusEIP\SNS\Js\MyTalk\Content\ShareList.js
	\DeskPlusEIP\SNS\MyTalk\Menu\Menu.aspx
	\DeskPlusEIP\SNS\Js\MyTalk\Menu\Menu.js
	\DeskPlusEIP\SNS\WorkTalk\Content\List.aspx
	\DeskPlusEIP\SNS\Js\WorkTalk\Content\List.js
	\DeskPlusEIP\SNS\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.zh-tw.resx

	--Statistics
	\DeskPlusEIP\Statistics\Content\Attach\List.aspx
	\DeskPlusEIP\Statistics\Js\Content\Attach\List.js
	\DeskPlusEIP\Statistics\Content\Login\List.aspx
	\DeskPlusEIP\Statistics\Js\Content\Login\List.js
	\DeskPlusEIP\Statistics\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Statistics\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Statistics\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Statistics\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Statistics\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Statistics\Resource\GlobalString\string.zh-tw.resx

	--Task
	\DeskPlusEIP\Task\Admin\Content\List.aspx
	\DeskPlusEIP\Task\Js\Admin\Content\List.js
	\DeskPlusEIP\Task\Content\Forward\List.aspx
	\DeskPlusEIP\Task\Js\Content\Forward\List.js
	\DeskPlusEIP\Task\Content\Receive\List.aspx
	\DeskPlusEIP\Task\Js\Content\Receive\List.js
	\DeskPlusEIP\Task\Content\Reference\List.aspx
	\DeskPlusEIP\Task\Js\Content\Reference\List.js
	\DeskPlusEIP\Task\Content\Request\List.aspx
	\DeskPlusEIP\Task\Js\Content\Request\List.js
	\DeskPlusEIP\Task\Content\Total\List.aspx
	\DeskPlusEIP\Task\Js\Content\Total\List.js
	\DeskPlusEIP\Task\Draft\AddList.aspx
	\DeskPlusEIP\Task\Js\Draft\AddList.js
	\DeskPlusEIP\Task\Draft\List.aspx
	\DeskPlusEIP\Task\Js\Draft\List.js
	\DeskPlusEIP\Task\ImpoUser\List.aspx
	\DeskPlusEIP\Task\Js\ImpoUser\List.js
	\DeskPlusEIP\Task\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Task\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Task\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Task\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Task\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Task\Resource\GlobalString\string.zh-tw.resx

	--Visit
	\DeskPlusEIP\Visit\Content\ListMain.aspx
	\DeskPlusEIP\Visit\Js\Content\ListMain.js
	\DeskPlusEIP\Visit\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Visit\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Visit\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Visit\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Visit\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Visit\Resource\GlobalString\string.zh-tw.resx

	--WebHard
	\DeskPlusEIP\WebHard\Content\List.aspx
	\DeskPlusEIP\WebHard\Js\Content\List.js
	\DeskPlusEIP\WebHard\Content\ListIcon.aspx
	\DeskPlusEIP\WebHard\Js\Content\ListIcon.js
	\DeskPlusEIP\WebHard\Content\ListImpo.aspx
	\DeskPlusEIP\WebHard\Js\Content\ListImpo.js
	\DeskPlusEIP\WebHard\Content\ListImpoIcon.aspx
	\DeskPlusEIP\WebHard\Js\Content\ListImpoIcon.js
	\DeskPlusEIP\WebHard\Content\ListImpoPreview.aspx
	\DeskPlusEIP\WebHard\Js\Content\ListImpoPreview.js
	\DeskPlusEIP\WebHard\Content\ListPreview.aspx
	\DeskPlusEIP\WebHard\Js\Content\ListPreview.js
	\DeskPlusEIP\WebHard\Trash\List.aspx
	\DeskPlusEIP\WebHard\Js\Trash\List.js
	\DeskPlusEIP\WebHard\Trash\ListIcon.aspx
	\DeskPlusEIP\WebHard\Js\Trash\ListIcon.js
	\DeskPlusEIP\WebHard\Trash\ListPreview.aspx
	\DeskPlusEIP\WebHard\Js\Trash\ListPreview.js
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\WebHard\Resource\GlobalString\string.zh-tw.resx

: 2017-03-09 by withsuki ---------------------------------------------------------
[전자결재] 알람 포맷 변경 (DP요청사항)

	\DeskPlusEIP\Approval\App_Data\Confirm\en.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\fr.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\ja.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\ko.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\zh-cn.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\zh-tw.xml

: 2017-03-09 by withsuki ---------------------------------------------------------
[전자결재] 인쇄시, 관련 내용도 함께 출력 (기획)

	\DeskPlusEIP\Approval\Admin\FormGeneration\Add.aspx
	\DeskPlusEIP\Approval\Common\Popup\PreView.aspx
	\DeskPlusEIP\Approval\Common\Popup\Print.aspx
	\DeskPlusEIP\Approval\Css\Func_ApprovalView.css
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Resource\GlobalString\
	\DeskPlusEIP\Approval\Status\Common\View.aspx
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\Common.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewCir.js
	\DeskPlusEIP\Approval\Js\Common\Popup\View.js
	\DeskPlusEIP\Approval\Js\Common\Popup\Print.js
	\DeskPlusEIP\Approval\Js\Disuse\View.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewHistory.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\Preference\FavoriteForm\View.js
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusEIP\Util\Component\Print\print.html
	\DeskPlusEIP\Util\Component\Print\smsx2.cab

: 2017-03-09 by famo23 ---------------------------------------------------------
[자원관리] 담당자 승인이 필요한 자원인지 보여주는 부분 개발 (기획업무)
	리소스추가
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_RM.css
	\DeskPlusEIP\RM\Content\AddResource.aspx
	\DeskPlusEIP\RM\Content\ViewQRCode.aspx
	\DeskPlusEIP\RM\Content\ViewQRCode.aspx.cs
	\DeskPlusEIP\RM\Content\WholeDay.aspx.cs
	\DeskPlusEIP\RM\Content\WholeWeek.aspx.cs
	\DeskPlusEIP\RM\Js\Content\AddResource.js
	\DeskPlusEIP\RM\Jt\Content\JTListDay.htm
	\DeskPlusEIP\RM\Jt\Content\JTListWeek.htm
	\DeskPlusEIP\RM\Ws\Content\ListDay.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\ListWeek.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ReserveInfo.cs

[자원관리] 관리자가 볼수 있는 전체 예약자원관리 화면 개발 
	리소스추가
	\DeskPlusEIP\RM\Admin\Folder\Tree.aspx
	\DeskPlusEIP\RM\Admin\Reserve\Mod.aspx
	\DeskPlusEIP\RM\Admin\Reserve\ModN.aspx
	\DeskPlusEIP\RM\Admin\Reserve\Usable0List.aspx
	\DeskPlusEIP\RM\Admin\Reserve\Usable1List.aspx
	\DeskPlusEIP\RM\Admin\Reserve\Usable2List.aspx
	\DeskPlusEIP\RM\Admin\Reserve\View.aspx
	\DeskPlusEIP\RM\Folder\Tree.aspx
	\DeskPlusEIP\RM\Js\Admin\Folder\Tree.js
	\DeskPlusEIP\RM\Js\Admin\Reserve\Mod.js
	\DeskPlusEIP\RM\Js\Admin\Reserve\Usable0List.js
	\DeskPlusEIP\RM\Js\Admin\Reserve\Usable1List.js
	\DeskPlusEIP\RM\Js\Admin\Reserve\Usable2List.js
	\DeskPlusEIP\RM\Js\Admin\Reserve\View.js
	\DeskPlusEIP\RM\Jt\Admin\Reserve\JTReply.htm
	\DeskPlusEIP\RM\Jt\Admin\Reserve\JTUsableList.htm
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Mod.asmx.cs
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable0List.asmx.cs
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable1List.asmx.cs
	\DeskPlusEIP\RM\Ws\Admin\Reserve\Usable2List.asmx.cs
	\DeskPlusEIP\RM\Ws\Admin\Reserve\View.asmx.cs

: 2017-03-09 by maniaysw ---------------------------------------------------------
[게시판] 메인팝업 등록시 팝업사이즈 예시 제공 (기획업무)

	\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Content\ModN.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusEIP\Board\Js\Content\Mod.js
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx


: 2017-03-08 by famo23 ---------------------------------------------------------
[자원관리] 본인이 주담당자인 담당자원내역 보기
	리소스추가
	QRY_RM_List_SelListEtc
	\DeskPlusEIP\RM\Content\Manager\List.aspx
	\DeskPlusEIP\RM\Content\Manager\Usable0List.aspx
	\DeskPlusEIP\RM\Content\Manager\Usable0List.aspx.cs
	\DeskPlusEIP\RM\Content\Manager\Usable1List.aspx
	\DeskPlusEIP\RM\Content\Manager\Usable1List.aspx.cs
	\DeskPlusEIP\RM\Content\Manager\Usable2List.aspx
	\DeskPlusEIP\RM\Content\Manager\Usable2List.aspx.cs
	\DeskPlusEIP\RM\Js\Content\Manager\List.js
	\DeskPlusEIP\RM\Js\Content\Manager\Usable0List.js
	\DeskPlusEIP\RM\Js\Content\Manager\Usable1List.js
	\DeskPlusEIP\RM\Js\Content\Manager\Usable2List.js
	\DeskPlusEIP\RM\Jt\Content\Manager\JTList.htm
	\DeskPlusEIP\RM\Ws\Content\Manager\List.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ListInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\List.cs

[자원관리] 담당자원관리 소스 이동 (Content폴더에서 하위폴더로 이동)
	Usable0List.aspx
	Usable1List.aspx 
	Usable2List.aspx
	관련 경로 전체수정

: 2017-03-08 by maniaysw -------------------------------------------------------
[게시판] 폴더 권한 사용자가 볼수 있는 기능 화면 추가 (기획업무)
	\DeskPlusEIP\Board\Content\AuthInfo.aspx
	\DeskPlusEIP\Board\Content\AuthInfo.aspx.cs
	\DeskPlusEIP\Board\Content\BlogList.aspx
	\DeskPlusEIP\Board\Content\BlogList.aspx.cs
	\DeskPlusEIP\Board\Content\GalleryList.aspx
	\DeskPlusEIP\Board\Content\GalleryList.aspx.cs
	\DeskPlusEIP\Board\Content\List.aspx
	\DeskPlusEIP\Board\Content\List.aspx.cs
	\DeskPlusEIP\Board\Js\Content\BlogList.js
	\DeskPlusEIP\Board\Js\Content\GalleryList.js
	\DeskPlusEIP\Board\Js\Content\List.js

: 2017-03-07 by 이지원 ---------------------------------------------------------
[게시판] 전사 아닌경우 알람 미발송 버그 수정 (개선업무)	

	-소스
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Lists.cs

	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Js\Content\Mod.js

	\DeskPlusEIP\Board\Admin\Content\Mod.aspx
	\DeskPlusEIP\Board\Admin\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Admin\Content\ModN.aspx
	\DeskPlusEIP\Board\Js\Admin\Content\Mod.js

: 2017-03-07 by famo23 ---------------------------------------------------------
[자원관리] 자원담당자 설명 추가 / '정' 체크박스 바로 선택되게 수정
	리소스 추가 ResourcesUserStr 
	\DeskPlusEIP\RM\Admin\Content\Add.aspx
	\DeskPlusEIP\RM\Admin\Content\Mod.aspx
	\DeskPlusEIP\RM\Admin\Content\View.aspx
	\DeskPlusEIP\RM\Js\Admin\Content\Add.js
	\DeskPlusEIP\RM\Js\Admin\Content\Mod.js

[자원관리] 승인대기 리스트 예약일정 빠른 순으로 정렬
	\DeskPlusEIP\RM\Content\Usable0List.aspx.cs

[포틀릿] 자원담당자용 승인대기 리스트 포틀릿(기획업무)
	DP_PT_RM_SelUsable0List
	DP_PT_RM_SelUsable0Cnt
	\DeskPlusEIP\Portlet\Jt\Package\RM\JTRmMaList.htm
	\DeskPlusEIP\Portlet\Package\RM\RMManager.ascx
	\DeskPlusEIP\Portlet\Ws\Package\RM\RMManager.asmx
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\RM.cs

: 2017-03-06 by famo23 ---------------------------------------------------------
[자원관리] js파일 순서위로 다시 변경
	\DeskPlusEIP\RM\Default.aspx

[포틀릿] 당일 전체 자원현황나오는 포틀릿에서 날짜 클릭시 상세화면 팝업처리
	\DeskPlusEIP\Portlet\Js\Package\ResourceList.js
	\DeskPlusEIP\Portlet\Jt\Package\JTResourceList.htm
	\DeskPlusEIP\Portlet\Ws\Package\Resource.asmx.cs

: 2017-03-06 by maniaysw ---------------------------------------------------------
[인사공통] 2차 패스워드 발송 개인 이메일 뒤에 2자리 * 처리 (메일아이디가 2자면 1자리만 * 처리) (기획업무)
	\DeskPlusEIP\Site\Auth\SendPassword.aspx.cs

: 2017-03-03 by withsuki ---------------------------------------------------------
[전자결재-기획] '타부서열람권한' 설정 UI 개선 : 전체내역조회 기능추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\FolderViewAuthorityInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FolderViewAuthority.cs
	\DeskPlusEIP\Approval\Admin\FolderAuth\List.aspx
	\DeskPlusEIP\Approval\Admin\FolderAuth\TotalList.aspx
	\DeskPlusEIP\Approval\Js\Admin\FolderAuth\List.js
	\DeskPlusEIP\Approval\Js\Admin\FolderAuth\TotalList.js
	\DeskPlusEIP\Approval\Jt\Admin\FolderAuth\JTTotalList.htm
	\DeskPlusEIP\Approval\Resource\GlobalString
	\DeskPlusEIP\Approval\Ws\Admin\FolderAuth\List.asmx

	sp추가) DP_APP_FolderViewAuth_SelTotList


: 2017-03-03 by famo23 -----------------------------------------------------------
[자원관리] 담당자원관리에 대기자원 카운트 보이게 처리 (기획업무)
		   승인요청알람 자원관리자(주)에게 가게 수정
		   보안예약보기 권한 수정
	QRY_RM_ListUser_SelByGubun
	\DeskPlusEIP\RM\Folder\Tree.aspx
	\DeskPlusEIP\RM\Folder\Tree.aspx.cs
	\DeskPlusEIP\RM\Content\ListMain.aspx
	\DeskPlusEIP\RM\Content\ListMain.aspx.cs
	\DeskPlusEIP\RM\Content\Mod.aspx
	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Content\Usable0List.aspx.cs
	\DeskPlusEIP\RM\Content\Usable1List.aspx.cs
	\DeskPlusEIP\RM\Content\Usable2List.aspx.cs
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Content\WholeDay.aspx.cs
	\DeskPlusEIP\RM\Content\WholeWeek.aspx.cs
	\DeskPlusEIP\RM\Home\Main.aspx
	\DeskPlusEIP\RM\Home\Main.aspx.cs
	\DeskPlusEIP\RM\Js\Content\ListDay.js
	\DeskPlusEIP\RM\Js\Content\ListMain.js
	\DeskPlusEIP\RM\Js\Content\ListMonth.js
	\DeskPlusEIP\RM\Js\Content\ListWeek.js
	\DeskPlusEIP\RM\Js\Content\Usable0List.js
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Js\Content\WholeDay.js
	\DeskPlusEIP\RM\Js\Content\WholeMonth.js
	\DeskPlusEIP\RM\Js\Content\WholeWeek.js
	\DeskPlusEIP\RM\Js\Home\Main.js
	\DeskPlusEIP\RM\Ws\Content\ListDay.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\ListWeek.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\Usable0List.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\Usable1List.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\Usable2List.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeDay.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\ListUser.cs


: 2017-03-02 by withsuki ---------------------------------------------------------
[전자결재-기획] '정보'아이콘 에 대한 기능 개선 (2안)
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_Approval.css
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentProcessInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	\DeskPlusEIP\Approval\Js\Status\Common\GetDocFlow.js
	\DeskPlusEIP\Approval\Ws\Status\Common\GetDocFlow.asmx.cs

	view수정)
		VW_APP_DocProcess
		VW_APP_Doc
	sp수정)
		DP_APP_CounterPartUser_UpdDecide


: 2017-03-02 by famo23 ---------------------------------------------------------
[자원관리] 메뉴 명칭 변경 (기획업무)
	리소스 수정

[자원관리] 거부시에 거부사유 입력하게 수정 (기획업무)
	리소스추가
	DP_RM_Reject 테이블
	QRY_RM_Reject_Sel
	DP_RM_Reject_Ins	
	DP_RM_Reject_Del
	DP_RM_Reserve_DelALL
	DP_RM_Reserve_DelByStartDate
	DP_RM_Reserve_DelUpd
	DP_RM_Reserve_DelByPID
	DP_RM_Reserve_UpdByUsable
	\DeskPlusEIP\RM\Content\Reject\AddReject.aspx
	\DeskPlusEIP\RM\Content\ListDay.aspx
	\DeskPlusEIP\RM\Content\ListMonth.aspx
	\DeskPlusEIP\RM\Content\ListWeek.aspx
	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Content\Usable0List.aspx
	\DeskPlusEIP\RM\Content\Usable1List.aspx
	\DeskPlusEIP\RM\Content\Usable2List.aspx
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Content\WholeDay.aspx
	\DeskPlusEIP\RM\Content\WholeMonth.aspx
	\DeskPlusEIP\RM\Content\WholeWeek.aspx
	\DeskPlusEIP\RM\Js\Content\Reject\AddReject.js
	\DeskPlusEIP\RM\Js\Content\ListDay.js
	\DeskPlusEIP\RM\Js\Content\ListMonth.js
	\DeskPlusEIP\RM\Js\Content\ListWeek.js
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Js\Content\WholeDay.js
	\DeskPlusEIP\RM\Js\Content\WholeMonth.js
	\DeskPlusEIP\RM\Js\Content\WholeWeek.js
	\DeskPlusEIP\RM\Jt\Content\JTReject.htm
	\DeskPlusEIP\RM\Ws\Content\Reject\AddReject.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\View.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\RejectInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Reject.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Reserve.cs

[자원관리] 보기페이지에서 처리내역(미승인/승인/거부) 보이게 수정
	리소스추가
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Content\ViewTotal.aspx
	\DeskPlusEIP\RM\Content\ViewTotal.aspx.cs

[자원관리] contextMenu에 나오는 값 처리내역(미승인/승인/거부)에 따라 다르게 나오게 수정
	\DeskPlusEIP\RM\Content\ListDay.aspx
	\DeskPlusEIP\RM\Content\ListMonth.aspx
	\DeskPlusEIP\RM\Content\ListWeek.aspx
	\DeskPlusEIP\RM\Content\WholeDay.aspx
	\DeskPlusEIP\RM\Content\WholeDay.aspx.cs
	\DeskPlusEIP\RM\Content\WholeMonth.aspx
	\DeskPlusEIP\RM\Content\WholeWeek.aspx
	\DeskPlusEIP\RM\Content\WholeWeek.aspx.cs
	\DeskPlusEIP\RM\Js\Content\ListDay.js
	\DeskPlusEIP\RM\Js\Content\ListMonth.js
	\DeskPlusEIP\RM\Js\Content\ListWeek.js
	\DeskPlusEIP\RM\Js\Content\WholeDay.js
	\DeskPlusEIP\RM\Js\Content\WholeMonth.js
	\DeskPlusEIP\RM\Js\Content\WholeWeek.js
	\DeskPlusEIP\RM\Jt\Content\JTListDay.htm
	\DeskPlusEIP\RM\Jt\Content\JTListWeek.htm
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx.cs

: 2017-02-28 by famo23 ---------------------------------------------------------
[포틀릿] 자원현황 포틀릿에서 승인된 자원만 보이게 처리
	DP_RM_Reserve_SelPortlet

: 2017-02-27 by maniaysw
[메신저] ID/PW 틀릴시 Exception 찍히는 문제 수정
	\DeskPlusEIP\Common\Messenger\UC\SLO.aspx.cs

: 2017-02-27 By 이지원  ----------------------------------------------------------
[공통] 회사 추가시 공휴일 설정
	DP_COM_Company_Ins
	DP_COM_Company_Del

: 2017-02-24 by 이지원 -----------------------------------------------------------
[게시판] 선택알람 전사게시판만 적용 수정
	--소스	
	\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Add.js

	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Mod.js

	\DeskPlusEIP\Board\Admin\Content\Add.aspx	
	\DeskPlusEIP\Board\Admin\Content\AddN.aspx
	\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Js\Admin\Content\Add.js

	\DeskPlusEIP\Board\Admin\Content\Mod.aspx	
	\DeskPlusEIP\Board\Admin\Content\ModN.aspx
	\DeskPlusEIP\Board\Admin\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Js\Admin\Content\Mod.js

: 2017-02-24 by 이지원 ---------------------------------------------------------
[개인업무] 관리자 메뉴 초기 리스트 변경
	\DeskPlusEIP\Pim\Admin\Default.aspx

: 2017-02-24 by famo23 ---------------------------------------------------------
[자원관리] 스킨12 적용시 일간/주간/월간 테이블 테두리가 안 보이는 문제
	/DeskPlusEIP/Design/C12/Css/Func_RM.css

[메신저] 리턴값 수정
	\DeskPlusEIP\Common\Messenger\UC\SLO.aspx.cs

[업무협업] 홈화면에 최신업데이트 업무 추가
	리소스추가
	DP_TAS_List_SelRecentList
	DP_TAS_List_SelRecentAdminList
	\DeskPlusEIP\Task\Common\AddComplete.aspx.cs
	\DeskPlusEIP\Task\Common\AddComplete.aspx
	\DeskPlusEIP\Task\Common\AddCompleteN.aspx
	\DeskPlusEIP\Task\Common\ModList.aspx.cs
	\DeskPlusEIP\Task\Common\ModList.aspx
	\DeskPlusEIP\Task\Common\ModListN.aspx
	\DeskPlusEIP\Task\Content\Receive\AddAccept.aspx.cs
	\DeskPlusEIP\Task\Content\Receive\AddAccept.aspx
	\DeskPlusEIP\Task\Content\Receive\AddAcceptN.aspx
	\DeskPlusEIP\Task\Content\Request\Mod.aspx.cs
	\DeskPlusEIP\Task\Content\Request\Mod.aspx
	\DeskPlusEIP\Task\Content\Request\ModN.aspx
	\DeskPlusEIP\Task\Home\AdminMain.aspx
	\DeskPlusEIP\Task\Home\AdminMain.aspx.cs
	\DeskPlusEIP\Task\Home\Main.aspx
	\DeskPlusEIP\Task\Home\Main.aspx.cs
	\DeskPlusEIP\Task\Home\View.aspx.cs
	\DeskPlusEIP\Task\Home\View.aspx
	\DeskPlusEIP\Task\Home\ViewN.aspx
	\DeskPlusEIP\Task\Js\Common\AddComplete.js
	\DeskPlusEIP\Task\Js\Common\ModList.js
	\DeskPlusEIP\Task\Js\Content\Receive\AddAccept.js
	\DeskPlusEIP\Task\Js\Content\Request\Mod.js
	\DeskPlusEIP\Task\Js\Home\AdminMain.js
	\DeskPlusEIP\Task\Js\Home\Main.js
	\DeskPlusEIP\Task\Js\Home\View.js
	\DeskPlusEIP\Task\Jt\Home\JTAdminRecent.htm
	\DeskPlusEIP\Task\Jt\Home\JTRecent.htm
	\DeskPlusEIP\Task\Ws\Home\AdminMain.asmx
	\DeskPlusEIP\Task\Ws\Home\Main.asmx
	\Projects\TaskProject\Zeospace.DeskPlus.Task.DAL\List.cs

: 2017-02-22 by freeman ---------------------------------------------------------
[전자결재] IE11에서 자주지정하는결재자 보완 (why, IE11에서, window.createPopup지원안함)
	- Iframe 위에서 Context Menu DIV 가 구동 될수 있도록 수정 
      : Context Menu가 뜨는 위치에 300 * 150 크기의 Iframe을 뒤에 숨겨서 AcitveX,Tagfree 위에서 보이도록 처리

	  \DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	  \DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

	  DeskPlusEIPBase.css  
	   : .popup 에 z-index:999 추가

	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Common.js

: 2017-02-21 by withsuki ---------------------------------------------------------
[전자결재] IE11에서 자주지정하는결재자 에러 (why, IE11에서, window.createPopup지원안함)
	- non-ActiveX버전으로 처리.
	- 사이트마다, 결재지정범위와 에디터간의 간격이 좁은경우, height값을 수정해야할수도 있음 (기본:100px)
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js : 함수명:Common.popUpFavList(e)


: 2017-02-16 by freeice122 ---------------------------------------------------------
[기타] 인사정보 메일링크
	소스수정)
	\DeskPlusEIP\Account\Common\AccountInfo.aspx
	\DeskPlusEIP\Account\Common\AccountInfo.aspx.cs
	\DeskPlusEIP\Account\js\Common\AccountInfo.js

: 2017-02-15 by maniaysw -------------------------------------------------------
[인사공통] 로그인 실패시 2차 인증메일 발송 개발 (개선업무)
	[aspx]
	\DeskPlusEIP\Site\Auth\SendPassword.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Account\Admin\User\Add.aspx
	\DeskPlusEIP\Account\Admin\User\Mod.aspx
	\DeskPlusEIP\Account\Admin\AuthEmail\List.aspx
	\DeskPlusEIP\Account\Admin\AuthEmail\Menu.aspx
	\DeskPlusEIP\Account\Admin\AuthEmail\Mod.aspx

	[js]
	DeskPlusEIP\Js\Site\Auth\SendPassword.js
	\DeskPlusEIP\Js\Site\Auth\Default.js
	\DeskPlusEIP\Account\Js\MyInfo\Mod.js
	\DeskPlusEIP\Account\Js\Admin\AuthEmail\List.js
	\DeskPlusEIP\Account\Js\Admin\AuthEmail\Menu.js
	\DeskPlusEIP\Account\Js\Admin\AuthEmail\Mod.js
	\DeskPlusEIP\EIP\Js\Package\Menu.js

	[cs]
	\DeskPlusEIP\Account\Admin\User\Add.aspx.cs
	\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs
	\DeskPlusEIP\Site\Auth\SendPassword.aspx.cs
	\DeskPlusEIP\Account\Admin\AuthEmail\List.aspx.cs
	\DeskPlusEIP\Account\Admin\AuthEmail\Menu.aspx.cs
	\DeskPlusEIP\Account\Admin\AuthEmail\Mod.aspx.cs
	\DeskPlusEIP\Account\Ws\Admin\AuthEmail\List.asmx.cs
	\AccountProject\Zeospace.DeskPlus.Account.DAL\PasswordHistory.cs
	\AccountProject\Zeospace.DeskPlus.Account.DAL\PasswordManager.cs
	\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs
	\AccountProject\Zeospace.DeskPlus.Account.BE\UserInfo.cs

	[xml]
	\DeskPlusEIP\EIP\App_Data\AdminMenu\ko.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\en.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\fr.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\ja.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\zh-cn.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\zh-tw.xml

	[resource]
	\DeskPlusEIP\Account\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Account\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-tw.resx

	[Procedure]
	DP_ACC_User_Upd
	DP_ACC_User_Ins
	DP_ACC_User_UpdPwdCount
	DP_ACC_USER_SelAuthMail
	DP_ACC_User_InsPwdManager
	DP_ACC_User_UpdPwdManager
	DP_ACC_User_DelPwdManager
	DP_ACC_User_SelPwdManager
	Dp_acc_user_UPDSendPWD
	DP_ACC_User_SelPwdHistoryList
	DP_ACC_User_SelPwdHistoryCnt

	[JT]
	\DeskPlusEIP\Account\Jt\Admin\AuthEmail\JTList.htm


	[Table]
	DP_ACC_PwdHistory
	DP_ACC_PwdManager

	[View]
	VW_UserInfo


: 2017-02-15 by kkzz52 ---------------------------------------------------------
[일정관리] 일정분류 선택의 모호함과 개인화 개선 검토 및 선택 분류 일정 제공 (개선업무)
	\DeskPlusEIP\Pim\Js\Schedule\ListDay.js
	\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
	\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js
	\DeskPlusEIP\Pim\Schedule\ListDay.aspx
	\DeskPlusEIP\Pim\Schedule\ListWeek.aspx
	\DeskPlusEIP\Pim\Schedule\ListMonth.aspx
	\DeskPlusEIP\Pim\Ws\Schedule\ListDay.asmx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\ListWeek.asmx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\ListMonth.asmx.cs
	\DeskPlusEIP\Pim\Schedule\ScheduleMain.aspx.cs

	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironment.cs

	\Design\Dev\C\Css\Func_Pim.css

	DP_ACC_USERENV PimCheckMenu 필드 추가
	DP_ACC_USERENV_UpdPimCheckMenu 추가
	DP_ACC_USERENV_SelPimCheckMenu 추가

: 2017-02-15 by freeice122 ---------------------------------------------------------
[게시판] URL복사 기능 추가 (개선업무)
	소스추가)
	\DeskPlusEIP\Base\Html\js\Clipboard\Clipboard.js
	\DeskPlusEIP\Base\Html\js.min\Clipboard\Clipboard.min.js

	소스수정)
	\DeskPlusEIP\Board\js\Content\view.js
	
	\DeskPlusEIP\Board\Content\SendMailDeep.aspx.cs
	\DeskPlusEIP\Board\js\Content\SendMailDeep.js

	리소스 추가)
	CopyURL, CopyURLComplete


[게시판] 메일발송 DeepUI변경
	소스추가)
	\DeskPlusEIP\Board\Content\SendMailDeep.aspx
	\DeskPlusEIP\Board\Content\SendMailDeep.aspx.cs
	\DeskPlusEIP\Board\js\Content\SendMailDeep.js
	소스수정)
	\DeskPlusEIP\Board\js\Content\view.js


: 2017-02-15 by 방영환 ---------------------------------------------------------
[결재] 문서보기의 근거문서 조회시 상단 정보 개선-문서 제목표기 (개선업무)
	소스수정)
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Folder\Common\ViewBaseDoc.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Status\Common\View.aspx
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\View.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Common\View.asmx.cs
	\DeskPlusEIP\Approval\Ws\FormFlow\Basic\GetBaseDoc.asmx.cs
	\DeskPlusEIP\Approval\Ws\FormFlow\Common\Common.asmx.cs
	\DeskPlusEIP\Approval\Ws\Process\Basic\Approve.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\Common\View.asmx.cs
	\DeskPlusEIP\Approval\Ws\Common\Popup\ViewTot.asmx.cs

	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Comon\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\GetBaseDoc.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js

	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTBaseDoc.htm
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTBaseDoc.htm
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTBaseDocPopup.htm
	\DeskPlusEIP\Approval\Jt\FormFlow\Common\JTBaseDocEdit.htm
	\DeskPlusEIP\Approval\Jt\FormFlow\Temporary\JTBaseDocEdit.htm
	\DeskPlusEIP\Approval\Jt\Process\Basic\JTBaseDoc.htm
	\DeskPlusEIP\Approval\Jt\Status\Common\JTBaseDoc.htm

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\CirculationBaseDocumentInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DraftBaseDocInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CirculationBaseDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DraftBaseDoc.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\BaseDocument.cs

	프로시저수정)
	DP_APP_DraftBaseDoc_Sel


	쿼리수정)
	QRY_APP_CirBaseDoc_Sel
	QRY_APP_BaseDoc_Sel

: 2017-02-15 by 방영환 ---------------------------------------------------------
[결재] 설문조사 탭형식 포틀릿 (개선업무)
	소스추가)
	\DeskPlusEIP\Portlet\Package\TabOpinion.ascx
	\DeskPlusEIP\Portlet\Package\TabOpinion.ascx.cs
	\DeskPlusEIP\Portlet\Jt\Package\JTMyOpinionVote.htm
	\DeskPlusEIP\Portlet\Jt\Package\JTMyOpinionVoteNon.htm

	소스수정)
	\DeskPlusEIP\Portlet\Ws\Package\Opinion.asmx.cs

	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\Opinion.cs

	프로시저추가)
	DP_PT_Opinion_SelMyVoteCnt
	DP_PT_Opinion_SelMyVoteList
	DP_PT_Opinion_SelMyVoteNonCnt
	DP_PT_Opinion_SelMyVoteNonList

: 2017-02-15 by withsudki ---------------------------------------------------------
[결재] 개인설정 항목에 결재시 패스워드 입력 및 변경 기능 (개선업무)

	소스)
	\DeskPlusEIP\Approval\Preference\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Preference\Setting\List.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx

	\DeskPlusEIP\Approval\Js\Preference\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\Preference\Setting\List.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	\DeskPlusEIP\Approval\Resource\GlobalString\

	DB) 테이블 필드추가 : DP_ACC_UserEnv (IsApprovePwd)
		SP추가 : DP_ACC_UserEnv_UpdApprovePwd

: 2017-02-15 by famo23 ---------------------------------------------------------
[포틀릿] 일정리스트 포틀릿 추가 (개선업무)
	리소스추가
	QRY_PORTLET_Schedule_SelDate
	QRY_PORTLET_Todo_SelDate
	\DeskPlusEIP\Portlet\Js\Package\PimList.js
	\DeskPlusEIP\Portlet\Package\PimList.ascx.cs
	\DeskPlusEIP\Portlet\Ws\Package\PimList.asmx
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.BE\PimInfo.cs
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\MyPim.cs

: 2017-02-14 by 방영환 ---------------------------------------------------------
[전자결재] 전자결재 메인페이 분할 (메인페이지와 검색페이지 분할)
	소스추가)
	\DeskPlusEIP\Approval\Home\Content.aspx.cs
	\DeskPlusEIP\Approval\Home\Menu.aspx.cs
	\DeskPlusEIP\Approval\Home\Search.aspx.cs

	\DeskPlusEIP\Approval\Ws\Home\Content.asmx
	\DeskPlusEIP\Approval\Ws\Home\Menu.asmx
	\DeskPlusEIP\Approval\Ws\Home\Search.asmx

	\DeskPlusEIP\Approval\Js\Home\Content.js
	\DeskPlusEIP\Approval\Js\Home\Menu.js
	\DeskPlusEIP\Approval\Js\Home\Search.js

	소스수정)
	\DeskPlusEIP\Approval\Home\Default.aspx.cs
	\DeskPlusEIP\Approval\Js\Home\Default.js
	\DeskPlusEIP\Approval\Ws\Home\Default.asmx.cs

	\DeskPlusEIP\Approval\Jt\JTFavAPP.htm
	\DeskPlusEIP\Approval\Jt\JTFavForm.htm
	\DeskPlusEIP\Approval\Jt\JTFile.htm
	\DeskPlusEIP\Approval\Jt\JTImpo.htm
	\DeskPlusEIP\Approval\Jt\JTListTab.htm
	\DeskPlusEIP\Approval\Jt\JTReply.htm
	\DeskPlusEIP\Approval\Jt\JTListDoc.htm

: 2017-02-13 by kkzz52 ---------------------------------------------------------
[일정관리] 할일, 일정 댓글 Count 추가 (개선업무)
	\DeskPlusEIP\Pim\Js\Schedule\ListDay.js
	\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
	\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js
	\DeskPlusEIP\Pim\Todo\SortTodoList.aspx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\ToDo.asmx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx.cs
	\DeskPlusEIP\Pim\Jt\ToDo\JTTodoList.htm

	\Projects\PimProject\Zeospace.DeskPlus.Pim.BE\ToDoInfo.cs

: 2017-02-10 by withsuki -----------------------------------------------------------
[포탈] 포탈 스킨변경 시, 메일스킨 함께 적용되도록 (개선업무)
--소스
	\DeskPlusEIP\EIP\Ws\Portal\GetSkin.asmx

: 2017-02-10 by withsuki -----------------------------------------------------------
[전자결재] 근거문서/첨부파일 건수 클릭 시, Actioin처리 (개선업무)
--소스
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx
	\DeskPlusEIP\Approval\Disuse\View.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\View.aspx
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Folder\Common\ViewBaseDoc.aspx
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Process\Confirm\Approve.aspx
	\DeskPlusEIP\Approval\Process\Confirm\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Status\Common\View.aspx
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.aspx
	\DeskPlusEIP\Approval\Js\Disuse\View.aspx
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.aspx
	\DeskPlusEIP\Approval\Js\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.aspx
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.aspx
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Js\Process\Confirm\Approve.aspx
	\DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Js\Status\Common\View.aspx
	\DeskPlusEIP\Approval\Ws\Common\Popup\ViewTot.asmx.cs
	\DeskPlusEIP\Approval\Ws\Disuse\View.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\View.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Common\View.asmx.cs
	\DeskPlusEIP\Approval\Ws\Process\Basic\Approve.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\Common\View.asmx.cs


: 2017-02-10 by 이지원 -----------------------------------------------------------
[공통] 자동완성 제거 (개선업무)
--소스
	--Account
	\DeskPlusEIP\Account\Js\Admin\User\MultiComUser.js
	\DeskPlusEIP\Account\Js\Common\AccountInfo.js
	\DeskPlusEIP\Account\Js\Search\Tree.js
	--Address
	\DeskPlusEIP\Address\Js\Content\Send.js
	--Approval
	\DeskPlusEIP\Approval\Js\Process\Basic\AlarmList.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeReceiveEdit.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeReceive.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeModReceive.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeMod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeLegacy.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeEdit.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\SendMail.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\ForwardMemo.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\CirMemo.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\SendMail.js
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDeptUserSelect.js
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDeptUser.js
	--Coop
	\DeskPlusEIP\Coop\Js\Common\GetUserList.js
	\DeskPlusEIP\Coop\Js\Common\GetUserDept.js
	--EIP
	\DeskPlusEIP\EIP\Js\Admin\Auth\Mod.js
	--Opinion
	\DeskPlusEIP\Opinion\Js\Folder\GetUserDept.js
	--Pim
	\DeskPlusEIP\Pim\Js\Admin\Share\List.js
	\DeskPlusEIP\Pim\Js\Admin\Share\ShareUserPop.js
	\DeskPlusEIP\Pim\Js\Admin\ShareUser\Add.js
	\DeskPlusEIP\Pim\Js\Admin\ShareUser\GetUser.js
	\DeskPlusEIP\Pim\Js\Common\ShareUser.js
	\DeskPlusEIP\Pim\Js\ShortReply\GetUser.js
	--Proposal
	\DeskPlusEIP\Proposal\Js\Common\GetUserList.js
	\DeskPlusEIP\Proposal\Js\Content\Admin\AuthList.js
	--Report
	\DeskPlusEIP\Report\Js\Content\AuthList.js
	--RM
	\DeskPlusEIP\RM\Js\Content\AddShareUser.js
	--SNS
	\DeskPlusEIP\SNS\Js\MyTalk\Content\GetUser.js
	--Visit
	\DeskPlusEIP\Visit\Js\Admin\Content\Share.js
	\DeskPlusEIP\Visit\Js\Content\AddShare.js
	--WebHard
	\DeskPlusEIP\WebHard\Js\Send\GetTarget.js
	\DeskPlusEIP\WebHard\Js\Share\GetShare.js

: 2017-02-08 by 이지원 -----------------------------------------------------------
[게시판] 선택알람 (개선업무)
	--소스	
	\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Add.js

	\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Js\Content\Mod.js

	\DeskPlusEIP\Board\Common\GetUserDept.aspx
	\DeskPlusEIP\Board\Common\GetUserDept.aspx.cs
	\DeskPlusEIP\Board\Js\Common\GetUserDept.js

	\DeskPlusEIP\Board\Common\GetUserDeptAll.aspx
	\DeskPlusEIP\Board\Common\GetUserDeptAll.aspx.cs
	\DeskPlusEIP\Board\Js\Common\GetUserDeptAll.js

	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\ListInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\MyTemplateInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\ListAlarmInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Lists.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\ListAlarm.cs

	--DB
	DP_BOA_ListAlarm

	--프로시져
	DP_BOA_List_Del
	DP_BOA_ListAlarm_Ins
	DP_BOA_ListAlarm_Del
	DP_BOA_ListAlarm_SelByID

	--쿼리
	QRY_BOA_ListAlarm_SelByID

: 2017-02-08 by maniaysw -----------------------------------------------------------
[자원관리] 나의예약 및 담당자원관리 엑셀 다운로드 (개선업무)
	\DeskPlusEIP\RM\Content\ExcelDown.aspx

: 2017-02-08 by 방영환 -----------------------------------------------------------
[전자결재] 결재진행중 참조문서 조회 가능하도록 수정(개선업무)
	소스수정)
	\DeskPlusEIP\Approval\Admin\Config\List.aspx
	\DeskPlusEIP\Approval\Admin\Config\List.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceive.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceive.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveEdit.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeReceiveEdit.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs
	\DeskPlusEIP\Approval\Status\Common\View.aspx
	\DeskPlusEIP\Approval\Status\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs

	\DeskPlusEIP\Approval\Common\Status\FolderStatusTreeScript.ascx.cs
	\DeskPlusEIP\Approval\Status\ListApprove.aspx.cs
	\DeskPlusEIP\Approval\Status\ListProxy.aspx.cs
	\DeskPlusEIP\Approval\Status\ListRelated.aspx.cs

	\DeskPlusEIP\Approval\Ws\Status\ListRelated.asmx.cs

	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentProcessInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DraftInfo.cs

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Draft.cs

	리소스 추가)
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
	
	CarbonUserViewOption - 참조자 열람 옵션
	CarbonUserViewOptionItem1 - 결재 진행중에도 열람
	CarbonUserViewOptionItem2 - 결재 완료후에만 열람
	Msg_CarbonUserViewOption - 참조자가 문서를 열람하는 시점을 설정할 수 있습니다.
	ReportUserCarbonViewOption - 상신자 참조열람 옵션
	Msg_ReportUserCarbonViewOption - 결재완료후 열람으로 설정시에도 상신자가 참조자에게 문서를 열람할 수 있도록 허용합니다
	CarbonView 참조열람

	테이블 수정)
	DP_APP_DocProcess - CarbonView 컬럼추가
	DP_APP_Draft -CarbonView 컬럼추가
	DP_APP_Doc - 	CarbonView 컬럼추가

	프로시저 수정)
	DP_APP_DocProcess_Ins
	DP_APP_Draft_Ins
	DP_APP_Draft_Sel
	DP_APP_DocProcess_SelPageListRelated
	DP_APP_DocProcess_SelPageCntRelated

	DPM_APP_DocProcess_SelRelated
	DPM_APP_DocProcess_SelRelatedCnt
	DPM_APP_SelCnt

	뷰수정)
	VW_APP_Doc
	VW_APP_DocProcess
	DP_APP_DocProcess_UpdEtcs

: 2017-02-08 by famo23 -----------------------------------------------------------
[자원관리] 본인의 당일승인완료자원/ 승인대기자원 탭포틀릿 개발(개선업무)
	VW_RM_ReserveTotal
	DP_PT_RM_SelMyUsable0List
	DP_PT_RM_SelMyUsable1List
	DP_PT_RM_SelMyUsable0Cnt
	DP_PT_RM_SelMyUsable1Cnt
	\DeskPlusEIP\Portlet\Jt\Package\RM\JTMyUsable0.htm
	\DeskPlusEIP\Portlet\Jt\Package\RM\JTMyUsable1.htm
	\DeskPlusEIP\Portlet\Package\RM\Tab.ascx
	\DeskPlusEIP\Portlet\Ws\Package\RM\List.asmx
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.BE\RMInfo.cs
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\RM.cs
	Func_Package.css

: 2017-02-07 by kkzz52 ---------------------------------------------------------
[전자결재] 부서참조조회 개발 (개선업무)
	\DeskPlusEIP\Approval\Status\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx.cs

	\DeskPlusEIP\Approval\Folder\Common\DocUserSeq.aspx
	\DeskPlusEIP\Approval\Js\Folder\Common\DocUserSeq.js
	\DeskPlusEIP\Approval\Ws\Folder\Common\DocUserSeq.asmx.cs
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTDocUserSeqCA.htm
	\DeskPlusEIP\Approval\Status\Common\DocUserSeq.aspx
	\DeskPlusEIP\Approval\Js\Status\Common\DocUserSeq.js
	\DeskPlusEIP\Approval\Ws\Status\Common\DocUserSeq.asmx.cs
	\DeskPlusEIP\Approval\Jt\Status\Common\JTDocUserSeqCA.htm

	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\List.js
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\List.asmx.cs
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTList.htm

	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\GroupList.js 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\GroupList.aspx 추가
	\DeskPlusEIP\Approval\Folder\Carboncopy\GroupList.aspx.cs 추가
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\GroupList.asmx 추가
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTGroupList.htm 추가


	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\CarbonCopyUserInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\CarbonCopyUserGroupInfo.cs 추가
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CarbonCopyUser.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CarbonCopyUserGroup.cs 추가

	DP_APP_Doc_SelPageListCarbon 수정
	DP_APP_CarbonCopyUser_UpdViewingDate 수정
	DP_APP_CarbonCopyUserGroup_Sel 추가
	ufn_GetCarbonCopySubDept 추가
	DP_APP_CarbonCopyUserGroup 추가

: 2017-02-07 by maniaysw ---------------------------------------------------------
[일정관리] 메인포틀릿에서 날짜선택시 선택한 일자의 일정관리 일간페이지로 이동 (개선업무)
	DeskPlusEIP\Pim\Default.aspx
	DeskPlusEIP\Pim\Schedule\ScheduleMain.aspx.cs
	DeskPlusEIP\Pim\Menu\PimMenu.aspx.cs

: 2017-02-02 by maniaysw -----------------------------------------------------------
[작성공통] 익스 제외 브라우저에서는 ActiveX 탭 보이지 않도록 (영업팀요청)
	\DeskPlusEIP\Visit\Js\Content\Add.js(98):            
	\DeskPlusEIP\Visit\Js\Content\Mod.js(97):            
	\DeskPlusEIP\Task\Js\Admin\Menu\Template.js(27):            
	\DeskPlusEIP\Task\Js\Common\AddComplete.js(105):                
	\DeskPlusEIP\Task\Js\Common\ModList.js(117):                
	\DeskPlusEIP\Task\Js\Common\ViewPopupTotal.js(119):                
	\DeskPlusEIP\Task\Js\Content\Forward\View.js(142):                
	\DeskPlusEIP\Task\Js\Content\Receive\AddAccept.js(126):                
	\DeskPlusEIP\Task\Js\Content\Receive\View.js(239):                
	\DeskPlusEIP\Task\Js\Content\Reference\View.js(153):                
	\DeskPlusEIP\Task\Js\Content\Request\Add.js(168):                
	\DeskPlusEIP\Task\Js\Content\Request\Mod.js(142):                
	\DeskPlusEIP\Task\Js\Content\Request\View.js(217):                
	\DeskPlusEIP\Task\Js\Content\Total\View.js(142):                
	\DeskPlusEIP\Task\Js\Draft\Add.js(133):                
	\DeskPlusEIP\Task\Js\Home\View.js(282):
	\DeskPlusEIP\Board\Js\Admin\Content\Add.js(89):            
	\DeskPlusEIP\Board\Js\Admin\Content\Mod.js(98):            
	\DeskPlusEIP\Board\Js\Admin\Content\Reply.js(50):            
	\DeskPlusEIP\Board\Js\Admin\Template\Add.js(29):            
	\DeskPlusEIP\Board\Js\Content\Add.js(136):            
	\DeskPlusEIP\Board\Js\Content\Mod.js(96):            
	\DeskPlusEIP\Board\Js\Content\Reply.js(56):            
	\DeskPlusEIP\Board\Js\Temp\Add.js(50):            
	\DeskPlusEIP\Board\Js\Temp\Mod.js(26):            
	\DeskPlusEIP\Pim\Js\Admin\Holiday\Add.js(71):            
	\DeskPlusEIP\Pim\Js\Admin\Holiday\Mod.js(59):            
	\DeskPlusEIP\Pim\Js\Anniversary\Add.js(53):            
	\DeskPlusEIP\Pim\Js\Anniversary\Mod.js(55):            
	\DeskPlusEIP\Pim\Js\Schedule\Add.js(149):            
	\DeskPlusEIP\Pim\Js\Schedule\Mod.js(158):            
	\DeskPlusEIP\Pim\Js\ToDo\Add.js(82):        
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesAdd.js(32):            
	\DeskPlusEIP\Opinion\Js\Admin\Content\QuesMod.js(57):            
	\DeskPlusEIP\RM\Js\Content\Add.js(92):            
	\DeskPlusEIP\RM\Js\Content\Mod.js(92):            
	\DeskPlusEIP\Community\Js\Site\Admin\Function\Main\Add.js(28):            
	\DeskPlusEIP\Community\Js\Site\Apps\Board\Add.js(36):            
	\DeskPlusEIP\Community\Js\Site\Apps\Board\Mod.js(30):            
	\DeskPlusEIP\Community\Js\Site\Apps\Board\Reply.js(39):            
	\DeskPlusEIP\Community\Js\Site\Apps\Schedule\Add.js(93):            
	\DeskPlusEIP\Community\Js\Site\Apps\Schedule\Mod.js(95):            
	\DeskPlusEIP\Community\Js\Site\Main\Mail\Default.js(71):            
	\DeskPlusEIP\PMS\Js\Board\Add.js(29):            
	\DeskPlusEIP\PMS\Js\Board\Mod.js(30):            
	\DeskPlusEIP\PMS\Js\Board\Reply.js(29):            
	\DeskPlusEIP\PMS\Js\Schedule\Add.js(68):            
	\DeskPlusEIP\PMS\Js\Schedule\Mod.js(90):            
	\DeskPlusEIP\PMS\Js\Template\Add.js(23):            
	\DeskPlusEIP\PMS\Js\Template\Mod.js(23):            
	\DeskPlusEIP\PMS\Js\Unit\Add.js(93):            
	\DeskPlusEIP\PMS\Js\Unit\Mod.js(76): 


: 2017-02-01 By withsuki  ----------------------------------------------------------
[전자결재] 문서함 트리버그 수정  (개선업무)
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Folder\Menu\Tree.js

: 2017-01-23 By withsuki   ----------------------------------------------------------
[전자결재] 문서열람초기화 (개선업무)
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Resource\GlobalString\
	\DeskPlusEIP\Approval\Ws\Admin\Document\ViewDocProc.asmx
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs
	DP_APP_Seq_UpdViewingForce

: 2017-01-23 By maniaysw   ----------------------------------------------------------
[일정관리,자원관리,방문예약] 일간,주간,월간 탭 저장 (개선업무)
	테이블 DP_ACC_USERENV

	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironment.cs

일정관리 
[js]
	\DeskPlusEIP\Pim\Js\Schedule\ScheduleMain.js

[Procedure]
	DP_ACC_USERENV_SelPimTabMenu
	DP_ACC_USERENV_UpdPimTabMenu


[cs]
	\DeskPlusEIP\Pim\ScheduleMain.aspx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\ScheduleMain.asmx.cs

자원관리
[js]
	\DeskPlusEIP\RM\Js\Content\ListMain.js

[Procedure]
	DP_ACC_USERENV_SelRMTabMenu
	DP_ACC_USERENV_UpdRMTabMenu

[cs]
	\DeskPlusEIP\RM\Content\ListMain.aspx.cs
	\DeskPlusEIP\RM\Ws\Home\Main.asmx.cs

방문예약
[js]
	\DeskPlusEIP\Visit\Js\Content\ListMain.js

[Procedure]
	DP_ACC_USERENV_SelVisitTabMenu
	DP_ACC_USERENV_UpdVisitTabMenu

[cs]
	\DeskPlusEIP\Visit\Content\ListMain.aspx.cs
	\DeskPlusEIP\Visit\Ws\Content\Mod.asmx.cs


: 2017-01-20 By 이지원  ----------------------------------------------------------
[일정관리] 공휴일, 지정휴일관리 개발 (내부개발)
	* 공휴일관리
	--소스
	--리스트
	\DeskPlusEIP\Pim\Admin\HolidayR\List.aspx
	\DeskPlusEIP\Pim\Admin\HolidayR\List.aspx.cs
	\DeskPlusEIP\Pim\Js\Admin\HolidayR\List.js
	\DeskPlusEIP\Pim\Jt\Admin\HolidayR\JTList.htm
	\DeskPlusEIP\Pim\Ws\Admin\HolidayR\List.asmx.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BE\HolidayRInfo.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\HolidayR.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.DAL\HolidayR.cs	
	--저장
	\DeskPlusEIP\Pim\Admin\HolidayR\Add.aspx
	\DeskPlusEIP\Pim\Admin\HolidayR\Add.aspx.cs
	\DeskPlusEIP\Pim\Js\Admin\HolidayR\Add.js
	\DeskPlusEIP\Pim\Ws\Admin\HolidayR\Add.asmx.cs
	\DeskPlusEIP\Pim\Admin\HolidayR\Mod.aspx
	\DeskPlusEIP\Pim\Admin\HolidayR\Mod.aspx.cs
	\DeskPlusEIP\Pim\Js\Admin\HolidayR\Mod.js	
	\DeskPlusEIP\Pim\Ws\Admin\HolidayR\Mod.asmx.cs

	--DB
	--테이블
	DP_PIM_Holiday

	-프로시져
	DP_PIM_HolidayR_List	
	DP_PIM_HolidayR_CheckCnt	
	DP_PIM_HolidayR_Ins
	DP_PIM_HolidayR_SelByHoliDayID
	DP_PIM_HolidayR_CheckCnt_SelByHoliDayID
	DP_PIM_HolidayR_Upd	

	*휴일관리
	--소스
	\DeskPlusEIP\Pim\Admin\HolidayD\List.aspx
	\DeskPlusEIP\Pim\Admin\HolidayD\List.aspx.cs
	\DeskPlusEIP\Pim\Js\Admin\HolidayD\List.js
	\DeskPlusEIP\Pim\Jt\Admin\HolidayD\JTList.htm
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BE\HolidayDInfo.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\HolidayD.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.DAL\HolidayD.cs	
	--저장
	\DeskPlusEIP\Pim\Admin\HolidayD\Add.aspx
	\DeskPlusEIP\Pim\Admin\HolidayD\Add.aspx.cs
	\DeskPlusEIP\Pim\Js\Admin\HolidayD\Add.js
	\DeskPlusEIP\Pim\Ws\Admin\HolidayD\Add.asmx.cs
	\DeskPlusEIP\Pim\Admin\HolidayD\Mod.aspx
	\DeskPlusEIP\Pim\Admin\HolidayD\Mod.aspx.cs
	\DeskPlusEIP\Pim\Js\Admin\HolidayD\Mod.js	
	\DeskPlusEIP\Pim\Ws\Admin\HolidayD\Mod.asmx.cs

	--DB
	-프로시져
	DP_PIM_HolidayD_List	
	DP_PIM_HolidayD_CheckCnt	
	DP_PIM_HolidayD_Ins
	DP_PIM_HolidayD_SelByHoliDayID
	DP_PIM_HolidayD_CheckCnt_SelByHoliDayID
	DP_PIM_HolidayD_Upd	
	
	*공휴일보기	
	--소스
	--캐쉬할당
	\DeskPlusEIP\Site\Auth\Default.aspx
	\DeskPlusEIP\Site\Auth\Default1.aspx
	\DeskPlusEIP\Site\Auth\Default2.aspx
	\DeskPlusEIP\Site\Auth\Default3.aspx
	\DeskPlusEIP\Site\Auth\Default4.aspx
	\DeskPlusEIP\Site\Auth\Default5.aspx
	\DeskPlusEIP\Site\Auth\Default6.aspx
	\DeskPlusEIP\Site\Auth\Default7.aspx
	\DeskPlusEIP\Site\Auth\Default8.aspx
	\DeskPlusEIP\Site\Auth\Default9.aspx
	\DeskPlusEIP\Site\Auth\Default10.aspx
	--전자결재
	\DeskPlusEIP\Common\Calendar\Calendar.ascx.cs
	\DeskPlusEIP\Common\Calendar\CalendarDualDialog.aspx
	\DeskPlusEIP\Common\Calendar\CalendarDualDialog.aspx.cs
	--포틀릿
	\DeskPlusEIP\Portlet\Package\MyPim.ascx.cs
	--일정관리
	\DeskPlusEIP\Pim\Menu\PimMenu.aspx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx.cs
	\DeskPlusEIP\Pim\Js\Schedule\ListDay.js
	\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
	\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js
	\DeskPlusEIP\Pim\Js\FullCalendar\fullcalendar.js
	--자원관리
	\DeskPlusEIP\RM\Home\Main.aspx.cs
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx.cs

	--커뮤니티
	\DeskPlusEIP\Community\Ws\Site\Apps\Schedule\List.asmx.cs
	--방문예약
	\DeskPlusEIP\Visit\Content\Tree.aspx.cs
	\DeskPlusEIP\Visit\Ws\Content\ListMonth.asmx.cs
	--프로젝트관리
	\DeskPlusEIP\PMS\Schedule\List.aspx.cs
	--연동문서

	--DB
	-프로시져
	DP_PIM_HolidayR_SelByYear


: 2017-01-19 By 이지원   ----------------------------------------------------------
[일정관리] 공휴일관리 -> 기념일관리 변경 (내부개발)
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Menu\Menu.aspx
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\List.aspx
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\Add.aspx
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\AddN.aspx
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\Mod.aspx
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\ModN.aspx
	\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\Mod.aspx.cs

: 2017-01-18 By freeman  ----------------------------------------------------------
[통계] 관리자 서버 성능 정보 및 포틀릿 beta

\DeskPlusSolution\DeskPlusEIP\Html\Js\Chart\chartJs\Chart.min.js
\DeskPlusSolution\DeskPlusEIP\EIP\Package\Main.aspx

\DeskPlusSolution\DeskPlusEIP\Statistics\Content\PerformMon\List.aspx
\DeskPlusSolution\DeskPlusEIP\Statistics\Js\Content\PerformMon\List.js
\DeskPlusSolution\DeskPlusEIP\Statistics\Ws\Content\PerformMon\List.asmx

\DeskPlusSolution\Projects\StatisticsProject\Zeospace.DeskPlus.Statistics.BE\PerformMonInfo.cs
\DeskPlusSolution\Projects\StatisticsProject\Zeospace.DeskPlus.Statistics.DAL\PerformMon.cs


\DeskPlusSolution\DeskPlusEIP\Portlet\Js\Package\STA\PerformMon.js
\DeskPlusSolution\DeskPlusEIP\Portlet\Js\Package\STA\PerformMonDisk.js
\DeskPlusSolution\DeskPlusEIP\Portlet\Package\STA\PerformMon.ascx
\DeskPlusSolution\DeskPlusEIP\Portlet\Package\STA\PerformMonDisk.ascx

\DeskPlusSolution\DeskPlusEIP\EIP\App_Data\AdminMenu\*.xml


: 2017-01-18 By withsuki  ----------------------------------------------------------
[전자결재] SMS알람발송 시, 대상자 번호가 없는경우, 에러처리
	\DeskPlusSolution\DeskPlusEIP\Approval\Ws\Preference\Proxy\List.asmx.cs
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

: 2017-01-17 By famo23  ----------------------------------------------------------
[전자결재] 의견추가할때 viewing 체크버그수정 (4.5.1버전에 적용) 
	DP_APP_Seq_SelViewingEtc

: 2017-01-10 By 이지원 ----------------------------------------------------------
[전자결재] 진행중인문서 합의 결재 없을경우 합의추가 (선진) (4.5.1버전에 적용)
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

: 2017-01-09 By 이지원 ---------------------------------------------------------
[일정관리] 오늘의 할일 수정시 본문 이미지경로 설정 (4.5.1버전에 적용)
	\DeskPlusEIP\Pim\ToDo\Add.aspx.cs

: 2017-01-09 By 이지원 ----------------------------------------------------------
[사용자] 사용자등록 중복 클릭 방지 (선진) 
	\DeskPlusEIP\Account\Admin\User\Add.aspx
	\DeskPlusEIP\Account\Js\Admin\User\Add.js

: 2017-01-05 By freeman  ------------------------------------------------------------
[공통] 문서 모드를 IE=EmulateIE8 에서 IE=edge 로 변경 (HTML5 지원)  
    - XP 및 IE8 이하에서 문제 생길수 있음. 

: 2017-01-05 By famo23  ------------------------------------------------------------
[업무협업] 전달현황 수신자이름이 문서확인전에 나오게 수정 (4.5.1버전에 적용)
	DP_TAS_Forward_Ins

: 2017-01-04 By famo23  ------------------------------------------------------------
[모바일관리] 알람세팅 세분화 개발 (뉴트리바이오텍 요구사항) 
	\DeskPlusEIP\MDM\Config\Default.aspx
	\DeskPlusEIP\MDM\Config\Default.aspx.cs
	\DeskPlusEIP\MDM\Config\Setting.aspx
	\DeskPlusEIP\MDM\Config\Setting.aspx.cs
	\DeskPlusEIP\MDM\Js\Config\Default.js
	\DeskPlusEIP\MDM\Js\Config\Setting.js
	\DeskPlusEIP\MDM\Ws\Config\Default.asmx.cs
	\DeskPlusEIP\MDM\Ws\Config\Setting.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\EnvInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\Env.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\List.cs
	DP_COM_Company_Ins
	DP_MDM_Env_Ins
	DP_MDM_List_UpdAll
	DP_MDM_List_Upd
	DP_MDM_Env 테이블 필드변경**
    초기값 변경
    INSERT INTO [DP_MDM_Env]
           ([CompanyID],[MailAlarm],[EtcAlarm]
           ,[LoginType],[AccessRight],[Approve]
           ,[WriteUserID],[WriteUserName],[WriteDate],[ModifyUserID],[ModifyUserName],[ModifyDate])
     VALUES
           (1,'Y','Approval,Board,Pim,TASK,Report,Community,ResourceManager,SNS,MDM,'
           ,2,0,1
           ,1,'deskplus',GETDATE(),1,'deskplus',GETDATE())



: 2017-01-04 By 방영환   ------------------------------------------------------------
[전자결재] 결재의견 추가시 알람 발송 (뉴트리바이오텍 요구사항)
	**소스 수정
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs
	\DeskPlusEIP\Approval\Status\Common\View.aspx.cs

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

	**프로시저 추가
	DP_APP_Seq_SelAlarmOpinionAddAll

: 2016-12-30 By famo23   ------------------------------------------------------------
[일정관리] 댓글 작성시 작성자한테는 알람 안 오게 수정 (사장님 지시사항)
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\ScheduleTalk.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\ToDoTalk.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\ScheduleReply.cs
	\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\ToDoReply.cs



### DeskPlusSP v4.5.1 (2016.12.23) #########################################################

: 2016-12-30 By famo23 ------------------------------------------------------------
[전자결재] 부서명에 & 기호 있으면 문서작성창 에러수정
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeLegacy.js
	\DeskPlusEIP\Approval\Js\Preference\FavoriteForm\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\List.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\List.js
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs

: 2016-12-09 by withsuki -----------------------------------------------------------
[전자결재] 결재 작성 시, 처리중 이미지 수정
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_Approval.css
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\Icon\loadingB.gif

: 2016-12-08 by withsuki -----------------------------------------------------------
[전자결재] 홈화면 문서보기에서 [PDF다운로드] 추가
	\DeskPlusEIP\Approval\ViewTot.aspx
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js

: 2016-12-02 by 이지원 -------------------------------------------------------------
[게시판] 즐겨찾기 추가후 통합게시판으로 처리되는 부분 수정
	\DeskPlusEIP\Board\Category\Tree.aspx.cs

: 2016-12-02 by famo23 -------------------------------------------------------------
[게시판] 홈화면에서 마지막 목록을 첫번째 목록이라고 보여주는 리소스 버그수정
	LastList
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx

: 2016-12-01 by famo23 -------------------------------------------------------------
[사용자관리] 날짜검색 버그수정
	\DeskPlusEIP\Account\Admin\User\List.aspx.cs

: 2016-11-30 by famo23 -------------------------------------------------------------
[자원관리]주간리스트 최소사이즈 지정
	\DeskPlusEIP\RM\Content\ListWeek.aspx
	\DeskPlusEIP\RM\Content\WholeWeek.aspx

: 2016-11-29 by freeman -------------------------------------------------------------
[포탈] 시작탭 설정 기능 버그
	\DeskPlusSolution\DeskPlusEIP\EIP\Package\Menu.aspx.cs

: 2016-11-29 by famo23 -------------------------------------------------------------
[게시판] pdf 파일 다운로드 버그수정
	\DeskPlusEIP\Common\PDF\PDFDownLoad.aspx.cs

[자원관리] 자원의 첨부파일 등록시 버그수정
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Attach.cs

[자원관리] 예약자원 합쳐 보는 주간화면에서 버그수정(예약날짜가 3일이상일때 중간날짜가 안 보이는 현상)
	\DeskPlusEIP\RM\Content\WholeWeek.aspx.cs

: 2016-11-25 by ellepooky -------------------------------------------------------------
[일정관리] 인쇄버튼 숨기기
	\DeskPlusEIP\Pim\Schedule\ScheduleMain.aspx

: 2016-11-25 by famo23 -------------------------------------------------------------
[통계이력관리] 다운로드통계 엑셀다운로드 안되는 버그수정
	\DeskPlusEIP\Statistics\Content\Attach\ExcelDown.aspx.cs

[주소록] 즐겨찾기 폴더추가 수정
	\DeskPlusEIP\Address\Js\Folder\Tree.js

[통계이력관리] 다운로드통계 모듈별 사용량 통계 안보이게 처리
	\DeskPlusEIP\Statistics\Content\Attach\List.aspx

[통계이력관리] 접속통계 / 다운로드통계 엑셀다운로드 한글 깨지는 버그수정
	\DeskPlusEIP\Statistics\Content\Attach\ExcelDown.aspx.cs
	\DeskPlusEIP\Statistics\Content\Login\ExcelDown.aspx.cs

[통계이력관리] 다운로드통계 모듈별 통계 엑셀다운로드시 모듈명 영어로 나오는 버그수정
	\DeskPlusEIP\Statistics\Content\Attach\ExcelDown.aspx.cs

[통계이력관리] 접속통계 리스트랑 카운트랑 맞지 않는 버그수정
	DP_STA_AccessLog_SelPageCNT
	DP_STA_AccessLogF_SelPageCNT
	DP_STA_AccessLogM_SelPageCNT
	DP_STA_AccessLogMF_SelPageCNT

[통계이력관리] 다운로드통계 검색시에 카운트잘못 나오는 버그수정
	\DeskPlusEIP\Statistics\Js\Content\Attach\List.js

: 2016-11-24 by withsuki -------------------------------------------------------------
[주소록] 폴더추가/수정/삭제 후, 트리 덮힘
	\DeskPlusEIP\Address\Folder\Add.aspx
	\DeskPlusEIP\Address\Folder\Tree.aspx
	\DeskPlusEIP\Address\Js\Folder\Add.js
	\DeskPlusEIP\Address\Js\Folder\Tree.js

: 2016-11-24 by famo23 -------------------------------------------------------------
[코드관리] 검색키값 수정
	\DeskPlusEIP\EIP\Admin\Code\List.aspx.cs

[사용자관리] 리스트에서 부서변경시에 리스트수정
	\DeskPlusEIP\Account\Js\Admin\User\Move.js

[사용자관리] 인사기록정보 버튼 안 보이게 처리
	\DeskPlusEIP\Account\Admin\User\Extend\CareerInfo.aspx
	\DeskPlusEIP\Account\Admin\User\Extend\EduInfo.aspx
	\DeskPlusEIP\Account\Admin\User\Extend\FamilyInfo.aspx
	\DeskPlusEIP\Account\Admin\User\Extend\RewardInfo.aspx
	\DeskPlusEIP\Account\Admin\User\Extend\ScholarInfo.aspx

[사용자관리] 부서정보 버튼 안 보이게 처리
	\DeskPlusEIP\Account\Admin\User\UserDept\List.aspx

[첨부파일사용이력] 파일명 a태그제거
	\DeskPlusEIP\EIP\Jt\Admin\Static\Attach\JTList.htm

: 2016-11-24 by 이지원 -------------------------------------------------------------
[게시판] 즐겨찾기추가후 리스트이동
	\DeskPlusEIP\Board\Content\FavoriteAdd.aspx
	\DeskPlusEIP\Board\Js\Category\Tree.js

: 2016-11-23 by famo23 -------------------------------------------------------------
[포틀릿관리] 폴더명 수정시에 다국어명에 상위그룹명이 나오는 버그수정
	\DeskPlusEIP\EIP\Admin\Portlet\AddFolder.aspx.cs

[포틀릿관리] 폴더 삭제시에 버그수정
	\DeskPlusEIP\EIP\Js\Admin\Portlet\AddFolder.js

: 2016-11-18 by withsuki -------------------------------------------------------------
[게시판] 즐겨찾기추가 이후, 좌측 메뉴 슬라이드가 완전히 열리지 않음
	\DeskPlusEIP\Board\Category\Tree.aspx

[업무일지] 댓글이 2번씩 등록됨
	\DeskPlusEIP\Report\Content\ShortReply.aspx

: 2016-11-17 by famo23 -------------------------------------------------------------
[관리자] 메뉴관리 상위메뉴변경 했을 시에 levels / path 경로가 수정 안되는 버그수정
	DP_EIP_Menu_UpdPID

: 2016-11-16 by withsuki -------------------------------------------------------------
[자원관리] 자원등록이 되지 않는 버그수정 (validation관련 js파일 누락)
	\DeskPlusEIP\RM\Admin\Content\Add.aspx


: 2016-11-16 by famo23 -------------------------------------------------------------
[주소록] 블로그형 공용주소록 하위그룹검색안되는 버그수정
	\DeskPlusEIP\Address\Content\ListBlog.aspx.cs

[주소록] 공용/개인주소록 추가시에 상위폴더명 잘못나오는버그
	\DeskPlusEIP\Address\Admin\Folder\Add.aspx.cs
	\DeskPlusEIP\Address\Folder\Add.aspx.cs

[주소록] 개인폴더상위폴더 추가안되는 버그
	DP_ADD_PrivateCat_Ins

: 2016-11-14 by famo23 -------------------------------------------------------------
[마이톡] 공유담벼락 검색수정
	DP_SNS_MyTalk_SelSharePageList
	DP_SNS_MyTalk_SelSharePageCnt

[마이톡] 공유담벼락에서 공개대상관리 버튼 클릭했을 시에 반응 없는 버그수정
	\DeskPlusEIP\SNS\Js\MyTalk\Content\ShareList.js

[마이톡] 비공개댓글이 보이는 버그수정
	\DeskPlusEIP\SNS\Js\MyTalk\Content\List.js
	\DeskPlusEIP\SNS\Js\MyTalk\Content\ShareList.js
	\DeskPlusEIP\SNS\Js\Common\MyTalkGroup.js

[마이톡] 공유에 본인 나오지 않게 수정
	DP_SNS_OpenList_SelOpenPageCnt
	DP_SNS_OpenList_SelOpenPageList
	DP_SNS_OpenList_SelPageCnt
	DP_SNS_OpenList_SelPageList


: 2016-11-10 by famo23 -------------------------------------------------------------
[웹폴더] 복사할때 복사한 사용자 정보 등록안되는 버그 수정
	\DeskPlusEIP\WebHard\Ws\Folder\CopyMove.asmx.cs

: 2016-11-07 by famo23 -------------------------------------------------------------
[업무협업]  t달받은 협업 / 업무협업통계권한 있는 사용자에게 답변기능추가
	DP_PT_TASK_SelForwardList
	DP_TAS_List_SelWriteCommentList
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTForward.htm
	\DeskPlusEIP\Portlet\Package\TabTaskMain.ascx
	\DeskPlusEIP\Portlet\Ws\Package\Task\List.asmx.cs
	\DeskPlusEIP\Task\Admin\Content\View.aspx.cs
	\DeskPlusEIP\Task\Common\CreateZipFile.aspx.cs
	\DeskPlusEIP\Task\Common\ModList.aspx.cs
	\DeskPlusEIP\Task\Common\ViewHistory.aspx.cs
	\DeskPlusEIP\Task\Common\ViewPopup.aspx.cs
	\DeskPlusEIP\Task\Common\ViewPopupTotal.aspx.cs
	\DeskPlusEIP\Task\Content\Forward\View.aspx
	\DeskPlusEIP\Task\Content\Forward\ViewN.aspx
	\DeskPlusEIP\Task\Content\Forward\View.aspx.cs
	\DeskPlusEIP\Task\Content\Receive\View.aspx.cs
	\DeskPlusEIP\Task\Content\Reference\View.aspx.cs
	\DeskPlusEIP\Task\Content\Request\View.aspx.cs
	\DeskPlusEIP\Task\Content\Total\View.aspx
	\DeskPlusEIP\Task\Content\Total\View.aspx.cs
	\DeskPlusEIP\Task\Content\Total\ViewN.aspx
	\DeskPlusEIP\Task\Home\View.aspx.cs
	\DeskPlusEIP\Task\Js\Common\ModList.js
	\DeskPlusEIP\Task\Js\Content\Forward\List.js
	\DeskPlusEIP\Task\Js\Content\Forward\View.js
	\DeskPlusEIP\Task\Js\Content\Total\List.js
	\DeskPlusEIP\Task\Js\Content\Total\View.js
	\DeskPlusEIP\Task\Js\Home\View.js
	\DeskPlusEIP\Task\Ws\Content\Forward\View.asmx
	\DeskPlusEIP\Task\Ws\Content\Total\View.asmx



: 2016-11-07 by ellepooky -------------------------------------------------------------
[검색엔진] DB검색으로 변경 및 관리자 검색관리 주석처리
	\DeskPlusEIP\EIP\Js\Package\Menu.js
	\DeskPlusEIP\EIP\App_Data\AdminMenu\ko.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\en.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\fr.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\ja.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\zh-cn.xml
	\DeskPlusEIP\EIP\App_Data\AdminMenu\zh-tw.xml

: 2016-11-04 by famo23 -------------------------------------------------------------
[휴가관리] 휴가분류 2차분류 추가 / 업무대행자 조직도처리
	휴가원양식수정
	ETC 리소스추가
	DP_HOLI_Cat 수정
	QRY_HOLI_Cat_Sel
	DP_HOLI_Used_SelPageList
	DP_HOLI_Used_SelAllList
	DP_ETC_HoliAdminView_SelPageList
	\DeskPlusEIP\Legacy\HoliDay.aspx
	\DeskPlusEIP\Legacy\HoliDay_Complete.aspx
	\DeskPlusEIP\ETC\Holiday\GetUserInfo.aspx
	\DeskPlusEIP\ETC\Js\Holiday\GetUserInfo.js

: 2016-11-02 by famo23 -------------------------------------------------------------
[방문예약]파일등록, 알람 에러처리
	\DeskPlusEIP\App_Data\VISIT.xml
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\FileHelper.cs

: 2016-10-07 by 이지원 -----------------------------------------------------------
  [사용자정보] 저장시 캐쉬 정보 Insert 변경
	-소스
	\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs
	\EIPProject\Zeospace.DeskPlus.EIP.BLL\Menu.cs

	-프로시져
	DP_EIP_Menu_SelGetUserAllOne

: 2016-10-06 By 이지원 ----------------------------------------------------------
[전자결재] 상신시 결재자 정보 다른때 에러처리
	--소스
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js

: 2016-08-18 by famo23 -------------------------------------------------------------
[업무협업] 수신자, 참조자에 퇴사자가 있을 시에 수정, 접수, 임시저장, 변경저장할때 null값들어가는 버그수정
	QRY_TAS_DraftTO_Sel
	QRY_TAS_DraftCC_Sel
	QRY_TAS_ListTO_SelActive
	QRY_TAS_ListCC_SelActive
	DP_TAS_ListTO_Ins
	DP_TAS_ListCC_Ins
	\DeskPlusEIP\Task\Content\Receive\AddAccept.aspx.cs
	\DeskPlusEIP\Task\Content\Request\Mod.aspx.cs
	\Projects\TaskProject\Zeospace.DeskPlus.Task.DAL\ListCC.cs
	\Projects\TaskProject\Zeospace.DeskPlus.Task.DAL\ListTO.cs

: 2016-08-11 by 이지원 -------------------------------------------------------------
[주소록] 중요표시 버그
	-소스
	\DeskPlusEIP\Address\Ws\Admin\Content\ListBlog.asmx.cs
	\DeskPlusEIP\Address\Ws\Content\List.asmx.cs	
	\DeskPlusEIP\Address\Ws\Content\ListBlog.asmx.cs

	\Projects\AddressProject\Zeospace.DeskPlus.Address.BLL\List.cs	

	-프로시져
	DP_ADD_SelPageList
	DP_ADD_SelPageListWithSub

: 2016-07-26 by famo23 -------------------------------------------------------------
[모바일관리] 요청일, 승인일 짤려서 보이는 부분수정
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx

: 2016-07-22 by freeman -------------------------------------------------------------
[전자결재] 크롬에서 자주지정하는 결재자 팝업 않되는 버그	
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js  popUpFavList_onclick  

: 2016-07-18 by famo23 -------------------------------------------------------------
[업무협업] 리스트페이지에서 소분류 선택 제대로 안되는 버그	
	\DeskPlusEIP\Task\Admin\Content\List.aspx.cs
	\DeskPlusEIP\Task\Common\BaseTaskList.aspx.cs
	\DeskPlusEIP\Task\Content\Forward\List.aspx.cs
	\DeskPlusEIP\Task\Content\Receive\List.aspx.cs
	\DeskPlusEIP\Task\Content\Reference\List.aspx.cs
	\DeskPlusEIP\Task\Content\Request\List.aspx.cs
	\DeskPlusEIP\Task\Content\Total\List.aspx.cs
	\DeskPlusEIP\Task\Draft\AddList.aspx.cs
	\DeskPlusEIP\Task\Draft\List.aspx.cs      
	\DeskPlusEIP\Task\ImpoUser\List.aspx.cs  

[업무협업] 임시저장문서 디자인수정
	\DeskPlusEIP\Task\Draft\AddList.aspx

: 2016-07-06 by withsuki -------------------------------------------------------------
[전자결재] 결재대상 포틀릿 수정 (클릭 시, 바로 결재페이지로 이동)

	DP_APP_DocProcess_SelPageListApprove

	\DeskPlusEIP\Portlet\Jt\Package\Approval\JTTabListA.htm
	\DeskPlusEIP\Portlet\Jt\Package\Approval\JTListA.htm
	\DeskPlusEIP\Portlet\Package\Approval\JTTabListA.ascx
	\DeskPlusEIP\Portlet\Package\Approval\JTListA.ascx
	\DeskPlusEIP\Portlet\Ws\Package\Approval\Tab.asmx.cs


: 2016-07-06 by famo23 -------------------------------------------------------------
[업무협업] 홈화면 전체업무에 완료된 업무 추가 
	리소스 추가 CompleteList ComNoContent
	\DeskPlusEIP\Task\Jt\Home\JTAdminCom.htm
	\DeskPlusEIP\Task\Home\AdminMain.aspx
	\DeskPlusEIP\Task\Home\AdminMain.aspx.cs
	\DeskPlusEIP\Task\Js\Home\AdminMain.js	
	\DeskPlusEIP\Task\Home\View.aspx
	\DeskPlusEIP\Task\Home\View.aspx.cs
	\DeskPlusEIP\Task\Home\ViewN.aspx
	\DeskPlusEIP\Task\Js\Home\View.js
	\DeskPlusEIP\Task\Common\AddComplete.aspx
	\DeskPlusEIP\Task\Common\AddComplete.aspx.cs
	\DeskPlusEIP\Task\Common\AddCompleteN.aspx
	\DeskPlusEIP\Task\Js\Common\AddComplete.js
	\DeskPlusEIP\Task\Common\ModList.aspx      
	\DeskPlusEIP\Task\Common\ModList.aspx.cs
	\DeskPlusEIP\Task\Common\ModListN.aspx    
	\DeskPlusEIP\Task\Js\Common\ModList.js
	\DeskPlusEIP\Task\Content\Receive\AddAccept.aspx
	\DeskPlusEIP\Task\Content\Receive\AddAccept.aspx.cs
	\DeskPlusEIP\Task\Content\Receive\AddAcceptN.aspx
	\DeskPlusEIP\Task\Js\Content\Receive\AddAccept.js
	\DeskPlusEIP\Task\Content\Request\Mod.aspx
	\DeskPlusEIP\Task\Content\Request\Mod.aspx.cs
	\DeskPlusEIP\Task\Content\Request\ModN.aspx    
	\DeskPlusEIP\Task\Js\Content\Request\Mod.js

[업무협업] 요청자가 수신자나 참조자에도 있을때 답변 알람이 중복으로 오는 부분 수정
	\TaskProject\Zeospace.DeskPlus.Task.BLL\Alarm.cs

[업무협업] 메신저 알람창에 답변하기 추가
	\DeskPlusEIP\App_Data\TASK.xml
	\TaskProject\Zeospace.DeskPlus.Task.BLL\Alarm.cs
	\DeskPlusEIP\Task\Common\ViewPopupTotal.aspx
	\DeskPlusEIP\Task\Common\ViewPopupTotal.aspx.cs
	\DeskPlusEIP\Task\Common\ViewPopupTotalN.aspx

[업무협업] 알람온 협업 삭제되었을때 표시 수정
	리소스 추가 DelCont
	\DeskPlusEIP\Task\Common\ViewPopupTotal.aspx.cs

: 2016-06-28 by famo23 -------------------------------------------------------------
[업무협업] 참조에 부서 2개 이상 지정했을 경우 알람이 오지 않는 버그수정
	QRY_TAS_ListTO_SelAlarm
	QRY_TAS_ListTO_SelAlarmEtc

: 2016-06-22 by freeman -------------------------------------------------------------
[CSS] DeskPlusEIPBase에서 Import 방식에서 통합 하는 방식으로 변경 및 압축 함

  Design\C\Css\DeskPlusEIPBase.css   통합 및 압축
  Design\C\Css\DeskPlusEIPBase_.css  원본


: 2016-06-22 by freeman -------------------------------------------------------------
[위치이동] 메인 공지(팝업공지게시물) 위치를 Portlet 하위로 이동 

  /DeskPlusEIP/BoardMainPopup.aspx  ===> /DeskPlusEIP/Portlet/BoardPopup/BoardMainPopup.aspx
  /DeskPlusEIP/BoardPopup.aspx  ===> /DeskPlusEIP/Portlet/BoardPopup/BoardPopup.aspx

  \DeskPlusEIP\EIP\Package\Main.aspx.cs   
  \DeskPlusEIP\Portlet\Js\BoardPopup\BoardMainPopup.js
  \DeskPlusEIP\Portlet\Js\BoardPopup\BoardPopUp.js


: 2016-06-03 by withsuki -------------------------------------------------------------
[전자결재] 자주지정하는 결재자 기능추가 (부서 지정가능)

	DP_APP_FavoriteApp 필드추가 (IsDept=G+M / IsSub=1+0) : 기존 데이타 업데이트필요(IsDept=M, IsSub=0)
	
	DP_ACC_Dept_Del
	DP_ACC_Dept_Upd
	DP_APP_FavoriteApp_Del
	DP_APP_FavoriteApp_DelModifyList
	DP_APP_FavoriteApp_Ins
	DP_APP_FavoriteApp_SelByUserID
	DP_APP_FavoriteApp_SelList
	DP_APP_FavoriteApp_SelListCnt
	DP_APP_FavoriteApp_SelModifyList
	DP_APP_FavoriteApp_SelModifyListCnt
	DP_APP_FavoriteApp_Upd
	DP_APP_FavoriteAppNet_Del
	DP_APP_FavoriteAppNet_Ins
	DP_APP_FavoriteAppNet_Sel

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FavoriteApproval.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\FavoriteApprovalInfo.cs
	\DeskPlusEIP\Approval\Ws\Preference\Favorite\List.asmx
	\DeskPlusEIP\Approval\Ws\FormFlow\Common\Common.asmx
	\DeskPlusEIP\Approval\Resource\GlobalString\
	\DeskPlusEIP\Approval\Preference\Favorite\List.aspx.cs
	\DeskPlusEIP\Approval\Jt\Preference\Favorite\JTList.htm
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\Preference\Favorite\List.js


: 2016-06-03 by 이지원 -------------------------------------------------------------
[커뮤니티] 커뮤니티명 특수문자 처리
	\DeskPlusEIP\Community\Ws\Main\Menu.asmx.cs
	\DeskPlusEIP\Community\Ws\Main\Content.asmx.cs
	\DeskPlusEIP\Community\Site\Main\Menu.aspx.cs

: 2016-05-30 by 이지원 -------------------------------------------------------------
[전자결재] 결재문서함-개인별문서함 Insert 수정 모바일
	DPM_APP_DocProcess_UpdConclude

: 2016-05-30 by kkzz52 -----------------------------------------------------------
[전자결재] - 자사폴더 권한지정 시 하위폴더 옵션 추가
	\DeskPlusEIP\Approval\Js\Admin\Folder\List.js
	\DeskPlusEIP\Approval\Common\Popup\GetDept.aspx
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDept.js
	\DeskPlusEIP\Approval\Ws\Admin\Folder\List.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\FoldersAuthority.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Folder.cs

	리소스 MSG_ApplySubFolder
	
	insert dp_query (category,qryid,qryvalue,owner,memo,regdate)
	values ('APPROVAL','QRY_APP_Folder_SelSubTreeList'
	,'SELECT * FROM DP_APP_Folder WHERE CompanyID = @CompanyID AND FolderID IN ((SELECT id FROM ufn_GetAppFolderChildtree(@CompanyID,@FolderID))) AND IsActive in ({0}) ORDER BY FolderOrder'
	,'김형석','하위폴더리스트 가져오기','20160530')

: 2016-05-13 by withsuki -------------------------------------------------------------
[인사] 인사관리 관리자 권한추가에 따른 소스수정 - 인사관리 모듈만 인사팀에게 권한부여하는 업체들을 위해 처리

	\DeskPlusEIP\Account\Base\BasePageAccountAdmin.cs

	* 권한정보 
		: account = 인사관리 관리자(사용자,부서,직급,직책)
		: accountExp = 사용자관리만 허용

: 2016-05-12 by freeman -------------------------------------------------------------
[공통] DPUpload 버전 업그레이드 : 특정 OS에서 우클릭 않된는 문제 ==> 기능 제거
	\DeskPlusEIP\Data\Cab\DPUpload.cab  -> 신규
	\DeskPlusEIP\Html\Js\IEActiveX\activexDPUploadEditor.js  : 버전 4,5,0,0
	\DeskPlusEIP\Html\Js\IEActiveX\activexDPUploadFileAttach.js  : 버전 4,5,0,0
	\DeskPlusEIP\Html\Js\IEActiveX\activexDPUploadTotalDownLoad.js  : 버전 4,5,0,0
	\DeskPlusEIP\Html\Js\IEActiveX\activexDPUploadWebFolder.js  : 버전 4,5,0,0

: 2016-05-04 by 이지원 -------------------------------------------------------------
[전자결재] 결재문서함-개인별문서함 Insert 수정
	DP_APP_DocProcess_UpdConclude

: 2016-04-21 by 방영환 -------------------------------------------------------------
[자원관리] 자원관리 View페이지 보안설정 Label로 변경
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Js\Content\View.js

: 2016-04-21 by 방영환 -------------------------------------------------------------
[자원관리] 댓글시 공유자에게 알람발송
	\DeskPlusEIP\RM\Content\ShortReply.aspx.cs
	\DeskPlusEIP\RM\Content\ShortReplyMod.aspx.cs

	\DeskPlusEIP\RM\Js\Content\ShortReply.js
	\DeskPlusEIP\RM\Js\Content\ShortReplyMod.js
	\DeskPlusEIP\RM\Js\Content\View.js

	\DeskPlusEIP\RM\WS\Content\ShortReply.asmx.cs
	\DeskPlusEIP\RM\WS\Content\ShortReplyMod.asmx.cs

	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\ShortReply.cs
	

: 2016-04-20 by 이지원 -------------------------------------------------------------
[전자결재] 휴가원 자원관리 연동수정
	\DeskPlusSolution\DeskPlusEIP\RM\Ws\Interface\Add.asmx.cs

: 2016-04-15 by withsuki -------------------------------------------------------------
[전자결재] 회람 메일발송 시, 결재의견 누락됨
	\DeskPlusEIP\Approval\Js\Folder\Dept\CirMemo.js


: 2016-04-14 by 이지원 -------------------------------------------------------------
[전자결재] 페이징 검색조건 유지 수정	
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\List.js				개인보관함-임시보관함
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\List.js				개인보관함-상신문서함

: 2016-04-08 by 방영환 -------------------------------------------------------------
[자원관리] 자원관리 보안, 공유자 추가, 홈화면 수정
	**소스 수정 
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserEnvironmentInfo.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironmentInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ReserveInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Reserve.cs
	\Projects\VisitProject\Zeospace.DeskPlus.Visit.BLL\Reserve.cs
	
	\DeskPlusEIP\RM\Default.aspx.cs
	
	\DeskPlusEIP\RM\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Content\AddN.aspx.cs
	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Content\ModN.aspx.cs
	\DeskPlusEIP\RM\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Content\WholeDay.cs
	\DeskPlusEIP\RM\Content\WholeWeek.cs

	\DeskPlusEIP\RM\Home\Main.aspx.cs
	\DeskPlusEIP\RM\Content\View.aspx.cs

	\DeskPlusEIP\RM\Js\Content\Add.js
	\DeskPlusEIP\RM\Js\Content\Mod.js
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Js\Home\Main.js
	\DeskPlusEIP\RM\Js\Folder\Tree.js

	\DeskPlusEIP\RM\Jt\Content\JTListDay.htm
	\DeskPlusEIP\RM\Jt\Content\JTListWeek.htm

	\DeskPlusEIP\RM\Ws\Content\ListDay.asmx
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx
	\DeskPlusEIP\RM\Ws\Content\ListWeek.asmx
	\DeskPlusEIP\RM\Ws\Content\View.asmx
	\DeskPlusEIP\RM\Ws\Content\Usable0List.asmx
	\DeskPlusEIP\RM\Ws\Content\Usable1List.asmx
	\DeskPlusEIP\RM\Ws\Content\Usable2List.asmx
	\DeskPlusEIP\RM\Ws\Content\WholeDay.asmx
	\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx		

	\DeskPlusEIP\RM\Ws\Home\Main.asmx
	\DeskPlusEIP\RM\Ws\Interface\Add.asmx

	\DeskPlusEIP\RM\App_Data\en.xml
	\DeskPlusEIP\RM\App_Data\fr.xml
	\DeskPlusEIP\RM\App_Data\ja.xml
	\DeskPlusEIP\RM\App_Data\ko.xml
	\DeskPlusEIP\RM\App_Data\zh-cn.xml
	\DeskPlusEIP\RM\App_Data\zh-tw.xml

	\DeskPlusEIP\App_Data\RESOURCEMANAGER.xml

	**소스 추가
	\DeskPlusEIP\RM\Content\AddShareUser.aspx.cs
	\DeskPlusEIP\RM\Js\Content\AddShareUser.js
	\DeskPlusEIP\RM\Jt\Content\JT\Home\MainList.htm

	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ShareUser.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\ShareUser.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\ShareUser.cs

	**리소스 추가
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
	
	SecuritySetting - 보안설정
	Security -보안
	Normal -일반
	ShareUser -공유자
	ShareUserAdd -공유자추가
	ObjePerson -대상자
	Msg_AddShareUser -공유자를 추가 해주세요.
	Msg_OverlapShareUser -이미 추가한 사용자(부서) 입니다.
	Msg_AddMe	-본인은 참가자로 추가 할 수 없습니다.

	**테이블 추가
	DP_RM_ShareUser

	**테이블 수정
	DP_RM_Reserve [컬럼 추가 IsGuard char(1)]
	DP_ACC_UserEnv [컬럼추가 RMHomeMenu nvarchar(50)]

	**프로시저 추가
	DP_RM_ShareUser_Sel
	DP_RM_ShareUser_De
	DP_ACC_UserEnv_UpdRMHomeMenu
	DP_RM_Reserve_SelMainList
	DP_RM_ShareUser_SelCntByID
	DP_RM_ShareUser_Upd
	DP_RM_Reserve_SelMainList

	**프로시저 수정
	DP_RM_Reserve_Ins
	DP_RM_Reserve_Upd
	DP_RM_Reserve_SelDayList
	DP_RM_Reserve_SelWholeList
	DP_RM_Reserve_SelDeptList

	**뷰 수정
	VW_RM_ReserveList
	VW_RM_Reserve

	**쿼리 추가
	insert into dp_query (category, qryid, qryvalue, owner, memo, regdate)
	values('RESOURCEMANAGER', 'QRY_RM_ShareUser_Sel', 'SELECT U.UserID, U.LoginID, U.MailID, U.MailDomain, U.MobilePhone FROM DP_ACC_User U WHERE CompanyID = @CompanyID AND UserID IN (SELECT UserID FROM DP_RM_ShareUser WHERE CompanyID = @CompanyID AND ReservationID = @ReservationID AND Kind = ''M'') UNION SELECT U.UserID, U.LoginID, U.MailID, U.MailDomain, U.MobilePhone FROM VW_UserInfo U WHERE CompanyID = @CompanyID AND DeptID IN ( SELECT s.DeptID FROM DP_RM_ShareUser AS SU CROSS APPLY dbo.ufn_GetSubTree(SU.DeptID, SU.CompanyID) s WHERE SU.CompanyID = @CompanyID AND SU.Kind = ''G'' AND SU.ReservationID = @ReservationID )', 
	'방영환', '자원 공유자 정보가져오기(알람대상자)', '20160407')

: 2016-03-31 by 이지원 -------------------------------------------------------------
[전자결재] 관리자수신참조기능 수정
	//관리자등록
	-소스
	\DeskPlusEIP\Approval\Admin\FormGeneration\ModSetting.aspx
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\ModSetting.js
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Forms.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\FormAppointerInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\FormAppointer.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FormAppointer.cs
	-테이블
	DP_APP_FormAppointer
	-프로시져
	DP_APP_FormAppointer_Ins
	DP_APP_FormAppointer_Del
	DP_APP_FormAppointer_SelByFormID

	//순서변경
	-소스
	\DeskPlusEIP\Approval\Admin\FormGeneration\ModUserSequence.aspx
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\ModUserSequence.js
	\DeskPlusEIP\Approval\Ws\Admin\FormGeneration\ModUserSequence.asmx
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\ModUserSequence.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ModUserSequence.cs

	-프로시져
	DP_APP_FormAppointer_ModSeq

	//문서상신
	-소스
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Ws\FormFlow\Basic\Add.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FormAppointer.cs
	-프로시져
	DP_APP_FormAppointer_SelByFormIDIsActive

: 2016-03-30 by maniaysw -------------------------------------------------------------
[일정관리] 일정 수정페이지 알람 정보 등록 후 다시 수정때 정보 안가져오는 문제 수정, 알람시간 미 선택시 validation 체크
	\DeskPlusEIP\Pim\Js\Schedule\Mod.js
	\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs


: 2016-03-25 by withsuki -------------------------------------------------------------
[인사정보/메신저] 썸네일(사진) 생성 시, 확장자와 사진속성정보가 달라서, 잘못된 파일이 생성됨.
	\DeskPlusEIP\Account\Admin\CSV\PictureSeal.aspx.cs
	\DeskPlusEIP\Account\Admin\User\Add.aspx.cs
	\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs
	\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs

:2016-03-25 by kkzz52--------------------------------------------------------
[전자결재] ZQ 중간단계 수신부서에게 반려 후 재상신시 결재망 처리
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js

	DP_APP_CounterPartUser_UpdDecide

: 2016-03-17 by 방영환 -------------------------------------------------------------
[전자결재] 전자결재 홈 검색
	**소스 수정
	\DeskPlusEIP\Approval\Home\Default.aspx.cs
	\DeskPlusEIP\Approval\Js\Home\Default.js	
	\DeskPlusEIP\Approval\Ws\Home\Default.asmx

	**소스 추가
	\DeskPlusEIP\Approval\Jt\Home\JTListDoc.htm

	**리소스 추가
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx

	**프로시저 수정
	DP_EIP_Search_Scheduler

	**프로시저 추가
	DP_APP_HOME_SelPageListDocAll
	DP_APP_HOME_SelPageCntDocAll

	**테이블 수정
	DP_EIP_Search_New 필드 추가
	 - Etc1, Etc2, Etc3, Etc4, Etc5 

: 2016-03-17 by famo23 -------------------------------------------------------------
[자원관리] 사이즈 짤리는 현상 수정
	\DeskPlusEIP\RM\Content\WholeMonth.aspx
	\DeskPlusEIP\RM\Content\WholeWeek.aspx

: 2016-03-16 by withsuki -------------------------------------------------------------
[업무톡] 팝업게시물 스크롤 미생성 및 댓글조회 시, 에러
	\DeskPlusEIP\SNS\Js\WorkTalk\Content\List.js
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.DAL\BoardTalk.cs

: 2016-03-16 by withsuki -----------------------------------------------------------
[포탈] IP대역 및 외부접속관리 / 메뉴접근확인 체크부분 버그수정 및 소스정리
   접속불가 에러 후, 해당 도메인 keyin하여 접속하면, 자동접속됨. (인증쿠키가 그대로 남아있음)
   /DeskPlusEIP/Site/Auth/Default.aspx.cs
   /DeskPlusEIP/Site/Auth/LoginOutSide.aspx.cs
   /DeskPlusEIP/Site/Auth/Default1.aspx.cs
   /DeskPlusEIP/Site/Auth/Default2.aspx.cs
   /DeskPlusEIP/Site/Auth/Default3.aspx.cs
   /DeskPlusEIP/Site/Auth/Default4.aspx.cs
   /DeskPlusEIP/Site/Auth/Default5.aspx.cs
   /DeskPlusEIP/Site/Auth/Default6.aspx.cs
   /DeskPlusEIP/Site/Auth/Default7.aspx.cs
   /DeskPlusEIP/Site/Auth/Default8.aspx.cs
   /DeskPlusEIP/Site/Auth/Default9.aspx.cs
   /DeskPlusEIP/Site/Auth/Default10.aspx.cs

: 2016-03-15 by withsuki -----------------------------------------------------------
[전자결재] 결재현황수정
	\DeskPlusEIP\Approval\Js\Status\Common\GetDocFlow.js
	\DeskPlusEIP\Approval\Status\Common\GetDocFlow.aspx
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\Approval\
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_AppStatus.css
	리소스

: 2016-03-10 by withsuki -----------------------------------------------------------
[전자결재] 반려버그 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\Approval\Js\Status\ListReject.js

	DP_APP_Seq_SelByReject
	DP_APP_DocProcess_SelPageCntReject
	DP_APP_DocProcess_SelPageListReject

	수신반려문서 카테고리 삭제 : DELETE FROM [dbo].[DP_APP_FolderStatus] where folderstatusid='ReceiveReject'

: 2016-03-02 by withsuki -----------------------------------------------------------
[일정관리]
	월별보기에서 공유자의 할일이 보이지 않음 (= 해당 날짜의 본인 할일이 있는 경우만 보여짐)

	DP_PIM_TODO_SEL_SHARE_CNT 수정 (부서공유 시, 하위부서 포함되도록)


: 2016-02-23 by famo23 -------------------------------------------------------------
[주소록] 0-9 검색 스타일 변경 안되는 버그수정
	\DeskPlusEIP\Address\Js\Admin\Content\ListBlog.js
	\DeskPlusEIP\Address\Js\Content\List.js
	\DeskPlusEIP\Address\Js\Content\ListBlog.js
	\DeskPlusEIP\Address\Js\Content\ListBlogImpo.js
	\DeskPlusEIP\Address\Js\Content\ListImpo.js

: 2016-02-22 by withsuki -----------------------------------------------------------
[전자결재] 크롬에서 인쇄 불가.
	\DeskPlusEIP\Approval\js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\js\Admin\Transfer\View.js
	\DeskPlusEIP\Approval\js\Admin\Transfer\ViewCir.js
	\DeskPlusEIP\Approval\js\Common\Popup\PreView.js
	\DeskPlusEIP\Approval\js\Common\Popup\PreView.js
	\DeskPlusEIP\Approval\js\Common\Popup\View.js
	\DeskPlusEIP\Approval\js\Common\Popup\ViewCir.js
	\DeskPlusEIP\Approval\js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\js\Disuse\View.js
	\DeskPlusEIP\Approval\js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\js\Folder\Common\View.js
	\DeskPlusEIP\Approval\js\Folder\Common\View.js
	\DeskPlusEIP\Approval\js\Folder\Common\View.js
	\DeskPlusEIP\Approval\js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\js\Folder\Common\ViewHistory.js
	\DeskPlusEIP\Approval\js\Folder\Enforce\Send.js
	\DeskPlusEIP\Approval\js\Folder\Enforce\Send.js
	\DeskPlusEIP\Approval\js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\js\Process\Confirm\Approve.js
	\DeskPlusEIP\Approval\js\Process\Confirm\ApprovePopup.js
	\DeskPlusEIP\Approval\js\Status\Common\View.js

: 2016-02-19 by withsuki -------------------------------------------------------------
[전자결재] 관리자 결재문서 관리에서 비활성화된 폴더가져오지 않음
	\DeskPlusEIP\Approval\Common\FormFlow\FormTreeScript.ascx.cs
	\DeskPlusEIP\Approval\Common\FormFlow\GetFormTreeScript.aspx.cs

[전자결재] 자주 사용하는 양식 - 비활성화 폴더에 있는 양식리스트도 가져옴 (활성화 양식만 가져오도록 수정)
	DP_APP_FavoriteForm_SelPageList
	DP_APP_FavoriteForm_SelPageCnt


: 2016-02-19 by famo23 -------------------------------------------------------------
[전자결재] 반려문서 재상신 취소한 이후에 진행중인 문서로 가는 버그수정
	\DeskPlusEIP\Approval\Js\Status\Menu\Tree.js

[게시판] 템플릿관리 카운트 버그 수정
	\DeskPlusEIP\Board\Admin\Template\Add.aspx
	\DeskPlusEIP\Board\Admin\Template\AddN.aspx
	\DeskPlusEIP\Board\Admin\Template\List.aspx
	\DeskPlusEIP\Board\Js\Admin\Template\Add.js
	\DeskPlusEIP\Board\Js\Admin\Template\List.js

: 2016-02-18 by famo23 -------------------------------------------------------------
[전자결재] 개인별문서함 저장된 페이지카운트 가져오지 못하는 버그수정
	\DeskPlusEIP\Approval\Folder\Private\List.aspx.cs
	\DeskPlusEIP\Approval\Folder\Private\ListFolder.aspx.cs

[전자결재] 공문리스트 페이지카운트 저장하는 부분 없는 버그 수정
	\DeskPlusEIP\Approval\Js\Folder\Enforce\List.js
	\DeskPlusEIP\Approval\Js\Folder\Enforce\ListSend.js
	\DeskPlusEIP\Approval\Js\Folder\Enforce\ListSubmit.js

: 2016-01-26 by withsuki -----------------------------------------------------------
[전자결재] 고정수신결재망 버그수정 (수신부서 수정후재상신 시, 결재라인 순서오류)
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

: 2016-01-13 by famo23 -------------------------------------------------------------
[업무협업] 홈화면개발 , 포틀릿 수정 
	DP_ACC_UserEnv 필드추가 (TaskHomeMenu)
	DP_ACC_UserEnv_UpdTaskHomeMenu
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserEnvironmentInfo.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironment.cs
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTReceive.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTReference.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTRequest.htm
	\DeskPlusEIP\Portlet\Package\TabTaskMain.ascx
	Func_Home.css
	TaskProject 전체

[자원관리] 홈화면 개발
	Func_Home.css
	Func_Res.css
	RMProject 전체	

:2016-01-11 by kkzz52
[포틀릿] 음력 생일자 체크 시 년도 비교가 빠져있어 잘못 표기되는 오류 수정
	\DeskPlusEIP\Portlet\Ws\Package\Congratulation.asmx.cs

: 2015-12-29 by freeman -------------------------------------------------------------
[검색] 업무협업2, 자원관리 전문검색
    \DeskPlusEIP\Search\*.*
	\Projects\SearchProject\Zeospace.DeskPlus.Search.BE\RM\*.*
	\Projects\SearchProject\Zeospace.DeskPlus.Search.BE\TAS\*.*
	\Projects\SearchProject\Zeospace.DeskPlus.Search.BLL\RM\*.*
	\Projects\SearchProject\Zeospace.DeskPlus.Search.BLL\TAS\*.*
	\Projects\SearchProject\Zeospace.DeskPlus.Search.DAL\RM\*.*
	\Projects\SearchProject\Zeospace.DeskPlus.Search.DAL\TAS\*.*

	VW_Search_RM_GetAttachList
	VW_Search_RM_GetReserveList
	VW_Search_TAS_GetAttachList
	VW_Search_TAS_GetGroupIDByUserID
	VW_Search_TAS_GetTASList

: 2015-12-29 by withsuki -------------------------------------------------------------
[전자결재] 홈화면
	DP_ACC_UserEnv 필드추가 (ApprovalHomeMenu)

	DP_APP_FavoriteApp_SelList 수정
	DP_APP_Home_ListApprove
	DP_APP_Home_ListCarbon
	DP_APP_Home_ListComplete
	DP_APP_Home_ListTotal
	DP_APP_Home_SelAttach
	DP_APP_Home_SelAttachCnt
	DP_APP_Home_SelShortReply
	DP_APP_Home_SelShortReplyCnt
	DP_APP_Home_TodayCnt
	DP_ACC_UserEnv_UpdAppHomeMenu

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\AttachFileInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentProcessInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\FavoriteApprovalInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\SequenceInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Home.cs
	\DeskPlusEIP\Approval\FormFlow\Default.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Home\Default.aspx
	\DeskPlusEIP\Approval\Js\Home\Default.js
	\DeskPlusEIP\Approval\Js\Preference\Menu\Tree.js
	\DeskPlusEIP\Approval\Jt\Home\JTFavApp.htm
	\DeskPlusEIP\Approval\Jt\Home\JTFavForm.htm
	\DeskPlusEIP\Approval\Jt\Home\JTFile.htm
	\DeskPlusEIP\Approval\Jt\Home\JtImpo.htm
	\DeskPlusEIP\Approval\Jt\Home\JTReply.htm
	\DeskPlusEIP\Approval\Jt\Home\JTListTab.htm
	\DeskPlusEIP\Approval\Preference\Default.aspx
	\DeskPlusEIP\Approval\Preference\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Ws\Home\Default.asmx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx

: 2015-12-21 by withsuki -------------------------------------------------------------
[전자결재] 자주지정하는 양식 버그수정 (삭제된 양식처리)
	DP_APP_FavoriteForm_Sel
	DP_APP_FavoriteForm_SelPageList
	DP_APP_FavoriteForm_SelPageCnt
	DP_APP_Form_Del

: 2015-12-21 by famo23 -------------------------------------------------------------
[자원관리] 아이콘 이미지 변경
	\DeskPlusEIP\RM\Admin\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Admin\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Admin\Content\View.aspx.cs
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\RM\rs_icon1.png~rs_icon9.png

: 2015-12-18 by withsuki -------------------------------------------------------------
[전자결재] 대결자지정 버그수정 (겸직대상자를 대결지정한 경우 에러)
	DP_APP_Proxy_ProxyCnt

: 2015-12-18 by famo23 -------------------------------------------------------------
[자원관리] 자원 예약 관리
	DP_RM_Reserve_SelUsablePageCnt
	DP_RM_Reserve_SelUsablePageList
	QRY_RM_List_SelByUserID
	리소스 추가
	\DeskPlusEIP\RM\Content\Usable0List.aspx
	\DeskPlusEIP\RM\Content\Usable1List.aspx
	\DeskPlusEIP\RM\Content\Usable2List.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Folder\Tree.aspx
	\DeskPlusEIP\RM\Folder\Tree.aspx.cs
	\DeskPlusEIP\RM\Js\Content\Usable0List.js
	\DeskPlusEIP\RM\Js\Content\Usable1List.js
	\DeskPlusEIP\RM\Js\Content\Usable2List.js
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Js\Folder\Tree.js
	\DeskPlusEIP\RM\Jt\Content\JTUsableList.htm
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ReserveInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\List.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Reserve.cs

[자원관리] 나의 예약 현황 보기 
	DP_RM_Reserve_SelPageCnt
	DP_RM_Reserve_SelPageList
	리소스 추가
	\DeskPlusEIP\RM\Content\MyList.aspx
	\DeskPlusEIP\RM\Content\MyList.aspx.cs
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Folder\Tree.aspx
	\DeskPlusEIP\RM\Js\Content\Add.js
	\DeskPlusEIP\RM\Js\Content\MyList.js
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Js\Folder\Tree.js
	\DeskPlusEIP\RM\Jt\Content\JTMyList.htm
	\DeskPlusEIP\RM\Ws\Content\MyList.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ReserveInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Reserve.cs


: 2015-12-17 by shimpark -----------------------------------------------------------
[게시판 홈화면] 

\DeskPlusEIP\Board\Home\ 폴더 내부
\DeskPlusEIP\Board\Js\Home\ 폴더 내부
\DeskPlusEIP\Board\Jt\Home\ 폴더 내부

1. entity

\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\MaxShortReplyerInfo.cs
\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\MaxWriterInfo.cs
\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\ListViewModel.cs


2. busingess logic layer

\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\HomeList.cs


3. data access layer

\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\HomeList.cs

4. stored procedure

DP_BOA_HOME_statistics
DP_SEARCH_BoardUser_Ins


: 2015-12-14 by shimpark -----------------------------------------------------------
[검색엔진] 검색 키워드 추가 modulename

\DeskPlusEIP\Search\Js\Default.js
\DeskPlusEIP\Search\Js\List.js

\DeskPlusEIP\Search\Default.aspx
\DeskPlusEIP\Search\Default.aspx.cs

\DeskPlusEIP\Search\List.aspx
\DeskPlusEIP\Search\List.aspx.cs

: 2015-12-04 by withsuki -----------------------------------------------------------
[전자결재] 중요표시 기능개발

	** 테이블추가
	DP_APP_ImpoUser 

	** SP추가
	DP_APP_ImpoPageCnt
	DP_APP_ImpoPageList
	DP_APP_ImpoUser_Del
	DP_APP_ImpoUser_Ins

	** SP수정
	DP_APP_DocProcess_SelPageListReject
	DP_APP_DocProcess_SelPageListComplete	
	DP_APP_Doc_SelPageListDeptFolder
	DP_APP_Doc_SelPageListRecv
	DP_APP_Doc_SelPageListSend
	DP_APP_Doc_SelPageListDept
	DP_APP_Doc_SelPageListApp
	DP_APP_Doc_SelPageListAppFolder
	DP_APP_Doc_SelPageListArb
	DP_APP_Doc_SelPageListArbFolder
	DP_APP_Doc_SelPageListCarbon
	DP_APP_Doc_SelPageListForward
	DP_APP_Doc_SelPageListPrivate
	DP_APP_Doc_SelPageListPrivateFolder
	DP_APP_Doc_SelPageListReject
	DP_APP_Doc_SelPageListRejectFolder

	** 소스
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\ImpoUserInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ImpoUser.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Folder\Impo\List.aspx
	\DeskPlusEIP\Approval\Folder\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListRejectFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListReject.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\List.js
	\DeskPlusEIP\Approval\Js\Folder\Approve\ListFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\List.js
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\ListFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\List.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\SendList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\ReceiptList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\CommonList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\List.js
	\DeskPlusEIP\Approval\Js\Folder\Impo\List.js
	\DeskPlusEIP\Approval\Js\Folder\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\Folder\Private\List.js
	\DeskPlusEIP\Approval\Js\Folder\Private\ListFolder.js
	\DeskPlusEIP\Approval\Js\Status\JTListReject.js
	\DeskPlusEIP\Approval\Js\Status\JTListComplete.js
	\DeskPlusEIP\Approval\Jt\Folder\Approve\ListRejectFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\ListReject.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\ListFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Arbitrary\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Arbitrary\ListFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\ForwardList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\SendList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\ReceiptList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\CommonList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Impo\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Menu\Tree.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\ListFolder.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListReject.htm
	\DeskPlusEIP\Approval\Jt\Status\JTListComplete.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\ListRejectFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\ListReject.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Approve\ListFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Arbitrary\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Arbitrary\ListFolder.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\ForwardList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\SendList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\ReceiptList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\CommonList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Dept\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Impo\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Menu\Tree.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\List.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\ListFolder.htm
	\DeskPlusEIP\Approval\Resource\GlobalString\
	\DeskPlusEIP\Approval\Ws\Folder\Approv\List.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Approv\ListFolder.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Arbitrary\List.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Arbitrary\ListFolder.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\ForwardList.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\List.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Dept\CommonList.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Dept\ReceiptList.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Dept\SendList.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Dept\List.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Impo\List.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Private\List.asmx
	\DeskPlusEIP\Approval\Ws\Folder\Private\ListFolder.asmx
	\DeskPlusEIP\Approval\Ws\Status\ListComplete.asmx
	\DeskPlusEIP\Approval\Ws\Status\ListReject.asmx



### DeskPlusSP v4.4 (2015.11.17) #########################################################

: 2015-12-16 by famo23 -----------------------------------------------------------
[업무협업] 참조 답변쓰고 페이지 이동 잘못된 버그 수정
	\DeskPlusEIP\Task\Content\Reference\View.aspx.cs

: 2015-12-01 by withsuki -----------------------------------------------------------
[전자결재] DP_Query 쿼리수정 
 - WITH(NOLOCK)힌트 추가 및 합의결재자 체크관련 쿼리 변경
	update dp_query set qryvalue='SELECT COUNT(CompanyID) FROM DP_APP_Account WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DeptID = @DeptID AND AccountType = @AccountType AND UserID = @UserID AND IsActive = ''1''' WHERE qryid = 'QRY_APP_Account_SelGetUser'
	update dp_query set qryvalue='SELECT * FROM DP_APP_Attach WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID AND (FileType = ''.jpg'' OR FileType = ''.jpeg'' OR FileType = ''.gif'' OR FileType = ''.png'' OR  FileType = ''.bmp'' )' WHERE qryid = 'QRY_APP_Attach_SelGetImage'
	update dp_query set qryvalue='SELECT * FROM DP_APP_Attach WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID AND AttachFileID = @AttachFileID' WHERE qryid = 'QRY_APP_AttachFile_SelByID'
	update dp_query set qryvalue='SELECT * FROM DP_APP_BaseDoc WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID ORDER BY BaseOrder' WHERE qryid = 'QRY_APP_BaseDoc_Sel'
	update dp_query set qryvalue='SELECT COUNT(CompanyID) FROM DP_APP_BaseDoc WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID' WHERE qryid = 'QRY_APP_BaseDoc_SelCnt'
	update dp_query set qryvalue='SELECT * FROM DP_APP_Cabinet WITH(NOLOCK) WHERE CompanyID = @CompanyID AND ActiveCabinet in ({0})' WHERE qryid = 'QRY_APP_Cabinet_Sel'
	update dp_query set qryvalue='SELECT * FROM DP_APP_Cabinet WITH(NOLOCK) WHERE CompanyID = @CompanyID AND ActiveCabinet in ({0}) AND CabinetID = @CabinetID' WHERE qryid = 'QRY_APP_Cabinet_SelByID'
	update dp_query set qryvalue='SELECT * FROM DP_APP_CirAttachFile WITH(NOLOCK) WHERE CompanyID = @CompanyID AND CirculationDocumentID = @CirculationDocumentID ORDER BY DocumentID, FileOrder' WHERE qryid = 'QRY_APP_CirAttachFile_Sel'
	update dp_query set qryvalue='SELECT * FROM DP_APP_CirAttachFile WITH(NOLOCK) WHERE CompanyID = @CompanyID AND CirculationDocumentID = @CirculationDocumentID AND CirculationAttachFileID = @CirculationAttachFileID' WHERE qryid = 'QRY_APP_CirAttachFile_SelByID'
	update dp_query set qryvalue='SELECT COUNT(CompanyID) FROM DP_APP_CirAttachFile WITH(NOLOCK) WHERE CompanyID = @CompanyID AND CirculationDocumentID = @CirculationDocumentID ' WHERE qryid = 'QRY_APP_CirAttachFile_SelCnt'
	update dp_query set qryvalue='SELECT * FROM DP_APP_CirAttachFile WITH(NOLOCK) WHERE CompanyID = @CompanyID AND CirculationDocumentID = @CirculationDocumentID AND (FileType = ''.jpg'' OR FileType = ''.jpeg'' OR FileType = ''.gif'' OR FileType = ''.png'' OR  FileType = ''.bmp'' )' WHERE qryid = 'QRY_APP_CirAttachFile_SelGetImage'
	update dp_query set qryvalue='SELECT * FROM DP_APP_CirBaseDoc WITH(NOLOCK) WHERE CompanyID = @CompanyID AND CirculationDocumentID = @CirculationDocumentID ORDER BY DocumentID, BaseOrder' WHERE qryid = 'QRY_APP_CirBaseDoc_Sel'
	update dp_query set qryvalue='SELECT COUNT(CompanyID) FROM DP_APP_CirBaseDoc WITH(NOLOCK) WHERE CompanyID = @CompanyID AND CirculationDocumentID = @CirculationDocumentID' WHERE qryid = 'QRY_APP_CirBaseDoc_SelCnt'
	update dp_query set qryvalue='SELECT ContentHTML FROM DP_APP_CirDoc WITH(NOLOCK) Where CompanyID = @CompanyID AND CirculationDocumentID = @CirculationDocumentID' WHERE qryid = 'QRY_APP_CirDoc_SelContentHtml'
	update dp_query set qryvalue='SELECT ContentHTML FROM DP_APP_Doc WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID' WHERE qryid = 'QRY_APP_Doc_SelContentHtml'
	update dp_query set qryvalue='SELECT GroupID FROM DP_APP_Doc WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID' WHERE qryid = 'QRY_APP_Doc_SelGroupID'
	update dp_query set qryvalue='SELECT UpPathID FROM DP_APP_Doc WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID' WHERE qryid = 'QRY_APP_Doc_SelUpPath'
	update dp_query set qryvalue='SELECT ContentHTML FROM DP_APP_DocProcess WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID' WHERE qryid = 'QRY_APP_DocProcess_SelContentHtml'
	update dp_query set qryvalue='SELECT COUNT(CompanyID) FROM DP_APP_DocProcess WITH(NOLOCK) WHERE CompanyID = @CompanyID AND FormID = @FormID' WHERE qryid = 'QRY_APP_DocProcess_SelFormID'
	update dp_query set qryvalue='SELECT GroupID FROM DP_APP_DocProcess WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID' WHERE qryid = 'QRY_APP_DocProcess_SelGroupID'
	update dp_query set qryvalue='SELECT COUNT(FormID) FROM DP_APP_Draft WITH(NOLOCK) WHERE CompanyID = @CompanyID AND FormID = @FormID' WHERE qryid = 'QRY_APP_Draft_SelFormID'
	update dp_query set qryvalue='SELECT *,((SELECT COUNT(FolderID) FROM DP_APP_Folder A WITH(NOLOCK) WHERE A.CompanyID = f.CompanyID AND A.ParentID = f.FolderID AND A.IsActive in ({0})) + (SELECT COUNT(FormID) FROM DP_APP_Form B WITH(NOLOCK) WHERE B.CompanyID = f.CompanyID AND B.FolderID = f.FolderID AND B.IsActive in ({0}))) AS CHILDCNT FROM DP_APP_Folder f WITH(NOLOCK) WHERE f.CompanyID = @CompanyID  AND f.IsActive in ({0}) AND f.ParentID = @FolderID ORDER BY f.FolderOrder ASC' WHERE qryid = 'QRY_APP_Folder_SelByParentID'
	update dp_query set qryvalue='SELECT * FROM DP_APP_Folder WITH(NOLOCK) WHERE CompanyID = @CompanyID AND IsActive in ({0}) AND TypeInfo in ({1}) ORDER BY FolderOrder ASC' WHERE qryid = 'QRY_APP_Folder_SelByType'
	update dp_query set qryvalue='SELECT * FROM DP_APP_Folder WITH(NOLOCK) WHERE CompanyID = @CompanyID AND ParentID = @FolderID AND IsActive in ({0}) Order by FolderOrder' WHERE qryid = 'QRY_APP_Folder_SelSubList'
	update dp_query set qryvalue='SELECT * FROM DP_APP_FolderAuth WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DeptID = @DeptID' WHERE qryid = 'QRY_APP_FolderAuth_SelByDeptID '
	update dp_query set qryvalue='SELECT f.CompanyID, f.FolderAuthID, f.FolderID, f.DeptID, d.NameBase, d.NameKo, d.NameEn, d.NameJa, d.NameFr, d.NameZhcn, d.NameZhtw, d.ManyName FROM [dbo].[DP_APP_FolderAuth] f WITH(NOLOCK) LEFT OUTER JOIN [dbo].[DP_ACC_Dept] d WITH(NOLOCK) ON f.CompanyID = d.CompanyID AND f.DeptID = d.DeptID WHERE f.CompanyID = @CompanyID AND f.FolderID = @FolderID' WHERE qryid = 'QRY_APP_FolderAuth_SelByFolderID'
	update dp_query set qryvalue='SELECT * FROM DP_APP_FolderPageSize WITH(NOLOCK) WHERE CompanyID = @CompanyID AND FolderID = @FolderID AND UserID = @UserID AND Types = @Types' WHERE qryid = 'QRY_APP_FolderPageSize_Sel'
	update dp_query set qryvalue='SELECT * FROM dbo.DP_APP_FolderStatus WITH(NOLOCK) WHERE CompanyID = @CompanyID AND ActiveFolder = ''1'' ORDER BY ParentFolderStatusID ASC, Orders ASC' WHERE qryid = 'QRY_APP_FolderStatus_Sel'
	update dp_query set qryvalue='SELECT * FROM DP_APP_FolderStatus WITH(NOLOCK) WHERE CompanyID = @CompanyID AND ActiveFolder in ({0}) AND FolderStatusID= @FolderStatusID' WHERE qryid = 'QRY_APP_FolderStatus_SelByID'
	update dp_query set qryvalue='SELECT * FROM DP_ACC_Dept WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DeptID IN (SELECT TargetDeptID FROM DP_APP_FolderViewAuth WITH(NOLOCK) WHERE CompanyID = @CompanyID AND UserID = @UserID AND Type = @Type GROUP BY TargetDeptID) AND ActiveDept = ''1'' ORDER BY Levels ASC, DeptOrder ASC' WHERE qryid = 'QRY_APP_FolderViewAuth_SelGetDept'
	update dp_query set qryvalue='SELECT * FROM DP_APP_Form WITH(NOLOCK) WHERE CompanyID = @CompanyID AND FolderID != 4 ORDER BY FolderID, FormOrder, FormID DESC' WHERE qryid = 'QRY_APP_Form_Sel'
	update dp_query set qryvalue='SELECT * FROM DP_APP_Form WITH(NOLOCK) WHERE CompanyID = @CompanyID AND FolderID = @FolderID AND IsActive in ({0}) ORDER BY FormOrder, FormID DESC' WHERE qryid = 'QRY_APP_Form_SelByFolderID'
	update dp_query set qryvalue='SELECT * FROM DP_APP_ForwardDoc WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID AND ForwardDocumentID = @ForwardDocumentID' WHERE qryid = 'QRY_APP_ForwardDoc_Sel'
	update dp_query set qryvalue='SELECT COUNT(CompanyID) FROM DP_APP_History WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID' WHERE qryid = 'QRY_APP_History_SelGetCount'
	update dp_query set qryvalue='SELECT * FROM DP_APP_PrivateFolder WITH(NOLOCK) WHERE CompanyID = @CompanyID AND UserID = @UserID AND FolderID = @FolderID' WHERE qryid = 'QRY_APP_PrivateFolder_SelByID'
	update dp_query set qryvalue='SELECT * FROM DP_APP_PublicFolder WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DeptID = @DeptID AND FolderID = @FolderID' WHERE qryid = 'QRY_APP_PublicFolder_SelByID'
	update dp_query set qryvalue='SELECT Count(CompanyID) FROM DP_APP_SEQ WITH(NOLOCK) WHERE CompanyID = @CompanyID AND DocumentID = @DocumentID AND Sequence < @Sequence AND ApprovalTag LIKE ''ZA%''' WHERE qryid = 'QRY_APP_Seq_SelZA'


: 2015-11-27 by withsuki -----------------------------------------------------------
[일정관리] 댓글순서 변경

	DP_PIM_ScheduleReply_SelPageList
	DP_PIM_ToDoReply_SelPageList


: 2015-11-25 by withsuki -----------------------------------------------------------
[자원관리] 자원상세보기 링크제거 (해당 페이지 없었음)
	\DeskPlusSolution\DeskPlusEIP\RM\Content\WholeWeek.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\RM\Content\WholeDay.aspx.cs


: 2015-11-23 by famo23 -----------------------------------------------------------
[모바일관리] 핸드폰번호 가져오는 버그 수정
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js
	\DeskPlusEIP\Account\Js\MyInfo\Mobile.js

: 2015-11-16 by shimpark -----------------------------------------------------------
[게시판] business logic layer 의 transaction 처리

  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\AttachFile.cs
  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Authority.cs
  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Category.cs
  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\CreatePDF.cs
  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Lists.cs
  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\MyAttachFile.cs
  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\MyTemplate.cs
  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Ratings.cs
  \Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\ShortReply.cs

: 2015-11-13 by famo23 -----------------------------------------------------------
[모바일관리] 업무협업 알람설정 수정
	\DeskPlusEIP\Account\Js\MyInfo\Mobile.js
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx.cs
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js

: 2015-11-11 by famo23 -----------------------------------------------------------
[모바일관리] 환경설정 기본값 세팅 
	DP_COM_Company_Ins

: 2015-11-03 by shimpark -----------------------------------------------------------
[게시판] 통계자료

통계 차트 교체 ie 8 이상 가동
/*Developed by Insook Choe (choe.insook@nhn.com), Inho Jung(inho.jung@nhn.com)*/
//네이버 오픈소스임. 상업적 사용 가능

\DeskPlusEIP\Html\Js\ChartNwagon
\DeskPlusEIP\Board\Admin\Statistic\LineChart.aspx
\DeskPlusEIP\Board\Admin\Statistic\LineChart.aspx.cs

: 2015-11-03 by withsuki -----------------------------------------------------------
[전자결재] Transaction 처리

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\AttachFile.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DraftSequence.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\EnforceAdmin.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\EnforceHistory.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FavoriteApproval.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FavSeq.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FavSeqLine.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\PrivateDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\PublicDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\SequenceLater.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ShortReply.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\SequenceBackUp.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Proxy.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\History.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ForwardDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FormSeq.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Form.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FolderViewAuthority.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FolderAuthority.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Folder.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FavoriteForm.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FavoriteApprovalNet.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\EnforceStamp.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Enforce.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DraftFile.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DraftBaseDoc.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Draft.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CounterPartUser.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CirculationUser.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CirculationDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CarbonCopyUser.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\BaseDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ArbitraryDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Account.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ApprovalDocument.cs

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Enforce.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Transfer.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\PassedDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\ForwardDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Forms.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\FoldersAuthority.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DraftFile.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Draft.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\CirculationDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\AttachFile.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs


: 2015-10-28 by withsuki -----------------------------------------------------------
[전자결재] 대결자 알람처리
	
	sp) DP_APP_Seq_SelAlarmNext

: 2015-10-27 by withsuki -----------------------------------------------------------
[전자결재] 
	- 메일알람 참조의 경우, 묶음 처리 (기존의 경우, eml파일이, 수신인 만큼 생성됨. 참조의 경우, 하나의 eml파일로 처리)
	- 알람 발송 시, DB Access수를 줄임. (기존의 경우, 80명에게 메일발송 시, 160번의 DB Access되는 로직있었음. 해당 부분 제거)

	소스)
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

	SP)
		DP_APP_Seq_SelAlarmApproveCNext
		DP_APP_Seq_SelAlarmApproveD
		DP_APP_Seq_SelAlarmApproveDC
		DP_APP_Seq_SelAlarmApproving
		DP_APP_Seq_SelAlarmCirculation
		DP_APP_Seq_SelAlarmComplete
		DP_APP_Seq_SelAlarmForward
		DP_APP_Seq_SelAlarmNext
		DP_APP_Seq_SelAlarmNextAll
		DP_APP_Seq_SelAlarmOpinion
		DP_APP_Seq_SelAlarmOpinionAll
		DP_APP_Seq_SelAlarmReject
		DP_APP_Seq_SelAlarmRejectAll

: 2015-10-23 by withsuki -----------------------------------------------------------
[전자결재] 
	- 부서별 문서함 열람권한 View 방법
	- 문서함의 초기메뉴 설정 기능

	\DeskPlusEIP\Approval\Folder\Approve\List.aspx
	\DeskPlusEIP\Approval\Folder\ListRejectFolder.aspx
	\DeskPlusEIP\Approval\Folder\ListReject.aspx
	\DeskPlusEIP\Approval\Folder\ListFolder.aspx
	\DeskPlusEIP\Approval\Folder\Arbitrary\List.aspx
	\DeskPlusEIP\Approval\Folder\Arbitrary\ListFolder.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirReceiptList.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardList.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirSendList.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\List.aspx
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Folder\Dept\CommonList.aspx
	\DeskPlusEIP\Approval\Folder\Dept\ReceiptList.aspx
	\DeskPlusEIP\Approval\Folder\Dept\SendList.aspx
	\DeskPlusEIP\Approval\Folder\Dept\List.aspx
	\DeskPlusEIP\Approval\Folder\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Folder\Private\List.aspx
	\DeskPlusEIP\Approval\Folder\Private\ListFolder.aspx
	\DeskPlusEIP\Approval\Js\Folder\Approve\List.js
	\DeskPlusEIP\Approval\Js\Folder\ListRejectFolder.js
	\DeskPlusEIP\Approval\Js\Folder\ListReject.js
	\DeskPlusEIP\Approval\Js\Folder\ListFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\List.js
	\DeskPlusEIP\Approval\Js\Folder\Arbitrary\ListFolder.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReceiptList.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\ForwardList.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendList.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\List.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\CommonList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\ReceiptList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\SendList.js
	\DeskPlusEIP\Approval\Js\Folder\Dept\List.js
	\DeskPlusEIP\Approval\Js\Folder\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\Folder\Private\List.js
	\DeskPlusEIP\Approval\Js\Folder\Private\ListFolder.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusEIP\Approval\Ws\Folder\Common\SetDefault.asmx
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironment.cs

	DP_ACC_UserEnv_UpdAppPublicMenu
	

: 2015-10-22 by withsuki -----------------------------------------------------------
[전자결재] 부서별문서함 - 부서별공용폴더에서 직급별 조회 누락됨. (AccessTitleID)
	sp>
		DP_APP_Doc_SelPageListDeptFolder
		DP_APP_Doc_SelPageCntDeptFolder

	source>
		\DeskPlusEIP\Approval\Folder\Dept\CommonList.aspx
		\DeskPlusEIP\Approval\Folder\Dept\CommonList.aspx.cs
		\DeskPlusEIP\Approval\WS\Folder\Dept\CommonList.asmx
		\DeskPlusEIP\Approval\JS\Folder\Dept\CommonList.js
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs

: 2015-10-21 by famo23 -----------------------------------------------------------
[모바일관리] 승인불가 사용자 부서 설정
	리소스추가
	DP_MDM_AuthGroup 테이블 생성
	QRY_MDM_AuthGroup_Sel
	DP_MDM_AuthGroup_Ins
	DP_MDM_AuthGroup_Del
	VW_MDM_AuthGroup
	\DeskPlusEIP\MDM\Auth\AuthGroup.aspx
	\DeskPlusEIP\MDM\Js\Auth\AuthGroup.js
	\DeskPlusEIP\MDM\Menu\Menu.aspx
	\DeskPlusEIP\MDM\Js\Menu\Menu.js
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\AuthGroupInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BLL\AuthGroup.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\AuthGroup.cs

: 2015-10-20 by kkzz52 -----------------------------------------------------------
[전자결재] 반려 가능 수신일 경우 문서번호 생성 시 수신부서ID로 카운트 하는 오류 수정
	DP_APP_DocProcess_SelByDocNumber
	DPM_APP_DocProcess_SelByDocNumber

: 2015-10-20 by famo23 -----------------------------------------------------------
[자원관리] 댓글 기능추가
	DP_RM_ShortReply 테이블 생성
	QRY_RM_ShortReply_Sel
	QRY_RM_ShortReply_SelByID
	DP_RM_ShortReply_Ins
	DP_RM_ShortReply_Upd
	DP_RM_ShortReply_Del
	리소스추가
	\DeskPlusEIP\App_Data\RESOURCEMANAGER.xml
	\DeskPlusEIP\RM\Content\ShortReply.aspx
	\DeskPlusEIP\RM\Content\ShortReplyMod.aspx
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Js\Content\ShortReply.js
	\DeskPlusEIP\RM\Js\Content\ShortReplyMod.js
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Jt\Content\JTReply.htm
	\DeskPlusEIP\RM\Ws\Content\ShortReply.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\ShortReplyMod.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\View.asmx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ShortReplyInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\ShortReply.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\ShortReply.cs

: 2015-10-20 by kkzz52 -----------------------------------------------------------
[전자결재] 휴가원 오전, 오후반차 추가, 휴가종류 휴일근무,리프레쉬 삭제 
	\DeskPlusEIP\Legacy\HoliDay.aspx
	\DeskPlusEIP\Legacy\HoliDay_Complete.aspx
	\DeskPlusEIP\Common\Calendar\CalendarDualDialog.aspx

	\DeskPlusEIP\ETC\Js\Holiday\ListAdmin.js
	\DeskPlusEIP\ETC\Holiday\ListAdmin.cs
	\DeskPlusEIP\ETC\Jt\Holiday\JTListAdmin.htm
	\DeskPlusEIP\ETC\Ws\Holiday\ListAdmin.cs
	\DeskPlusEIP\ETC\Ws\Holiday\ListUser.cs
	\DeskPlusEIP\ETC\Js\Holiday\ListUser.js
	\DeskPlusEIP\ETC\Holiday\Popup\ViewUser.aspx
	\DeskPlusEIP\ETC\Holiday\Popup\ViewUser.aspx.cs
	\DeskPlusEIP\ETC\Jt\Holiday\JTAdminManage.htm

	DP_HOLI_Cat
	DP_HOLI_Admin_SelPageList
	DP_HOLI_Admin_SelAllList
	DP_HOLI_Admin_SelHoliCnt
	DP_HOLI_Used_SelPageList
	DP_HOLI_Used_SelAllList
	DP_HOLI_Used_SelSrhPageList

: 2015-10-19 by famo23 -----------------------------------------------------------
[자원관리]신청날짜 보기화면에 추가
	리소스추가
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\ViewPopup.aspx


: 2015-10-19 by shimpark -----------------------------------------------------------
[게시판] 툴팁 기능 - 리스트, 블로그, 갤러리 형태 설명 주가

\DeskPlusEIP\Html\Js\Tooltip\jquery.qtip.min.css
\DeskPlusEIP\Html\Js\Tooltip\jquery.qtip.min.js

-- 이미지 title 태그에 기재 및 위의 css, js 참조

\DeskPlusEIP\Board\Content\BlogList.aspx(106):                    
\DeskPlusEIP\Board\Content\BlogListImpo.aspx(93):                    
\DeskPlusEIP\Board\Content\GalleryList.aspx(103):                    
\DeskPlusEIP\Board\Content\GalleryListImpo.aspx(94):                    
\DeskPlusEIP\Board\Content\List.aspx(103):
\DeskPlusEIP\Board\Content\ListImpo.aspx(91):
\DeskPlusEIP\Board\Temp\BlogList.aspx(85):                
\DeskPlusEIP\Board\Temp\GalleryList.aspx(84):                
\DeskPlusEIP\Board\Temp\List.aspx(84):  

-- js 기술

$('a[title]').qtip();

  \DeskPlusEIP\Board\Js\Content\BlogList.js(20):        
  \DeskPlusEIP\Board\Js\Content\BlogListImpo.js(20):        
  \DeskPlusEIP\Board\Js\Content\GalleryList.js(20):        
  \DeskPlusEIP\Board\Js\Content\GalleryListImpo.js(20):        
  \DeskPlusEIP\Board\Js\Content\List.js(21):        
  \DeskPlusEIP\Board\Js\Content\ListImpo.js(21):        
  \DeskPlusEIP\Board\Js\Temp\BlogList.js(20):        
  \DeskPlusEIP\Board\Js\Temp\GalleryList.js(20):        
  \DeskPlusEIP\Board\Js\Temp\List.js(21):        
     
-- 리소스 추가

\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx


: 2015-10-19 by shimpark -----------------------------------------------------------
[주소록]  폴더 => 그룹변경 리소스

\DeskPlusEIP\Address\Resource\GlobalString\string.ko.resx

: 2015-10-19 by shimpark -----------------------------------------------------------
[댓글 새창] 날짜 정렬 기능 추가

소스변경
	\DeskPlusEIP\Board\Content\ShortReplyList.aspx
	\DeskPlusEIP\Board\Js\Content\ShortReplyList.js
	\DeskPlusEIP\Board\Jt\Content\JTShortReplyList.htm
	\DeskPlusEIP\Board\Ws\Content\ShortReplyList.asmx.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\ShortReply.cs

sp 변경
	DP_BOA_ShortReply_SelPageList

: 2015-10-15 by withsuki -----------------------------------------------------------
[전자결재 - 부서별문서함 수정]

** 소스
	\DeskPlusEIP\Approval\Common\Folder\GetFolderTreeScript.aspx.cs
	\DeskPlusEIP\Approval\Common\Folder\FolderDeptTreeScript.ascx
	\DeskPlusEIP\Approval\Folder\Dept\CompList.aspx.cs
	\DeskPlusEIP\Approval\Folder\Dept\ReceiptList.aspx.cs
	\DeskPlusEIP\Approval\Folder\Dept\SendList.aspx.cs
	\DeskPlusEIP\Approval\Folder\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Folder\OtherDept\ReceiptList.aspx.cs
	\DeskPlusEIP\Approval\Folder\OtherDept\SendList.aspx.cs
	\DeskPlusEIP\Approval\Jt\Folder\Dept\JTReceiptList.htm
	\DeskPlusEIP\Approval\Ws\Folder\Dept\ReceiptList.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Dept\CompList.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Dept\SendList.asmx.cs
	\DeskPlusEIP\Approval\Folder\OtherDept\SendList.aspx
	\DeskPlusEIP\Approval\Folder\OtherDept\ReceiptList.aspx
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\SendList.aspx
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\ReceiptList.aspx
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs

** DB
	DP_APP_Doc_SelTreeDept		(변경)
	DP_APP_Doc_SelTreeDeptRecv	(추가)
	DP_APP_Doc_SelTreeDeptSend	(추가)
	DP_APP_Doc_SelPageListRecv	(추가)
	DP_APP_Doc_SelPageListSend	(추가)
	DP_APP_Doc_SelPageListSendRecv	(사용안함)
	DP_APP_Doc_SelPageCntRecv	(추가)
	DP_APP_Doc_SelPageCntSend	(추가)
	DP_APP_Doc_SelPageCntSendRecv	(사용안함)
	VW_APP_PublicReceiveDoc		(변경)
	DP_APP_DocProcess_UpdConclude   (변경 : 완료 시, 수신부서도 부서별문서함에 보여짐 : PublicDoc에 Insert)
	DPM_APP_DocProcess_UpdConclude (변경)
	DPM_APP_DocProcess_Return (변경)
	DPM_APP_DocProcess_UpdConcludeR (추가)
	DP_APP_PrivateDoc 필드추가 (PrivateDocumentID)

** 수신완료문서함 제거
	DELETE FROM [dbo].[DP_APP_Cabinet] where CabinetID ='Dept_Complete'


: 2015-10-14 by shimpark -----------------------------------------------------------
[통합게시판]

: 인사 GMU 명칭 변경하기

** 변경	

	: \Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs (QRY_ACC_User_SelGetAll 로 변경)

	public List<UserInfo> GetUserInfo(int _companyID, bool _activeUser)
	public List<int> GetUserIDList(int _companyID, bool _activeUser)

	- QRY_ACC_User_SelGetUserID => QRY_ACC_User_SelGetAll

	SELECT * FROM VW_UserInfo
	WHERE COMPANYID = @CompanyID  AND ActiveCompany = '1'
	AND BASEDEPT = '1' AND ActiveUser IN({0})
	ORDER By titlecodeorder asc, dutycodeorder asc, userdeptorder desc, deptorder asc, writedate desc

	- QRY_ACC_User_SelByUserIDDate 변경

	SELECT 
		Info.*,
		(SELECT Count(*) 
		   FROM DP_PIM_ShortReply 
		   WHERE CompanyID = Info.CompanyID 
		   AND GroupID in (SELECT GroupID From DP_PIM_ShortReply 
		   WHERE CompanyID = Info.CompanyID 
		   AND (RUserID = Info.USerID or UserID = Info.USerID))) as ShortCnt
		, ISNULL((SELECT Count(*) 
		  FROM DP_PIM_ShortReply 
		  WHERE CompanyID = Info.CompanyID 
		  AND GroupID in (SELECT GroupID From DP_PIM_ShortReply
		  WHERE CompanyID = Info.CompanyID 
		  AND (RUserID = Info.USerID or UserID = Info.USerID)
	  AND DateDiff(hour, InputDate, GETDATE()) <= 24)),0) as TodayCnt
	FROM VW_UserInfo as Info
	WHERE  
		Info.ActiveCompany = '1'
	AND Info.UserID = @UserID AND Info.ActiveUser IN({0})
	AND Info.BaseDept IN({1})


** 추가

	: \Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs 
	public List<UserInfo> GetUserTotInfo(int _companyID, string _userList, bool _activeUser, bool _baseDept)

	- QRY_ACC_User_SelGetTot

	SELECT * FROM VW_UserInfo 
	WHERE ActiveCompany = '1'
	AND ActiveUser IN ({0})
	AND BaseDept IN ({1})

** 삭제됨 (별 필요 없음)

- QRY_ACC_User_Sel

** board.xml 추가
: 게시판 관리 화면에서 통합폴더를 제공함.
<Item ID="UseMultiCompanyBoard" Value="Y"  Content="전사 게시판 사용여부" />

** 저장프로시저명

DP_BOA_Template_SelPageList
DP_BOA_Template_SelPageCount
DP_BOA_Template_InsUpd
DP_BOA_Template_Del
DP_BOA_ShortReply_Upd
DP_BOA_ShortReply_SelPageList
DP_BOA_ShortReply_SelPageCnt
DP_BOA_ShortReply_Ins
DP_BOA_ShortReply_DelByID
DP_BOA_ShortReply_Del
DP_BOA_Rating_SelPageList
DP_BOA_Rating_SelPageCount
DP_BOA_Rating_Ins
DP_BOA_MyTemplateEtc_Upd
DP_BOA_MyTemplateEtc_Ins
DP_BOA_MyTemplateEtc_Del
DP_BOA_MyTemplate_UpdScreenShot
DP_BOA_MyTemplate_Upd
DP_BOA_MyTemplate_SelPageList
DP_BOA_MyTemplate_SelPageCnt
DP_BOA_MyTemplate_InsMove
DP_BOA_MyTemplate_Ins
DP_BOA_MyTemplate_Del
DP_BOA_MyAttach_Ins
DP_BOA_MyAttach_DelByID
DP_BOA_MyAttach_Del
DP_BOA_ListWrite_List_Cnt
DP_BOA_ListWrite_List
DP_BOA_ListViewer_SelPageList
DP_BOA_ListViewer_SelPageCount
DP_BOA_ListViewer_List_Cnt
DP_BOA_ListViewer_List
DP_BOA_ListViewer_Ins
DP_BOA_ListViewer_Del
DP_BOA_ListEtc_Upd
DP_BOA_ListEtc_Ins
DP_BOA_ListEtc_Del
DP_BOA_List_UpdScreenShot
DP_BOA_List_Upd
DP_BOA_List_SelPageList
DP_BOA_List_SelPageCnt
DP_BOA_List_Move
DP_BOA_List_Ins
DP_BOA_List_DelByUserID
DP_BOA_List_Del
DP_BOA_ImpoUser_Ins
DP_BOA_ImpoUser_Del
DP_BOA_ImpoPageList
DP_BOA_ImpoPageCnt
DP_BOA_Header_InsUpd
DP_BOA_Header_Del
DP_BOA_Favorite_UpdPID
DP_BOA_Favorite_UpdOrder
DP_BOA_Favorite_Upd
DP_BOA_Favorite_InsCopy
DP_BOA_Favorite_Ins
DP_BOA_Favorite_Del
DP_BOA_CategoryUser_InsUpd
DP_BOA_Category_UpdOrder
DP_BOA_Category_UpdMove
DP_BOA_Category_Upd
DP_BOA_Category_Ins
DP_BOA_Category_Del
DP_BOA_Auth_SelGetMyAuth
DP_BOA_Auth_Ins
DP_BOA_Auth_Del
DP_BOA_Attach_Ins
DP_BOA_Attach_DelByID
DP_BOA_Attach_Del

** 뷰명

VW_BOA_List
VW_BOA_Auth


** GMU

QRY_BOA_Attach_SelGetImage
QRY_BOA_Attach_SelGetImageOne
QRY_BOA_AttachFile_Sel
QRY_BOA_AttachFile_SelByID
QRY_BOA_Authority_SelByID
QRY_BOA_Category_Path
QRY_BOA_Category_Sel
QRY_BOA_Category_SelByID
QRY_BOA_Category_SelByPID
QRY_BOA_CategoryHeader_Sel
QRY_BOA_CategoryHeader_SelByID
QRY_BOA_CategoryUser_Sel
QRY_BOA_Favorite_Sel
QRY_BOA_Favorite_SelByPID
QRY_BOA_Favorite_SelGetTree
QRY_BOA_Header_Sel
QRY_BOA_Header_SelByHeaderID
QRY_BOA_List_SelByID
QRY_BOA_List_SelCountByCatID
QRY_BOA_List_SelGetdate
QRY_BOA_ListEtc_Sel
QRY_BOA_ListEtc_SelByID
QRY_BOA_ListEtc_SelMainPopup
QRY_BOA_ListEtc_SelMainPopupCount
QRY_BOA_MyAttach_SelByAttachID
QRY_BOA_MyAttach_SelByID
QRY_BOA_MyAttach_SelGetImage
QRY_BOA_MyTemplate_SelByID
QRY_BOA_MyTemplateEtc_SelByID
QRY_BOA_Path_SelByID
QRY_BOA_Rating_SelByUserID
QRY_BOA_Rating_SelGetPoint
QRY_BOA_ShortReply_Sel
QRY_BOA_ShortReply_SelByID
QRY_BOA_Statistic_YYMM
QRY_BOA_Template_Sel
QRY_BOA_Template_SelByID



: 2015-10-08 by famo23 -----------------------------------------------------------
[내정보관리] 내정보관리에서 본인의 모바일 기기 정보 보이기
	리소스추가
	DP_MDM_List_UpdByUser
	QRY_MDM_List_SelAll
	\DeskPlusEIP\Account\Js\MyInfo\Mobile.js
	\DeskPlusEIP\Account\Js\MyInfo\Mod.js
	\DeskPlusEIP\Account\MyInfo\Mobile.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\MDM\Ws\Device\DeviceInfo.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\List.cs

[모바일관리] 모바일관리 알람설정 보이기 처리
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js
	DP_MDM_List_Upd
	DP_MDM_List_UpdAll

[전자결재] 결재문서관리 발신부서상신자 검색 에러 수정
	DP_APP_Doc_SelPageListForm
	DP_APP_DocProcess_SelPageListForm

: 2015-10-08 by withsuki -----------------------------------------------------------
[공통] - 파일처리 중복코드 주석처리
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\FileHelper.cs

: 2015-10-07 by freeice122 -----------------------------------------------------------
[전자결재] - 대결대상문서리스트
	DP_APP_FolderStatus 항목추가

	sp추가>
		DP_APP_DocProcess_SelPageCntProxy
		DP_APP_DocProcess_SelPageListProxy

	소스>
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
		\DeskPlusEIP\Portlet\Js\Package\Myinfo.js
		\DeskPlusEIP\Approval\Common\Status\FolderStatusTreeScript.ascx.cs
		\DeskPlusEIP\Approval\Status\ListProxy.aspx
		\DeskPlusEIP\Approval\Ws\Status\ListProxy.asmx
		\DeskPlusEIP\Approval\Jt\Status\JTListProxy.htm
		\DeskPlusEIP\Approval\Js\Status\ListProxy.js
		\DeskPlusEIP\Approval\Js\Status\Menu\Tree.js


: 2015-10-07 by withsuki -----------------------------------------------------------
[전자결재] - 반려함
	DP_APP_FolderStatus 항목추가
	DP_APP_Cabinet 항목추가
	DP_APP_AppDoc 필드추가

	sp추가>
		DP_APP_DocProcess_UpdConcludeR
		DP_APP_DocProcess_SelPageListReject
		DP_APP_DocProcess_SelPageCntReject
		DP_APP_Doc_UpdIsConfirmR
		DP_APP_Doc_SelTreeReject
		DP_APP_Doc_SelPageListReject
		DP_APP_Doc_SelPageCntReject

	sp수정>
		DP_APP_Seq_UpdDecide
		DP_APP_DocProcess_SelPageListComplete
		DP_APP_DocProcess_SelPageCntComplete
		DP_APP_DocProcess_SelPageListProcess
		DP_APP_DocProcess_SelPageCntProcess

		DP_APP_Doc_SelTreeApp
		DP_APP_Doc_SelTreePrivate
		DP_APP_Doc_SelPageCntPrivate
		DP_APP_Doc_SelPageCntPrivateFolder
		DP_APP_Doc_SelPageCntPrivateSubmit
		DP_APP_Doc_SelPageListPrivate
		DP_APP_Doc_SelPageListPrivateFolder
		DP_APP_Doc_SelPageListPrivateSubmit
		DP_APP_Doc_SelPageCntApp
		DP_APP_Doc_SelPageCntAppFolder
		DP_APP_Doc_SelPageListApp
		DP_APP_Doc_SelPageListAppFolder
		DP_APP_DocProcess_SelPageListReceiveReject
		DP_APP_DocProcess_SelPageCntReceiveReject
		DP_APP_AppDoc_Ins
		DP_APP_AppDoc_InsTransfer

	소스>
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\ApprovalDocumentInfo.cs
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ApprovalDocument.cs
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs
		\DeskPlusEIP\Approval\Common\Folder\FolderDeptTreeScript.ascx
		\DeskPlusEIP\Approval\Common\Folder\GetFolderTreeScript.aspx
		\DeskPlusEIP\Approval\Common\Status\FolderStatusTreeScript.ascx
		\DeskPlusEIP\Approval\Folder\Approve\
		\DeskPlusEIP\Approval\Folder\Approve\
		\DeskPlusEIP\Approval\Folder\Approve\ListReject.aspx
		\DeskPlusEIP\Approval\Folder\Common\View.aspx
		\DeskPlusEIP\Approval\Folder\Menu\Tree.aspx
		\DeskPlusEIP\Approval\FormFlow\Default.aspx
		\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
		\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
		\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx
		\DeskPlusEIP\Approval\Js\Folder\Approve\ListReject.js
		\DeskPlusEIP\Approval\Js\Folder\Common\View.js
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
		\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeEdit.js
		\DeskPlusEIP\Approval\Js\Status\ListReject.js
		\DeskPlusEIP\Approval\Js\Status\Menu\Tree.js
		\DeskPlusEIP\Approval\Jt\Folder\Approve\JTListReject.htm
		\DeskPlusEIP\Approval\Jt\Status\JTListReject.htm
		\DeskPlusEIP\Approval\Resource\GlobalString\
		\DeskPlusEIP\Approval\Status\ListReject.aspx
		\DeskPlusEIP\Approval\Status\Menu\Tree.aspx
		\DeskPlusEIP\Approval\Ws\Folder\Approve\List.asmx.cs
		\DeskPlusEIP\Approval\Ws\Status\ListReject.asmx.cs
		\DeskPlusEIP\App_Data\APPROVAL.xml

: 2015-10-07 by famo23 -----------------------------------------------------------
[모바일관리] 설정값변경 처리
	\DeskPlusEIP\MDM\Config\Default.aspx
	\DeskPlusEIP\MDM\Config\Setting.aspx
	\DeskPlusEIP\MDM\Js\Config\Setting.js
	\DeskPlusEIP\MDM\Js\Menu\Menu.js
	\DeskPlusEIP\MDM\Menu\Menu.aspx
	\DeskPlusEIP\MDM\Ws\Config\Setting.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\ListInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\List.cs
	DP_MDM_List_UpdAll

: 2015-10-06 by famo23 -----------------------------------------------------------
[모바일관리] 승인사용자 상세화면 이동 처리
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Js\Auth\ListUse.js
	\DeskPlusEIP\MDM\Js\Dept\List.js
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js
	\DeskPlusEIP\MDM\Jt\Auth\JtListUse.htm
	\DeskPlusEIP\MDM\Ws\Auth\ListUse.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\ListInfo.cs
	DP_MDM_List_SelPageList

: 2015-10-06 by 방영환 -----------------------------------------------------------
[게시판] - 팝업 게시판(메인팝업 기능 추가)
	** 소스수정
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\AddN.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Mod.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\ModN.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\View.aspx.cs

	\DeskPlusSolution\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Content\AddN.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Content\ModN.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Content\View.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Content\ViewPopup.aspx.cs

	\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Add.js
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Mod.js
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Mod.js

	\DeskPlusSolution\DeskPlusEIP\Board\Ws\Content\Mod.asmx.cs

	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\ListEtc.cs

	\DeskPlusSolution\DeskPlusEIP\EIP\DeskPlusEPDefault.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\EIP\Package\Main.aspx.cs

	\DeskPlusSolution\DeskPlusEIP\EIP\Js\Package\Main.js

	** 소스추가
	\DeskPlusSolution\DeskPlusEIP\Board\Content\ViewPopupMain.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Common.js

	\DeskPlusSolution\DeskPlusEIP\Base\BoardMainPopup.aspx.cs

	** Resource
	\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx

	** 쿼리 추가
	QRY_BOA_ListEtc_SelMainPopupCount
	QRY_BOA_ListEtc_SelMainPopup


: 2015-10-06 by withsuki -----------------------------------------------------------
[업무일지] 버그수정 (띄어쓰기처리) - 보기부분과 작성(수정)부분이 다름
	\DeskPlusEIP\Report\Content\View.aspx.cs
	\DeskPlusEIP\Report\Content\ViewPT.aspx.cs

: 2015-10-04 by withsuki -----------------------------------------------------------
[디자인] - 포틀릿 높이 고정
	/DeskPlusEIP/Portlet/Package/ 이하 모두.
	/DeskPlusEIP/Portlet/Jt/ 이하 모두.
	/DeskPlusEIP/Portlet/Ws/Package/Weather.asmx
	/DeskPlusEIP/Design/C/Css/Func_Package.css

: 2015-10-05 by famo23 -----------------------------------------------------------
[모바일관리] 모바일승인, 불가처리 할때 알람처리 (처음에 인증하는 부분)
	DP_MDM_List_SelByListID
	\DeskPlusEIP\MDM\App_Data\
	\DeskPlusEIP\MDM\Ws\Auth\ListAdd.asmx.cs
	\DeskPlusEIP\MDM\Ws\Auth\ListReject.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\List.cs


: : 2015-10-02 by maniaysw -----------------------------------------------------------
[포틀릿] 이미지 게시판 추가
	
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\ListInfo.cs
	\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\Lists.cs

	\DeskPlusEIP\Portlet\Package\Board\NoticeBanner.ascx
	\DeskPlusEIP\Portlet\Package\Board\NoticeBanner.ascx.cs
	\DeskPlusEIP\Portlet\Ws\Package\Board\Album.asmx.cs
	\DeskPlusEIP\Portlet\Jt\Package\Board\JTNoticeBanner.htm

	-프로시저
	DP_BOA_List_SelPageNoticeList
	DP_BOA_LIST_SelPageNoticeCnt

: 2015-10-02 by maniaysw -----------------------------------------------------------
[주소록] 리스트 상단 개인 주소록 추가버튼

	\DeskPlusEIP\Address\Content\List.aspx
	\DeskPlusEIP\Address\Content\List.aspx.cs

	\DeskPlusEIP\Address\Content\AddPrivate.aspx
	\DeskPlusEIP\Address\Content\AddPrivate.aspx.cs

	\DeskPlusEIP\Address\Content\PrivateTree.aspx
	\DeskPlusEIP\Address\Content\PrivateTree.aspx.cs

	\DeskPlusEIP\Address\Content\ListBlog.aspx
	\DeskPlusEIP\Address\Content\ListBlog.aspx.cs

	\DeskPlusEIP\Address\Js\Content\AddPrivate.js
	\DeskPlusEIP\Address\Js\Content\ListBlog.js
	\DeskPlusEIP\Address\Js\Content\List.js
	\DeskPlusEIP\Address\Js\Content\PrivateTree.js


	\DeskPlusEIP\Address\Jt\Content\JTListPrivate.htm


: 2015-10-02 by famo23 -----------------------------------------------------------
[웹폴더] 전달받은 파일 전달자 보이기 처리
		
	DP_WHD_File 테이블 필드추가
	DP_WHD_File_InsByFID
	DP_WHD_ImpoUser_SelPageList

	리소스추가 SendUser
	\DeskPlusEIP\WebHard\Content\List.aspx
	\DeskPlusEIP\WebHard\Content\ListImpo.aspx
	\DeskPlusEIP\WebHard\Js\Content\List.js
	\DeskPlusEIP\WebHard\Js\Content\ListIcon.js
	\DeskPlusEIP\WebHard\Js\Content\ListImpo.js
	\DeskPlusEIP\WebHard\Js\Content\ListPreview.js
	\DeskPlusEIP\WebHard\Jt\Content\JTListIconSend.htm
	\DeskPlusEIP\WebHard\Jt\Content\JTListImpo.htm
	\DeskPlusEIP\WebHard\Jt\Content\JTListImpoIcon.htm
	\DeskPlusEIP\WebHard\Jt\Content\JTListImpoPreview.htm
	\DeskPlusEIP\WebHard\Jt\Content\JTListPreviewSend.htm
	\DeskPlusEIP\WebHard\Jt\Content\JTListSend.htm
	\DeskPlusEIP\WebHard\Ws\Content\List.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListIcon.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListImpo.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListImpoIcon.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListImpoPreview.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListPreview.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Send\Send.asmx.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.BE\FileInfo.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.BE\ListInfo.cs
	\Projects\WebHardProject\Zeospace.DeskPlus.WebHard.DAL\File.cs

: 2015-10-01 by withsuki -----------------------------------------------------------
[전자결재] 결재라인 변경화면의 상신자 고정 fix 처리
	\DeskPlusEIP\Approval\Js\FormFlow\Common\ModSequence.js
	\DeskPlusEIP\Approval\FormFlow\Common\ModSequence.aspx

: 2015-10-01 by famo23 -----------------------------------------------------------
[모바일관리] App을 자동승인할지 관리자가 승인할지 환경설정부분 개발 / Location 부분 제거
	리소스추가
	DP_MDM_Env테이블필드수정
	DP_MDM_List테이블필드수정
	DP_MDM_Env_Ins
	DP_MDM_List_Upd
	DP_MDM_List_UpdByListID
	\DeskPlusEIP\MDM\Config\Default.aspx
	\DeskPlusEIP\MDM\Config\Default.aspx.cs
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Js\Config\Default.js
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js
	\DeskPlusEIP\MDM\Ws\Config\Default.asmx.cs
	\DeskPlusEIP\MDM\Ws\Device\DeviceInfo.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\EnvInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\ListInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\Env.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\List.cs

: 2015-09-24 by shimpark -----------------------------------------------------------
[사용자관리] 사용자 관리의 활성화, 비활성화 부분 딥, 제타 연동 작업

C:\SiteProject\KOPLA\DeskPlusSolution\DeskPlusEIP\Account\Ws\Admin\User\List.asmx.cs
 : public bool ModActive(string p)
C:\SiteProject\KOPLA\DeskPlusSolution\DeskPlusEIP\Account\Ws\Admin\User\ListBlog.asmx.cs
 :public bool ModActive(string p)

: 2015-09-21 by freeman -----------------------------------------------------------
[공통]Microsoft Edge Browser에서 첨부파일 다운로드시 파일명이 깨지는 버그 수정
	\DeskPlusEIP\Common\DownLoad\DownLoad.aspx.cs  138 Line  header.IndexOf("Edge") > -1) 추가 

: 2015-09-15 by famo23 -----------------------------------------------------------
[커뮤니티]여러명 사용자 가입복귀, 등급변경, 강제탈퇴 할때 버그 수정
	DP_CMN_UserInfo_Upd
	DP_CMN_UserInfo_UpdByStatus

: 2015-09-11 by shimpark -----------------------------------------------------------
[주소 5자리]

  \DeskPlusEIP\Account\Js\Admin\Dept\List.js(395):    new daum.Postcode({
  \DeskPlusEIP\Account\Js\Admin\User\Add.js(270):    new daum.Postcode({
  \DeskPlusEIP\Account\Js\Admin\User\Add.js(324):    new daum.Postcode({
  \DeskPlusEIP\Account\Js\Admin\User\Mod.js(326):    new daum.Postcode({
  \DeskPlusEIP\Account\Js\Admin\User\Mod.js(381):    new daum.Postcode({
  \DeskPlusEIP\Account\Js\MyInfo\Mod.js(271):    new daum.Postcode({
  \DeskPlusEIP\Account\Js\MyInfo\Mod.js(325):    new daum.Postcode({
  \DeskPlusEIP\EIP\Js\Admin\Company\Add.js(309):    new daum.Postcode({
  \DeskPlusEIP\EIP\Js\Admin\Subsidiary\Add.js(216):    new daum.Postcode({
  \DeskPlusEIP\Address\Js\Admin\Content\Add.js(201):    new daum.Postcode({
  \DeskPlusEIP\Address\Js\Admin\Content\Add.js(255):    new daum.Postcode({
  \DeskPlusEIP\Address\Js\Content\Add.js(203):    new daum.Postcode({
  \DeskPlusEIP\Address\Js\Content\Add.js(258):    new daum.Postcode({
  \DeskPlusEIP\Approval\Js\Admin\EnforceSeal\Add.js(316):    new daum.Postcode({
  \DeskPlusEIP\Approval\Js\Admin\EnforceSeal\Mod.js(320):    new daum.Postcode({
  \DeskPlusEIP\Approval\Js\Folder\Enforce\Add.js(66):    new daum.Postcode({
  \DeskPlusEIP\Approval\Js\Folder\Enforce\Mod.js(48):    new daum.Postcode({

  \DeskPlusEIP\Account\Admin\Dept\List.aspx(20):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Account\Admin\User\Add.aspx(21):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Account\Admin\User\Mod.aspx(20):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Account\MyInfo\Mod.aspx(20):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\EIP\Admin\Company\Add.aspx(25):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\EIP\Admin\Subsidiary\Add.aspx(20):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Address\Admin\Content\Add.aspx(23):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Address\Content\Add.aspx(21):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Approval\Admin\EnforceSeal\Add.aspx(20):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Approval\Admin\EnforceSeal\Mod.aspx(21):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Approval\Folder\Enforce\Add.aspx(20):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
  \DeskPlusEIP\Approval\Folder\Enforce\Mod.aspx(20):    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>

: 2015-09-11 by famo23 -----------------------------------------------------------
[전자결재] 필요없는 구문 주석처리
	DP_APP_Seq_UpdDecide

: 2015-08-26 by famo23 -----------------------------------------------------------
[설문조사] 목록버튼 선택후 왼쪽 메뉴가 이중으로 생기는 버그
	\DeskPlusEIP\Opinion\Js\Content\Result.js

: 2015-08-20 by famo23 -----------------------------------------------------------
[자원관리] psk 소스를 2014년 12월 03일 merge 한거로 보임. 해당 부분 수정함
	\DeskPlusEIP\RM\Js\Content\Add.js
	\DeskPlusEIP\RM\Js\Content\Mod.js

: 2015-08-17 by famo23 -----------------------------------------------------------
[목표관리] 첨부파일 없을때 리스트 없다는 내용 처리
	\DeskPlusEIP\Pim\Js\Target\Add.js
	\DeskPlusEIP\Pim\Js\Target\List.js
	\DeskPlusEIP\Pim\Js\Target\SubAdd.js
	\DeskPlusEIP\Pim\Jt\Target\JTList.htm
	
: 2015-08-13 by famo23 -----------------------------------------------------------
[주소록] 리스트 내용 없을때 헤더 갯수 안 맞는 버그
	\DeskPlusEIP\Address\Jt\Content\JTList.htm

: 2015-08-13 by withsuki -----------------------------------------------------------
[로그인] 로그인화면 추가 및 로그인페이지 설정 (10개)
	\DeskPlusEIP\Site\Auth\Default.aspx
	\DeskPlusEIP\Site\Auth\Default1.aspx
	\DeskPlusEIP\Site\Auth\Default2.aspx
	\DeskPlusEIP\Site\Auth\Default3.aspx
	\DeskPlusEIP\Site\Auth\Default4.aspx
	\DeskPlusEIP\Site\Auth\Default5.aspx
	\DeskPlusEIP\Site\Auth\Default6.aspx
	\DeskPlusEIP\Site\Auth\Default7.aspx
	\DeskPlusEIP\Site\Auth\Default8.aspx
	\DeskPlusEIP\Site\Auth\Default9.aspx
	\DeskPlusEIP\Site\Auth\Default10.aspx
	\DeskPlusEIP\Js\Site\Auth\Default.js
	\DeskPlusEIP\App_Data\Config.xml
	\\192.168.1.20\DeskPlus40\FileData\Dev\1\Login

: 2015-08-13 by withsuki -----------------------------------------------------------
[커뮤니티] 본문내용 스크롤 처리
	\DeskPlusEIP\Community\Site\Apps\Board\View.aspx
	\DeskPlusEIP\Community\Site\Apps\Board\ViewPopup.aspx

: 2015-08-12 by famo23 -----------------------------------------------------------
[설문조사] 취소버튼 선택후 왼쪽 메뉴가 이중으로 생기는 버그
	\DeskPlusEIP\Opinion\Js\Content\Vote.js

[설문조사] 등록일 칸 늘리기
	\DeskPlusEIP\Opinion\Jt\Admin\Content\JtUnitList.htm

: 2015-08-10 by withsuki -----------------------------------------------------------
[포틀릿] 보낸편지함 포틀릿 : 리스트가 나오지 않음
	\DeskPlusEIP\Portlet\Jt\Package\Mail\JTSendList.htm


: 2015-08-05 by withsuki -----------------------------------------------------------
[전자결재] 결재문서-관리자 : 결재문서 삭제 후, 리스트 처리 수정. 
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js


: 2015-08-05 by famo23 -----------------------------------------------------------
[전자결재] 재상신할때 첨언테이블 값 변경 안되는 버그 
	DP_APP_ShortReply_Upd
	DP_APP_DocProcess_DelMod
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ShortReply.cs
	
: 2015-08-04 by famo23 -----------------------------------------------------------
[전자결재] 수신담당자변경했을 때 알람처리
	DP_APP_Counterpartuser_SelAlarm
	\DeskPlusEIP\Approval\App_Data\Confirm\en.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\fr.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\ja.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\ko.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\zh-cn.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\zh-tw.xml
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

[전자결재] 참조부서지정시에 알람 안 오는 버그
	DP_APP_Seq_SelAlarmComplete
	DP_APP_Seq_SelAlarmOpinionAll
	DP_APP_Seq_SelAlarmRejectAll
	DP_APP_Seq_SelAlarmNextAll

[전자결재] 결재자, 수신자 본문/결재선 수정할때 결재의견 DP_APP_ShortReply 테이블에 안 들어가는 버그
	DP_APP_History_Ins

: 2015-08-03 by famo23 -----------------------------------------------------------
[전자결재] ZQ수신있는 문서는 후열문서확인할때 [후열]표시가 다른 위치로 잘못 들어가는 버그
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx

: 2015-07-28 by famo23 -----------------------------------------------------------
[전자결재] 결재정보에서 참조자가 부서일때 // 나오는 버그, (이하)가 안 나오는 버그
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTDocUserSeqCA.htm
	\DeskPlusEIP\Approval\Jt\Status\Common\JTDocUserSeqCA.htm
	\DeskPlusEIP\Approval\Ws\Folder\Common\DocUserSeq.asmx.cs
	\DeskPlusEIP\Approval\Ws\Status\Common\DocUserSeq.asmx.cs

[전자결재] 결재취소 다국어 처리 안된 버그 
	\DeskPlusEIP\Approval\Status\ListRelated.aspx
	\DeskPlusEIP\Approval\Js\Status\ListRelated.js

[전자결재] 수신자 담당자 변경에서 부서 체크박스 제거
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

[전자결재] 수신자 담당자 변경에서 조직도 검색으로 사용자 선택시 이름에 부서명, 직급명까지 나오는 버그 
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

[전자결재] 참조문서함 문서양식트리가 부서로 지정되어 있는 문서에는 안 나오는 버그
	\DeskPlusEIP\Approval\Common\Folder\GetFolderTreeScript.aspx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	DP_APP_Doc_SelTreeCarbonCopy	

[전자결재] 사인생성 버그
	\DeskPlusEIP\Approval\Ws\Common\Popup\GetSign.asmx.cs
	
[모바일관리] 모바일관리 알람설정 제거
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js
	DP_MDM_List_Upd

[커뮤니티] 커뮤니티게시판 관련테이블 외래키제거

[커뮤니티] 커뮤니티바로가기로 갔을 때 방문횟수, 포인트 안 쌓이는 버그
	\DeskPlusEIP\Community\Js\Site\Main\Menu.js
	\DeskPlusEIP\Community\Site\Main\Menu.aspx
	\DeskPlusEIP\Community\Ws\Site\Main\Menu.asmx.cs

[커뮤니티]게시판 글 지울 때 답변글 안 지워지는 버그 
	DP_CMN_Boa_Del
	DP_CMN_Boa_DelAll
	QRY_CMN_BoaAttach_SelAll
	QRY_CMN_BoaAttach_SelFilePath
	\DeskPlusEIP\Community\Site\Apps\Board\View.aspx.cs
	\Projects\CommunityProject\Zeospace.DeskPlus.Community.BLL\Boa.cs
	\Projects\CommunityProject\Zeospace.DeskPlus.Community.DAL\Boa.cs
	\Projects\CommunityProject\Zeospace.DeskPlus.Community.DAL\BoaAttach.cs

[커뮤니티] 게시판 글 정렬 수정
	DP_CMN_Boa_SelPageList

[커뮤니티] 등록일 칸 늘리기
	\DeskPlusEIP\Community\Jt\Site\Apps\Board\JTShortReplyList.htm


: 2015-07-27 by famo23 -----------------------------------------------------------
[업무톡-결재톡] 수신부서 첨언은 체크 안되는 버그 
	DP_APP_DocProcess_Ins
	DP_APP_ShortReply_Ins
	DP_SNS_Work_SelAppPageCnt
	DP_SNS_Work_SelAppPageList
	VW_APP_DocTot

[전자결재] 투데이 상신에서 수신대기카운트는 안나오는 버그
	DP_PT_MyInfo_SelUserID

[전자결재] 임시저장한 문서에서 참조자들이 안나오는 버그
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

[전자결재] 자주지정하는 결재자리스트가 에디터뒤로 숨는 버그
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

: 2015-07-24 by famo23 -----------------------------------------------------------
[전자결재] 수신문서 상신할때 스타일 변경 안되는 버그 수정
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs

[전자결재] 결재취소하고 본문/결재선수정 할때 결재정보확인에서 본인이 안나오는 버그
	DP_APP_Seq_SelByLineNonApp

[전자결재] 반려대상자에서 병렬은 라디오버튼 비활성화 안되는 버그
	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx.cs

[커뮤니티] 커뮤니티지기 이름에 개설자 이름 나오는 버그 수정
	QRY_CMN_Com_SelByCommunityID

[커뮤니티] 회원정보에 퇴사자 안 나오게 처리
	QRY_CMN_Com_SelByCommunityID
	QRY_CMN_ComEtc_Sel
	QRY_CMN_UserInfo_Sel
	DP_CMN_UserInfo_SelPageCnt
	DP_CMN_UserInfo_SelPageCntByCmnID
	DP_CMN_UserInfo_SelPageCntBySys
	DP_CMN_UserInfo_SelPageList
	DP_CMN_UserInfo_SelPageListByCmnID
	DP_CMN_UserInfo_SelPageListBySys
	DP_CMN_Com_SelPageList

[커뮤니티] 이슈정렬변경
	QRY_CMN_BoaIssue_Sel

[커뮤니티] 댓글정렬변경
	QRY_CMN_BoaShortReply_Sel
	QRY_CMN_BoaShortReply_SelByCID

[커뮤니티] 퀴즈 질문, 답변 안 나오는 버그
	\DeskPlusEIP\Community\Site\Admin\General\Info.aspx.cs

[커뮤니티] 퇴사자이름 안 보이는 문제처리
	DP_CMN_Com_SelPageList
	\DeskPlusEIP\Community\Site\Apps\Board\View.aspx.cs
	\DeskPlusEIP\Community\Site\Apps\Board\ViewPopup.aspx.cs
	\DeskPlusEIP\Community\Site\Main\Content.aspx.cs
	\DeskPlusEIP\Community\Site\Main\Join.aspx.cs
	\DeskPlusEIP\Community\Ws\Admin\Site\ListClose.asmx.cs
	\DeskPlusEIP\Community\Ws\Main\Content.asmx.cs
	\DeskPlusEIP\Community\Ws\Site\Apps\Board\List.asmx.cs
	\DeskPlusEIP\Community\Ws\Site\Apps\Board\ListGallery.asmx.cs
	\DeskPlusEIP\Community\Ws\Site\Apps\Board\ReadList.asmx.cs
	\DeskPlusEIP\Community\Ws\Site\Apps\Board\RecomList.asmx.cs
	\DeskPlusEIP\Community\Ws\Site\Apps\Board\ShortReplyList.asmx.cs
	\DeskPlusEIP\Community\Ws\Site\Apps\Board\View.asmx.cs
	\DeskPlusEIP\Community\Ws\Site\Main\Content.asmx.cs

[커뮤니티] 회원가입방식 잘못 들어가는 버그
	\DeskPlusEIP\Community\Ws\Admin\Site\ListAdd.asmx
	\Projects\CommunityProject\Zeospace.DeskPlus.Community.DAL\Com.cs
	\Projects\CommunityProject\Zeospace.DeskPlus.Community.DAL\ComApply.cs	

[커뮤니티]게시판 답변 첨부파일 등록시 글 등록 안되는 버그
	\DeskPlusEIP\Community\Site\Apps\Board\Reply.aspx.cs

[자원관리]QRCode 경로 잘못 들어간 버그
	DP_RM_List_Ins
	\DeskPlusEIP\RM\Admin\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Admin\Content\Mod.aspx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ListInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Attach.cs

[자원관리]일간 자원담당자 상세보기 할때 화면 짤리는 버그
	\DeskPlusEIP\RM\Content\ListDay.aspx
	\DeskPlusEIP\RM\Content\WholeDay.aspx
	
[자원관리]참석자 기능 버그로 우선 안 보이게 처리
	\DeskPlusEIP\RM\Admin\Content\Add.aspx
	\DeskPlusEIP\RM\Admin\Content\Mod.aspx
	\DeskPlusEIP\RM\Admin\Content\View.aspx
	\DeskPlusEIP\RM\Content\Add.aspx
	\DeskPlusEIP\RM\Content\AddN.aspx
	\DeskPlusEIP\RM\Content\Mod.aspx
	\DeskPlusEIP\RM\Content\ModN.aspx
	\DeskPlusEIP\RM\Js\Content\Add.js
	\DeskPlusEIP\RM\Js\Content\Mod.js

[업무톡-결재톡] 등록자 상세보기에 상신자 정보 나오는 버그
	\DeskPlusEIP\SNS\Jt\WorkTalk\Content\JTShortAppList.htm

: 2015-07-23 by famo23 -----------------------------------------------------------
[전자결재] 수신문서 결재자 없이 완결처리 되게 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs 1989

: 2015-07-21 by amk0601 -----------------------------------------------------------
[로그인 페이지] 새로운 디자인 적용
	\DeskPlusEIP\Site\Auth\Default1.aspx
	\DeskPlusEIP\Site\Auth\Default2.aspx
	\DeskPlusEIP\Site\Auth\Default3.aspx
	\DeskPlusEIP\Site\Auth\Default4.aspx
	\DeskPlusEIP\Site\Auth\Default5.aspx
	\DeskPlusEIP\Site\Auth\Default6.aspx
	\DeskPlusEIP\Site\Auth\Default7.aspx
	\DeskPlusEIP\Site\Auth\Default8.aspx
	\DeskPlusEIP\Site\Auth\Default9.aspx
	\DeskPlusEIP\Site\Auth\Default10.aspx

	\DeskPlusEIP\Resource\GlobalString\string.ko.resx

: 2015-07-16 by shimpark -----------------------------------------------------------
[게시물 관리자 이동] 

C:\SiteProject\Koramco\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Move.aspx
C:\SiteProject\Koramco\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Move.js

: 2015-07-14 by withsuki -----------------------------------------------------------
[전자결재] 버그 - 커스텀연동 호출시점 버그수정 (_Cancel.aspx)
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js


: 2015-07-08 by freeman -----------------------------------------------------------
[공통] 다국어 OS 에서 언어 설정을 않했을시 한국어로 표시되는문제
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\Authenticator.cs 136


: 2015-06-25 by freeman -----------------------------------------------------------
[일정관리] 구글 캘린더 연동

	\DeskPlusEIP\Pim\GoogleCalendar\*.*

	\DeskPlusEIP\Pim\Js\Schedule\ScheduleMain.js
	\DeskPlusEIP\Pim\Js\Schedule\SyncCalendar\Default.js

	\DeskPlusEIP\Pim\Resource\GlobalString\*.*

	\DeskPlusEIP\Pim\Schedule\SyncCalendar\*.*

	\DeskPlusEIP\Pim\Schedule\ScheduleMain.aspx

	\DeskPlusEIP\Pim\Schedule\LinkPim.aspx 삭제
	\DeskPlusEIP\Pim\Ws\Schedule\LinkPim.asmx.cs

	\DeskPlusEIP\Pim\packages.config
	\DeskPlusEIP\Pim\web.config

	\DeskPlusEIP\web.config

	\DeskPlusEIP\WebAppSettings.config
	

	* 삭제
	\DeskPlusSolution\DeskPlusEIP\Pim\Schedule\LinkPim.*

	* SP 삭제
	  - DP_PIM_GOOGLE_GETUSER

	* SP 명칭변경
	  - DP_PIM_GOOGLE_SYNC ==> DP_PIM_GoogleCalendar_Sync
	  - DP_PIM_TEMP_GOOGLE_DEL ==> DP_PIM_GoogleCalendarTemp_Del
	  - DP_PIM_TEMP_GOOGLE_DELAll  ==> DP_PIM_GoogleCalendarTemp_DelAll
	  - DP_PIM_GoogleUserGet ==> DP_PIM_GoogleAccount_Get
  
	* Table 명칭변경
	  - TEMP_GOOGLE  ==> DP_PIM_GoogleCalendarTemp

	* 추가 
	  QRY_PIM_GoogleCalendar_SelLatest

: 2015-06-24 by famo23 -----------------------------------------------------------
[업무협업] 접수하기, 완료하기 할때 첨부파일등록 버그
	\Projects\TaskProject\Zeospace.DeskPlus.Task.BLL\List.cs

: 2015-06-16 by shimpark -----------------------------------------------------------

     : 결재 게시판 이관 후, 게시판에서 첨부파일 다운로드 할때 확장자 누락 버그 fix
       \DeskPlusEIP\Board\Common\TransferAppoval.aspx.cs

: 2015-06-12 by freeman -----------------------------------------------------------
* SNS 정보관리 : 패스워드 항목 삭제 me2day 삭제(서비스종료)
	\DeskPlusEIP\Account\MyInfo\SNS.aspx *.*
	\DeskPlusEIP\Account\Js\MyInfo\SNS.js

: 2015-06-05 by freeman -----------------------------------------------------------
* IP 대역에 따른 접근 권한 설정 (특정 사용자만 하용 기능 (영업요청 테크로스)
  \DeskPlusEIP\App_Data\Config.xml      AllowUserIPAccess 
  \DeskPlusEIP\Site\Auth\Default.aspx.cs
  \DeskPlusEIP\Resource\GlobalString\string.en.resx  AccessNoAuthIP

  \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\IPListHelper.cs

   관리자 도구
   1. 코드관리 : 접근허용IP리스트(AllowHostIP) 그룹 추가한후 접근이 가능한 사내 IP목록을 외부연동코드에 추가 ex (192.168.1.1-192.168.1.254  , 59.150.252.150)
   2. 권한관리 : 사외접근허용사용자(allowUserIPAccess) 추가 하면 외부 IP에서도 접속이 가능한 사용자 등록 가능


: 2015-05-29 by 방영환 -----------------------------------------------------------
[사용자관리 PDF일괄 다운로드 추가]
** 소스수정
\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Mod.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\View.aspx
\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\View.cs
\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\View.js
\DeskPlusSolution\DeskPlusEIP\Board\Content\Add.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Board\Content\Mod.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Board\Content\View.aspx
\DeskPlusSolution\DeskPlusEIP\Board\Content\View.cs
\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\View.js
\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Add.js
\DeskPlusSolution\DeskPlusEIP\Board\Ws\Admin\Content\List.asmx
\DeskPlusSolution\DeskPlusEIP\Board\SetBoard.asmx

\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\Lists.cs

\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx.cs

\DeskPlusSolution\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
\DeskPlusSolution\DeskPlusEIP\Approval\Admin\Document\ViewDoc.cs
\DeskPlusSolution\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx
\DeskPlusSolution\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.cs
\DeskPlusSolution\DeskPlusEIP\Approval\Folder\Common\View.aspx
\DeskPlusSolution\DeskPlusEIP\Approval\Folder\Common\View.cs
\DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
\DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
\DeskPlusSolution\DeskPlusEIP\Approval\Js\Folder\Common\View.js
\DeskPlusSolution\DeskPlusEIP\Approval\Js\Status\Common\View.js
\DeskPlusSolution\DeskPlusEIP\Approval\Status\Common\View.aspx
\DeskPlusSolution\DeskPlusEIP\Approval\Status\Common\View.cs

\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

\DeskPlusSolution\DeskPlusEIP\App_Data\COMMON.xml
\DeskPlusSolution\DeskPlusEIP\App_Data\APPROVAL.xml
\DeskPlusSolution\DeskPlusEIP\App_Data\BOARD.xml

** 소스추가
\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\CreatePDF.cs
\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\PDFInfo.cs
\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\CreatePDF.cs
\Projects\CommonProject\Zeospace.DeskPlus.Common.DAL\CreatePDF.cs
\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\CreatePDF.cs

\DeskPlusSolution\DeskPlusEIP\Common\DownLoad\PDFDownLoad.aspx

\DeskPlusSolution\DeskPlusEIP\Common\PDF\PDFCreate.aspx
\DeskPlusSolution\DeskPlusEIP\Common\PDF\PDFDownLoad.aspx

\DeskPlusSolution\DeskPlusEIP\Common\Ws\PDF\PDF.asmx

** Resource
\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx

\DeskPlusEIP\Board\Resource\GlobalString\string.en.resx
\DeskPlusEIP\Board\Resource\GlobalString\string.ko.resx
\DeskPlusEIP\Board\Resource\GlobalString\string.zh-tw.resx
\DeskPlusEIP\Board\Resource\GlobalString\string.zh-cn.resx
\DeskPlusEIP\Board\Resource\GlobalString\string.ja.resx

\DeskPlusEIP\Common\Resource\GlobalString\string.en.resx
\DeskPlusEIP\Common\Resource\GlobalString\string.ko.resx
\DeskPlusEIP\Common\Resource\GlobalString\string.zh-tw.resx
\DeskPlusEIP\Common\Resource\GlobalString\string.zh-cn.resx
\DeskPlusEIP\Common\Resource\GlobalString\string.ja.resx

** 테이블 추가
DP_COM_PDFLog

** 저장프로시저 추가
DP_COM_PDFLog_Ins
DP_COM_PDFLog_Del

** 저장프로시저 수정
DP_APP_Doc_Del
DP_APP_DocProcess_Del

** 쿼리 추가
INSERT INTO dp_query (category, qryid, qryvalue, owner, memo, regdate) values('COMMON', 'QRY_COM_PDFLog_Sel', ' SELECT * FROM DP_COM_PDFLog WHERE CompanyID=@CompanyID AND Module = @Module AND ListID = @ListID ', '방영환', 'PDF로그 가져오기', '20150421')


: 2015-05-28 by shimpark -----------------------------------------------------------
[메인탭] HOME 버튼 누를 경우, 권한별 첫번째 탭에 설정하기

	\DeskPlusEIP\EIP\Package\Menu.aspx.cs

: 2015-05-21 by famo23 -----------------------------------------------------------
[업무협업] 업무협업 개발

	\DeskPlusEIP\App_Data\TASK.xml

	\DeskPlusEIP\Portlet\리소스
	\DeskPlusEIP\Portlet\Js\Package\MyInfo.js
	\DeskPlusEIP\Portlet\Jt\Package\JTMyInfo.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTForward.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTReceive.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTReference.htm
	\DeskPlusEIP\Portlet\Jt\Package\Task\JTRequest.htm
	\DeskPlusEIP\Portlet\Ws\Package\MyInfo.asmx.cs
	\DeskPlusEIP\Portlet\Ws\Package\Task\List.asmx.cs
	\DeskPlusEIP\Portlet\Package\TabTaskMain.ascx.cs
	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\Task.cs

	\DeskPlusEIP\EIP\Js\Package\Menu.js
	\DeskPlusEIP\EIP\Jt\Package\JTTodayInfo.htm

	\DeskPlusEIP\MDM\리소스
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js

	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\AppSettingHelper.cs
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\FileHelper.cs

	Task프로젝트추가

	DB = DP_TAS_Cabinet 기본값 
	     DP_COM_Env 값 입력
		 DP_TAS_테이블 , 프로시저 , 뷰

		 INSERT INTO [DP_MDM_Menu]
           ([CompanyID],[GubunID],[NameBase],[NameKo],[NameEn],[NameJa],[NameFr],[NameZhcn],[NameZhtw])
		 VALUES
           (1,10,'업무협업','업무협업','Business','業務提携','업무협업','???同','業務協同')

		INSERT INTO [dbo].[DP_COM_SYSCode]
           ([CompanyID]  ,[InterfaceCode]
           ,[CodeID],[ParentCodeID]
           ,[NameBase],[NameKo],[NameEn],[NameJa],[NameFr],[NameZhcn],[NameZhtw],[ManyName]
           ,[ActiveCode],,[Description],[WriteUserID],[WriteDate])
        VALUES
           (1,'','TabTaskMain','PORTLET','30'
           ,'','','','','','','30||||||',1,'',1,GETDATE())

	디자인 - Func_Task.css / 이미지폴더
	
	스케줄러 (DP_TAS_ListOption_UpdScheduler)
	Today포틀릿 DP_PT_MyInfo_SelUserID
	업무협업포틀릿  DP_PT_Task_xxx
	알람 (DP_MDM_List_Upd / SQL_DP_ALARM_Common_uc)


: 2015-04-29 by withsuki -----------------------------------------------------------
[전자결재] - PDF일괄다운로드 (폴더생성)
	\DeskPlusEIP\Common\Zip\PrintBulk.aspx.cs

: 2015-04-29 by withsuki -----------------------------------------------------------
[전자결재] - 대결자 알람발송
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserInfo.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs
	\DeskPlusEIP\Approval\App_Data\Proxy\ko.xml
	\DeskPlusEIP\Approval\Js\Preference\Proxy\List.js
	\DeskPlusEIP\Approval\Jt\Preference\Proxy\JTList.htm
	\DeskPlusEIP\Approval\Preference\Proxy\List.aspx
	\DeskPlusEIP\Approval\Ws\Preference\Proxy\List.asmx
	\DeskPlusEIP\App_Data\APPROVAL.xml
	\DeskPlusEIP\Approval\Resource\GlobalString\	

	** DB - VW_UserInfo수정

: 2015-04-29 by freeice -----------------------------------------------------------
[메신저-메일카운트] Asp -> Aspx 컨버전
	\DeskPlusEIP\Etc\Mail\MailCount.aspx
	\DeskPlusEIP\Etc\Mail\MailCount.aspx.cs

    Etc Project에 Common.Bll 추가
	Web.Config에 NoAuth페이지 추가


: 2015-04-28 by freeman -----------------------------------------------------------
[Security] SHAR512 Hash Function 추가 
	\DeskPlusEIP\App_Data\Config.xml
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Security\HashManager.cs

	\DeskPlusEIP\Util\Enc\HASHTest.aspx
	\DeskPlusEIP\Util\Enc\HASHTest.aspx.cs
	

: 2015-04-20 by freeman -----------------------------------------------------------
[Component] OFFICE / PDF 변환 컴포넌트 최신 버전 및 라이센스 적용
	 \Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\AsposeEmail.cs
	 \Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\AsposeLicense.cs
     \Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\Aspose.Total.lic
	 \Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\AsposePPT.cs
	 \Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\SlideToText.cs

	 * 참조

	 * Search 프로젝트

: 2015-04-23 by shimpark -----------------------------------------------------------

[SMS 문자] - 80 자리 잘라주기

	\DeskPlusEIP\Portlet\Js\Package\SMS\AddressBook.js
	\DeskPlusEIP\Portlet\Js\Package\SMSNew\SendSMS.js

	\DeskPlusEIP\Portlet\Jt\Package\SMS\JTList.htm
	\DeskPlusEIP\Portlet\Jt\Package\SMSNew\JTList.htm
	\DeskPlusEIP\Portlet\Jt\Package\SMSNew\JTRecentNumber.htm

	\DeskPlusEIP\Portlet\Package\SMS\AddressBook.aspx
	\DeskPlusEIP\Portlet\Package\SMSNew\SendSMS.aspx

	\DeskPlusEIP\Portlet\Package\SMSNew\Sample\simple.css

	\DeskPlusEIP\Portlet\Ws\Package\SMSNew\SendSMS.asmx.cs

	: 슈어엠 - Surem 설정
	C:\SiteProject\Koramco\DeskPlusSolution\DeskPlusEIP\App_Data\SMS.xml

	: 글자 80 자리 분리작업
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\UtilHelper.cs


: 2015-03-20 by famo23 -----------------------------------------------------------
[마이톡] 첨부파일(이미지파일)추가 기능개발
	리소스추가
	DP_SNS_Attach 테이블추가
	DP_SNS_MyTalk_SelPageList
	DP_SNS_MyTalk_SelSharePageList	
	DP_SNS_MyTalk_SelGroupList	
	DP_SNS_MyTalk_Ins
	DP_SNS_MyTalk_DelByGroupID	
	DP_SNS_Attach_Ins
	QRY_SNS_Attach_Sel
	\DeskPlusEIP\SNS\Common\MyTalkGroup.aspx
	\DeskPlusEIP\SNS\Common\MyTalkGroup.aspx.cs
	\DeskPlusEIP\SNS\MyTalk\Content\List.aspx
	\DeskPlusEIP\SNS\MyTalk\Content\List.aspx.cs
	\DeskPlusEIP\SNS\MyTalk\Content\ShareList.aspx
	\DeskPlusEIP\SNS\MyTalk\Content\ShareList.aspx.cs
	\DeskPlusEIP\SNS\Js\Common\MyTalkGroup.js
	\DeskPlusEIP\SNS\Js\MyTalk\Content\List.js
	\DeskPlusEIP\SNS\Js\MyTalk\Content\ShareList.js
	\DeskPlusEIP\SNS\Jt\Common\JTMyTalk.htm
	\DeskPlusEIP\SNS\Jt\Common\JTMyTalkGroup.htm
	\DeskPlusEIP\SNS\Jt\MyTalk\Content\JTList.htm
	\DeskPlusEIP\SNS\Jt\MyTalk\Content\JTShareList.htm
	\DeskPlusEIP\SNS\Jt\MyTalk\Content\JTSpoolFile.htm
	\DeskPlusEIP\SNS\Ws\Common\MyTalk.asmx
	\DeskPlusEIP\SNS\Ws\Common\MyTalkGroup.asmx
	\DeskPlusEIP\SNS\Ws\MyTalk\Content\List.asmx
	\DeskPlusEIP\SNS\Ws\MyTalk\Content\ShareList.asmx
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BE\AttachInfo.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BE\MyTalkInfo.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\Attach.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.BLL\MyTalk.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.DAL\Attach.cs
	\Projects\SNSProject\Zeospace.DeskPlus.SNS.DAL\MyTalk.cs

	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\FileHelper.cs
	\DeskPlusEIP\Portlet\Jt\Package\JTMyCommentList.htm
	\DeskPlusEIP\Portlet\Ws\Package\MyComment.asmx.cs

: 2015-03-19 by amk0601 -----------------------------------------------------------
[자원관리-참석자] 참석자 추가시 알람(메신저,메일,SMS)
	\DeskPlusEIP\RM\Ws\Content\ListDay.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\ListWeek.asmx.cs

	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs

: 2015-03-17 by freeman -----------------------------------------------------------
[공통-버그] 브라우져에서 패키지가 제공하지 않는 언어 리소스 사용시 문제 ==> userLang.StartsWith("en") || userLang.StartsWith("fr") || userLang.StartsWith("ja") || userLang.StartsWith("ko") || userLang.StartsWith("zh")
	\DeskPlusEIP\Site\Auth\Default.aspx.cs(50)
	\DeskPlusEIP\Site\Auth\Default.aspx.cs(73)
	\DeskPlusEIP\Site\Auth\ModPassword.aspx.cs(42)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\Authenticator.cs(161)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBaseControl.cs(121)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBaseControl.cs(146)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBasePage.cs(125)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBasePage.cs(151)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBaseService.cs(121)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentMasterPage.cs(119)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentMasterPage.cs(145)
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\UserInfoHelper.cs(150)
	\DeskPlusEIP\Address\Common\AddressMailTree.ascx.cs(28)
	\DeskPlusEIP\Address\Common\GetAddressMailTree.aspx.cs(41)
	\DeskPlusEIP\Address\Mail\AddressAdd.aspx.cs(47)
	\DeskPlusEIP\Address\Mail\AddressSearch.aspx.cs(49)
	\DeskPlusEIP\Approval\Common\Popup\View.aspx.cs(38)
	\DeskPlusEIP\RM\Content\ViewQRCode.aspx.cs(36)
	\DeskPlusEIP\RM\Content\ViewQRCode.aspx.cs(49)


: 2015-03-12 by amk0601 -----------------------------------------------------------
[자원관리-참석자] 참석자 추가시 부서선택 가능
	\DeskPlusEIP\RM\Content\Add.aspx
	\DeskPlusEIP\RM\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Content\AddN.aspx
	\DeskPlusEIP\RM\Content\AddReserveUser.aspx
	\DeskPlusEIP\RM\Content\AddReserveUser.aspx.cs
	\DeskPlusEIP\RM\Content\Mod.aspx
	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Content\ModN.aspx
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs

	\DeskPlusEIP\RM\Js\Content\Add.js
	\DeskPlusEIP\RM\Js\Content\AddReserveUser.js
	\DeskPlusEIP\RM\Js\Content\Mod.js
	\DeskPlusEIP\RM\Js\Content\View.js

	\DeskPlusEIP\RM\Jt\Content\JTReserveUser.htm

	\DeskPlusEIP\RM\Ws\Content\Add.asmx
	\DeskPlusEIP\RM\Ws\Content\Mod.asmx
	\DeskPlusEIP\RM\Ws\Content\View.asmx

	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ReserveUserInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\ReserveUser.cs

: 2015-03-04 by amk0601 -----------------------------------------------------------
[자원관리-참석자] 자원추가시 참석자기능 사용여부 선택
	\DeskPlusEIP\RM\Admin\Content\Add.aspx
	\DeskPlusEIP\RM\Admin\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Admin\Content\Mod.aspx
	\DeskPlusEIP\RM\Admin\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Admin\Content\View.aspx
	\DeskPlusEIP\RM\Admin\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\Add.aspx
	\DeskPlusEIP\RM\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Content\AddN.aspx
	\DeskPlusEIP\RM\Content\Mod.aspx
	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Content\ModN.aspx
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\View.aspx.cs

	\DeskPlusEIP\RM\Js\Content\Add.js
	\DeskPlusEIP\RM\Js\Content\Mod.js
	\DeskPlusEIP\RM\Js\Content\View.js

	\DeskPlusEIP\RM\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-tw.resx

	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ListInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\List.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\List.cs 

	DP_RM_List 컬럼추가, 컬럼명:IsReserveUser(char(1))
	뷰 수정 : VW_RM_List, VW_RM_ReserveList
	저장프로시저 수정 : DP_RM_List_Ins, DP_RM_List_UpdByID

: 2015-02-27 by shimpark -----------------------------------------------------------
[업무협업] 요청한 업무, 요청 받은 업무 - 1차 분류 트리 보여주기
	\DeskPlusSolution\DeskPlusEIP\Coop\Category\Tree.aspx.cs

: 2015-02-27 by shimpark -----------------------------------------------------------
[업무협업] 2차분류 지정자 부분 개선하기
	\DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Request\Add.js
	\DeskPlusSolution\DeskPlusEIP\Coop\Ws\Content\Request\Add.asmx.cs


### DeskPlusSP v4.3 (2015.02.27)#########################################################

: 2015-02-26 by shimpark -----------------------------------------------------------
[업무협업] 대상선택 사용자 나오기
	\DeskPlusEIP\Coop\Js\Common\GetUserList.js

: 2015-02-26 by shimpark -----------------------------------------------------------
[업무협업] 근거문서 보기화면 
	\DeskPlusEIP\Coop\Jt\Content\Request\JTGetListBase.htm

: 2015-02-23 by maniaysw -----------------------------------------------------------
[전자결재] 문서함 메일발송 정규식 오류 수정 

DeskPlusEIP\Approval\Ws\Folder\Dept\SendMail.asmx.cs


: 2015-02-17 By amk0601 ------------------------------------------------------------
[자원관리-참석자] 기존데이터 수정시 오류 처리 및 다국어 추가

	\DeskPlusEIP\RM\Content\AddReserveUser.aspx.cs
	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Js\Content\AddReserveUser.js

	\DeskPlusEIP\RM\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ja.resx

: 2015-02-16 By amk0601 ------------------------------------------------------------
[자원관리-참석자] 자원수정할때 반복설정 있으면 참석자 사라지는 문제 수정

	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs

: 2015-02-13 By SHIMPARK ------------------------------------------------------------
[PMS] 폴더 선택 버그 FIX

	\DeskPlusSolution\DeskPlusEIP\PMS\Js\Admin\Project\ProjectInfo.js

: 2015-02-13 By SHIMPARK ------------------------------------------------------------
[PMS] 게시판 새창 부분 버그 FIX

	\DeskPlusSolution\DeskPlusEIP\PMS\Board\View.aspx
	\DeskPlusSolution\DeskPlusEIP\PMS\Board\View.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\PMS\Board\ViewPopup.aspx
	\DeskPlusSolution\DeskPlusEIP\PMS\Js\Board\View.js

: 2015-02-13 By amk0601 ------------------------------------------------------------
[자원관리-참석자] 자원선택시 참석자 추가기능
	\DeskPlusEIP\RM\Content\Add.aspx
	\DeskPlusEIP\RM\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Content\AddReserveUser.aspx
	\DeskPlusEIP\RM\Content\AddReserveUser.aspx.cs
	\DeskPlusEIP\RM\Content\AddN.aspx
	\DeskPlusEIP\RM\Content\Mod.aspx
	\DeskPlusEIP\RM\Content\Mod.aspx.cs
	\DeskPlusEIP\RM\Content\ModN.aspx
	\DeskPlusEIP\RM\Content\View.aspx
	\DeskPlusEIP\RM\Content\ViewPopup.aspx
	\DeskPlusEIP\RM\Js\Content\Add.js
	\DeskPlusEIP\RM\Js\Content\AddReserveUser.js
	\DeskPlusEIP\RM\Js\Content\Mod.js
	\DeskPlusEIP\RM\Js\Content\View.js
	\DeskPlusEIP\RM\Jt\Content\JTReserveUser.htm
	\DeskPlusEIP\RM\Ws\Content\Mod.asmx.cs
	\DeskPlusEIP\RM\Ws\Content\View.asmx.cs
	\DeskPlusEIP\RM\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\RM\Resource\GlobalString\string.ja.resx
	\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ReserveUserInfo.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs
	\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\ReserveUser.cs

: 2015-02-12 By amk0601 ------------------------------------------------------------
[조직도 - 부서] 부서명 괄호() 입력 가능하게 수정
	\DeskPlusEIP\Account\Js\Admin\Dept\List.js
	\DeskPlusEIP\AccountProject\Zeospace.DeskPlus.Account.DAL\Dept.cs

: 2015-02-11 By famo23 ------------------------------------------------------------
[전자결재] 수신완료문서함에서 문서보안적용 안되어 있는 버그수정
	DP_APP_Doc_SelPageListSendRecv
	DP_APP_Doc_SelPageCntSendRecv

: 2015-01-29 By famo23 ------------------------------------------------------------
[전자결재] Skype 구동 관련수정
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Process\Afterward\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Process\Confirm\Approve.aspx
	\DeskPlusEIP\Approval\Process\Confirm\ApprovePopup.aspx

[전자결재] 본문/결재선 수정에서 열람권한 잘못 나오는 버그 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js

[전자결재] 다중태그들에 지정되어 있는 사용자가 본문수정할때 안 나오는 버그수정
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	
:2015-01-27 by famo23 ----------------------------------------------------------
[전자결재] 본문/결재선 수정에서 문서종류 잘못 나오는 버그 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js

:2015-01-27 by shimpark ----------------------------------------------------------
[검색창] 불필요한 portlet 의 smart 폴더 삭제 ( js, jt, asmx~)

	\DeskPlusSolution\DeskPlusEIP\Portlet\

:2015-01-27 by shimpark ----------------------------------------------------------
[검색창] 모든 검색창에 "& 글자" 입력시 오류 발생

- String.prototype.securityText

	\DeskPlusEIP\Html\Js\jQuery\jquery.common.js

- GetParmforParameter 함수

	\DeskPlusSolution\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\UtilHelper.cs

:2015-01-22 by shimpark ----------------------------------------------------------
[iframe 조절 - 해당 map 파일 없어서 스크립트 오류남.]

http://static.aciaaraguari.com.br/js/vendor/jquery/plugins/jquery.iframeresizer/src/

위의 사이트에서 가져왔음. 현재 문제는 없어 보임.

\DeskPlusEIP\Html\Js\src\iframeResizer.contentWindow.map
\DeskPlusEIP\Html\Js\src\iframeResizer.map


:2015-01-22 by famo23 ----------------------------------------------------------
[일정관리 - 리소스없는 버그수정]
	\DeskPlusEIP\Pim\Schedule\AddN.aspx


: 2015-01-22 By shimpark ----------------------------------------------------------
[포틀릿] - 날씨 포틀릿 추가

-- 소스

	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ko.resx

	\DeskPlusEIP\Portlet\Package\Weather.ascx
	\DeskPlusEIP\Portlet\Package\Weather.ascx.cs

	\DeskPlusEIP\Portlet\Ws\Package\Weather.asmx
	\DeskPlusEIP\Portlet\Ws\Package\Weather.asmx.cs

	\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\Weather.cs


-- 테이블 및 저장프로시저

CREATE TABLE [dbo].[DP_ACC_Weather](
	[WeatherID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[Etc] [nvarchar](50) NULL
) ON [PRIMARY]

GO


GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DP_ACC_User_SelWeather]
	@CompanyID int,
	@UserID int
	
AS
	DECLARE @iUserID int
	DECLARE @iCity NVARCHAR(50)
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT  @iUserID = IsNull( UserID, -1) FROM  DP_ACC_Weather  WHERE CompanyID = @CompanyID AND  UserID = @UserID

	IF (@iUserID is null)
	BEGIN
		SELECT	'Seoul' as City
	END
	Else
	BEGIN
		SET @iCity = (SELECT City FROM  DP_ACC_Weather  WHERE CompanyID = @CompanyID AND UserID = @UserID)
		SELECT @iCity as City
	END
    -- Insert statements for procedure here
	
END

GO


SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DP_ACC_User_UpdWeather]
	@CompanyID int,
	@UserID int,
	@City NVARCHAR(50)
	
AS
	DECLARE @iUserID int
BEGIN

	SET NOCOUNT ON;
		SELECT  @iUserID = IsNull( UserID, -1) FROM  DP_ACC_Weather  WHERE CompanyID = @CompanyID AND UserID = @UserID
	IF (@iUserID is null)
	BEGIN
		INSERT INTO DP_ACC_Weather(CompanyID, UserID, City) VALUES (@CompanyID, @UserID, @City)
	END
	ELSE
	BEGIN
		UPDATE DP_ACC_Weather SET CITY =  @City WHERE CompanyID = @CompanyID AND UserID = @UserID
	END

END

GO



: 2015-01-21 By kkzz52 ------------------------------------------------------------
[전자결재] 부서명에 &, <, > 라는 특수문자가 있을 경우 상신 시 오류 수정(이유는 xml 형식을 사용하다보니...ㅡㅡ;)
  \DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js (Common.GetSeqXML, Common.GetSeqXMLZQZC)


: 2015-01-21 By shimpark ----------------------------------------------------------
[사용자] - 메일 계정 정리

\DeskPlusSolution\DeskPlusEIP\Account\Admin\User\Add.aspx
\DeskPlusSolution\DeskPlusEIP\Account\Admin\User\Add.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Account\Admin\User\Mod.aspx
\DeskPlusSolution\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Account\MyInfo\Mod.aspx
\DeskPlusSolution\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs

\DeskPlusSolution\DeskPlusEIP\Account\Admin\User\List.aspx
\DeskPlusSolution\DeskPlusEIP\Account\Admin\User\ListBlog.aspx

\DeskPlusSolution\DeskPlusEIP\Account\Js\Admin\User\List.js
\DeskPlusSolution\DeskPlusEIP\Account\Js\Admin\User\ListBlog.js
\DeskPlusSolution\DeskPlusEIP\Account\Js\MyInfo\Mod.js

\DeskPlusSolution\DeskPlusEIP\Account\Ws\Admin\User\List.asmx.cs
\DeskPlusSolution\DeskPlusEIP\Account\Ws\Admin\User\ListBlog.asmx.cs


: 2015-01-20 By shimpark ----------------------------------------------------------
[회원정보] 오류 제거 완료
	
	\DeskPlusSolution\DeskPlusEIP\Account\Js\Search\List.js

: 2015-01-20 By shimpark ----------------------------------------------------------
[포틀릿] 최대 등록자, 최대 조회글

	-- 기존 함수 2개 삭제
	\DeskPlusSolution\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\Lists.cs

	-- 신규 클래스 생성 및 메소드 2개 추가
	\DeskPlusSolution\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\Board.cs

	-- htm 파일 디자인 
	\DeskPlusSolution\DeskPlusEIP\Portlet\Jt\Package\Board\JTTopReader.htm
	\DeskPlusSolution\DeskPlusEIP\Portlet\Jt\Package\Board\JTTopRegister.htm

	-- 기존 함수 제거
	\DeskPlusSolution\DeskPlusEIP\Portlet\Ws\Package\Board\List.asmx.cs	

	-- 파일 내용 수정 => htm 및 asmx 경로 변경
	\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Board\CountTab.ascx

	-- ascx 새로 생성
	\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Board\TopReader.ascx
	\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Board\TopReader.ascx.cs
	\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Board\TopRegister.ascx
	\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Board\TopRegister.ascx.cs

	-- 저장프로시저
	DP_PT_Board_SelTopReader	
	DP_PT_Board_SelTopRegister	


: 2015-01-15 By 방영환 ----------------------------------------------------------
[전자결재] 결재취소시 해당문서 상태 체크
	\DeskPlusEIP\Approval\Status\ListRelated.aspx
	\DeskPlusEIP\Approval\Js\Status\ListRelated.js
	\DeskPlusEIP\Approval\Ws\Status\ListRelated.asmx

	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs

	sp 추가) DP_APP_Doc_IsCancelApproval 추가

: 2015-01-14 By famo23 ----------------------------------------------------------
[전자결재] 자주지정하는 결재자 없을 경우 높이 생기는 문제수정
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js	

[공통] 회사추가시에 기본값 수정
	DP_COM_Company_Ins

: 2015-01-12 by withsuki
[전자결재-공람프로세스 추가 : ZR태그사용, Sequence=0 으로 들어감]
	공람 : ZR태그사용, Sequence=0으로 처리, 진행&완료 모두, 문서확인 기능
source>
	\DeskPlusEIP\Approval\Admin\FormGeneration\Add.aspx
	\DeskPlusEIP\Approval\Admin\FormGeneration\ModSequence.aspx
	\DeskPlusEIP\Approval\App_Data\Confirm\en.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\fr.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\ja.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\ko.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\zh-cn.xml
	\DeskPlusEIP\Approval\App_Data\Confirm\zh-tw.xml
	\DeskPlusEIP\Approval\Common\Status\FolderStatusTreeScript.ascx
	\DeskPlusEIP\Approval\Folder\Common\DocUserSeq.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\Add.js
	\DeskPlusEIP\Approval\Js\Folder\Common\DocUserSeq.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Confirm\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Status\ListConfirm.js
	\DeskPlusEIP\Approval\Js\Status\Common\DocUserSeq.js
	\DeskPlusEIP\Approval\Js\Status\Menu\Tree.js
	\DeskPlusEIP\Approval\Jt\Status\JTListConfirm.htm
	\DeskPlusEIP\Approval\Jt\Status\Common\JTDocUserSeq.htm
	\DeskPlusEIP\Approval\Jt\Status\Common\JTDocUserSeqCA.htm
	\DeskPlusEIP\Approval\Process\Confirm\Approve.aspx
	\DeskPlusEIP\Approval\Process\Confirm\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Resource\GlobalString\모두
	\DeskPlusEIP\Approval\Status\ListConfirm.aspx
	\DeskPlusEIP\Approval\Status\Common\DocUserSeq.aspx
	\DeskPlusEIP\Approval\Ws\Status\ListConfirm.asmx
	\DeskPlusEIP\Approval\Ws\Status\Common\DocUserSeq.asmx
	\DeskPlusEIP\App_Data\APPROVAL.xml
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentInfo.cs            
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentProcessInfo.cs     
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Sequence.cs               
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs        
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs               
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\SequenceBackUp.cs         
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs               
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs 
		 
sp>
	DP_APP_DocProcess_SelPageCntConfirm
	DP_APP_DocProcess_SelPageListConfirm
	DP_APP_SeqBackup_UpdConfirm
	DP_APP_Seq_UpdConfirm
	DP_APP_Seq_SelByReject
	DP_APP_Seq_SelAlarmNext
	DP_APP_Doc_UpdContentHTML
	DP_APP_Seq_SelIsApprove

Design>
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\Lang


: 2015-01-09 By famo23 ----------------------------------------------------------
[포틀릿] 디자인수정
	\DeskPlusEIP\Portlet\Jt\Template\JTList.htm
	\DeskPlusEIP\Design\Dev\C\Css\Func_Package.css

: 2015-01-05 By amk0601 ----------------------------------------------------------
[포틀릿 나라명 다국어 리소스 추가] 
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.zh-tw.resx

: 2015-01-05 By SHIMPARK ----------------------------------------------------------
[태그프리 갱신]  - 3.7 이상

위치
 \DeskPlusSolution\DeskPlusEIP\Data\Editor


: 2015-01-05 By famo23 ------------------------------------------------------------
[포틀릿] 사용자정보가 길면 안보이는 문제처리
	\DeskPlusEIP\Portlet\Package\MyInfo.ascx

: 2015-01-05 By SHIMPARK ----------------------------------------------------------
[포틀릿 ip 기능 추가] 


- 생성 table 
 : [DP_EIP_PortletIP] 

- 추가 GMU
 : INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('EIP','QRY_EIP_PortletIP_Sel','SELECT * FROM DP_EIP_PortletIP WHERE CompanyID = @CompanyID','심재운','포틀릿 ip 정보 가져오기','20150105')
 : INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('EIP','QRY_EIP_PortletIP_SelByID','SELECT * FROM DP_EIP_PortletIP WHERE CompanyID = @CompanyID AND PortletID = @PortletID','심재운','포틀릿 ip id로 정보 가져오기','20150105')

- 추가 SP
 : DP_EIP_PortletIP_Del
 : DP_EIP_PortletIP_Ins

- 추가 페이지 및 수정

\DeskPlusSolution\DeskPlusEIP\EIP\Admin\Portlet\Mod.aspx
\DeskPlusSolution\DeskPlusEIP\EIP\Admin\Portlet\Mod.aspx.cs
\DeskPlusSolution\DeskPlusEIP\EIP\Admin\Portlet\ModTab.aspx
\DeskPlusSolution\DeskPlusEIP\EIP\Admin\Portlet\ModTab.aspx.cs
\DeskPlusSolution\DeskPlusEIP\EIP\Admin\Portlet\AddIP.aspx
\DeskPlusSolution\DeskPlusEIP\EIP\Admin\Portlet\AddIP.aspx.cs

\DeskPlusSolution\DeskPlusEIP\EIP\Js\Admin\Portlet\AddIP.js
\DeskPlusSolution\DeskPlusEIP\EIP\Js\Admin\Portlet\Mod.js
\DeskPlusSolution\DeskPlusEIP\EIP\Js\Admin\Portlet\ModTab.js

\DeskPlusSolution\DeskPlusEIP\EIP\Jt\Admin\Portlet\JTIP.htm

\DeskPlusSolution\DeskPlusEIP\EIP\Ws\Admin\Portlet\AddIP.asmx
\DeskPlusSolution\DeskPlusEIP\EIP\Ws\Admin\Portlet\Mod.asmx.cs
\DeskPlusSolution\DeskPlusEIP\EIP\Ws\Admin\Portlet\ModTab.asmx.cs

\DeskPlusSolution\Projects\EIPProject\Zeospace.DeskPlus.EIP.BE\PortletIPInfo.cs
\DeskPlusSolution\Projects\EIPProject\Zeospace.DeskPlus.EIP.BLL\PortletIP.cs
\DeskPlusSolution\Projects\EIPProject\Zeospace.DeskPlus.EIP.DAL\PortletIP.cs

\DeskPlusSolution\DeskPlusEIP\EIP\Package\Main.aspx.cs


: 2015-01-02 By SHIMPARK ----------------------------------------------------------
[업무협업] 

-- SP 패치

delete from dp_query where category = 'COOP'
GO

INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_APPBaseDoc_Sel','SELECT Base.AppBaseDocID, Base.BaseOrder, Base.DocumentID, Base.Name, VW.*  FROM VW_COOP_List AS VW   LEFT OUTER JOIN DP_COOP_APPBaseDoc AS Base  ON VW.CompanyID = Base.CompanyID  AND VW.ListID = Base.ListID  
WHERE VW.CompanyID = @CompanyID AND Base.ListID IN ({0})  ORDER BY BaseOrder','shine','근거문서 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_Attach_Sel','SELECT *   FROM DP_COOP_Attach   WHERE CompanyID = @CompanyID AND ListID=@ListID   ORDER BY AttachFileID','심재운','첨부파일 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_Attach_SelByID','SELECT *   FROM DP_COOP_Attach   WHERE CompanyID = @CompanyID AND ListID=@ListID AND AttachFileID = @AttachFileID','심재운','첨부파일 ID 별 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_BaseCoop_Sel','SELECT CompanyID, ListID, GroupID, BaseCoopID, BaseOrder, Subject 
FROM DP_COOP_BaseCoop
WHERE CompanyID = @CompanyID AND ListID = @ListID AND GroupID = @GroupID
ORDER BY BaseOrder ASC','shimpark','협업 근거문서 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_Cabinet_Sel','SELECT * FROM DP_COOP_Cabinet  WHERE CompanyID = @CompanyID AND ActiveCabinet = ''1''  ORDER BY CompanyID, ParentCabinetID, CabinetOrder','심재운','캐비넷 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardAttach_Sel','SELECT *   FROM DP_COOP_ForwardAttach   WHERE CompanyID = @CompanyID  AND ForwardListID=@ForwardListID   ORDER BY AttachFileID','심재운','전달받은 첨부파일 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardAttach_SelByID','SELECT *   FROM DP_COOP_ForwardAttach   WHERE CompanyID = @CompanyID   AND ForwardListID=@ForwardListID   ORDER BY AttachFileID','심재운','전달받은 문서 첨부파일 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardBaseCoop_Sel','SELECT CompanyID, ForwardListID, ForwardBaseCoopID, ListID, GroupID, BaseCoopID, BaseOrder, Subject 
FROM DP_COOP_ForwardBaseCoop
WHERE CompanyID = @CompanyID AND ForwardListID = @ForwardListID
ORDER BY BaseOrder ASC','shimpark','협업 전달 근거문서 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardBaseDoc_Sel','SELECT Base.BaseOrder, Base.DocumentID, Base.Name, VW.*  
FROM VW_COOP_ForwardList AS VW   LEFT OUTER JOIN DP_COOP_ForwardBaseDoc AS Base  
ON VW.CompanyID = Base.CompanyID  AND VW.ForwardListID = Base.ForwardListID  
WHERE VW.CompanyID = @CompanyID AND Base.ForwardListID = @ForwardListID 
ORDER BY BaseOrder','shine','전달받은 근거문서 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardList_SelForwardListID','SELECT * FROM VW_COOP_ForwardList  WHERE CompanyID = @CompanyID   AND ForwardUserID = @UserID  AND ForwardListID IN ({0})','shine','전달받은 협업 리스트 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardList_SelGroupID','SELECT * FROM VW_COOP_ForwardList  WHERE CompanyID = @CompanyID   AND ForwardUserID = @ForwardUserID  AND ForwardGroupID = @ForwardGroupID  ORDER BY CompanyID, ListOrder','shine','그룹아이디로 리스트 가져오기  ','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardList_SelListID','SELECT * FROM VW_COOP_ForwardList  WHERE CompanyID = @CompanyID   AND ForwardUserID = @ForwardUserID  AND ForwardListID = @ForwardListID','shine','전달받은 협업 리스트 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardListCC_Sel','SELECT List.*, DC.DeptBase, DC.DeptKo, DC.DeptEn, DC.DeptJa, DC.DeptFr, DC.DeptZhcn, DC.DeptZhtw, DC.DeptManyName,   DC.TitleBase, DC.TitleKo, DC.TitleEn, DC.TitleJa, DC.TitleFr, DC.TitleZhcn, DC.TitleZhtw, DC.TitleManyName,   DC.DutyBase, DC.DutyKo, DC.DutyEn, DC.DutyJa, DC.DutyFr, DC.DutyZhcn, DC.DutyZhtw, DC.DutyManyName  FROM DP_COOP_ForwardListCC List  LEFT OUTER JOIN  DP_COM_DeptCode AS DC ON List.CompanyID = DC.CompanyID AND List.DeptCodeID = DC.DeptCodeID  WHERE List.CompanyID = @CompanyID AND List.ForwardListID = @ForwardListID','심재운','전달받은 참조자 정보 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ForwardListTO_Sel','SELECT List.*, DC.DeptBase, DC.DeptKo, DC.DeptEn, DC.DeptJa, DC.DeptFr, DC.DeptZhcn, DC.DeptZhtw, DC.DeptManyName,   DC.TitleBase, DC.TitleKo, DC.TitleEn, DC.TitleJa, DC.TitleFr, DC.TitleZhcn, DC.TitleZhtw, DC.TitleManyName,   DC.DutyBase, DC.DutyKo, DC.DutyEn, DC.DutyJa, DC.DutyFr, DC.DutyZhcn, DC.DutyZhtw, DC.DutyManyName  FROM DP_COOP_ForwardListTO List  LEFT OUTER JOIN  DP_COM_DeptCode AS DC ON List.CompanyID = DC.CompanyID AND List.DeptCodeID = DC.DeptCodeID  WHERE List.CompanyID = @CompanyID AND List.ForwardListID = @ForwardListID','심재운','전달받은 수신자 정보 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_Item_Sel','SELECT * FROM DP_COOP_Item WHERE CompanyID = @CompanyID AND ActiveGroup in ({0})  ORDER BY COMPANYID ASC, Orders ASC','심재운','1단계정보가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_Item_SelByID','SELECT * FROM DP_COOP_Item WHERE CompanyID = @CompanyID AND ItemID = @ItemID AND ActiveGroup in ({0}) ','심재운','1단계정보가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ItemFavUser_Sel','SELECT a.*,   b.NameBase,   b.NameKo,   b.NameEn,   b.NameJa,   b.NameFr,   b.NameZhcn,   b.NameZhtw  FROM DP_COOP_ItemFavUser a  INNER JOIN VW_USERINFO b ON a.CompanyID = b.CompanyID   AND a.UserID = b.UserID  WHERE a.CompanyID = @CompanyID   AND a.ItemSubID = @ItemSubID   AND b.ActiveUser = ''1''   AND BaseDept = ''1''  ORDER BY a.Orders  ','심재운','자주지정사용자','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ItemSub_Sel','SELECT *,  (SELECT COUNT(*) FROM   DP_COOP_Template WHERE CompanyID = DP_COOP_ItemSub.CompanyID   AND ItemSubID = DP_COOP_ItemSub.ItemSubID) TempCnt  FROM DP_COOP_ItemSub WHERE CompanyID = @CompanyID AND ItemID = @ItemID AND ActiveGroup in ({0})  ORDER BY COMPANYID ASC, Orders ASC','심재운','2단계정보가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ItemSub_SelByID','SELECT *,  (SELECT COUNT(*) FROM   DP_COOP_Template WHERE CompanyID = DP_COOP_ItemSub.CompanyID   AND ItemSubID = DP_COOP_ItemSub.ItemSubID) TempCnt   FROM DP_COOP_ItemSub   WHERE CompanyID = @CompanyID AND ItemID = @ItemID AND ItemSubID = @ItemSubID AND ActiveGroup in ({0}) ','심재운','2단계정보가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_List_Sel','SELECT * FROM VW_COOP_List  WHERE CompanyID = @CompanyID AND ListID IN ({0})','심재운','리스트 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_List_SelByGroupID','SELECT * FROM VW_COOP_List  WHERE CompanyID = @CompanyID AND GroupID = @GroupID  ORDER BY CompanyID, ListOrder','심재운','리스트 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ListCC_Sel','SELECT List.*, DC.DeptBase, DC.DeptKo, DC.DeptEn, DC.DeptJa, DC.DeptFr, DC.DeptZhcn, DC.DeptZhtw, DC.DeptManyName,   DC.TitleBase, DC.TitleKo, DC.TitleEn, DC.TitleJa, DC.TitleFr, DC.TitleZhcn, DC.TitleZhtw, DC.TitleManyName,   DC.DutyBase, DC.DutyKo, DC.DutyEn, DC.DutyJa, DC.DutyFr, DC.DutyZhcn, DC.DutyZhtw, DC.DutyManyName  FROM DP_COOP_ListCC List  LEFT OUTER JOIN  DP_COM_DeptCode AS DC ON List.CompanyID = DC.CompanyID AND List.DeptCodeID = DC.DeptCodeID  WHERE List.CompanyID = @CompanyID AND List.ListID = @ListID','심재운','참조자 정보 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ListCC_SelTodayCnt','SELECT COUNT(*) FROM DP_COOP_ListViewer WHERE CompanyID = @CompanyID AND UserID = @UserID AND TOCC = ''CC''  AND ListID IN (SELECT ListID FROM DP_COOP_ListCC WHERE CompanyID = @CompanyID AND UserID = @UserID)','심재운','참조자가 볼 수 있는 리스트 금일 총 카운트 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ListCC_SelTotCnt','SELECT COUNT(*) FROM VW_COOP_ListCC WHERE CompanyID = @CompanyID AND UserID = @UserID','심재운','참조자가 볼 수 있는 리스트 총 카운트 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ListOption_Sel','SELECT * FROM DP_COOP_ListOption  WHERE CompanyID = @CompanyID AND ListID = @ListID','심재운','옵션정보 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ListTO_Sel','SELECT List.*, DC.DeptBase, DC.DeptKo, DC.DeptEn, DC.DeptJa, DC.DeptFr, DC.DeptZhcn, DC.DeptZhtw, DC.DeptManyName,   DC.TitleBase, DC.TitleKo, DC.TitleEn, DC.TitleJa, DC.TitleFr, DC.TitleZhcn, DC.TitleZhtw, DC.TitleManyName,   DC.DutyBase, DC.DutyKo, DC.DutyEn, DC.DutyJa, DC.DutyFr, DC.DutyZhcn, DC.DutyZhtw, DC.DutyManyName  FROM DP_COOP_ListTO List  LEFT OUTER JOIN  DP_COM_DeptCode AS DC ON List.CompanyID = DC.CompanyID AND List.DeptCodeID = DC.DeptCodeID  WHERE List.CompanyID = @CompanyID AND List.ListID = @ListID','심재운','수신자 정보 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ListTO_SelTodayCnt','SELECT COUNT(*) FROM DP_COOP_ListViewer WHERE CompanyID = @CompanyID AND UserID = @UserID AND TOCC = ''TO''  AND ListID IN (SELECT ListID FROM dbo.DP_COOP_ListTO WHERE CompanyID = @CompanyID AND UserID = @UserID)','심재운','수신자가 볼 수 있는 리스트 금일 총 카운트 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ListTO_SelTotCnt','SELECT COUNT(*) FROM VW_COOP_ListTO WHERE CompanyID = @CompanyID AND UserID = @UserID','심재운','수신자가 볼 수 있는 리스트 총 카운트 가져오기','20150102')
INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_Template_Sel','SELECT * FROM DP_COOP_Template  WHERE CompanyID = @CompanyID AND ItemSubID = @ItemSubID','심재운','템플릿 가져오기','20150102')


: 2015-01-02 By amk0601 ----------------------------------------------------------
[시계 포틀릿 추가] 
	\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Clock 폴더 하위 추가 (css. image, js)
	\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Clock\AnalogClock.ascx
	\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Clock\AnalogClock.ascx.cs

[나라명 다국어 리소스 추가] 
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Portlet\Resource\GlobalString\string.en.resx

	
: 2015-01-02 By famo23 ------------------------------------------------------------
[전자결재] 폴더 경로 수정 (값이 빈값일때 base 나오게 처리)
	\DeskPlusEIP\Approval\Ws\Preference\FavoriteForm\List.asmx.cs

[전자결재,SNS] 다국어처리
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
    \DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirSendList.aspx
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirSendList.js
	\DeskPlusEIP\Approval\Jt\Folder\Carboncopy\JTCirSendList.htm
	\DeskPlusEIP\Approval\Admin\Menu\Tree.aspx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.ja.resx    
	\DeskPlusEIP\SNS\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\SNS\Resource\GlobalString\string.zh-tw.resx

[전자결재] - 유동결재망 태그가 없을때 지정취소가 안되는 버그 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

[전자결재 - 알람xml정보 못 가져오는 버그 수정]
	\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

[전자결재] 대결자지정 설명변경
	\DeskPlusEIP\Approval\Preference\Proxy\List.aspx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
    \DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx

: 2015-01-02 By withsuki ------------------------------------------------------------
[전자결재] 결재태그가없는경우, 예외처리수정.
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

: 2014-12-26 By famo23 ------------------------------------------------------------
[포틀릿] 전자결재 대결자 카운트 버그수정(상신보류도 나오는 문제)
	DP_PT_MyInfo_SelUserID

: 2014-12-19 By famo23 ------------------------------------------------------------
[전자결재] 통계-조건별사용현황 100%일때 화면 깨지는 버그수정
	\DeskPlusEIP\Approval\Jt\Admin\Statistics\ListTop.htm
	\DeskPlusEIP\Design\C\Css\Core_Component.css

: 2014-12-19 By shimpark ------------------------------------------------------------
[달력] - 큰 달력에 공휴일 설명 기재

\DeskPlusSolution\DeskPlusEIP\App_Data\Config.xml

	<Item ID="SolarDesc" Value="신정,삼일절,어린이날,현충일,광복절,개천절,한글날,성탄절" Content="양력날짜 설명" />
	<Item ID="LunarDesc" Value="설날,석가탄신일,추석" Content="음력날짜 설명" />


\DeskPlusSolution\DeskPlusEIP\Community\Ws\Site\Apps\Schedule\List.asmx.cs
\DeskPlusSolution\DeskPlusEIP\RM\Ws\Content\ListMonth.asmx.cs
\DeskPlusSolution\DeskPlusEIP\RM\Ws\Content\WholeMonth.asmx.cs
\DeskPlusSolution\DeskPlusEIP\Visit\Ws\Content\ListMonth.asmx.cs
\DeskPlusSolution\DeskPlusEIP\Pim\Ws\Schedule\Schedule.asmx.cs

\DeskPlusSolution\DeskPlusEIP\Pim\Js\Schedule\ListMonth.js

: 2014-12-19 By shimpark ------------------------------------------------------------
[xfree] 에디터 jsp 파일 링크 안되는 부분 개선

 \DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Add.js(121,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Mod.js(147,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Reply.js(84,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Template\Add.js(67,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Add.js(122,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Mod.js(143,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Reply.js(90,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Board\Js\Temp\Add.js(84,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Board\Js\Temp\Mod.js(79,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Pim\Js\Admin\Holiday\Add.js(107,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Pim\Js\Admin\Holiday\Mod.js(93,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Pim\Js\Anniversary\Add.js(90,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Pim\Js\Anniversary\Mod.js(94,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Pim\Js\Schedule\Add.js(235,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Pim\Js\Schedule\Mod.js(193,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Pim\Js\ToDo\Add.js(116,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\FormGeneration\Add.js(68,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Basic\EditorActiveX.js(44,9):        strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js(929,17):                strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js(123,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Opinion\Js\Admin\Content\QuesAdd.js(75,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Opinion\Js\Admin\Content\QuesMod.js(101,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\RM\Js\Content\Add.js(450,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\RM\Js\Content\Mod.js(550,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Community\Js\Site\Admin\Function\Main\Add.js(63,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Community\Js\Site\Apps\Board\Add.js(70,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Community\Js\Site\Apps\Board\Mod.js(90,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Community\Js\Site\Apps\Board\Reply.js(74,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Community\Js\Site\Apps\Schedule\Add.js(194,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Community\Js\Site\Apps\Schedule\Mod.js(213,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Community\Js\Site\Main\Mail\Default.js(244,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Board\Add.js(208,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Board\Mod.js(99,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Board\Reply.js(209,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Schedule\Add.js(238,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Schedule\Mod.js(253,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Template\Add.js(84,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Template\Mod.js(83,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Unit\Add.js(413,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\PMS\Js\Unit\Mod.js(441,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Admin\Category\Add.js(71,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Admin\Category\Mod.js(76,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Receive\AddAccept.js(297,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Receive\AddComplete.js(228,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Receive\Mod.js(612,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Receive\View.js(806,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Receive\ViewComplete.js(280,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Reference\Mod.js(554,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Reference\View.js(560,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Request\Add.js(533,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Request\AddRecall.js(885,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Request\Mod.js(895,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Request\ModReply.js(613,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Request\View.js(690,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Total\ViewPopup.js(214,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Visit\Js\Content\Add.js(469,13):            strHtml = strHtml.replace(".js\"", "\"");
  \DeskPlusSolution\DeskPlusEIP\Visit\Js\Content\Mod.js(815,13):            strHtml = strHtml.replace(".js\"", "\"");

: 2014-12-18 By famo23 ------------------------------------------------------------
[방문예약] 작은 달력 음력표시 버그수정
	\DeskPlusEIP\Visit\Content\Tree.aspx.cs

[일정관리] 작은 달력 음력표시 버그수정 / 현재년도와 다른 년도의 공휴일에 일정있을때 색깔표시가 검은 색 나오는 버그수정
	\DeskPlusEIP\Pim\Menu\PimMenu.aspx.cs

[포틀릿] 작은 달력 음력표시 버그수정 / 현재년도와 다른 년도의 공휴일에 일정있을때 색깔표시가 검은 색 나오는 버그수정
	\DeskPlusEIP\Portlet\Package\MyPim.ascx.cs
	\DeskPlusEIP\Portlet\Smart\MyPim.ascx.cs

: 2014-12-09 By shimpark ------------------------------------------------------------
[입력박스 설명서] - jquery.placeholder 기능 갱신

	: https://github.com/mathiasbynens/jquery-placeholder

	\DeskPlusSolution\DeskPlusEIP\Html\Js\jQuery\jquery.placeholder.js
	\DeskPlusSolution\DeskPlusEIP\Html\Js\jQuery\validator.js

	\DeskPlusSolution\DeskPlusEIP\Js\Site\Auth\Default.js
	\DeskPlusSolution\DeskPlusEIP\Site\Auth\Default.aspx

	\DeskPlusSolution\DeskPlusEIP\Address\Content\List.aspx
	\DeskPlusSolution\DeskPlusEIP\Address\Content\ListBlog.aspx

	\DeskPlusSolution\DeskPlusEIP\Address\Js\Content\List.js
	\DeskPlusSolution\DeskPlusEIP\Address\Js\Content\ListBlog.js

: 2014-11-21 by 방영환  ----------------------------------------------------------
[전자결재] - 결재취소 기능추가.

소스)
	\DeskPlusSolution\DeskPlusEIP\Approval\Status\ListRelated.aspx
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Status\ListRelated.js
	\DeskPlusSolution\DeskPlusEIP\Approval\Ws\Status\ListRelated.asmx.cs
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\SequenceInfo.cs
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	\DeskPlusSolution\DeskPlusEIP\Util\Component\HtmlAgilityPack.dll
	Approval Web Project 참조추가-HtmlAgilityPack.dll

	\DeskPlusSolution\DeskPlusEIP\Approval\Base\BaseWSApproval.cs
	\DeskPlusSolution\DeskPlusEIP\Approval\AppData\Confirm\en.xml
	\DeskPlusSolution\DeskPlusEIP\Approval\AppData\Confirm\fr.xml
	\DeskPlusSolution\DeskPlusEIP\Approval\AppData\Confirm\ja.xml
	\DeskPlusSolution\DeskPlusEIP\Approval\AppData\Confirm\ko.xml
	\DeskPlusSolution\DeskPlusEIP\Approval\AppData\Confirm\zh-cn.xml
	\DeskPlusSolution\DeskPlusEIP\Approval\AppData\Confirm\zh-tw.xml
SP)
	DP_APP_Seq_UpdCancelApproval
	DP_APP_DocProcess_SelPageListRelated
	ufn_IsCancelApproval
	DP_APP_Seq_SelAlarmApproveCNext

: 2014-12-04 By famo23 ------------------------------------------------------------
[방문예약] 다국어 리소스 처리	
	\DeskPlusEIP\Visit\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Visit\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Visit\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Visit\Resource\GlobalString\string.zh-tw.resx

: 2014-12-03 By famo23 ------------------------------------------------------------
[포틀릿 - 상신보류카운트 나오는 버그 수정]
	DP_PT_MyInfo_SelUserID

:2014-12-03 by freeman ----------------------------------------------------------
[KM 포탈 지원] IFrameSize 조정
	\DeskPlusEIP\Pim\Js\Schedule\ScheduleMain.js
	\DeskPlusEIP\Pim\Schedule\ListDay.aspx
	\DeskPlusEIP\Pim\Schedule\ListMonth.aspx
	\DeskPlusEIP\Pim\Schedule\ListWeek.aspx
	\DeskPlusEIP\Pim\Schedule\ScheduleMain.aspx

	\DeskPlusEIP\RM\Js\Content\ListMain.js
	\DeskPlusEIP\RM\Content\ListDay.aspx
	\DeskPlusEIP\RM\Content\ListMain.aspx
	\DeskPlusEIP\RM\Content\ListMonth.aspx
	\DeskPlusEIP\RM\Content\ListWeek.aspx

:2014-12-03 by freeman ----------------------------------------------------------
[KM 포탈 지원] IFrameSize 조정
	\DeskPlusEIP\Html\Js\jQuery\iframeResizer.min.js
	\DeskPlusEIP\Html\Js\jQuery\iframeResizer.contentWindow.min.js

: 2014-12-03 By shimpark ------------------------------------------------------------
[방문자 - 본문이미지 추가 기능 포함 tagfree + xfree 둘다 적용 (xfree 땜시~)]
	\DeskPlusSolution\DeskPlusEIP\Visit\Content\AddN.aspx
	\DeskPlusSolution\DeskPlusEIP\Visit\Content\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Visit\Content\ModN.aspx
	\DeskPlusSolution\DeskPlusEIP\Visit\Content\Mod.aspx.cs

	\DeskPlusSolution\DeskPlusEIP\Visit\Js\Content\Add.js
	\DeskPlusSolution\DeskPlusEIP\Visit\Js\Content\Mod.js

: 2014-12-03 By shimpark ------------------------------------------------------------
[전자결재 = xfree 작업]

- xfree upload 기술
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs

\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Receive\Mdd.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Receive\MddContent.aspx.cs

\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs

\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx.cs

- 디자인
\DeskPlusSolution\DeskPlusEIP\Approval\FormFlow\Basic\EditorNonActiveX.aspx

- js
(GetHtmlValue => getHtmlValue 변경)
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\FormGeneration\Add.js(61):        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\Common\Popup\PreView.js(79):        objHtml = objEditor.getHtmlValue();
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js(932):        $("#htaEditorHTML").val(objEditor.getHtmlValue());
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js(112):        $("#htaEditorHTML").val(objEditor.getHtmlValue());

(SetHtmlValue = > setHtmlValue 변경)
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Basic\EditorNonActiveX.js(15):        xfe.setHtmlValue(TemplateHeader + $("#htaEditorHTML", parent.parent.document).val() + TemplateFooter);
  \DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Basic\EditorNonActiveX.js(17):        xfe.setHtmlValue(TemplateHeader + TemplateFooter);



: 2014-12-03 By shimpark ------------------------------------------------------------
[xfree 메뉴 수정]

	\DeskPlusEIP\Data\Editor\XFE2\config\cn_s.xml
	\DeskPlusEIP\Data\Editor\XFE2\config\cn_t.xml
	\DeskPlusEIP\Data\Editor\XFE2\config\en.xml

: 2014-12-03 By famo23 ------------------------------------------------------------
[방문예약 - 본문스크롤 생기지 않는 문제 수정]
	\DeskPlusEIP\Visit\Content\View.aspx

: 2014-12-02 By famo23 ------------------------------------------------------------
[전자결재 - 파일백업 문서번호에 특수기호있을때 다운 안되는 버그 수정]
	\DeskPlusEIP\Common\Zip\CreateZipFile.aspx.cs

[전자결재 - 파일백업 회람문서 파일명 안 나오는 버그 수정]
	\DeskPlusEIP\Common\Zip\CreateZipFile.aspx.cs

: 2014-11-21 By SHIMPARK ------------------------------------------------------------
[주소록] - 외부주소록 가져오기 버그 FIX

\DeskPlusEIP\Address\Ws\GoogleContactService.asmx.cs
\Projects\AddressProject\Zeospace.DeskPlus.Address.BLL\GoogleContactService.cs

\DeskPlusSolution\DeskPlusEIP\Util\GoogleApi 
- 안에 있는 GOOGLE API 파일 최신버전 2.2 갱신

\DeskPlusSolution\DeskPlusEIP\bin
- 구글 API 2.2 최신으로 갱신

: 2014-11-20 By famo23 ------------------------------------------------------------
[전자결재] - 크롬에서 닫기버튼 동작 안하는 버그수정
	\DeskPlusEIP\Approval\Js\FormFlow\Common\ModSequence.js

[전자결재 - 완료된 문서 의견추가 & 후열문서확인에서 문서 잘못 업데이트 되는 오류 수정]
	DP_APP_Doc_UpdContentHTML

:2014-11-18 by kkzz52----------------------------------------------------------
[전자결재 - 완료된 수신문서(ZP)에 의견추가를 했을 때 발신문서에 업데이트 되는 오류 수정]
	DP_APP_Doc_UpdContentHTML

: 2014-11-14 By famo23 ------------------------------------------------------------
[전자결재] 참조등 다중태크에 사용자 지정이 10명 이상 될경우 임시저장했을 시에 9명만 값을 가지고 오는 버그 수정
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js


:2014-11-07 by shimpark ----------------------------------------------------------
[전자결재 파일 백업 기능]

C:\SiteProject\Inoct\DeskPlusSolution\DeskPlusEIP\Common\Zip\CreateZipFile.aspx.cs

// 윈도우 파일명에는 \/:*?<>|" 특수문자를 넣을 수 없다. shimpark
mailDataInfo.Subject = Regex.Replace(docInfo.Subject, @"[\\/:*?<>|""]{1}", "");

:2014-11-04 by shimpark ----------------------------------------------------------
[게시판 권한, 전자결재 결재양식 - 자동화 완성]

	- 사용자 검색 웹서비스 추가
	 \DeskPlusSolution\DeskPlusEIP\Account\Ws\Common\GetUserDept.asmx
	 \DeskPlusSolution\DeskPlusEIP\Account\Ws\Common\GetUserDept.asmx.cs

	- 게시판 권한 검색 
	 \DeskPlusSolution\DeskPlusEIP\Board\Admin\Category\Mod.aspx
	
	- 자동화 완성 css 생성
	<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />

	- 자동완성 jquery 기능 추가
	 \DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Category\Mod.js

	 $("#DeptUserTreeScript1_tbDeptUser").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
                data: "{ 'p': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //Callback method when the precess is success
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Name,
                            value: item.Values
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Communication Error");
                }
            });
        },
        focus: function (event, ui) {
            // prevent autocomplete from updating the textbox
            event.preventDefault();
            // manually update the textbox
            $(this).val(ui.item.value);
        },
        select: function (event, ui) {
            // prevent autocomplete from updating the textbox
            event.preventDefault();
            // manually update the textbox and hidden field
            $(this).val(ui.item.value);
            $("#DeptUserTreeScript1_tbDeptUser").val(ui.item.value);
        },
        minLength: 1
    }).autocomplete("widget").addClass("fixed-height");


	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	: css 추가
	<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
	
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Common.js
	: 변경

	/**************************************************************
	트리메뉴제어
	**************************************************************/
	Common.SetMenu = function (_index) {
    
		$(".accordion").accordion({
			heightStyle: "fill",
			autoHeight: false,

			//fillSpace: true, // CHANGED (previously fillSpace: true)
			animated: 'slide',
			active: _index
		});
	}

	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Tree.js

	: 추가사항

	//자동화완성 - shimpark
        $("#UCDeptUserTreeScript1_tbDeptUser").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/DeskPlusEIP/Account/Ws/Common/GetUserDept.asmx/GetDeptUserSearch",
                    data: "{ 'p': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        //Callback method when the precess is success
                        response($.map(data.d, function (item) {
                            return {
                                label: item.Name,
                                value: item.Values
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Communication Error");
                    }
                });
            },
            focus: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.value);
            },
            select: function (event, ui) {
                event.preventDefault();
                $(this).val(ui.item.value);
                $("#UCDeptUserTreeScript1_tbDeptUser").val(ui.item.value);
            },
            minLength: 1
        }).autocomplete("widget").addClass("fixed-height");

        //--jquery-ui 1.10.3 는 resize 없어짐 ㅡㅡ; shimpark 수정
        var accordHeight = $(window).height() - 250;
        $(".menusub").css("height", accordHeight);
        $(".menusub").css("background", "#ffff");
        --//


:2014-10-30 by shimpark ----------------------------------------------------------
[외부 주소록 가져오기 - 버그 FIX]
	\DeskPlusSolution\DeskPlusEIP\Address\Outlook\InGoogleStep2.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Address\Outlook\InOutStep2.aspx.cs


:2014-10-23 by famo23 ----------------------------------------------------------
[전자결재 - 수신문서반려창변경]
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ConfirmReject.js
	\DeskPlusEIP\Approval\FormFlow\Receive\ConfirmReject.aspx

[전자결재 - 담당자변경 히스토리저장]
	DP_APP_CounterPartUserModify 추가
	DP_APP_CounterPartUser_UpdReplace

:2014-10-22 by shimpark ----------------------------------------------------------
[업무협업 알람 발송 개발 - 수신자에게만]
	\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\SMS.cs
	\Projects\CoopProject\Zeospace.DeskPlus.Coop.BLL\Alarm.cs


:2014-10-20 by famo23 ----------------------------------------------------------
[전자결재 - 수신 담당자변경 시 선택 필수체크]
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDeptUser.js
	\DeskPlusEIP\Approval\Common\Popup\GetDeptUser.aspx

:2014-10-20 by kkzz52 ----------------------------------------------------------
[전자결재 - 재상신시에 첨부파일 순서가 바뀌는 오류 수정]
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\AttachFile.cs

:2014-10-16 by kkzz52 ----------------------------------------------------------
[전자결재 - 수신문서 재상신시에 수신자가 첨부한 첨부파일 삭제되는 버그수정]
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs
	\DeskPlusSolution\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

:2014-10-16 by freeman ----------------------------------------------------------
[공통 - Zip 파일 생성시 가비지 파일로 인한 오류 수정]
	\DeskPlusEIP\Common\Zip\CreateZipFile.aspx.cs

:2014-10-15 by shimpark --------------------------------------------------------------
[설문조사] 관리자 삭제 안되는 현상 및 기타 버그  FIX
	C:\SiteProject\Inoct\DeskPlusSolution\DeskPlusEIP\Opinion\Js\Admin\Content\QuesMod.js
	C:\SiteProject\Inoct\DeskPlusSolution\DeskPlusEIP\Opinion\Ws\Admin\Content\QuesMod.asmx.cs
	C:\SiteProject\Inoct\DeskPlusSolution\DeskPlusEIP\Opinion\Js\Admin\Content\QuesAdd.js


:2014-10-15 by shimpark ----------------------------------------------------------

[주소검색 = DAUM 사용]

new daum.Postcode({
        oncomplete: function (data) {
            var zipcode = data.postcode1 + "" + data.postcode2;

            $("#ctbHomeZipCode").val(zipcode);
            $("#ctbHomeAddress1").val(data.address);
            $("#ctbHomeAddress2").val("");
            $("#ctbHomeAddress2").focus();
        }
    }).open();

    return;

\DeskPlusEIP\Account\Js\Admin\Dept\List.js(399): 
\DeskPlusEIP\Account\Js\Admin\User\Add.js(305): 
\DeskPlusEIP\Account\Js\Admin\User\Add.js(335): 
\DeskPlusEIP\Account\Js\Admin\User\Mod.js(341): 
\DeskPlusEIP\Account\Js\Admin\User\Mod.js(370): 
\DeskPlusEIP\Account\Js\MyInfo\Mod.js(293): 
\DeskPlusEIP\Account\Js\MyInfo\Mod.js(322): 
\DeskPlusEIP\EIP\Js\Admin\Company\Add.js(314): 
\DeskPlusEIP\EIP\Js\Admin\Subsidiary\Add.js(221): 
\DeskPlusEIP\Address\Js\Admin\Content\Add.js(219): 
\DeskPlusEIP\Address\Js\Admin\Content\Add.js(247): 
\DeskPlusEIP\Address\Js\Content\Add.js(239): 
\DeskPlusEIP\Address\Js\Content\Add.js(272): 
\DeskPlusEIP\Approval\Js\Admin\EnforceSeal\Add.js(321): 
\DeskPlusEIP\Approval\Js\Admin\EnforceSeal\Mod.js(324): 
\DeskPlusEIP\Approval\Js\Folder\Enforce\Add.js(70): 
\DeskPlusEIP\Approval\Js\Folder\Enforce\Mod.js(52): 

========================================================================

<script src="http://dmaps.daum.net/map_js_init/postcode.js"></script> 추가

\DeskPlusEIP\Account\Admin\Dept\List.aspx(12):
\DeskPlusEIP\Account\Admin\User\Add.aspx(21):
\DeskPlusEIP\Account\Admin\User\Mod.aspx(20):
\DeskPlusEIP\Account\MyInfo\Mod.aspx(20):
\DeskPlusEIP\EIP\Admin\Company\Add.aspx(25):
\DeskPlusEIP\EIP\Admin\Subsidiary\Add.aspx(20):
\DeskPlusEIP\Address\Admin\Content\Add.aspx(23):
\DeskPlusEIP\Address\Content\Add.aspx(21):
\DeskPlusEIP\Approval\Admin\EnforceSeal\Add.aspx(20):
\DeskPlusEIP\Approval\Admin\EnforceSeal\Mod.aspx(21):
\DeskPlusEIP\Approval\Folder\Enforce\Add.aspx(20):
\DeskPlusEIP\Approval\Folder\Enforce\Mod.aspx(20):


:2014-09-30 by freeman --------------------------------------------------------------
[프로젝트관리] 크롬에서 window.showModalDialog 지원하지 않는 문제
	\DeskPlusSolution\DeskPlusEIP\PMS\Js\Admin\Member\List.js

:2014-09-29 by freeman --------------------------------------------------------------
[전자결재] 크롬에서 window.showModalDialog 지원하지 않는 문제
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js 
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\ModSetting.js      
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\ModSequence.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ConfirmReject.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\Process\Afterward\ConfirmSeqLine.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js

	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx (73)


:2014-09-24 by freeman --------------------------------------------------------------
[전자결재] C Root 에 C:\DeskPlusEIP\FileData\1\Approval\Document\...  생기는 버그 수정
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs (2093)


:2014-09-23 by withsuki --------------------------------------------------------------
[PDF일괄다운로드] - Aspose Component이용, 전자결재만 우선 적용.
	\DeskPlusEIP\Common\Zip\PrintBulk.aspx
	\DeskPlusEIP\App_Data\APPROVAL.xml
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDocProc.aspx
	\DeskPlusEIP\Approval\Folder\Common\View.aspx
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusEIP\Approval\Status\Common\View.aspx
	\Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\AsposeEmail.cs
	\Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\AsposeExcel.cs
	\Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\AsposePDF.cs
	\Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\AsposePPT.cs
	\Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\AsposeWord.cs

:2014-09-05 by SEO --------------------------------------------------------------
[cach 로딩 방법 개선 : 개별로딩 --> 한번에 로딩 처리](사용자 수 만큼 DB Connection 일어 나는 부분을 한번에 처리) 
	D:\pack\40\DeskPlusSolution.root\DeskPlusSolution\Projects\EIPProject\Zeospace.DeskPlus.EIP.DAL\Menu.cs
	D:\pack\40\DeskPlusSolution.root\DeskPlusSolution\Projects\EIPProject\Zeospace.DeskPlus.EIP.BLL\Menu.cs

	SP : DP_EIP_Menu_SelGetUserAllOne

:2014-09-01 by SHIMPARK ----------------------------------------------------------
[게시판 팝업창 : 당일 날짜 오류 발생 버그 fix]
	C:\SiteProject\Inoct\DeskPlusSolution\DeskPlusEIP\EIP\DeskPlusEPDefault.aspx.cs
	C:\SiteProject\Inoct\DeskPlusSolution\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\ListEtc.cs

:2014-09-01 by shine ----------------------------------------------------------
[제안제도]

\DeskPlusEIP\Proposal\Base
\DeskPlusEIP\Proposal\Common
\DeskPlusEIP\Proposal\Content
\DeskPlusEIP\Proposal\Content\Admin
\DeskPlusEIP\Proposal\Content\Judge
\DeskPlusEIP\Proposal\Js\Common
\DeskPlusEIP\Proposal\Js\Content
\DeskPlusEIP\Proposal\Js\Content\Admin
\DeskPlusEIP\Proposal\Js\Content\Judge
\DeskPlusEIP\Proposal\Js\Menu
\DeskPlusEIP\Proposal\Jt\Common
\DeskPlusEIP\Proposal\Jt\Content
\DeskPlusEIP\Proposal\Jt\Content\Admin
\DeskPlusEIP\Proposal\Jt\Content\Judge
\DeskPlusEIP\Proposal\Menu
\DeskPlusEIP\Proposal\Resource
\DeskPlusEIP\Proposal\Ws\Common
\DeskPlusEIP\Proposal\Ws\Content
\DeskPlusEIP\Proposal\Ws\Admin
\DeskPlusEIP\Proposal\Ws\Judge

[Table]
DP_PROP_Attach
DP_PROP_Estimate
DP_PROP_Exam
DP_PROP_Judge
DP_PROP_JudgeUser
[SP]
DP_PROP_ 시작하는 것

[GMU]
QRY_PROP_ 시작하는 것

:2014-08-25 by SHIMPARK ----------------------------------------------------------
[결재 - 게시판 이관기능 : 결재문서 JPG 로 첨부파일 개발]	

	결재 이관기능 게시판에서 처리함.
		\DeskPlusEIP\Board\Common\TransferAppoval.aspx
		\DeskPlusEIP\Board\Common\TransferAppoval.aspx.cs

	결재 form 2개 추가
		\DeskPlusEIP\Approval\Folder\Common\View.aspx

	결재 -> 이관기능 수정
		\DeskPlusEIP\Approval\Js\Folder\Common\View.js

	결재 -> 백업하기 기능
		\DeskPlusEIP\Common\Zip\CreateZipFile.aspx.cs

	결재 백업화일 형식
		\DeskPlusEIP\App_Data\APPROVAL.xml
		(<Item ID="IsBackupFile" Value="IMAGE" Content="결재 본문 백업파일 형식 (MHT, IMAGE, PDF)" />)

:2014-08-25 by SHIMPARK ----------------------------------------------------------
[게시판 - 섬네일 작업]
	\Projects\BoardProject\Zeospace.DeskPlus.Board.BLL\AttachFile.cs
	\DeskPlusEIP\Board\Admin\Content\View.aspx.cs
	\DeskPlusEIP\Board\Content\View.aspx.cs
	\DeskPlusEIP\Board\Content\ViewImpo.aspx.cs
	\DeskPlusEIP\Board\Content\ViewPopup.aspx.cs
	\DeskPlusEIP\Board\Content\ViewPopupImpo.aspx.cs


: 2014-08-22 By famo23 ------------------------------------------------------------
[전자결재] 부서명,직급명에 특수기호 있을때 깨지는 버그 수정
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx.cs

:2014-08-13 by famo23 ----------------------------------------------------------
[자원관리 - 자원한꺼번에 보기 화면에서 자원이 같은 시간에 2개 이상 예약되면 제대로 안 나오는 버그수정]
	\DeskPlusEIP\RM\Content\WholeDay.aspx.cs
	\DeskPlusEIP\RM\Content\WholeWeek.aspx.cs

[자원관리 - 자원한꺼번에 보기 화면에서 예약등록하고 구분값 제대로 못 가져오는 버그수정]
	\DeskPlusEIP\RM\Js\Content\ListMain.js

:2014-08-01 by famo23 ----------------------------------------------------------
[전자결재 - 수신상신자가 문서 재상신시에 dp_App_AppDoc에 안 들어가는 버그수정]
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

[전자결재 - 수신상신자가 상신시에 Approvaltag 잘려서 들어가는 버그수정]	
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ApprovalDocument.cs

: 2014-08-1 By Freeman ------------------------------------------------------------
[전자결재] 제목에 ' 값이 들어 있을때 근거문서 추가 않되는 버그 수정
  \DeskPlusSolution\DeskPlusEIP\Approval\Ws\FormFlow\Basic\GetBaseDoc.asmx.cs(242)

: 2014-07-31 By Freeman ------------------------------------------------------------
[전자결재] find('xscript') 사용시 Error : 알수 없는 런타임 오류 문제 
  \DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js(202)
  \DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js(98)
  \DeskPlusEIP\Approval\Js\Admin\Transfer\View.js(163)
  \DeskPlusEIP\Approval\Js\Admin\Transfer\ViewCir.js(143)
  \DeskPlusEIP\Approval\Js\Common\Popup\View.js(45)
  \DeskPlusEIP\Approval\Js\Common\Popup\ViewCir.js(94)
  \DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js(58)
  \DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js(108)
  \DeskPlusEIP\Approval\Js\Disuse\View.js(117)
  \DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js(183)
  \DeskPlusEIP\Approval\Js\Folder\Common\View.js(257)
  \DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js(150)
  \DeskPlusEIP\Approval\Js\Folder\Common\ViewHistory.js(54)
  \DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js(1806)
  \DeskPlusEIP\Approval\Js\Status\Common\View.js(135)

: 2014-07-29 By Freeman ------------------------------------------------------------
[IE 설정] 신뢰할수 있는 사이트 등록 / 서브 도메인이 다른 시스템(메일)과 연동시 사용
	\DeskPlusEIP\Site\Auth\AddTrustedSite.aspx.cs
	\DeskPlusEIP\Resource\GlobalString\string.ko.resx

	\DeskPlusEIP\App_Data\Config.xml(TrustSiteDomain)
	\DeskPlusEIP\Js\Site\Auth\AddTrustedSite.js


: 2014-07-25 By withsuki ------------------------------------------------------------
[전자결재] 결재망저장
source>
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\FavoriteApprovalNetInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FavoriteApprovalNet.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Ws\FormFlow\Basic\Add.asmx
	리소스
table>
	DP_APP_FavoriteAppNet필드변경
sp>
	DP_APP_FavoriteAppNet_Del
	DP_APP_FavoriteAppNet_Ins
	DP_APP_FavoriteAppNet_Sel

: 2014-07-25 By freeman ------------------------------------------------------------
[메뉴 Cache에 사용자 정보가 없을 경우 다시 한번 Cache 생성 후 시도 하는 방식으로 보완 ]
	\DeskPlusSolution\DeskPlusEIP\EIP\Package\Menu.aspx.cs

: 2014-07-24 By freeman ------------------------------------------------------------
[전자결재 결재 승인시 패스워드 불일치 문제 : 패스워드에 특수문자 & 를 사용할 경우 문제 ]
	\DeskPlusEIP\Approval\Ws\Process\Basic\ConfirmPassword.asmx.cs
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js


: 2014-07-24 By freeman ------------------------------------------------------------
[일정관리 - 주간 달력 헤더 스크롤 고정]
	\DeskPlusEIP\Pim\Js\FullCalendar\fullcalendar.js
	\DeskPlusEIP\Pim\Js\Schedule\ListDay.js
	\DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
	\DeskPlusEIP\Pim\Schedule\ListWeek.aspx

: 2014-07-22 By freeman ------------------------------------------------------------
[LoginOutSide Exception 정리 : Base-64 문자 배열 또는 문자열의 길이가 잘못되었습니다.]
	\DeskPlusEIP\Site\Auth\LoginOutSide.aspx.cs

: 2014-07-22 By freeman ------------------------------------------------------------
[전자결재 첨부파일 다운로드 (jpg,pdf) Exception 정리 : 원격 호스트와 통신하는 동안 오류가 발생했습니다]
	\DeskPlusEIP\Common\Zip\CreateZipFile.aspx.cs

: 2014-07-22 By freeman ------------------------------------------------------------
[전자결재 - 파일백업 시 결재 문서를 MHT / IMAGE / PDF 로 다운로드 받게 함]
	\DeskPlusEIP\App_Data\APPROVAL.xml   IsBackupFile

	\DeskPlusEIP\Common\Zip\CreateZipFile.aspx.cs 
	
	\DeskPlusSolution\Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\HtmlToImage.cs
	\DeskPlusSolution\Projects\ComponentProject\Zeospace.DeskPlus.Component.Converter\HtmlToPDF.cs


	\DeskPlusSolution\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Exceptions\AppException.cs

: 2014-07-18 By freeman ------------------------------------------------------------
[포틀릿 MyPims 관련 Exception 정리 : 문자열을 유효한 DateTime으로 인식할 수 없습니다]
	\DeskPlusEIP\Portlet\Package\MyPim.ascx.cs

: 2014-07-18 By freeman ------------------------------------------------------------
[로그인 관련 Exception 정리 : 스레드가 중단되었습니다.]
	\DeskPlusEIP\Site\Auth\LoginOutSide.aspx.cs

: 2014-07-18 By freeman ------------------------------------------------------------
[메뉴 관련 Exception 정리 : 개체 참조가 개체의 인스턴스로 설정되지 않았습니다]
	\DeskPlusSolution\DeskPlusEIP\EIP\Package\Menu.aspx.cs

: 2014-07-17 By freeman ------------------------------------------------------------
[메일 기능에서 Top 메뉴의 HomeLogo Image 를 클릭할 때 포탈 홈으로 않가는 버그 수정]
	\DeskPlusSolution\DeskPlusEIP\EIP\Package\Menu.aspx

: 2014-07-17 By freeman ------------------------------------------------------------
[메뉴관련 불필요한 Exception 로그 수정]
	\DeskPlusSolution\Projects\EIPProject\Zeospace.DeskPlus.EIP.BLL\Menu.cs

: 2014-07-17 By freeman ------------------------------------------------------------
[포탈 메뉴 버그 - 오른쪽 끝에 있는 메뉴의 서브메뉴가 않보이는 버그]
	\DeskPlusSolution\DeskPlusEIP\EIP\Js\Package\Menu.js

: 2014-07-17 By freeman ------------------------------------------------------------
[메신저 SLO 버그 - Base64 인코딩 문제]
	\DeskPlusEIP\Common\Messenger\UC\SLO.aspx.cs

: 2014-07-16 By freeman ------------------------------------------------------------
[일정관리 버그 - 인쇄시 다음달이 표시되는 현상 / 선택된 일정이 인쇄가 않되는 버그]
	\DeskPlusEIP\Pim\Js\Schedule\ScheduleMain.js 
	\DeskPlusEIP\Pim\Schedule\PrintPage.aspx.cs
	  
:2014-07-11 by famo23 ----------------------------------------------------------
[전자결재 - 수신문서 결재자 없이 상신해서 완결처리 될때 기존 문서 삭제 안되는 버그수정]
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

: 2014-06-30 By famo23 ------------------------------------------------------------
[전자결재 - 회람문서 메일 발송 버그 수정]
	\DeskPlusEIP\Approval\Folder\Carboncopy\SendMail.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\SendMail.aspx.cs
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\SendMail.js
	\DeskPlusEIP\Approval\Ws\Folder\Carboncopy\SendMail.asmx.cs

[전자결재 - 디자인수정]
	\DeskPlusEIP\Approval\Jt\Folder\Common\JTHistoryDoc.htm

[전자결재 - 알람버그수정]
	DP_APP_Seq_SelAlarmComplete
	DP_APP_Seq_SelAlarmNext

[전자결재 - ZQ가 2개 이상일때 결재상황 조회리스트 중복으로 나오는 버그수정]
	\DeskPlusEIP\Approval\Js\Status\Common\GetDocFlow.js

[전자결재 - ZP수신자 알람주소 버그수정]
	\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

[전자결재 - 결재문서관리 리스트 날짜간격수정]
	\DeskPlusEIP\Approval\Jt\Admin\Document\JTList.htm
	\DeskPlusEIP\Approval\Jt\Admin\Document\JTListProcess.htm

:2014-06-26 by hooper2 ----------------------------------------------------------
주소록 추가, 검색(DeepMail)
Address\js\Mail\AddressAdd.js - 추가
Address\js\Mail\AddressSearch.js

Address\Jt\Mail\JTList.htm
Address\Mail\AddressAdd.aspx
Address\Mail\AddressAdd.aspx.cs
Address\Mail\AddressSearch.aspx
Address\Mail\AddressSearch.aspx.cs
리소스 추가

Zeospace.Deskplus.Common.BLL
 - Mail.cs
 - MailDeep.cs

Portlet\Ws\Package\Mail\List.asmx.cs
Portlet\Package\DeepFrame.aspx
Portlet\Package\DeepFrameAdmin.aspx

Site\Auth\ModPassword.aspx.cs

:2014-06-25 by shimpark ----------------------------------------------------------
[모바일 - 승인사용자]

D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\MDM\Auth\ListUse.aspx
D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\MDM\Auth\ListUse.aspx.cs
D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\MDM\Js\Auth\ListUse.js
D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\MDM\Js\Menu\Menu.js
D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\MDM\Ws\Auth\ListUse.asmx.cs
D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\MDM\Jt\Auth\JtListUse.htm


:2014-06-24 by shimpark ----------------------------------------------------------
[상단 메뉴 - 로그아웃]
	C:\SiteProject\SOMO\DeskPlusSolution\DeskPlusEIP\EIP\Package\Menu.aspx

:2014-06-18 by hooper2 ----------------------------------------------------------
[결재] 승인/반려시 알람대상 설정(모두, 사용자지정)
	\DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx, cs
	\DeskPlusEIP\Approval\Process\Basic\AlarmList.aspx, cs
	\DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx		
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx, cs

	\DeskPlusEIP\Approval\Js\Process\Basic\AlarmList.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

	\Projects\Approval\BLL\DocumentProcess.cs
	\Projects\Approval\DAL\Sequence.cs

프로시져
	DP_APP_Seq_SelAlarmRejectAll
	DP_APP_Seq_SelAlarmOpinionAll
	DP_APP_Seq_SelAlarmNextAll

리소스
	\DeskPlusEIP\Approval\Resource		AddAlarmTarget, AlarmTarget,AllAlarmTarget

:2014-06-18 by withsuki ----------------------------------------------------------
[결재] 자주지정하는결재자리스트가 많은 경우, 에디터뒤로 숨는 현상->스크롤처리.
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

	$(oContextMenuCtrl.oEl).attr("style", "height:140px;overflow:auto;");


:2014-06-17 by famo23 ----------------------------------------------------------
[공통 - 처리메세지 오류수정]
	\DeskPlusEIP\Common\Notify\CallNotify.aspx

[자원관리 - 알람포맷변경]
	\DeskPlusEIP\RM\App_Data\en.xml
	\DeskPlusEIP\RM\App_Data\fr.xml
	\DeskPlusEIP\RM\App_Data\ja.xml
	\DeskPlusEIP\RM\App_Data\ko.xml
	\DeskPlusEIP\RM\App_Data\zh-cn.xml
	\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs

[자원관리 - 자원담당자만 승인처리 할수 있게 변경]
	\DeskPlusEIP\RM\Content\Add.aspx.cs
	\DeskPlusEIP\RM\Content\Mod.aspx.cs

[방문예약]	

     * DB작업 
		 기본값 - INSERT INTO [dbo].[DP_COM_Env]
			   ([CompanyID],[Gubun],[KeyID],[KeyValue],[WriteUserID],[WriteDate])
				VALUES (회사아이디,'VISIT','ListCount','10',1,getdate())

		 뷰 - VW_VIS_OpenList / VW_VIS_Share
		 테이블 - DP_VIS_로 시작하는 테이블 
		 프로시저 - DP_VIS_로 시작하는 프로시저
					DP_RM_Reserve_SelByVisit1
					DP_RM_Reserve_SelByVisit2
		 GMU - VISIT 카테고리
	 
	 * 소스작업
		\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\AppSettingHelper.cs
		\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\FileHelper.cs
		\DeskPlusEIP\RM\Content\Add.aspx.cs
		\Projects\RMProject\Zeospace.DeskPlus.RM.BE\ReserveInfo.cs
		\Projects\RMProject\Zeospace.DeskPlus.RM.BLL\Reserve.cs
		\Projects\RMProject\Zeospace.DeskPlus.RM.DAL\Reserve.cs
		VISIT 프로젝트 추가

	 * Design (번역이미지없음)
		 \\192.168.1.20\DeskPlus40\Design\Dev\C\Img\Lang\ko\ico_visit.gif 
		 Func_Visit.css

[공통 - 프린트시 select 박스 값 버그수정]
	\DeskPlusEIP\Html\Js\jQuery\jquery.print.js

[통계 - 모바일접속카운트 안 나오는 버그수정]
	DP_STA_Login_SelTop

[전자결재 - 타부서문서열람권한 다중체크 버그 수정]
	\DeskPlusEIP\Approval\Ws\Admin\FolderAuth\List.asmx.cs

[MDM - 페이징 관련 스크립트 오류 수정]
	\DeskPlusEIP\MDM\Js\Auth\ListAdd.js
	\DeskPlusEIP\MDM\Js\Auth\ListReject.js

[전자결재 - 결재대상문서에서 상신자상세화면 버그 수정]
	\DeskPlusEIP\Approval\Ws\Status\ListApprove.asmx.cs
	\DeskPlusEIP\Approval\Jt\Status\JTListApprove.htm	
        \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentProcessInfo.cs

[마이톡 - 담벼락 글쓰기 버그수정]
	\DeskPlusEIP\SNS\Js\Common\MyTalkGroup.js
	\DeskPlusEIP\SNS\Common\MyTalkGroup.aspx

[전자결재 - 자주지정하는 결재자 팝업버그수정]
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

[전자결재 - 결재자가 본문/결재선 수정할때 ZA단계가 지난상태에서 ZA추가할 수 있는 버그 수정]
    리소스추가
	QRY_APP_Seq_SelZA
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

[전자결재 - 결재자가 본문/결재선 수정할때 ZA가 2개 이상일때 다음결재자 Sequence 잘못들어가는 버그 수정]
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs

[전자결재 - ZA가 결재차례일 경우에 진행중인문서,관계있는문서,결재대상문서 리스트 잘못나오는 버그 수정]
	DP_APP_DocProcess_SelPageListApprove
	DP_APP_DocProcess_SelPageListRelated
	DP_APP_DocProcess_SelPageListProcess

[전자결재 - 서명을 생성하지 않고 확인을 누르지 못하도록 처리 (빈 이미지 생성을 못하게 함) 30pixcel 이상 ]
	\DeskPlusEIP\Approval\Common\Popup\GetSign.aspx
	\DeskPlusEIP\Approval\Js\Common\Popup\GetSign.js 
	\DeskPlusEIP\Approval\Resource\GlobalString\*.*  MSG_GETSIGN 추가   

:2014-06-16 by famo23 ----------------------------------------------------------
[전자결재 - 폐기문서함]

	QRY_APP_Form_Sel
	DP_APP_Doc_UpdDisuse
	DP_APP_Doc_SelPageCntDisuse
	DP_APP_Doc_SelPageListDisuse
	DP_APP_Attach_SelDisuse
	DP_APP_Attach_SelDisuseCnt
	DP_APP_BaseDoc_SelDisuse
	DP_APP_BaseDoc_SelDisuseCnt
	
	리소스 추가
	\DeskPlusEIP\Approval\Disuse - 연관js,jt,ws 추가

	\DeskPlusEIP\Approval\Ws\Admin\Document\ViewDoc.asmx.cs
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx.cs
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js	
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\BaseDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\AttachFile.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Form.cs

:2014-06-13 by shine ----------------------------------------------------------
[자원관리 모아보기]
	DeskplusEIP\Content\ListMain.aspx
	DeskplusEIP\Content\ListMain.aspx.cs
	DeskplusEIP\Content\WholeDay.aspx
	DeskplusEIP\Content\WholeDay.aspx.cs
	DeskplusEIP\Content\WholeWeek.aspx
	DeskplusEIP\Content\WholeWeek.aspx.cs
	DeskplusEIP\Content\ListDay.aspx
	DeskplusEIP\Content\ListDay.aspx.cs
	DeskplusEIP\Content\ListWeek.aspx
	DeskplusEIP\Content\ListWeek.aspx.cs
	DeskplusEIP\Content\ListMonth.aspx
	DeskplusEIP\Content\ListMonth.aspx.cs
	DeskplusEIP\Content\Add.aspx
	DeskplusEIP\Content\Add.aspx.cs
	DeskplusEIP\Content\AddN.aspx
	DeskplusEIP\Content\Mod.aspx
	DeskplusEIP\Content\Mod.aspx.cs
	DeskplusEIP\Content\ModN.aspx
	DeskplusEIP\Content\View.aspx
	DeskplusEIP\Content\View.aspx.cs

	DeskplusEIP\Js\Content\WholeDay.js
	DeskplusEIP\Js\Content\WholeWeek.js
	DeskplusEIP\Js\Content\WholeMonth.js
	DeskplusEIP\Js\Content\View.js
	DeskplusEIP\Js\Content\Mod.js
	DeskplusEIP\Js\Content\ListDay.js
	DeskplusEIP\Js\Content\ListMain.js
	DeskplusEIP\Js\Content\ListMonth.js
	DeskplusEIP\Js\Content\ListWeek.js

	DeskplusEIP\Jt\Content\JTWholeMonth.htm

	DeskplusEIP\Ws\Content\WholeMonth.asmx

	Zeospace.DeskPlus.RM.BE\ReserveInfo.cs
	Zeospace.DeskPlus.RM.DAL\Reserve.cs

	[table]
	DP_RM_Reserve 필드추가 UserImg

	[sp,gmu,view]
	DP_RM_Reserve_SelWholeList
	DP_RM_Reserve_Ins
	DP_RM_Reserve_Mod
	DP_RM_Reserve_SelWeekList
	QRY_RM_Reserve_Sel
	VW_RM_Reserve
	VW_RM_ReserveList

:2014-06-13 by hooper2 ----------------------------------------------------------
[TOP5 (최다등록자/최다조회글)포틀릿]
	\DeskPlusEIP\Portlet\Package\Board\CountTab.ascx
	\DeskPlusEIP\Portlet\Package\Board\LargestRegistrant.ascx
	\DeskPlusEIP\Portlet\Package\Board\LargestCheckWriting.ascx

	\DeskPlusEIP\Portlet\Jt\Package\Board\JTLargestRegistrant.htm
	\DeskPlusEIP\Portlet\Jt\Package\Board\JTLargestCheckWriting.htm

	\DeskPlusEIP\Portlet\Ws\Package\Board\List.asmx\GetLargestRegistrant

	\DeskPlusEIP\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\ListInfo.cs
	\DeskPlusEIP\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\Lists.cs

프로시져
	DP_BOA_LargestRegistrant_Sel
	DP_BOA_LargestCheckWriting_Sel

코드 추가
	CodeID = TOP_LIST

:2014-06-11 by hooper2 ----------------------------------------------------------
[문서작성시 미리보기 인쇄]
	\DeskPlusEIP\Approval\Common\Popup\PreView.aspx
	\DeskPlusEIP\Approval\Js\Common\Popup\PreView.js

[결재 알람 개인 설정]
	\DeskPlusEIP\Account\MyInfo\Mod.aspx
	\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs

	\DeskPlusEIP\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserEnvironmentInfo.cs
	\DeskPlusEIP\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEnvironment.cs
	\DeskPlusEIP\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

테이블 컬럼 추가
	DP_ACC_UserEnv 테이블에 AlarmMail, AlarmMessenger, AlarmSMS 추가

프로시져 추가
	DP_ACC_UserEnv_UpdAppAlarm

리소스 추가
	SetApprovalAlarm
	PasswordChange
	AlarmMail
	AlarmMessenger
	AlarmSMS


:2014-06-11 by shimpark ----------------------------------------------------------
[게시판 - 답변글에 원문글 표기]
- 소스
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Board\Content\Reply.aspx
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Board\Content\ReplyN.aspx

	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Board\Content\Reply.aspx.cs
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Reply.js

- 리소스
	<data name="BeforeContent" xml:space="preserve">
		<value>원문글</value>
	</data>



:2014-06-11 by shimpark ----------------------------------------------------------
[포탈 - 메인]
- 오류 발생 시, 체크
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\EIP\Package\Main.aspx.cs

:2014-06-10 by shimpark ----------------------------------------------------------
[포틀릿 - 생일자]

페이지
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Congratulation.ascx
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Congratulation.ascx.cs

	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Portlet\Jt\Package\JTCongratulation.htm
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Portlet\Ws\Package\Congratulation.asmx
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Portlet\Ws\Package\Congratulation.asmx.cs

	D:\Site\DeskPlus40\DeskPlusSolution\Projects\PortletProject\Zeospace.DeskPlus.Portlet.BE\CongratulationInfo.cs
	D:\Site\DeskPlus40\DeskPlusSolution\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\Congratulation.cs

옵션추가
	- D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\App_Data\BOARD.xml

	<Item ID="UseReplyBeforeContent" Value="Y"  Content="답변 글 작성시, 기존 원본글 에디터 보이기" />

GMU 수정	
	QRY_PORTLET_Congratulation_Sel

	select  'B' as num, 
		substring(BirthDay,5,4) as congratulation, 
		CompanyID, UserID, 
		NameBase, NameKo, NameEn, NameFr, NameJa, NameZhcn, NameZhtw,
		TitleBase, TitleKo, TitleEn, TitleFr, TitleJa, TitleZhcn, TitleZhtw,
		DutyBase, DutyKo, DutyEn, DutyFr, DutyJa, DutyZhcn, DutyZhtw,
		DeptBase, DeptKo, DeptEn, DeptFr, DeptJa, DeptZhcn, DeptZhtw,
		PhotoImage, Lunar, BirthDay, WeddingAnniversary
		from vw_userinfo 
		where CompanyID = @CompanyID 
		and len(BirthDay) = 8
		and ActiveUser = '1' and BaseDept = '1'
		UNION ALL
		select 'W' as num, 
		substring(WeddingAnniversary,5,4) as congratulation, 
		CompanyID, UserID, 
		NameBase, NameKo, NameEn, NameFr, NameJa, NameZhcn, NameZhtw,
		TitleBase, TitleKo, TitleEn, TitleFr, TitleJa, TitleZhcn, TitleZhtw,
		DutyBase, DutyKo, DutyEn, DutyFr, DutyJa, DutyZhcn, DutyZhtw,
		DeptBase, DeptKo, DeptEn, DeptFr, DeptJa, DeptZhcn, DeptZhtw,
		PhotoImage, Lunar, BirthDay, WeddingAnniversary
		from vw_userinfo  
	where CompanyID = @CompanyID 
	and len(WeddingAnniversary) = 8
	and ActiveUser = '1' and BaseDept = '1'
	order by num, congratulation asc



:2014-06-10 by shimpark ----------------------------------------------------------
[업무협업 - 자주지정자]

테이블 생성
	DP_COOP_ItemFavUser

저장프로시저
	[DP_COOP_ItemFavUser_Del]
	[DP_COOP_ItemFavUser_Ins]
	[DP_COOP_ItemFavUser_SelPageCnt]
	[DP_COOP_ItemFavUser_SelPageList]
	[DP_COOP_ItemFavUser_UpdOrder]

GMU
	INSERT INTO DP_QUERY (Category,qryid,qryvalue,owner,memo,regdate) VALUES ('COOP','QRY_COOP_ItemFavUser_Sel','SELECT a.*,
		b.NameBase,
		b.NameKo,
		b.NameEn,
		b.NameJa,
		b.NameFr,
		b.NameZhcn,
		b.NameZhtw
	FROM DP_COOP_ItemFavUser a
	INNER JOIN VW_USERINFO b ON a.CompanyID = b.CompanyID
		AND a.UserID = b.UserID
	WHERE a.CompanyID = @CompanyID
		AND a.ItemSubID = @ItemSubID
		AND b.ActiveUser = ''1''
		AND BaseDept = ''1''
	ORDER BY a.Orders
	','심재운','자주지정사용자','20140610')

BE
	\DeskPlusSolution\Projects\CoopProject\Zeospace.DeskPlus.Coop.BE\ItemFavUserInfo.cs

DAL
	\DeskPlusSolution\Projects\CoopProject\Zeospace.DeskPlus.Coop.DAL\ItemFavUser.cs

JS, JT, WS 의 각 폴더 내부 파일 패치
	\DeskPlusSolution\DeskPlusEIP\Coop\Js\Admin\Category
	\DeskPlusSolution\DeskPlusEIP\Coop\Jt\Admin\Category
	\DeskPlusSolution\DeskPlusEIP\Coop\Ws\Admin\Category

: 2014-06-10 By hooper2 ------------------------------------------------------------
[Rolling Portlet]
	\DeskPlusEIP\Portlet\Package\Board\NoticeImage.ascx
	\DeskPlusEIP\Portlet\Jt\Package\Board\JTNoticeImage.htm
	\DeskPlusEIP\Portlet\Ws\Package\Board\Album.asmx		GetBoardPageList() 함수의 스크린샷 부분 수정

[GM Words Portlet]
	\DeskPlusEIP\Portlet\Package\Board\GMWords.ascx
	\DeskPlusEIP\Portlet\Jt\Package\Board\JTGMWords.htm
	
	\DeskPlusEIP\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\CEOBoard.cs

	DP_QUERY 테이블의 QRY_PORTLET_CEOBoard_Sel qryid 추가

[주소록리스트에 회사명 추가]
	\DeskPlusEIP\Address\Jt\Content\JTList.htm

[전달/회람시 알람]
	\DeskPlusEIP\Approval\Folder\Dept\ForwardMemo.aspx
	\DeskPlusEIP\Approval\Folder\Dept\ForwardMemo.aspx.cs
	\DeskPlusEIP\Approval\Js\Folder\Dept\ForwardMemo.js
	\DeskPlusEIP\Approval\Folder\Dept\CirMemo.aspx
	\DeskPlusEIP\Approval\Folder\Dept\CirMemo.aspx.cs
	\DeskPlusEIP\Approval\Js\Folder\Dept\CirMemo.js
	\DeskPlusEIP\Approval\Ws\Status\ListProcess.asmx.cs

	\DeskPlusEIP\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\DeskPlusEIP\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Sequence.cs
	\DeskPlusEIP\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DocumentProcessInfo.cs
	\DeskPlusEIP\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

	[SP 추가]
	DP_APP_Seq_SelAlarmForward
	DP_APP_Seq_SelAlarmCirculation

: 2014-06-10 by shine ------------------------------------------------------------
[팝업]
	\DeskPlusSolution\DeskPlusEIP\BoardPopUp.aspx 추가
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Add.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Content\AddN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Add.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\AddN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Mod.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Content\ModN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Mod.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Mod.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\ModN.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Add.js
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\Mod.js
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Add.js
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Content\Mod.js
	\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\View.js
	\DeskPlusEIP\EIP\DeskPlusEPDefault.aspx
	\DeskPlusEIP\EIP\DeskPlusEPDefault.aspx.cs

	\DeskPlusSolution\Projects\BoardProject\Zeospace.DeskPlus.Board.BE\CategoryInfo.cs
	\DeskPlusSolution\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\Category.cs
	\DeskPlusSolution\Projects\BoardProject\Zeospace.DeskPlus.Board.DAL\ListEtc.cs

	[table]
	DP_BOA_Category의 OptionPopUp 필드 추가
	[sp 수정]
	DP_BOA_Category_Ins
	DP_BOA_Category_Upd
	[GMU 추가]
	QRY_BOA_ListEtc_Sel

[휴가내역관리]
	\DeskPlusSolution\DeskPlusEIP\Etc\Holiday\AdminManage.aspx 추가
	\DeskPlusSolution\DeskPlusEIP\Etc\Js\Holiday\AdminManage.js 추가
	\DeskPlusSolution\DeskPlusEIP\Etc\JT\Holiday\JTAdminManage.htm 추가
	\DeskPlusSolution\DeskPlusEIP\Etc\Ws\Holiday\AdminManage.asmx 추가

	\DeskPlusSolution\Projects\EtcProject\Zeospace.DeskPlus.Etc.DAL\HoliCat.cs 추가
	\DeskPlusSolution\Projects\EtcProject\Zeospace.DeskPlus.Etc.DAL\HoliUsed.cs

	[Table 추가]
	DP_HOLI_Cat
	[Table 수정]
	DP_HOLI_Used의 DeptCodeID 추가
	[sp 추가]
	DP_HOLI_Used_SelSrhPageCnt
	DP_HOLI_Used_SelSrhPageList
	[GMU 추가]
	QRY_HOLI_Cat_Sel

: 2014-05-27 By shimpark ------------------------------------------------------------
[게시판 통계 : FusionCharts 사용]

	\DeskPlusSolution\DeskPlusEIP\Board\Js\Admin\Menu\Menu.js
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Statistic\LineChart.aspx
	\DeskPlusSolution\DeskPlusEIP\Board\Admin\Statistic\LineChart.aspx.cs
	\DeskPlusSolution\DeskPlusEIP\Util\Component\FusionCharts 폴더 하위 추가
	\DeskPlusSolution\DeskPlusEIP\Util\Component\FusionCharts.dll
	\DeskPlusSolution\DeskPlusEIP\Util\Component\FusionCharts.xml

	- 게시판 FusionCharts.dll 참조하기

:2014-05-14 by 심재운 ----------------------------------------------------------
[게시판 본문 이미지 보기 섬네일]
- ie 11 및 firefox 사이즈 조절 버그 fixed
	\DeskPlusEIP\Html\Js\jQuery\jquery.resize.js

:2014-05-13 by 심재운 ----------------------------------------------------------
[.trim() = > .trimText();] 치환

	\DeskPlusEIP\WebHard\Js\Content\ListIcon.js(438,60):        var attachFileName = $("#fileName_" + el.id).text().trimText();
	\DeskPlusEIP\WebHard\Js\Content\ListIcon.js(1130,62):        var attachFileName = $("#fileName_" + _fileID).text().trimText();
	\DeskPlusEIP\WebHard\Js\Content\ListImpo.js(461,61):        var attachFileName = $("#fileName_" + FileID).text().trimText();
	\DeskPlusEIP\WebHard\Js\Content\ListImpoIcon.js(444,61):        var attachFileName = $("#fileName_" + FileID).text().trimText();
	\DeskPlusEIP\WebHard\Js\Content\ListImpoPreview.js(444,61):        var attachFileName = $("#fileName_" + FileID).text().trimText();
	\DeskPlusEIP\WebHard\Js\Content\ListPreview.js(435,60):        var attachFileName = $("#fileName_" + el.id).text().trimText();
	\DeskPlusEIP\WebHard\Js\Content\ListPreview.js(1128,62):        var attachFileName = $("#fileName_" + _fileID).text().trimText();
	\DeskPlusEIP\WebHard\Js\Trash\List.js(380,56):    var attachFileName = $("#fileName_" + el.id).text().trimText();
	\DeskPlusEIP\WebHard\Js\Trash\ListIcon.js(362,56):    var attachFileName = $("#fileName_" + el.id).text().trimText();
	\DeskPlusEIP\WebHard\Js\Trash\ListIcon.js(674,58):    var attachFileName = $("#fileName_" + _fileID).text().trimText();
	\DeskPlusEIP\WebHard\Js\Trash\ListPreview.js(362,56):    var attachFileName = $("#fileName_" + el.id).text().trimText();
	\DeskPlusEIP\WebHard\Js\Trash\ListPreview.js(673,58):    var attachFileName = $("#fileName_" + _fileID).text().trimText();
	\DeskPlusEIP\Portlet\Js\Package\SMS\common.js(462,52):			bm_iframe.ifrm_bmname.value = bookmarkname.value.trimText();
	\DeskPlusEIP\PMS\Js\Admin\Content\JListClose.js(445,41)://	var searchValue = $("tbSearch").value.trimText();
	\DeskPlusEIP\PMS\Js\Admin\Content\ListClose.js(57,41)://	var searchValue = $("tbSearch").value.trimText();
	\DeskPlusEIP\PMS\Js\Admin\Content\ListOpen.js(46,41)://	var searchValue = $("tbSearch").value.trimText();
	\DeskPlusEIP\PMS\Js\Admin\Member\List.js(52,41)://	var searchValue = $("tbSearch").value.trimText();
	\DeskPlusEIP\PMS\Js\Member\List.js(38,45):    //	var searchValue = $("tbSearch").value.trimText();
	\DeskPlusEIP\PMS\Js\Project\List.js(68,44)://    var searchValue = $("tbSearch").value.trimText();
	\DeskPlusEIP\Search\Js\List.js(322,30):    SearchValue = SearchValue.trimText(); // 앞뒤 공백 삭제

:2014-02-28 by famo23 ----------------------------------------------------------
[전자결재 - LastDocID 잘못 들어가는 버그 수정]
	DP_APP_DocProcess_Ins

:2014-02-20 by famo23 ----------------------------------------------------------
[전자결재 - 근거문서에 전달문서함 추가개발]
	\DeskPlusEIP\Approval\FormFlow\Basic\GetBaseDoc.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\GetBaseDoc.js
	\DeskPlusEIP\Approval\Ws\FormFlow\Basic\GetBaseDoc.asmx.cs

: 2014-02-18 By freeman ----------------------------------------------------------
[우편번호] 도로명 검색 
	\DeskPlusEIP\Common\Js\ZipCode\List.js(74, 154~, 301)
	\DeskPlusEIP\Common\Jt\ZipCode\JTListNew.htm


: 2014-02-13 By withsuki ----------------------------------------------------------
[전자결재] 비밀번호에 특수문자가 있는 경우,  결재승인불가 (에러메세지 : 비밀번호가 맞지않습니다).
	\DeskPlusEIP\Approval\Ws\Process\Basic\ConfirmPassword.asmx.cs

:2014-02-10 by famo23 ----------------------------------------------------------
[자원관리 - 자원트리 버그 수정]
	\DeskPlusEIP\RM\Content\AddResource.aspx.cs

: 2014-02-06 By withsuki ----------------------------------------------------------
[전자결재] 커스텀연동 (_Cancel.aspx) 호출부분 변경.
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js (Process.SuccessCustomChk함수부분)


: 2014-02-05 By shimpark ------------------------------------------------------------
[파일upload 시, cookie 미사용시 버그 fix]
	\DeskPlusSolution\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\FileHelper.cs

: 2014-02-05 By shimpark ------------------------------------------------------------
[로긴 화면 회사 마크 수정]
	D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Site\Auth\Default.aspx.cs

: 2014-01-27 By shimpark ------------------------------------------------------------
[디자인 스크롤 미발생 - ie 10 이상] 
[사용자의 부서 넣는곳 : overflow:auto; 추가]	
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Account\Admin\User\UserDept\List.aspx
[포틀릿의 권한 추가 화면]	
	D:\Site\DeskPlus40\DeskPlusSolution\DeskPlusEIP\EIP\Admin\Portlet\AddAuth.aspx

: 2014-01-24 By shine ------------------------------------------------------------
[전자결재 - 개인보관함의 임시보관함에서 존재하는 문서 중 
		   이미지 새로 저장 시 이미지가 안나오는 버그 수정]
  \DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx

:2014-01-24 by famo23 ----------------------------------------------------------
[자원관리 - 자원설명 엔터값 적용 안된 버그수정]
	\DeskPlusEIP\RM\Admin\Content\View.aspx.cs
	\DeskPlusEIP\RM\Content\AddResource.aspx
	\DeskPlusEIP\RM\Js\Content\AddResource.js
	\DeskPlusEIP\RM\Ws\Content\AddResource.asmx.cs

:2014-01-23 by famo23 ----------------------------------------------------------
[게시판 - 리스트 정렬 스타일 안 바끼는 버그 수정
          중요표시리스트에서 등록일로 정렬 할때 버그 수정]
	\DeskPlusEIP\Board\Js\Content\List.js
	\DeskPlusEIP\Board\Jt\Content\JTList.htm
	\DeskPlusEIP\Board\Js\Content\ListImpo.js
	\DeskPlusEIP\Board\Jt\Content\JTListImpo.htm
	\DeskPlusEIP\Board\Js\Temp\List.js	
	\DeskPlusEIP\Board\Jt\Temp\JTList.htm
	\DeskPlusEIP\Board\Js\Admin\Content\List.js
	\DeskPlusEIP\Board\Jt\Admin\Content\JTList.htm
	\DeskPlusEIP\Board\Js\Admin\Template\List.js
	\DeskPlusEIP\Board\Jt\Admin\Template\JTTemplate.htm

[임직원조회 - input박스 크기조정]
	\DeskPlusEIP\Account\Search\List.aspx

[업무협업 - 관리자페이지 상태값 디자인 변경안된 버그수정]
	\DeskPlusEIP\Coop\Admin\Content\List.aspx
	\DeskPlusEIP\Coop\Admin\Content\List.aspx.cs

[사내SNS - 기간검색 리소스 버그수정]
	\DeskPlusEIP\SNS\WorkTalk\Content\List.aspx

[프로젝트관리 - ddlListCount 값이랑 리스트 갯수랑 안 맞는 버그수정]
	\DeskPlusEIP\PMS\Admin\Content\JListClose.aspx.cs
	\DeskPlusEIP\PMS\Admin\Content\JListLinked.aspx.cs
	\DeskPlusEIP\PMS\Admin\Content\JListOpen.aspx.cs

:2014-01-21 by famo23 ----------------------------------------------------------
[커뮤니티 - 기간검색 리소스 버그수정]
	\DeskPlusEIP\Community\Admin\Site\ListClose.aspx

[전자결재 - 결재대상문서에 회람버튼 보이게 처리]
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx
	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs

:2014-01-17 by famo23 ----------------------------------------------------------
[게시판 - ETC2 값 없는 게시물 중요표시 갤러리형에서 버그수정]
	\DeskPlusEIP\Board\Ws\Content\GalleryListImpo.asmx.cs

[모바일메뉴권한관리개발 (관리자페이지->연동관리->모바일메뉴관리) / 모바일도 개발]
	MDMProject -> MenuAuth 추가개발

:2014-01-16 by famo23 ----------------------------------------------------------
[커뮤니티 - 크롬에서 탭스타일 버그 수정]
	Func_Community.css

:2014-01-14 by famo23 ----------------------------------------------------------
[일정관리 - 크롬에서 탭스타일 버그 수정]
	Func_Pim.css

[전자결재 - 공문관리 기간검색 달력스타일적용 안된 버그 수정
			기간검색 상신일이 아닌 등록일로 문구 버그 수정]
	\DeskPlusEIP\Approval\Folder\Enforce\List.aspx
	\DeskPlusEIP\Approval\Folder\Enforce\ListSend.aspx
	\DeskPlusEIP\Approval\Folder\Enforce\ListSubmit.aspx
	\DeskPlusEIP\Approval\Folder\EnforceAdmin\ListSend.aspx
	\DeskPlusEIP\Approval\Folder\EnforceAdmin\ListSubmit.aspx
	\DeskPlusEIP\Approval\Js\Folder\Enforce\ListSend.js
	\DeskPlusEIP\Approval\Js\Folder\Enforce\ListSubmit.js
	\DeskPlusEIP\Approval\Js\Folder\EnforceAdmin\ListSend.js
	\DeskPlusEIP\Approval\Js\Folder\EnforceAdmin\ListSubmit.js

[전자결재 - 수신자 대결표시 없는 버그 수정]
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx

[전자결재 - 부서,직급변경, 대결자 결재, 겸직관련 버그 수정 - 모바일도 수정]	
	(결재시 부서 변경되면 부서정보 잘못 들어가는 버그)
	(수신문서 상신시에 부서 변경되면 부서정보 잘못 들어가는 버그)
	(수신문서 본문수정시에 부서 변경되면 수신자 안 나오는 버그)
	(수신문서 재상신시에 부서 변경되면 사용자 정보 안 나오는 버그)
	(결재할때 겸직정보를 가지고 오지 못하는 버그)
	(대결자가 수신문서 반려할때, 수신완료할때 대결자정보 잘못 들어가는 버그)
	(대결자가 수신문서 상신할때 DB에 정보틀리게 들어가는 버그)
	(겸직:최상위부서정보변경안되는버그/상신자정보변경안되는버그/Number1값잘못들어가는버그등..)
	(겸직:상신자가 재상신할때 부서정보가 바끼는 버그/수신자의 부서,직급,직책정보를 못 가지고 오는 버그등..)
	(임시저장, 상신문서함에서 문서불러올때 상신자부서정보가 변경되면 상신자 안 나오는 버그 수정)
	
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs

	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeModReceive.aspx.cs

	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs

	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs

	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js

	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeEdit.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeLegacy.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeMod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeModReceive.js

	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\List.js

	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\List.js

	\DeskPlusEIP\Approval\Js\Preference\FavoriteForm\Add.js

	\DeskPlusEIP\Approval\Process\Basic\Approve.aspx.cs
	\DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs

	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx

    \DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js

	\DeskPlusEIP\Approval\Ws\FormFlow\Common\Common.asmx.cs
    \DeskPlusEIP\Approval\Ws\FormFlow\Submit\Add.asmx.cs

	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

	QRY_ACC_User_SelByUserIDDeptID
	DP_APP_CounterPartUser_UpdDecide
	DP_APP_PassedDoc_UpdDecide
	
	리소스추가	

:2014-01-13 by famo23 ----------------------------------------------------------
[전자결재 - 다중태크,단일태그 둘다 되는 태그들(ZH,ZQ..) 지정 버그 수정
			(임시저장, 상신문서함에서 문서불러올 때/문서수정할때 다중태그 사용자 정보 안 나오는 버그)]
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
    \DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js

[전자결재 - 양식관리에서 양식안에 클릭시 객체 못 찾는 버그 수정]
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\Add.js

[전자결재 - 수신문서 본문/결재선 수정할때 열람권한, 문서종류 값 잘못 들어가는 버그 수정]
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js

[전자결재 - 조직도에서 결재자선택후에 자주지정하는 결재자 팝업 안 없어지는 버그 수정]
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Common.js

[전자결재 - 공문직인관리 기간검색 달력 스타일 적용 안된 버그 수정]
	\DeskPlusEIP\Approval\Admin\EnforceSeal\List.aspx

[전자결재 - 공문 직인관리 계열사명 검색 버그 수정]
	\DeskPlusEIP\Approval\Admin\EnforceSeal\List.aspx.cs

[전자결재 - 공문 직인관리 리소스 버그 수정(상신일이 아닌 등록일자)]
	\DeskPlusEIP\Approval\Admin\EnforceSeal\List.aspx
	\DeskPlusEIP\Approval\Js\Admin\EnforceSeal\List.js

[전자결재 - 회람 발신부서상신자검색 버그 수정]
	DP_APP_CirDoc_SelPageListRecv
	DP_APP_CirDoc_SelPageCntRecv
	DP_APP_CirDoc_SelPageCntSend
	DP_APP_CirDoc_SelPageListSend

[전자결재 - 기간검색 버그 수정 (완료일이 아닌 상신일로 수정- 회람,관리자페이지 진행중인문서 검색버그,리소스버그 등으로 통일성있게 모든 기간은 상신일로 통일처리)
			FileCount 값 sp 수정]
	DP_APP_CirDoc_SelPageListRecv
	DP_APP_CirDoc_SelPageCntRecv
	DP_APP_CirDoc_SelPageCntSend
	DP_APP_CirDoc_SelPageListSend
	DP_APP_Doc_SelPageCntApp
	DP_APP_Doc_SelPageCntAppAll
	DP_APP_Doc_SelPageCntAppFolder
	DP_APP_Doc_SelPageCntArb
	DP_APP_Doc_SelPageCntArbAll
	DP_APP_Doc_SelPageCntArbFolder
	DP_APP_Doc_SelPageCntCarbon
	DP_APP_Doc_SelPageCntCarbonAdmin
	DP_APP_Doc_SelPageCntDept
	DP_APP_Doc_SelPageCntDeptAdmin
	DP_APP_Doc_SelPageCntDeptAll
	DP_APP_Doc_SelPageCntDeptFolder
	DP_APP_Doc_SelPageCntDisuse
	DP_APP_Doc_SelPageCntEnforce
	DP_APP_Doc_SelPageCntForm
	DP_APP_Doc_SelPageCntForward
	DP_APP_Doc_SelPageCntPrivate
	DP_APP_Doc_SelPageCntPrivateAll
	DP_APP_Doc_SelPageCntPrivateFolder
	DP_APP_Doc_SelPageCntPrivateSubmit
	DP_APP_Doc_SelPageCntSendRecv
	DP_APP_Doc_SelPageListApp
	DP_APP_Doc_SelPageListAppAll
	DP_APP_Doc_SelPageListAppFolder
	DP_APP_Doc_SelPageListArb
	DP_APP_Doc_SelPageListArbAll
	DP_APP_Doc_SelPageListArbFolder
	DP_APP_Doc_SelPageListCarbon
	DP_APP_Doc_SelPageListCarbonAdmin
	DP_APP_Doc_SelPageListDept
	DP_APP_Doc_SelPageListDeptAdmin
	DP_APP_Doc_SelPageListDeptAll
	DP_APP_Doc_SelPageListDeptFolder
	DP_APP_Doc_SelPageListDisuse
	DP_APP_Doc_SelPageListEnforce
	DP_APP_Doc_SelPageListForm
	DP_APP_Doc_SelPageListForward
	DP_APP_Doc_SelPageListPrivate
	DP_APP_Doc_SelPageListPrivateAll
	DP_APP_Doc_SelPageListPrivateFolder
	DP_APP_Doc_SelPageListPrivateSubmit
	DP_APP_Doc_SelPageListSendRecv
	DP_APP_DocProcess_SelPageCntComplete
	DP_APP_DocProcess_SelPageCntForm
	DP_APP_DocProcess_SelPageCntProcess
	DP_APP_DocProcess_SelPageListForm
	\DeskPlusEIP\Approval\FormFlow\Basic\GetBaseDoc.aspx
	\DeskPlusEIP\Approval\Ws\FormFlow\Basic\GetBaseDoc.asmx.cs

[전자결재 - 임시보관함 기간검색 리소스 버그 수정 (상신일자가 아닌 등록일자)]
	\DeskPlusEIP\Approval\FormFlow\Temporary\List.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\List.js

[전자결재 - 임시보관함 검색 조건 리소스 수정(제목->임시저장명/양식명->문서명)(기안년도 검색조건 제거)]
	\DeskPlusEIP\Approval\FormFlow\Temporary\List.aspx.cs

[전자결재 - 양식관리 폴더검색 버그 수정(초기값이 잘못 들어가 있음)]
	UPDATE DP_APP_Folder
	SET  ManyName = NameBase + '|' + Nameko + '|' + NameEn + '|' + NameJa + '|' + NameZhcn + '|' + NameZhtw + '|'
	WHERE Folderid in (1,2,3,4)
	
[전자결재 - 결재정보확인에서 수신자가 반려한 표시 안 나오는 버그 수정
			결재정보확인에서 수신자, 수신확인 아이콘 반대로 나오는 버그 수정
			결재정보확인에서 반려대상자아이콘 안 나오는 버그 수정]
	\DeskPlusEIP\Approval\Js\Status\Common\GetDocFlow.js

[전자결재 - 수신자가 반려할때 결재의견 입력칸이 2군데인 버그 수정]
	\DeskPlusEIP\Approval\FormFlow\Receive\ConfirmReject.aspx - 추가
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ConfirmReject.js - 추가
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js	

[전자결재 - 결재의견에서 엔터값 적용 안되는 버그 수정]
	\DeskPlusEIP\Approval\Js\Folder\EnforceAdmin\ApproveConfirm.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	\DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js
	\DeskPlusEIP\Approval\Js\Process\Opinion\ConfirmOpinion.js

[전자결재 - 대결자가 ZP,이관문서 상신했을 경우 개인결재함에 안 나오는 버그 수정]
	\DeskPlusEIP\Approval\Folder\Private\List.aspx
	\DeskPlusEIP\Approval\Folder\Private\ListFolder.aspx
	\DeskPlusEIP\Approval\Js\Folder\Private\List.js
	\DeskPlusEIP\Approval\Js\Folder\Private\ListFolder.js
	\DeskPlusEIP\Approval\Jt\Folder\Private\JTList.htm
	\DeskPlusEIP\Approval\Jt\Folder\Private\JTListFolder.htm
	\DeskPlusEIP\Approval\Ws\Folder\Private\List.asmx.cs
	\DeskPlusEIP\Approval\Ws\Folder\Private\ListFolder.asmx.cs
	DP_APP_Doc_SelPageListPrivate
	DP_APP_Doc_SelPageCntPrivate
	DP_APP_DocProcess_UpdConclude
	DPM_APP_DocProcess_UpdConclude

[전자결재 - 대결자가 ZQ문서 상신했을 경우에 대결자정보가 결재문서함에 안 나오는 버그 수정]
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

[전자결재 - ZQ문서 열람권한, 문서종류 수정할때 발신문서 변경 안되는 버그 수정]
	DP_APP_DocProcess_Ins

[전자결재 - 직인등록 삭제시에 직인등록여부 DB정보 수정 안하는 버그 수정]
	DP_APP_EnforceStamp_UpdByFile
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\EnforceStamp.cs
	\DeskPlusEIP\Approval\Ws\Admin\EnforceSeal\Mod.asmx.cs
	\DeskPlusEIP\Approval\Js\Admin\EnforceSeal\Mod.js
	\DeskPlusEIP\Approval\Admin\EnforceSeal\Mod.aspx

[전자결재 - 직인등록 다국어 새창크기 수정]
	\DeskPlusEIP\Approval\Js\Admin\EnforceSeal\Add.js

[전자결재 - 결재정보확인에서 참조 부서이하표시 안 나오는 버그 수정]	
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CarbonCopyUser.cs
	\DeskPlusEIP\Approval\Status\Common\GetDocFlow.aspx
	\DeskPlusEIP\Approval\Js\Status\Common\GetDocFlow.js

[전자결재 - 결재정보확인에서 상신대기(수신)일때 테두리없는 버그 수정]	
	\DeskPlusEIP\Approval\Js\Status\Common\GetDocFlow.js

[전자결재 - 조직도에서 팝업나온 이후에 양식 클릭시에 팝업 안 없어지는 버그 수정]	
    \DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

[전자결재 - ZQ 2단계이상 수신부서 담당자 변경할때 문서내에 담당자변경 수정 안되는 버그 수정]	
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

[전자결재 - 관리자페이지 리스트,문서보기에서 문서 삭제 버그수정]
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx.cs
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Ws\Admin\Document\ViewDoc.asmx.cs
	\DeskPlusEIP\Approval\Ws\Admin\Document\ViewDocProc.asmx.cs
	\DeskPlusEIP\Approval\Ws\Admin\Document\List.asmx.cs
	\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Document.cs
	\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	QRY_APP_DocTotal_SelByGroupID
	QRY_APP_DocTotal_SelSubtree
	QRY_APP_DocProcess_SelByGroupID
	ufn_GetDocTotalPathSubtree
	VW_APP_DocTot	

[전자결재 - 결재상황조회에서 문서폐기시에 버그수정]
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\Approval\Ws\Status\ListProcess.asmx.cs
	\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

[전자결재 - 마이그레이션 한 문서를 이관할때 에디터관련 버그수정]
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

[전자결재 - 마이그레이션 한 문서의 class 대소문자구분으로 스타일 적용 안되는 버그수정 (formfieldinput , formFieldInput)]
  \DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

[커뮤니티 - 회원등급명 관리 글씨 깨지는 버그 수정]
	\DeskPlusEIP\Community\Js\Site\Admin\General\Common\GetGrade.js

[커뮤니티 - 이슈게시판 날짜 넓이수정/모달창 높이수정]
	\DeskPlusEIP\Community\Jt\Site\Main\JTIssueList.htm
	\DeskPlusEIP\Community\Js\Site\Admin\General\Grade.js
	\DeskPlusEIP\Community\Js\Site\Admin\General\Point.js


[게시판 - 임시저장수정페이지, 수정페이지 글자색값 가지고 오지 못하는 버그 수정
		  임시저장갤러리페이지에서 제목글자색 안 나오는 버그수정
		  임시저장갤러리페이지에서 공지,일반 이상하게 나오는 버그수정
		  임시저장블로그형페이지에서 N표시 제목 옆으로 수정
		  공지사항폰트 적용 안되는 버그 수정
		  블로그형 N 표시 겹치는 버그 수정]

	\DeskPlusEIP\Board\Content\Mod.aspx.cs
	\DeskPlusEIP\Board\Temp\Mod.aspx
	\DeskPlusEIP\Board\Temp\Mod.aspx.cs
	\DeskPlusEIP\Board\Temp\ModN.aspx

	\DeskPlusEIP\Board\Jt\Content\JTBlogList.htm
	\DeskPlusEIP\Board\Jt\Temp\JTBlogList.htm
	\DeskPlusEIP\Board\Jt\Temp\JTGalleryList.htm

	\DeskPlusEIP\Board\Ws\Admin\Content\List.asmx.cs
	\DeskPlusEIP\Board\Ws\Admin\Statistic\ReadCount.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\BlogList.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\BlogListImpo.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\GalleryList.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\GalleryListImpo.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\List.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\ListImpo.asmx.cs
	\DeskPlusEIP\Board\Ws\GetBoard.asmx.cs

	\DeskPlusEIP\Portlet\Ws\Package\Board\Album.asmx.cs
	\DeskPlusEIP\Portlet\Ws\Package\Board\Blog.asmx.cs
	\DeskPlusEIP\Portlet\Ws\Package\Board\List.asmx.cs

[게시판 - 중요표시 블로그형  N 표시 겹치는 버그수정
		  중요표시 갤러리형 체크박스 안 나오는 버그수정
		  중요표시 갤러리형 제목 안 나오는 버그수정
		  블로그형 중요표시 안 나오는 버그수정]
	\DeskPlusEIP\Board\Jt\Content\JTBlogListImpo.htm	
	\DeskPlusEIP\Board\Jt\Content\JTGalleryListImpo.htm
	\DeskPlusEIP\Board\Jt\Content\JTGalleryList.htm

[게시판 - 관리자에서 상위폴더변경에서 같은 폴더로 이동 버그수정]
	\DeskPlusEIP\Board\Admin\Category\Move.aspx
	\DeskPlusEIP\Board\Js\Admin\Category\Move.js
	리소스추가

: 2014-01-09 By shimpark ----------------------------------------------------------
[게시판 임시저장글 수정시 제목부분 width값 추가]
	DeskPlusEIP\Board\Temp\ModN.aspx (86)

:2014-01-09 by shimpark ----------------------------------------------------------
[인사정보 - 확장]
입력값에 & 나 " 사용시 오류 대체
.replace(/&/g, '＆').replace(/"/g, '＂');

\DeskPlusEIP\Account\Js\Admin\User\Extend\CareerInfo.js
\DeskPlusEIP\Account\Js\Admin\User\Extend\EduInfo.js
\DeskPlusEIP\Account\Js\Admin\User\Extend\FamilyInfo.js
\DeskPlusEIP\Account\Js\Admin\User\Extend\RewardInfo.js
\DeskPlusEIP\Account\Js\Admin\User\Extend\ScholarInfo.js

\DeskPlusEIP\Account\Js\User\Extend\CareerInfo.js
\DeskPlusEIP\Account\Js\User\Extend\EduInfo.js
\DeskPlusEIP\Account\Js\User\Extend\FamilyInfo.js
\DeskPlusEIP\Account\Js\User\Extend\RewardInfo.js
\DeskPlusEIP\Account\Js\User\Extend\ScholarInfo.js

\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserScholar.cs
\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserSNS.cs
\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserReward.cs
\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEducation.cs
\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserFamily.cs
\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserCareer.cs



: 2014-01-09 By hooper2 ----------------------------------------------------------
[게시판 임시저장글 수정시 제목부분 width값 추가]
	DeskPlusEIP\Board\Temp\Mod.aspx (86)

: 2014-01-08 By 방영환 ----------------------------------------------------------
[사용자 정보 경력사항 입력시 & 기호 들어가면 에러]
	\DeskPlusEIP\Account\Js\MyInfo\Extend\CareerInfo.js

:2014-01-08 by famo23 ----------------------------------------------------------
[모바일관리 - 리스트헤더값 변경등]
	\DeskPlusEIP\MDM\Auth\ListAdd.aspx
	\DeskPlusEIP\MDM\Auth\ListAdd.aspx.cs
	\DeskPlusEIP\MDM\Auth\ListReject.aspx
	\DeskPlusEIP\MDM\Auth\ListReject.aspx.cs	
	\DeskPlusEIP\MDM\Js\Auth\ListAdd.js
	\DeskPlusEIP\MDM\Js\Auth\ListReject.js
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js
	\DeskPlusEIP\MDM\Jt\Auth\JtListAdd.htm
	\DeskPlusEIP\MDM\Jt\Auth\JtListReject.htm
	\DeskPlusEIP\MDM\Ws\Auth\ListAdd.asmx.cs
	\DeskPlusEIP\MDM\Ws\Auth\ListReject.asmx.cs

:2014-01-03 by freeman ----------------------------------------------------------
[다운로드] EDM 파일도 다운이 될수 있게 수정 
	\DeskPlusEIP\Common\DownLoad\DownLoad.aspx (29)

:2014-01-02 by famo23 ----------------------------------------------------------
[모바일관리 - 알람설정 수정 - 모바일도 수정]
	DP_MDM_List_Upd
	DP_MDM_List_UpdByListID
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx
	\DeskPlusEIP\MDM\Device\DeviceInfo.aspx.cs
	\DeskPlusEIP\MDM\Js\Device\DeviceInfo.js
	\DeskPlusEIP\MDM\Ws\Device\DeviceInfo.asmx.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.BE\ListInfo.cs
	\Projects\MDMProject\Zeospace.DeskPlus.MDM.DAL\List.cs
	리소스추가

:2013-12-31 by shimpark ----------------------------------------------------------
[zip - 중국어 안되는 현상]
	- 중국어만 난수로 zip 하도록 하였음.(방안 c# 4.5 전환)
	D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Common\Zip\CreateZipFile.aspx.cs

:2013-12-13 / 26 by shimpark, freeman ----------------------------------------------------------
[태그프리 수정 - IE 11 대응]
* PutHTML() 함수 대응
입력모드 -> htmlvalue
수정모드 -> bodyvalue

D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Mod.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Reply.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Admin\Content\Reply.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Admin\Template\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Admin\Template\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Content\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Content\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Content\Mod.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Content\Reply.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Content\Reply.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Temp\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Board\Temp\Mod.aspx.cs
                                                                                                   
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Approval\Admin\FormGeneration\Add.aspx
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Basic\EditorActiveX.js
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Approval\Js\FormFlow\Basic\EditorActiveX.js
                                                                                                   
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Community\Site\Admin\Function\Main\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Community\Site\Admin\Function\Main\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Community\Site\Apps\Board\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Community\Site\Apps\Board\Mod.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Community\Site\Apps\Board\Reply.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Community\Site\Apps\Schedule\Add.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Community\Site\Apps\Schedule\Mod.aspx.cs
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Community\Site\Main\Mail\Default.aspx.cs

D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Admin\Category\Add.aspx.cs(117):		
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Admin\Category\Mod.aspx.cs(122):         
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Forward\ViewPopup.aspx.cs(223):  
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Receive\AddAccept.aspx.cs(148):  
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Receive\AddComplete.aspx.cs(144):
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Receive\Mod.aspx.cs(132):        
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Receive\View.aspx.cs(279):       
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Receive\ViewComplete.aspx.cs(229)
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Reference\Mod.aspx.cs(132):      
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Reference\View.aspx.cs(227):     
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Request\Add.aspx.cs(132):		
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Request\AddRecall.aspx.cs(147):  
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Request\Mod.aspx.cs(151):        
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Request\ModReply.aspx.cs(131):   
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Request\View.aspx.cs(256):       
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Coop\Content\Total\ViewPopup.aspx.cs(266):    

D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\ETC\Branch\Edit.aspx.cs(137):			
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\ETC\Branch\Register.aspx.cs(130):			
                                                                                           
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Opinion\Admin\Content\QuesAdd.aspx.cs(72):		
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Opinion\Admin\Content\QuesAdd.aspx.cs(73):		
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Opinion\Admin\Content\QuesMod.aspx.cs(147):	
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Opinion\Admin\Content\QuesMod.aspx.cs(152):   
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Opinion\Admin\Content\QuesMod.aspx.cs(153):   
         
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\Add.aspx.cs(84):     
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\Mod.aspx.cs(541):    
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\Admin\Holiday\List.aspx
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\Anniversary\Add.aspx.cs(82):       
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\Anniversary\Mod.aspx.cs(444):      
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\Schedule\Add.aspx.cs(114):         
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\Schedule\Mod.aspx.cs(1097):        
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\ToDo\Add.aspx
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\Pim\ToDo\Add.aspx.cs(485):  

//2013-12-31 수정
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\PMS\Board\Mod.aspx.cs(117)
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\PMS\Template\Mod.aspx.cs(99)
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\PMS\Unit\Mod.aspx.cs(169)           
                                                                                    
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\RM\Content\Add.aspx.cs(119):           
D:\Site\Chest40\DeskPlusSolution\DeskPlusEIP\RM\Content\Mod.aspx.cs(194):



:by freeman ----------------------------------------------------------
[IE11 대응 - IE11에서 전체 다운로드 화면 깨지는 문제
	\DeskPlusEIP\Common\DownLoad\TotalDownLoad.aspx(41)
	\DeskPlusEIP\Html\Js\IEActiveX\activexDPUploadTotalDownLoad.js(1)

:by freeman ----------------------------------------------------------
[IE11 대응 - IE11에서 createPopup() 이 제거 되어 contextMenu를 NonactiveX용으로 대체함
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js(517)




:2013-12-24 by freeman ----------------------------------------------------------
[검색엔진] 전문검색 Prototype 버전
\DeskPlusSolution\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\AppSettingHelper.cs (602)
\DeskPlusSolution\DeskPlusEIP\App_Data\Config.xml (28)

\DeskPlusSolution\DeskPlusEIP\Search\*.*
\DeskPlusSolution\Projects\Search\*.*

Design
\Design\C\Css\Func_Search.css
\Design\C\Img\Icon\icon_file.gif
\Design\C\Img\Icon\ajax-loader.gif

SQL
 * GMU
   - SEARCH Category

 * Table
   - DP_SEARCH_BoardUser 
   - DP_SEARCH_IndexLog
   - DP_SEARCH_IndexLogDetail 

 * VIEW
   - VW_Search_APP_GetAttachList
   - VW_Search_APP_GetDocumentIDByUserID
   - VW_Search_APP_GetDocumentList
   - VW_Search_BOA_GetAttachList
   - VW_Search_BOA_GetBoardList
   - VW_Search_BOA_GetCategoryIDByUserID

 * SP
   - DP_SEARCH_BoardUser_Schedular
   - DP_SEARCH_IndexLog_Ins
   - DP_SEARCH_IndexLog_SelByLogID
   - DP_SEARCH_IndexLog_Upd
   - DP_SEARCH_IndexLogDetail_Ins

 * 스케줄
   - EXEC  DP_SEARCH_BoardUser_Schedular 


:2013-12-19 by hooper2 ----------------------------------------------------------
[관리자 시스템코드관리 수정오류 수정]
	DP_COM_SYSCode_Upd
	\DeskPlusEIP\EIP\Admin\Code\AddCategory.aspx.cs(101,152,217)

:2013-12-18 by hooper2 ----------------------------------------------------------
[통합검색 검색출력 오류부분 수정]
	\DeskPlusEIP\EIP\Js\Search\List.js (50)

[통합검색 제목, 파일명 검색오류 DB 프로시져수정]
	DP_EIP_SEARCH_SelPageCnt
	DP_EIP_SEARCH_SelPageList

[통합검색 상세바로가기버튼 안보이게 처리]
	\DeskPlusEIP\EIP\Search\List.aspx (181, 192)

:2013-12-18 by maniaysw ----------------------------------------------------------
[제타메일 - 사용자 활성화 관련 API호출]
	
	\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs (492)
	\DeskPlusEIP\Account\Ws\Admin\User\List.asmx.cs (214~252)
	\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs (1751)

	\CommonProject\Zeospace.DeskPlus.Common.BLL\MailZetta65.cs (253~296)
		
	\DeskPlusEIP\App_Data\MAIL.xml (29,30)

	GMU: QRY_ACC_User_SelUserID 추가

:2013-12-18 by famo23 ----------------------------------------------------------
[전자결재 - 자주사용하는 양식 초기로딩때 페이징 버그 수정]
	\DeskPlusEIP\Approval\Preference\FavoriteForm\List.aspx.cs

[전자결재 - 자주사용하는 양식 추가, 삭제시에 페이징 버그 수정]
	\DeskPlusEIP\Approval\Js\Preference\FavoriteForm\List.js

[전자결재 - 자주사용하는 양식 추가시에 formid 뒷숫자가 같으면 추가 안되는 버그 수정(예:formid 8, 18일때)]
	\DeskPlusEIP\Approval\Js\Preference\FavoriteForm\GetForm.js
	\DeskPlusEIP\Approval\Ws\Preference\FavoriteForm\List.asmx.cs

[전자결재 - 자주사용하는양식, 자주지정하는 결재자 순서변경 리소스 버그 수정]
	\DeskPlusEIP\Approval\Preference\Favorite\ModSequence.aspx
	\DeskPlusEIP\Approval\Preference\FavoriteForm\ModSequence.aspx

:2013-12-17 by freeman ----------------------------------------------------------
[웹메일 - 영문 환경에서 제타메일 한글환경으로 않 나오는 버그 수정]
C:\Solutions\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Portlet\Package\ZettaFrame.asp
 - language 파라미터 추가
 - 관리자 메뉴 환경에서도 language 파라미터 추가해야함
 
C:\Solutions\DeskPlus40\DeskPlusSolution\DeskPlusEIP\Portlet\Package\ZettaFrame.asp

:2013-12-17 by famo23 ----------------------------------------------------------
[전자결재 - 문서함 폴더 이동 리소스 버그 수정]
	\DeskPlusEIP\Approval\Folder\Approve\Move.aspx
	\DeskPlusEIP\Approval\Folder\Arbitrary\Move.aspx
	\DeskPlusEIP\Approval\Folder\Private\Move.aspx

[전자결재 - 근거문서 디자인 수정]
	\DeskPlusEIP\Approval\Jt\FormFlow\Basic\JTGetBaseDocList.htm

:2013-12-16 by famo23 ----------------------------------------------------------
[전자결재 - 양식관리에서 임시저장건수 안 나오는 버그 수정]
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Draft.cs
	\DeskPlusEIP\Approval\Admin\FormGeneration\ModSetting.aspx
	\DeskPlusEIP\Approval\Admin\FormGeneration\ModSetting.aspx.cs
	QRY_APP_Draft_SelFormID

[전자결재 - 검색 기간선택시 입력박스 "-" 기호 위치버그수정]
    Func_Approval.css

[전자결재 - 상신후 호출 로직 버그 수정]
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js

[전자결재 - 이관문서인지 아닌 지 체크하기 위한 GroupID 값 가져오기]
    \DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx

[전자결재 - 이관문서일때 입력버튼 활성화 버그수정]
    \DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js

[전자결재 - 라디오 버튼 비활성화 처리 버그수정]
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Admin\Transfer\View.js
	\DeskPlusEIP\Approval\Js\Admin\Transfer\ViewCir.js
	\DeskPlusEIP\Approval\Js\Common\Popup\View.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewCir.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Js\Disuse\View.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewHistory.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js

[전자결재 - 마이그레이션 한 문서를 이관할때 에디터관련 버그수정]
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

[전자결재 - 후열문서에서 첨부파일 이미지 보기 버그 수정]
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js

[전자결재 - 후열문서에서 결재정보 패치파일 뜨는 버그 수정]
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js

[전자결재 - 후열문서리스트 후열자 안 나오는 버그 수정]	
	\DeskPlusEIP\Approval\Jt\Status\JTListAfterward.htm
	\DeskPlusEIP\Approval\Status\ListAfterward.aspx
	\DeskPlusEIP\Approval\Ws\Status\ListAfterward.asmx.cs
	DP_APP_DocProcess_SelPageCntAfterward
	DP_APP_DocProcess_SelPageListAfterward

[전자결재 - 수신,이관문서 상신할때 문서종류, 열람권한 발신부서의 정보 가져오지 못하는 버그수정]
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

[전자결재 - 타부서문서열람권한 페이징 안나오는 버그 수정]
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\List.js
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\ReceiptList.js
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\SendList.js

[전자결재 - 이관문서 본문/결재선수정, 반려문서 재상신할때 페이지 이동 버그 수정]
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js
	\DeskPlusEIP\Approval\FormFlow\Default.aspx
	\DeskPlusEIP\Approval\FormFlow\Default.aspx.cs

[전자결재 - 회람현황 카운트 버그 수정]
    \DeskPlusEIP\Approval\Js\Folder\Carboncopy\CirReadList.js

[전자결재 - 문서함 헤더 안 나오는 버그 수정]
	\DeskPlusEIP\Approval\Folder\Approve\List.aspx
	\DeskPlusEIP\Approval\Folder\Approve\ListFolder.aspx
	\DeskPlusEIP\Approval\Folder\Arbitrary\List.aspx
	\DeskPlusEIP\Approval\Folder\Arbitrary\ListFolder.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirReceiptList.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\CirSendList.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\ForwardList.aspx
	\DeskPlusEIP\Approval\Folder\Carboncopy\List.aspx
	\DeskPlusEIP\Approval\Folder\Private\List.aspx
	\DeskPlusEIP\Approval\Folder\Private\ListFolder.aspx

[전자결재 - 정렬안되는 버그수정]
	DP_APP_Doc_SelPageListForm

[인사 - 최상위부서값 가져오기 버그 수정]
    \Projects\AccountProject\Zeospace.DeskPlus.Account.BLL\Dept.cs

[자원관리 - 등록자 Sort 디자인 변경 안되는 버그수정]
	\DeskPlusEIP\RM\Js\Admin\Content\List.js
	\DeskPlusEIP\RM\Jt\Admin\Content\JTList.htm



: 2013-11-30 By withsuki ----------------------------------------------------------
[버그수정]고정수신망info누락 버그수정
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

:2013-11-28 by seolove777 ----------------------------------------------------------
[버그 - 사용자 삭제시 메일 Pop 까지 다 삭제 되도록 호출 API 변경]

	\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs
	\DeskPlusEIP\Account\Ws\Admin\User\List.asmx.cs

:2013-11-28 by freeman ----------------------------------------------------------
[보완 - 전자결재 ContentTEXT 에 input type value 값이 들어가지 않아서 본문 검색이 않되는 문제 보완
  - form의 input value 값을 가져오는 GetInputVal(obj) 함수 추가 

  - 이전
    $("#htaFormTEXT").val($("HTML", ifrmObj).text());

  - 변경
    $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj)); // input type value값들을 추가함

\DeskPlusEIP\Html\Js\jQuery\jquery.common.js(964)  
\DeskPlusEIP\Approval\Js\Folder\Common\View.js(595)
\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js(310)
\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js(426)
\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js(348)
\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js(290)
\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js(95)
\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js(387)
\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js(485)
\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js(543)
\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js(289)
\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js(247)
\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js(284)
\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js(283)
\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js(523)
\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js(449)
\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js(524)
\DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js(469)
\DeskPlusEIP\Approval\Js\Status\Common\View.js(451)


:2013-11-26 by freeman ----------------------------------------------------------
[버그 - 게시판 정렬 문제]
\DeskPlusEIP\Board\Jt\Admin\Content\JTList.htm
\DeskPlusEIP\Board\Jt\Content\JTList.htm
\DeskPlusEIP\Board\Jt\Content\JTListImpo.htm
\DeskPlusEIP\Board\Jt\Temp\JTList.htm

:2013-11-22 by withsuki ----------------------------------------------------------
[버그 - 수신의 경우, 2~3단계 이상의 수신담당자가 문서수정 시, 문서가 삭제되는 버그 수정]

	DP_APP_SeqEtc_DelByGroupID


:2013-11-21 by freeman ----------------------------------------------------------
[IE11 대응 - 전자결재 - 서명관리 수정 - IE11에서 drag 및 offsetX 문제 ]

	\DeskPlusEIP\Approval\Common\Popup\GetSign.aspx (43)
	\DeskPlusEIP\Approval\Js\Common\Popup\GetSign.js (211) 


:2013-11-21 by freeman ----------------------------------------------------------
[전자결재 - 리소스 추가 - 아이콘 title 설명 다국어 누락건]

	\DeskPlusEIP\Approval\Status\Common\GetDocFlow.aspx (72 ~ 81)



:2013-11-20 by withsuki ----------------------------------------------------------
[버그 - 고정수신망 -> 수신결재라인 진행 중, 최초발신자에게 반려 -> 수정후 재상신 시 버그.]

	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js

	DP_APP_Seq_SelByLineNonApp
	DP_APP_Seq_SelByLine


:2013-11-20 by shimpark ----------------------------------------------------------
[ie 11 다운로드 수정]
\common\download\download.aspx

:2013-11-18 by famo23 ----------------------------------------------------------
[일정관리 - 디자인깨지는 현상 수정]
\DeskPlusEIP\Pim\Admin\Holiday\List.aspx

:2013-11-13 by withsuki ----------------------------------------------------------
[결재처리로직 변경] Trans문제로 처리로직 변경
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs

: 2013-11-13 By shimpark ----------------------------------------------------------
[공용주소록 수정 - 추가 이후 이동 문제]
\DeskPlusEIP\Address\Admin\Content\Add.aspx
\DeskPlusEIP\Address\Admin\Content\Add.aspx.cs
\DeskPlusEIP\Address\Js\Admin\Content\Add.js


: 2013-11-12 By kkzz52 ----------------------------------------------------------
[반려 알람대상자 SP 버그 수정]
DP_APP_Seq_SelAlarmReject

:2013-11-06 by withsuki ----------------------------------------------------------
버그) 결재라인관련하여, DP_COM_DeptCode 의 DeptCodeID에 잘못된 값이 들어가는 경우 (0 or NULL)

	DP_COM_DeptCode_SelGetIDByDeptID
	DP_APP_Seq_SelByLine
	DP_APP_Seq_SelByLineNonApp


버그) 유동결재망이 하단에 있는 경우, 팝업처리가 하단으로 떨어짐
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js


:2013-11-05 by famo23  ----------------------------------------------------------
[전자결재 - 결재대상문서에 보류문서 안 나오게 수정]
   DP_APP_DocProcess_SelPageListApprove
   DP_APP_DocProcess_SelPageCntApprove
   DPM_APP_DocProcess_SelApprove

:2013-10-31 by freeman ----------------------------------------------------------
[공통 - 파라미터 암호화 진행시 파라미터 값에 특수문자(=) 이후 값이 사라지는 문제 수정


	\DeskPlusEIP\Site\Auth\ModPassword.aspx.cs(205):     //특수문자 이후 사라지는 문제 ByFreeman 20131031
	\DeskPlusEIP\Common\Ws\GetCommon.asmx.cs(88):        //특수문자 이후 사라지는 문제 ByFreeman 20131031
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBaseControl.cs(275):        //특수문자 이후 사라지는 문제 ByFreeman 20131031
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentBasePage.cs(287):			  //특수문자 이후 사라지는 문제 ByFreeman 20131031
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Auth\UserEnvironmentMasterPage.cs(276):		//특수문자 이후 사라지는 문제 ByFreeman 20131031
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\Base\BaseService.cs(102):			//특수문자 이후 사라지는 문제 ByFreeman 20131031
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\UtilHelper.cs(326):				//특수문자 이후 사라지는 문제 ByFreeman 20131031
	\Projects\ComponentProject\Zeospace.DeskPlus.Component.Handler\UploadFileAttachHandler.cs(331):				//특수문자 이후 사라지는 문제 ByFreeman 20131031   
	\DeskPlusEIP\Approval\Common\Popup\View.aspx.cs(244):			//특수문자 이후 사라지는 문제 ByFreeman 20131031      
	\DeskPlusEIP\RM\Content\ViewQRCode.aspx.cs(197):				//특수문자 이후 사라지는 문제 ByFreeman 20131031  




:2013-10-30 by freeman ----------------------------------------------------------
[공통 - 파일 업로드시 파일명에 특수문자(') 제한]
	\DeskPlusEIP\Upload\DPUpload\Main.aspx   32, 59 line  //파일명 특수문자 제한 ByFreeman 20131030
	\DeskPlusEIP\Upload\DPUpload\Upload.js   193 line  //파일명 특수문자 제한 ByFreeman 20131030

	리소스명 (UploadBlockFileName)
	\DeskPlusEIP\Resource\GlobalString\string.en.resx
	\DeskPlusEIP\Resource\GlobalString\string.ja.resx
	\DeskPlusEIP\Resource\GlobalString\string.ko.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusEIP\Resource\GlobalString\string.zh-tw.resx


    *** ActiveX DPUPload 에 우선 적용함 NonActiveX 는 추후 검토

:2013-10-30 by famo23 ----------------------------------------------------------
[전자결재 - 전결문서함 처리 버그수정 - ZQ(빨간수신)가 있는 문서에서 발신전결이 생겼을 때 전결문서함에 안 나오는 부분]
\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
DP_APP_ArbitraryDoc_InsSeq
DPM_APP_DocProcess_ApproveA
DP_APP_DocProcess_Del

: 2013-10-30 by withsuki ---------------------------------------------------------------
버그수정) 수신리스트에서 수신담당자 변경 시, 내용부분 변경안됨
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\Js\Common\Popup\GetDeptUser.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CounterPartUser.cs
	DP_APP_CounterPartUser_UpdReplace

버그수정) 결재문서 관리에서 연동폴더가 나오지 않음
	\DeskPlusEIP\Approval\Admin\Document\JList.aspx
	\DeskPlusEIP\Approval\Common\FormFlow\FormTreeScript.ascx

버그수정) 근거문서 제목에서 ' 제거
	\DeskPlusEIP\Approval\Ws\FormFlow\Basic\GetBaseDoc.asmx

: 2013-10-28 by withsuki ---------------------------------------------------------------

관리자 전자결재 양식관리에서 양식 수정 후 해당 페이지로 다시 이동하는게 아니라 무조건 첫페이지로 이동하는 현상 발생.
	\DeskPlusSolution\DeskPlusEIP\Approval\Js\Admin\FormGeneration\List.js

임직원 조회 시, 겸직자들은 표시 안됨.
	DP_ACC_User_SelSrhPageCnt
	DP_ACC_User_SelSrhPageList
	\DeskPlusSolution\DeskPlusEIP\Account\Ws\Search\List.asmx

: 2013-10-28 by shimpark ---------------------------------------------------------------
DP_COM_DeptCode_UpdModule
- 특정 인사의 부서 변경시 DeptCodeID 수정 테이블 변경 함수

: **** 2013-10-23 by freeman ================================================================ 진행중 <- 주석처리함
- 전자결재 문서 작성시 클릭한 순서로 결재정보 확인이 되도록 수정 (기존에 협조/합의가 밑으로 가던 사항 수정) 
     \DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx				  80 line  // 선택한 순서로 결재진행 표시 ByFreeman 20131023
	 \DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx				  66 line  // 선택한 순서로 결재진행 표시 ByFreeman 20131023
	 \DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx		  66 line  // 선택한 순서로 결재진행 표시 ByFreeman 20131023

	 \DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx		  80 line  // 선택한 순서로 결재진행 표시 ByFreeman 20131023
	 \DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx		  64 line  // 선택한 순서로 결재진행 표시 ByFreeman 20131023
	 
	 \DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx			64 line  // 선택한 순서로 결재진행 표시 ByFreeman 20131023

	 \DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx			67 line  // 선택한 순서로 결재진행 표시 ByFreeman 20131023

	 \DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js			 388, 421, 1106 line  // 선택한 순서로 결재진행 표시 ByFreeman 20131023

  ====================================================================================== 진행중

: 2013-10-22 By 방영환 ----------------------------------------------------------

[자주사용하는 양식 지정시 두번째 노드부터는 UseCheckBox 설정 적용안됨]
	\DeskPlusEIP\Approval\Common\FormFlow\GetFormTreeScript.aspx
	\DeskPlusEIP\Approval\Common\FormFlow\GetFormTreeScript.cs

[수신받은문서 상신시 제목에 특수문자가 제대로 표시되지 않음]
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.cs

: 2013-10-22 by freeman ---------------------------------------------------------------
 - IE11 에서는 User Agent 문자열에서 MSIE 문자열이 사라져서 IE 확인이 제대로 않되는 현상
	\DeskPlusEIP\Html\Js\jQuery\jquery.js - jQuery Browser Plugin 추가
	


: 2013-10-18 by 방영환 ---------------------------------------------------------------
 - 메인 메모장, 축하합니다 포틀릿 링크 제거
	\DeskPlusEIP\Portlet\Js\Package\BirthDay.js
	\DeskPlusEIP\Portlet\Js\Package\MyMemo.js
	\DeskPlusEIP\Portlet\Package\BirthDay.ascx
	\DeskPlusEIP\Portlet\Package\MyMemo.ascx

: 2013-10-15 by freeman ---------------------------------------------------------------

 - Convert 관련 기본 Component 추가  

   Zeospace.DeskPlus.Component.Converter.*.* 

   * HtmlToImage : HTML 을 이미지로 변경
   * HtmlToPDF : HTML 을 PDF 로 변경 (중간에 이미지로 변환후 PDF 로 변경 / CSS 파일 파싱에 문제가 있어서 이미지 사용함)

   사용법 

   * /DeskPlusEIP/Util/PDF/HtmlToPdf.aspx 참고


: 2013-10-08 by seo ---------------------------------------------------------------

 관리자 - 통계이력관리 - 다운로드통계 
 - 다운로드 된 이력이 없는 경우 에러 발생 예외 처리 

   \Statistics\Content\Attach\List.aspx.cs  


: 2013-09-24 By withsuki ----------------------------------------------------------

: 2013-09-24 By withsuki ----------------------------------------------------------
===================================================================================
- 인사연동 : 사용자 추가/수정/삭제/인증

- 스킨수정

- 주소록 띄우기 : 	DEEP->GW / GW->DEEP

- 메일발송

- 포틀릿구성

- 자동완성sp
	- 사용자 목록에서 검색시 : SQL_DEEP_Mail_SelectAddressNameMail
	- 그룹 목록에서 검색시 : SQL_DEEP_Mail_Address_DeptInfo

----------------------------------------------------------------------------------
수정페이지>

메일 스킨정보>

	- 필드추가 (DP_ACC_UserEnv -> MailDesign)
	- 뷰 [VW_UserInfo] 수정
	\Projects\AccountProject\Zeospace.DeskPlus.Account.BE\UserInfo.cs
	\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs


IF페이지>
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\App_Data\Config.xml
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\App_Data\MAIL.xml
	D:\Source\DeskPlusEIP40\DeskPlusSolution\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\Mail.cs
	D:\Source\DeskPlusEIP40\DeskPlusSolution\Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\MailDeep.cs


인증>
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Site\Auth\Default.aspx.cs
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Site\Auth\LegacyLogin.aspx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Site\Auth\LoginOutSide.aspx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Account\MyInfo\SLO.aspx.cs
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Admin\Company\SLO.aspx.cs


추가>
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Account\Admin\User\Add.aspx.cs


수정>
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Account\Ws\Admin\User\Password\Change.asmx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Account\Ws\MyInfo\Password\Change.asmx


스킨>
	QRY_ACC_User_SelGetSkinMail 추가

	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\App_Data\Config.xml
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Portal\GetSkinMail.aspx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Js\Portal\GetSkinMail.js
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Ws\Portal\GetSkinMail.asmx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.BLL\User.cs
	D:\Source\DeskPlusEIP40\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Resource\GlobalString\string.en.resx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Resource\GlobalString\string.ja.resx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Resource\GlobalString\string.ko.resx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Resource\GlobalString\string.zh-cn.resx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\EIP\Resource\GlobalString\string.zh-tw.resx


사용자관리 버그수정>
	DP_ACC_User 필드수정 (MailPassword)
	DP_ACC_UserMailDomain 필드수정 (MailPassword)

	DP_ACC_User_Ins
	DP_ACC_User_Upd
	DP_ACC_User_UpdPwd
	DP_ACC_UserMailDomain_Ins
	DP_ACC_UserMailDomain_Upd

	D:\Source\DeskPlusEIP40\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs
	D:\Source\DeskPlusEIP40\DeskPlusSolution\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserMailDomain.cs


SLO처리>
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Common\Mail\Deep\DeepSLO.aspx


메일발송-주소록>
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Common\Mail\Deep\DeepWrite.aspx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Address\Content\List.aspx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Address\Content\ListImpo.aspx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Address\Content\ListBlog.aspx
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Address\Content\ListBlogImpo.aspx

DEEP-주소록>
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Address\Js\Mail\AddressSearch.js
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Address\Mail\AddressSearch.aspx



: 2013-09-24 By withsuki ----------------------------------------------------------
[일정관리 - 전체공유일정에서 퇴사자는 안나오게 처리.]
	DP_PIM_AllUSERSchedule

: 2013-09-23 By withsuki ----------------------------------------------------------
[전자결재 - tag관련 속성 대소문자 수정.]
	
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\Add.js
	\DeskPlusEIP\Approval\Js\Admin\Transfer\View.js
	\DeskPlusEIP\Approval\Js\Admin\Transfer\ViewCir.js
	\DeskPlusEIP\Approval\Js\Common\Popup\View.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewCir.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewHistory.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js"(1734,48)
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js

: 2013-09-23 By shimpark ----------------------------------------------------------
[로그인 watershow]
	\DeskPlusEIP\Resource\GlobalString\
	\DeskPlusEIP\Site\Auth\Default.aspx
	\DeskPlusEIP\Site\Auth\Default.aspx.cs

: 2013-09-16 By shimpark ----------------------------------------------------------
[jQuery UI 1.8 - > 1.10.3 버전 업그래이드]
	\DeskPlusEIP\Html\Js\jQuery\jquery.ui.all.js (1.10.3)
	\DeskPlusEIP\Html\Js\jQuery\jquery.ui.all_1.8.js (추가 : accordian 용도)
	Accordian 을 사용하는 화면 aspx 는 전부 jquery.ui.all_1.8.js 으로 사용한다. (화면 자동 resize 조절 가능)

: 2013-09-05 By kkzz52 ----------------------------------------------------------
[임시저장 본문 이미지 오류 수정]
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx 

: 2013-09-03 By shimpark ----------------------------------------------------------
[게시판 관리자 폴더 이동]
	\DeskPlusEIP\Board\Admin\Category\Move.aspx
	\DeskPlusEIP\Board\Common\BoardTreeScript.ascx.cs 
	\DeskPlusEIP\Board\Common\GetBoardTreeScript.aspx.cs 

: 2013-09-03 By shimpark ----------------------------------------------------------
[jQuery 1.7 - > 1.8.3 버전 업그래이드]
	\DeskPlusEIP\Html\Js\jQuery\jquery.js
	\DeskPlusEIP\Html\Js\JModal\DeskPlusModal.js

	 jQuery("#zeoModalContact").css({
        "position": "absolute",
        "top": '50%',
        "left": '50%',
        "overflow": "hidden",
        "margin-top": controlHeight,
        "margin-left": controlWidth
    });


: 2013-08-28 By withsuki ----------------------------------------------------------
[커스텀관련 수정 - 양식내의 dropdown정보가 안맞는 경우.]
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js

: 2013-08-07 By withsuki ----------------------------------------------------------
[문서작성(상신문서함) : 부서변경된 경우, 예외처리]
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js

:2013-08-07 By famo23 ----------------------------------------------------------
 월별 조회 버그수정
 DP_RM_Reserve_SelMonthList

:2013-07-26 By famo23 ----------------------------------------------------------
[휴가원] 달력 공휴일 표시 수정
\DeskPlusEIP\Common\Calendar\Calendar.ascx.cs

: 2013-07-25 By freeman ----------------------------------------------------------
 - JQuery 업로드 기능 추가 
   \DeskPlusEIP\Upload\JSUpload\*.*
   \DeskPlusEIP\Html\Js\jQuery\jquery.common.js
 
 - 첨부파일 호출 페이지 변경 (모듈별로 silverlight 업로드 호출하던 부분을 공통 jquery.common.js 에서 변경 가능하게 처리
 //    var url;
//    if ($.browser.msie) {
//        url = "/DeskPlusEIP/Upload/DPUpload/Main.aspx";
//    } else {
//        url = "/DeskPlusEIP/Upload/SilverUpload/Main.aspx";
//    }
    var url = GetAttachPathURL();  //jquery.common.js 에 있음

 - 첨부파일 페이지 호출 js 65개 수정


: 2013-07-24 By withsuki ----------------------------------------------------------
- 수신부서 결재자가, 상신부서 작성자에게 반려했을 경우, 에러처리.
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js


: 2013-07-19 By famo23 ----------------------------------------------------------
- colspan 갯수 수정, 내용 없음 안나오는 버그 수정
\DeskPlusEIP\Pim\Jt\Admin\ShareUser\JTList.htm

: 2013-07-18 By shimpark ----------------------------------------------------------
- 수신문서 재상신시 스크립트 오류
\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js 

원인 및 조치 : 결재망에 ZQ 태그 호출됨, DOM 없음, 왜 들어오는지 모르겠음. 단지 IF  문으로 차단만 했음.

// 고정태그의 경우는 이미 존재. 다중태그의 경우만 생성.
if ($("#" + newID, ifrmObj).get() == "") {
    var itemHTML = "<span class='" + className + "' id='" + newID + "' idx='" + cnt + "'></span>";
    nowEl = $("#" + className, ifrmObj)[0];

	//########################## 변경한 코드
    if (nowEl != undefined) {
        nowEl.insertAdjacentHTML("beforeEnd", itemHTML); // <span>aaa<span newid>bbb></span></span>
    }

}


: 2013-07-12 By withsuki ----------------------------------------------------------
- 커스텀연동파일 이벤트 추가.
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js


: 2013-07-12 By kkzz52 ----------------------------------------------------------
[알툴바 태그삭제처리]
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
		\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js (2군데)
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
		\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
		\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
		\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
		\DeskPlusEIP\Approval\Js\Folder\Common\View.js
		\DeskPlusEIP\Approval\Js\Status\Common\View.js		
		\DeskPlusEIP\HTML\Js\JQuery\jquery.common.js


: 2013-07-08 By withsuki ----------------------------------------------------------
[모바일승인 시, 결재톡 미반영 버그수정.]
	DPM_APP_Seq_UpdDecide 수정

[결재의견 관련 글자수count 버그수정.]
	DeskPlusEIP\Approval\Process\Basic\ConfirmPassword.aspx
	DeskPlusEIP\Approval\Process\Basic\ConfirmReject.aspx
	DeskPlusEIP\Approval\Js\Process\Basic\ConfirmPassword.js
	DeskPlusEIP\Approval\Js\Process\Basic\ConfirmReject.js
	DeskPlusEIP\Approval\Js\Process\Opinion\ConfirmOpinion.js

: 2013-07-04 By Freeman ----------------------------------------------------------
사용하지 않는 포탈 테이블 삭제 및 관련 SP 수정
* DP_EIP_Skin
-DP_COM_Company_Del


: 2013-07-04 By Freeman ----------------------------------------------------------
사용하지 않는 커뮤니티 테이블 삭제 및 관련 SP 수정
* DP_CMN_ComAdmin
-DP_COM_Company_Del

* DP_CMN_ComAdminMenu
-DP_COM_Company_Del

* DP_CMN_Skin
-DP_COM_Company_Del


: 2013-07-03 By kkzz52 ----------------------------------------------------------
[결재자 지정 시 이벤트 변경]
		\DeskPlusEIP\Approval\Common\FormFlow\DeptUserScript.ascx		
		\DeskPlusEIP\Approval\Js\FormFlow\Menu\Common.js


: 2013-07-03 By withsuki ----------------------------------------------------------
[script보완처리]
		\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
		\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
		\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
		\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx
		\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
		\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
		\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
		\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
		\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
		\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
		\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
		\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
		\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
		\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
		\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
		\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx


: 2013-07-03 By withsuki ----------------------------------------------------------
[기능추가 - 수신문서 본문내용 수정기능]

	* 소스)
		\DeskPlusEIP\Approval\FormFlow\Default.aspx
		\DeskPlusEIP\Approval\FormFlow\Menu\TreeModReceive.aspx
		\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
		\DeskPlusEIP\Approval\FormFlow\Receive\ModContent.aspx
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
		\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
		\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js
		\DeskPlusEIP\Approval\Js\FormFlow\Menu\TreeModReceive.js
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
		\DeskPlusEIP\Approval\Js\FormFlow\Receive\ModContent.js
		\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
		\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
		\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
		\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
		\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
		\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
		\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CounterPartUser.cs
		\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs

	* DB)
		DP_APP_SeqEtc_DelByGroupID 추가
		DP_APP_CounterPartUser_Ins 변경

[버그수정 - 본문내용 수정 시, 근거문서 누락 건]
	* 소스)
		\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx


: 2013-07-01 By withsuki ----------------------------------------------------------
[에디터 & 제목 처리]
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\SaveDraft.js
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CirculationDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Enforce.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Draft.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Form.cs


: 2013-07-01 By Freeman ----------------------------------------------------------
사용하지 않는 WORK(업무일지) 테이블 삭제 및 관련 SP 수정
* DP_WORK_Attach
-DP_COM_Company_Del

* DP_WORK_Auth
-DP_COM_Company_Del

* DP_WORK_List
-DP_COM_Company_Del

* DP_WORK_ShortReply
-DP_COM_Company_Del


: 2013-07-01 By Freeman ----------------------------------------------------------
사용하지 않는 일정관리 테이블 삭제 및 관련 SP 수정

* DP_PIM_ToDoShareUser 삭제
-DP_COM_Company_Del

* DP_PIM_Type 삭제
-DP_COM_Company_Del

=======================================
* DP_PIM_OpenList 삭제
-DP_COM_Company_Del

=======================================
* DP_PIM_DeptOpenList 삭제
-DP_COM_Company_Del
-DP_PIM_TOTALSEARCH
-DP_PIM_TOTALSEARCH_CNT
-DP_PIM_USERSchedule
-DP_PIM_USERSchedule_Cnt

=======================================
* DP_PIM_OpenUserGroup 삭제
-DPM_PIM_ToDo_SelByType2
-DP_PIM_Schedule_ListDay_Share
-DP_PIM_Schedule_ListWeek_Share
-DPM_PIM_ToDo_SelMonthByType2
-DP_PIM_TotalSearchExcel
-DP_PIM_TotalSearchExcel
-DP_PIM_AllUSERSchedule
-DP_PIM_AllUSERSchedule
-DP_PIM_OPENUSER_ADMIN
-DP_PIM_Schedule_ListMonth_Share
-DP_COM_Company_Del
-DP_PIM_AllUSERSchedule_BackUp
-DP_PIM_AllUSERSchedule_BackUp
-DP_PIM_TotalListSearch
-DP_PIM_TotalListSearch
-DP_PIM_TotalListSearchCnt
-DP_PIM_TotalListSearchCnt
-DP_PIM_TOTALSEARCH_0
-DP_PIM_TOTALSEARCH_CNT_0
-DP_REPORT_PIM_SelMySche
-DP_PIM_TOTALSEARCH_CURRENT_CNT
-DP_PIM_TODO_SEL_SHARE
-DPM_PIM_Schedule_SelByType2
-DP_PIM_TODO_SEL_SHARE_CNT
-DP_PIM_OpenUserGroup_Ins
-DP_PIM_TOTALSEARCH_CURRENT
-DPM_PIM_Schedule_SelMonthByType2


: 2013-06-28 By Freeman ----------------------------------------------------------
사용하지 않는 테이블 삭제 및 관련 SP 수정

* DP_ACC_UserCode
-DP_COM_Company_Del

* DP_ADD_Attach
-DP_COM_Company_Del


: 2013-06-28 By withsuki ----------------------------------------------------------
[HTML스크립트 처리 - .ToEditorXSS]
	\Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Helper\UtilHelper.cs

: 2013-06-28 By withsuki ----------------------------------------------------------
[문서내용백업 - 문서생성/완결]
	테이블생성)
	- BAK_APP_DocProcess
	SP추가)
	- BAK_APP_DocProcess_Ins 
	- BAK_APP_DocProcess_Del
	SP변경)
	- DP_APP_DocProcess_Ins
	- DP_APP_DocProcess_UpdConclude
	- DPM_APP_DocProcess_UpdConclude

: 2013-06-27 By withsuki ----------------------------------------------------------
[수신결재망 대소문자처리]
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs

: 2013-06-25 By freeman  ----------------------------------------------------------
- 전자결재 자주지정하는 결재자 지정시 사용하는 ContextMenuCtrl 메뉴 멀티브라우져 위치 문제 적용

  \DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js 수정
  \DeskPlusEIP\Html\Js\DeskPlusEIPComponent.js 수정


: 2013-06-25 By freeman  ----------------------------------------------------------
- 메뉴 회사 CI 클릭 했을때 메인화면 로드시 2번 호출되는 상황 수정
  \DeskPlusEIP\EIP\Package\Menu.aspx 수정
  
  * HOME 메뉴의 호출 id를 "home" 으로 지정



: 2013-06-21 By freeman  ----------------------------------------------------------
- 추가/수정 로딩 페이지 디자인 수정
   \DeskPlusEIP\Common\Notify\CallNotify.aspx

  * 리소스 추가
  	\DeskPlusEIP\Common\Resource\GlobalString\string
    - CallNotifyMessage

  * 이미지 추가
	\DeskPlusEIP\Html\Js\JQDialog\c_loader.gif

  * CSS 수정
	\DeskPlusEIP\Html\Js\JQDialog\jqdialog.css





: 2013-06-19 By freeman  ----------------------------------------------------------
- 썸네일 적용
	\DeskPlusEIP\Board\Ws\Content\BlogList.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\BlogListImpo.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\GalleryList.asmx.cs
	\DeskPlusEIP\Board\Ws\Content\GalleryListImpo.asmx.cs
	\DeskPlusEIP\Board\Ws\Temp\BlogList.asmx.cs
	\DeskPlusEIP\Board\Ws\Temp\GalleryList.asmx.cs


	\DeskPlusEIP\Community\Ws\Site\Apps\Board\ListGallery.asmx.cs


	\DeskPlusEIP\Pim\Ws\ShortReply\ShortReplyList.asmx.cs
	\DeskPlusEIP\Pim\Ws\ShortReply\ShortReplyGroup.asmx.cs
	\DeskPlusEIP\Pim\Ws\Schedule\ShortReplyList.asmx.cs


	\DeskPlusEIP\Portlet\Ws\Package\Board\Blog.asmx.cs
	\DeskPlusEIP\Portlet\Ws\Package\Board\Album.asmx.cs

	\DeskPlusEIP\WebHard\Ws\Content\ListPreview.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListImpoPreview.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Content\ListIcon.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Trash\ListPreview.asmx.cs
	\DeskPlusEIP\WebHard\Ws\Trash\ListIcon.asmx.cs


: 2013-06-14 By freeman  ----------------------------------------------------------
- 이미지 표시할때 썸네일 방식 표시 기능 추가 (포틀릿 등에서 이미지 표현시 용량커서 느려지는 문제)
	- \DeskPlusEIP\Common\DownLoad\ImageView.aspx.cs  
	  * 파라미터 추가
		thumnail : Y 일 경우 썸네일 처리
		thumWidth : width 값 강제로 지정 (지정하지 않았을 경우 150)
		thumHeight : height 값 강제로 지정 (지정하지 않았을 경우 자동 계산)

	  * 사용 법
        Dictionary<string, string> paraDic = new Dictionary<string, string>();
        paraDic.Add("filePath", info.ScreenShot);
        paraDic.Add("thumnail", "Y");
		paraDic.Add("thumWidth", "200");  
		paraDic.Add("thumHeight", "250");
        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
        info.ScreenShot = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

	- 포틀릿 변경
	  * \DeskPlusEIP\Portlet\Ws\Package\Board\Albumasmx.cs  
	  * \DeskPlusEIP\Portlet\Ws\Package\Board\Blog.asmx.cs
	  * \DeskPlusEIP\Portlet\Ws\Package\MyComment.asmx.cs


    * 원본이미지를 보여줄 필요 없는곳에서 적용 필요


: 2013-06-14 By withsuki ----------------------------------------------------------
- 메신저 호출 팝업 닫기버튼 수정.
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Board\Js\Content\View.js
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Coop\Js\Content\Total\ViewPopup.js


: 2013-06-14 By withsuki ----------------------------------------------------------
- 결재알람 수정. 결재대상으로 알람을 받은 경우, 결재페이지 이동.

1. SP
	- DP_APP_Seq_SelAlarmNext
2. 소스
	- \DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx
	- \DeskPlusEIP\Approval\Common\Popup\ViewCir.aspx
	- \DeskPlusEIP\Approval\Common\Popup\View.aspx
	- \DeskPlusEIP\Approval\Js\Process\Basic\ApprovePopup.js
	- \DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx
	- \DeskPlusEIP\Approval\Process\Basic\ApprovePopup.aspx.cs
	- \DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx
	- \DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx
	- \DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx
	- \DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx
	- \DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx
	- \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\UserInfo.cs
	- \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	- \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs
	- \DeskPlusEIP\App_Data\APPROVAL.xml

: 2013-06-11 By shimpark ------------------------------------------------------------
프로젝트관리
- 일정추가 부분

D:\SiteProjects\DeskPlus40\DeskPlusEIP\PMS\Schedule\Add.aspx
D:\SiteProjects\DeskPlus40\DeskPlusEIP\PMS\Schedule\AddN.aspx
D:\SiteProjects\DeskPlus40\DeskPlusEIP\PMS\Js\Schedule\Add.js


: 2013-06-11 By kkzz52 ------------------------------------------------------------
게시기능적용
- 메뉴위치 : 전자결재 완결문서 보기

1. SP
   - DP_APP_Doc_InsBoard 추가

2. 소스변경
   \DeskPlusEIP\Approval\Folder\Common\View.aspx
   \DeskPlusEIP\Approval\Folder\Common\View.cs
   \DeskPlusEIP\Approval\Js\Folder\Common\View.js
   \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
   \Projects\BaseProject\Zeospace.DeskPlus.Base.ApplicationFrameworks\Mail\SendMail.cs
   \Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\Mail.cs

3. 소스추가
   \DeskPlusEIP\Approval\Ws\Folder\Dept\SendBoard.asmx
   \DeskPlusEIP\Approval\Ws\Folder\Dept\SendBoard.asmx.cs

: 2013-06-10 By SHIMPARK ----------------------------------------------------------
- 게시판 임시저장 페이지 수정(결재와 공통 사용하기 위함)
\DeskPlusEIP\Board\Js\Temp\Move.js
\DeskPlusEIP\Board\Js\Temp\View.js
\DeskPlusEIP\Board\Temp\View.aspx

: 2013-06-10 By withsuki ----------------------------------------------------------
- 수신페이지 (결재라인지정) 이중클릭 체크
\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js


: 2013-06-05 By Freeman ----------------------------------------------------------
- 게시판 댓글 관련 SP 수정
dbo.DP_BOA_ShortReply_SelPageList


: 2013-05-28 By withsuki ----------------------------------------------------------
- IE10에서 mime값 디코딩 시, 깨진문자 replace처리
\DeskPlusEIP\Approval\Admin\FormGeneration\Add.aspx.cs
\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs
\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx.cs
\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs
\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs


: 2013-05-28 By shimpark ----------------------------------------------------------
[인사확장]
- 인사 확장 정보
: AESManager.Encrypt 암호화 하여 db 넣는 과정에 문제 이슈

\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserCareer.cs
\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserEducation.cs
\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserFamily.cs
\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserReward.cs
\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\UserScholar.cs


: 2013-05-23 By shimpark ----------------------------------------------------------
[포틀릿]
- 나의 협업 포틀릿

* 저장프로시저
dbo.DP_PT_MyCoop_SelCCList
dbo.DP_PT_MyCoop_SelCCTotCnt
dbo.DP_PT_MyCoop_SelForwardList
dbo.DP_PT_MyCoop_SelForwardTotCnt
dbo.DP_PT_MyCoop_SelReqList
dbo.DP_PT_MyCoop_SelReqTotCnt
dbo.DP_PT_MyCoop_SelTOList
dbo.DP_PT_MyCoop_SelTOTotCnt

* 소스
\DeskPlusEIP\Portlet\Package\TabCoopMain.ascx
\DeskPlusEIP\Portlet\Ws\Package\Coop\List.asmx
\Projects\PortletProject\Zeospace.DeskPlus.Portlet.DAL\MyCoop.cs


: 2013-05-22 By shimpark ----------------------------------------------------------
[포틀릿]
- 결재탭 메인 포틀릿

/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabAppMainApp.htm  
/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabAppMainCarbon.htm
/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabAppMainCir.htm
/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabAppMainForward.htm
/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabAppMainReport.htm

/DeskPlusEIP/Portlet/Package/Approval/TabAppMain.ascx


: 2013-05-21 By shimpark ----------------------------------------------------------
[EIP]
- 메인화면 포틀릿 추가의 리스트 화면
	리스트 출력시, 비활성화 포틀릿은 감추기

- 저장프로시저 : DP_EIP_Portlet_SelTabPageList, DP_EIP_Portlet_SelTabPageCnt

\DeskPlusEIP\EIP\Ws\Portal\GetPortlet.asmx.cs
\Projects\EIPProject\Zeospace.DeskPlus.EIP.DAL\Portlet.cs

[EIP]
- 포틀릿 카테고리 보이기 부분 화면
	
\DeskPlusEIP\EIP\Admin\Portal\GetPortlet.aspx
\DeskPlusEIP\EIP\Admin\Portlet\CopyPortlet\Tree.aspx
\DeskPlusEIP\EIP\Admin\Portlet\Move.aspx
\DeskPlusEIP\EIP\Admin\Portlet\Tree.aspx
\DeskPlusEIP\EIP\Portal\AddPortlet.aspx
\DeskPlusEIP\EIP\Portal\AddPortlet.aspx
\DeskPlusEIP\EIP\Portal\AddPortletUser.aspx
\DeskPlusEIP\EIP\Portal\GetPortlet.aspx
\DeskPlusEIP\EIP\Portal\GetPortlet.aspx
\DeskPlusEIP\EIP\Admin\Portlet\AddPortlet.aspx


\DeskPlusEIP\EIP\Common\PortletCategoryTree.ascx.cs
\DeskPlusEIP\EIP\Common\PortletTreeScript.ascx.cs



: 2013-04-24 By shimpark ----------------------------------------------------------
[업무일지]
- 폴더의 카운트
	개인함 (총건수), 부서함(미확인건수/총건수), 확인요청함(미확인건수/총건수), 공유함(미확인건수/총건수)
- 리스트 : 안읽을 경우 일자 굵게

추가 개발로 인해 SP 가 전역적으로 반영이 되어 일일이 기술이 어려우므로
업무일지 4.0 을 소스 전체 패치하시기 바랍니다. 
소스 : REPORT, BE, BLL, DAL 
DB : dbo.DP_REPORT_ListViewer 추가
SP : 전부 변경
gmu : 변경없음


: 2013-04-18 By MANIAYSW ----------------------------------------------------------
[메신저 로그인 인증 SLO Base64 디코딩 처리]

\CommonProject\Zeospace.DeskPlus.Common.DAL\UCMessenger.cs
\DeskPlusEIP\Common\Messenger\UC\SLO.aspx.cs



: 2013-04-18 By SHIMPARK ----------------------------------------------------------
[ie 10- 일정관리 전체공유리스트 테이블 디자인 깨진 부분] 

경로 : /Design/C/Css/Core_Table.css
1. css 변경
- 원본
table.table_typeA {table-layout:fixed;float:left;width:100%;border-left:1px solid #c0c0c0;border-right:1px solid #c0c0c0;border-collapse:collapse;}
- 변경
table.table_typeA {table-layout:fixed;float:left;width:100%;border-left:1px solid #c0c0c0;border-right:1px solid #c0c0c0;border-collapse:separate;border-spacing:0px;}



: 2013-04-17 By MANIAYSW ----------------------------------------------------------
[사용자 패스워드 특수문자 허용]

\DeskPlusEIP\Account\Ws\Admin\User\Password\Change.asmx.cs
\DeskPlusEIP\Account\Ws\MyInfo\Password\Change.asmx.cs

\DeskPlusEIP\Account\Js\MyInfo\Password\Change.js
\DeskPlusEIP\Account\Js\MyInfo\Password\MailChange.js

\DeskPlusEIP\Account\Js\Admin\User\Password\MailChange.js
\DeskPlusEIP\Account\Js\Admin\User\Password\Change.js

\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs

[메일 도메인 소문자 처리]

\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs
\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs



: 2013-04-17 By SHIMPARK ----------------------------------------------------------
[게시판 관리자 화면 - 부서별게시판 전체보여주기] 

1. 소스
	\DeskPlusEIP\Board\Admin\Content\List.aspx.cs

: 2013-04-17 By SHIMPARK ----------------------------------------------------------
[SITEMAP 개발] 

1. 소스
	\DeskPlusEIP\Portlet\Package\SiteMap.aspx
	\DeskPlusEIP\Portlet\Package\SiteMap.aspx.cs

2. 디자인
	\DeskPlusEIP\Design\C\Img\icon\sitemap.png

: 2013-04-17 By SHIMPARK ----------------------------------------------------------
[포탈 메뉴 버그 FIX] 
	- 홈메뉴의 서브메뉴 보이지 않음 (처음 화면 로딩 시)

1. 소스
	\DeskPlusEIP\EIP\Package\Menu.aspx.cs (line 178)


: 2013-04-08 By withsuki ----------------------------------------------------------
[전자결재 통계기능개발] 
	- 관리자화면 -> 통계-월별사용현황
	- 관리자화면 -> 통계-조건별사용현황

1. 소스
	\DeskPlusEIP\Approval\Admin\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Admin\Statistics\
	\DeskPlusEIP\Approval\Js\Admin\Statistics\
	\DeskPlusEIP\Approval\Jt\Admin\Statistics\
	\DeskPlusEIP\Approval\Ws\Admin\Statistics\

2. view & sp
	VW_APP_DocStatistics
	DP_APP_Statistics_SelMonthDept
	DP_APP_Statistics_SelMonthDeptCnt
	DP_APP_Statistics_SelMonthDeptSrhName
	DP_APP_Statistics_SelMonthForm
	DP_APP_Statistics_SelMonthFormCnt
	DP_APP_Statistics_SelMonthFormSrhName
	DP_APP_Statistics_SelMonthUser
	DP_APP_Statistics_SelMonthUserCnt
	DP_APP_Statistics_SelMonthUserSrhName
	DP_APP_Statistics_SelTopDept
	DP_APP_Statistics_SelTopDeptCnt
	DP_APP_Statistics_SelTopForm
	DP_APP_Statistics_SelTopFormCnt
	DP_APP_Statistics_SelTopTot
	DP_APP_Statistics_SelTopUser
	DP_APP_Statistics_SelTopUserCnt


: 2013-04-10 By shimpark ----------------------------------------------------------
[게시판 권한 수정]
: 임시저장글의 게시판 복사 시, 쓰기권한 폴더 문제
: 게시판 관리자 페이지, 리스트 읽기폴더의 쓰기 버튼 감추는 작업

\DeskPlusEIP\Board\Common\BoardTreeScript.ascx
\DeskPlusEIP\Board\Common\BoardTreeScript.ascx.cs
\DeskPlusEIP\Board\Common\GetBoardTreeScript.aspx
\DeskPlusEIP\Board\Common\GetBoardTreeScript.aspx.cs
\DeskPlusEIP\Board\Admin\Content\List.aspx.cs


: 2013-04-03 By shimpark ----------------------------------------------------------
[패스워드 변경]

변경
\DeskPlusEIP\Account\MyInfo\Password\Change.aspx
\DeskPlusEIP\Account\MyInfo\Password\Change.aspx.cs

\DeskPlusEIP\Account\MyInfo\Mod.aspx
\DeskPlusEIP\Account\MyInfo\Mod.aspx.cs

\DeskPlusEIP\Account\Admin\User\Mod.aspx
\DeskPlusEIP\Account\Admin\User\Mod.aspx.cs

\DeskPlusEIP\Account\Js\MyInfo\Mod.js
\DeskPlusEIP\Account\Js\MyInfo\Password\Change.js
\DeskPlusEIP\Account\Js\Admin\User\Mod.js
\DeskPlusEIP\Account\Js\Admin\User\Password\Change.js

\DeskPlusEIP\Account\Ws\MyInfo\Password\Change.asmx.cs

추가
\DeskPlusEIP\Account\MyInfo\Password\MailChange.aspx
\DeskPlusEIP\Account\MyInfo\Password\MailChange.aspx.cs

\DeskPlusEIP\Account\Admin\User\Password\MailChange.aspx
\DeskPlusEIP\Account\Admin\User\Password\MailChange.aspx.cs

\DeskPlusEIP\Account\Js\MyInfo\Password\MailChange.js
\DeskPlusEIP\Account\Js\Admin\User\Password\MailChange.js

리소스 추가 : Account

SP 변경
DP_ACC_User_UpdPwd

: 2013-04-02 By shimpark ----------------------------------------------------------
[구글연동 어플]

DP_PIM_GOOGLE_SYNC 수정
DP_PIM_Schedule_Ins 수정
DP_PIM_GoogleCalendar 테이블의 scheduleid 필드 추가


: 2013-03-29 By Freeman ----------------------------------------------------------
[버그]일정관리 시간 계산 잘못 처리 부분 수정
 - parseInt 진수 처리
 \DeskPlusEIP\Pim\Js\Admin\Holiday\Add.js
 \DeskPlusEIP\Pim\Js\Schedule\Add.js
 \DeskPlusEIP\Pim\Js\Schedule\AddSimple.js
 \DeskPlusEIP\Pim\Js\Schedule\ListDay.js
 \DeskPlusEIP\Pim\Js\Schedule\ListMonth.js
 \DeskPlusEIP\Pim\Js\Schedule\ListWeek.js
 \DeskPlusEIP\Pim\Js\Schedule\Mod.js
 \DeskPlusEIP\Pim\Js\Schedule\PopPimAdd.js
 \DeskPlusEIP\Pim\Js\ToDo\Add.js



: 2013-03-27 By shimpark ----------------------------------------------------------
[라이선스 정책]

\DeskPlusEIP\EIP\Admin\Company\Add.aspx
\DeskPlusEIP\EIP\Admin\Company\Add.aspx.cs
\DeskPlusEIP\EIP\Admin\Company\List.aspx
\DeskPlusEIP\EIP\Admin\Company\List.aspx.cs
\DeskPlusEIP\EIP\Ws\Admin\Company\List.asmx.cs
\DeskPlusEIP\EIP\Admin\Subsidiary\Add.aspx
\DeskPlusEIP\EIP\Admin\Subsidiary\Add.aspx.cs
\DeskPlusEIP\Account\Admin\User\List.aspx
\DeskPlusEIP\Account\Admin\User\List.aspx.cs
\DeskPlusEIP\Account\Admin\User\ListBlog.aspx
\DeskPlusEIP\Account\Admin\User\ListBlog.aspx.cs

\DeskPlusEIP\EIP\Jt\Admin\Company\JTAdd.htm
\DeskPlusEIP\EIP\Jt\Admin\Company\JTList.htm

\DeskPlusEIP\EIP\Js\Admin\Company\Add.js
\DeskPlusEIP\EIP\Js\Admin\Company\List.js
\DeskPlusEIP\EIP\Js\Admin\Subsidiary\Add.js


\DeskPlusEIP\Account\Js\Admin\User\List.js
\DeskPlusEIP\Account\Js\Admin\User\ListBlog.js
\DeskPlusEIP\Account\Ws\Admin\User\List.asmx.cs
\Projects\AccountProject\Zeospace.DeskPlus.Account.DAL\User.cs (gmu : DP_COM_Company_SelCompanyID)
리소스

\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\CompanyInfo.cs
\Projects\CommonProject\Zeospace.DeskPlus.Common.DAL\Company.cs

** SP 수정 : 
dbo.DP_COM_Company_Ins
dbo.DP_COM_Company_Upd

** TABLE  변경
dbo.DP_COM_Company 테이블의 LicenseCnt (INT) 필드 추가



: 2013-03-21 By shimpark ----------------------------------------------------------
[관리자 조직도 감추기]deskplus 계정은 조직도에서 보이지 않음

\DeskPlusEIP\Account\Common\GetDeptUserScript.aspx.cs
\DeskPlusEIP\Account\Common\UserDeptTreeScript.ascx

\DeskPlusEIP\Address\Common\AddressTree.ascx.cs
\DeskPlusEIP\Address\Common\GetAddressTree.aspx

\DeskPlusEIP\Approval\Common\FormFlow\DeptUserScript.ascx.cs
\DeskPlusEIP\Approval\Common\FormFlow\GetDeptUserScript.aspx


: 20130320 ByFreeman ----------------------------------------------------------
[멀티법인] 멀티법인별 로고/이미지 처리
	\DeskPlusEIP\Site\Auth\Defaul.aspx
	\DeskPlusEIP\Site\Auth\Defaul.aspx.cs

	\DeskPlusEIP\Js\Site\Auth\Default.js

	\DeskPlusEIP\Ws\Site\Auth\Default.asmx.cs


: 20130315 ByFreeman ----------------------------------------------------------
[멀티법인] 법인별 로그인/메뉴 이미지 변경 기능
	\DeskPlusEIP\Site\Auth\Default.aspx
	\DeskPlusEIP\Site\Auth\Default.aspx.cs

	\DeskPlusEIP\EIP\Package\Menu.aspx
	\DeskPlusEIP\EIP\Package\Menu.aspx.cs

	\DeskPlusEIP\EIP\Admin\Subsidiary\Add.aspx
	\DeskPlusEIP\EIP\Admin\Subsidiary\Add.aspx.cs

	\DeskPlusEIP\EIP\Js\Admin\Subsidiary\Add.js

	\DeskPlusEIP\EIP\Ws\Admin\Subsidiary\Add.asmx.cs

	\DeskPlusEIP\EIP\Resource\GlobalString\*.resx  (LogInImage, MenuImage)



: 20130315 ByFreeman ----------------------------------------------------------
[업로드버그] 다국어(중국어) 글자 깨지는 현상 패치 ~ 변환 오류
	\Projects\CommonProject\Zeospace.DeskPlus.Component.Handler\UploadFileAttachHandler.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Component.Handler\UploadFileSaveHandler.cs


: 20130313 ByFreeman ----------------------------------------------------------
[멀티법인] 멀티법인 추가 기능 보완
	\DeskPlusEIP\EIP\Admin\Company\Add.aspx
	\DeskPlusEIP\EIP\Admin\Company\Add.aspx.cs
	\DeskPlusEIP\EIP\Js\Admin\Company\Add.js

	\Projects\CommonProject\Zeospace.DeskPlus.Common.BE\CompanyInfo.cs
	\Projects\CommonProject\Zeospace.DeskPlus.Common.DAL\Company.cs

	- 리소스 추가 : DefaultValue
	\DeskPlusEIP\EIP\Resource\GlobalString\string.ko.resx  
	\DeskPlusEIP\EIP\Resource\GlobalString\string.zh-cn.resx                  
	\DeskPlusEIP\EIP\Resource\GlobalString\string.zh-tw.resx                  
	\DeskPlusEIP\EIP\Resource\GlobalString\string.en.resx  
	\DeskPlusEIP\EIP\Resource\GlobalString\string.fr.resx  
	\DeskPlusEIP\EIP\Resource\GlobalString\string.ja.resx  


 테이블 추가 : DP_ASP_Company_Temp
 SP 수정 : DP_COM_Company_Ins

: 20130313 ByWithsuki ----------------------------------------------------------
[포틀릿 추가] 전달/참조/회람 포틀릿추가 및 문서확인 부분 수정
	\DeskPlusEIP\Approval\Common\Popup\ViewCir.aspx
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewCir.js
	\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx.cs

	\DeskPlusEIP\Portlet\Jt\Package\Approval\
	\DeskPlusEIP\Portlet\Package\Approval
	\DeskPlusEIP\Portlet\Ws\Package\Approval\Tab.asmx.cs

: 20130312 ByWithsuki ----------------------------------------------------------
[포틀릿 버그수정] 일반 보기페이지.
	D:\Source\DeskPlusEIP40\DeskPlusSolution\DeskPlusEIP\Portlet\Package\Approval\Tab.ascx

: 20130311 ByFreeman ----------------------------------------------------------
[디자인] 일정관리 공유일정 보기에서 공유일정 댓글 숫자 CSS 변경 반복일정에 한글 표시 (다국어 지원 않되는문제)

\DeskPlusEIP\Pim\Jt\Schedule\JTAllchedule.htm

: 20130308 ByFreeman ----------------------------------------------------------
[다국어] 일정관리 반복일정에 한글 표시 (다국어 지원 않되는문제)

\DeskPlusEIP\Pim\Ws\Schedule\ScheduleRepeat.asmx.cs

\Projects\PimProject\Zeospace.DeskPlus.Pim.BLL\RepeatScheduleProc.cs


: 20130308 ByFreeman ----------------------------------------------------------
[이미지] 다국어 이미지 반영 않된 부분 적용

이미지 추가
\Design\C\Img\Lang\en\not.gif
\Design\C\Img\Lang\ja\not.gif
\Design\C\Img\Lang\ko\not.gif
\Design\C\Img\Lang\zh-cn\not.gif
\Design\C\Img\Lang\zh-tw\not.gif

소스 변경
\DeskPlusEIP\Board\Js\Temp\BlogList.js(389)
\DeskPlusEIP\Board\Ws\Admin\Content\List.asmx.cs(475)
\DeskPlusEIP\Board\Ws\Admin\Statistic\ReadCount.asmx.cs(436)
\DeskPlusEIP\Board\Ws\Content\BlogList.asmx.cs(430)
\DeskPlusEIP\Board\Ws\Content\BlogListImpo.asmx.cs(385)
\DeskPlusEIP\Board\Ws\Content\GalleryList.asmx.cs(368)
\DeskPlusEIP\Board\Ws\Content\GalleryListImpo.asmx.cs(381)
\DeskPlusEIP\Board\Ws\Content\List.asmx.cs(370)
\DeskPlusEIP\Board\Ws\Content\ListImpo.asmx.cs(385)
\DeskPlusEIP\Board\Ws\GetBoard.asmx.cs(366)
\DeskPlusEIP\Board\Ws\GetBoard.asmx.cs(1901)
\DeskPlusEIP\Portlet\Ws\Package\Board\Album.asmx.cs(269)
\DeskPlusEIP\Portlet\Ws\Package\Board\Blog.asmx.cs(271)
\DeskPlusEIP\Portlet\Ws\Package\Board\List.asmx.cs(323)


: 20130307 ByWithsuki ----------------------------------------------------------
[창환단자 수정] xscript처리 (Helper/UtilHelper.cs : ToEditorXSS 함수에서 script를 xscript로 치환함으로서, 
                             문자열로 인식됨에 따라, 코드가 string으로 그대로 보여짐.
			     모든 viewing페이지에서 해당object로 잡아서, 제거처리.)
  1. 소스
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Admin\Transfer\View.js
	\DeskPlusEIP\Approval\Js\Admin\Transfer\ViewCir.js
	\DeskPlusEIP\Approval\Js\Common\Popup\View.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewHistory.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js


: 20130304 ByWithsuki ----------------------------------------------------------
[버그수정] 결재대상문서 포틀릿 - 문서보기 링크가 관리자 페이지로 되어있음. 해당 부분 수정
	\DeskPlusEIP\Portlet\Package\Approval\ListA.ascx
	\DeskPlusEIP\Portlet\Package\Approval\ListR.ascx

: 20130227 ByWithsuki ----------------------------------------------------------
[버그수정] 타부서열람 관련 수정
	\DeskPlusEIP\Approval\Folder\OtherDept\Tree.aspx.cs
	\DeskPlusEIP\Approval\Js\Folder\OtherDept\List.js


: 20130226 ByWithsuki ----------------------------------------------------------
[기능개발] 자주사용하는양식 - 자주사용하는 양식을 관리함. (개인설정&문서작성)

1. 소스
	\DeskPlusEIP\Approval\Common\FormFlow\FormTreeScript.ascx
	\DeskPlusEIP\Approval\Common\FormFlow\GetFormTreeScript.aspx
	\DeskPlusEIP\Approval\Common\Popup\GetDept.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\Preference\FavoriteForm\
	\DeskPlusEIP\Approval\Jt\Preference\FavoriteForm\
	\DeskPlusEIP\Approval\Preference\FavoriteForm\
	\DeskPlusEIP\Approval\Preference\Menu\Tree.aspx
	\DeskPlusEIP\Approval\Resource\GlobalString
	\DeskPlusEIP\Approval\Ws\Preference\FavoriteForm\
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\FavoriteFormInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\FavoriteForm.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\FavoriteForm.cs
2. DB
	테이블 (DP_APP_FavoriteForm) & sp 추가
3. Design
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Css\Func_Approval.css
	\\192.168.1.20\DeskPlus40\Design\Dev\C\Img\Approval\doc.gif

: 20130221 ByWithsuki ----------------------------------------------------------
[창환단자 요청] 인쇄시, settime으로 인해, 정상처리되지 않음.
  1. 소스
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDocProc.js
	\DeskPlusEIP\Approval\Js\Admin\Transfer\View.js
	\DeskPlusEIP\Approval\Js\Admin\Transfer\ViewCir.js
	\DeskPlusEIP\Approval\Js\Common\Popup\View.js
	\DeskPlusEIP\Approval\Js\Common\Popup\ViewTot.js
	\DeskPlusEIP\Approval\Js\Folder\Carboncopy\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewBaseDoc.js
	\DeskPlusEIP\Approval\Js\Folder\Common\ViewHistory.js
	\DeskPlusEIP\Approval\Js\Folder\Enforce\Send.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\Process\Afterward\Approve.js
	\DeskPlusEIP\Approval\Js\Process\Basic\Approve.js
	\DeskPlusEIP\Approval\Js\Status\Common\View.js


: 20130221 ByWithsuki ----------------------------------------------------------
[창환단자 요청] 보존년한 수정 (1년,3년,5년,영구) , 1년 기본값
  1. 소스
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	리소스

: 20130217 BySeo ----------------------------------------------------------
[추가개발] 전자결재 EDM 이관 개발 
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs    
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ko.resx  
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-cn.resx                  
	\DeskPlusEIP\Approval\Resource\GlobalString\string.zh-tw.resx                  
	\DeskPlusEIP\Approval\Resource\GlobalString\string.en.resx  
	\DeskPlusEIP\Approval\Resource\GlobalString\string.fr.resx  
	\DeskPlusEIP\Approval\Resource\GlobalString\string.ja.resx  
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js            
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js            
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js              
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js              
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js       
	\DeskPlusEIP\Approval\Js\Admin\FormGeneration\ModSetting.js            
	\DeskPlusEIP\Approval\Js\Folder\Common\View.js              
	\DeskPlusEIP\Approval\Js\Admin\Document\ViewDoc.js          
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx             
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs          
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.designer.cs 
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx             
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs          
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.designer.cs 
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx        
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs     
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.designer.cs               
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.designer.cs   
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx               
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs            
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.designer.cs   
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx               
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs            
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.designer.cs   
	\DeskPlusEIP\Approval\Folder\Common\View.aspx               
	\DeskPlusEIP\Approval\Folder\Common\View.aspx.cs            
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx.designer.cs                  
	\DeskPlusEIP\Approval\Admin\FormGeneration\ModSetting.aspx.cs                                      
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx           
	\DeskPlusEIP\Approval\Admin\Document\ViewDoc.aspx.cs        
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\Transfer.cs           
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Transfer.cs           
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\SmartPlusEDM\VersionInfo.cs               
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\SmartPlusEDM\FileInfo.cs                  
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\SmartPlusEDM\DocumentInfo.cs              
	\DeskPlusEIP\Approval\Ws\EDMS\SpEDMS\SendEDMS.asmx.cs                
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\SmartPlusEDM\CoworkerInfo.cs                 
	\DeskPlusEIP\Approval\Ws\EDMS\SpEDMS\SendEDMS.asmx                                      
	\DeskPlusEIP\App_Data\APPROVAL.xml

: 20130212 ByFreeman ----------------------------------------------------------
[공통UI버그] 
	- 일정관리 주간/월간 스크롤 문제 (크롬)
	\DeskPlusEIP\Pim\Schedule\ScheduleMain.aspx
	- 주소록 전달
	\DeskPlusEIP\Address\Content\Send.aspx    

: 20130208 ByWithsuki ----------------------------------------------------------
[창환요청수정] 메신저에서 참조 알람 수신 시, 열람확인여부 체크
	D:\Source\SiteCPT\DeskPlusSolution\DeskPlusEIP\Approval\Common\Popup\ViewTot.aspx.cs


: 20130206 ByWithsuki ----------------------------------------------------------
[버그수정] 겸직변경 (4개이상의 겸직의 경우, 겸직변경 시, 화면이 다 보이지않음.)
	\DeskPlusEIP\EIP\Package\Menu.aspx


: 20130125 ByWithsuki ----------------------------------------------------------
[버그수정] 후열처리 시, 발신부서 상신자 예외처리
1. 소스
	\DeskPlusEIP\Approval\Js\Status\ListProcess.js


[버그수정] 수신부서에서 결재선본문수정 시, 제목처리 수정.
1. 소스
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\ModContent.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js

: 20130124 ByWithsuki ----------------------------------------------------------
겸직관련 버그 수정.
1. 소스
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeEdit.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeLegacy.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\TreeMod.cs
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\ModContent.js
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Common.js

	

: 20130122 ByWithsuki ----------------------------------------------------------
상신완결 프로세스 추가.
1. 소스
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BLL\DocumentProcess.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Basic\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Receive\Mod.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx.cs
	\DeskPlusEIP\Approval\FormFlow\Temporary\Add.aspx.cs
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Receive\Mod.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js


======================================================================================================================================================
: 20130118 ByWithsuki ----------------------------------------------------------
개인문서함 재상신 기능 추가.

1. web.config

2. DB작업
	DP_APP_Doc_SelPageCntPrivateSubmit 추가
	DP_APP_Doc_SelPageListPrivateSubmit 추가
	DP_APP_SeqBackup_SelByLine 추가
	DP_APP_BaseDoc_SelDocID	   수정

3. 소스변경
	\DeskPlusEIP\Approval\Common\FormFlow\PrivateTree.ascx
	\DeskPlusEIP\Approval\Common\FormFlow\PrivateTree.cs
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.aspx
	\DeskPlusEIP\Approval\FormFlow\Menu\Tree.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\Add.cs
	\DeskPlusEIP\Approval\FormFlow\Submit\List.aspx
	\DeskPlusEIP\Approval\FormFlow\Submit\List.cs
	\DeskPlusEIP\Approval\Js\FormFlow\Common\Common.js
	\DeskPlusEIP\Approval\Js\FormFlow\Menu\Tree.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Submit\List.js
	\DeskPlusEIP\Approval\Jt\FormFlow\Submit\JTList.htm
	\DeskPlusEIP\Approval\Ws\FormFlow\Common\Common.asmx.cs
	\DeskPlusEIP\Approval\Ws\FormFlow\Submit\Add.asmx
	\DeskPlusEIP\Approval\Ws\FormFlow\Submit\Add.asmx.cs
	\DeskPlusEIP\Approval\Ws\FormFlow\Submit\List.asmx
	\DeskPlusEIP\Approval\Ws\FormFlow\Submit\List.asmx.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\BaseDocumentInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\BaseDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\PrivateDocument.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\SequenceBackUp.cs
	리소스파일모두


======================================================================================================================================================
: 20130117 ByWithsuki ----------------------------------------------------------
임시저장 버그수정.

1. DP_APP_DraftSequence 테이블에 필드추가(3개)/삭제(1개) 
	
	- 기존 데이타 기본값 처리 (필드삭제하기전에 처리)
	UPDATE DP_APP_DraftSequence SET IsGroup='M' WHERE UserID > 0
	DELETE FROM DP_APP_DraftSequence WHERE UserID = 0


2. sp변경
	- DP_APP_DraftSequence_Ins
	- DP_APP_DraftSequence_Sel

3. 소스변경
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.aspx
	\DeskPlusEIP\Approval\FormFlow\Basic\Add.cs
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.aspx
	\DeskPlusEIP\Approval\FormFlow\Common\SaveDraft.cs
	\DeskPlusEIP\Approval\Js\FormFlow\Basic\Add.js
	\DeskPlusEIP\Approval\Js\FormFlow\Temporary\Add.js
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.BE\DraftSequenceInfo.cs
	\Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DraftSequence.cs

======================================================================================================================================================

--------------------------------------------------------------------------------
: 20130111 ByFreeman ----------------------------------------------------------
1. 검색기능에서 특수문자 검색이 되도록 수정
   - jquery.common.js 수정
    String.prototype.securityText = function () {
    var re = /['"\/\\;:\-\<>+()#&]/g;   // 특수문자가 검색이 될수 있도록 요청하면 해당 부분을 확인
    //var re = /=&/g;
    return jQuery.trim(this.replace(re, ""));

   - @SearchValue 를 사용하는 모든 SP 대략 180개 수정함
	--ByFreeman 20130111 특수문자 검색 가능하도록 아래 코드 추가 수정 
	SET @SearchValue = Replace(@SearchValue, '&', '&amp;')
	SET @SearchValue = Replace(@SearchValue, '"', '&quot;')
	SET @SearchValue = Replace(@SearchValue, '#', '&#35;')
	SET @SearchValue = Replace(@SearchValue, '''', '&#39;')	
	SET @SearchValue = Replace(@SearchValue, '(', '&#40;')
	SET @SearchValue = Replace(@SearchValue, ')', '&#41;')
	SET @SearchValue = Replace(@SearchValue, '<', '&lt;')
	SET @SearchValue = Replace(@SearchValue, '>', '&gt;')	


: 20130110 ByWithsuki ----------------------------------------------------------

1. 문서이관기능적용 (관리자)
   - 메뉴위치 : 전자결재관리자 -> 문서이관
   - /DeskplusEIP/Approval/Admin/Transfer/
     /DeskplusEIP/Approval/Js/Admin/Transfer/
     /DeskplusEIP/Approval/Jt/Admin/Transfer/
     /DeskplusEIP/Approval/Ws/Admin/Transfer/     
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CarbonCopyUser.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\PublicDocument.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\PrivateDocument.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CirculationUser.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ApprovalDocument.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ArbitraryDocument.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\ForwardDocument.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\CirculationDocument.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Document.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\DocumentProcess.cs
     \Projects\ApprovalProject\Zeospace.DeskPlus.Approval.DAL\Sequence.cs
     Dal소스 및 관련 SP
     리소스모두

: 20130109 ByFreeman ----------------------------------------------------------
* 수정/변경 사항
 1. 라이센스 정책 적용
    - /DeskPlusEIP/Account/Admin/User/List.aspx
	- /DeskPlusEIP/Account/Admin/User/List.aspx.cs
	- /DeskPlusEIP/Account/Js/Admin/User/List.js
	- 리소스 추가 : License, TotalUsers, License_Error

 2. 공휴일 지정 방식 변경
   변경 : 공휴일 앞/뒤에 몇일식 추가로 쉴수 있도록 설정
        "공휴일 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날"

     ex) 01-01|1:2|1:2:3|,04-08,08-15|1:|1:
        - 1월 1일 앞에 연속 2일 쉬고 뒤로 연속 3일쉼
        - 4월 8일 쉼
        - 8월 15일 앞에 1일 쉬고 뒤로 1일 쉼

     - Applicationframework/helper/CalendarHelper.cs 추가
	 - 공통
       DeskPlusEIP/App_Data/Config.xml

     - 일정관리
       Pim/Js/Schedule/ListDay.js
       Pim/Js/Schedule/ListWeek.js
       Pim/Js/Schedule/ListMonth.js
       Pim/Menu/PimMenu.aspx.cs
       Pim/Ws/Schedule/Schedule.asmx.cs

    - 자원관리
       RM/Ws/Content/ListMonth.asmx.cs

    - 커뮤니티
       Community/Ws/Site/Apps/Schedule/List.asmx.cs

    - 프로젝트관리
       PMS/Schedule/List.aspx.cs
    
	- 포틀릿
       Portlet/Package/MyPim.ascx.cs
       Portlet/Smart/MyPim.ascx.cs

: 20130313 Byshine ----------------------------------------------------------
* 수정/변경 사항
1. 통계이력관리

 - 접속통계
	EIP/Admin/Static/Attach/List.aspx
	테이블 수정 : DP_COM_AccessLog (필드 추가)
	SP 수정 : DP_COM_AccessLog_Ins
			 DP_COM_AccessLog_SelPageList

 - 첨부파일 사용내역
	EIP/Admin/Static/LogIn/List.aspx
	테이블 수정 : DP_COM_DownAttach (필드 추가)
	SP 수정 : DP_COM_DownAttach_Ins
			 DP_COM_DownAttach_SelPageList

 - 로그인실패
	EIP/Admin/Static/Password/List.aspx
	테이블 수정 : DP_COM_PwdErrorLog (필드 추가)
	SP 수정 : DP_COM_PwdErrorLog_Ins
			 DP_COM_PwdErrorLog_SelPageList

: 20130314 Byshine ------------------------------------------------------------
* 수정/변경 사항
1. 주소록

 - 한국어가 아닐 경우 외부연동, 초성검색 안보이게 처리
   Address/Js/Content/List.js
-------------------------------------------------------------------------------
