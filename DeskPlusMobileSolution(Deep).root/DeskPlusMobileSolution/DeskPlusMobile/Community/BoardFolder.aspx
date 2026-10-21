<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoardFolder.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Community.BoardFolder" %>

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
    <div data-role="page" id="BoardFolder">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <span id="listTitle" runat="server">
                        </span>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                </ul>
                <ul class="mobliecontent " id="phBoardFolder" style="clear: both;">
                </ul>
                <ul class="mobliecontent " id="phEmpty" style="clear: both;">
                </ul>
                <ul class="mobliecontent " id="phTemp" style="display: none">
                </ul>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var CommunityID = "<%=communityID%>";
        var Grade = "<%=grade%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";   
        var R_Menu = "<%=GetResource("Menu")%>";  
        var R_NoReadAuth = "<%=GetResource("NoReadAuth")%>";  
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Community/BoardFolder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Community/BoardFolder.js");
    </script>
</body>
</html>
