<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Admin.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <style type="text/css">
        #cbtnReset { box-sizing: border-box; height: 30px; margin: 0; flex-shrink: 0; white-space: nowrap; }
        #layout_srh { box-sizing: border-box; height: auto; padding: 14px 20px; }
        #layout_srh .edu-search-grid { display: flex; flex-direction: column; gap: 8px; width: 100%; margin-left: 0; }
        #layout_srh .edu-search-row { display: flex; align-items: center; flex-wrap: wrap; gap: 8px 18px; min-width: 0; }
        #layout_srh .edu-search-field { display: flex; align-items: center; gap: 6px; min-width: 0; min-height: 30px; }
        #layout_srh .edu-date-field { flex: 0 0 280px; width: 280px; }
        #layout_srh .edu-user-field { flex: 0 0 212px; width: 212px; }
        #layout_srh .edu-title-field { flex: 0 1 300px; width: 300px; }
        #layout_srh .edu-search-label { flex: 0 0 auto; width: auto; margin: 0; color: #333; white-space: nowrap; }
        #layout_srh .edu-search-value { display: flex; align-items: center; gap: 6px; flex: 1; min-width: 0; }
        #layout_srh select, #layout_srh input[type="text"] { box-sizing: border-box; height: 30px; margin: 0; min-width: 0; }
        #layout_srh .edu-search-grid select.searchselect { padding-left: 6px; padding-right: 4px; }
        #layout_srh .edu-search-value > select, #layout_srh .edu-search-value > input[type="text"] { flex: 1; width: 100%; }
        #layout_srh #ddlSearchUserField { flex: 0 0 80px; width: 80px; }
        #layout_srh #ddlItem, #layout_srh #ddlStatus { flex: 0 0 80px; width: 80px; }
        #layout_srh #ctbSearchUser { max-width: 126px; }
        #layout_srh #ctbSearchTitle { max-width: 240px; }
        #layout_srh .edu-date-range { display: grid; grid-template-columns: minmax(0, 1fr) auto minmax(0, 1fr); align-items: center; gap: 6px; flex: 1; min-width: 0; }
        #layout_srh .edu-month-field { position: relative; display: inline-flex; align-items: center; min-width: 0; }
        #layout_srh .edu-month-input { width: 100%; padding: 0 28px 0 6px; background: #fff; cursor: pointer; }
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
        #layout_srh .edu-target-field { flex: 0 1 auto; }
        #layout_srh .edu-target-field .edu-search-value { flex: 0 1 auto; }
        #layout_srh #ddlSearchTarget { flex: 0 0 80px; width: 80px; }
        #layout_srh #targetNames { flex: 1 1 240px; width: 240px; max-width: 240px; min-width: 0; color: #555; background: #fff; cursor: default; }
        #layout_srh #ddlEmployment, #layout_srh #ddlComplete, #layout_srh #ddlConfirm { flex: 0 0 80px; width: 80px; }
        #layout_srh .edu-search-actions { display: flex; align-items: center; justify-content: flex-start; gap: 8px; flex: 0 0 auto; margin-left: 0; }
        #layout_srh .edu-search-buttons { display: flex; align-items: center; gap: 6px; }
        #layout_srh .edu-search-buttons a, #layout_srh #cbtnSelectTargets { box-sizing: border-box; display: inline-flex; align-items: center; justify-content: center; flex-shrink: 0; height: 30px; margin: 0; }
        #layout_srh #cbtnDashBoard { width: auto; min-width: 100px; padding: 0 12px; white-space: nowrap; }
        #layout_srh .edu-disabled { opacity: .55; cursor: default; }
        #phList { overflow-x: auto; }
        #phList .table_typeA { width: 100%; min-width: 1500px; table-layout: fixed; }
        #phList .table_typeA td { vertical-align: middle; overflow-wrap: break-word; }
        #phList .edu-title-link { display: block; line-height: 20px; }
        #phList [action], #layout_srh a, #cbtnExcel { cursor: pointer; }
        #phList input:disabled { cursor: default; }
        @media (max-width: 680px) {
            #layout_srh { padding: 12px; }
            #layout_srh .edu-search-grid, #layout_srh .edu-search-row { gap: 10px; }
            #layout_srh .edu-user-field, #layout_srh .edu-title-field, #layout_srh .edu-target-field { flex-basis: 100%; width: 100%; }
            #layout_srh .edu-target-field .edu-search-value { flex: 1; flex-wrap: wrap; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="layout_body">
            <div class="layout_full">
                <div id="layout_header">
                    <h1>사용자 리스트</h1>
                </div>
                <div id="layout_srh" aria-label="사용자 검색">
                    <div class="edu-search-grid">
                        <div class="edu-search-row edu-primary-search">
                            <div class="edu-search-field edu-date-field">
                                <label for="startDate" class="edu-search-label">시작년/월</label>
                                <div class="edu-date-range">
                                    <span class="edu-month-field">
                                        <input type="text" id="startDate" class="searchtext form-control edu-month-input" readonly="readonly" autocomplete="off" aria-label="시작 월" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false" />
                                        <button type="button" class="edu-month-trigger" data-month-input="startDate" aria-label="시작 월 달력 열기" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false"><svg width="15" height="15" viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.3" aria-hidden="true" focusable="false"><rect x="1.5" y="3" width="13" height="11.5" rx="1" /><path d="M4.5 1v4M11.5 1v4M2 6.5h12M5 9h2M9 9h2M5 12h2M9 12h2" /></svg></button>
                                    </span>
                                    <span>~</span>
                                    <span class="edu-month-field">
                                        <input type="text" id="endDate" class="searchtext form-control edu-month-input" readonly="readonly" autocomplete="off" aria-label="종료 월" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false" />
                                        <button type="button" class="edu-month-trigger" data-month-input="endDate" aria-label="종료 월 달력 열기" aria-haspopup="dialog" aria-controls="eduMonthPicker" aria-expanded="false"><svg width="15" height="15" viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.3" aria-hidden="true" focusable="false"><rect x="1.5" y="3" width="13" height="11.5" rx="1" /><path d="M4.5 1v4M11.5 1v4M2 6.5h12M5 9h2M9 9h2M5 12h2M9 12h2" /></svg></button>
                                    </span>
                                </div>
                            </div>
                            <div class="edu-search-field edu-target-field">
                                <label for="ddlSearchTarget" class="edu-search-label">대상</label>
                                <div class="edu-search-value">
                                    <asp:DropDownList ID="ddlSearchTarget" runat="server" CssClass="searchselect form-control">
                                        <asp:ListItem Text="전체" Value="A" /><asp:ListItem Text="부서" Value="G" />
                                    </asp:DropDownList>
                                    <input type="text" id="targetNames" class="searchtext form-control" readonly="readonly" aria-label="선택한 사용자 또는 부서" placeholder="조직도에서 선택해 주세요." style="display:none;" />
                                    <cc3:ClientSmartButton ID="cbtnSelectTargets" runat="server" Class="btn" Text="선택" />
                                </div>
                            </div>
                            <div class="edu-search-field">
                                <label for="ddlItem" class="edu-search-label">분류</label>
                                <div class="edu-search-value"><asp:DropDownList ID="ddlItem" runat="server" CssClass="searchselect form-control" /></div>
                            </div>
                            <div class="edu-search-field">
                                <label for="ddlStatus" class="edu-search-label">진행상태</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="searchselect form-control">
                                    <asp:ListItem Text="전체" Value="" />
                                    <asp:ListItem Text="대기" Value="1" />
                                    <asp:ListItem Text="진행" Value="2" />
                                    <asp:ListItem Text="완료" Value="3" />
                                    <asp:ListItem Text="지연" Value="4" />
                                </asp:DropDownList>
                            </div>
                            <div class="edu-search-actions">
                                <div id="loading"></div>
                                <div class="edu-search-buttons">
                                    <button type="button" id="cbtnReset" class="searchbtn btn-secondary">초기화</button>
                                    <cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary" Text="검색" />
                                    <cc3:ClientSmartButton ID="cbtnExcel" runat="server" Class="searchbtn btn-secondary" Text="엑셀" />
                                    <a id="cbtnDashBoard" class="searchbtn btn-secondary" href="<%=HttpUtility.HtmlAttributeEncode(ReturnDashBoardUrl)%>">전체현황이동</a>
                                </div>
                            </div>
                        </div>
                        <div class="edu-search-row edu-secondary-search">
                            <div class="edu-search-field edu-user-field">
                                <asp:DropDownList ID="ddlSearchUserField" runat="server" CssClass="searchselect form-control">
                                    <asp:ListItem Text="이름" Value="Name" /><asp:ListItem Text="아이디" Value="LogInID" />
                                </asp:DropDownList>
                                <div class="edu-search-value"><cc3:ClientSmartTextBox ID="ctbSearchUser" runat="server" MaxLength="100" InputMode="None" class="searchtext form-control" aria-label="사용자 검색어" /></div>
                            </div>
                            <div class="edu-search-field edu-title-field">
                                <label for="ctbSearchTitle" class="edu-search-label">과정명</label>
                                <div class="edu-search-value"><cc3:ClientSmartTextBox ID="ctbSearchTitle" runat="server" MaxLength="100" InputMode="None" class="searchtext form-control" placeholder="과정명을 입력해 주세요." /></div>
                            </div>
                            <div class="edu-search-field">
                                <label for="ddlEmployment" class="edu-search-label">재직여부</label>
                                <asp:DropDownList ID="ddlEmployment" runat="server" CssClass="searchselect form-control">
                                    <asp:ListItem Text="전체" Value="" />
                                    <asp:ListItem Text="재직" Value="1" />
                                    <asp:ListItem Text="퇴직" Value="0" />
                                </asp:DropDownList>
                            </div>
                            <div class="edu-search-field">
                                <label for="ddlComplete" class="edu-search-label">완료여부</label>
                                <asp:DropDownList ID="ddlComplete" runat="server" CssClass="searchselect form-control">
                                    <asp:ListItem Text="전체" Value="" />
                                    <asp:ListItem Text="완료" Value="3" />
                                    <asp:ListItem Text="미완료" Value="0" />
                                </asp:DropDownList>
                            </div>
                            <div class="edu-search-field">
                                <label for="ddlConfirm" class="edu-search-label">확인여부</label>
                                <asp:DropDownList ID="ddlConfirm" runat="server" CssClass="searchselect form-control">
                                    <asp:ListItem Text="전체" Value="" />
                                    <asp:ListItem Text="확인" Value="Y" />
                                    <asp:ListItem Text="미확인" Value="N" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="layout_content">
                    <div class="row">
                        <div class="col-12 content-area-body">
                            <div id="phList" style="clear: both;"></div>
                            <!-- 페이징 시작 //-->
                            <div id="Pager1" name="Pager1" class="paging_num_s1">
                                <h3><span id="pageInfo">0 / 0</span></h3><span id="pager"></span>
                                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                                    <asp:ListItem Text="5" Value="5" />
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="20" Value="20" />
                                    <asp:ListItem Text="30" Value="30" />
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                    <asp:ListItem Text="150" Value="150" />
                                    <asp:ListItem Text="200" Value="200" />
                                    <asp:ListItem Text="250" Value="250" />
                                </asp:DropDownList>
                            </div>
                            <!-- 페이징 끝 //-->
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
    <div id="zeoModalContact">
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <form id="educationExportForm" method="post" action="/DeskPlusEIP/Education/Admin/ExcelDown.aspx" target="educationExportFrame" style="display:none;"><input type="hidden" id="exportParameters" name="p" /></form>
    <iframe id="educationExportFrame" name="educationExportFrame" src="about:blank" style="display:none;" title="목록 엑셀 다운로드"></iframe>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var InitialSearch = <%=InitialSearchJson%>;
        var DefaultSearchMonth = "<%=DefaultSearchMonth%>";
        var DefaultSearchStartMonth = "<%=HttpUtility.JavaScriptStringEncode(DefaultSearchStartMonth)%>";
        var CurrentPage = "<%=CurrentPage%>";
        var TotalItemCount = 0;
        var RecordPerPage = "<%=RecordPerPage%>";
        var R_Previous = "이전";
        var R_Next = "다음";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Admin/List.js?v=3"></script>
</body>
</html>
