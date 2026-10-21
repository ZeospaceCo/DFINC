<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Portal.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.Portal" %>

<%@ Register Src="Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta http-equiv="Expires" content="-1" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="No-Cache" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/<%=SkinType%>/navimobile.css" />
    <style type="text/css">
        body.mobile.mobilehome
        {           
            -webkit-background-size: cover;
            -moz-background-size: cover;
            -o-background-size: cover;
            background-size: auto;
            height: 100vh;
            background-attachment: scroll;
        }
    </style>
</head>
<body class="mobile mobilehome navihide scrollup">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="home">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock topcontent" id="mainpage">
                <ul class="mobilecontent mycard">
                    <li class="identity">
                        <div class="imgbox">
                            <%=PhotoImage%>
                        </div>
                        <span class="namespan" id="spanUserInfo" runat="server">
                            <div><%=UserInfoName%></div>
                        </span>
                        <span class="nameselect" id="selUserInfo" runat="server">
                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlUserInfo" AutoPostBack="false" onChange="Portal.SetBaseDept()"></asp:DropDownList>
                        </span>
                    </li>
                    <li class="date"><%=Today%>&nbsp;&nbsp;</li>
                    <%=TopPortal%>
                    <%=BottomPortal%>
                </ul>
            </div>
            <div class="introcontentblock bottomcontent">
                <!-- 받은 편지함 -->
                <div id="mpMail" runat="server">
                    <ul class="mobilecontent ">
                        <li class="title text-primary">
                            <i class="fas fa-envelope dropbtn "></i>
                            <a href="#" id="mailMore" class="text-primary"><%=GetResource("ReceiveMail")%></a>
                        </li>
                        <div id="phMail" style="clear: both;">
                        </div>
                    </ul>
                </div>
                <!-- //받은 편지함 -->
                <!-- 일정관리 -->
                <div id="mpPims" runat="server">
                    <ul class="mobilecontent ">
                        <li class="title text-primary">
                            <i class="fas fa-calendar dropbtn "></i>
                            <a href="#" id="pimMore" class="text-primary"><%=GetResource("Pims")%> ( Top 10 )</a> </li>
                            <div id="phScheduleToDo" style="clear: both;"></div>
                        <%--<div id="phSchedule" style="clear: both;">
                        </div>
                        <div id="phToDo" style="clear: both;">
                        </div>--%>
                    </ul>
                </div>
                <!-- //일정관리 -->
                <!-- 게시판 -->
                <div id="mpNotice" runat="server">
                    <ul class="mobilecontent ">
                        <li class="title text-primary">
                            <i class="fas fa-align-left dropbtn "></i>
                            <a href="#" id="aBoard" class="text-primary"><%=GetResource("Board")%></a>
                        </li>
                        <div id="phBoard" style="clear: both;">
                        </div>
                    </ul>
                </div>
                <!-- //게시판 -->
                <!-- 전자결재 -->
                <div id="mpApproval" runat="server">
                    <ul class="mobilecontent ">
                        <li class="title text-primary">
                            <i class="fas fa-file-signature  dropbtn "></i>
                            <a href="#" id="aApproval" class="text-primary"><%=GetResource("Approval")%></a>
                        </li>
                        <div id="phApproval" style="clear: both;">
                        </div>
                    </ul>
                </div>
                <!-- //전자결재 -->
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/jquery.cookie.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var NowYear = "<%=System.DateTime.Now.Year.ToString()%>";
        var NowMonth = "<%=System.DateTime.Now.Month.ToString()%>";
        var NowDay = "<%=System.DateTime.Now.Day.ToString()%>";
        var NowDate = "";
        var CompanyID = "<%=CompanyID%>"
        var UserID = "<%=UserID%>";    
        var MainCID = "<%=MainCID%>";
        var PCatID = "<%=PCatID%>";
        var NoticeCID = "<%=NoticeCID%>";
        var UUID = "<%=UUID%>";
        var mailCheck = "<%=mailCheck%>";
        var approvalCheck = "<%=approvalCheck%>";
        var noticeCheck = "<%=noticeCheck%>";
        var pimsCheck = "<%=pimsCheck%>";
        var attendCheck = "<%=attendCheck%>";
        var BadgeCnt = "<%=BadgeCnt%>";
        var MobileType = "<%=MobileType %>";

        var BtnClickCheck = "";
        var NowHour = "<%=NowHour%>"; 
        var NowMinute = "<%=NowMinute%>"; 
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

        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_NoTodaySchedule = "<%=GetResource("NoTodaySchedule") %>";
        var R_NoTodayToDo = "<%=GetResource("NoTodayToDo") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_Receive = "<%=GetResource("State_Receive")%>";
        var R_Refer = "<%=GetResource("Refer")%>";
        var R_Refer2 = "<%=GetResource("Refer2")%>";

        var Attendance_Early = "<%=GetResource("Attendance_Early") %>";   // 조퇴
        var Attendance_NotGotoWork = "<%=GetResource("Attendance_NotGotoWork") %>"; // 출근 상태가 아닙니다.
        var Attendance_GotoWorkRe = "<%=GetResource("Attendance_GotoWorkRe") %>"; // 출근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
        var Attendance_OffWorkConfirm = "<%=GetResource("Attendance_OffWorkConfirm") %>"; // 퇴근 처리 하시겠습니까?
        var Attendance_OffWorkRe = "<%=GetResource("Attendance_OffWorkRe") %>";  // 퇴근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
        var R_LocationIn = "<%=GetResource("LocationIn") %>";  // 사내
        var R_LocationOut = "<%=GetResource("LocationOut") %>";  // 사외
        var R_Location = "<%=GetResource("Location") %>";  // 위치
        var R_GotoTime = "<%=GetResource("GotoTime") %>";  // 출근시간
        var R_GotoSucceed = "<%=GetResource("GotoSucceed") %>";  // 출근 처리되었습니다.
        var R_OffWorkTime = "<%=GetResource("OffWorkTime") %>";  // 퇴근시간
        var R_OffWorkSucceed = "<%=GetResource("OffWorkSucceed") %>";  // 퇴근 처리되었습니다.
        var R_EarlyTime = "<%=GetResource("EarlyTime") %>";  // 조퇴시간
        var R_EarlySucceed = "<%=GetResource("EarlySucceed") %>";  // 조퇴 처리되었습니다.
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Portal/Portal.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Portal/Portal.js");
    </script>
</body>
</html>
