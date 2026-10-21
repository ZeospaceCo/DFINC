<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FolderAdd.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.FolderAdd" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
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
        var FolderID = "<%=FolderID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja        
        var R_Msg_ErrInputFolder = "<%=GetResource("Add_ReplyInputContent")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/FolderAdd.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Report/Js/Content/FolderAdd.js");
    </script>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><asp:literal ID="ltlTitle" runat="server"></asp:literal></h1>
            <ul class="btn_right">
                 <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                 <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3"  style="min-height:370px">
            
            <table class="table_typeB">
            <caption></caption>
            <colgroup>
			    <col width="80px" />
				<col />
            </colgroup>
			<tbody>	
                <tr>
				    <th><%=GetResource("ParentFolderName")%></th>
					<td>
                        <asp:Literal ID="ltlParentFolderName" runat="server"></asp:Literal>
                    </td>
                </tr>		    
                <tr>
				    <th><%=GetResource("GroupBaseName")%></th>
					<td>
                        <cc3:ClientSmartTextBox ID="ctbFolderName" InputMode="None" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("IsActive")%></th>
                    <td><asp:CheckBox ID="chkIsActive" runat="server" Checked="true" /></td>
                </tr>
				</tbody>
			</table> 

            <!-- 다국어 시작 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("SettingMultiLang") %></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("SettingMultiLang") %></caption>
            <colgroup>
                <col width="80px" />
				<col />
            </colgroup>
            <tbody>
            <tr style="<%=IsKo %>">
                <th><%=GetResource("Korean")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameKO" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr style="<%=IsEn %>">
                <th><%=GetResource("English")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameEN" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr style="<%=IsJa %>">
                <th><%=GetResource("Japanese")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameJA" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr style="<%=IsFr %>">
                <th><%=GetResource("French")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameFR" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr style="<%=IsZhCn %>">
                <th><%=GetResource("Chinese")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameZHCN" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr style="<%=IsZhTw %>">
                <th><%=GetResource("Taiwanese")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameZHTW" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            </tbody>
            </table>
        </div> 
    </div>
</div>

<div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
</div>
    </form>
</body>
</html>
