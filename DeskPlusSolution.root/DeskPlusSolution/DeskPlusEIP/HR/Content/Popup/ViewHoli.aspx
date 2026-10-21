<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewHoli.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Content.Popup.ViewHoli" %>
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
        <div class="layout_scroll" style="min-height:320px;height:320px;">
        	<!-- 테이블 시작 -->
            <div id="placeholder" style="clear: both;"></div>
            <!-- 테이블 끝 -->
            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <!-- 페이징 끝 //-->
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
        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";

        var R_Number = "<%=GetResource("Number") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_UserName = "<%=GetResource("Name") %>";
        var R_TitleName = "<%=GetResource("TitleName") %>";
        var R_StartDate = "<%=GetResource("StartDate") %>";
        var R_StartHalf = "<%=GetResource("StartHalf") %>";
        var R_EndDate = "<%=GetResource("EndDate") %>";
        var R_EndHalf = "<%=GetResource("EndHalf") %>";
        var R_Duration = "<%=GetResource("Duration") %>";
        var R_Gubun = "<%=GetResource("Gubun") %>";
        var R_DocView = "<%=GetResource("DocumentID") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Content/Popup/ViewHoli.js"></script>
</body>
</html>