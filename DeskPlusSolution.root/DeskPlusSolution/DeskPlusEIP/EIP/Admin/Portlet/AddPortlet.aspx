<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPortlet.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddPortlet" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <h1>
                    <%=GetResource("Organize_Tab")%></h1>
                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" class="btn btn-secondary" runat="server"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>                    
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3">
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("SelectFolder")%></caption>
                    <colgroup>
                        <col width="100px;" />
                        <col />                        
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("Tab_Name")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbTabName" Required="true"  runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                            </td>                            
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("SelectedPortlet")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbTargetPortlet" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="layout_scroll" style="min-height:255px;height:255px;">
                    <zeoPortletCategoryTree:UCPortletCategoryTreeScript ID="UCPortletCategoryTreeScript1" runat="server" UseIcon="true" UseActive="false"
                SetCallbackComplete="onCallbackComplete" SetNodeSelect="PortletNodeSelect_onNodeSelect"  />
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var PortletID = "<%=PortletID %>";
        var PortletTabID = "<%=PortletTabID %>";
        var TargetPortletID = "";
        var PortletType = "";
        var MultiLang = "<%=MultiLang %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_CheckPortlet = "<%=GetResource("CheckPortlet") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Portlet/AddPortlet.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Portlet/AddPortlet.js");
    </script>
</body>
</html>
