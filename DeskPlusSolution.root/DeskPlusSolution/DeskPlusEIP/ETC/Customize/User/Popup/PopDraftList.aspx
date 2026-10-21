<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopDraftList.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.User.Popup.PopDraftList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>        
</head>
<body>

<form id="form1" runat="server">

<div class="layer_popup">
    <div class="top-left"></div>
    <div class="top-right"></div>

    <div class="inside">
        <div class="popup_header">
            <h5>임시저장</h5>
            <ul class="btn_right">
                <li class="btn_blue"><cc3:ClientSmartButton ID="CbtnSave" runat="server"/></li>
                <li><cc3:ClientSmartButton ID="CbtnLoad" runat="server"/></li>
                <li><cc3:ClientSmartButton ID="CbtnDelete" runat="server"/></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <table class="table_typeB">
                <caption>임시저장</caption>
				<colgroup>
					<col width="80px" />
					<col />
				</colgroup>
				<tbody>
					<tr>
						<th>임시저장명</th>
						<td><cc3:ClientSmartTextBox ID="tbDraftName" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
					</tr>
				</tbody>
			</table> 
        </div>
        <div class="gap1"></div>
        <div class="popup_layout_style3">
            <div id="scrollDiv">
            <div class="table_typeLtop"><p></p></div>
            <div id="placeholder" style="clear: both;"></div>
            <div class="table_typeLbottom"><p></p></div>
            </div>
        </div>
    </div>

    <div class="bottom-left"></div>
    <div class="bottom-right"></div>
</div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = 0; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수        
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>"; 
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>"; 
        var UserID = "<%=UserID %>";
        var SelDraftID = "";
        var IsCard = "<%=IsCard %>";
        var CheckVal = "<%=CheckVal %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/User/Popup/PopDraft.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/User/Popup/PopDraft.js");
    </script>
</body>
</html>
