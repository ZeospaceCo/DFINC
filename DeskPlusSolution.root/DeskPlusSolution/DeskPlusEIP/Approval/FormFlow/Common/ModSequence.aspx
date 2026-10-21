<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModSequence.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Common.ModSequence" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
<form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("ConfirmAppInfo")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="height:385px;">  

            <div id="divSeqZQLine" class="app_captit" style="float:left;clear:both;"></div>
            <div id="AccessTitleName" class="app_captit" style="float:left;clear:both;"></div>
            <div class="layout_scroll" style="min-height:295px;height:295px;">
                <div class="DD">
                    <ul id="ulWidgetsZD0"></ul>
                    <ul id="ulWidgets"></ul>
                </div>
            </div>
            <span class="app_captit"><%=GetResource("ModSeqMsg3")%></span>

        </div>
    </div>
</div>

</form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>

    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        var LineSeq = 0;
        //리소스
        var R_Msg_Success = "<%=GetResource("MSG_SUCCESS") %>";
        var R_Msg_SeqErrZD  = "<%=GetResource("Msg_SeqErrZD") %>";
        var R_Msg_SeqErrZA = "<%=GetResource("Msg_SeqErrZA") %>";
        var R_Msg_SeqErrZH = "<%=GetResource("Msg_SeqErrZH") %>";
        var R_ETC3 = "<%=GetResource("ETC3") %>";
        var R_Person = "<%=GetResource("Person") %>";
        var R_ZQ = "<%=GetResource("Receive") %>";
        var R_AccessTitle = "<%=GetResource("ViewAuth") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Common/ModSequence.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Common/ModSequence.js");
    </script>

</body>
</html>
