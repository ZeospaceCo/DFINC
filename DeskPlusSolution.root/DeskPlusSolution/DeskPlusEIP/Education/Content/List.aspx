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
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <style type="text/css">
        #layout_header {
            border-bottom: 0 !important;
        }

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

        .edu-summary-title strong {
            color: #2468c5;
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
            background: #3b82d0;
            transition: width .25s ease;
        }

        .edu-progress-rate {
            width: 48px;
            color: #555;
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

        <div id="educationSummary" class="edu-summary" aria-label="교육 진행 현황">
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
                            aria-label="교육 완료 진행률" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"></div>
                    </div>
                    <span id="educationProgressRate" class="edu-progress-rate">0%</span>
                </div>
            </div>
            <div class="edu-summary-status">
                <div class="edu-summary-donut-wrap">
                    <div id="educationStatusDonut" class="edu-summary-donut" role="img"
                        aria-label="교육 상태 분포: 전체 0건, 대기 0건, 진행중 0건, 완료 0건, 지연 0건">
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
                    <li class="edu-summary-count progressing" data-status="2" role="button" tabindex="0" aria-pressed="false"><span>진행중</span><strong><em id="summaryProgressCount">0</em>건</strong></li>
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
                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="searchselect form-control"></asp:DropDownList>                        
                        <asp:DropDownList ID="ddlComplete" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <asp:DropDownList ID="ddlSearchDateField" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Text="시작일" Value="StartDate"></asp:ListItem>
                            <asp:ListItem Text="종료일" Value="EndDate"></asp:ListItem>
                        </asp:DropDownList>
                        <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                        <asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Text="과정명" Value="Title"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </li>                
                <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                </li>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/

        var SearchItem = "<%=SearchItem %>";
        var SearchStatus = "<%=SearchStatus %>";
        var SearchComplete = "<%=SearchComplete %>"; 
        var SearchDateField = "<%=SearchDateField %>";   
        var SearchDateValue = "<%=SearchDateValue %>";       
        var SearchField = "<%=SearchField %>"; 
        var SearchValue = "<%=SearchValue %>"; 

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

         var R_PlaceholderText =  "검색어 입력";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <!--script type="text/javascript" src="/DeskPlusEIP/Education/Js/Content/List.js"></script-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Education/Js/Content/List.js");
    </script>
</body>
</html>
