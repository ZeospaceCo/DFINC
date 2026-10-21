<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.MyTalk.View" %>

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
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="MyTalkView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class='fas fa-comments dropbtn '></i>
                            <%=GetResource("CommentView")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" class="btn btn-lg btn-danger" id="btDel" style="display: none">
                                    <span class="btnr"><%=GetResource("Del")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btAdd">
                                    <span class="btnb"><%=GetResource("Comment")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock padding0">
                        <div class="title">
                            <span id="title"></span>
                            <div id="userInfo"></div>
                            <div id="date"></div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title">
                            <div class="rc" id="content"></div>
                        </div>
                    </li>
                    <li class="contentblock" id="reply" style="display: none">
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
        var GroupID = "<%=groupID %>";    
        var PageNo = "<%=pageNo%>";
        var RUserID = 0;
        var DWidth = "";   
        var R_DelList = "<%= GetResource("DelList") %>";
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_SecurityMsg = "<%=GetResource("SecurityMsg")%>";    
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/MyTalk/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/MyTalk/View.js");
    </script>
</body>
</html>
