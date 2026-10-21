<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Main.Menu" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <META HTTP-EQUIV="CACHE-CONTROL" CONTENT="NO-CACHE">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>    
</head>
<body>
<form id="form1" runat="server">
<div id="layout_left" class="left-menu">
    <ul class="left-menu-container">
       <li class="writebtn ">
            <!-- 개설신청 //-->
            <div class="cm_left_line">
                <a onclick="Menu.Add()"><span class="btn btn-lg btn-primary btn-block"><%=GetResource("MakeCommunity")%></span></a>
                <span id="adminMode" runat="server" class="cap2" style="font-size:13px;"><asp:CheckBox ID="cbShowImage" runat="server" /><%=GetResource("GoAdmin")%></span>
            </div>
        </li>
        <li>
            <!-- 기타목록 //-->
            <div id="layout_treeboxAcc">
                <div class="accordion">
                    <div class="menu left-main-menu" style="border-top:0px solid red;">
                        <span><a href="#" onclick=""><%=GetResource("MyCommunityList")%></a></span>
                    </div>
                    <div class="menusub left-sub-menu">
                        <ul id="phMyList" style="clear: both;"></ul>
                    </div>
                    <div class="menu left-main-menu">
                        <span><a href="#" onclick="" ><%=GetResource("ApplyCommunityList")%></a></span>
                    </div>
                    <div class="menusub left-sub-menu">
                        <ul id="phApplyList" style="clear: both;"></ul>
                    </div>
                </div>
            </div>
        </li>
    </ul>    
</div>
</form>
</body>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
<script type="text/javascript">
    var AccordionActive = <%=AccActive %>;
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang %>";
    var CommunityID = "";
    var Grade = "";        
    var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
    var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    var R_Del = "<%=GetResource("Del") %>";
    var R_DelAdminCommunity  = "<%=GetResource("DelAdminCommunity") %>";
</script>
<script type="text/javascript" src="/DeskPlusEIP/Community/Js/Main/Menu.js"></script>
<script type="text/javascript">
    //JsCall("/DeskPlusEIP/Community/Js/Main/Menu.js");
</script>
</html>
