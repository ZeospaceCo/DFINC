<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddParam.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddParam" %>
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
        <div class="popup_header">
            <h1><%=GetResource("Parameter")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" class="btn btn-secondary" runat="server"/>
                <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"/>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            
            <table class="table_typeB">
            <caption><%=GetResource("MenuManagement")%></caption>
			<colgroup>
                <col width="80px" />
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><label title="<%=GetResource("IsEncrypt")%>"><%=GetResource("Encrypt")%></label></th>
                    <td>
                        <ul class="aLeft">
                            <li>
                                <asp:DropDownList ID="ddlEncript" runat="server" class="searchselect form-control"></asp:DropDownList>
                            </li>                            
                            <cc3:ClientSmartTextBox ID="ctbEncETC" MaxLength="200" runat="server" BeginEndTag="li"  CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>                            
                        </ul>
                    </td>
                </tr>
                <tr>
                    <th><label title="<%=GetResource("Target")%>"><%=GetResource("Target")%></label></th>
                    <td>
                        <ul class="aLeft">
                            <li>
                                <asp:DropDownList ID="ddlTargetType" runat="server" class="searchselect form-control"></asp:DropDownList>
                            </li>
                            <li>
                                <asp:DropDownList ID="ddlTargetDataType" runat="server" class="searchselect form-control"></asp:DropDownList>
                            </li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <th><label title="<%=GetResource("ParameterName")%>"><%=GetResource("UserName")%></label></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbParamName" MaxLength="200" runat="server" Text="" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Values")%></th>
                    <td style="min-height:95px;height:95px">
                        <ul class="aLeft">
                            <li>
                                <asp:DropDownList ID="ddlLDAP" runat="server" class="searchselect form-control"></asp:DropDownList>
                            </li>
                        </ul>
                        <cc3:ClientSmartTextBox ID="ctbParamValue" TextMode="MultiLine" MaxLength="200" runat="server" CssStyle="width:95%; height:50px !important;"></cc3:ClientSmartTextBox>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Comments")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbDescription" MaxLength="200" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                </tr>
            </tbody>
            </table>

        </div>
    </div>

</div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>           
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var PortletID = "<%=PortletID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_InputEncode = "<%=GetResource("InputEncode") %>";
        var R_InputContent = "<%=GetResource("InputContent") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Portlet/AddParam.js" ></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Portlet/AddParam.js");
    </script>
</body>
</html>