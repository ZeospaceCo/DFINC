<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Folder.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("ManageFolder")%></h1>
            <ul class="btn_right">
                 <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                 <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            
            <table class="table_typeB">
            <caption><%=GetResource("ManageFolder")%></caption>
            <colgroup>
			    <col width="80px" />
				<col />
            </colgroup>
			<tbody>
			    <tr>
				    <th><%=GetResource("FolderPath")%></th>
					<td><asp:Label ID="lbFolder" runat="server"></asp:Label></td>
                </tr>
                <tr>
				    <th><%=GetResource("FolderName")%></th>
					<td title="">
                        <cc3:ClientSmartTextBox ID="ctbFolderName" InputMode="None" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("IsEnable")%></th>
                    <td><asp:CheckBox ID="chkIsActive" runat="server" Checked="true" /></td>
                </tr>
				</tbody>
			</table> 

            <!-- 다국어 시작 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("MultiLangSetting")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("MultiLang2")%></caption>
            <colgroup>
                <col width="80px" />
				<col />
            </colgroup>
            <tbody>
            <tr>
                <th><%=GetResource("Korean")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameKO" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("English")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameEN" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("Japanese")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameJA" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("Franch")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameFR" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("Chinese1")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameZHCN" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("Chinese2")%></th>
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

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        var R_Msg_ErrInputFolder = "<%=GetResource("NeedFolderName") %>"; //"폴더명을 입력하세요.";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Folder/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Folder/Add.js");
    </script>

    </form>
</body>
</html>
