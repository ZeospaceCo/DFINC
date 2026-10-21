<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseView.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Coop.BaseView" %>
<%@ Register src="../Portal/Menu.ascx" tagname="Menu" tagprefix="uc1" %>
<%@ Register src="../Portal/Footer.ascx" tagname="Footer" tagprefix="uc2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
<meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; minimum-scale=0.5; user-scalable=yes" />
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main.css"/>
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main_<%=SkinType%>.css"/>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
<script type="text/javascript">
    var BPage = "<%=bPage%>", ListID = "<%=listID%>", GroupID = "<%=groupID%>";
    var PageNo = "<%=pageNo%>", MoreSize = "<%=moreSize%>";
    var CompanyID = "<%=CompanyID%>";
    var UserID = "<%=UserID%>";    
    var SearchValue = "<%=searchValue%>";
    var PreLang = "<%=PreLang %>";
  
    var R_Menu = "<%=GetResource("Menu")%>";
    var R_Requester = "<%=GetResource("Requester")%>";
    var R_To = "<%=GetResource("To")%>";
    var R_Cc = "<%=GetResource("Cc")%>";
    var R_CoopStatus1 = "<%=GetResource("CoopStatus1")%>";
    var R_CoopStatus2 = "<%=GetResource("CoopStatus2")%>";
    var R_CoopStatus3 = "<%=GetResource("CoopStatus3")%>";
    var R_CoopStatus4 = "<%=GetResource("CoopStatus4")%>";
    var R_CoopStatus5 = "<%=GetResource("CoopStatus5")%>";
    var R_IsClose1 = "<%=GetResource("IsClose1")%>";
    var R_IsClose2 = "<%=GetResource("IsClose2")%>";
    var R_DelCoop  = "<%=GetResource("DelCoop")%>";
    var R_NoDownload = "<%=GetResource("NoDownload") %>";

</script>
<script type="text/javascript">
    JsCall("/DeskPlusMobile/Js/Coop/BaseView.js");
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading"></div>
    <div data-role="page" id="ApprovalView">
    <div data-role="header" >

    <div id="hd">
    <div class="tit"><a href="../Portal/Portal.aspx"><p><%=CompanyName%></p></a></div>
    <div class="hd_menu"><p class="hd_select"><%=GetResource("Menu")%> ▼</p></div>
    </div><!-- // hd -->

    <div class="to4"><a href="#" class="mct"><font id="listTitle" class="my"><%=GetResource("Coop")%><%=GetResource("BaseDoc")%></font></a>
    <p class="we"> 
    <span class="dy"><%=Today%></span> </p>
    </div>

    <uc1:Menu ID="Menu1" runat="server" />
    </div>

    <div data-role="content">
    <!-- info -->
	<div class="h3"  >
		<h2 id="subject"></h2>
		<span class="dif" id="etc"></span><br />
        <span class="dih"><%=GetResource("First")%>:</span><span id="item"></span>, <span class="dih"><%=GetResource("Second")%>:</span><span id="itemsub"></span><br />
        <span class="dih"><%=GetResource("RequestDate")%>:</span><span id="requestDate"></span> / <span class="dih"><%=GetResource("CompleteRequestDate")%>:</span><span id="completeRequestDate"></span><br />
        <span class="dih"><%=GetResource("StartDate")%>:</span><span id="startDate"></span> / <span class="dih"><%=GetResource("EndDate")%>:</span><span id="endDate"></span><br />

	</div>

    <ul id="appro_line" class="di2"  >
       <li>
       <div  class="mct">
       <div id="requester" style="width:100%"></div>      
       <div id="to" style="width:100%"></div>      
       <div id="cc" style="width:100%"></div>      
       </div>
       </li>
    </ul>
    <!-- //info -->

    <!-- history view -->
    <div  class="dv">
        <asp:Panel ID="pnContent" runat="server"></asp:Panel>
    </div>
    <!-- //history view -->
    
    </div>

    <uc2:footer ID="Footer1" runat="server" />

    </div>

    </form>
</body>
</html>
