<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpinionView.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Common.Popup.OpinionView" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>C1/Css/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>C1/Css/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>C1/Css/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layer_popup" class="view">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("Opinion")%></h1>
                <div class="btn_right">
                    <ul class="float-left">
                        <li><a href="#" id="Close" class="btn"><span><%=GetResource("Close")%></span></a></li>
                    </ul>
                </div>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="padding:20px;height:470px;">
                        <div id="placeholder_app" style="clear: both;"></div>
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
        var CompanyID = "<%=CompanyID %>";
        var ChildDocID = "<%=ChildDocID %>";
        var UpPathID = "<%=UpPathID %>";

        var R_NoContent = "<%=GetResource("NoContent") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Common/Popup/OpinionView.js"></script>
</body>
</html>