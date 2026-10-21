<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMenu.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Portal.AddMenu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var AccordionActive = 0;
        var MenuID = 0;        
        var IsAdmin = "<%=IsAdmin %>";
        var CategoryID = "<%=CategoryID %>";
        var Modules = "<%=Modules %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Complete = "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_CheckPortletCategory = "<%=GetResource("CheckPortletCategory") %>";
        var R_CheckPortletGroup = "<%=GetResource("CheckPortletGroup") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Portal/AddMenu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Portal/AddMenu.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("MenuManagement")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"/></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
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
                    <th><%=GetResource("MenuName")%></th>
                    <td><span id="portletName" style="width:95%"></span></td>
                </tr>
            </tbody>
            </table>
            <div class="layout_scroll"  style="min-height:280px;height:280px;">
                <zeoMenuTree:UCMenuTreeScript ID="UCMenuTreeScript1" runat="server" UseIcon="true" Disable2Depth="true" 
                    SetCallbackComplete="onCallbackComplete" SetNodeSelect="MenuNodeSelect_onNodeSelect" />
            </div>
        </div>
    </div>

</div>
    </form>
</body>
</html>
