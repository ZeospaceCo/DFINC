<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attendance.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.HR.Attendance" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="AttendanceAdd">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span>
                            <i class="fas fa-user dropbtn "></i><%=GetResource("Attendance")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                </ul>
                <ul class="mobilecontent to8" style="display:none;">
                    <li class="contentblock" runat="server" id="dvAdd">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" class="btn btn-lg btn-primary" id="btCancel">
                                    <span class="btnb">GetResource("Cancel")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobilecontent">
                    <li class="title full-width">
                        <div style="text-align: center; font-size: x-large">
                            <b><%=NowDate%></b>
                        </div>
                        <div style="text-align: center; font-size: x-large">
                            <b><span id="hr"><%=Nowhr%></span> : <span id="minute"><%=Nowmin%></span> <span id="period"><%=Nowperiod%></span></b>
                        </div>
                        <div id="StateWork" style="text-align: center; font-size: 50px;" class="text-primary">
                            <b><%=GetResource("Attendance_Beforework") %></b>
                        </div>
                        <div class="wr">
                            <dl class="wrc">
                                <dd>
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 99%;">
                                        <tr>
                                            <td style="text-align: center;">
                                                <button type="button" class="btn btn-lg btn-primary" style="cursor: pointer; float:none; width:120px;" id="GotoWork">
                                                    <%=GetResource("Attendance_GotoWork")%></button>
                                            </td>
                                            <td style="text-align: center;">
                                                <button type="button" class="btn btn-lg btn-danger" style="cursor: pointer; float:none; width:120px;" id="HROffWork">
                                                    <%=GetResource("Attendance_OffWork")%></button>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                            </dl>
                        </div>
                        <br />
                        <div class="wr">
                            <dl class="wrc">
                                <dd>
                                    <textarea id="reason" class="form-control ta3" style="height: 7em"></textarea>
                                </dd>
                            </dl>
                        </div>
                        <div class="wr">
                            <dl class="wrc" style="margin-bottom: 1.7rem;">
                                <dd>
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 99%;">
                                        <tr>
                                            <td style="width: 60%">
                                                <span style="color: #9c9c9c;">▶&nbsp;<%=GetResource("Attendance_GotoWork")%>&nbsp;-</span><span
                                                    id="attendanceGotoWork" style="padding-left: 5px;"></span>
                                            </td>
                                            <td>
                                                <span id="spOffWork" style="display: none;">
                                                    <span style="color: #9c9c9c;">▶&nbsp;<%=GetResource("Attendance_OffWork")%>&nbsp;-</span><span
                                                        id="attendanceOffWork" style="padding-left: 5px;"></span>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                            </dl>
                        </div>
                        <div class="wr" id="trEarly" style="display: none">
                            <dl class="wrc" style="margin-bottom: 1.7rem;">
                                <dd>
                                    <span style="color:#9c9c9c;">▶&nbsp;<%=GetResource("Attendance_Early")%>&nbsp;-</span>&nbsp;<asp:DropDownList ID="ddlEarly" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                </dd>
                            </dl>
                        </div>
                        <div class="wr" id="trEarlyTime" style="display: none">
                            <dl class="wrc" style="margin-bottom: 1.7rem;">
                                <dd>
                                    <span style="color:#9c9c9c;">▶&nbsp;<%=GetResource("Attendance_Early")%>&nbsp;-</span><span id="spEarlyTime" style="padding-left:5px;"></span>
                                    &nbsp;&nbsp;<button type="button" class="btn btn-sm btn-danger" style="cursor:pointer; float:none;" id="btnEarlyCancel"><%=GetResource("Cancel")%></button>
                                </dd>
                            </dl>
                        </div>
                        <div class="wr">
                            <dl class="wrc">
                                <dd>
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 99%;">
                                        <tr>
                                            <td style="width: 60%">
                                                <span style="color:#9c9c9c;">▶&nbsp;<%=GetResource("WeekOverTime")%></span><br />
                                                <span style="color:#9c9c9c; font-size: 17px; padding-left: 25px"><%=GetResource("WeekOverTimeMsg")%></span>
                                            </td>
                                            <td>
                                                <span style="font-size: 25pt"><span id="spOverTime" style="color:#dc3545; padding-left:5px;"></span>&nbsp;/&nbsp;12</span>&nbsp;<span style="color:#9c9c9c;"><%=GetResource("Time")%></span>
                                            </td>
                                        </tr>
                                    </table>       
                                </dd>
                            </dl>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";    
        var NowHour = "<%=NowHour%>"; 
        var NowMinute = "<%=NowMinute%>";
        var BtnClickCheck = "";  

        var bWS = "<%=bWS%>";
        var WSTime = "<%=WSTime%>";
        var bWC = "<%=bWC%>";
        var WCTime = "<%=WCTime%>";    
        var WCTimeHour = "<%=WCTimeHour%>"; // 퇴근시간 Hour
        var WCTimeMinute = "<%=WCTimeMinute%>"; // 퇴근시간 Minute

        var bWSTimeHour = "<%=bWSTimeHour%>"; // 정규 출근시간 Hour
        var bWCTimeHour = "<%=bWCTimeHour%>"; // 정규 퇴근시간 Hour
        var bWSTimeMinute = "<%=bWSTimeMinute%>"; // 정규 출근시간 Minute
        var bWCTimeMinute = "<%=bWCTimeMinute%>"; // 정규 퇴근시간 Minute

        var isHoli = "<%=isHoli%>"; //휴일 여부
        var OverTime = "<%=OverTime%>"; //주간 연장근무시간

        var late = "<%=late%>"; //포탈에서 지각일 경우 '1'
        var early = "<%=early%>"; //포탈에서 조퇴일 경우 '1'

        var Attendance_OffWork = "<%=GetResource("Attendance_OffWork") %>"; // 퇴근
        var Attendance_Early = "<%=GetResource("Attendance_Early") %>";   // 조퇴
        var Attendance_Reason = "<%=GetResource("Attendance_Reason") %>";  // 사유를 입력 하시기 바랍니다.
        var Attendance_NotGotoWork = "<%=GetResource("Attendance_NotGotoWork") %>"; // 출근 상태가 아닙니다.
        var Attendance_GotoWorkRe = "<%=GetResource("Attendance_GotoWorkRe") %>"; // 출근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
        var Attendance_OffWorkConfirm = "<%=GetResource("Attendance_OffWorkConfirm") %>"; // 퇴근 처리 하시겠습니까?
        var Attendance_OffWorkRe = "<%=GetResource("Attendance_OffWorkRe") %>";  // 퇴근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.

        var Attendance_EarlyConfirm = "<%=GetResource("Attendance_EarlyConfirm") %>"; // 조퇴 처리 하시겠습니까?
        var Attendance_EarlyRe = "<%=GetResource("Attendance_EarlyRe") %>"; // 조퇴 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
        var Attendance_EarlyTimeMsg = "<%=GetResource("Attendance_EarlyTimeMsg") %>"; // 조퇴 예정시간을 선택하세요.
        var Attendance_EarlyCancelConfirm = "<%=GetResource("Attendance_EarlyCancelConfirm") %>"; // 취소 처리 하시겠습니까?

        var R_Attendance_Beforework = "<%=GetResource("Attendance_Beforework") %>";  // 출근전
        var R_Attendance_Onduty = "<%=GetResource("Attendance_Onduty") %>";  // 근무중

        var R_LocationIn = "<%=GetResource("LocationIn") %>";  // 사내
        var R_LocationOut = "<%=GetResource("LocationOut") %>";  // 사외
        var R_Location = "<%=GetResource("Location") %>";  // 위치
        var R_GotoTime = "<%=GetResource("GotoTime") %>";  // 출근시간
        var R_GotoSucceed = "<%=GetResource("GotoSucceed") %>";  // 출근 처리되었습니다.
        var R_OffWorkTime = "<%=GetResource("OffWorkTime") %>";  // 퇴근시간
        var R_OffWorkSucceed = "<%=GetResource("OffWorkSucceed") %>";  // 퇴근 처리되었습니다.
        var R_EarlyTime = "<%=GetResource("EarlyTime") %>";  // 조퇴시간
        var R_EarlySucceed = "<%=GetResource("EarlySucceed") %>";  // 조퇴 처리되었습니다.

        var R_Menu = "<%=GetResource("Menu")%>";
        var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>";     
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/HR/Attendance.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/HR/Attendance.js");
    </script>
</body>
</html>
