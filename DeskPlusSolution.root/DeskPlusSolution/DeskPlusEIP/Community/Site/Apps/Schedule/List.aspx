<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
<div class="layout_full">
    <div id="layout_header">
        <h1><%=MenuName %></h1>
        <ul class="btn_right">
            <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
        </ul>
    </div>

    <div id="layout_srh">
        <div class="cm_caldate">
            <a href="" class="b_prev" id="pvMonth"><i class="fas fa-angle-left"></i></a>
            <span id="timeList" runat="server"></span>
             <a href="" class="b_next" id="nxMonth"><i class="fas fa-angle-right"></i></a>
        </div>
        <ul class="btn_searchbox">
            <li>
            <asp:DropDownList ID="ddlYear" runat="server" CssClass="searchselect form-control"></asp:DropDownList> <%=GetResource("Year")%>
            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="searchselect form-control"></asp:DropDownList> <%=GetResource("Month1")%>
            &nbsp;
            </li>     
        </ul>
    </div>

    <div id="layout_content">
         <div class="row">
			 <div class="col-12 content-area-body"><!-- width 100%(col-12) -->   
                <div id="phListMonth" style="clear: both;"></div>  
                <div class="table_bt"><p></p></div>  
              </div>
         </div>   
    </div>
</div>
</div>  
<!-- ================================== Hidden Input ===================================== -->	
 <div style="display:none;">
<asp:TextBox ID="tbYear" Style="display: none" runat="server" Width="0px"></asp:TextBox>  
</div>
<!-- ==================================================================================== -->     
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">        
        var CommunityID = "<%=CommunityID %>";
        var MenuID = "<%=MenuID %>";
        var TmpDate = "<%=TmpDate%>";
        var TargetYear = "<%=TargetYear%>";
        var Grade = "<%=Grade %>"; 
        var CheckAuth = "<%=CheckAuth %>";
        var R_Sunday = "<%=GetResource("Sunday") %>";
        var R_Monday = "<%=GetResource("Monday") %>";
        var R_Tuesday = "<%=GetResource("Tuesday") %>";
        var R_Wednesday = "<%=GetResource("Wednesday") %>";
        var R_Thursday = "<%=GetResource("Thursday") %>";
        var R_Friday = "<%=GetResource("Friday") %>";
        var R_Saturday = "<%=GetResource("Saturday") %>";
        var R_NoWriteAuth = "<%=GetResource("NoWriteAuth") %>";
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Apps/Schedule/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Apps/Schedule/List.js");
    </script>
</body>
</html>
