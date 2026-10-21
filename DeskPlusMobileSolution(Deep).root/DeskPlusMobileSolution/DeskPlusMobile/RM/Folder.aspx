<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Folder.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.RM.Folder" %>

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
    <div data-role="page" id="RMFolder">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <i class="fas fa-calendar-minus dropbtn "></i>
                        <%=GetResource("ResourceManager")%>
                        <div class="float-right date text-dark">
                            <a href="#" id="add">
                                +<%=GetResource("ResourcesAdd")%>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="title linkDiv" id="aReserve">
                            <a href="#" class="mct">
                               <i class="fas fa-user-clock"></i> <font><%=GetResource("ResourcesReserve")%></font>
                                <br />
                                <span class="istc"></span>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title linkDiv" id="aMyReserve">
                            <a href="#" class="mct">
                                <i class="fas fa-user-tag"></i><font><%=GetResource("MyReserve")%></font>
                                <br />
                                <span class="istc"></span>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock" id="MenuAdmin" runat="server" visible="false">
                        <div class="title linkDiv" id="aApprove">
                            <a href="#" class="mct">
                               <i class="fas fa-user-lock"></i> <font><%=GetResource("Usable0Reserve")%></font><font class="lif"><span id="app"></span></font>
                                <br />
                                <span class="istc"></span>
                            </a>
                        </div>
                    </li>
                </ul>
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
    <script type="text/javascript" src="/DeskPlusMobile/Js/RM/Folder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/RM/Folder.js");
    </script>
</body>
</html>
