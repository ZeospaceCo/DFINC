<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Attendance.Add" %>

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
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/<%=SkinType%>/navimobile.css" />
    <style>        
        .blue
        {
            color: Blue;
        }
        
        .red
        {
            color: Red;
        }
    </style>
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
                            <b><span id="hr"><%=Nowhr%></span> : <span id="minute"><%=Nowminute%></span> <span id="period"><%=Nowperiod%></span></b>
                        </div>
                        <div id="StateWork" style="text-align: center; font-size: 50px;">
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
                                                <button type="button" class="btn btn-lg btn-danger" style="cursor: pointer; float:none; width:120px;" id="OffWork">
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
                                    <textarea id="reason" class="form-control ta3"></textarea>
                                </dd>
                            </dl>
                        </div>
                        <div class="wr">
                            <dl class="wrc">
                                <dd>
                                    <span style="color: #9c9c9c;">▣<%=GetResource("Attendance_GotoWork")%>:</span><span
                                        id="attendanceGotoWork" style="padding-left: 5px;"></span>
                                </dd>
                            </dl>
                        </div>
                        <div class="wr">
                            <dl class="wrc">
                                <dd>
                                    <span style="color: #9c9c9c;">▣<%=GetResource("Attendance_OffWork")%>:</span><span
                                        id="attendanceOffWork" style="padding-left: 5px;"></span>
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
        var rtnAttendanceData = "<%=rtnAttendanceData%>"; 
        var NowHour = "<%=NowHour%>"; 
        var BtnClickCheck = "";                   
        var bWS = "";
        var WSTime = "";
        var bWC = "";
        var WCTime = "";

        var R_Attendance_Early = "<%=GetResource("Attendance_Early") %>";   
        var R_Attendance_Beforework = "<%=GetResource("Attendance_Beforework") %>";   
        var R_Attendance_Onduty = "<%=GetResource("Attendance_Onduty") %>";   
        var R_Attendance_OffWork = "<%=GetResource("Attendance_OffWork") %>";

        var R_Menu = "<%=GetResource("Menu")%>";  
                     
        var R_Attendance_Reason = "<%=GetResource("Attendance_Reason") %>";     
        var R_Attendance_OffWorkConfirm = "<%=GetResource("Attendance_OffWorkConfirm") %>";     
        var R_Attendance_OffWorkRe = "<%=GetResource("Attendance_OffWorkRe") %>";     
        var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>";     
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";

 
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Attendance/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Task/AddReply.js");
    </script>
</body>
</html>
