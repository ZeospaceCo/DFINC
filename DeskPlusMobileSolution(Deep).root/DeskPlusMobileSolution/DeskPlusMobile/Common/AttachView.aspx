<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttachView.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Common.AttachView" %>
<%@ Register src="../Portal/Menu.ascx" tagname="Menu" tagprefix="uc1" %>
<%@ Register src="../Portal/Footer.ascx" tagname="Footer" tagprefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
<title></title>
<meta charset="uft8" />
<meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; minimum-scale=0.5; user-scalable=yes" />
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main.css"/>
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main_<%=SkinType%>.css"/>
</head>
<body>
    <form id="form1" runat="server">

    <div id="divprg" align="center" class="loading"></div>
    <div data-role="page" id="ApprovalBaseView">
    <div data-role="header">

    <div id="hd">
    <div class="tit"><a href="../Portal/Portal.aspx"><p><%=CompanyName%></p></a></div>
    <div class="hd_menu"><p class="hd_select"><%=GetResource("Menu")%> ▼</p></div>
    </div><!-- // hd -->

    <div class="to4"><font class="my"><%=GetResource("AttachView")%></font>
    <p class="we"> 
    <span class="dy"><%=Today%></span> </p>

    </div>

    <uc1:Menu ID="Menu1" runat="server" />
    </div>

    <div data-role="content">

    <!-- docu view -->
    <div class="dv" id="contentHTML" runat="server"></div>
    <!-- //docu view -->


    </div>

    <uc2:footer ID="Footer1" runat="server" />
    </div>

    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript">
        var R_Menu = "<%=GetResource("Menu")%>";
    </script>
</body>
</html>
