<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaveDraft.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Common.SaveDraft" %>
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
            <h1><%=GetResource("PrivateDraft")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <span class="app_captit" style="padding:0px 0px 5px 0px;"><%=GetResource("DraftSubTitle")%></span>
            <table class="table_typeB">
                <caption><%=GetResource("PrivateDraft")%></caption>
				<colgroup>
					<col width="80px" />
					<col />
				</colgroup>
				<tbody>
					<tr>
						<th><%=GetResource("PrivateDraftName")%></th>
						<td><cc3:ClientSmartTextBox ID="tbDraftName" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
					</tr>
				</tbody>
			</table> 
            <p>&nbsp;</p>
        </div>
    </div>
</div>
    
    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>

        <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaFormTEXT" style="DISPLAY: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaEditorHTML" style="DISPLAY: none" name="htaEditorHTML" rows="2" cols="40" runat="server"></textarea>
        <asp:TextBox ID="htaEditorMime" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        
        <asp:textbox id="tbFormID" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbSwfUploadGUID" Runat="server" Width=""></asp:textbox>
        <asp:textbox id="tbNonActiveX" Runat="server" Width=""></asp:textbox>
        <asp:textbox id="tbUploadPath" Runat="server" Width=""></asp:textbox>
        <asp:textbox id="tbIsUseFlow" Runat="server" Width=""></asp:textbox>
        
        <asp:textbox id="tbIsGuard" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbAccessTitleID" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbIsLinkedEtc" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbCarbonView" Runat="server" Width="0"></asp:textbox>
        <asp:TextBox ID="tbBaseDocIDs" runat="server"></asp:TextBox>
        <textarea id="htaXmlSeq" style="DISPLAY: none" name="htaXmlSeq" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaXmlSeqZQ" style="DISPLAY: none" name="htaXmlSeqZQ" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaXmlSeqZC" style="DISPLAY: none" name="htaXmlSeqZC" rows="2" cols="40" runat="server"></textarea>

        <asp:TextBox ID="tbDraftID" runat="server" Width="0" Text="0"></asp:TextBox>
        <asp:TextBox ID="tbWriteTimeYMD" runat="server" Width="0" Text=""></asp:TextBox>
        
    </div>
    <!-- ==================================================================================== -->

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL") %>";
        var R_Msg_InputDraftName = "<%=GetResource("Msg_InputDraftName") %>";
        var R_Msg_ConfirmUpdate = "<%=GetResource("Msg_ConfirmUpdate") %>";
        var R_MsgAlertScript = "<%=GetResource("MsgAlertScript") %>";
    </script>
    <script type="text/javascript">
        //window.name = "dialogPopup";
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Common/SaveDraft.js");
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Common/SaveDraft.js"></script>

</form>
</body>
</html>