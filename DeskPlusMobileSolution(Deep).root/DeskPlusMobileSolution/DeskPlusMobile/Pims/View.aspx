<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Pims.View" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=2.0, minimum-scale=0.5, user-scalable=yes, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loadingH">
    </div>
    <div data-role="page" id="MailView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-calendar  dropbtn "></i><%=GetResource("PimsView")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock" runat="server" id="dvAdd">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" class="btn btn-lg btn-danger" id="btDel">
                                    <span class="btnr"><%=GetResource("Del")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btMod">
                                    <span class="btnb"><%=GetResource("Mod")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btAdd">
                                    <span class="btnb"><%=GetResource("Comment")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title">
                            <a>
                                <div class="conbox">
                                    <span class="LNoready" id="title" style="font-weight:bold;font-size:1.1em;"></span> <br />
                                    <span id="date"></span> <br />
                                    <span class="sch_user" id="typeEtc" style="font-size:0.9em;">
                                    </span>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock bg-white">
                        <div class="title">
                            <div id="content" class="rc"></div>
                        </div>
                    </li>
                    <li class="contentblock" id="reply" style="display:none;">
                    </li>
                    <li class="contentblock" id="att_line" style="display: none">
                        <div class="title">
                            <span class="istM">
                                <%=GetResource("Attach")%></span>
                            <br />
                            <div id="attach" style="width: 100%">
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup full-width">
                            <button type="button" class="btn btn-outline-light full-width" id="before"><%=GetResource("List")%></button>
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
        var OpenID = "<%=openID%>";
        var OpenName = "<%=openName%>";
        var OpenDate = "<%=openDate%>";
        var ScheduleID = "<%=scheduleID%>";
        var ToDoID = "<%=toDoID%>";
        var RealUserID = "<%=realUserID%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var DisplayID = "<%=displayID%>";
        var MobileType = "<%=MobileType %>";
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var R_Share = "<%=GetResource("Share") %>";     
        var R_Private = "<%=GetResource("Private") %>";     
        var R_All = "<%=GetResource("All") %>";     
        var R_Dept = "<%=GetResource("Dept") %>";     
        var R_Day = "<%=GetResource("Day") %>";     
        var R_Status1 = "<%=GetResource("Status1") %>";     
        var R_Status2 = "<%=GetResource("Status2") %>";     
        var R_Status3 = "<%=GetResource("Status3") %>";     
        var R_Status4 = "<%=GetResource("Status4") %>";     
        var R_Status5 = "<%=GetResource("Status5") %>";     
        var R_Status6 = "<%=GetResource("Status6") %>"; 
        var R_Priority1 = "<%=GetResource("Priority1") %>"; 
        var R_Priority2 = "<%=GetResource("Priority2") %>"; 
        var R_Priority3 = "<%=GetResource("Priority3") %>"; 
        var R_Status = "<%=GetResource("Status") %>";    
        var R_DelList = "<%=GetResource("DelList") %>"; 
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_NoDownload = "<%=GetResource("NoDownload") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_SecurityMsg = "<%=GetResource("SecurityMsg")%>";
        var R_Attach = "<%=GetResource("Attach")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Pims/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Pims/View.js");
    </script>
</body>
</html>
