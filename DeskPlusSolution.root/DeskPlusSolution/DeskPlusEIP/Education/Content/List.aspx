<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Content.List" %>
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
        #cbtnReset { box-sizing: border-box; height: 30px; margin: 0; white-space: nowrap; }
        #layout_header {
            border-bottom: 0 !important;
        }

        #layout_srh .edu-course-search {
            display: inline-flex;
            align-items: center;
            gap: 8px;
        }

        #layout_srh .edu-filter-field {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            margin-right: 8px;
            vertical-align: middle;
        }

        #layout_srh .edu-filter-field select {
            margin: 0;
        }

        #layout_srh .edu-filter-field label,
        #layout_srh .edu-course-search label {
            margin: 0;
            color: #333;
            white-space: nowrap;
        }

        #layout_srh .edu-course-search #ctbSearch {
            box-sizing: border-box;
            height: 30px;
            margin: 0;
        }

        #layout_srh .edu-date-field { display: inline-flex; align-items: center; gap: 6px; width: 280px; max-width: 100%; vertical-align: middle; }
        #layout_srh .edu-date-field > label { flex: 0 0 auto; margin: 0; color: #333; white-space: nowrap; }
        #layout_srh .edu-date-range { display: grid; grid-template-columns: minmax(0, 1fr) auto minmax(0, 1fr); align-items: center; gap: 6px; flex: 1; min-width: 0; }
        #layout_srh .edu-month-field { position: relative; display: inline-flex; align-items: center; min-width: 0; }
        #layout_srh .edu-month-input { box-sizing: border-box; width: 100%; height: 30px; min-width: 0; margin: 0; padding: 0 28px 0 6px; background: #fff; cursor: pointer; }
        #layout_srh .edu-month-trigger { position: absolute; top: 1px; right: 1px; display: inline-flex; align-items: center; justify-content: center; width: 26px; height: 28px; padding: 0; border: 0; background: transparent; color: #3978da; cursor: pointer; }
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

        .edu-summary {
            display: -ms-flexbox;
            display: flex;
            margin: 0 30px 14px;
            border: 1px solid #dfe3e8;
            border-radius: 5px;
            background: #fff;
            box-sizing: border-box;
            overflow: hidden;
        }

        .edu-summary-progress {
            width: 50%;
            padding: 16px 20px;
            box-sizing: border-box;
        }

        .edu-summary-title {
            margin: 0 0 12px;
            color: #333;
            font-size: 14px;
        }

        #educationSummary .edu-summary-title strong {
            color: #343a40 !important;
            font-size: 16px;
        }

        .edu-progress-row {
            display: -ms-flexbox;
            display: flex;
            -ms-flex-align: center;
            align-items: center;
        }

        .edu-progress-label {
            width: 58px;
            color: #333;
            font-weight: bold;
            font-size: 12px;
        }

        .edu-progress-track {
            position: relative;
            -ms-flex: 1;
            flex: 1;
            height: 10px;
            border-radius: 5px;
            background: #edf0f3;
            overflow: hidden;
        }

        .edu-progress-bar {
            width: 0;
            height: 100%;
            border-radius: 5px;
            background: #343a40;
            transition: width .25s ease;
        }

        #educationSummary .edu-progress-rate {
            width: 48px;
            color: #343a40 !important;
            text-align: right;
            font-size: 12px;
        }

        .edu-summary-status {
            display: -ms-flexbox;
            display: flex;
            width: 50%;
            border-left: 1px solid #e5e7eb;
            box-sizing: border-box;
        }

        .edu-summary-donut-wrap {
            display: -ms-flexbox;
            display: flex;
            -ms-flex: 0 0 116px;
            flex: 0 0 116px;
            -ms-flex-align: center;
            align-items: center;
            -ms-flex-pack: center;
            justify-content: center;
            padding: 8px 10px;
            border-right: 1px solid #f0f1f3;
            box-sizing: border-box;
        }

        .edu-summary-donut {
            position: relative;
            width: 78px;
            height: 78px;
        }

        .edu-summary-donut svg {
            display: block;
            width: 100%;
            height: 100%;
        }

        .edu-donut-ring {
            fill: none;
            stroke-width: 12;
        }

        .edu-donut-ring.background { stroke: #edf0f3; }
        .edu-donut-ring.wait { stroke: #75ace9; }
        .edu-donut-ring.progressing { stroke: #8bc63e; }
        .edu-donut-ring.complete { stroke: #343a40; }
        .edu-donut-ring.delay { stroke: #9e9080; }

        .edu-donut-center {
            position: absolute;
            top: 50%;
            left: 50%;
            width: 52px;
            color: #555;
            text-align: center;
            -ms-transform: translate(-50%, -50%);
            transform: translate(-50%, -50%);
        }

        .edu-donut-center span {
            display: block;
            font-size: 10px;
        }

        .edu-donut-center strong {
            display: block;
            margin-top: 2px;
            color: #111827;
            font-size: 13px;
        }

        .edu-donut-center em {
            color: inherit !important;
            font-style: normal;
        }

        .edu-summary-counts {
            display: -ms-flexbox;
            display: flex;
            -ms-flex: 1;
            flex: 1;
            width: auto;
            min-width: 0;
            margin: 0;
            padding: 0;
            list-style: none;
            box-sizing: border-box;
        }

        .edu-summary-count {
            position: relative;
            display: -ms-flexbox;
            display: flex;
            -ms-flex: 1;
            flex: 1;
            -ms-flex-direction: column;
            flex-direction: column;
            -ms-flex-pack: center;
            justify-content: center;
            min-width: 0;
            padding: 12px 4px;
            text-align: center;
            box-sizing: border-box;
            cursor: pointer;
            transition: background-color .15s ease;
        }

        .edu-summary-count:hover,
        .edu-summary-count:focus {
            outline: 0;
            background: #f7f9fc;
        }

        .edu-summary-count:focus {
            box-shadow: inset 0 0 0 2px #c9d5e5;
        }

        .edu-summary-count.filter-active:after {
            position: absolute;
            right: 20%;
            bottom: 0;
            left: 20%;
            height: 3px;
            content: "";
        }

        .edu-summary-count.total.filter-active:after { background: #111827; }
        .edu-summary-count.wait.filter-active:after { background: #75ace9; }
        .edu-summary-count.progressing.filter-active:after { background: #8bc63e; }
        .edu-summary-count.complete.filter-active:after { background: #343a40; }
        .edu-summary-count.delay.filter-active:after { background: #9e9080; }

        .edu-summary-count + .edu-summary-count {
            border-left: 1px solid #f0f1f3;
        }

        .edu-summary-count > span {
            color: #333;
            font-weight: bold;
            font-size: 12px;
        }

        .edu-summary-count.wait > span,
        .edu-summary-count.progressing > span,
        .edu-summary-count.complete > span,
        .edu-summary-count.delay > span {
            -ms-flex-item-align: center;
            align-self: center;
            padding: 3px 8px;
            border-radius: 2px;
            color: #fff;
            line-height: 18px;
            white-space: nowrap;
        }

        .edu-summary-count.wait > span { background: #75ace9; }
        .edu-summary-count.progressing > span { background: #8bc63e; }
        .edu-summary-count.complete > span { background: #343a40; }
        .edu-summary-count.delay > span { background: #9e9080; }

        .edu-summary-count strong {
            margin-top: 7px;
            color: #111827;
            font-size: 13px;
        }

        .edu-summary-count em {
            color: inherit !important;
            font-style: normal;
        }

        .edu-summary-count.total strong { color: #111827; }
        .edu-summary-count.wait strong { color: #75ace9; }
        .edu-summary-count.progressing strong { color: #8bc63e; }
        .edu-summary-count.complete strong { color: #343a40; }
        .edu-summary-count.delay strong { color: #9e9080; }

        @media (max-width: 900px) {
            .edu-summary {
                display: block;
            }

            .edu-summary-progress,
            .edu-summary-status {
                width: 100%;
            }

            .edu-summary-status {
                border-top: 1px solid #e5e7eb;
                border-left: 0;
            }
        }

        @media (max-width: 560px) {
            .edu-summary {
                margin-right: 12px;
                margin-left: 12px;
            }

            .edu-summary-status {
                display: block;
            }

            .edu-summary-donut-wrap {
                width: 100%;
                padding: 12px 10px;
                border-right: 0;
                border-bottom: 1px solid #f0f1f3;
            }

            .edu-summary-counts {
                width: 100%;
                -ms-flex-wrap: wrap;
                flex-wrap: wrap;
            }

            .edu-summary-count {
                -ms-flex: 0 0 33.333%;
                flex: 0 0 33.333%;
            }

            .edu-summary-count:nth-child(4) {
                border-left: 0;
            }

            .edu-summary-count:nth-child(n+4) {
                -ms-flex: 0 0 50%;
                flex: 0 0 50%;
                border-top: 1px solid #f0f1f3;
            }
        }
    </style>
</head>
<body>
   <form id="form1" runat="server">
<div id="layout_body">

    <div class="layout_full">
    
        <div id="layout_header">            
            <h1>교육리스트</h1>
        </div>

        <div id="educationSummary" class="edu-summary" aria-label="진행 현황">
            <div class="edu-summary-progress">
                <p class="edu-summary-title">
                    전체 <strong id="summaryTotalText">0</strong>건 중
                    <strong id="summaryCompleteText">0</strong>건 완료
                    (<strong id="summaryRateText">0</strong>%)
                </p>
                <div class="edu-progress-row">
                    <span class="edu-progress-label">진행률</span>
                    <div class="edu-progress-track">
                        <div id="educationProgressBar" class="edu-progress-bar" role="progressbar"
                            aria-label="완료 진행률" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"></div>
                    </div>
                    <span id="educationProgressRate" class="edu-progress-rate">0%</span>
                </div>
            </div>
            <div class="edu-summary-status">
                <div class="edu-summary-donut-wrap">
                    <div id="educationStatusDonut" class="edu-summary-donut" role="img"
                        aria-label="상태 분포: 전체 0건, 대기 0건, 진행 0건, 완료 0건, 지연 0건">
                        <svg viewBox="0 0 100 100" aria-hidden="true" focusable="false">
                            <g transform="rotate(-90 50 50)">
                                <circle class="edu-donut-ring background" cx="50" cy="50" r="37"></circle>
                                <circle id="educationDonutWait" class="edu-donut-ring wait" cx="50" cy="50" r="37"
                                    stroke-dasharray="0 232.478" stroke-dashoffset="0"></circle>
                                <circle id="educationDonutProgress" class="edu-donut-ring progressing" cx="50" cy="50" r="37"
                                    stroke-dasharray="0 232.478" stroke-dashoffset="0"></circle>
                                <circle id="educationDonutComplete" class="edu-donut-ring complete" cx="50" cy="50" r="37"
                                    stroke-dasharray="0 232.478" stroke-dashoffset="0"></circle>
                                <circle id="educationDonutDelay" class="edu-donut-ring delay" cx="50" cy="50" r="37"
                                    stroke-dasharray="0 232.478" stroke-dashoffset="0"></circle>
                            </g>
                        </svg>
                        <div class="edu-donut-center">
                            <span>전체</span>
                            <strong><em id="summaryDonutTotalCount">0</em>건</strong>
                        </div>
                    </div>
                </div>
                <ul class="edu-summary-counts">
                    <li class="edu-summary-count total" data-status="" role="button" tabindex="0" aria-pressed="false"><span>전체</span><strong><em id="summaryTotalCount">0</em>건</strong></li>
                    <li class="edu-summary-count wait" data-status="1" role="button" tabindex="0" aria-pressed="false"><span>대기</span><strong><em id="summaryWaitCount">0</em>건</strong></li>
                    <li class="edu-summary-count progressing" data-status="2" role="button" tabindex="0" aria-pressed="false"><span>진행</span><strong><em id="summaryProgressCount">0</em>건</strong></li>
                    <li class="edu-summary-count complete" data-status="3" role="button" tabindex="0" aria-pressed="false"><span>완료</span><strong><em id="summaryCompleteCount">0</em>건</strong></li>
                    <li class="edu-summary-count delay" data-status="4" role="button" tabindex="0" aria-pressed="false"><span>지연</span><strong><em id="summaryDelayCount">0</em>건</strong></li>
                </ul>
            </div>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">

                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm">
                        <span class="edu-filter-field">
                            <label for="ddlItem">분류</label>
                            <asp:DropDownList ID="ddlItem" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="edu-filter-field">
                            <label for="ddlComplete">완료여부</label>
                            <asp:DropDownList ID="ddlComplete" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="edu-filter-field">
                            <label for="ddlStatus">진행상태</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </span>
                        <span class="edu-date-field">
                            <label for="startDate">시작년/월</label>
                            <span class="edu-date-range">
                                <span class="edu-month-field">
                                    <input type="text" id="startDate" class="searchtext form-control edu-month-input" value="<%=HttpUtility.HtmlAttributeEncode(SearchStartMonth)%>" readonly="readonly" autocomplete="off" aria-label="시작 월" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false" />
                                    <button type="button" class="edu-month-trigger" data-month-input="startDate" aria-label="시작 월 달력 열기" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false"><svg width="15" height="15" viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.3" aria-hidden="true" focusable="false"><rect x="1.5" y="3" width="13" height="11.5" rx="1" /><path d="M4.5 1v4M11.5 1v4M2 6.5h12M5 9h2M9 9h2M5 12h2M9 12h2" /></svg></button>
                                </span>
                                <span>~</span>
                                <span class="edu-month-field">
                                    <input type="text" id="endDate" class="searchtext form-control edu-month-input" value="<%=HttpUtility.HtmlAttributeEncode(SearchEndMonth)%>" readonly="readonly" autocomplete="off" aria-label="종료 월" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false" />
                                    <button type="button" class="edu-month-trigger" data-month-input="endDate" aria-label="종료 월 달력 열기" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false"><svg width="15" height="15" viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.3" aria-hidden="true" focusable="false"><rect x="1.5" y="3" width="13" height="11.5" rx="1" /><path d="M4.5 1v4M11.5 1v4M2 6.5h12M5 9h2M9 9h2M5 12h2M9 12h2" /></svg></button>
                                </span>
                            </span>
                        </span>
                    </div>
                </li>                
                <li class="edu-course-search">
                    <input type="text" id="dummy" style="display: none;" />
                    <label for="ctbSearch">과정명</label>
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="과정명을 입력해 주세요."></cc3:ClientSmartTextBox>
                </li>
                <li><button type="button" id="cbtnReset" class="searchbtn btn-secondary">초기화</button></li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
        	<div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
            
                    <div id="phList" style="clear: both;"></div>
            
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1">
                        <h3>
                            <span id="pageInfo"></span>
                        </h3>
                        <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->

		        </div><!-- end col content-area-body-right -->
                    
			</div><!-- end row  -->
		</div><!-- end layout_content  -->
        
	</div><!-- end layout_full  -->
</div><!-- end layout_body  -->
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/

        var SearchItem = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchItem) %>";
        var SearchStatus = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchStatus) %>";
        var SearchComplete = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchComplete) %>"; 
        var SearchDateField = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchDateField) %>";   
        var SearchDateValue = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchDateValue) %>";       
        var SearchField = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchField) %>"; 
        var SearchValue = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchValue) %>"; 
        var DefaultSearchMonth = "<%=DefaultSearchMonth%>";
        var DefaultSearchStartMonth = "<%=DefaultSearchStartMonth%>";

        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "0"; //총게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수

        var R_Previous = "이전";
        var R_Next = "다음";

        var AttachFileInfoList = null;
        var WindowObjectReference = null; 
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; 

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Content/List.js?v=3"></script>
</body>
</html>
