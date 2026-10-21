
### DeskPlusMobile v4.5.2 (2016.12.xx) #########################################################
: 2026-08-26 by maniaysw --------------------------------------------------------
[일정관리] 알람 기간 추가 4~7일 
	\DeskPlusMobile\Pims\Add.aspx.cs
	\DeskPlusMobile\Pims\Mod.aspx.cs
	
	
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx

: 2025-04-03 by kkzz52 ----------------------------------------------------------
[전자결재] 대결 시 원결재자의 이름 및 인장으로 표시, 결재의견은 대결자의 이름으로 저장
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs

	DPM_APP_Seq_UpdDecide

: 2023-10-11 by shpark1325 --------------------------------------------------------
[전자결재]결재대상문서 정렬 오름차순으로 수정 요청
	-DB
	DPM_APP_DocProcess_SelApprove

: 2023-02-16 by famo23 --------------------------------------------------------
[전자결재] 최종결재자는 전결버튼이 안 나오게 처리
	\DeskPlusMobile\Approval\AlarmView.aspx.cs
	\DeskPlusMobile\Approval\View.aspx.cs

: 2022-12-14 by famo23 ------------------------------------------------------
[App] Android App버전변경에 따른 작업
	1) 로그인&로그아웃
	2) 앱종료
	3) 뱃지
	4) 로그인페이지 app다운로드링크변경
	5) js파일 버전변경
	6) Dpmanager 푸쉬관련 수정 
	7) dp_mdm_list mobileTyle='android' 백업및삭제처리

	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Html\Js\jquery.common.js
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\Return.js
	\DeskPlusMobile\Js\Portal\AnLogin.js
	\DeskPlusMobile\Js\Portal\Portal.js
	\DeskPlusMobile\Portal\Default.aspx
	\DeskPlusMobile\Portal\LogOut.aspx
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\SetLogOut.aspx

: 2022-10-20 by kkzz52 ------------------------------------------------------
[전자결재] 상신취소 알람 추가
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Approval\Ws\BLL\DocumentProcess.cs
	\DeskPlusMobile\Approval\Ws\Data\Sequence.cs

: 2022-10-11 by famo23 ------------------------------------------------------
[App] ios App버전변경에 따른 작업
	1) 첨부파일열기
	2) 주소록 문자,전화걸기
	3) 뱃지카운트	
	4) 로그인,로그아웃 
	5) alert, confirm 관련 수정
	6) 로그인페이지에서 iosApp다운로드 숨기기	
	7) 로그인페이지 계열사별 분기관련 수정
	8) js,css파일 버전변경
	9) Dpmanager 푸쉬관련 수정 
	10) dp_mdm_list mobileTyle='ios' 백업및삭제처리

: 2022-10-07 by kkzz52 --------------------------------------------------------
[전자결재] 결재 시 첨언이 있을 경우 첨언 메일발송
	\DeskPlusMobile\Approval\Ws\BE\DocInfo.cs
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\XmlData\Confirm\ko~.xml

: 2022-09-06 by kkzz52 --------------------------------------------------------
[전자결재] 진행중인문서를 근거문서로 첨부 가능하게 처리 (개인별 상신문서)
	\DeskPlusMobile\Js\Approval\GetBaseDoc.js
	\DeskPlusMobile\Approval\GetBaseDoc.aspx
	\DeskPlusMobile\Approval\Ws\Common.asmx.cs
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\App_Data\SQLApproval.xml (SEL_PROC_PrivateProcessCnt, SEL_PROC_DocProcessPrivate 추가)

	-리소스 추가
	Msg_ErrBaseSameDoc

	DPM_APP_DocProcess_SelPrivateCnt 추가
	DPM_APP_DocProcess_SelPrivate 추가
	DPM_APP_DocProcess_UpdConclude

: 2022-06-21 by kkzz52 --------------------------------------------------------
[공통] 공휴일이 일요일일때 대체휴무 오류 수정
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobileSolution\DeskPlusMobile\Common\CalendarDualDialog.aspx.cs
	\DeskPlusMobile\Common\Ws\GetCommon.asmx.cs
	\DeskPlusMobile\Coop\Ws\GetCoop.asmx.cs
	\DeskPlusMobile\HR\Attendance.aspx.cs
	\DeskPlusMobile\HR\Ws\GetAttendance.asmx.cs
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs
	\DeskPlusMobile\Portal\Portal.aspx.cs
	\DeskPlusMobile\RM\Ws\GetRM.asmx.cs
	\DeskPlusMobile\Task\Ws\GetTask.asmx.cs

: 2022-05-11 by famo23 ------------------------------------------------------
[전자결재] 문서상신 계열사부서정보 가져오는 버그수정
	\DeskPlusMobile\App_Data\SQLAccount.xml
	\DeskPlusMobile\Account\Data\Dept.cs	

: 2022-03-17 by kkzz52 -----------------------------------------------------------
[전자결재] 문서작성(상신문서함) : ZQ문서도 나오게 수정
	\DeskPlusMobile\Js\Approval\SubmitAdd.js
	\DeskPlusMobile\Approval\SubmitAdd.aspx
	\DeskPlusMobile\Approval\SubmitAdd.aspx.cs
	\DeskPlusMobile\Approval\Ws\BE\DocInfo.cs
	\DeskPlusMobile\Approval\Ws\Data\Document.cs

	DPM_APP_Doc_SelSubmit
	DPM_APP_Doc_SelSubmitCnt

: 2022-01-18 by kkzz52 --------------------------------------------------------
[전자결재] 결재의견 보기 추가
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\style.css
	\DeskPlusMobile\Js\Approval\AlarmView.js
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocList.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Approval\AlarmView.aspx
	\DeskPlusMobile\Approval\AlarmView.aspx.cs
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\BaseView.aspx.cs
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx.cs
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx.cs
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Approval\Jt\JTCirDocList.htm
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\App_Data\SQLApproval.xml (SEL_PROC_SelectShortReply 추가)

: 2022-01-14 by famo23 ---------------------------------------------------------
[전자결재] 문서상신후 안드로이드에서 뒤로가기할때 다시 상신되는 문제수정
	\DeskPlusMobile\Approval\Add.aspx
	\DeskPlusMobile\Approval\DraftAdd.aspx
	\DeskPlusMobile\Approval\Mod.aspx
	\DeskPlusMobile\Approval\SubmitAdd.aspx

: 2021-11-08 by kkzz52 ------------------------------------------------------------
[전자결재] EDMS 이관 시 상신자 정보를 최초기안자로 수정
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs

: 2021-08-05 by kkzz52 ---------------------------------------------------------
[전자결재] 메신저 알람 링크 오류 수정 (참조인데 수신 링크로 가는 문제)
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs

: 2021-03-16 by famo23 -----------------------------------------------
[일정관리] 일정등록할때 알람추가, 수정할때 알람삭제 후 재등록, 삭제시 알람삭제
	DPM_PIM_Schedule_Ins
	DPM_PIM_Schedule_Upd

	\DeskPlusMobile\App_Data\SQLPim.xml
	\DeskPlusMobile\Js\Pims\Add.js
	\DeskPlusMobile\Js\Pims\Mod.js
	\DeskPlusMobile\Js\Pims\View.js
	\DeskPlusMobile\Messenger\BE\AlarmInfo.cs
	\DeskPlusMobile\Messenger\BLL\Alarm.cs
	\DeskPlusMobile\Messenger\Data\Alarm.cs
	\DeskPlusMobile\Pims\Ws\BE\ScheduleInfo.cs
	\DeskPlusMobile\Pims\Ws\SetPims.asmx
	\DeskPlusMobile\Pims\Add.aspx
	\DeskPlusMobile\Pims\Add.aspx.cs
	\DeskPlusMobile\Pims\Mod.aspx
	\DeskPlusMobile\Pims\Mod.aspx.cs
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx


: 2020-06-17 by famo23 -----------------------------------------------
[알림함] pc 알림발송기
	DPM_MDM_NotificationBox_Sel  
	DPM_MDM_NotificationBox_SelCnt
	DPM_MDM_NotificationBox_SelNotReadCnt
    \DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\Js\Config\List.js
	\DeskPlusMobile\MDM\Jt\JTList.htm
	\DeskPlusMobile\MDM\WS\BE\NotificationInfo.cs
	\DeskPlusMobile\MDM\WS\GetNotification.asmx.cs
	\DeskPlusMobile\Messenger\BE\AlarmInfo.cs
	\DeskPlusMobile\Messenger\BLL\Alarm.cs
	\DeskPlusMobile\Messenger\Data\Alarm.cs
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

: 2020-06-15 by famo23 -----------------------------------------------
[전자결재] 휴가원 커스텀파일 : 휴가사유 특수문자로 일정관리, 자원관리에 안 들어가는 버그로 사유등록 삭제
	\DeskPlusMobile\Legacy\HoliDay_Complete.aspx
	DP_COM_Interface_RM_Ins
	DP_COM_Interface_ToDo_Ins	

: 2020-06-09 by famo23 -----------------------------------------------
[로그인] 알림함 보기 관련해서 보완
	DPM_MDM_List_UpdApprove

[알림함] 모바일관리 알람 설정 삭제
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\Js\Config\List.js

: 2020-03-31 by kkzz52 -----------------------------------------------------------
[전자결재] 결재취소 시 병렬결재자일 경우 ApprovalState가 잘못 변경되는 오류 수정
	\DeskPlusMobile\App_Data\SQLApproval.xml ( SEL_PROC_IsCancelZA 추가 )
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs

: 2020-03-16 by famo23 -----------------------------------------------
[Push] pwd 조건 추가 
	\DeskPlusMobileSolution\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobileSolution\DeskPlusMobile\Messenger\BLL\Alarm.cs
	\DeskPlusMobileSolution\DeskPlusMobile\Portal\LoginOutSide.aspx.cs

: 2020-01-31 by shpark1325 ----------------------------------------------------------
[자원관리] 예약시 자원리스트 가져오는 쿼리문 수정
	\DeskPlusMobile\App_Data\SQLRM.xml

: 2020-01-16 by famo23 ----------------------------------------------------------
[환경설정] 나의 일정이 아닌 공유일정보기로 했을 시에, 본인을 공유대상에 넣지 않으면 나의 공유일정을 볼수 없어서 수정
	DPM_PIM_ToDo_SelByType2
	DPM_PIM_ToDo_SelMonthByType2

: 2020-01-07 by famo23 ----------------------------------------------------------
[App] 모바일 기기 등록시 기본설정값이 기존기기정보로 가져오게 수정
	DPM_MDM_List_Ins
	DPM_MDM_List_UpdApprove

: 2019-11-26 by famo23 ----------------------------------------------------------
[로그인] 웹로그인화면도 스타일시트 변경작업
	\DeskPlusMobile\Portal\Default.aspx

[로그인] 회사명 선택 드롭다운 디자인 수정, 앱다운로드 버튼 디자인 추가 
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\ml.css

[로그인] 배경이미지제거 및 배경색상수정
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\ml.css
	\DeskPlusMobile\Js\Portal\AnLogin.js
	\DeskPlusMobile\Js\Portal\Default.js
	\DeskPlusMobile\Js\Portal\IosLogin.js
	\DeskPlusMobile\Portal\AnLogin.aspx
	\DeskPlusMobile\Portal\Default.aspx
	\DeskPlusMobile\Portal\IosLogin.aspx

[포탈] 배경이미지제거
	\DeskPlusMobile\Portal\Portal.aspx

: 2019-11-12 by maniaysw --------------------------------------------------------
[로그인] 스타일시트 변경(선진적용버전) 
	- 로그인화면의 스타일시트 사이즈 줄이기 위해서 필요한내역만 빼서 ml.css만듬
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\ml.css 추가
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\style.css

	\DeskPlusMobile\Portal\AnLogin.aspx
	\DeskPlusMobile\Portal\IosLogin.aspx
	
: 2019-10-22 by famo23 ----------------------------------------------------------
[환경설정] 알림함 옵션 추가 
	DP_MDM_UserEnv
	DPM_MDM_UserEnv_Ins
	DPM_MDM_NotificationBox_Sel
	DPM_MDM_NotificationBox_SelCnt
	DPM_MDM_NotificationBox_SelNotReadCnt
	\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobile\Config\Ws\SetConfig.asmx.cs
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\Js\Config\List.js
	\DeskPlusMobile\Js\MDM\List.js
	\DeskPlusMobile\MDM\WS\Data\Notification.cs
	\DeskPlusMobile\MDM\WS\GetNotification.asmx.cs
	\DeskPlusMobile\MDM\List.aspx.cs
	\DeskPlusMobile\Portal\Menu.ascx.cs
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

: 2019-10-02 by famo23 ----------------------------------------------------------
[전자결재] 보기페이지에서 문서사이즈 조정
	\DeskPlusMobile\Approval\AlarmView.aspx
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Js\Approval\AlarmView.js
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js

: 2019-10-01 by famo23 ----------------------------------------------------------
[공통] 모바일 알림 읽기표시
	DP_MDM_NotificationBox 필드추가
	DP_ALARM_Common 필드추가
	TRI_Alarm_Common_Insert
	SQL_DP_ALARM_Common_Insert
	DPM_MDM_NotificationBox_ReadByRandom
	\DeskPlusMobile\Messenger\BE\AlarmInfo.cs
	\DeskPlusMobile\Messenger\BLL\Alarm.cs
	\DeskPlusMobile\Messenger\Data\Alarm.cs
	\DeskPlusMobile\Portal\LoginOutSide.aspx.cs

: 2019-09-27 By famo23 ---------------------------------------------
[공통] Push 클릭시 url 경로로 이동
	 \DeskPlusMobile\Web.config
	 DPM_ALARM_Common_Sel
	 DPM_ALARM_Common_SelByIOS
	 Dpmanager 수정

: 2019-09-25 By famo23 ---------------------------------------------
[전자결재] 중간결재자한테 반려시 알림 안 가는 버그수정
	DP_APP_Seq_SelAlarmReject
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs

[전자결재] 모바일 알림 페이지 추가 
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Ws\BLL\DocumentProcess.cs
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Approval\AddOpinion.aspx
	\DeskPlusMobile\Approval\AddOpinion.aspx.cs
	\DeskPlusMobile\Approval\AlarmView.aspx
	\DeskPlusMobile\Approval\AlarmView.aspx.cs
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\Approve.aspx.cs
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Approval\Return.aspx.cs
	\DeskPlusMobile\Js\Approval\AddOpinion.js
	\DeskPlusMobile\Js\Approval\AlarmView.js
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\Return.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

: 2019-09-18 By famo23 ---------------------------------------------
[자원관리] 모바일 알림 페이지 추가 
	\DeskPlusMobile\Js\RM\AlarmView.js
	\DeskPlusMobile\Js\RM\RejectAdd.js
	\DeskPlusMobile\Js\RM\ReplyAdd.js
	\DeskPlusMobile\RM\Ws\GetRM.asmx.cs
	\DeskPlusMobile\RM\Ws\SetRM.asmx.cs
	\DeskPlusMobile\RM\AlarmView.aspx
	\DeskPlusMobile\RM\AlarmView.aspx.cs
	\DeskPlusMobile\RM\RejectAdd.aspx
	\DeskPlusMobile\RM\RejectAdd.aspx.cs
	\DeskPlusMobile\RM\ReplyAdd.aspx
	\DeskPlusMobile\RM\ReplyAdd.aspx.cs
	\DeskPlusMobile\RM\UsableList.aspx.cs

: 2019-09-17 By famo23 ---------------------------------------------
[커뮤니티] 리스트에서 날짜 안 보이는 버그 수정 
	\DeskPlusMobile\Community\Jt\JTBoardList.htm

[커뮤니티] 모바일 알림 페이지 추가 
	\DeskPlusMobile\Community\Ws\SetCommunity.asmx
	\DeskPlusMobile\Community\Add.aspx
	\DeskPlusMobile\Community\Add.aspx.cs
	\DeskPlusMobile\Community\AlarmView.aspx
	\DeskPlusMobile\Community\AlarmView.aspx.cs
	\DeskPlusMobile\Js\Community\Add.js
	\DeskPlusMobile\Js\Community\AlarmView.js

[마이톡] 모바일 알림 추가 & 모바일 알림 링크
	\DeskPlusMobile\MyTalk\Ws\SetMyTalk.asmx.cs

[메뉴] 인트라 버전 디자인적용
	\DeskPlusMobile\Content\ko\Base\Css\C1\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C2\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C3\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C4\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C5\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C6\navimobile.css

: 2019-09-16 By 이중호 ---------------------------------------------
[일정관리] 지정시간에 알람인 경우에도 알람함 리스트에 전부 보이던 점 수정
	Table)
	DP_MDM_NotificationBox (SendDate 추가)
	
	Trigger)
	TRI_Alarm_Common_Insert

	SP)
	DeskPlusAlarm	DPM_ALARM_Common_Sel
	DeskPlusEIP		DPM_MDM_NotificationBox_SelReceiverID
	DeskPlusEIP		DPM_MDM_NotificationBox_SelReceiverIDCnt
	DeskPlusEIP		DPM_MDM_NotificationBox_SelReceiverIDNotReadCnt
	DeskPlusEIP		DPM_MDM_NotificationBox_RegularDel

: 2019-09-16 By famo23 ---------------------------------------------
[일정관리] 모바일 알림 페이지 추가 
	\DeskPlusMobile\Js\Pims\AlarmView.js
	\DeskPlusMobile\Js\Pims\ReplyAdd.js
	\DeskPlusMobile\Pims\Ws\BE\ScheduleReplyInfo.cs
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs
	\DeskPlusMobile\Pims\Ws\SetPims.asmx.cs
	\DeskPlusMobile\Pims\AlarmView.aspx
	\DeskPlusMobile\Pims\AlarmView.aspx.cs
	\DeskPlusMobile\Pims\ReplyAdd.aspx
	\DeskPlusMobile\Pims\ReplyAdd.aspx.cs

[일정관리] 댓글 버튼 디자인수정
	\DeskPlusMobile\Pims\View.aspx

[일정] 메신저 알림 링크경로가 잘못되어 있는 버그 수정 
	\DeskPlusMobile\Pims\Ws\SetPims.asmx.cs

[업무협업] 모바일 알림 페이지 추가 
	\DeskPlusMobile\Js\Task\AddAccept.js
	\DeskPlusMobile\Js\Task\AddComplete.js
	\DeskPlusMobile\Js\Task\AddReject.js
	\DeskPlusMobile\Js\Task\AddReply.js
	\DeskPlusMobile\Js\Task\AlarmView.js
	\DeskPlusMobile\Js\Task\BaseView.js
	\DeskPlusMobile\Js\Task\DelayView.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\View.js
	\DeskPlusMobile\Task\Ws\BLL\Alarm.cs
	\DeskPlusMobile\Task\Ws\GetTask.asmx.cs
	\DeskPlusMobile\Task\AddAccept.aspx
	\DeskPlusMobile\Task\AddAccept.aspx.cs
	\DeskPlusMobile\Task\AddComplete.aspx
	\DeskPlusMobile\Task\AddComplete.aspx.cs
	\DeskPlusMobile\Task\AddReject.aspx
	\DeskPlusMobile\Task\AddReject.aspx.cs
	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Task\AddReply.aspx.cs
	\DeskPlusMobile\Task\AlarmView.aspx
	\DeskPlusMobile\Task\AlarmView.aspx.cs
	\DeskPlusMobile\Task\BaseView.aspx
	\DeskPlusMobile\Task\BaseView.aspx.cs

[게시판] 댓글 알림이 모바일설정으로 했는데 메신저 알림으로 들어가는 버그수정
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs

[게시판] 모바일 알림 페이지 추가 
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Board\Add.aspx
	\DeskPlusMobile\Board\Add.aspx.cs
	\DeskPlusMobile\Board\AddShortReply.aspx
	\DeskPlusMobile\Board\AddShortReply.aspx.cs
	\DeskPlusMobile\Board\AlarmView.aspx
	\DeskPlusMobile\Board\AlarmView.aspx.cs
	\DeskPlusMobile\Board\ModShortReply.aspx
	\DeskPlusMobile\Board\ModShortReply.aspx.cs
	\DeskPlusMobile\Js\Board\Add.js
	\DeskPlusMobile\Js\Board\AddShortReply.js
	\DeskPlusMobile\Js\Board\AlarmView.js
	\DeskPlusMobile\Js\Board\ModShortReply.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

[알림함] TargetUrl 링크 이동
	\DeskPlusMobile\Js\MDM\List.js
	\DeskPlusMobile\MDM\WS\GetNotification.asmx.cs

[공통] 인증관련페이지 추가 
	\DeskPlusMobile\Portal\LoginOutSide.aspx
	
[공통] 알림링크 경로 
	\DeskPlusMobile\Messenger\BLL\Alarm.cs

: 2019-09-09 By 이중호 ---------------------------------------------
[메뉴 뱃지 카운트 제한]
	\DeskPlusMobile\Portal\Menu.ascx

: 2019-09-06 By 이중호 ---------------------------------------------
[메뉴 디자인 변경 : 1단 구성]
	\DeskPlusMobile\Content\ko\Base\Css\C1\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C2\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C3\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C4\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C5\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C6\navimobile.css

	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx

	\DeskPlusMobile\Portal\Menu.ascx

	\DeskPlusMobile\Portal\Menu_Backup.ascx ( 2단 구성 )

: 2019-09-03 By 이중호 ---------------------------------------------
[알림함] Push 메세지 리스트
[설정] 알람 시간 설정
	\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobile\App_Data\SQLPortal.xml

	\DeskPlusMobile\Approval\Ws\SetApproval.asmx

	\DeskPlusMobile\Config\List.aspx

	\DeskPlusMobile\Config\Ws\GetConfig.asmx
	\DeskPlusMobile\Config\Ws\SetConfig.asmx

	\DeskPlusMobile\Config\Ws\BE\ListInfo.cs

	\DeskPlusMobile\Js\MDM\List.js
	\DeskPlusMobile\Js\Config\List.js

	\DeskPlusMobile\MDM\List.aspx

	\DeskPlusMobile\MDM\Jt\JTList.htm

	\DeskPlusMobile\MDM\Ws\GetNotification.asmx
	\DeskPlusMobile\MDM\Ws\SetNotification.asmx

	\DeskPlusMobile\MDM\BE\ListInfo.cs
	\DeskPlusMobile\MDM\BE\Notification.cs

	\DeskPlusMobile\MDM\BLL\Notification.cs

	\DeskPlusMobile\MDM\Data\Notification.cs

	\DeskPlusMobile\Portal\Menu.ascx

	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

	DP_MDM_NotificationBox 테이블 추가
	DPM_ALARM_Common TargetMobileUrl 필드추가
	DPM_ALARM_CommonIOS TargetMobileUrl 필드추가 

	SP)
	SQL_DP_ALARM_Common_Insert
	DPM_MDM_List_Ins
	DPM_ALARM_Common_Sel
	DPM_ALARM_Common_SelByIOS

	DP_MDM_NotificationBox_Ins
	DPM_MDM_NotificationBox_SelReceiverID
	DPM_MDM_NotificationBox_SelReceiverIDCnt
	DPM_MDM_NotificationBox_SelReceiverIDNotReadCnt
	DPM_MDM_NotificationBox_Read
	DPM_MDM_NotificationBox_ReadAll
	DPM_MDM_NotificationBox_DelAll
	DPM_MDM_NotificationBox_RegularDel

	TABLE)
	DP_MDM_List 필드추가

	Trigger)
	TRI_Alarm_Common_Insert

: 2019-09-03 By famo23 ---------------------------------------------
[메뉴] 환경설정 메뉴 권한수정
	\DeskPlusMobile\Portal\Menu.ascx.cs

: 2019-08-30 By famo23 ---------------------------------------------
[전자결재]보안,긴급문서 표시 처리
	DPM_APP_DocProcess_SelReport
	DPM_APP_DocProcess_SelRelated
	DPM_APP_DocProcess_SelApprove
	\DeskPlusMobile\Content\ko\Base\Img\Emergency.gif
	\DeskPlusMobile\Content\ko\Base\Img\Security.gif
	\DeskPlusMobile\Approval\Jt\JTCirDocList.htm
	\DeskPlusMobile\Approval\Jt\JTDocList.htm
	\DeskPlusMobile\Approval\Jt\JTList.htm
	\DeskPlusMobile\Approval\Ws\BE\ListInfo.cs
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Portal\Jt\JTApproval.htm
	\DeskPlusMobile\Portal\Ws\BE\ApprovalInfo.cs
	\DeskPlusMobile\Portal\Ws\GetPortal.asmx.cs
	
[전자결재] 커스텀파일 
	\DeskPlusMobile\Legacy\Basic.aspx
	\DeskPlusMobile\Legacy\HoliDay.aspx
	\DeskPlusMobile\Legacy\Work.aspx
	\DeskPlusMobile\Legacy\WorkReport.aspx

[전자결재] NonAcitveXEditor에서 상신한 문서보기 할때 메뉴가 깨지는 문제 수정 
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs

[전자결재] 문서작성	
	DPM_APP_DocProcess_SelReport
	DPM_APP_DocProcess_SelApprove
	DPM_APP_DocProcess_SelRelated
	DPM_ACC_APP_SelName
	\DeskPlusMobile\App_Data\Approval.xml
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\App_Data\SQLAccount.xml
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\Account\BLL\Dept.cs
	\DeskPlusMobile\Account\Data\Dept.cs
	\DeskPlusMobile\Approval\Jt\JTBaseList.htm
	\DeskPlusMobile\Approval\Jt\JTFavForm.htm
	\DeskPlusMobile\Approval\Jt\JTForm.htm
	\DeskPlusMobile\Approval\Ws\BE\AttachInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\BaseDocumentInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\CarbonCopyUserInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\CirculationDocumentInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\CounterPartUserInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\DocumentProcessInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\FavoriteApprovalInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\FavSeqInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\FavSeqLineInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\FormInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\FormSeqInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\ListInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\SequenceInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\UserDeptInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\UserInfo.cs.cs
	\DeskPlusMobile\Approval\Ws\BLL\AttachFile.cs
	\DeskPlusMobile\Approval\Ws\BLL\CreatePDF.cs
	\DeskPlusMobile\Approval\Ws\BLL\DocumentProcess.cs
	\DeskPlusMobile\Approval\Ws\Data\AccessGrade.cs
	\DeskPlusMobile\Approval\Ws\Data\AttachFile.cs
	\DeskPlusMobile\Approval\Ws\Data\BaseDocument.cs
	\DeskPlusMobile\Approval\Ws\Data\CarbonCopyUser.cs
	\DeskPlusMobile\Approval\Ws\Data\CounterPartUser.cs
	\DeskPlusMobile\Approval\Ws\Data\DocumentProcess.cs
	\DeskPlusMobile\Approval\Ws\Data\Form.cs
	\DeskPlusMobile\Approval\Ws\Data\List.cs
	\DeskPlusMobile\Approval\Ws\Data\Sequence.cs
	\DeskPlusMobile\Approval\Ws\Data\ShortReply.cs
	\DeskPlusMobile\Approval\Ws\Common.asmx
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx
	\DeskPlusMobile\Approval\Add.aspx
	\DeskPlusMobile\Approval\EditorNonActiveX.aspx
	\DeskPlusMobile\Approval\Folder.aspx
	\DeskPlusMobile\Approval\Form.aspx
	\DeskPlusMobile\Approval\GetBaseDoc.aspx
	\DeskPlusMobile\Approval\Mod.aspx
	\DeskPlusMobile\Approval\ModSequence.aspx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Approval\WFolder.aspx
	\DeskPlusMobile\Content\ko\Base\Css\C1\mobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C2\mobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C3\mobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C4\mobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C5\mobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C6\mobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C1\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C2\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C3\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C4\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C5\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C6\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Img\ContextMenu\
	\DeskPlusMobile\Content\ko\Base\Img\CheckOut.gif
	\DeskPlusMobile\Editor\
	\DeskPlusMobile\Helper\WebComponent\Utility.cs
	\DeskPlusMobile\Helper\FileHelper.cs
	\DeskPlusMobile\Helper\UploadHandler.cs
	\DeskPlusMobile\Html\Js\JModal\DeskPlusModal.css
	\DeskPlusMobile\Html\Js\JModal\DeskPlusModal.js
	\DeskPlusMobile\Html\Js\DeskPlusEIPComponent.js
	\DeskPlusMobile\Html\Js\jquery.common.js
	\DeskPlusMobile\Html\Js\jquery.hashtable.js
	\DeskPlusMobile\Js\Approval\Add.js
	\DeskPlusMobile\Js\Approval\Common.js
	\DeskPlusMobile\Js\Approval\CommonFlow.js
	\DeskPlusMobile\Js\Approval\EditorNonActiveX.js
	\DeskPlusMobile\Js\Approval\Folder.js
	\DeskPlusMobile\Js\Approval\Form.js
	\DeskPlusMobile\Js\Approval\GetBaseDoc.js
	\DeskPlusMobile\Js\Approval\Mod.js
	\DeskPlusMobile\Js\Approval\ModSequence.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Js\Approval\WFolder.js
	\DeskPlusMobile\Portal\Jt\JTApproval.htm
	\DeskPlusMobile\Portal\Ws\BE\ApprovalInfo.cs
	\DeskPlusMobile\Portal\Ws\GetPortal.asmx.cs
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

[전자결재] 상신보류, 상신보류해제
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx
	
[전자결재] 상신취소 
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Ws\BLL\CirculationDocument.cs
	\DeskPlusMobile\Approval\Ws\BLL\DocumentProcess.cs
	\DeskPlusMobile\Approval\Ws\Data\CirculationDocument.cs
	\DeskPlusMobile\Approval\Ws\Data\DocumentProcess.cs
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

[전자결재] 후열
	DPM_APP_DocProcess_ApproveL
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx
	\DeskPlusMobile\Approval\Afterward.aspx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Js\Approval\Afterward.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

[전자결재] 임시저장, 모바일 임시보관함 
	DP_APP_Draft_Sel
	DPM_APP_Draft_SelCnt
	DPM_APP_Draft_Sel
	\DeskPlusMobile\App_Data\Approval.xml
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Jt\JTDraftList.htm	
	\DeskPlusMobile\Approval\Ws\BE\DraftBaseDocInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\DraftFileInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\DraftInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\DraftSequenceInfo.cs	
	\DeskPlusMobile\Approval\Ws\BE\ListInfo.cs
	\DeskPlusMobile\Approval\Ws\BLL\Draft.cs
	\DeskPlusMobile\Approval\Ws\BLL\DraftFile.cs	
	\DeskPlusMobile\Approval\Ws\Data\Draft.cs
	\DeskPlusMobile\Approval\Ws\Data\DraftBaseDoc.cs
	\DeskPlusMobile\Approval\Ws\Data\DraftFile.cs
	\DeskPlusMobile\Approval\Ws\Data\DraftSequence.cs	
	\DeskPlusMobile\Approval\Ws\Common.asmx
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx
	\DeskPlusMobile\Approval\Add.aspx	
	\DeskPlusMobile\Approval\DraftList.aspx
	\DeskPlusMobile\Approval\DraftAdd.aspx	
	\DeskPlusMobile\Approval\SaveDraft.aspx	
	\DeskPlusMobile\Js\Approval\Add.js
	\DeskPlusMobile\Js\Approval\Common.js
	\DeskPlusMobile\Js\Approval\CommonFlow.js
	\DeskPlusMobile\Js\Approval\DraftList.js
	\DeskPlusMobile\Js\Approval\DraftAdd.js
	\DeskPlusMobile\Js\Approval\SaveDraft.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

[전자결재] 모바일 상신문서함 
	DPM_APP_Doc_SelSubmitCnt
	DPM_APP_Doc_SelSubmit
	\DeskPlusMobile\App_Data\Approval.xml
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\App_Data\SQLApproval.xml	
	\DeskPlusMobile\Approval\Jt\JTSubmitList.htm
	\DeskPlusMobile\Approval\Ws\BE\DocInfo.cs
	\DeskPlusMobile\Approval\Ws\Data\Document.cs
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx
	\DeskPlusMobile\Approval\Add.aspx
	\DeskPlusMobile\Approval\SubmitAdd.aspx
	\DeskPlusMobile\Approval\SubmitList.aspx
	\DeskPlusMobile\Js\Approval\Add.js	
	\DeskPlusMobile\Js\Approval\SubmitAdd.js
	\DeskPlusMobile\Js\Approval\SubmitList.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx
	
[전자결재] 커스텀파일 
	\DeskPlusMobile\Legacy\Basic.aspx
	\DeskPlusMobile\Legacy\HoliDay.aspx
	\DeskPlusMobile\Legacy\Work.aspx
	\DeskPlusMobile\Legacy\WorkReport.aspx

[전자결재] 커스텀파일 - 결재선숨기기
	\DeskPlusMobile\Approval\Add.aspx
	\DeskPlusMobile\Legacy\ShowHide.aspx

[전자결재] 커스텀파일 - 특근내역가져오기
	\DeskPlusMobile\ETC\Jt\JTWorkList.htm
	\DeskPlusMobile\ETC\Ws\GetETC.asmx
	\DeskPlusMobile\ETC\WorkList.aspx
	\DeskPlusMobile\Js\ETC\WorkList.js

[전자결재] 커스텀파일 - 업무대행자
	DPM_ACC_SelName
	\DeskPlusMobile\ETC\Ws\BE\UserInfo.cs
	\DeskPlusMobile\ETC\Ws\GetETC.asmx
	\DeskPlusMobile\ETC\GetUserInfo.aspx
	\DeskPlusMobile\Js\ETC\GetUserInfo.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

[전자결재] 커스텀파일 - 달력
	\DeskPlusMobile\Common\Jt\JTCalendar.htm
	\DeskPlusMobile\Common\Jt\JTCalendarE.htm
	\DeskPlusMobile\Common\Jt\JTCalendarS.htm
	\DeskPlusMobile\Common\Ws\BE\CalendarInfo.cs
	\DeskPlusMobile\Common\Ws\GetCommon.asmx.cs
	\DeskPlusMobile\Common\Calendar.aspx
	\DeskPlusMobile\Common\CalendarDialog.aspx
	\DeskPlusMobile\Common\CalendarDualDialog.aspx
	\DeskPlusMobile\Js\Common\Calendar.js
	\DeskPlusMobile\Js\Common\CalendarDialog.js
	\DeskPlusMobile\Js\Common\CalendarDualDialog.js

: 2019-08-14 By famo23 ---------------------------------------------
[공통] 체크박스 전체선택이 영어일때 안되는 버그수정
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	AllSelect 변경

: 2019-08-09 By famo23 ---------------------------------------------
[전자결재] 대결자결재 디자인 깨지는 부분 수정
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js

: 2019-08-08 By famo23 ---------------------------------------------
[전자결재] 근거문서가 문서번호가 아니고 제목으로 나오는 부분수정
	\DeskPlusMobile\Js\Approval\DocView.js

[전자결재] 이중스크롤 나오는 부분 수정
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx

[전자결재] 유동결재망 테두리가 안 보이는 현상
	( css 파일에서 table{border-collapse:collapse}부분 제거)
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\bootstrap.mintable.css

: 2019-08-06 By famo23 ---------------------------------------------
[전자결재] 진행중인문서 상태값에 따라서 디자인수정
	\DeskPlusMobile\Approval\Jt\JTList.htm
	\DeskPlusMobile\Portal\Jt\JTApproval.htm
	\DeskPlusMobile\Content\ko\Base\Css\C1\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C2\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C3\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C4\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C5\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\C6\navimobile.css

[문서스트리밍] 파일등록시 경로를 deskpluseip -> deskplusmobile로 변경
	\DeskPlusMobile\Helper\WebComponent\DocStream.cs

[컴포넌트] 첨부파일 -> PDF 파일로 변환
	참조추가
	\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\Common\Ws\BE\PDFInfo.cs
	\DeskPlusMobile\Common\Ws\BLL\CreatePDF.cs
	\DeskPlusMobile\Common\Ws\Data\CreatePDF.cs
	\DeskPlusMobile\Helper\WebComponent\Aspose.Total.lic
	\DeskPlusMobile\Helper\WebComponent\AsposeEmail.cs
	\DeskPlusMobile\Helper\WebComponent\AsposeExcel.cs
	\DeskPlusMobile\Helper\WebComponent\AsposeLicense.cs
	\DeskPlusMobile\Helper\WebComponent\AsposePDF.cs
	\DeskPlusMobile\Helper\WebComponent\AsposePPT.cs
	\DeskPlusMobile\Helper\WebComponent\AsposeWord.cs
	\DeskPlusMobile\Helper\WebComponent\HtmlToPDF.cs
	\DeskPlusMobile\Helper\FileHelper.cs
	\DeskPlusMobile\Util\Aspose\
	\DeskPlusMobile\Util\PDF\
	\DeskPlusMobile\Global.asax

[전자결재] 결재승인 메신저 url 변경	
	\DeskPlusMobile\App_Data\Approval.xml

[공통] 파일 첨부로직 수정 (게시판,마이톡,전자결재)
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\Write.aspx.cs
	\DeskPlusMobile\Helper\UploadHandler.cs
	\DeskPlusMobile\MyTalk\Ws\GetMyTalk.asmx.cs
	\DeskPlusMobile\MyTalk\Ws\SetMyTalk.asmx.cs
	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\Add.aspx.cs
	\DeskPlusMobile\Web.config

: 2019-07-23 By famo23 ---------------------------------------------
[전자결재] 참조문서함에 완결문서만 나오게 수정 
	DPM_APP_Doc_SelCarboncopy
	DPM_APP_Doc_SelCarboncopyCnt
	DPM_APP_Doc_TopDown_Carboncopy

[전자결재] 결재문서함에서 FolderID 값 체크 안하게 수정 
	DPM_APP_Doc_SelApproval
	DPM_APP_Doc_SelApprovalCnt

: 2019-07-08 By famo23 ------------------------------------------------------------
[메일,게시판,웹폴더,마이톡] 첨부파일 이미지파일말고 다른파일형식도 등록되게,용량제한수정
	(마이톡은 용량제한만)
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\Helper\UploadAttachHandler.cs
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\Write.aspx.cs
	\DeskPlusMobile\Mail\DraftSend.aspx
	\DeskPlusMobile\Mail\DraftSend.aspx.cs
	\DeskPlusMobile\Mail\Send.aspx
	\DeskPlusMobile\Mail\Send.aspx.cs
	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\Add.aspx.cs
	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\WebHard\List.aspx.cs
	\DeskPlusMobile\WebHard\Send.aspx
	\DeskPlusMobile\WebHard\Send.aspx.cs
	\DeskPlusMobile\Js\Board\Write.js
	\DeskPlusMobile\Js\Mail\DraftSend.js
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Js\MyTalk\Add.js
	\DeskPlusMobile\Js\WebHard\List.js
	\DeskPlusMobile\Js\WebHard\Send.js

: 2019-05-23 By famo23 ------------------------------------------------------------
[공통] 첨부파일명에 특수기호 있을때 처리 (&,+)
	\DeskPlusMobile\Common\Ws\GetCommon.asmx.cs
	\DeskPlusMobile\Common\AttachUrl.aspx
	\DeskPlusMobile\Common\AttachUrl.aspx.cs
	\DeskPlusMobile\Common\AttachView.aspx.cs
	\DeskPlusMobile\Web.config
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Js\Board\View.js   
	\DeskPlusMobile\Js\Community\BoardView.js
	\DeskPlusMobile\Js\Pims\View.js  
	\DeskPlusMobile\Js\Task\BaseView.js
	\DeskPlusMobile\Js\Task\DelayView.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\View.js   

: 2019-05-31 By 방영환 수정중...------------------------------------------------------------
[메일] 리스트 검색조건 추가

	소스 수정)
	\DeskPlusMobile\App_Data\Mail.xml

	\DeskPlusMobile\Helper\Security\AES.cs

	\DeskPlusMobile\Js\Mail\DraftSend.js
	\DeskPlusMobile\Js\Mail\DraftView.js
	\DeskPlusMobile\Js\Mail\List.js
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Js\Mail\View.js

	\DeskPlusMobile\Mail\List.aspx.cs
	\DeskPlusMobile\Mail\Send.aspx.cs
	\DeskPlusMobile\Mail\Trash.aspx.cs
	\DeskPlusMobile\Mail\View.aspx.cs

	\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs
	\DeskPlusMobile\Mail\Ws\Data\List.cs

	\DeskPlusMobile\Potal\Menu.ascx.cs
	\DeskPlusMobile\Mail\Ws\GetPortal.asmx.cs

	리소스 추가)
	InputMail	제목,보낸이,받는이

: 2019-05-14 By famo23 ------------------------------------------------------------
[로그인] App다운로드 버튼이 체크가 안되는 브라우저가 있어서 모두 보이게 처리
	\DeskPlusMobile\Js\Portal\Default.js
	\DeskPlusMobile\Portal\Default.aspx

[메일] 전달, 답장, 전체답장일때는 본문내용 입력안해도 발송되게 수정
	\DeskPlusMobile\Js\Mail\Send.js

: 2019-05-14 By 방영환 수정중...------------------------------------------------------------
[메일] 메일발송시 첨부파일 첨부
	소스 추가)	
	DeskPlusMobile\WebHard\Send.aspx.cs

	DeskPlusMobile\WebHard\Ws\BE\AttachInfo.cs

	소스 수정)
	DeskPlusMobile\Address\List.aspx.cs
	DeskPlusMobile\WebHard\List.aspx.cs
	DeskPlusMobile\WebHard\Send.aspx.cs

	DeskPlusMobile\Js\Address\List.js
	DeskPlusMobile\Js\WebHard\List.js
	DeskPlusMobile\Js\WebHard\Send.js
	DeskPlusMobile\Js\Mail\Send.js
	DeskPlusMobile\Js\Mail\DraftSend.js
	
	DeskPlusMobile\WebHard\Ws\GetWebHard.asmx.cs
	DeskPlusMobile\WebHard\Ws\SetWebHard.asmx.cs
	DeskPlusMobile\Mail\Ws\GetMAil.asmx.cs
	DeskPlusMobile\Mail\Ws\SetMail.asmx.cs

	DeskPlusMobile\Helper\UploadMailHandler.cs

	DeskPlusMobile\App_Data\Mail.xml

	리소스 추가)
	MailError4	메일형식이 잘못되었습니다
	MailError5	메일주소가 잘못되었습니다

: 2019-05-02 By 이중호 ------------------------------------------------------------
[일정관리] 디자인 및 일정 정렬 변경
	DeskPlusMobile\Pims\Ws\GetPims.asmx
	DeskPlusMobile\Portal\Ws\GetPortal.asmx

	DeskPlusMobile\Pims\List.aspx
	DeskPlusMobile\Portal\Portal.aspx

	DeskPlusMobile\Js\Pims\List.js
	DeskPlusMobile\Js\Portal\Portal.js

	DeskPlusMobile\Pims\Jt\JTSchedule.htm
	DeskPlusMobile\Pims\Jt\JTToDo.htm
	DeskPlusMobile\Portal\Jt\JTSchedule.htm
	DeskPlusMobile\Portal\Jt\JTToDo.htm

: 2019-04-30 By 이중호 ------------------------------------------------------------
[일정관리] 디자인 및 일정 정렬 변경
	DeskPlusMobile\Address\Ws\GetAddress.asmx

	DeskPlusMobile\Content\ko\Base\Css\C1\navimobile.css
	DeskPlusMobile\Content\ko\Base\Css\C2\navimobile.css
	DeskPlusMobile\Content\ko\Base\Css\C3\navimobile.css
	DeskPlusMobile\Content\ko\Base\Css\C4\navimobile.css
	DeskPlusMobile\Content\ko\Base\Css\C5\navimobile.css
	DeskPlusMobile\Content\ko\Base\Css\C6\navimobile.css

	DeskPlusMobile\Pims\Jt\JTSchedule.htm
	DeskPlusMobile\Pims\Jt\JTToDo.htm
	DeskPlusMobile\Pims\Ws\GetPims.asmx

	DeskPlusMobile\Portal\Jt\JTSchedule.htm
	DeskPlusMobile\Portal\Jt\JTToDo.htm
	DeskPlusMobile\Portal\Ws\GetPortal.asmx


: 2019-04-12 By famo23 ------------------------------------------------------------
[App] 로그아웃시 기기 삭제 하지 않고 알람 안 받게 처리 
	DP_MDM_List 테이블 필드추가 LogOutYN
	DPM_MDM_List_Ins
	DPM_MDM_List_UpdLogOutYN
	DPM_ALARM_Common_Sel
	DPM_ALARM_Common_SelByIOS		
	\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	\DeskPlusMobile\Portal\SetLogOut.aspx.cs

: 2019-03-29 By famo23 ------------------------------------------------------------
[App] ios - 주소록에서 전화걸기,문자보내기 안되는 현상수정
	\DeskPlusMobile\Address\View.aspx
	\DeskPlusMobile\Js\Address\View.js

[App] ios - 첨부파일열기 새창으로 처리 
     \DeskPlusMobile\Common\AttachUrl.aspx
	 \DeskPlusMobile\Common\AttachUrl.aspx.cs	
	 \DeskPlusMobile\Js\Approval\BaseView.js
	 \DeskPlusMobile\Js\Approval\CirDocView.js
	 \DeskPlusMobile\Js\Approval\DocView.js
	 \DeskPlusMobile\Js\Approval\View.js 
	 \DeskPlusMobile\Js\Board\View.js    
	 \DeskPlusMobile\Js\Community\BoardView.js  
	 \DeskPlusMobile\Js\Mail\View.js      
	 \DeskPlusMobile\Js\Mail\ViewTrash.js 
	 \DeskPlusMobile\Js\Pims\View.js     
	 \DeskPlusMobile\Js\Task\BaseView.js 
	 \DeskPlusMobile\Js\Task\DelayView.js
	 \DeskPlusMobile\Js\Task\ForwardView.js
	 \DeskPlusMobile\Js\Task\View.js      
	 \DeskPlusMobile\Js\WebHard\List.js 	 
	 \DeskPlusMobile\Approval\BaseView.aspx
	 \DeskPlusMobile\Approval\CirDocView.aspx
	 \DeskPlusMobile\Approval\DocView.aspx
	 \DeskPlusMobile\Approval\View.aspx
	 \DeskPlusMobile\Board\View.aspx     
	 \DeskPlusMobile\Community\BoardView.aspx
	 \DeskPlusMobile\Mail\View.aspx    
	 \DeskPlusMobile\Mail\ViewTrash.aspx
	 \DeskPlusMobile\Pims\View.aspx      
	 \DeskPlusMobile\Task\BaseView.aspx  
	 \DeskPlusMobile\Task\DelayView.aspx
	 \DeskPlusMobile\Task\ForwardView.aspx
	 \DeskPlusMobile\Task\View.aspx    
	 \DeskPlusMobile\WebHard\List.aspx  
	 \DeskPlusMobile\Web.config

[App] 알람쿼리 수정 - null 값체크
	DPM_ALARM_Common_Sel
	DPM_ALARM_Common_SelByIOS


[공통] 쿠키 시간 설정 48시간으로 설정변경 
	\DeskPlusMobile\Web.config

[전자결재] 진행중인 문서 보기에서 문서 없을 시 에러 처리
	\DeskPlusMobile\Approval\View.aspx.cs

[전자결재] 결재하고선 back 버튼 실행시에 문서 정보 다시 가져오게 처리 
	\DeskPlusMobile\Approval\Approve.aspx.cs
	\DeskPlusMobile\Approval\Return.aspx.cs

[App] 안드로이드 로그인 페이지에서 back버튼 클릭시 종료버튼 설정
	\DeskPlusMobile\Js\Portal\AnLogin.js
	\DeskPlusMobile\Portal\LogOut.aspx
	\DeskPlusMobile\Portal\SetLogOut.aspx

[App] 아이폰 X버전 이상에서 상단 메뉴 검은색문제 
	\DeskPlusMobile\Html\Js\Design\navimobile.js
	\DeskPlusMobile\Js\Portal\IosLogin.js
	\DeskPlusMobile\Portal\IosLogin.aspx

[App] 안드로이드 - 첨부파일 열기 롤백작업
	 \DeskPlusMobile\Js\Approval\BaseView.js
	 \DeskPlusMobile\Js\Approval\CirDocView.js
	 \DeskPlusMobile\Js\Approval\DocView.js
	 \DeskPlusMobile\Js\Approval\View.js 
	 \DeskPlusMobile\Js\Board\View.js    
	 \DeskPlusMobile\Js\Community\BoardView.js  
	 \DeskPlusMobile\Js\Mail\View.js      
	 \DeskPlusMobile\Js\Mail\ViewTrash.js 
	 \DeskPlusMobile\Js\Pims\View.js     
	 \DeskPlusMobile\Js\Task\BaseView.js 
	 \DeskPlusMobile\Js\Task\DelayView.js
	 \DeskPlusMobile\Js\Task\ForwardView.js
	 \DeskPlusMobile\Js\Task\View.js      
	 \DeskPlusMobile\Js\WebHard\List.js 	
     \DeskPlusMobile\Approval\BaseView.aspx
	 \DeskPlusMobile\Approval\CirDocView.aspx
	 \DeskPlusMobile\Approval\DocView.aspx
	 \DeskPlusMobile\Approval\View.aspx
	 \DeskPlusMobile\Board\View.aspx     
	 \DeskPlusMobile\Community\BoardView.aspx
	 \DeskPlusMobile\Mail\View.aspx    
	 \DeskPlusMobile\Mail\ViewTrash.aspx
	 \DeskPlusMobile\Pims\View.aspx      
	 \DeskPlusMobile\Task\BaseView.aspx  
	 \DeskPlusMobile\Task\DelayView.aspx
	 \DeskPlusMobile\Task\ForwardView.aspx
	 \DeskPlusMobile\Task\View.aspx    
	 \DeskPlusMobile\WebHard\List.aspx  
	 \DeskPlusMobile\Helper\MobileBasePage.cs
	 \DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	 DPM_MDM_List_Ins
	 DPM_MDM_List_UpdByRegID
	 DP_MDM_List AppVersion 필드삭제

[App] 안드로이드 파일첨부 방식 수정 (4.0대 버전에서는 작동하지 않음)
	\DeskPlusMobile\Js\Board\Write.js
	\DeskPlusMobile\Js\MyTalk\Add.js
	\DeskPlusMobile\Js\WebHard\List.js

[App] Badge 변경
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Portal.aspx.cs
	\DeskPlusMobile\Portal\LogOut.aspx
	\DeskPlusMobile\Portal\SetLogOut.aspx
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\Return.js
	\DeskPlusMobile\Js\Portal\Portal.js
	
[App] 버전업데이트, 안드로이드 푸시동의화면
	\DeskPlusMobile\App\agree.htm
	\DeskPlusMobile\App\android_version.dat
	\DeskPlusMobile\App\ios_version.dat
	\DeskPlusMobile\App\android_update.htm
	\DeskPlusMobile\App\ios_update.htm

[App] 로그아웃 수정 
	\DeskPlusMobile\Web.config
	\DeskPlusMobile\Portal\LogOut.aspx
	\DeskPlusMobile\Portal\LogOut.aspx.cs
	\DeskPlusMobile\Portal\SetLogOut.aspx
	\DeskPlusMobile\Portal\SetLogOut.aspx.cs
	\DeskPlusMobile\Config\List.aspx

[App] ios - 로그인  
	\DeskPlusMobile\Web.config
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	\DeskPlusMobile\Portal\IosLogin.aspx
	\DeskPlusMobile\Portal\IosLogin.aspx.cs
	\DeskPlusMobile\Js\Portal\IosLogin.js

[App] Android - 로그인 
	\DeskPlusMobile\Web.config	
	\DeskPlusMobile\Portal\AnLogin.aspx
	\DeskPlusMobile\Portal\AnLogin.aspx.cs
	\DeskPlusMobile\Js\Portal\AnLogin.js	
	\DeskPlusMobile\App\Info.aspx

[App] Push 테스트 개발 
	\DeskPlusMobile\App\FCM.aspx
	\DeskPlusMobile\App\iosPushTest.aspx
	\DeskPlusMobile\App\Push.aspx
	\DeskPlusMobile\App\PushTest.aspx	

: 2018-03-20 by kkzz52 --------------------------------------------------------
[근태] 근태관리 수정 (출퇴근 시 지각,조퇴 분 추가 및 출퇴근 시 메시지 표시)
	\DeskPlusMobile\Js\Portal\Portal.js
	\DeskPlusMobile\Js\HR\Attendance.js
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\HR\Attendance.aspx
	\DeskPlusMobile\HR\Ws\SetAttendance.asmx.cs

	-리소스 추가
	LocationIn
	LocationOut
	Location
	GotoTime
	GotoSucceed
	OffWorkTime
	OffWorkSucceed
	EarlyTime
	EarlySucceed

: 2019-03-19 By 방영환(수정중) ------------------------------------------------------------
[메일] DEEP 메일 첨부파일 추가
	소스 수정)	
	\DeskPlusMobile\Mail\Folder.aspx
	\DeskPlusMobile\Mail\Send.aspx
	\DeskPlusMobile\Mail\List.aspx

	\DeskPlusMobile\Address\List.aspx

	\DeskPlusMobile\Mail\Ws\GetMail.asmx
	\DeskPlusMobile\Mail\Ws\SetMail.asmx

	\DeskPlusMobile\Js\Mail\Folder.js
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Js\Mail\List.js

	\DeskPlusMobile\Js\Address\List.js
	 
	소스 추가)
	\DeskPlusMobile\Mail\DraftSend.aspx
	\DeskPlusMobile\Mail\DraftView.aspx

	\DeskPlusMobile\Js\Mail\DraftSend.js
	\DeskPlusMobile\Js\Mail\DraftView.js
	
	\DeskPlusMobile\Mail\Ws\BE\AttachInfo.cs

	\DeskPlusMobile\Helper\UploadMailHandler.cs

	리소스 추가)
	DraftMail	임시보관함
	DraftSend	메일발송

: 2019-03-11 By 방영환 ------------------------------------------------------------
[일정관리] 일정, 할일 수정시 날짜 변경이 가능 하도록 수정
	소스 수정)
	\DeskPlusMobile\Pims\Mod.aspx
	\DeskPlusMobile\Pims\View.aspx
	\DeskPlusMobile\Pims\Ws\GetPims.asmx
	\DeskPlusMobile\Pims\Ws\GetPims.asmx
	\DeskPlusMobile\Js\Pims\Add.js
	\DeskPlusMobile\Js\Pims\Mod.js

	프로시저 수정)
	DPM_PIM_ToDo_Upd

: 2018-12-27 By famo23 ------------------------------------------------------------
[로그인] 접속 타입구분 (Web(0),App(1),타업체접속(2),겸직변경(3) 구분)
	\DeskPlusMobile\Js\Portal\Default.js
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	DPM_COM_AccessLog LogType 필드추가
	DPM_COM_AccessLog_Ins

: 2018-12-26 by famo23 --------------------------------------------------------
[공통] Android App에서 첨부파일이 안 열리는 현상
	 \DeskPlusMobile\Common\AttachUrl.aspx
	 \DeskPlusMobile\Common\AttachUrl.aspx.cs	
	 \DeskPlusMobile\Common\AttachMail.aspx
	 \DeskPlusMobile\Common\AttachMail.aspx.cs
	 \DeskPlusMobile\Js\Approval\BaseView.js
	 \DeskPlusMobile\Js\Approval\CirDocView.js
	 \DeskPlusMobile\Js\Approval\DocView.js
	 \DeskPlusMobile\Js\Approval\View.js
	 \DeskPlusMobile\Js\Board\View.js   
	 \DeskPlusMobile\Js\Community\BoardView.js
	 \DeskPlusMobile\Js\Mail\View.js
	 \DeskPlusMobile\Js\Mail\ViewTrash.js
	 \DeskPlusMobile\Js\Pims\View.js    
	 \DeskPlusMobile\Js\Task\BaseView.js
	 \DeskPlusMobile\Js\Task\DelayView.js
	 \DeskPlusMobile\Js\Task\ForwardView.js
	 \DeskPlusMobile\Js\Task\View.js   
	 \DeskPlusMobile\Js\WebHard\List.js 
	 \DeskPlusMobile\Approval\BaseView.aspx
	 \DeskPlusMobile\Approval\CirDocView.aspx
	 \DeskPlusMobile\Approval\DocView.aspx
	 \DeskPlusMobile\Approval\View.aspx
	 \DeskPlusMobile\Board\View.aspx   
	 \DeskPlusMobile\Community\BoardView.aspx
	 \DeskPlusMobile\Mail\View.aspx
	 \DeskPlusMobile\Mail\ViewTrash.aspx
	 \DeskPlusMobile\Pims\View.aspx    
	 \DeskPlusMobile\Task\BaseView.aspx 
	 \DeskPlusMobile\Task\DelayView.aspx
	 \DeskPlusMobile\Task\ForwardView.aspx
	 \DeskPlusMobile\Task\View.aspx   
	 \DeskPlusMobile\WebHard\List.aspx 
	 \DeskPlusMobile\Web.config
	 \DeskPlusMobile\Helper\MobileBasePage.cs
	 \DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	 DPM_MDM_List_Ins
	 DPM_MDM_List_UpdByRegID
	 DP_MDM_List AppVersion 필드추가

: 2018-12-20 by kkzz52 --------------------------------------------------------
[포탈] 근태 포틀릿 추가
	\DeskPlusMobile\Js\Portal\Portal.js
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Portal.aspx.cs
	\DeskPlusMobile\Js\HR\Attendance.js
	\DeskPlusMobile\HR\Attendance.aspx
	\DeskPlusMobile\HR\Attendance.aspx.cs

: 2018-12-20 by 방영환 --------------------------------------------------------
[전자결재] 결재취소(관계있는문서) 
	소스 수정)
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\XmlData\Confirm\en.xml
	\DeskPlusMobile\XmlData\Confirm\fr.xml
	\DeskPlusMobile\XmlData\Confirm\ja.xml
	\DeskPlusMobile\XmlData\Confirm\ko.xml
	\DeskPlusMobile\XmlData\Confirm\zh-cn.xml
	\DeskPlusMobile\XmlData\Confirm\zh-tw.xml

	리소스 추가)
	CancelApprovalCheck	해당문서는 이미 결재가 진행중이거나, 다음결재자가 이미 결재한 상태입니다.

	프로시저 추가)
	DPM_APP_DocProcess_CancelApproval

: 2018-12-20 by famo23 --------------------------------------------------------
[포탈] 전자결재, 업무협업 메인포틀릿 추가 
	\DeskPlusMobile\Js\Portal\Portal.js
	\DeskPlusMobile\Portal\Portal.aspx.cs
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

[업무협업] 지연된 업무 추가 
	DPM_TAS_List_SelDelayCnt
	DPM_TAS_List_SelDelay
	DPM_TAS_List_TopDown_Delay
	DPM_TAS_SelCnt
	\DeskPlusMobile\App_Data\SQLTask.xml
	\DeskPlusMobile\Task\Ws\Data\List.cs
	\DeskPlusMobile\Task\Ws\GetTask.asmx.cs
	\DeskPlusMobile\Js\Task\AddComplete.js
	\DeskPlusMobile\Js\Task\AddReply.js
	\DeskPlusMobile\Js\Task\DelayList.js
	\DeskPlusMobile\Js\Task\DelayView.js
	\DeskPlusMobile\Js\Task\Folder.js
	\DeskPlusMobile\Task\DelayList.aspx
	\DeskPlusMobile\Task\DelayView.aspx
	\DeskPlusMobile\Task\Folder.aspx
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

: 2018-12-18 by famo23 --------------------------------------------------------
[포탈] 관리자가 설정한 포틀릿이 나오게 처리
	\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Portal.aspx.cs

: 2018-12-13 by 이중호 --------------------------------------------------------
[기능 버튼에 대한 Class 변경 파일정보]
	\DeskPlusMobile\Approval\view.aspx
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\DocOpinion.aspx
	\DeskPlusMobile\Approval\Return.aspx

	\DeskPlusMobile\Board\Add.aspx
	\DeskPlusMobile\Board\AddShortReply.aspx
	\DeskPlusMobile\Board\ModShortReply.aspx
	\DeskPlusMobile\Board\Write.aspx

	\DeskPlusMobile\Pims\add.aspx
	\DeskPlusMobile\Pims\mod.aspx

	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\View.aspx

	\DeskPlusMobile\Task\AddAccept.aspx
	\DeskPlusMobile\Task\AddComlete.aspx
	\DeskPlusMobile\Task\AddReject.aspx
	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Task\View.aspx

	\DeskPlusMobile\RM\RejectAdd.aspx
	\DeskPlusMobile\RM\RepeatDel.aspx
	\DeskPlusMobile\RM\ReplyAdd.aspx
	\DeskPlusMobile\RM\View.asp

	\DeskPlusMobile\HR\Attendance.aspx

: 2018-12-13 by 이중호 --------------------------------------------------------
[스킨설정]
	\DeskPlusMobile\Content\ko\Base\Css\C1\
	\DeskPlusMobile\Content\ko\Base\Css\C2\
	\DeskPlusMobile\Content\ko\Base\Css\C3\
	\DeskPlusMobile\Content\ko\Base\Css\C4\
	\DeskPlusMobile\Content\ko\Base\Css\C5\
	\DeskPlusMobile\Content\ko\Base\Css\C6\

	\DeskPlusMobile\Content\ko\Base\Img\C1.png
	\DeskPlusMobile\Content\ko\Base\Img\C2.png
	\DeskPlusMobile\Content\ko\Base\Img\C3.png
	\DeskPlusMobile\Content\ko\Base\Img\C4.png
	\DeskPlusMobile\Content\ko\Base\Img\C5.png
	\DeskPlusMobile\Content\ko\Base\Img\C6.png

	\DeskPlusMobile\Address\Add.aspx
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Address\View.aspx

	\DeskPlusMobile\Approval\AddOpinion.aspx
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CarboncopyList.aspx
	\DeskPlusMobile\Approval\CirDocList.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DeptList.aspx
	\DeskPlusMobile\Approval\DocList.aspx
	\DeskPlusMobile\Approval\DocOpinion.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\Folder.aspx
	\DeskPlusMobile\Approval\List.aspx
	\DeskPlusMobile\Approval\Memo.aspx
	\DeskPlusMobile\Approval\Proxy.aspx
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Approval\View.aspx

	\DeskPlusMobile\Attendance\Add.aspx

	\DeskPlusMobile\Board\Add.aspx
	\DeskPlusMobile\Board\AddShortReply.aspx
	\DeskPlusMobile\Board\Folder.aspx
	\DeskPlusMobile\Board\List.aspx
	\DeskPlusMobile\Board\ModShortReply.aspx
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\Write.aspx

	\DeskPlusMobile\Community\Add.aspx
	\DeskPlusMobile\Community\BoardFolder.aspx
	\DeskPlusMobile\Community\BoardList.aspx
	\DeskPlusMobile\Community\BoardView.aspx
	\DeskPlusMobile\Community\Folder.aspx

	\DeskPlusMobile\Config\List.aspx

	\DeskPlusMobile\HR\Attendance.aspx

	\DeskPlusMobile\Mail\DraftSend.aspx
	\DeskPlusMobile\Mail\DraftView.aspx
	\DeskPlusMobile\Mail\Folder.aspx
	\DeskPlusMobile\Mail\List.aspx
	\DeskPlusMobile\Mail\Private.aspx
	\DeskPlusMobile\Mail\Send.aspx
	\DeskPlusMobile\Mail\Trash.aspx
	\DeskPlusMobile\Mail\View.aspx
	\DeskPlusMobile\Mail\ViewTrash.aspx

	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\List.aspx
	\DeskPlusMobile\MyTalk\View.aspx

	\DeskPlusMobile\Pims\Add.aspx
	\DeskPlusMobile\Pims\List.aspx
	\DeskPlusMobile\Pims\Mod.aspx
	\DeskPlusMobile\Pims\ReplyAdd.aspx
	\DeskPlusMobile\Pims\Share.aspx
	\DeskPlusMobile\Pims\View.aspx

	\DeskPlusMobile\Portal\Portal.aspx

	\DeskPlusMobile\RM\Add.aspx
	\DeskPlusMobile\RM\Folder.aspx
	\DeskPlusMobile\RM\List.aspx
	\DeskPlusMobile\RM\RejectAdd.aspx
	\DeskPlusMobile\RM\RepeatDel.aspx
	\DeskPlusMobile\RM\ReplyAdd.aspx
	\DeskPlusMobile\RM\UsableList.aspx
	\DeskPlusMobile\RM\View.aspx

	\DeskPlusMobile\Task\AddAccept.aspx
	\DeskPlusMobile\Task\AddComplete.aspx
	\DeskPlusMobile\Task\AddReject.aspx
	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Task\BaseView.aspx
	\DeskPlusMobile\Task\Folder.aspx
	\DeskPlusMobile\Task\FowardList.aspx
	\DeskPlusMobile\Task\FowardView.aspx
	\DeskPlusMobile\Task\List.aspx
	\DeskPlusMobile\Task\View.aspx

	\DeskPlusMobile\WebHard\Folder.aspx
	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\WebHard\Send.aspx
	\DeskPlusMobile\WebHard\Share.aspx



: 2018-12-07 by kkzz52 --------------------------------------------------------
[근태] 근태관리 추가
	\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\Helper\XmlDataBaseHelper.cs	
	\DeskPlusMobile\App_Data\SQLHR.xml 추가
	\DeskPlusMobile\Js\HR\Attendance.js 추가
	\DeskPlusMobile\HR\Attendance.aspx 추가
	\DeskPlusMobile\HR\Ws\GetAttendance.asmx 추가
	\DeskPlusMobile\HR\Ws\SetAttendance.asmx 추가
	\DeskPlusMobile\HR\Ws\Data\HR.cs 추가
	\DeskPlusMobile\Legacy\HoliDay_Complete.aspx
        \DeskPlusMobile\Legacy\Basic_Complete.aspx
        \DeskPlusMobile\Legacy\Work_Complete.aspx
        \DeskPlusMobile\Legacy\WorkReport_Complete.aspx

	-리소스 추가
	WeekOverTime
	WeekOverTimeMsg
	Attendance_EarlyTime
	Attendance_EarlyConfirm
	Attendance_EarlyRe
	Attendance_EarlyCancelConfirm
	Attendance_EarlyTimeMsg
	Attendance_GotoWorkRe

: 2018-12-04 by 이지원 --------------------------------------------------------
[휴일관리] 대체휴일사용여부 공휴일/토 옵션 수정
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobile\Coop\Ws\GetCoop.asmx.cs
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs
	\DeskPlusMobile\RM\Ws\GetRM.asmx.cs
	\DeskPlusMobile\Task\Ws\GetTask.asmx.cs


: 2018-12-04 By famo23  ----------------------------------------------------------
[메일]ios기기에서 자동완성할때 이전입력문자와 이어서 문자완성되는 문제 
	\DeskPlusMobile\Js\Mail\Send.js

[게시판]하위게시판 앞뒤게시물 가져오기 버그 수정 
	\DeskPlusMobile\Board\View.aspx.cs

: 2018-10-30 By 이중호  ----------------------------------------------------------
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Js\Board\List.js
	\DeskPlusMobile\Js\Mail\List.js
	\DeskPlusMobile\Js\Mail\Trash.js
	\DeskPlusMobile\Js\MyTalk\Add.js
	\DeskPlusMobile\Js\MyTalk\List.js
	\DeskPlusMobile\Js\Pims\View.js
	\DeskPlusMobile\Js\Portal\Default.js
	\DeskPlusMobile\Js\Portal\Portal.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\View.js
	\DeskPlusMobile\Js\WebHard\List.js

	\DeskPlusMobile\Content\ko\Base\Css\DStyle\mobile.css
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\navimobile.css
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\style.css

	\DeskPlusMobile\Address\Add.aspx

	\DeskPlusMobile\Approval\AddOpinion.aspx
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DeptList.aspx.cs
	\DeskPlusMobile\Approval\DocOpinion.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\Folder.aspx
	\DeskPlusMobile\Approval\Memo.aspx
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Approval\View.aspx

	\DeskPlusMobile\Attendance\Add.aspx

	\DeskPlusMobile\Board\Add.aspx
	\DeskPlusMobile\Board\AddShortReply.aspx
	\DeskPlusMobile\Board\ModShortReply.aspx
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\JtWJTList.htm

	\DeskPlusMobile\Community\Add.aspx
	\DeskPlusMobile\Community\BoardList.aspx.cs
	\DeskPlusMobile\Community\BoardView.aspx
	\DeskPlusMobile\Community\Jt\JTBoardFolder.htm
	\DeskPlusMobile\Community\Jt\JTBoardList.htm
	\DeskPlusMobile\Community\Jt\JTFolder.htm

	\DeskPlusMobile\Config\List.aspx

	\DeskPlusMobile\Mail\List.aspx
	\DeskPlusMobile\Mail\Trash.aspx
	\DeskPlusMobile\Mail\View.aspx
	\DeskPlusMobile\Mail\ViewTrash.aspx
	\DeskPlusMobile\Mail\Jt\JTList.htm
	\DeskPlusMobile\Mail\Jt\JTTrash.htm
	\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs

	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\Jt\JtList.htm
	\DeskPlusMobile\MyTalk\Jt\JtSNSList.htm
	\DeskPlusMobile\MyTalk\Ws\GetMyTalk.asmx.cs

	\DeskPlusMobile\Pims\Add.aspx
	\DeskPlusMobile\Pims\List.aspx
	\DeskPlusMobile\Pims\Mod.aspx
	\DeskPlusMobile\Pims\ReplyAdd.aspx
	\DeskPlusMobile\Pims\View.aspx

	\DeskPlusMobile\Portal\Defalut.aspx
	\DeskPlusMobile\Portal\LogOut.aspx
	\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Jt\JTMail.htm
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	\DeskPlusMobile\Portal\Ws\GetPortal.asmx.cs

	\DeskPlusMobile\RM\RejectAdd.aspx
	\DeskPlusMobile\RM\RepeatDel.aspx
	\DeskPlusMobile\RM\View.aspx

	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Task\BaseView.aspx
	\DeskPlusMobile\Task\ForwardView.aspx
	\DeskPlusMobile\Task\View.aspx

	\DeskPlusMobile\WebHard\Folder.aspx
	\DeskPlusMobile\WebHard\List.aspx

	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx


: 2018-10-30 By 이중호  ----------------------------------------------------------
[디자인] 수정사항
	\DeskPlusMobile\App_Data\SQLPortal.xml

	\DeskPlusMobile\Board\Jt\JTShortReply.htm

	\DeskPlusMobile\Content\ko\Base\Css\DStyle\mobile.css
	\DeskPlusMobile\Content\ko\Base\Css\DStyle\navimobile.css

	\DeskPlusMobile\Mail\Jt\JTList.htm
	\DeskPlusMobile\Mail\Jt\JTTrash.htm
	\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs
	\DeskPlusMobile\Mail\Ws\BE\ListInfo.cs

	\DeskPlusMobile\Portal\Default.aspx.cs
	\DeskPlusMobile\Portal\LogOut.aspx
	\DeskPlusMobile\Portal\Portal.aspx

: 2018-10-30 By 이중호  ----------------------------------------------------------
[디자인] 수정사항
	\DeskPlusMobile\Address\Add.aspx
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Address\View.aspx

	\DeskPlusMobile\Approval\CarboncopyList.aspx
	\DeskPlusMobile\Approval\CirDocList.aspx
	\DeskPlusMobile\Approval\DeptList.aspx
	\DeskPlusMobile\Approval\DocList.aspx
	\DeskPlusMobile\Approval\List.aspx

	\DeskPlusMobile\Attendance\Add.aspx

	\DeskPlusMobile\Board\AddShortReply.aspx
	\DeskPlusMobile\Board\List.aspx
	\DeskPlusMobile\Board\ModShortReply.aspx
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\Jt\JTList.htm
	\DeskPlusMobile\Board\Jt\JTShortReply.htm

	\DeskPlusMobile\Community\Add.aspx
	\DeskPlusMobile\Community\BoardList.aspx
	\DeskPlusMobile\Community\BoardView.aspx

	\DeskPlusMobile\Config\List.aspx

	\DeskPlusMobile\Content\\ko\Base\Css\DStyle\mobile.css
	\DeskPlusMobile\Content\\ko\Base\Css\DStyle\navimobile.css
	\DeskPlusMobile\Content\\ko\Base\Css\DStyle\style.css

	\DeskPlusMobile\Js\MyTalk\View.js
	\DeskPlusMobile\Js\Pims\List.js
	\DeskPlusMobile\Js\Pims\View.js
	\DeskPlusMobile\Js\RM\List.js
	\DeskPlusMobile\Js\RM\View.js
	\DeskPlusMobile\Js\Task\FowardView.js
	\DeskPlusMobile\Js\Task\View.js

	\DeskPlusMobile\Mail\Jt\JTList.htm
	\DeskPlusMobile\Mail\Jt\JTTrash.htm

	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\List.aspx
	\DeskPlusMobile\MyTalk\View.aspx
	\DeskPlusMobile\MyTalk\Jt\JTList.htm
	\DeskPlusMobile\MyTalk\Jt\JTSNSList.htm

	\DeskPlusMobile\Pims\List.aspx
	\DeskPlusMobile\Pims\View.aspx
	\DeskPlusMobile\Pims\Jt\JTSchedule.htm
	\DeskPlusMobile\Pims\Jt\JTShare.htm
	\DeskPlusMobile\Pims\Jt\JTToDo.htm
	\DeskPlusMobile\Pims\Ws\GetPims.asmx

	\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Jt\JTBoard.htm
	\DeskPlusMobile\Portal\Jt\JTMail.htm
	\DeskPlusMobile\Portal\Jt\JTSchedule.htm
	\DeskPlusMobile\Portal\Jt\JTToDo.htm
	\DeskPlusMobile\Portal\Ws\GetPortal.asmx
	\DeskPlusMobile\Portal\Ws\BE\MailInfo.cs

	\DeskPlusMobile\RM\List.aspx
	\DeskPlusMobile\RM\UsableList.aspx
	\DeskPlusMobile\RM\View.aspx
	\DeskPlusMobile\RM\Jt\JTList.htm
	\DeskPlusMobile\RM\Jt\JTMyReserveList.htm
	\DeskPlusMobile\RM\Jt\JTReserveList.htm
	
	\DeskPlusMobile\Task\FowardList.aspx
	\DeskPlusMobile\Task\List.aspx
	\DeskPlusMobile\Task\Jt\JTFowardList.htm
	\DeskPlusMobile\Task\Jt\JtList.htm

	\DeskPlusMobile\WebHard\List.aspx

: 2018-10-26 by 이중호  ----------------------------------------------------------
[디자인] 주소록, 웹폴더, 커뮤니티, 마이톡, 업무협업, 자원관리, 근태관리, 환경설정
	\DeskPlusMobile\Address\Add.aspx
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Address\View.aspx
	\DeskPlusMobile\Address\Jt\JTList.htm
	\DeskPlusMobile\Address\Ws\GetAddress.asmx.cs

	\DeskPlusMobile\Approval\AddOpinion.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\View.aspx

	\DeskPlusMobile\Attendance\Add.aspx

	\DeskPlusMobile\Common\ImageView.aspx.cs

	\DeskPlusMobile\Community\Add.aspx
	\DeskPlusMobile\Community\BoardFolder.aspx
	\DeskPlusMobile\Community\BoardList.aspx
	\DeskPlusMobile\Community\BoardView.aspx
	\DeskPlusMobile\Community\Folder.aspx
	\DeskPlusMobile\Community\Jt\JTBoardFolder.htm
	\DeskPlusMobile\Community\Jt\JTBoardList.htm
	\DeskPlusMobile\Community\Jt\JTFolder.htm

	\DeskPlusMobile\Config\List.aspx

	\DeskPlusMobile\Content\ko\Base\Css\DStyle\mobile.cs
	\DeskPlusMobile\Content\ko\Base\Img\DStyle\myimgdefault.gif

	\DeskPlusMobile\Js\Address\View.js
	\DeskPlusMobile\Js\Attendance\Add.js
	\DeskPlusMobile\Js\Community\Folder.js
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Js\MyTalk\Add.js
	\DeskPlusMobile\Js\RM\List.js
	\DeskPlusMobile\Js\Task\List.js
	\DeskPlusMobile\Js\WebHard\List.js
	
	\DeskPlusMobile\Mail\Send.aspx

	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\List.aspx
	\DeskPlusMobile\MyTalk\View.aspx
	\DeskPlusMobile\MyTalk\Jt\JtList.htm
	\DeskPlusMobile\MyTalk\Jt\JtSNSList.htm
	\DeskPlusMobile\MyTalk\Ws\GetMyTalk.asmx.cs

	\DeskPlusMobile\Pims\View.aspx
	\DeskPlusMobile\Pims\Jt\JtSchedule.htm

	\DeskPlusMobile\Portal\LogOut.aspx
	\DeskPlusMobile\Portal\NPortal.aspx
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Jt\JTApproval.htm
	\DeskPlusMobile\Portal\Jt\JTBoard.htm
	\DeskPlusMobile\Portal\Jt\JTMail.htm
	\DeskPlusMobile\Portal\Jt\JTSchedule.htm
	\DeskPlusMobile\Portal\Jt\JTToDo.htm

	\DeskPlusMobile\RM\Add.aspx
	\DeskPlusMobile\RM\Folder.aspx
	\DeskPlusMobile\RM\List.aspx
	\DeskPlusMobile\RM\RejectAdd.aspx
	\DeskPlusMobile\RM\RepeatDel.aspx
	\DeskPlusMobile\RM\ReplyAdd.aspx
	\DeskPlusMobile\RM\UsableList.aspx
	\DeskPlusMobile\RM\View.aspx
	\DeskPlusMobile\RM\Jt\JTList.htm
	\DeskPlusMobile\RM\Jt\JTMyReserveList.htm
	\DeskPlusMobile\RM\Jt\JTReserveList.htm

	\DeskPlusMobile\Task\AddAccept.aspx
	\DeskPlusMobile\Task\AddComplete.aspx
	\DeskPlusMobile\Task\AddReject.aspx
	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Task\BaseView.aspx
	\DeskPlusMobile\Task\Folder.aspx
	\DeskPlusMobile\Task\ForwardList.aspx
	\DeskPlusMobile\Task\ForwardView.aspx
	\DeskPlusMobile\Task\List.aspx
	\DeskPlusMobile\Task\View.aspx
	\DeskPlusMobile\Task\Jt\JTForwardList.htm
	\DeskPlusMobile\Task\Jt\JtList.htm

	\DeskPlusMobile\WebHard\Folder.aspx
	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\WebHard\Share.aspx
	\DeskPlusMobile\WebHard\Jt\JTList.htm
	\DeskPlusMobile\WebHard\Jt\JTShare.htm

: 2018-10-22 by 이중호  ----------------------------------------------------------
[디자인] 부트스트랩, Css, 메인, E-mail, 전자결재, 게시판, 일정관리. 주소록 메인
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Address\Jt\JTList.htm

	\DeskPlusMobile\Approval\AddOpinion.aspx
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CarboncopyList.aspx
	\DeskPlusMobile\Approval\CirDocList.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DeptList.aspx
	\DeskPlusMobile\Approval\DocList.aspx
	\DeskPlusMobile\Approval\DocOpinion.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\Folder.aspx
	\DeskPlusMobile\Approval\List.aspx
	\DeskPlusMobile\Approval\Memo.aspx
	\DeskPlusMobile\Approval\Proxy.aspx
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\Jt\JTCirDocList.htm
	\DeskPlusMobile\Approval\Jt\JTDocList.htm
	\DeskPlusMobile\Approval\Jt\JTList.htm
	\DeskPlusMobile\Approval\Jt\JTProxyList.htm

	\DeskPlusMobile\Board\Add.aspx
	\DeskPlusMobile\Board\Folder.aspx
	\DeskPlusMobile\Board\List.aspx
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\Jt\JTFolder.htm
	\DeskPlusMobile\Board\Jt\JTList.htm

	\DeskPlusMobile\Content\ko\Base\Css\DStyle
	\DeskPlusMobile\Content\ko\Base\Css\font-awesome

	\DeskPlusMobile\Html\Js\jquery.common.js

	\DeskPlusMobile\Js\Address\List.js
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocList.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DeptList.js
	\DeskPlusMobile\Js\Approval\DocList.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\List.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Js\Mail\Folder.js
	\DeskPlusMobile\Js\Mail\List.js
	\DeskPlusMobile\Js\Pims\List.js
	\DeskPlusMobile\Js\Portal\Default.js
	\DeskPlusMobile\Js\Portal\Portal.js

	\DeskPlusMobile\Mail\Folder.aspx
	\DeskPlusMobile\Mail\List.aspx
	\DeskPlusMobile\Mail\Private.aspx
	\DeskPlusMobile\Mail\Send.aspx
	\DeskPlusMobile\Mail\Trash.aspx
	\DeskPlusMobile\Mail\View.aspx
	\DeskPlusMobile\Mail\ViewTrash.aspx
	\DeskPlusMobile\Mail\Jt\JTList.htm
	\DeskPlusMobile\Mail\Jt\JTPrivate.htm
	\DeskPlusMobile\Mail\Jt\JTTrash.htm
	
	\DeskPlusMobile\Pims\Add.aspx
	\DeskPlusMobile\Pims\List.aspx
	\DeskPlusMobile\Pims\Mod.aspx
	\DeskPlusMobile\Pims\ReplyAdd.aspx
	\DeskPlusMobile\Pims\Share.aspx
	\DeskPlusMobile\Pims\View.aspx
	\DeskPlusMobile\Pims\Jt\JTSchedule.htm
	\DeskPlusMobile\Pims\Jt\JTShare.htm
	\DeskPlusMobile\Pims\Jt\JTToDo.htm

	\DeskPlusMobile\Portal\Default.aspx
	\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\Portal\NDefault.aspx
	\DeskPlusMobile\Portal\NPortal.aspx
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Jt\JTApproval.htm
	\DeskPlusMobile\Portal\Jt\JTBoard.htm
	\DeskPlusMobile\Portal\Jt\JTMail.htm
	\DeskPlusMobile\Portal\Jt\JTSchedule.htm
	\DeskPlusMobile\Portal\Jt\JTToDo.htm

: 2018-10-17 By kkzz52  ----------------------------------------------------------
[게시판] 댓글의 댓글 기능 추가
	\DeskPlusMobile\Js\Board\View.js
	\DeskPlusMobile\Js\Board\AddShortReply.js 추가
	\DeskPlusMobile\Js\Board\ModShortReply.js 추가
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\View.aspx.cs
	\DeskPlusMobile\Board\AddShortReply.aspx 추가
	\DeskPlusMobile\Board\AddShortReply.aspx.cs 추가
	\DeskPlusMobile\Board\ModShortReply.aspx 추가
	\DeskPlusMobile\Board\ModShortReply.aspx.cs 추가
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Board\Ws\BE\ShortReplyInfo.cs
	\DeskPlusMobile\Board\Ws\BE\ShortReplyGoodInfo.cs 추가
	\DeskPlusMobile\Board\Ws\Data\ShortReply.cs
	\DeskPlusMobile\Board\Ws\Data\ShortReplyGood.cs 추가
	\DeskPlusMobile\Board\Jt\JTShortReply.htm 추가

	\DeskPlusMobile\App_Data\SQLBoard.xml ( SEL_QRY_ShortReplyID 추가 )
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

	DPM_BOA_ShortReply_Sel 수정
	DPM_BOA_ShortReply_SelCnt 추가

	리소스 
	AddShortReply 추가
	DelReply 추가
	ModComment 추가
	MyComment 추가
	AlreadyComment 추가
	Recommend 추가
	Decommendation 추가

: 2018-10-10 by kkzz52 --------------------------------------------------------
[전자결재] 알람 수신자 언어로 가도록 수정
	\DeskPlusMobile\XmlData\Confirm\en.xml
	\DeskPlusMobile\App_Data\SQLApproval.xml (SEL_QRY_ACC_SelByUserID 추가)
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Approval\Ws\BE\DocInfo.cs	

: 2018-07-18 by famo23 ----------------------------------------------------------------------------
[환경설정] 모바일 메뉴에 따른 알람설정 보이는 부분 수정 
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs

: 2018-07-17 by famo23 ----------------------------------------------------------------------------
[업무협업]최신업데이트 업무 버튼 상태에 따라서 나오게 수정
	\DeskPlusMobile\Js\Task\AddComplete.js
	\DeskPlusMobile\Js\Task\AddReply.js
	\DeskPlusMobile\Js\Task\View.js
	\DeskPlusMobile\Task\Ws\SetTask.asmx.cs
	\DeskPlusMobile\Task\View.aspx
	\DeskPlusMobile\Task\View.aspx.cs

: 2018-07-11 by 이지원 ----------------------------------------------------------------------------
[게시판] 알람타입(Mobile) 추가
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs

: 2018-05-08 by famo23----------------------------------------------------------------------------
[일정관리]공유자 일정은 공유일정, 할일만 나오게 수정 
	\DeskPlusMobile\App_Data\SQLPim.xml
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs
	DPM_PIM_Schedule_SelShare
	DPM_PIM_Schedule_SelMonthShare
	DPM_PIM_ToDo_SelShare
	DPM_PIM_ToDo_SelMonthShare

[일정관리] 본인일정에서는 이름안나오게 수정 
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs

: 2018-05-04 by 이지원----------------------------------------------------------------------------
[근태관리] 기획업무 추가개발
	\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\Attendance\Add.aspx
	\DeskPlusMobile\Attendance\Add.aspx.cs
	\DeskPlusMobile\Js\Attendance\Add.js

	\DeskPlusMobile\Helper\XmlDataBaseHelper.cs
	\DeskPlusMobile\App_Data\SQLAttendance.xml
	\DeskPlusMobile\Attendance\Ws\GetAttendance.asmx.cs
	\DeskPlusMobile\Attendance\Ws\SetAttendance.asmx.cs
	\DeskPlusMobile\Attendance\Ws\Data\List.cs

	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

: 2018-05-04 by famo23----------------------------------------------------------------------------
[일정관리] 메뉴 갯수 버그수정
	\DeskPlusMobile\Portal\Menu.ascx.cs


: 2018-04-20 By 이중호  -----------------------------------------------------------------------------
[문서스트리밍]
	* 프로시저 수정
	DP_APP_DS_SelList 

	* 안드로이드 폰에서 웹폴더 업로드시에 문서스트리밍 적용
	\DeskPlusMobile\Helper\UploadAttachHandler.cs


: 2018-04-25 By famo23  -----------------------------------------------------------------------------
[App] 안드로이드 특정폰에서 app다운이 안되는 현상 
	target="_blank" 제거 
	\DeskPlusMobile\Portal\Default.aspx

: 2018-04-20 By 이중호  -----------------------------------------------------------------------------
[공통] 문서 스트리밍 적용
	* XML 데이타 파일
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\App_Data\SQLBoard.xml
	\DeskPlusMobile\App_Data\SQLCommunity.xml
	\DeskPlusMobile\App_Data\SQLPim.xml
	\DeskPlusMobile\App_Data\SQLSNS.xml
	\DeskPlusMobile\App_Data\SQLTask.xml
	\DeskPlusMobile\App_Data\SQLWebHard.xml

	* 전자결재
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs

	* 게시판
	\DeskPlusMobile\Board\View.aspx.cs
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Board\Ws\Data\AttachFile.cs

	* 공통
	\DeskPlusMobile\Common\SetDocStream.aspx

	* 커뮤니티
	\DeskPlusMobile\Community\BoardView.aspx.cs
	\DeskPlusMobile\Community\Ws\Data\BoaAttach.cs

	* 웹콤포넌트
	\DeskPlusMobile\Helper\UploadAttachHandler.cs
	\DeskPlusMobile\Helper\WebComponent\DocStream.cs

	*마이톡
	\DeskPlusMobile\MyTalk\Ws\SetMyTalk.asmx.cs

	*일정관리
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs

	*업무협업
	\DeskPlusMobile\Task\View.aspx.cs
	\DeskPlusMobile\Task\Ws\Data\Attach.cs

	*웹하드
	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\WebHard\Ws\GetWebHard.asmx.cs
	\DeskPlusMobile\WebHard\Ws\SetWebHard.asmx.cs

	* JS 파일
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Js\Board\View.js
	\DeskPlusMobile\Js\Communityl\BoardView.js
	\DeskPlusMobile\Js\Pims\View.js
	\DeskPlusMobile\Js\Task\View.js
	\DeskPlusMobile\Js\WebHard\List.js

	--프로시저
	DP_APP_DS_SelList
	DPM_BOA_DS_Sel
	DPM_CMN_DS_Sel
	DPM_TAS_DS_Sel
	DPM_WHD_DS_Sel

: 2018-04-10 By famo23  -----------------------------------------------------------------------------
[공통]ios계열 모바일기기에서 자동완성된 내역 select 할때 2번 클릭해야 하는 문제
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Js\Mail\Send.js

: 2018-02-21 By 이지원  -----------------------------------------------------------------------------
[공통] 공휴일 반영 수정
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs	게시판
	\DeskPlusMobile\Coop\Ws\GetCoop.asmx.cs		
	\DeskPlusMobile\Task\Ws\GetTask.asmx.cs		업무협업
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs		일정관리	
	\DeskPlusMobile\RM\Ws\GetRM.asmx.cs			자원관리

: 2017-07-25 by famo23 ----------------------------------------------------------------------------
[전자결재] 알람포맷이 PC버전하고 안 맞는 버그 수정
	\DeskPlusMobile\XmlData\Confirm\en.xml
	\DeskPlusMobile\XmlData\Confirm\fr.xml
	\DeskPlusMobile\XmlData\Confirm\ja.xml
	\DeskPlusMobile\XmlData\Confirm\ko.xml
	\DeskPlusMobile\XmlData\Confirm\zh-cn.xml
	\DeskPlusMobile\XmlData\Confirm\zh-tw.xml

: 2017-07-10 by famo23 ----------------------------------------------------------------------------
[업무협업] 보기페이지 스타일 수정
	\DeskPlusMobile\Js\Task\BaseView.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\View.js
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

: 2017-06-30 by 방영환 ----------------------------------------------------------------------------
[자원관리] 자원관리 - 수정사항

	* 소스추가
	\DeskPlusMobile\Js\RM\RejectAdd.js
	\DeskPlusMobile\RM\RejectAdd.aspx.cs
	\DeskPlusMobile\RM\Ws\BE\RejectInfo.cs
	\DeskPlusMobile\RM\Ws\BE\CalendarInfo.cs
	\DeskPlusMobile\RM\Ws\Data\Reject.cs

	\DeskPlusMobile\RM\Jt\JTCalendar.htm


	* 소스수정
	\DeskPlusMobile\App_Data\SQLRM.xml

	\DeskPlusMobile\Jt\JTList.htm
	\DeskPlusMobile\Jt\JTMyReserveList.htm
	\DeskPlusMobile\Jt\JTReserveList.htm

	\DeskPlusMobile\Js\RM\ReplyAdd.js
	\DeskPlusMobile\Js\RM\View.js
	\DeskPlusMobile\Js\RM\List.js

	\DeskPlusMobile\RM\Folder.aspx.cs
	\DeskPlusMobile\RM\View.aspx.cs
	\DeskPlusMobile\RM\ReplyAdd.aspx.cs

	\DeskPlusMobile\RM\Ws\GetRM.asmx.cs
	\DeskPlusMobile\RM\Ws\SetRM.asmx.cs

	\DeskPlusMobile\RM\Ws\BE\ListInfo.cs

	* 프로시저 수정
	DPM_RM_Reserve_SelMyReserve
	DPM_RM_Reserve_SelReserve
	DPM_RM_Reserve_SelUsable

	* 프로시저 추가
	DPM_RM_Reserve_SelMonth

	* 리소스 추가
	ModifyDate	신청일자
	RejectTitle	거부사유작성
	RejectStr	거부사유
	RequestQty	신청수량

	* 리소스 수정
	ResourcesAdd		Reservation (영문만)
	ResourcesReserve	Reservation list (영문만)
	ResourceManager		Resource (영문만)
	SelectResource		Resource (영문만)

: 2017-06-26 by 이지원 ----------------------------------------------------------------------------
[웹폴더] 공용폴더 추가
	--소스
	\DeskPlusMobile\WebHard\Folder.aspx
	\DeskPlusMobile\WebHard\Folder.aspx.cs
	\DeskPlusMobile\Js\WebHard\Folder.js

	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\WebHard\List.aspx.cs
	\DeskPlusMobile\Js\WebHard\List.js

	\DeskPlusMobile\App_Data\SQLWebHard.xml
	\DeskPlusMobile\WebHard\Ws\GetWebHard.asmx.cs
	\DeskPlusMobile\Helper\UploadAttachHandler.cs

	--프로시져
	SEL_PROC_Folder
	SEL_QRY_Folder_FolderID
	SEL_QRY_Folder_PublicFolderID
	SEL_PROC_PublicFolder_FolderID
	SEL_PROC_PublicFolderFile_FolderID
	SEL_PROC_PublicFolderFile_SelByUserID
	DPM_WHD_PublicFolder_SelByFID
	ufn_GetWhdFolPathSubUptree

: 2017-06-22 by maniaysw ----------------------------------------------------------------------------
[게시판] 첨부파일 외부 다운로드 불가	
	\DeskPlusMobile\Account\Data\User.cs

	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\Write.aspx.cs
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\View.aspx.cs
	\DeskPlusMobile\Account\Data\User.cs
	\DeskPlusMobile\Board\Ws\Data\ListEtc.cs
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Common\Ws\GetCommon.asmx.cs
	\DeskPlusMobile\Js\Board\View.js
	\DeskPlusMobile\Js\Board\Write.js
	\DeskPlusMobile\App_Data\SQLBoard.xml
	\DeskPlusMobile\App_Data\SQLAccount.xml

	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

: 2017-06-12 By 방영환  -----------------------------------------------------------------------------
[자원관리] 자원관리 추가 (수정중)
	* 소스수정
	\DeskPlusMobile\Portal\Menu.ascx		** 추후 자원관리 이미지 추가 해야함

	\DeskPlusMobile\Helper\XmlDataBaseHelper.cs
	\DeskPlusMobile\Helper\FileHelper.cs

	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

	* 소스추가
	\DeskPlusMobile\App_Data\SQLRM.xml
	\DeskPlusMobile\App_Data\RM.xml

	\DeskPlusMobile\Js\RM\Add.js
	\DeskPlusMobile\Js\RM\Folder.js
	\DeskPlusMobile\Js\RM\List.js
	\DeskPlusMobile\Js\RM\UsableList.js
	\DeskPlusMobile\Js\RM\Mod.js
	\DeskPlusMobile\Js\RM\ReplyAdd.js
	\DeskPlusMobile\Js\RM\View.js
	\DeskPlusMobile\Js\RM\RepeatDel.js

	\DeskPlusMobile\RM\Add.aspx.cs
	\DeskPlusMobile\RM\Folder.aspx.cs
	\DeskPlusMobile\RM\List.aspx.cs
	\DeskPlusMobile\RM\UsableList.aspx.cs	
	\DeskPlusMobile\RM\ReplyAdd.aspx.cs
	\DeskPlusMobile\RM\View.aspx.cs
	\DeskPlusMobile\RM\RepeatDel.aspx.cs

	\DeskPlusMobile\Jt\JTList.htm
	\DeskPlusMobile\Jt\JTMyReserveList.htm
	\DeskPlusMobile\Jt\JTReserveList.htm

	\DeskPlusMobile\Ws\GetRM.asmx.cs
	\DeskPlusMobile\Ws\SetRM.asmx.cs

	\DeskPlusMobile\Ws\BE\ListInfo.cs	
	\DeskPlusMobile\Ws\BE\ReserveInfo.cs	
	\DeskPlusMobile\Ws\BE\ShortReplyInfo.cs
	\DeskPlusMobile\Ws\BE\AttachInfo.cs
	\DeskPlusMobile\Ws\BE\ShareUser.cs
	\DeskPlusMobile\Ws\BE\AccountInfo.cs

	\DeskPlusMobile\Ws\BLL\Reserve.cs
	\DeskPlusMobile\Ws\BLL\ShortReply.cs
	\DeskPlusMobile\Ws\BLL\Attach.cs

	\DeskPlusMobile\Ws\Data\Reserve.cs	
	\DeskPlusMobile\Ws\Data\ReserveList.cs
	\DeskPlusMobile\Ws\Data\ShortReply.cs
	\DeskPlusMobile\Ws\Data\Attach.cs
	\DeskPlusMobile\Ws\Data\Repeat.cs
	\DeskPlusMobile\Ws\Data\ShareUser.cs

	\DeskPlusMobile\XmlData\RM\en.xml
	\DeskPlusMobile\XmlData\RM\fr.xml
	\DeskPlusMobile\XmlData\RM\ja.xml
	\DeskPlusMobile\XmlData\RM\ko.xml
	\DeskPlusMobile\XmlData\RM\zh-cn.xml
	\DeskPlusMobile\XmlData\RM\zh-tw.xml

	* 리소스 추가
	Reservation			예약
	Usable0Reserve		승인 대기
	MyReserve			나의 예약현황
	ResourcesReserve	자원 예약현황
	Usable0				미승인
	Usable1				승인
	Usable2				거부
	SecurityReservation	보안예약
	InputRM				제목,자원명,예약자
	ReserveDate			일시
	SecuritySetting		보안설정
	SelectResource		자원선택
	ResourcesAdd		자원예약
	SelectResources		자원을 선택해 주세요
	QtyZero				예약가능수량이 없습니다
	UsableResource		선택한 날짜에 이미 자원이 예약되어 있습니다
	ReserveResourceView	예약자원보기
	RepeatDel			반복일정삭제
	RepeatGubun1		이번 일정만
	RepeatGubun2		향후 모든 일정
	RepeatGubun3		모든 일정
	RepeatEx1			반복일정의 다른 모든 일정을 그대로 유지합니다
	RepeatEx2			해당 일정 및 향후 모든 일정이 삭제됩니다
	RepeatEx3			반복되는 모든 일정을 삭제합니다

	* 프로시저 추가
	DPM_RM_Reserve_SelReserve
	DPM_RM_Reserve_SelMyReserve
	DPM_RM_Reserve_SelUsableCnt
	DPM_RM_Reserve_SelUsable

	DPM_RM_Reserve_SelAlarmUser

	* 뷰 수정
	VW_RM_ReserveList	DeptCodeID 추가

	* 이미지 추가
	\DeskPlusMobile\Content\ko\Base\Img\resource.png

: 2017-05-18 By 방영환  -----------------------------------------------------------------------------
[업무일지, 업무협업] 알람발송 수정
	\DeskPlusMobile\Task\Ws\BLL\Alarm.asmx.cs
	\DeskPlusMobile\Pims\Ws\SetPims.asmx.cs
	\DeskPlusMobile\App_Data\SQLTask.xml

: 2017-05-18 By 이지원  -----------------------------------------------------------------------------
[게시판,커뮤니티] 알람발송수정
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Community\Ws\SetCommunity.asmx.cs
	\DeskPlusMobile\App_Data\SQLCommunity.xml
	\DeskPlusMobile\App_Data\SQLAccount.xml

: 2017-05-15 By famo23  -----------------------------------------------------------------------------
[공통]회사명 다국어처리
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

: 2017-04-28 by kkzz52 ----------------------------------------------------------------------------
[전자결재] 휴가원 최종결재 시 대결자 추가
	\DeskPlusMobile\Legacy\HoliDay_Complete.aspx
	\DeskPlusMobile\Approval\Ws\Proxy\Add.asmx 추가
	\DeskPlusMobile\Approval\Ws\BE\AccountInfo.cs
	\DeskPlusMobile\Approval\Ws\BE\ProxyInfo.cs
	\DeskPlusMobile\Helper\WebComponent\DateHelper.cs
	\DeskPlusMobile\XmlData\Proxy\ko~.xml 추가
	\DeskPlusMobile\App_Data\SQLApproval.xml 
	- <Item ID="SEL_UserInfo" Value="SELECT * FROM VW_UserInfo WHERE CompanyID = {2}  AND ActiveCompany = '1' AND UserID = {3} AND ActiveUser IN({0}) AND BaseDept IN({1})" Content="사용자 정보 가져오기" />
	- <Item ID="INS_PROC_Proxy" Value="DP_APP_Proxy_Ins" Content="대결자 저장"></Item>

: 2017-04-25 By famo23  -----------------------------------------------------------------------------
[공통] 다국어 관련수정
	리소스추가
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Portal.aspx.cs
	\DeskPlusMobile\Task\Folder.aspx

: 2017-04-18 by withsuki ----------------------------------------------------------------------------
[공통/전자결재] 결재알람처리 (개인화)
	\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Approval\Ws\BE\AccountInfo.cs
	\DeskPlusMobile\Helper\XmlDataBaseHelper.cs
	\DeskPlusMobile\XmlData\Confirm\

: 2017-02-24 by withsuki ----------------------------------------------------------------------------
[전자결재] 결재전용 비밀번호 사용
	\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\Ws\SetConfig.asmx.cs
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\Return.js
	\DeskPlusMobile\Js\Config\List.js
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	\DeskPlusMobile\Resource\GlobalString\

	DP_ACC_UserEnv 테이블 필드추가 : MobileIsApprovePwd
	DPM_ACC_UserEnv_Ins 수정

: 2017-02-17 by maniaysw ----------------------------------------------------------------------------
[로그인] 패스워드 틀린 횟수가 넘어가면 로그인이 되지 않도록
	\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx

: 2017-02-15 By famo23  -----------------------------------------------------------------------------
[공통] 세션으로 사용할때 웹서비스호출 문제수정
	전체 .asmx파일수정
	[WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]

: 2017-02-14 By freeman  -----------------------------------------------------------------------------
[App] Android Badge Count 표시

	* SP 추가 : DPM_APP_SelBadgeCnt
	* SP 수정 : DPM_ALARM_Common_Sel  (UserID 정보 가져 올수 있도록)

	* 소스 수정
	\DeskPlusMobileSolution\DeskPlusMobile\Js\Approval\Approve.js (badge count 변경을위한 dummy 파라미터 추가 &action=GetBadge)
	\DeskPlusMobileSolution\DeskPlusMobile\Js\Approval\Return.js (badge count 변경을위한 dummy 파라미터 추가 &action=GetBadge)
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs  (GetBadgeNum)
	\DeskPlusMobile\App_Data\SQLApproval.xml (SEL_PROC_BADGECNT)

	* DPManager 수정 
	\DeskPlusManagerSolution\GcmServer\GcmServerComponent.cs  (badgecnt 추가)

	* Android App 수정
	 - Badge.java 추가관련 PlugIn 관련 파일 수정
	 - GCMIntentService 수정 (Push 받을때 Badge 정보 변경)

	 - default.js 수정

: 2017-02-09 By 방영환  -----------------------------------------------------------------------------
[전자결재] 결재진행중 참조문서 조회 가능하도록 수정(개선업무)
	프로시저수정)
	DPM_APP_DocProcess_SelRelated
	DPM_APP_DocProcess_SelRelatedCnt
	DPM_APP_SelCnt

: 2017-02-08 By kkzz52  -----------------------------------------------------------------------------
[전자결재] 부서참조조회 개발  (개선업무)
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx

: 2017-01-31 By famo23  -----------------------------------------------------------------------------
[일정관리] 달력 빨간 날짜표시가 일정 있는 날은 표시가 안되는 부분 수정
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\Js\Pims\List.js
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs

: 2017-01-25 By 이지원  -----------------------------------------------------------------------------
[공통] 휴일 가져오기 (내부개발)
	\DeskPlusMobile\App_Data\SQLPim.xml
	\DeskPlusMobile\Pims\Ws\BE\HolidayRInfo.cs
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobile\Coop\Ws\GetCoop.asmx.cs
	\DeskPlusMobile\Task\Ws\GetTask.asmx.cs

: 2017-01-17 By famo23  -----------------------------------------------------------------------------
[전자결재] 의견추가 기능 (내부개발)
	DPM_APP_DocProcess_UpdOpinion
	DPM_APP_Doc_UpdOpinion
	리소스추가 AddOpinion
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx
	\DeskPlusMobile\Approval\AddOpinion.aspx
	\DeskPlusMobile\Approval\DocOpinion.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx.cs
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Js\Approval\AddOpinion.js
	\DeskPlusMobile\Js\Approval\DocOpinion.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js

: 2017-01-05 By famo23  (사장님 지시사항) ------------------------------------------------------------
[일정관리] 댓글 작성시 작성자한테는 알람 안 오게 수정
	\DeskPlusMobile\Pims\Ws\SetPims.asmx.cs

: 2016-12-29 By famo23 (사장님 지시사항) ------------------------------------------------------------
[마이톡] 사진 썸네일이 아닌 본이미지로 나오게 처리
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\MyTalk\Ws\GetMyTalk.asmx.cs
	\DeskPlusMobile\MyTalk\Ws\SetMyTalk.asmx.cs
		



### DeskPlusMobile v4.5.1 (2016.12.23) #########################################################

: 2016-12-22 By famo23 ------------------------------------------------------------
[업무협업] 최신순 협업 추가 (요청,수신,참조협업에서 최신글 업데이트 되는 순으로 나오는 협업)
	리소스추가 Recent RecentStr
	DPM_TAS_List_SelRecent
	DPM_TAS_List_SelRecentCnt
	DPM_TAS_List_TopDown_Recent
	VW_TAS_ListHome 뷰수정
	\DeskPlusMobile\App_Data\SQLTask.xml
	\DeskPlusMobile\Js\Task\AddAccept.js
	\DeskPlusMobile\Js\Task\AddComplete.js
	\DeskPlusMobile\Js\Task\AddReject.js
	\DeskPlusMobile\Js\Task\AddReply.js
	\DeskPlusMobile\Js\Task\Folder.js
	\DeskPlusMobile\Js\Task\List.js
	\DeskPlusMobile\Js\Task\View.js
	\DeskPlusMobile\Task\Ws\Data\List.cs
	\DeskPlusMobile\Task\Ws\GetTask.asmx.cs
	\DeskPlusMobile\Task\AddAccept.aspx
	\DeskPlusMobile\Task\AddAccept.aspx.cs
	\DeskPlusMobile\Task\AddComplete.aspx
	\DeskPlusMobile\Task\AddComplete.aspx.cs
	\DeskPlusMobile\Task\AddReject.aspx
	\DeskPlusMobile\Task\AddReject.aspx.cs
	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Task\AddReply.aspx.cs
	\DeskPlusMobile\Task\Folder.aspx
	\DeskPlusMobile\Task\List.aspx
	\DeskPlusMobile\Task\View.aspx
	\DeskPlusMobile\Task\View.aspx.cs

: 2016-12-12 By famo23 ------------------------------------------------------------
[전자결재] 반려했을 시에 DP_APP_AppDoc insert 안되는 버그수정
	DPM_APP_Seq_UpdDecide
	DPM_APP_AppDoc_Ins

: 2016-12-06 By famo23 ------------------------------------------------------------
[알람] 메일알람 멀티법인 가능하게 수정	
	DEEP_MAIL_Count 필드추가
	DEEP_MAIL_CountIOS 필드추가
	DPM_ALARM_Mail 필드추가
	DPM_ALARM_MailIOS 필드추가
	DEEP_MAIL_AlarmSchedular
	DEEP_MAIL_Count_InsUpd
	DPM_ALARM_Common_Sel
	DPM_ALARM_Common_SelByIOS

[업무협업] 문서보기에서 조회현황 체크하는 부분 버그수정
	\DeskPlusMobile\Task\View.aspx.cs

: 2016-12-05 By famo23 ------------------------------------------------------------
[전자결재] 첨언의 부서명. 직급명 관련 수정 (결재자지정부서로 나오게 수정)
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs

[전자결재] 결재취소 UI 변경
	리소스추가
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\View.js

[전자결재] 승인,전결,반려페이지에서 취소버튼 선택 -> 문서보기페이지 -> 
		   다시 승인,전결,반려할때 targetDeptID 값이 전달안되는 버그수정
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\Return.js

: 2016-12-01 By famo23 ------------------------------------------------------------
[전자결재] 전결시에 myturn 업데이트 버그수정
	DPM_APP_DocProcess_ApproveA

: 2016-11-30 By famo23 ------------------------------------------------------------
[로그아웃] 로그아웃 버튼 환결설정 메뉴로 변경
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Portal\Footer.ascx.cs

: 2016-11-09 By famo23 ------------------------------------------------------------
[메일] 메일 웹소비스 호출 파라메터 LoginID->MailID로 수정
	\DeskPlusMobile\Mail\Ws\Data\List.cs
	\DeskPlusMobile\Mail\List.aspx.cs
	\DeskPlusMobile\Mail\Trash.aspx.cs	
	\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs
	\DeskPlusMobile\Mail\Ws\SetMail.asmx.cs
	\DeskPlusMobile\Portal\Ws\GetPortal.asmx.cs
	\DeskPlusMobile\Portal\Menu.ascx.cs

: 2016-11-08 By famo23 ------------------------------------------------------------
[업무협업] 전달받은협업에서 답변쓰기 처리
	리소스추가
	\DeskPlusMobile\Js\Task\AddReply.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\List.js
	\DeskPlusMobile\Task\Jt\JtList.htm
	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Task\AddReply.aspx.cs
	\DeskPlusMobile\Task\BaseView.aspx
	\DeskPlusMobile\Task\BaseView.aspx.cs
	\DeskPlusMobile\Task\ForwardView.aspx
	\DeskPlusMobile\Task\ForwardView.aspx.cs
	\DeskPlusMobile\Task\View.aspx
	\DeskPlusMobile\Task\View.aspx.cs


: 2016-10-28 By famo23 ------------------------------------------------------------
[메일] 보낸메일함의 리스트에 받는 사람이 나오게 수정
	\DeskPlusMobile\Mail\Jt\JTList.htm
	\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs

: 2016-10-26 By famo23 ------------------------------------------------------------
[게시판,커뮤니티] 첨부파일명에 공백이 있을 경우에 다운로드가 안되는 버그수정
	\DeskPlusMobile\Community\BoardView.aspx.cs
	\DeskPlusMobile\Board\View.aspx.cs

: 2016-10-11 By famo23 ------------------------------------------------------------
[게시판] companyID 1이 아닌 경우에는 포탈게시판리스트 안 나오는 버그수정
	DPM_BOA_Category_SelCompany
	\DeskPlusMobile\Board\Ws\BLL\Category.cs
	\DeskPlusMobile\Board\Ws\Data\Category.cs

: 2016-08-30 By freeman ------------------------------------------------------------
[인사] 모바일 겸직기능
	DPM_ACC_UserDept_UpdBaseDept

	\DeskPlusMobile\App_Data\SQLAccount.xml   UPD_PROC_BASEDEPT

	\DeskPlusMobile\Js\Portal\Portal.js  Portal.SetBaseDept(), Portal.SLO()
	\DeskPlusMobile\Portal\Portal.aspx.*.*   LoadMoreJob()


	\DeskPlusMobileSolution\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs  GetLoginOutSideENC, LoginCheck
	\DeskPlusMobileSolution\DeskPlusMobile\Portal\Ws\GetPortal.asmx.cs	 SetBaseDept 

: 2016-08-25 By famo23 ------------------------------------------------------------
[전자결재] 보낸 회람함 카운트 쿼리 버그수정
	DPM_APP_CirDoc_SelSendCnt

[환경설정] 모바일 스킨설정 web에서도 작동되게 수정	
	테이블 DP_ACC_UserEnv 필드 추가 MobileSkin 
	\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\Config\Ws\SetConfig.asmx.cs
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\Helper\MobileBasePage.cs
	\DeskPlusMobile\Helper\MobileBaseService.cs
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

[환경설정] App에서만 작동되는 기능 web에서도 보이게 수정
	리소스추가
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs

: 2016-08-01 By famo23 ------------------------------------------------------------
[주소록] 체크박스 페이지 이동시에 초기화 된다는 경고처리
	리소스추가
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Js\Address\List.js

[공통] 검색조건 표시 
	리소스추가
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Js\Address\List.js
	\DeskPlusMobile\Approval\CarboncopyList.aspx
	\DeskPlusMobile\Approval\CirDocList.aspx
	\DeskPlusMobile\Approval\DeptList.aspx
	\DeskPlusMobile\Approval\DocList.aspx
	\DeskPlusMobile\Approval\List.aspx
	\DeskPlusMobile\Js\Approval\CarboncopyList.js
	\DeskPlusMobile\Js\Approval\CirDocList.js
	\DeskPlusMobile\Js\Approval\DeptList.js
	\DeskPlusMobile\Js\Approval\DocList.js
	\DeskPlusMobile\Js\Approval\List.js
	\DeskPlusMobile\Board\List.aspx
	\DeskPlusMobile\Js\Board\List.js
	\DeskPlusMobile\Community\BoardList.aspx
	\DeskPlusMobile\Js\Community\BoardList.js
	\DeskPlusMobile\Task\ForwardList.aspx
	\DeskPlusMobile\Task\List.aspx
	\DeskPlusMobile\Js\Task\ForwardList.js
	\DeskPlusMobile\Js\Task\List.js

[게시판] 등록자 검색 추가
	DPM_BOA_List_Sel
	DPM_BOA_List_SelCnt

[일정관리] 수정페이지 내용칸 조정
	\DeskPlusMobile\Pims\Mod.aspx

[게시판] 읽지 않은 게시물 카운트를 입사일 기준으로 체크하게 수정 
	DPM_BOA_List_NOREADList
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\Board\Ws\BLL\List.cs
	\DeskPlusMobile\Board\Ws\Data\List.cs
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobile\Portal\Menu.ascx.cs

: 2016-07-26 By famo23 ------------------------------------------------------------
[메일] 제목에 % 특수기호 있을때 발송 안되는 버그 수정
	\DeskPlusMobile\Mail\Ws\SetMail.asmx.cs

: 2016-07-22 By famo23 ------------------------------------------------------------
[웹폴더] 파일명 길어졌을때 줄 라인이 안 맞는 현상수정
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

: 2016-07-21 By famo23 ------------------------------------------------------------
[App] app을 삭제후에 다시 설치시에 regID 값이 변경되었는데 DB에 수정하지 못한 오류
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

: 2016-07-20 By famo23 ------------------------------------------------------------
[공통] 전체선택 전에 경고메시지 안 보이게 처리
	\DeskPlusMobile\Js\Mail\List.js
	\DeskPlusMobile\Js\Mail\Trash.js
	\DeskPlusMobile\Js\WebHard\List.js

: 2016-07-19 By famo23 ------------------------------------------------------------
[전자결재] 결재정보 부서명 길경우 디자인깨지는 부분 수정
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

[전자결재,업무협업] 보기페이지디자인 수정
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

[마이톡, 웹폴더] 파일선택수정
	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

: 2016-07-18 By famo23 ------------------------------------------------------------
[업무협업] 참조에 부서 2개 이상 지정했을 경우 알람이 오지 않는 버그수정
	\DeskPlusMobile\App_Data\SQLTask.xml

: 2016-07-12 By freeman ------------------------------------------------------------
[디자인] 스킨 기능 추가 
	
	/* DefaultSkinType 추가
	\DeskPlusMobile\App_Data\Base.xml	  

	/*  환경 수정 App은 개인 스킨 설정 기능 제공 / Web은 DefaultSkin 사용
	\DeskPlusMobile\Config\List.aspx    
	\DeskPlusMobile\Js\Config\List.js
	
	\DeskPlusMobile\Config\Ws\SetConfig.asmx.cs
	\DeskPlusMobile\Config\Ws\BE\ListInfo.cs
	
	/* SkinType Property 추가
	\DeskPlusMobile\Helper\MobileBasePage.cs

	/* mobile_main.css 를 사용하는 모든 aspx 에 <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main_<%=SkinType%>.css"/> 추가
	\DeskPlusMobile\*.aspx        

	/* 리소스
	\DeskPlusMobile\Resource\GlobalString\string.*.*   [MobileDesign]  추가

	/* DB 
	DP_MDM_List   [SkinType] 컬럼 추가

	DPM_MDM_List_Upd  수정

	/* 이미지
	\DeskPlusMobile\Content\ko\Base\Img\ColorSample\*.*
	\DeskPlusMobile\Content\ko\Base\Img\*.*    신규 아이콘으로 업데이트


: 2016-07-06 By famo23 ------------------------------------------------------------
[업무협업] 요청자가 수신자나 참조자에도 있을때 답변 알람이 중복으로 오는 부분 수정
	\DeskPlusMobile\Task\Ws\BLL\Alarm.cs

[업무협업] 메신저 알람창에 경로 수정
	\DeskPlusMobile\App_Data\Task.xml

: 2016-07-04 by famo23 -----------------------------------------------------------
[메일] 리스트 페이징 첫페이지, 마지막 페이지 체크 오류수정
	\DeskPlusMobile\Js\Mail\List.js
	\DeskPlusMobile\Js\Mail\Trash.js

: 2016-07-01 By famo23 ------------------------------------------------------------
[전자결재] 문서보기 스타일 수정
	\DeskPlusMobile\Content\ko\Base\Css\DeskPlusApprovalView.css
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js

[업무협업] 협업보기 스타일 깨지는 부분 수정
	\DeskPlusMobile\Task\BaseView.aspx.cs
	\DeskPlusMobile\Task\ForwardView.aspx.cs
	\DeskPlusMobile\Task\View.aspx.cs	

: 2016-06-27 By famo23 ------------------------------------------------------------
[공통] 보기페이지 확대 기능	
	\DeskPlusMobile\Mail\View.aspx
	\DeskPlusMobile\Mail\ViewTrash.aspx
	\DeskPlusMobile\MyTalk\View.aspx
	\DeskPlusMobile\Pims\View.aspx

: 2016-06-24 By famo23 ------------------------------------------------------------
[전자결재]문서안의 formfieldinput 스타일 수정
	\DeskPlusMobile\Content\ko\Base\Css\DeskPlusApprovalView.css

: 2016-06-21 By famo23 ------------------------------------------------------------
[게시판]권한 없는 게시판 카운트 가져오는 버그수정
	\DeskPlusMobile\Board\List.aspx.cs

[게시판]부서게시판 카운트 가져오는 버그수정
	DPM_BOA_List_SelCnt

[전자결재]회람 검색엔터값 안되는 부분 수정
	\DeskPlusMobile\Approval\CirDocList.aspx

: 2016-06-20 By famo23 ------------------------------------------------------------
[전자결재] 첨언간격이 그룹웨어랑 다르게 보이는 부분 수정
	\DeskPlusMobile\Content\ko\Base\Css\DeskPlusApprovalView.css
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\View.aspx

[공통] 첨부파일 열기할때 로딩이미지 처리 (iosApp일 경우만)
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Js\Board\View.js   
	\DeskPlusMobile\Js\Community\BoardView.js
	\DeskPlusMobile\Js\Mail\View.js   
	\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\Js\Pims\View.js    
	\DeskPlusMobile\Js\Task\BaseView.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\View.js   
	\DeskPlusMobile\Js\WebHard\List.js 
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\View.aspx 
	\DeskPlusMobile\Board\View.aspx    
	\DeskPlusMobile\Community\BoardView.aspx
	\DeskPlusMobile\Mail\View.aspx
	\DeskPlusMobile\Mail\ViewTrash.aspx
	\DeskPlusMobile\Pims\View.aspx
	\DeskPlusMobile\Task\BaseView.aspx
	\DeskPlusMobile\Task\ForwardView.aspx
	\DeskPlusMobile\Task\View.aspx
	\DeskPlusMobile\WebHard\List.aspx  
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

: 2016-06-14 by famo23 -----------------------------------------------------------
[전자결재] 검색에러 수정
	DPM_APP_CirDoc_SelRecv
	DPM_APP_CirDoc_SelRecvCnt	
	DPM_APP_CirDoc_SelSendCnt
	DPM_APP_Doc_SelForward
	DPM_APP_Doc_SelForwardCnt
	DPM_APP_Doc_SelCarboncopy
	DPM_APP_Doc_SelCarboncopyCnt	

[주소록] ios App에서 전화걸기 인식 안되는 문제 처리
	\DeskPlusMobile\Address\View.aspx

: 2016-06-13 by famo23 -----------------------------------------------------------
[공통] + 특수기호 처리로 escape - > encodeURIComponent 변환
   .js 파일수정

[App] 로그인패스워드 + & 기호 처리
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

[커뮤니티] 커뮤니티명에 특수문자 변환안되는 버그
	\DeskPlusMobile\Community\Ws\GetCommunity.asmx.cs

[전자결재] 첨부파일 / 근거문서 구분선 처리
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\View.aspx

[전자결재] 문서함리스트에서 날짜표시 iosApp에서 스타일 적용 안되는 문제 수정
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Approval\Jt\JTCirDocList.htm
	\DeskPlusMobile\Approval\Jt\JTDocList.htm

[공통] 메뉴아이콘 가운데 정렬
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\Portal\Menu.ascx

[공통] 폰트 사이즈 한단계 크게 / 버튼 사이즈 크게 / 검색박스 크게
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	각 페이지 aspx

[공통] 첨부파일 mht 파일 열기 수정
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\DeskPlusMobile\Js\Board\View.js   
	\DeskPlusMobile\DeskPlusMobile\Js\Community\BoardView.js	
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\DeskPlusMobile\Js\Pims\View.js    
	\DeskPlusMobile\DeskPlusMobile\Js\Task\BaseView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Task\View.js 
	\DeskPlusMobile\DeskPlusMobile\Js\WebHard\List.js

[공통] 첨부파일명 줄바꿈처리
	\DeskPlusMobile\Board\View.aspx.cs
	\DeskPlusMobile\Community\BoardView.aspx.cs
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\View.js	
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\DeskPlusMobile\Js\Pims\View.js

[공통] 첨부파일 크기 콤마표시 
	\DeskPlusMobile\Html\Js\jquery.common.js
	\DeskPlusMobile\Board\View.aspx.cs
	\DeskPlusMobile\Community\BoardView.aspx.cs
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\DeskPlusMobile\Js\Approval\View.js	
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\DeskPlusMobile\Js\Pims\View.js

[공통] 첨부파일 간격 조정
	\DeskPlusMobile\Board\View.aspx.cs
	\DeskPlusMobile\Community\BoardView.aspx.cs	
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\DeskPlusMobile\Js\Pims\View.js

[공통] 내용보기 왼쪽 공백 주기 
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css	

[공통] PC버전 안 보이게 처리
	\DeskPlusMobile\Portal\Footer.ascx.cs

[주소록] 리스트 정렬 틀어지는 문제
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css	

[주소록] 검색박스사이즈 조정
	\DeskPlusMobile\Address\List.aspx

[주소록] 탭높이 조정
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

[주소록] 상세보기 디자인수정
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css	
	\DeskPlusMobile\Address\View.aspx

[주소록] 탭이동시 회사색상이 먼저 나오는 부분 수정
	\DeskPlusMobile\Address\List.aspx

[주소록] 문자보내기 처리
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css	
	\DeskPlusMobile\Address\View.aspx
	\DeskPlusMobile\Js\Address\View.js
	리소스추가

[주소록] 공용주소록 상세보기 안 나온 버그 수정
	\DeskPlusMobile\App_Data\SQLAddress.xml

[메일] 주소록 검색, 바로가기, 탭이동 시에 이전 내용 안 가져오는 버그
	\DeskPlusMobile\Js\Address\List.js

: 2016-03-29 by freeman -----------------------------------------------------------
[배포] 배포 버전 관리 
	\DeskPlusMobile\Update\*.*
	\DeskPlusMobileSolution\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs [GetVersion]

: 2016-03-10 by withsuki -----------------------------------------------------------
[전자결재] 반려버그 수정
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Js\Approval\Return.js
	DPM_APP_DocProcess_Return

: 2016-01-12 by famo23 -----------------------------------------------------------
[포탈] 메뉴권한에 따른 리스트 체크부분 버그수정
	\DeskPlusMobile\Js\Portal\Portal.js

: 2015-12-11 by famo23 -----------------------------------------------------------
[공통] 첨부파일명에 & 처리수정
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Js\Board\View.js
	\DeskPlusMobile\Js\Community\BoardView.js
	\DeskPlusMobile\Js\Coop\BaseView.js
	\DeskPlusMobile\Js\Coop\ForwardView.js
	\DeskPlusMobile\Js\Coop\View.js  
	\DeskPlusMobile\Js\Mail\View.js   
	\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\Js\Pims\View.js
	\DeskPlusMobile\Js\Task\BaseView.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\View.js   
	\DeskPlusMobile\Js\WebHard\List.js

[메일] 자동완성에 띄워쓰기 인식수정
	\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs

: 2015-12-01 by famo23 -----------------------------------------------------------
[주소록] 겸직부서 나오게 수정
	\DeskPlusMobile\Address\Ws\GetAddress.asmx.cs
	\DeskPlusMobile\Address\View.aspx
	\DeskPlusMobile\App_Data\SQLAddress.xml
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\Js\Address\View.js

[주소록] 글씨 클릭시에 전화걸기, 메일 발송되게 처리
	\DeskPlusMobile\Address\View.aspx
	\DeskPlusMobile\Js\Address\View.js

[공통] 파라미터에 & 값 처리
	\DeskPlusMobile\Helper\MobileBaseControl.cs
	\DeskPlusMobile\Helper\MobileBasePage.cs
	\DeskPlusMobile\Helper\MobileBaseService.cs
	\DeskPlusMobile\Html\Js\jquery.common.js
	\DeskPlusMobile\Js\Address\Add.js     
	\DeskPlusMobile\Js\Address\List.js
	\DeskPlusMobile\Js\Address\View.js     
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\CarboncopyList.js
	\DeskPlusMobile\Js\Approval\CirDocList.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DeptList.js
	\DeskPlusMobile\Js\Approval\DocList.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\List.js
	\DeskPlusMobile\Js\Approval\Memo.js
	\DeskPlusMobile\Js\Approval\Return.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Js\Board\Add.js 
	\DeskPlusMobile\Js\Board\List.js
	\DeskPlusMobile\Js\Board\View.js 
	\DeskPlusMobile\Js\Board\Write.js
	\DeskPlusMobile\Js\Community\Add.js
	\DeskPlusMobile\Js\Community\BoardList.js
	\DeskPlusMobile\Js\Community\BoardView.js
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Js\Portal\Portal.js
	\DeskPlusMobile\Js\Task\AddAccept.js
	\DeskPlusMobile\Js\Task\AddComplete.js
	\DeskPlusMobile\Js\Task\AddReject.js
	\DeskPlusMobile\Js\Task\AddReply.js
	\DeskPlusMobile\Js\Task\BaseView.js
	\DeskPlusMobile\Js\Task\ForwardList.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\List.js
	\DeskPlusMobile\Js\Task\View.js

: 2015-11-17 By freeman ------------------------------------------------------------
[포탈/메뉴] 안드로이드 App에서 wake up 상태일때 포탈 화면으로 이동, 포탈정보 다시 가져오기
	\DeskPlusMobile\Portal\Menu.ascx


### DeskPlusMobile v4.4.0 (2015.xx.xx) #########################################################

: 2015-12-16 by famo23 -----------------------------------------------------------
[메일] App에서 첨부파일명 알기 위해서 수정
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js

: 2015-11-26 by famo23 -----------------------------------------------------------
[게시판] 글쓰기 특수기호 변환처리 안한 버그 수정
	\DeskPlusMobile\Board\Ws\Data\List.cs
	\DeskPlusMobile\Board\Ws\Data\ListEtc.cs
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs

[공통] 특수기호 검색 수정
	\DeskPlusMobile\Address\Ws\Data\List.cs
	\DeskPlusMobile\Address\Ws\GetAddress.asmx.cs
	DPM_APP_CirDoc_SelRecv
	DPM_APP_CirDoc_SelRecvCnt
	DPM_APP_CirDoc_SelSend
	DPM_APP_CirDoc_SelSendCnt
	DPM_APP_CirDoc_TopDown_Recv
	DPM_APP_CirDoc_TopDown_Send
	DPM_APP_Doc_SelApproval
	DPM_APP_Doc_SelApprovalCnt
	DPM_APP_Doc_SelArbitrary
	DPM_APP_Doc_SelArbitraryCnt
	DPM_APP_Doc_SelCarboncopy
	DPM_APP_Doc_SelCarboncopyCnt
	DPM_APP_Doc_SelForward
	DPM_APP_Doc_SelForwardCnt
	DPM_APP_Doc_SelPrivate
	DPM_APP_Doc_SelPrivateCnt
	DPM_APP_Doc_SelPublic
	DPM_APP_Doc_SelPublicCnt
	DPM_APP_Doc_SelReject
	DPM_APP_Doc_SelRejectCnt
	DPM_APP_Doc_TopDown_Approval
	DPM_APP_Doc_TopDown_Arbitrary
	DPM_APP_Doc_TopDown_Carboncopy
	DPM_APP_Doc_TopDown_Forward
	DPM_APP_Doc_TopDown_Private
	DPM_APP_Doc_TopDown_Public
	DPM_APP_Doc_TopDown_Reject
	DPM_APP_DocProcess_SelApprove
	DPM_APP_DocProcess_SelApproveCnt
	DPM_APP_DocProcess_SelRelated
	DPM_APP_DocProcess_SelRelatedCnt
	DPM_APP_DocProcess_SelReport
	DPM_APP_DocProcess_SelReportCnt
	DPM_APP_List_TopDown_Approve
	DPM_APP_List_TopDown_Related
	DPM_APP_List_TopDown_Report
	DPM_BOA_List_Sel
	DPM_BOA_List_SelCnt
	DPM_BOA_List_TopDown
	DPM_CMN_Boa_Sel
	DPM_CMN_Boa_SelCnt
	DPM_CMN_BOA_List_TopDown
	DPM_TAS_List_SelForward
	DPM_TAS_List_SelForwardCnt
	DPM_TAS_List_SelReceive
	DPM_TAS_List_SelReceiveCnt
	DPM_TAS_List_SelReference
	DPM_TAS_List_SelReferenceCnt
	DPM_TAS_List_SelRequest
	DPM_TAS_List_SelRequestCnt
	DPM_TAS_List_TopDown_Forward
	DPM_TAS_List_TopDown_Receive
	DPM_TAS_List_TopDown_Reference
	DPM_TAS_List_TopDown_Request

: 2015-11-25 by famo23 -----------------------------------------------------------
[게시판] 리홈 소스 잘못 가져온 부분 수정
	\DeskPlusMobile\Board\List.aspx
	\DeskPlusMobile\Board\List.aspx.cs

[게시판] 포탈화면에서 페이징 수정
	\DeskPlusMobile\Js\Portal\Portal.js
	\DeskPlusMobile\Portal\Ws\GetPortal.asmx.cs

: 2015-11-24 by famo23 -----------------------------------------------------------
[메일쓰기] 참조, 숨은참조 입력박스 사이즈 조정
	\DeskPlusMobile\Js\Mail\Send.js

: 2015-11-23 by famo23 -----------------------------------------------------------
[App] 안드로이드에서 regID 값이 없는 경우 처리 (알람을 못 받는다는 메세지 포탈에 보이기 / regID 값이 다시 생기면 DB에 저장하는 부분등)
	리소스
	DPM_MDM_List_UpdByRegID
	\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\Helper\MobileBaseControl.cs
	\DeskPlusMobile\Helper\MobileBasePage.cs
	\DeskPlusMobile\Helper\MobileBaseService.cs
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\Portal\Portal.aspx.cs

: 2015-11-19 By ellepooky ------------------------------------------------------------
[메일] 받은메일함이 아닌 다른 메일함에서 전달시 오류
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Mail\Ws\SetMail.asmx.cs


: 2015-11-16 By famo23 ------------------------------------------------------------
[메일] 페이징 숫자 수정
	\DeskPlusMobile\DeskPlusMobile\Mail\List.aspx.cs
	\DeskPlusMobile\DeskPlusMobile\Mail\Trash.aspx.cs

[마이톡] 카운트수정
	\DeskPlusMobile\DeskPlusMobile\Portal\Menu.ascx.cs

: 2015-11-13 By famo23 ------------------------------------------------------------
[게시판,마이톡] 안드로이드 App 첨부파일 관련 도메인정보 수정
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\Write.aspx.cs
	\DeskPlusMobile\Js\Board\Write.js
	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\Add.aspx.cs
	\DeskPlusMobile\Js\MyTalk\Add.js

: 2015-11-10 By famo23 ------------------------------------------------------------
[마이톡] 안드로이드 App 첨부파일 업로드 지원
	DPM_SNS_MyTalk 테이블 추가
	DPM_SNS_MyTalk_Ins
	\DeskPlusMobile\App_Data\SQLSNS.xml
	\DeskPlusMobile\Js\MyTalk\Add.js
	\DeskPlusMobile\MyTalk\Ws\BE\MyTalkInfo.cs
	\DeskPlusMobile\MyTalk\Ws\Data\List.cs
	\DeskPlusMobile\MyTalk\Ws\SetMyTalk.asmx.cs
	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\Add.aspx.cs

: 2015-11-09 By famo23 ------------------------------------------------------------
[게시판] 안드로이드 App 첨부파일 업로드 지원
	DPM_BOA_List 테이블 추가
	DPM_BOA_List_Ins
	\DeskPlusMobile\App_Data\SQLBoard.xml
	\DeskPlusMobile\Board\Ws\BE\ListInfo.cs
	\DeskPlusMobile\Board\Ws\Data\List.cs
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\Write.aspx.cs
	\DeskPlusMobile\Js\Board\Write.js

: 2015-11-06 By famo23 ------------------------------------------------------------
[앱로그인] 중국에서 구글API를 막아 두어서 regID 값을 못 받아도 로그인 되게 처리 (해당사용자는 알람을 받지 못함)
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	DPM_ALARM_Common_Sel
	DPM_ALARM_Common_SelByIOS

: 2015-11-05 By famo23 ------------------------------------------------------------
[공통] PC버전 안드로이드앱에서 안 보이게 처리
	\DeskPlusMobile\Portal\Footer.ascx
	\DeskPlusMobile\Portal\Footer.ascx.cs
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

[웹폴더] 파일등록 수정 
	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\WebHard\List.aspx.cs
	\DeskPlusMobile\Js\WebHard\List.js
	\DeskPlusMobile\Helper\UploadAttachHandler.cs

[앱로그인] 로그인 할때 패스워드 특수기호 변환처리  (& 기호 §변환해서 호출하기때문에 재변환 (파라미터구분으로 인식해서 처리))
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

: 2015-11-02 By shimpark ------------------------------------------------------------
[통합게시판] 보기의 이전, 다음 bug fix

	\DeskPlusMobile\Board\View.aspx.cs

: 2015-11-02 By famo23 ------------------------------------------------------------
[게시판 - 리스트 UI변경]
	DPM_BOA_List_Sel
	DPM_BOA_List_SelCnt
	\DeskPlusMobile\App_Data\SQLBoard.xml
	\DeskPlusMobile\Board\Ws\Data\List.cs
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobile\Board\Add.aspx
	\DeskPlusMobile\Board\Add.aspx.cs
	\DeskPlusMobile\Board\List.aspx
	\DeskPlusMobile\Board\List.aspx.cs
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\View.aspx.cs
	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\Write.aspx.cs
	\DeskPlusMobile\Js\Board\Add.js
	\DeskPlusMobile\Js\Board\List.js
	\DeskPlusMobile\Js\Board\View.js
	\DeskPlusMobile\Js\Board\Write.js

[게시판] DB Lock 처리 & 정렬수정
	\DeskPlusMobile\App_Data\SQLBoard.xml
	DPM_BOA_Attach_Sel
	DPM_BOA_Auth_Sel
	DPM_BOA_Category_Sel
	DPM_BOA_Category_SelByCID
	DPM_BOA_Category_SelByPCID
	DPM_BOA_List_NOREADList
	DPM_BOA_List_SelByCIDLID
	DPM_BOA_List_Sel
	DPM_BOA_List_Total
	DPM_BOA_ShortReply_Sel

: 2015-10-30 By famo23 ------------------------------------------------------------
[게시판] 검색할때 페이지 재호출
	\DeskPlusMobile\Js\Board\List.js

[공통] 이전글, 다음글 경고처리 수정
	리소스 추가 NoList
	\DeskPlusMobile\Mail\View.aspx
	\DeskPlusMobile\Mail\ViewTrash.aspx
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Community\BoardView.aspx
	\DeskPlusMobile\Js\Community\BoardView.js
	\DeskPlusMobile\Task\ForwardView.aspx
	\DeskPlusMobile\Task\View.aspx
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\View.js
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Js\Board\View.js

: 2015-10-29 By famo23 ------------------------------------------------------------
[전자결재]본문 가져오기 DB Access 이중 줄이기
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\View.js	

[공통] 정렬 수정
	\DeskPlusMobile\App_Data\SQLAddress.xml
	DPM_APP_DocProcess_SelReport
	DPM_APP_DocProcess_SelRelated
	DPM_APP_DocProcess_SelApprove
	DPM_WHD_File_Sel
	DPM_CMN_Boa_Sel
	DPM_SNS_MyTalk_Sel

[공통] db WITH(NOLOCK) 처리
	\DeskPlusMobile\App_Data\SQLAccount.xml
	\DeskPlusMobile\App_Data\SQLAddress.xml
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\App_Data\SQLCommunity.xml	
	\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobile\App_Data\SQLPim.xml
	\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\App_Data\SQLSNS.xml
	\DeskPlusMobile\App_Data\SQLTask.xml
	\DeskPlusMobile\App_Data\SQLWebHard.xml
	DPM_ACC_Dept_SelByDeptUpDown
	DPM_ACC_Dept_SelDeptTree
	DPM_ACC_Dept_SelDeptUser
	DPM_ACC_User_Sel
	DPM_APP_CounterPartUser_UpdDecideZQ
	DPM_APP_DocProcess_ApproveA
	DPM_APP_DocProcess_ApproveS
	DPM_APP_DocProcess_Cancel
	DPM_APP_DocProcess_SelApprove
	DPM_APP_DocProcess_SelApproveByUserID
	DPM_APP_DocProcess_SelApproveCnt
	DPM_APP_DocProcess_SelByDocNumber
	DPM_APP_DocProcess_SelRelated
	DPM_APP_DocProcess_SelRelatedCnt
	DPM_APP_List_TopDown_Approve
	DPM_APP_List_TopDown_Related
	DPM_APP_SelCnt
	DPM_APP_SelMenuCnt
	DPM_APP_Seq_SelZAUserCnt
	DPM_APP_Seq_UpdDecide
	DPM_APP_Seq_UpdViewing
	DPM_CMN_BOA_List_TopDown
	DPM_CMN_Boa_Sel
	DPM_CMN_Boa_SelCnt
	DPM_CMN_BoaShortReplyEtc_Ins
	DPM_CMN_UserVisitEtc_Ins
	DPM_COM_SYSCode_SEL
	DPM_EIP_Role_Sel
	DPM_EIP_SSO_Sel
	DPM_MAIL_User_Sel
	DPM_MAIL_User_SelByNameDeep
	DPM_MAIL_User_SelDeep
	DPM_PIM_Schedule_Ins
	DPM_PIM_Schedule_Sel
	DPM_PIM_Schedule_SelByType0
	DPM_PIM_Schedule_SelByType1
	DPM_PIM_Schedule_SelByType2
	DPM_PIM_Schedule_SelByType3
	DPM_PIM_Schedule_SelMonth
	DPM_PIM_Schedule_SelMonthByType0
	DPM_PIM_Schedule_SelMonthByType1
	DPM_PIM_Schedule_SelMonthByType2
	DPM_PIM_Schedule_SelMonthByType3
	DPM_PIM_Schedule_Upd
	DPM_PIM_ShortReply_Sel
	DPM_PIM_ToDo_Sel
	DPM_PIM_ToDo_SelByType0
	DPM_PIM_ToDo_SelByType1
	DPM_PIM_ToDo_SelByType2
	DPM_PIM_ToDo_SelByType3
	DPM_PIM_ToDo_SelMonth
	DPM_PIM_ToDo_SelMonthByType0
	DPM_PIM_ToDo_SelMonthByType1
	DPM_PIM_ToDo_SelMonthByType2
	DPM_PIM_ToDo_SelMonthByType3
	DPM_SNS_MyTalk_Sel
	DPM_SNS_MyTalk_SelCnt
	DPM_SNS_MyTalk_SelTodayCnt
	DPM_TAS_List_InsCancel
	DPM_TAS_List_InsComplete
	DPM_TAS_List_InsReject
	DPM_TAS_SelCnt
	DPM_TAS_SelMenuCnt
	DPM_WHD_File_Sel
	DPM_WHD_File_SelByFID
	DPM_WHD_File_SelCnt
	DPM_WHD_Folder_Sel
	DPM_WHD_Folder_SelByFID
	DPM_WHD_ShareFolder_Sel
	DPM_WHD_ShareFolder_SelByFID
	
[마이톡] 내용없음 리스트 디자인 변경
	\DeskPlusMobile\MyTalk\Jt\JtSNSList.htm

[메일, 웹폴더] 리스트 디자인변경
	\DeskPlusMobile\Mail\Jt\JTList.htm
	\DeskPlusMobile\Mail\Jt\JTTrash.htm
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

[커뮤니티] 댓글 버튼 디자인 수정
	\DeskPlusMobile\Community\BoardView.aspx

[메일 - 앞뒤내용 가져오기 다른 모듈하고 순차 같게 처리]
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js

[메일 - 앞뒤내용 가져오기 할때 메일 읽은 표시 못하는 버그 수정]
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js

[메일 - 앞뒤내용가져오기, 삭제, 복원 페이지재호출로 변경]
	\DeskPlusMobile\Js\Mail\List.js
	\DeskPlusMobile\Js\Mail\Trash.js
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js

[메일 - 리스트 UI변경]
	\DeskPlusMobile\Js\Mail\List.js
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Js\Mail\Trash.js
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\Mail\Ws\Data\List.cs
	\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs
	\DeskPlusMobile\Mail\List.aspx
	\DeskPlusMobile\Mail\List.aspx.cs
	\DeskPlusMobile\Mail\Send.aspx
	\DeskPlusMobile\Mail\Send.aspx.cs
	\DeskPlusMobile\Mail\Trash.aspx
	\DeskPlusMobile\Mail\Trash.aspx.cs
	\DeskPlusMobile\Mail\View.aspx
	\DeskPlusMobile\Mail\View.aspx.cs
	\DeskPlusMobile\Mail\ViewTrash.aspx
	\DeskPlusMobile\Mail\ViewTrash.aspx.cs

: 2015-10-28 By famo23 ------------------------------------------------------------
[업무협업 - 검색 페이지재호출로 변경]
	\DeskPlusMobile\Js\Task\ForwardList.js
	\DeskPlusMobile\Js\Task\List.js

[업무협업 - 리스트UI변경]
	DPM_TAS_List_SelRequestCnt
	DPM_TAS_List_SelReceiveCnt
	DPM_TAS_List_SelReferenceCnt
	DPM_TAS_List_SelForwardCnt
	\DeskPlusMobile\App_Data\SQLTask.xml
	\DeskPlusMobile\Task\Ws\Data\List.cs
	\DeskPlusMobile\Js\Task\AddAccept.js
	\DeskPlusMobile\Js\Task\AddComplete.js
	\DeskPlusMobile\Js\Task\AddReject.js
	\DeskPlusMobile\Js\Task\AddReply.js
	\DeskPlusMobile\Js\Task\BaseView.js
	\DeskPlusMobile\Js\Task\ForwardList.js
	\DeskPlusMobile\Js\Task\ForwardView.js
	\DeskPlusMobile\Js\Task\List.js
	\DeskPlusMobile\Js\Task\View.js
	\DeskPlusMobile\Task\Ws\Data\List.cs
	\DeskPlusMobile\Task\AddAccept.aspx
	\DeskPlusMobile\Task\AddAccept.aspx.cs
	\DeskPlusMobile\Task\AddComplete.aspx
	\DeskPlusMobile\Task\AddComplete.aspx.cs
	\DeskPlusMobile\Task\AddReject.aspx
	\DeskPlusMobile\Task\AddReject.aspx.cs
	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Task\AddReply.aspx.cs
	\DeskPlusMobile\Task\BaseView.aspx
	\DeskPlusMobile\Task\BaseView.aspx.cs
	\DeskPlusMobile\Task\ForwardList.aspx
	\DeskPlusMobile\Task\ForwardList.aspx.cs
	\DeskPlusMobile\Task\ForwardView.aspx
	\DeskPlusMobile\Task\ForwardView.aspx.cs
	\DeskPlusMobile\Task\List.aspx
	\DeskPlusMobile\Task\List.aspx.cs
	\DeskPlusMobile\Task\View.aspx
	\DeskPlusMobile\Task\View.aspx.cs

[마이톡 - 리스트UI변경]
	DPM_SNS_MyTalk_SelCnt
	\DeskPlusMobile\App_Data\SQLSNS.xml
	\DeskPlusMobile\MyTalk\Ws\Data\List.cs
	\DeskPlusMobile\MyTalk\Ws\GetMyTalk.asmx.cs
	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\Add.aspx.cs
	\DeskPlusMobile\MyTalk\List.aspx
	\DeskPlusMobile\MyTalk\List.aspx.cs
	\DeskPlusMobile\MyTalk\View.aspx
	\DeskPlusMobile\MyTalk\View.aspx.cs
	\DeskPlusMobile\Js\MyTalk\Add.js
	\DeskPlusMobile\Js\MyTalk\List.js
	\DeskPlusMobile\Js\MyTalk\View.js

[커뮤니티 - 앞뒤내용가져오기, 검색 페이지재호출로 변경]
	\DeskPlusMobile\Js\Community\BoardList.js
	\DeskPlusMobile\Js\Community\BoardView.js

[커뮤니티 - 리스트UI변경]
	SQL_PROC_BOA_LISTCNT
	\DeskPlusMobile\App_Data\SQLCommunity.xml
	\DeskPlusMobile\Community\Jt\JTBoardList.htm
	\DeskPlusMobile\Community\Ws\Data\List.cs
	\DeskPlusMobile\Community\Ws\GetCommunity.asmx.cs
	\DeskPlusMobile\Community\Add.aspx
	\DeskPlusMobile\Community\Add.aspx.cs
	\DeskPlusMobile\Community\BoardList.aspx
	\DeskPlusMobile\Community\BoardList.aspx.cs
	\DeskPlusMobile\Community\BoardView.aspx
	\DeskPlusMobile\Community\BoardView.aspx.cs
	\DeskPlusMobile\Js\Community\Add.js
	\DeskPlusMobile\Js\Community\BoardList.js
	\DeskPlusMobile\Js\Community\BoardView.js

[웹폴더 - 삭제 페이지재호출로 변경]
	\DeskPlusMobile\Js\WebHard\List.js

[웹폴더 - 리스트UI변경]
	SEL_PROC_FileCnt
	\DeskPlusMobile\App_Data\SQLWebHard.xml
	\DeskPlusMobile\Js\WebHard\List.js	
	\DeskPlusMobile\WebHard\Ws\DAL\List.cs
	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\WebHard\List.aspx.cs
	
[주소록 - 검색 페이지재호출로 변경]
	\DeskPlusMobile\Js\Address\List.js

[주소록 - 리스트UI변경]
	\DeskPlusMobile\Address\Ws\Data\List.cs
	\DeskPlusMobile\Address\Add.aspx
	\DeskPlusMobile\Address\Add.aspx.cs
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Address\List.aspx.cs
	\DeskPlusMobile\Address\View.aspx
	\DeskPlusMobile\Address\View.aspx.cs
	\DeskPlusMobile\App_Data\SQLAddress.xml
	\DeskPlusMobile\Js\Address\Add.js
	\DeskPlusMobile\Js\Address\List.js
	\DeskPlusMobile\Js\Address\View.js

[전자결재 - 앞뒤내용가져오기, 검색 페이지재호출로 변경]
	\DeskPlusMobile\Js\Approval\CarboncopyList.js
	\DeskPlusMobile\Js\Approval\CirDocList.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DeptList.js
	\DeskPlusMobile\Js\Approval\DocList.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\List.js
	\DeskPlusMobile\Js\Approval\View.js

[전자결재 - 리스트UI변경]
	리소스추가
	DPM_APP_DocProcess_SelReportCnt
	DPM_APP_DocProcess_SelApproveCnt
	DPM_APP_DocProcess_SelRelatedCnt
	DPM_APP_Doc_SelPublicCnt
	DPM_APP_Doc_SelPrivateCnt
	DPM_APP_Doc_SelApprovalCnt
	DPM_APP_Doc_SelRejectCnt
	DPM_APP_Doc_SelArbitraryCnt
	DPM_APP_Doc_SelCarboncopyCnt
	DPM_APP_Doc_SelForwardCnt
	DPM_APP_CirDoc_SelRecvCnt
	DPM_APP_CirDoc_SelSendCnt
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Ws\Data\List.cs
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\Approve.aspx.cs
	\DeskPlusMobile\Approval\CarboncopyList.aspx
	\DeskPlusMobile\Approval\CarboncopyList.aspx.cs
	\DeskPlusMobile\Approval\CirDocList.aspx
	\DeskPlusMobile\Approval\CirDocList.aspx.cs
	\DeskPlusMobile\Approval\CirDocView.aspx
	\DeskPlusMobile\Approval\CirDocView.aspx.cs
	\DeskPlusMobile\Approval\DeptList.aspx
	\DeskPlusMobile\Approval\DeptList.aspx.cs
	\DeskPlusMobile\Approval\DocList.aspx
	\DeskPlusMobile\Approval\DocList.aspx.cs
	\DeskPlusMobile\Approval\DocView.aspx
	\DeskPlusMobile\Approval\DocView.aspx.cs
	\DeskPlusMobile\Approval\List.aspx
	\DeskPlusMobile\Approval\List.aspx.cs
	\DeskPlusMobile\Approval\Memo.aspx
	\DeskPlusMobile\Approval\Memo.aspx.cs
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Approval\Return.aspx.cs
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\CarboncopyList.js
	\DeskPlusMobile\Js\Approval\CirDocList.js
	\DeskPlusMobile\Js\Approval\CirDocView.js
	\DeskPlusMobile\Js\Approval\DeptList.js
	\DeskPlusMobile\Js\Approval\DocList.js
	\DeskPlusMobile\Js\Approval\DocView.js
	\DeskPlusMobile\Js\Approval\List.js
	\DeskPlusMobile\Js\Approval\Memo.js
	\DeskPlusMobile\Js\Approval\Return.js
	\DeskPlusMobile\Js\Approval\View.js	

: 2015-10-28 by withsuki -----------------------------------------------------------
[전자결재] 
	- 메일알람 참조의 경우, 묶음 처리 (기존의 경우, eml파일이, 수신인 만큼 생성됨. 참조의 경우, 하나의 eml파일로 처리)
	- 알람 발송 시, DB Access수를 줄임. (기존의 경우, 80명에게 메일발송 시, 160번의 DB Access되는 로직있었음. 해당 부분 제거)
	
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Approval\Ws\BE\AccountInfo.cs


: 2015-10-26 By famo23 ------------------------------------------------------------
[전자결재] 합의, 합의불가 표시변경
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs

[전자결재] 이전글, 다음글에서 AccountTag 값 안 가져오는 버그수정
	\DeskPlusMobile\Js\Approval\View.js
	DPM_APP_List_TopDown_Approve
	DPM_APP_List_TopDown_Related
	DPM_APP_List_TopDown_Report
	
[전자결재] 문서함개발
	리소스추가
	DPM_APP_Doc_SelPublic
	DPM_APP_Doc_SelPrivate
	DPM_APP_Doc_SelApproval
	DPM_APP_Doc_SelReject
	DPM_APP_Doc_SelArbitrary
	DPM_APP_Doc_SelCarboncopy
	DPM_APP_Doc_SelForward
	DPM_APP_CirDoc_SelRecv
	DPM_APP_CirDoc_SelSend
	DPM_APP_Doc_TopDown_Public
	DPM_APP_Doc_TopDown_Private
	DPM_APP_Doc_TopDown_Approval
	DPM_APP_Doc_TopDown_Reject
	DPM_APP_Doc_TopDown_Arbitrary
	DPM_APP_Doc_TopDown_Carboncopy
	DPM_APP_Doc_TopDown_Forward
	DPM_APP_CirDoc_TopDown_Recv
	DPM_APP_CirDoc_TopDown_Send	
	\DeskPlusMobile\App_Data\SQLAccount.xml
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Jt\JTCirDocList.htm
	\DeskPlusMobile\Approval\Jt\JTDocList.htm
	\DeskPlusMobile\Approval\Ws\BE\ListInfo.cs
	\DeskPlusMobile\Approval\Ws\EDM\Transfer.cs
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\BaseView.aspx.cs
	\DeskPlusMobile\Approval\CarboncopyList.aspx <- 추가	
	\DeskPlusMobile\Approval\CirDocList.aspx <- 추가
	\DeskPlusMobile\Approval\CirDocView.aspx <- 추가
	\DeskPlusMobile\Approval\DeptList.aspx <- 추가
	\DeskPlusMobile\Approval\DocList.aspx <- 추가
	\DeskPlusMobile\Approval\DocView.aspx <- 추가
	\DeskPlusMobile\Approval\Folder.aspx
	\DeskPlusMobile\Approval\Memo.aspx  <- 추가
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\CarboncopyList.js <- 추가
	\DeskPlusMobile\Js\Approval\CirDocList.js <- 추가
	\DeskPlusMobile\Js\Approval\CirDocView.js <- 추가
	\DeskPlusMobile\Js\Approval\DeptList.js <- 추가
	\DeskPlusMobile\Js\Approval\DocList.js <- 추가
	\DeskPlusMobile\Js\Approval\DocView.js <- 추가
	\DeskPlusMobile\Js\Approval\Folder.js
	\DeskPlusMobile\Js\Approval\Memo.js <- 추가

: 2015-10-23 By freeman ------------------------------------------------------------
[웹폴더] 안드로이드 App 첨부파일 업로드 지원
	\DeskPlusMobileSolution\DeskPlusMobile\Js\WebHard\List.js   
	\DeskPlusMobileSolution\DeskPlusMobile\WebHard\List.aspx   .cs

: 2015-10-23 By SHIMPARK ------------------------------------------------------------
[게시판] 조회자 체크 저장하기

\DeskPlusMobileSolution\DeskPlusMobile\Board\View.aspx.cs

/// <summary>
/// 조회현황을 위하여.
/// </summary>
/// <param name="_readCount"></param>
private void SaveListViewers()
{
    //통합게시판 회사아이디
    int boaComID = Ws.BLL.Category.GetCompanyCategoryInfo(categoryID);

    Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ListViewer dal = new
            Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ListViewer();

    dal.Add(boaComID, categoryID, listID, UserInfo.UserID);
}

: 2015-10-23 By SHIMPARK ------------------------------------------------------------
[게시판 포탈] 포틀릿 읽지 않은 카운트 표기

*저장프로시저

- DPM_BOA_List_NOREADList

*소스
\DeskPlusMobileSolution\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs

 //통합게시판 회사아이디
                int boardComID = BLL.Category.GetCompanyCategoryInfo(Convert.ToInt32(arrCID[i]));
                categoryinfo = biz.GetNoReadList(boardComID, UserInfo.UserID, UserInfo.DeptID, Convert.ToInt32(arrCID[i]), PreLang);


: 2015-10-21 By famo23 ------------------------------------------------------------
[App] 승인 요청시에 승인불가 사용자 및 부서체크 
	\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

 2015-10-15 By shimpark ------------------------------------------------------------
 [통합게시판] : 글쓰기 부분 0 는 현상 fix

 [수정] 148 라인
 \DeskPlusMobileSolution\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs

	int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

 2015-10-15 By shimpark ------------------------------------------------------------
 [통합게시판]

 \DeskPlusMobile\Account\Data\Dept.cs
  : public DataSet GetDept(int _deptID) 추가

=====

\DeskPlusMobile\App_Data\SQLAccount.xml

<Item ID="SEL_QRY_DEPT_GetWhole"
          Value="SELECT * FROM DP_ACC_DEPT WHERE DeptID = {0} "
          Content="DeptID 로 부서 정보 가져오기(전사용)" />    
=====

	\DeskPlusMobile\Board\Ws\BLL\Category.cs
	\DeskPlusMobile\Board\Ws\Data\Category.cs
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs

** 저장프로시저 변경

	DPM_BOA_Attach_Sel
	DPM_BOA_Auth_Sel
	DPM_BOA_Auth_SelByCID
	DPM_BOA_Category_Sel
	DPM_BOA_Category_SelByCID
	DPM_BOA_Category_SelByPCID	
	DPM_BOA_List_NOREADList	
	DPM_BOA_List_Sel	
	DPM_BOA_List_SelByCIDLID	
	DPM_BOA_List_TopDown	
	DPM_BOA_List_Total	
	DPM_BOA_List_UpdByReadCount	
	DPM_BOA_ShortReply_Sel	


: public List<object> GetBoardList(string p) 함수 수정

	\DeskPlusMobileSolution\DeskPlusMobile\Portal\Portal.aspx.cs


: 2015-10-08 By famo23 ------------------------------------------------------------
[환경설정] 모바일관리 알람보이기
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\Js\Config\List.js

: 2015-10-07 By famo23 ------------------------------------------------------------
[주소록] 상세보기 UI 변경
	리소스추가 CellPhone / OfficePhone
	\DeskPlusMobile\Address\Ws\GetAddress.asmx.cs
	\DeskPlusMobile\Address\View.aspx
	\DeskPlusMobile\App_Data\SQLAddress.xml
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\Content\ko\Base\Img\email.png
	\DeskPlusMobile\Content\ko\Base\Img\hp.png
	\DeskPlusMobile\Content\ko\Base\Img\office.png
	\DeskPlusMobile\Content\ko\Base\Img\talk.png
	\DeskPlusMobile\Js\Address\View.js

: 2015-10-05 By famo23 ------------------------------------------------------------
[App] 모바일 승인 요청 알람
	\DeskPlusMobile\App_Data\SQLMDM.xml
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	\DeskPlusMobile\XmlData\MDM파일

: 2015-10-02 By famo23 ------------------------------------------------------------
[메일] 받는 사람, 참조 숨기기
	리소스추가 ViewToCc
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js
	\DeskPlusMobile\Mail\View.aspx
	\DeskPlusMobile\Mail\ViewTrash.aspx

[웹폴더] 전달받은 파일 전달자 보이기 처리
	리소스 추가 SendUser
	\DeskPlusMobile\WebHard\Jt\JTList.htm
	\DeskPlusMobile\WebHard\Ws\BE\FileInfo.cs
	\DeskPlusMobile\WebHard\Ws\GetWebHard.asmx.cs

: 2015-10-01 By famo23 ------------------------------------------------------------
[주소록] 다중선택으로 편지쓰기 가능하게 개발
	리소스추가
	\DeskPlusMobile\Address\Jt\JTList.htm
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\Address\Ws\BE\AddressInfo.cs
	\DeskPlusMobile\Address\Ws\GetAddress.asmx.cs
	\DeskPlusMobile\Address\List.aspx
	\DeskPlusMobile\Js\Address\List.js
	\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Mail\Send.aspx.cs	

: 2015-09-30 By famo23 ------------------------------------------------------------
[포탈] 첫화면에서 메뉴리스트 오픈으로 수정
	\DeskPlusMobile\Js\Portal\Portal.js

[App승인] App자동승인할지 , 관리자 승인일지에 따른 수정
	DPM_MDM_List_Ins
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

: 2015-09-15 By famo23 ------------------------------------------------------------
[전체] MaxJsonLength 용량 설정 수정
	\DeskPlusMobile\Helper\StringHelper.cs

: 2015-09-10 By famo23 ------------------------------------------------------------
[메일] 새로운 사용자 추가한 이후에 주소록에서 또 다른 사용자 추가할 경우에 이전 추가사용자 안 나오는 버그수정
	\DeskPlusMobile\Js\Mail\Send.js

: 2015-09-04 By famo23 ------------------------------------------------------------
[게시판] 조회자 체크 수정
	DPM_BOA_List_UpdByReadCount 삭제
	\DeskPlusMobile\Board\Ws\BLL\ListViewer.cs 삭제
	\DeskPlusMobile\Board\Ws\BLL\List.cs
	\DeskPlusMobile\Board\Ws\Data\List.cs
	\DeskPlusMobile\Board\View.aspx.cs

[게시판] 글쓰기 개발 / 댓글버튼 권한설정에 따른 제어
	리소스추가
	DPM_BOA_Category_SelByCID
	\DeskPlusMobile\Account\BLL\Dept.cs
	\DeskPlusMobile\Account\BLL\User.cs
	\DeskPlusMobile\Account\Data\Dept.cs
	\DeskPlusMobile\Account\Data\User.cs
	\DeskPlusMobile\Account\Data\UserDept.cs
	\DeskPlusMobile\App_Data\SQLAccount.xml
	\DeskPlusMobile\App_Data\SQLBoard.xml
	\DeskPlusMobile\Board\Jt\JTCalendar.htm
	\DeskPlusMobile\Board\Ws\BE\AttachInfo.cs
	\DeskPlusMobile\Board\Ws\BE\CalendarInfo.cs
	\DeskPlusMobile\Board\Ws\BE\HeaderInfo.cs
	\DeskPlusMobile\Board\Ws\BE\ListEtcInfo.cs
	\DeskPlusMobile\Board\Ws\BE\ListInfo.cs
	\DeskPlusMobile\Board\Ws\BE\XmlInfo.cs
	\DeskPlusMobile\Board\Ws\BLL\Authority.cs
	\DeskPlusMobile\Board\Ws\BLL\Category.cs
	\DeskPlusMobile\Board\Ws\Data\Authority.cs
	\DeskPlusMobile\Board\Ws\Data\Header.cs
	\DeskPlusMobile\Board\Ws\Data\List.cs
	\DeskPlusMobile\Board\Ws\Data\ListEtc.cs
	\DeskPlusMobile\Board\Ws\GetBoard.asmx.cs
	\DeskPlusMobile\Board\Ws\SetBoard.asmx.cs
	\DeskPlusMobile\Board\List.aspx
	\DeskPlusMobile\Board\List.aspx.cs
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\View.aspx.cs
	\DeskPlusMobile\Board\Write.aspx
	\DeskPlusMobile\Board\Write.aspx.cs
	\DeskPlusMobile\Helper\UploadBoardHandler.cs
	\DeskPlusMobile\Js\Board\List.js
	\DeskPlusMobile\Js\Board\Write.js
	\DeskPlusMobile\SMS\SMSHelper.cs
	\DeskPlusMobile\XmlData\BoardMessenger\
	\DeskPlusMobile\XmlData\FontColor\
	\DeskPlusMobile\XmlData\FontSize\
	\DeskPlusMobile\Web.config

: 2015-08-27 by withsuki -----------------------------------------------------------
[로그인] 로그인화면 추가 및 로그인페이지 설정 (10개)

	\DeskPlusMobile\Web.config
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\Portal\LogOut.aspx
	\DeskPlusMobile\Portal\Menu.ascx

	\DeskPlusMobile\Portal\Default1.aspx
	\DeskPlusMobile\Portal\Default2.aspx
	\DeskPlusMobile\Portal\Default3.aspx
	\DeskPlusMobile\Portal\Default4.aspx
	\DeskPlusMobile\Portal\Default5.aspx
	\DeskPlusMobile\Portal\Default6.aspx
	\DeskPlusMobile\Portal\Default7.aspx
	\DeskPlusMobile\Portal\Default8.aspx
	\DeskPlusMobile\Portal\Default9.aspx
	\DeskPlusMobile\Portal\Default10.aspx

: 2015-08-26 By famo23 ------------------------------------------------------------
[공통] trim() 부분 수정
	\DeskPlusMobile\Html\Js\jquery.common.js

: 2015-08-07 By famo23 ------------------------------------------------------------
[주소록] 회사주소록 정렬변경
	\DeskPlusMobile\App_Data\SQLAddress.xml

: 2015-08-06 By famo23 ------------------------------------------------------------
[메뉴] 건수표시 
	메일 : 받은 메일함+개인메일함 읽지 않은 메일 건수
	전자결재 : 결재대상문서,대결결재대상문서 건수
	게시판 : 읽지 않은 건수
	일정관리 : 당일 일정,할일
	마이톡:당일 등록된 글
	업무협업 : 요청받은 협업+참조받은협업+전달받은 협업 읽지 않은 건수
	커뮤니티 : 읽지 않은 게시판 글

	\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\Portal\Menu.ascx.cs
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\App_Data\SQLCommunity.xml
	\DeskPlusMobile\App_Data\SQLSNS.xml
	\DeskPlusMobile\App_Data\SQLTask.xml
	\DeskPlusMobile\Board\Ws\BLL\List.cs
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

	DPM_APP_SelMenuCnt
	DPM_SNS_MyTalk_SelCnt
	DPM_TAS_SelMenuCnt


: 2015-08-05 By famo23 ------------------------------------------------------------
[메일] 수신자 자동완성

	DPM_MAIL_User_SelDeep
	DPM_MAIL_User_SelByNameDeep

	MailError1~3 리소스추가
	\DeskPlusMobile\Html\Js\jquery-ui.css
	\DeskPlusMobile\Html\Js\jquery-ui.js
	\DeskPlusMobile\App_Data\SQLMail.xml

	\DeskPlusMobile\Mail\Ws\BE\SearchInfo.cs
	\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs
	\DeskPlusMobile\Mail\Ws\SetMail.asmx.cs
	\DeskPlusMobile\Mail\Send.aspx
	\DeskPlusMobile\Js\Mail\Send.js

[메일] 제타메일은 구분값을 ";"로 했는데 딥메일은 구분값을 "," 사용하므로 수정
	\DeskPlusMobile\DeskPlusMobile\Js\Mail\Send.js
	\DeskPlusMobile\Mail\Ws\SetMail.asmx.cs

: 2015-07-28 By famo23 ------------------------------------------------------------
[환경설정] 모바일관리 알람빼기
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\Js\Config\List.js

[커뮤니티] 퇴사자 정보 안보이는 문제처리
	\DeskPlusMobile\App_Data\SQLCommunity.xml
	\DeskPlusMobile\Community\Ws\GetCommunity.asmx.cs
	\DeskPlusMobile\Community\BoardView.aspx.cs
	DPM_CMN_Boa_Sel

[커뮤니티] 댓글 정렬변경
	\DeskPlusMobile\App_Data\SQLCommunity.xml

[디자인] 회사명 짤려 보이는 현상수정
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

[전자결재] 반려대상자 병렬 합의자 비활성화처리
	\DeskPlusMobile\Js\Approval\Return.js	

[전자결재] 반려 첨언 필수 체크
	\DeskPlusMobile\Js\Approval\Return.js	
	\DeskPlusMobile\Approval\Return.aspx

[전자결재] 문서css에 따른 디자인 깨지는 거 수정
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\View.aspx	
	

: 2015-06-23 By famo23 ------------------------------------------------------------
[앱로그인] 최초 로그인 시에 regID 값 필수로 받아야 하는 체크
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

: 2015-06-22 By famo23 ------------------------------------------------------------
[로그인] 모바일Safari에서 로그인버튼 디자인 수정
	\DeskPlusMobile\Portal\Default.aspx

: 2015-06-02 By famo23 ------------------------------------------------------------
[마이톡] 파일추가 버튼 숨기기
	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\Add.aspx.cs

: 2015-05-26 By famo23 ------------------------------------------------------------
[일정관리] 리스트에서 공유할일,개인할일등 설명 보여주기
	리소스추가
	\DeskPlusMobile\Pims\Jt\JTSchedule.htm
	\DeskPlusMobile\Pims\Jt\JTToDo.htm
	\DeskPlusMobile\Pims\Ws\BE\ScheduleInfo.cs
	\DeskPlusMobile\Pims\Ws\BE\ToDoInfo.cs
	\DeskPlusMobile\Pims\Ws\GetPims.asmx.cs   
	\DeskPlusMobile\Portal\Jt\JTSchedule.htm
	\DeskPlusMobile\Portal\Jt\JTToDo.htm
	\DeskPlusMobile\Portal\Ws\BE\ScheduleInfo.cs
	\DeskPlusMobile\Portal\Ws\BE\ToDoInfo.cs
	\DeskPlusMobile\Portal\Ws\GetPortal.asmx.cs 

[전체]목록 전체 클릭되게 수정
	\DeskPlusMobile\Address\Add.aspx
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Board\Add.aspx	
	\DeskPlusMobile\Community\Add.aspx
	\DeskPlusMobile\Coop\Add.aspx
	\DeskPlusMobile\Coop\AddAccept.aspx
	\DeskPlusMobile\Coop\AddChange.aspx
	\DeskPlusMobile\Coop\AddComplete.aspx
	\DeskPlusMobile\MyTalk\Add.aspx	
	\DeskPlusMobile\MyTalk\View.aspx
	\DeskPlusMobile\Pims\ReplyAdd.aspx
	\DeskPlusMobile\Pims\View.aspx
	\DeskPlusMobile\Task\AddAccept.aspx
	\DeskPlusMobile\Task\AddComplete.aspx
	\DeskPlusMobile\Task\AddReject.aspx
	\DeskPlusMobile\Task\AddReply.aspx
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css


: 2015-05-21 By famo23 ------------------------------------------------------------
[업무협업]모바일 버전 개발 Task
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\Js\Config\List.js

	DP_MDM_Menu 기본값세팅
	\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\Portal\Menu.ascx.cs
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css	
	\DeskPlusMobile\Helper\XmlDataBaseHelper.cs
	\DeskPlusMobile\Helper\FileHelper.cs
	\DeskPlusMobile\App_Data\SQLTask.xml
	\DeskPlusMobile\App_Data\Task.xml
	Task 추가

: 2015-04-06 By famo23 ------------------------------------------------------------
[업무협업]모바일 버전 개발 Coop

: 2015-03-24 By famo23 ------------------------------------------------------------
[공통] 사용자언어설정이 없을 경우
   \DeskPlusMobile\Pims\Add.aspx.cs
   \DeskPlusMobile\Html\Js\jquery.common.js

: 2015-03-19 By famo23 ------------------------------------------------------------
[마이톡] 이미지 업로드
	리소스
	DP_SNS_Attach 테이블
	DP_SNS_MyTalk_Ins
	DP_SNS_MyTalk_DelByGroupID
	DP_SNS_Attach_Ins
	DPM_SNS_MyTalk_Sel
	\DeskPlusMobile\Web.config
	\DeskPlusMobile\App_Data\SQLSNS.xml
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css
	\DeskPlusMobile\Helper\UploadMyTalkHandler.cs
	\DeskPlusMobile\Js\MyTalk\Add.js
	\DeskPlusMobile\Js\MyTalk\List.js
	\DeskPlusMobile\Js\MyTalk\View.js	
	\DeskPlusMobile\MyTalk\Jt\JtSNSList.htm
	\DeskPlusMobile\MyTalk\Ws\BE\AttachInfo.cs
	\DeskPlusMobile\MyTalk\Ws\BE\MyTalkInfo.cs
	\DeskPlusMobile\MyTalk\Ws\GetMyTalk.asmx.cs
	\DeskPlusMobile\MyTalk\Ws\SetMyTalk.asmx.cs
	\DeskPlusMobile\MyTalk\Add.aspx
	\DeskPlusMobile\MyTalk\Add.aspx.cs
	\DeskPlusMobile\MyTalk\List.aspx
	\DeskPlusMobile\Html\Js\jquery.event.drag-1.5.min.js
	\DeskPlusMobile\Html\Js\jquery.touchslider.js
	\DeskPlusMobile\Html\Js\jquery.form.min.js

: 2015-03-17 by freeman -----------------------------------------------------------
[공통-버그] 브라우져에서 패키지가 제공하지 않는 언어 리소스 사용시 문제 ==> userLang.StartsWith("en") || userLang.StartsWith("fr") || userLang.StartsWith("ja") || userLang.StartsWith("ko") || userLang.StartsWith("zh")
	\DeskPlusMobile\Helper\MobileBaseControl.cs(175)
	\DeskPlusMobile\Helper\MobileBasePage.cs(179)
	\DeskPlusMobile\Helper\MobileBaseService.cs(124)

### DeskPlusMobile v4.3.0 (2015.02.27) #########################################################

: 2015-03-02 By famo23 ------------------------------------------------------------
[웹하드] 모바일 이미지 업로드 
	리소스
	\DeskPlusMobile\Helper\UploadAttachHandler.cs
	\DeskPlusMobile\WebHard\List.aspx
	\DeskPlusMobile\WebHard\List.aspx.cs
	\DeskPlusMobile\Js\WebHard\List.js
	\DeskPlusMobile\Web.config
	\DeskPlusMobile\App_Data\Base.xml
	\DeskPlusMobile\App_Data\SQLWebHard.xml
	\DeskPlusMobile\WebHard\Ws\BE\FileInfo.cs
	\DeskPlusMobile\WebHard\Ws\BE\FolderInfo.cs
	\DeskPlusMobile\WebHard\Ws\DAL\File.cs
	\DeskPlusMobile\WebHard\Ws\DAL\Folder.cs
	\DeskPlusMobile\Content\ko\Base\Css\mobile_main.css

[웹하드] 정렬순서 버그수정
	\DeskPlusMobile\WebHard\List.aspx.cs
	\DeskPlusMobile\WebHard\Send.aspx.cs

: 2015-01-27 By famo23 ------------------------------------------------------------
[App] 알람 제목 나오게 처리 
	트리거 TRI_Alarm_Common_Insert
	DPM_ALARM_Common_SelByIOS
	DPM_ALARM_Common_Sel
	DPmanager소스수정

: 2015-01-23 By famo23 ------------------------------------------------------------
[게시판] 헤더글자수 변경
	\DeskPlusMobile\Board\List.aspx.cs

[커뮤니티] 헤더글자수 변경
	\DeskPlusMobile\Community\BoardFolder.aspx.cs

: 2015-01-13 By withsuki ------------------------------------------------------------
[전자결재 - 공람추가]
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Js\Approval\View.js
	\DeskPlusMobile\Resource\GlobalString\string.en.resx
	\DeskPlusMobile\Resource\GlobalString\string.fr.resx
	\DeskPlusMobile\Resource\GlobalString\string.ja.resx
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-cn.resx
	\DeskPlusMobile\Resource\GlobalString\string.zh-tw.resx

: 2015-01-05 By hooper2 ------------------------------------------------------------
[전자결재 - 알람 개인설정 추가]
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Approval\Ws\BE\AccountInfo.cs
	\DeskPlusMobile\App_Data\SQLApproval.xml

: 2015-01-05 By SHIMPARK ------------------------------------------------------------
[메일 - 휴지통 비우기]
	\DeskPlusMobile\Mail\Trash.aspx
	\DeskPlusMobile\Js\Mail\Trash.js

: 2015-01-02 By famo23 ------------------------------------------------------------
[공통 - JS파일 호출방식변경]
	JS파일 호출하는 모든 .aspx파일수정
	\DeskPlusMobile\Html\Js\jquery.common.js

[전자결재 - 검색추가(상신자/제목)]
	\DeskPlusMobile\Portal\Ws\GetPortal.asmx
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Approval\Approve.aspx
	\DeskPlusMobile\Approval\Approve.aspx.cs
	\DeskPlusMobile\Approval\BaseView.aspx
	\DeskPlusMobile\Approval\BaseView.aspx.cs
	\DeskPlusMobile\Approval\List.aspx
	\DeskPlusMobile\Approval\List.aspx.cs
	\DeskPlusMobile\Approval\Return.aspx
	\DeskPlusMobile\Approval\Return.aspx.cs
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\List.js
	\DeskPlusMobile\Js\Approval\Return.js
	\DeskPlusMobile\Js\Approval\View.js
	DPM_APP_List_TopDown_Report
	DPM_APP_List_TopDown_Approve
	DPM_APP_List_TopDown_Related
	DPM_APP_DocProcess_SelReport
	DPM_APP_DocProcess_SelApprove
	DPM_APP_DocProcess_SelRelated

[전자결재 - 대결자문서카운트수정]
	DPM_APP_SelCnt

[마이톡 - 디자인 변경]
	\DeskPlusMobile\MyTalk\Jt\JtList.htm
	\DeskPlusMobile\MyTalk\List.aspx

: 2014-12.09 By shimpark ------------------------------------------------------------
[메일 - 답장, 전체답장, 전달 원문글 미포함 버그 fix]
\DeskPlusMobileSolution\DeskPlusMobile\Js\Mail\Send.js 642

//Action 1 : 답장 / Action 2 : 전달 / Action 3 : 전체답장입니다.
    if ((Action == "1") || (Action == "2") || (Action == "3")) {
        //html() 은 인코딩하므로, 변환하여  순수글자를 만들어 준다. modify shimpark 2014-12-09
        content = content + "<br><br>" + $("#contentList").html().replace(/&lt;/g, '<').replace(/&gt;/g, '>');
    }

	 
\DeskPlusMobileSolution\DeskPlusMobile\Mail\Ws\SetMail.asmx.cs 261

//본문 인코딩 해야 함. modify shimpark
string param = string.Format(paramMailSend, UserInfo.MailDomain, UserInfo.LoginID.ToXSS(), UserInfo.MailPassword, strTo,
    strCc, strBcc, strSubject, "1", forward, Server.UrlEncode(strBody));



: 2014-12-02 By famo23 ------------------------------------------------------------
[공통 - 파라미터값 디코딩처리]
	\DeskPlusMobile\Html\Js\jquery.common.js
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx
	\DeskPlusMobile\Helper\MobileBaseControl.cs
	\DeskPlusMobile\Helper\MobileBasePage.cs
	\DeskPlusMobile\Helper\MobileBaseService.cs

	\DeskPlusMobile\Js\Address\Add.js
	\DeskPlusMobile\Js\Address\List.js
	\DeskPlusMobile\Js\Address\View.js
	\DeskPlusMobile\Js\Board\Add.js
	\DeskPlusMobile\Js\Board\List.js
	\DeskPlusMobile\Js\Board\View.js
	\DeskPlusMobile\Js\Community\Add.js
	\DeskPlusMobile\Js\Community\BoardList.js
	\DeskPlusMobile\Js\Community\BoardView.js

: 2014-10-17 By freeman ------------------------------------------------------------
[메일 - 보낸 날짜가 잘못 표시되는 버그]
	\DeskPlusMobile\Helper\SendMail.cs (177)

: 2014-08-26 By shimpark ----------------------------------------------------------
[게시판 - 게시판 보기의 이미지 보이기]
	\DeskPlusMobile\Board\View.aspx
	\DeskPlusMobile\Board\View.aspx.cs

: 2014-07-25 By famo23 ------------------------------------------------------------
[전자결재 결재 승인시 패스워드 불일치 문제 : 패스워드에 특수문자 & 를 사용할 경우 문제 ]
	\DeskPlusMobile\Js\Approval\Approve.js
	\DeskPlusMobile\Js\Approval\Return.js
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs

: 2014-07-24 By famo23 ----------------------------------------------------------
[메일 - 메일함 리소스 변경]
	\DeskPlusMobile\Resource\GlobalString\string.ko.resx

[메일 - 받는 사람이 안 나오는 버그수정]
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js

[메일 - 참조 안 나오는 버그수정]
	\DeskPlusMobile\Js\Mail\View.js
	\DeskPlusMobile\Js\Mail\ViewTrash.js

[메일 - 전체답장 메일주소 버그수정]
	\DeskPlusMobile\Js\Mail\Send.js

[메일 - 답장, 전달에서 본문내용에 받는사람,참조 안 나오는 버그수정]
	\DeskPlusMobile\Js\Mail\Send.js

[메일 - 체크박스 전체선택 버그수정]
	\DeskPlusMobile\Js\Mail\List.js
    \DeskPlusMobile\Js\Mail\Trash.js

[메일 - 삭제, 복원하고선 첫번째 메일 안 나오는 버그수정]
	\DeskPlusMobile\Js\Mail\List.js
    \DeskPlusMobile\Js\Mail\Trash.js

[메일 - 읽은표시 하고 첨부파일 부르는 버그수정]
	\DeskPlusMobile\Js\Mail\View.js

: 2014-07-22 By famo23 ----------------------------------------------------------
[메일 - 개인함 카운트 버그수정]
	\DeskPlusMobile\DeskPlusMobile\Mail\Ws\GetMail.asmx.cs

[메일 - 로그인 메일 Exception 버그수정]
	\DeskPlusMobile\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

: 2014-07-11 By famo23 ----------------------------------------------------------
[전자결재 - 그룹웨어알람변경된것 적용]
	\DeskPlusMobile\App_Data\Approval.xml
	\DeskPlusMobile\Approval\Ws\BE\AccountInfo.cs
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs

: 2014-07-11 By shimpark ----------------------------------------------------------
[버그 - 파라미터에 & 값이 있을경우 GetParmforParameter 오류]
	여러 페이지에 존재하는 GetParmforParameter() 함수를 
	C:\SiteProject\Somo\DeskPlusMobileSolution\DeskPlusMobile\Helper\WebComponent\Utility.cs 에 GetParmforParameter() 함수를
	넣어서 각 페이지에서 호출하도록 통합작업 함.


: 2014-06-19 By famo23 ----------------------------------------------------------
[전자결재 - 알람제목에 특수기호 변환 안된 버그수정]
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	
[전자결재 - 제목에 특수기호 변환 안되어서 나오는 버그 수정]
	\DeskPlusMobile\Js\Approval\View.js

: 2014-06-17 By famo23 ----------------------------------------------------------

[모바일메뉴권한관리개발 - 그룹웨어도 개발]

	\DeskPlusMobile\DeskPlusMobile\App_Data\SQLPortal.xml
	\DeskPlusMobile\DeskPlusMobile\Helper\UserInfoHelper.cs
	
	\DeskPlusMobile\DeskPlusMobile\Js\Address\View.js
	\DeskPlusMobile\DeskPlusMobile\Address\View.aspx
	\DeskPlusMobile\DeskPlusMobile\Address\View.aspx.cs

	\DeskPlusMobile\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\DeskPlusMobile\Js\Config\List.js

	\DeskPlusMobile\DeskPlusMobile\Mail\Send.aspx
	\DeskPlusMobile\DeskPlusMobile\Mail\Send.aspx.cs
	
	\DeskPlusMobile\DeskPlusMobile\Js\Portal\Portal.js
	\DeskPlusMobile\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
	\DeskPlusMobile\DeskPlusMobile\Portal\Menu.ascx
	\DeskPlusMobile\DeskPlusMobile\Portal\Menu.ascx.cs	
	\DeskPlusMobile\DeskPlusMobile\Portal\Portal.aspx
	\DeskPlusMobile\DeskPlusMobile\Portal\Portal.aspx.cs	
	\DeskPlusMobile\Helper\MobileBaseControl.cs
	\DeskPlusMobile\Helper\MobileBasePage.cs
	\DeskPlusMobile\Helper\MobileBaseService.cs

[앱로그인 제타메일제거]
	\DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs

[전자결재 - 결재시 부서,직급 변경되면 생기는 버그 수정 - 그룹웨어도 수정]	
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\App_Data\SQLApproval.xml

[전자결재 - 셀렉트 박스, 라디오버튼 disabled 처리]
	\DeskPlusMobile\Js\Approval\BaseView.js
	\DeskPlusMobile\Js\Approval\View.js

[전자결재 - 대결자가 ZP,이관문서 상신했을 경우 개인결재함에 안 나오는 버그 수정]
	DPM_APP_DocProcess_UpdConclude

[전자결재 - 겸직관련 버그 수정 - 그룹웨어도 수정]
	DPM_APP_DocProcess_SelReport
	DPM_APP_DocProcess_SelApprove
	DPM_APP_DocProcess_SelRelated
	DPM_APP_List_TopDown_Report
	DPM_APP_List_TopDown_Approve
	DPM_APP_List_TopDown_Related
	\DeskPlusMobile\App_Data\SQLApproval.xml
	\DeskPlusMobile\Approval\Jt\JTList.htm
	\DeskPlusMobile\Approval\Ws\BE\ListInfo.cs
	\DeskPlusMobile\Approval\Ws\GetApproval.asmx.cs
	\DeskPlusMobile\Approval\Ws\SetApproval.asmx.cs
	\DeskPlusMobile\Approval\List.aspx
	\DeskPlusMobile\Approval\List.aspx.cs
	\DeskPlusMobile\Approval\View.aspx
	\DeskPlusMobile\Approval\View.aspx.cs
	\DeskPlusMobile\Js\Approval\List.js
	\DeskPlusMobile\Js\Approval\View.js

[알람설정변경- 그룹웨어도 수정]
	\DeskPlusMobile\Config\Ws\BE\ListInfo.cs
	\DeskPlusMobile\Config\Ws\SetConfig.asmx.cs
	\DeskPlusMobile\Config\List.aspx
	\DeskPlusMobile\Config\List.aspx.cs
	\DeskPlusMobile\Js\Config\List.js
	리소스추가
	DPM_MDM_List_Upd
	DPM_ALARM_Common_Sel
	DPM_ALARM_Common_SelByIOS

[전자결재 - 반려할때 커스텀파일 호출 안되는 버그 수정]
 \DeskPlusMobile\Approval\Return.aspx.cs

[전자결재 - 결재대상문서에 보류문서 안 나오게 수정]
  DPM_APP_DocProcess_SelApprove

[전자결재 - 결재상황조회문서리스트쿼리수정(그룹웨어랑 맞추는 작업)
  DPM_APP_DocProcess_SelReport
  DPM_APP_DocProcess_SelRelated
  DPM_APP_DocProcess_SelApprove
  DPM_APP_SelCnt
  DPM_APP_List_TopDown_Report
  DPM_APP_List_TopDown_Related
  DPM_APP_List_TopDown_Approve

[전자결재 - 전결문서함 처리 버그수정 - ZQ(빨간수신)가 있는 문서에서 발신전결이 생겼을 때 전결문서함에 안 나오는 부분]
  DPM_APP_DocProcess_ApproveA

[아이폰 개발내역 추가]
 알람데이터베이스에 DPM_ALARM_CommonIOS , DPM_ALARM_MailIOS 테이블 추가
 DP_ALARM_Common 테이블 트리거 수정
 DPM_ALARM_Common_Sel 변경
 DPM_ALARM_Common_SelByIOS 추가

 \DeskPlusMobile\Portal\Default.aspx
 \DeskPlusMobile\Js\Portal\Default.js
 \DeskPlusMobile\Portal\Ws\GetDefault.asmx.cs
 \DeskPlusMobile\Portal\Footer.ascx.cs
 \DeskPlusMobile\Portal\LogOut.aspx
 \DeskPlusMobile\App_Data\SQLMDM.xml
 \DeskPlusMobile\Content\ko\Base\Css\mobile_login.css

[전자결재] 다국어깨지는 현상수정
 sp변경

 DPM_APP_DocProcess_ApproveA
 DPM_APP_DocProcess_ApproveH
 DPM_APP_DocProcess_ApproveS
 DPM_APP_DocProcess_Cancel
 DPM_APP_DocProcess_Return

[체크박스] 체크박스 전체 선택한 이후에 몇개 체크해제하고 삭제,복원했을 시에 해제한게 적용 안되는 버그 - 웹폴더
  \DeskPlusMobile\Js\WebHard\List.js

[로그인화면] APP 관련 디자인 변경
 
  \DeskPlusMobile\Portal\Default.aspx
  \DeskPlusMobile\Content\ko\Base\Css\mobile_login.css
  리소스추가 - AppDownload


: 2013-07-19 By famo23 ----------------------------------------------------------
[전자결재알람] 결재의견 알람 발송시에 깨지는 문제수정  
 
   \DeskPlusMobile\Approval\Ws\SetApproval.asmx

: 2013-07-01 By Freeman ----------------------------------------------------------
[테이블 정리]사용하지 않는 일정관리 테이블 삭제 및 관련 SP 수정
=======================================
* DP_PIM_OpenUserGroup 삭제
-DPM_PIM_ToDo_SelByType2
-DPM_PIM_ToDo_SelMonthByType2
-DPM_PIM_Schedule_SelByType2
-DPM_PIM_Schedule_SelMonthByType2


: 20130325 ByFreeman ----------------------------------------------------------
[안드로이드 바로기가 버튼] 안드로이드 App 테스트를 위한 바로 가기 버튼을 로그인 화면에 생성
	\DeskPlusMobile\Portal\Default.aspx

	\DeskPlusMobile\Js\Portal\Default.aspx

	\DeskPlusMobile\Content\ko\Base\Img\btn_AndApp.png



: 20130325 ByFreeman ----------------------------------------------------------
[디자인 변경] 메뉴 아이콘이 가운데 정렬 되도록 수정 (아이콘 3개 기준)
	\DeskPlusMobile\Portal\Menu.ascx
