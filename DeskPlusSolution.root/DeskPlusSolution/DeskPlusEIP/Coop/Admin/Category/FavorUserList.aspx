<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FavorUserList.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Admin.Category.FavorUserList" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var ItemSubID = "<%=ItemSubID %>"; //카테고리
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "0"; //게시물수
        var ListCount = "10"; //보여줄게시물수 
        var Order = "";  
        
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";     
                
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 

    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Coop/Js/Admin/Category/FavorUserList.js");
    </script>
</head>
<body>

<form id="form1" runat="server">

<div class="layer_popup">
    <div class="top-left"></div>
    <div class="top-right"></div>

    <div class="inside">
        <div class="popup_header">
            <h5>명단</h5>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layout_scroll" style="min-height:420px;height:420px;">
                
                <div id="phList" style="clear: both;"></div>

                <!-- 페이징 시작 //-->
                <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                    <h3><span id="pageInfo"></span></h3>
	                <span id="pager"></span>
                    <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight">
                        <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                        <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                        <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                        <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <!-- 페이징 끝 //-->
        </div>
    </div>

    <div class="bottom-left"></div>
    <div class="bottom-right"></div>
</div>
</form>
</body>
</html>
