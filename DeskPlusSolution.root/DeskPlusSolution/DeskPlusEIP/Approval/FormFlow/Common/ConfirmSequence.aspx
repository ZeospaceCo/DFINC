<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmSequence.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Common.ConfirmSequence" %>
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
        <div class="popup_layout_style3" style="min-height:270px;">
            
            <p class="app_capTit"><strong><%=GetResource("Report2")%></strong><%=GetResource("Report3")%> <strong><%=GetResource("AppOrder")%></strong><%=GetResource("Report4")%></p>
            <table class="table_typeB" style="overflow-y:auto;">
                <caption><%=GetResource("MakeDocFile")%></caption>
				<colgroup>
					<col width="80px" />
					<col />
				</colgroup>
				<tbody>
					<tr>
						<th><%=GetResource("AppOrder")%></th>
						<td><div id="divSeqLine"></div></td>
					</tr>
					<tr id="trZQ">
						<th><%=GetResource("Receive")%></th>
						<td><div id="divSeqZQLine"></div></td>
					</tr>
					<tr>
						<th><%=GetResource("ViewAuth")%></th>
                        <td><div id="AccessTitleName"></div></td>
					</tr>
				</tbody>
			</table> 
        </div>
    </div>
</div>

<div style="display:none;">
    <textarea id="htaXmlSeq" style="DISPLAY: none" name="htaXmlSeq" rows="2" cols="40" runat="server"></textarea>
</div>

    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Common/ConfirmSequence.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Common/ConfirmSequence.js");
    </script>

</body>
</html>
