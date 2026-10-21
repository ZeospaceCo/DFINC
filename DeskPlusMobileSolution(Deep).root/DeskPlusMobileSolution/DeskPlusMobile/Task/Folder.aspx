<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Folder.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Task.Folder" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/<%=SkinType%>/navimobile.css" />
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="CoopFolder">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <i class="fas fa-puzzle-piece dropbtn "></i>
                        <%=GetResource("Coop")%>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="title linkDiv" id="request">
                            <a href="#" class="mct">
                                <font><%=GetResource("Request")%></font> <font class="lif"><span id="req"></span></font>
                                <br />
                                <span class="istc"></span>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title linkDiv" id="receive">
                            <a href="#" class="mct">
                                <font><%=GetResource("Receive")%></font> <font class="lif"><span id="rec"></span></font>
                                <br />
                                <span class="istc"></span>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title linkDiv" id="reference">
                            <a href="#" class="mct">
                                <font><%=GetResource("Reference")%></font> <font class="lif"><span id="ref"></span></font>
                                <br />
                                <span class="istc"></span>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title linkDiv" id="forward">
                            <a href="#" class="mct">
                                <font><%=GetResource("Forward")%></font> <font class="lif"><span id="for"></span></font>
                                <br />
                                <span class="istc"></span>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title linkDiv" id="delay">
                            <a href="#" class="mct">
                                <font><%=GetResource("Delay")%></font> <font class="lif"><span id="del"></span></font>
                                <br />
                                <span class="istc"></span>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title linkDiv" id="recent">
                            <a href="#" class="mct">
                                <font><%=GetResource("Recent")%>
                                <br />
                                <%=GetResource("RecentStr")%></font>
                                <br />
                                <span class="istc"></span>
                            </a>
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
        var R_Menu = "<%=GetResource("Menu")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Task/Folder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Task/Folder.js");
    </script>
</body>
</html>
