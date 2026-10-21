<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopBankAccount.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.User.Popup.PopBankAccount" %>
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
            <h5>Bank Account</h5>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
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
        var VendorCD = "<%=VendorCD%>";    
        // 마스터 벤더 인지 디테일 벤더인지     
        var MasterDetailGubun = "<%=MasterDetailGubun %>"; 
        var IsAdmin = "<%=IsAdmin %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/User/Popup/PopBankAccount.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/User/Popup/PopBankAccount.js");
    </script>
</body>
</html>
