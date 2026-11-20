<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Admin.DashBoard" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <style type="text/css">
        #cbtnReset { box-sizing: border-box; height: 30px; margin: 0; flex-shrink: 0; white-space: nowrap; }
        #layout_header { border-bottom: 0 !important; }
        #layout_srh { height: auto; padding: 14px 20px; box-sizing: border-box; }
        #layout_srh .edu-dashboard-search { display: flex; align-items: center; flex-wrap: wrap; gap: 10px 18px; }
        #layout_srh .edu-search-field { display: flex; align-items: center; gap: 6px; min-width: 0; min-height: 30px; }
        #layout_srh label { margin: 0; color: #333; white-space: nowrap; }
        #layout_srh select, #layout_srh input { box-sizing: border-box; height: 30px; margin: 0; min-width: 0; }
        #layout_srh .edu-month-field { position: relative; display: inline-flex; align-items: center; }
        #layout_srh .edu-month-input { width: 112px; padding: 0 32px 0 8px; background: #fff; cursor: pointer; }
        #layout_srh .edu-month-trigger { position: absolute; top: 1px; right: 1px; display: inline-flex; align-items: center; justify-content: center; width: 28px; height: 28px; padding: 0; border: 0; background: transparent; color: #3978da; cursor: pointer; }
        #layout_srh .edu-month-trigger:focus { outline: 2px solid #9fc3f7; outline-offset: -2px; }
        .edu-month-picker { display: none; position: fixed; z-index: 1100; width: 244px; padding: 12px; box-sizing: border-box; border: 1px solid #d6dde6; border-radius: 5px; background: #fff; box-shadow: 0 4px 16px rgba(0,0,0,.15); }
        .edu-month-picker-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 10px; color: #333; }
        .edu-month-picker-header button { width: 28px; height: 28px; padding: 0; border: 1px solid #dee2e6; border-radius: 3px; background: #fff; color: #3978da; font-size: 20px; line-height: 1; cursor: pointer; }
        .edu-month-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 6px; }
        .edu-month-grid button { height: 34px; padding: 0; border: 1px solid transparent; border-radius: 3px; background: #f5f7fa; color: #333; cursor: pointer; }
        .edu-month-grid button.is-current { border-color: #9fc3f7; }
        .edu-month-grid button.is-selected { border-color: #3978da; background: #3978da; color: #fff; }
        .edu-month-picker button:disabled { background: #fafafa; color: #bcc2ca; cursor: default; }
        .edu-month-picker button:not(:disabled):hover, .edu-month-picker button:not(:disabled):focus { outline: 2px solid #9fc3f7; outline-offset: -2px; }
        #layout_srh #ddlTarget, #layout_srh #ddlGubun, #layout_srh #ddlCourseStatus { width: 85px; padding-left: 6px; }
        #layout_srh #targetNames { width: 230px; color: #555; background: #fff; cursor: default; }
        #layout_srh #ctbSearchTitle { width: 200px; max-width: 100%; padding: 0 8px; }
        #layout_srh .edu-search-buttons { display: flex; align-items: center; gap: 8px; }
        #layout_srh .edu-search-buttons a, #layout_srh #cbtnSelectTargets { display: inline-flex; align-items: center; justify-content: center; height: 30px; box-sizing: border-box; margin: 0; }
        #layout_srh .edu-disabled { opacity: .55; cursor: default; }
        .edu-dashboard-heading { margin: 18px 30px 12px; color: #333; font-weight: bold; font-size: 15px; }
        .edu-course-heading { display: flex; align-items: center; justify-content: space-between; gap: 12px; margin: 18px 30px 12px; }
        .edu-course-heading .edu-dashboard-heading { margin: 0; }
        .edu-course-navigation { display: flex; align-items: center; flex: 0 0 auto; }
        .edu-course-navigation .edu-course-page-button { display: inline-flex; align-items: center; justify-content: center; width: 30px; height: 28px; margin: 0; padding: 0; border: 1px solid #dee2e6; border-radius: 3px 0 0 3px; background: #fff; color: #3978da; font-size: 12px; line-height: 1; cursor: pointer; box-sizing: border-box; }
        .edu-course-navigation .edu-course-page-button + .edu-course-page-button { margin-left: -1px; border-radius: 0 3px 3px 0; }
        .edu-course-navigation .edu-course-page-button:disabled { background: #f8f9fa; color: #b2b8bf; cursor: default; }
        .edu-course-navigation .edu-course-page-button:not(:disabled):hover, .edu-course-navigation .edu-course-page-button:not(:disabled):focus { position: relative; border-color: #b8cce8; background: #eef4fb; color: #285eac; }
        .edu-course-navigation .edu-course-page-button:not(:disabled):focus { outline: 0; box-shadow: 0 0 0 2px rgba(57,120,218,.2); }
        .edu-course-page-info { position: absolute; width: 1px; height: 1px; padding: 0; margin: -1px; overflow: hidden; clip: rect(0,0,0,0); white-space: nowrap; border: 0; }
        .edu-dashboard-summary { display: flex; gap: 10px; margin: 0 30px 14px; box-sizing: border-box; }
        .edu-dashboard-summary .edu-summary-count { flex: 1; min-width: 0; display: flex; flex-direction: column; align-items: flex-start; justify-content: center; padding: 16px 20px; border: 1px solid #dfe3e8; border-radius: 5px; background: #fff; box-shadow: 0 2px 5px rgba(0,0,0,.03); cursor: pointer; box-sizing: border-box; }
        .edu-dashboard-summary .edu-summary-count:hover, .edu-dashboard-summary .edu-summary-count:focus { outline: 0; background: #f7f9fc; }
        .edu-dashboard-summary .edu-summary-count:focus { box-shadow: inset 0 0 0 2px #c9d5e5; }
        .edu-dashboard-summary .edu-summary-count.is-selected { border-color: #3978da; box-shadow: inset 0 0 0 1px #3978da; }
        .edu-dashboard-summary .edu-summary-count > span { font-weight: bold; font-size: 12px; color: #333; line-height: 18px; padding: 3px 8px; border-radius: 2px; }
        .edu-dashboard-summary .edu-summary-count.wait > span { background: #75ace9; color: #fff; }
        .edu-dashboard-summary .edu-summary-count.progressing > span { background: #8bc63e; color: #fff; }
        .edu-dashboard-summary .edu-summary-count.complete > span { background: #343a40; color: #fff; }
        .edu-dashboard-summary .edu-summary-count.delay > span { background: #9e9080; color: #fff; }
        .edu-dashboard-summary .edu-summary-count strong { margin-top: 12px; font-size: 25px; color: #111827; line-height: 32px; }
        .edu-dashboard-summary .edu-summary-count.wait strong { color: #75ace9; }
        .edu-dashboard-summary .edu-summary-count.progressing strong { color: #8bc63e; }
        .edu-dashboard-summary .edu-summary-count.complete strong { color: #343a40; }
        .edu-dashboard-summary .edu-summary-count.complete strong { display: flex; align-items: baseline; flex-wrap: wrap; gap: 4px 8px; max-width: 100%; }
        .edu-dashboard-summary .edu-complete-person, .edu-dashboard-summary .edu-complete-rate { white-space: nowrap; }
        .edu-dashboard-summary .edu-complete-person { font-size: inherit !important; font-weight: inherit; line-height: inherit; }
        .edu-dashboard-summary .edu-complete-rate { color: #3978da; font-size: 14px; line-height: 22px; }
        .edu-dashboard-summary .edu-summary-count.delay strong { color: #9e9080; }
        .edu-dashboard-summary .edu-summary-count em { color: inherit !important; font-size: inherit !important; font-weight: inherit; line-height: inherit; font-style: normal; }
        #phList .edu-course-panel { border: 1px solid #dfe3e8; border-radius: 5px; background: #fff; padding: 6px 20px; }
        #phList .edu-course-row { padding: 18px 0; }
        #phList .edu-course-row + .edu-course-row { border-top: 1px solid #f0f1f3; }
        #phList .edu-course-header { display: flex; align-items: flex-start; justify-content: space-between; gap: 16px; margin-bottom: 7px; }
        #phList .edu-course-title { min-width: 0; flex: 1; }
        #phList .edu-course-title .edu-title-link { color: #333; font-weight: bold; font-size: 14px; overflow-wrap: break-word; }
        #phList .edu-course-title .edu-title-link:hover { color: #3978da; }
        #phList .edu-course-info { margin-top: 3px; color: #777; font-size: 12px; line-height: 18px; }
        #phList .edu-course-count { color: #555; min-width: 0; max-width: 70%; white-space: normal; text-align: right; line-height: 20px; }
        #phList .edu-course-count > a { display: inline-block; color: inherit; white-space: nowrap; }
        #phList .edu-course-count > a:hover, #phList .edu-course-count > a:focus { color: #3978da; text-decoration: underline; }
        #phList .edu-course-empty { padding: 22px 0; color: #777; text-align: center; }
        #phList [action], #layout_srh a { cursor: pointer; }
        #phList .edu-title-link { display: block; line-height: 20px; }
        #phList .edu-progress-row { display: flex; align-items: center; gap: 10px; }
        #phList .edu-progress-track { display: flex; flex: 1; height: 10px; background: #edf0f3; border-radius: 5px; overflow: hidden; }
        #phList .edu-progress-bar { flex: 0 0 auto; min-width: 0; height: 100%; }
        .edu-progress-legend { display: flex; flex-wrap: wrap; gap: 8px 16px; margin: 0 30px 12px; color: #555; font-size: 12px; }
        .edu-progress-legend > span { display: inline-flex; align-items: center; gap: 5px; white-space: nowrap; }
        .edu-progress-legend .edu-legend-color { display: inline-block; width: 10px; height: 10px; border-radius: 2px; }
        #phList .edu-progress-bar.complete, .edu-progress-legend .complete { background: #343a40; }
        #phList .edu-progress-bar.wait, .edu-progress-legend .wait { background: #75ace9; }
        #phList .edu-progress-bar.progressing, .edu-progress-legend .progressing { background: #8bc63e; }
        #phList .edu-progress-bar.delay, .edu-progress-legend .delay { background: #9e9080; }
        @media (max-width: 700px) {
            #layout_srh { padding: 12px; }
            .edu-dashboard-summary { margin-right: 12px; margin-left: 12px; flex-wrap: wrap; }
            .edu-dashboard-summary .edu-summary-count { flex: 1 1 calc(33.333% - 10px); padding: 12px; }
            .edu-dashboard-summary .edu-summary-count:nth-child(n+4) { flex-basis: calc(50% - 10px); }
            .edu-dashboard-heading { margin-right: 12px; margin-left: 12px; }
            .edu-course-heading { margin-right: 12px; margin-left: 12px; }
            .edu-progress-legend { margin-right: 12px; margin-left: 12px; }
            #layout_srh .edu-target-field { flex-wrap: wrap; }
            #layout_srh #targetNames { width: 190px; }
            #phList .edu-course-header { flex-wrap: wrap; gap: 6px; }
            #phList .edu-course-title { flex-basis: 100%; }
            #phList .edu-course-count { max-width: 100%; }
        }
        @media (max-width: 420px) {
            #layout_srh .edu-search-field { flex-wrap: wrap; }
            #layout_srh .edu-month-input { width: 108px; }
        }
    </style>
</head>
<body>
   <form id="form1" runat="server">
<div id="layout_body">

    <div class="layout_full">
    
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>전체현황</h1>
        </div>

        <div id="layout_srh" aria-label="현황 검색">
            <div class="edu-dashboard-search">
                <div class="edu-search-field">
                    <label for="tbxStartYM">시작년/월</label>
                    <span class="edu-month-field">
                        <asp:TextBox ID="tbxStartYM" runat="server" ReadOnly="true" CssClass="searchtext form-control edu-month-input" aria-label="시작 월" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false" autocomplete="off" />
                        <button type="button" class="edu-month-trigger" data-month-input="tbxStartYM" aria-label="시작 월 달력 열기" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false"><svg width="15" height="15" viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.3" aria-hidden="true" focusable="false"><rect x="1.5" y="3" width="13" height="11.5" rx="1" /><path d="M4.5 1v4M11.5 1v4M2 6.5h12M5 9h2M9 9h2M5 12h2M9 12h2" /></svg></button>
                    </span>
                    <span>~</span>
                    <span class="edu-month-field">
                        <asp:TextBox ID="tbxEndYM" runat="server" ReadOnly="true" CssClass="searchtext form-control edu-month-input" aria-label="종료 월" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false" autocomplete="off" />
                        <button type="button" class="edu-month-trigger" data-month-input="tbxEndYM" aria-label="종료 월 달력 열기" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false"><svg width="15" height="15" viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.3" aria-hidden="true" focusable="false"><rect x="1.5" y="3" width="13" height="11.5" rx="1" /><path d="M4.5 1v4M11.5 1v4M2 6.5h12M5 9h2M9 9h2M5 12h2M9 12h2" /></svg></button>
                    </span>
                </div>
                <div class="edu-search-field edu-target-field">
                    <label for="ddlTarget">대상</label>
                    <asp:DropDownList ID="ddlTarget" runat="server" CssClass="searchselect form-control"><asp:ListItem Text="전체" Value="A" /><asp:ListItem Text="부서" Value="G" /></asp:DropDownList>
                    <input type="text" id="targetNames" readonly="readonly" class="searchtext form-control" placeholder="조직도에서 부서를 선택해 주세요." aria-label="선택한 부서" style="display:none;" />
                    <cc3:ClientSmartButton ID="cbtnSelectTargets" runat="server" Class="btn" Text="선택" />
                </div>
                <div class="edu-search-field"><label for="ddlGubun">분류</label><asp:DropDownList ID="ddlGubun" runat="server" CssClass="searchselect form-control" /></div>
                <div class="edu-search-field edu-course-status-field"><label for="ddlCourseStatus">진행상태</label><asp:DropDownList ID="ddlCourseStatus" runat="server" CssClass="searchselect form-control"><asp:ListItem Text="전체" Value="" /><asp:ListItem Text="대기" Value="1" /><asp:ListItem Text="진행" Value="2" /><asp:ListItem Text="완료" Value="3" /><asp:ListItem Text="지연" Value="4" /></asp:DropDownList></div>
                <div class="edu-search-field"><label for="ctbSearchTitle">과정명</label><input type="text" id="ctbSearchTitle" class="searchtext form-control" maxlength="100" placeholder="과정명을 입력해 주세요." aria-label="과정명" /></div>
                <div class="edu-search-buttons"><div id="loading"></div><button type="button" id="cbtnReset" class="searchbtn btn-secondary">초기화</button><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary" Text="검색" /><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary" Text="신규등록" /></div>
            </div>
        </div>

        <h2 class="edu-dashboard-heading">운영 현황</h2>
        <div id="educationDashboardSummary" class="edu-dashboard-summary" aria-label="검색한 진행 현황">
            <div class="edu-summary-count total" data-status="" role="button" tabindex="0" aria-controls="phList" aria-pressed="true"><span>전체</span><strong><em id="summaryTotalCount">0</em>건</strong></div>
            <div class="edu-summary-count wait" data-status="1" role="button" tabindex="0" aria-controls="phList" aria-pressed="false"><span>대기</span><strong><em id="summaryWaitCount">0</em>건</strong></div>
            <div class="edu-summary-count progressing" data-status="2" role="button" tabindex="0" aria-controls="phList" aria-pressed="false"><span>진행</span><strong><em id="summaryProgressCount">0</em>건</strong></div>
            <div class="edu-summary-count complete" data-status="3" role="button" tabindex="0" aria-controls="phList" aria-pressed="false"><span>완료</span><strong><span class="edu-complete-person"><em id="summaryCompleteCount">0</em>건</span><span class="edu-complete-rate">(완료율 <em id="summaryCompleteRate">0</em>%)</span></strong></div>
            <div class="edu-summary-count delay" data-status="4" role="button" tabindex="0" aria-controls="phList" aria-pressed="false"><span>지연</span><strong><em id="summaryDelayCount">0</em>건</strong></div>
        </div>
        <div class="edu-course-heading">
            <h2 class="edu-dashboard-heading">진행률</h2>
            <div class="edu-course-navigation" role="group" aria-label="진행률 페이지 이동">
                <span id="coursePageInfo" class="edu-course-page-info" aria-live="polite" aria-atomic="true"></span>
                <button type="button" id="cbtnCoursePrevious" class="edu-course-page-button" disabled="disabled" aria-controls="phList" title="이전" aria-label="이전"><i class="fas fa-chevron-left" aria-hidden="true"></i></button>
                <button type="button" id="cbtnCourseNext" class="edu-course-page-button" disabled="disabled" aria-controls="phList" title="다음" aria-label="다음"><i class="fas fa-chevron-right" aria-hidden="true"></i></button>
            </div>
        </div>
        <div class="edu-progress-legend" aria-label="진행상태 색상 안내">
            <span><i class="edu-legend-color complete" aria-hidden="true"></i>완료</span>
            <span><i class="edu-legend-color wait" aria-hidden="true"></i>대기</span>
            <span><i class="edu-legend-color progressing" aria-hidden="true"></i>진행</span>
            <span><i class="edu-legend-color delay" aria-hidden="true"></i>지연</span>
        </div>

        <div id="layout_content">
        	<div class="row">
				<div class="col-12 content-area-body">
                    <div id="phList" style="clear:both;"></div>
		        </div>
                    
			</div>
		</div>
        
	</div>
</div>
        <div id="eduMonthPicker" class="edu-month-picker" role="dialog" aria-labelledby="eduMonthPickerTitle">
            <div class="edu-month-picker-header">
                <button type="button" id="eduMonthPreviousYear" aria-label="이전 연도">&#8249;</button>
                <strong id="eduMonthPickerTitle" aria-live="polite"></strong>
                <button type="button" id="eduMonthNextYear" aria-label="다음 연도">&#8250;</button>
            </div>
            <div class="edu-month-grid" role="group" aria-label="월 선택"></div>
        </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">

        var HasAccess = <%=HasAccess ? "true" : "false"%>;
        var InitialSearch = <%=InitialSearchJson%>;
        var DefaultSearchMonth = '<%=DefaultSearchMonth%>';
        var DefaultSearchStartMonth = '<%=HttpUtility.JavaScriptStringEncode(DefaultSearchStartMonth)%>';

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Admin/DashBoard.js?v=3"></script>
</body>
</html>
