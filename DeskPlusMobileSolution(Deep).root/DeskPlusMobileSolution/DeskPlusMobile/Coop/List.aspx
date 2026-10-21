<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Coop.List" %>
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
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js" ></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
<script type="text/javascript">
    var BPage = "<%=bPage%>";
    var PageSize = "<%=moreSize%>", MoreSize = "<%=moreSize%>", MoreNo = "<%=pageNo%>", PageNo = 1;
    var UserID = "<%=UserID%>";
    var SearchValue = "<%=searchValue%>";

    var R_NoContent = "<%=GetResource("NoContent") %>";
    var R_Menu = "<%=GetResource("Menu")%>";
    var R_Request = "<%=GetResource("Request")%>";
    var R_Receive = "<%=GetResource("Receive")%>";
    var R_Reference = "<%=GetResource("Reference")%>";
    var R_Forward = "<%=GetResource("Forward")%>";

</script>
<script type="text/javascript">
    JsCall("/DeskPlusMobile/Js/Coop/List.js");
</script>

</head>
<body>
    <form id="form1" runat="server" AUTOCOMPLETE="off" onsubmit="return List.SetSearch();">
    <div id="divprg" align="center" class="loading"></div>
    <div data-role="page" id="CoopList">
    <div data-role="header">

    <div id="hd"><!-- hd -->
    <div class="tit"><a href="../Portal/Portal.aspx"><p><%=CompanyName%></p></a></div>
    <div class="hd_menu"><p class="hd_select"><%=GetResource("Menu")%> ▼</p></div>
    </div><!-- // hd -->

    <div class="to4"><a href="#" class="mct"><span id="listTitle" class="my"><%=GetResource("Request")%></span></a>
    <p class="we"> 
    <span class="dy"><%=Today%></span> </p>
    </div>

    <uc1:Menu ID="Menu1" runat="server" />
    </div>

    <div data-role="content">

    <div class="wr">
       <dl class="wr1">
       <dt><label for="searchtext"><%=GetResource("SearchWord")%></label></dt>
       <dd>
          &nbsp;<input type="text" id="tbSearch" maxlength="100" class="itcc" />
          <button type="button" class="bt5" id="search"><span class="btnb"><%=GetResource("Search")%></span></button>
       </dd>
       </dl>
    </div>

    <div id="phCoop" style="clear: both;"></div>      
    <div id="phEmpty" style="clear: both;"></div>
    <div id="phTemp" style="display:none"></div>  

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
    </div>

    <uc2:footer ID="Footer1" runat="server" />
    </form>
</body>
</html>

