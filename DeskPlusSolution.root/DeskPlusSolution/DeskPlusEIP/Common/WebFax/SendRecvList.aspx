<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendRecvList.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.WebFax.SendRecvList" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
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
        var Guid = "<%=Guid %>";
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = 0; //게시물수
        var RecordPerPage = 5;
        var Order = ""; //정렬

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_SUCCESS = "<%=GetResource("ACCOUNT_SUCCESS") %>";
        var R_ErrorProcessing = "<%=GetResource("Alert_Error") %>";
        var R_NoContent = "<%=GetResource("NoContents") %>";

        var R_FaxRecvName = "<%=GetResource("FaxRecvName") %>";
        var R_FaxNationalCode = "<%=GetResource("FaxNationalCode") %>";
        var R_FaxSendDate = "<%=GetResource("FaxSendDate") %>";
        var R_FaxNumber = "<%=GetResource("FaxNumber") %>";
        
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

    </script>
    <script type="text/javascript">
        JsCall("../Js/WebFax/SendRecvList.js");
    </script>
</head>
<body>

<form id="form1" runat="server">

<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1>수신자 명단</h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div id="scrollDiv">
            
            <div id="placeholder" style="clear: both;"></div>

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
</div>
</form>
</body>
</html>
