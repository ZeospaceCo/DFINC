<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShareList.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.MyTalk.ShareList" %>
<%@ Register src="../Portal/Menu.ascx" tagname="Menu" tagprefix="uc1" %>
<%@ Register src="../Portal/Footer.ascx" tagname="Footer" tagprefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
<title></title>
<meta charset="uft8" />
<meta name="viewport" content="initial-scale=1.0; maximum-scale=1.0; minimum-scale=1.0; user-scalable=no;" />
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main.css"/>
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main_<%=SkinType%>.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading"></div>
    <div data-role="page" id="AddressList">
    <div data-role="header">

    <div id="hd"><!-- hd -->
    <div class="tit"><a href="../Portal/Portal.aspx"><p><%=CompanyName%></p></a></div>
    <div class="hd_menu"><p class="hd_select"><%=GetResource("Menu")%> ▼</p></div>
    </div><!-- // hd -->

    <div class="to4"><font class="my"><%=GetResource("MyTalk")%></font>
    <p class="we">
    <span class="dy istSt" style="cursor:pointer;"><a href="#" id="address">+ <%=GetResource("Address")%></a></span>
    </div>

    <uc1:Menu ID="Menu1" runat="server" />
    </div>

    <div data-role="content">   
    <div class="to3">
		    <div class="pl">
			    <button type="button" class="bt5" id="btAdd"><span class="btnb"><%=GetResource("WallPost")%></span></button>                 
		    </div>
    </div> 
    <!-- list -->
    <div id="phMyTalk" style="clear: both;"></div>
    <div id="phEmpty" style="clear: both;"></div>
    <div id="phTemp" style="display:none"></div>
    <!-- //list -->

    <!-- footer -->
    <div class="ubt">
	    <div class="ubtsw">
		    <a href="#" class="ubtst">▲ <%=GetResource("Top")%></a>
	    </div>
    </div>
    <!-- //footer -->

    <!-- more list -->
    <div class="ml">
    <ul>
    <li><a href="#" class="mct" id="more"><%=GetResource("More")%></a></li>
    </ul>
    </div>
    <!-- //more list -->    

    </div>

    <uc2:footer ID="Footer1" runat="server" />
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript">
        var PageSize = "<%=moreSize%>", MoreSize = "<%=moreSize%>", MoreNo = "<%=pageNo%>", PageNo = 1;
        var UserID = "<%=UserID %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";  
        var R_Menu = "<%=GetResource("Menu")%>";  
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/MyTalk/ShareList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/MyTalk/ShareList.js");
    </script>
</body>
</html>
