<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReplyAdd.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Pims.ReplyAdd" %>

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
    <div data-role="page" id="BoardView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-calendar dropbtn "></i><%=GetResource("Comment")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock" runat="server" id="dvAdd">
                        <div class="container-fluid buttongroup">
                            <div class="float-left full-width">
                                <button type="button" class="btn btn-lg btn-danger" id="btCancel">
                                    <span class="btnb"><%=GetResource("Cancel")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btAdd">
                                    <span class="btnb"><%=GetResource("Save")%></span>
                                </button>
                                <span>
                                    (<span class="charsRemaining"></span>)
                                </span>
                                <div class="float-right">
                                    <input type="checkbox" id="cbOpen" class="rd" checked="true" /><%=GetResource("ReplyOpen")%>   
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <textarea id="memo" class="ta3" maxlength="200"></textarea>
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
        var PageNo = "<%=pageNo%>";
        var Alarm  = "<%=alarm %>";

        var R_InputComment = "<%=GetResource("InputComment") %>";    
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_Menu = "<%=GetResource("Menu")%>";    
        var R_LimitShortReply = "<%=GetResource("LimitShortReply")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Pims/ReplyAdd.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Pims/ReplyAdd.js");
    </script>
</body>
</html>
