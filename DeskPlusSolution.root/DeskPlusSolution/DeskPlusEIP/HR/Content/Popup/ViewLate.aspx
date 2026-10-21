<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewLate.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Content.Popup.ViewLate" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css" /> 
</head>
<body>
    <form id="form1" runat="server">
 <div class="layer_popup">
    <div class="inside">
        
        <div class="popup_header">
            <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
            <ul class="btn_right">
                <li style="background:none"><div id="loading"></div></li>
                <li style="background:none"><asp:DropDownList ID="ddlUser" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li id="sec" runat="server" class="sec"></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>        
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="overflow:hidden !Important;">
        <div class="layout_scroll" style="min-height:270px;height:270px;">
        	<!-- 테이블 시작 -->
            <div id="placeholder" style="clear: both;"></div>
            <!-- 테이블 끝 -->
        </div>
        </div>

    </div>
</div>
<!-- //layout_info -->
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script language="javascript">
        var UserID = "<%=UserID%>";
        var UserName = "<%=UserName%>";
        var DeptID = "<%=DeptID%>";
        var PathID = "<%=PathID%>";
        var DeptBase = "<%=DeptBase%>";
        var StartDate = "<%=StartDate%>";
        var EndDate = "<%=EndDate%>";
        var MinClosingID = "<%=MinClosingID%>";
        var MaxClosingID = "<%=MaxClosingID%>";
        var Gubun = "<%=Gubun%>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";

        var R_Gubun = "<%=GetResource("Gubun")%>";
        var R_DeptName = "<%=GetResource("DeptName")%>";
        var R_Name = "<%=GetResource("Name")%>";
        var R_TitleName = "<%=GetResource("TitleName")%>";
        var R_CalcDate = "<%=GetResource("CalcDate")%>";
        var R_Time = "<%=GetResource("Time")%>";
        var R_Reason2 = "<%=GetResource("Reason2")%>";
        var R_DocView = "<%=GetResource("DocumentID") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Content/Popup/ViewLate.js"></script>
</body>
</html>