<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Move.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portlet.Move" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">

        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ChangeGroup")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <table class="table_typeB">
			    <caption></caption>
			    <colgroup>
                    <col width="60px" />
                    <col />
			    </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("FolderName")%></th>
                        <td><cc3:ClientSmartTextBox ID="ctbName" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                    </tr>
                </tbody>
                </table>
                <div class="layout_scroll" style="min-height:285px;height:285px;">
                <zeoPortletTree:UCPortletTreeScript ID="UCUCPortletTreeScript1" runat="server" UseIcon="true" UseActive="false"
                    SetCallbackComplete="onCallbackComplete" SetNodeSelect="PortletNodeSelect_onNodeSelect" />
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var CategoryID = "<%=CategoryID %>";
        var PortletID = "<%=PortletID %>";
        var ParentCategoryID = 0;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_CheckMoveFolder = "<%=GetResource("CheckMoveFolder") %>";
        var R_ConfirmMove = "<%=GetResource("ConfirmMove") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Portlet/Move.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Portlet/Move.js");
    </script>
</body>
</html>
