<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewHistory.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Content.Popup.ViewHistory" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <div style="height: 320px; overflow-y: auto; min-height: 320px;">
                    <div id="placeholder" style="clear: both;"></div>
                </div>      
            </div>
        </div>
    </div>
    
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">        
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AttID = "<%=AttID%>";
        var TargetUserID = "<%=TargetUserID%>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_WSTime = "<%=GetResource("WSTime") %>";     //출근시간
        var R_WSConn = "<%=GetResource("WSConn") %>";     //접속
        var R_WCTime = "<%=GetResource("WCTime") %>";     //퇴근시간
        var R_Gubun = "<%=GetResource("Gubun") %>";       //구분
        var R_LateRemark = "<%=GetResource("LateRemark")%>"; //지각사유
        var R_EarlyRemark = "<%=GetResource("EarlyRemark")%>"; //조태사유
        var R_ModifyName = "<%=GetResource("ModifyName") %>";     //수정자
        var R_ModifyDate = "<%=GetResource("ModifyDate") %>";     //수정일
        var R_ModifyReason = "<%=GetResource("ModReason") %>";     //수정사유
        var R_NoContent = "<%=GetResource("NoContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Content/Popup/ViewHistory.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/HR/Js/Content/Popup/ViewHistory.js");
    </script>
</body>
</html>