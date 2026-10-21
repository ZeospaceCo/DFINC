<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Folder.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Coop.Folder" %>
<%@ Register src="../Portal/Menu.ascx" tagname="Menu" tagprefix="uc1" %>
<%@ Register src="../Portal/Footer.ascx" tagname="Footer" tagprefix="uc2" %>
<!DOCTYPE html>

<html>
<head runat="server">
<title></title>
<meta charset="uft8" />
<meta name="viewport" content="initial-scale=1.0; maximum-scale=1.0; minimum-scale=1.0; user-scalable=no;" />
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main.css"/>
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main_<%=SkinType%>.css"/>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
<script type="text/javascript">

    var CompanyID = "<%=CompanyID%>";
    var UserID = "<%=UserID%>";

    var R_Menu = "<%=GetResource("Menu")%>";

</script>
<script type="text/javascript">
    JsCall("/DeskPlusMobile/Js/Coop/Folder.js");
</script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading"></div>
    <div data-role="page" id="CoopFolder">
    <div data-role="header">

    <div id="hd"><!-- hd -->
    <div class="tit"><a href="../Portal/Portal.aspx"><p><%=CompanyName%></p></a></div>
    <div class="hd_menu"><p class="hd_select"><%=GetResource("Menu")%> ▼</p></div>
    </div><!-- // hd -->

    <div class="to2"><font class="my"><%=GetResource("Coop")%></font>
    <p class="we"><span class="dy"><%=Today%></span> </p>
    </div>

    <uc1:Menu ID="Menu1" runat="server" />
    </div>
    <div data-role="content">
    <!-- list -->
    <ul class="mli">

    <li><a href="#" class="mct" id="request"><font><%=GetResource("Request")%></font><font class="lif"><span id="req"></span></font>
    <br /><span class="istc"></span>
    </a></li>
    <li><a href="#" class="mct" id="receive"><font><%=GetResource("Receive")%></font><font class="lif"><span id="rec"></span></font>
    <br /><span class="istc"></span>
    </a></li>
    <li><a href="#" class="mct" id="reference"><font><%=GetResource("Reference")%></font><font class="lif"><span id="ref"></span></font>
    <br /><span class="istc"></span>
    </a></li>
    <li><a href="#" class="mct" id="forward"><font><%=GetResource("Forward")%></font><font class="lif"><span id="for"></span></font>
    <br /><span class="istc"></span>
    </a></li>
    </ul>
    <!-- //list -->
    </div>
    </div>

    <uc2:footer ID="Footer1" runat="server" />
    </form>
</body>
</html>
