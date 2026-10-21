<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetGrade.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Admin.General.Common.GetGrade" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>    
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("ModGradeNameManage")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>                        
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:310px;">

            <table class="table_typeB">
                <caption><%=GetResource("ChangeGradeNameManage")%></caption>
				<colgroup>
					<col width="100px" />
					<col />
				</colgroup>
				<tbody>
					<tr>
						<th><%=GetResource("MemeberGradeName")%></th>
						<td><cc3:ClientSmartTextBox ID="tbGradeName" runat="server" style="width:90%" /></td>
					</tr>
				</tbody>
			</table> 

            <!-- 다국어 시작 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("MultiLangSetting")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("MultiLang")%></caption>
            <colgroup>
                <col width="100px" />
				<col />
            </colgroup>
            <tbody>
            <tr>
                <th><%=GetResource("Korean")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameKo" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("English")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameEn" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("Japanese")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameJa" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("French")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameFr" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("Chinese")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameZhcn" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            <tr>
                <th><%=GetResource("Taiwanese")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbNameZhtw" InputMode="None" runat="server" Required="false" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            </tbody>
            </table>
        </div> 
    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CommunityID = "<%=CommunityID %>";
        var GroupID = "<%=GroupID %>";
        var NameBase = "<%=NameBase %>";
        var NameKo = "<%=NameKo %>";
        var NameEn = "<%=NameEn %>";
        var NameJa = "<%=NameJa %>";
        var NameFr = "<%=NameFr %>";
        var NameZhcn = "<%=NameZhcn %>";
        var NameZhtw = "<%=NameZhtw %>";
    </script>
    <script type='text/javascript' src='/DeskPlusEIP/Community/Js/Site/Admin/General/Common/GetGrade.js'></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Admin/General/Common/GetGrade.js");
    </script>
</body>
</html>
