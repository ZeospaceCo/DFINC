<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddIP.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddIP" %>
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
            <h1><%=GetResource("RegIP")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" class="btn btn-secondary" runat="server"/>
                <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"/>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            
            <table class="table_typeB">
            <tr>
                <td>
                    <ul class="aLeft">
                        <li>
                            <asp:DropDownList ID="ddlIPAccess" runat="server" class="searchselect form-control"></asp:DropDownList>                            
                        </li>
                        <li>
                            <input type="text" id="start1" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="start2" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="start3" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="start4" maxlength="3" style="width:40px !important;" />
                            &nbsp;~ &nbsp;
                            <input type="text" id="end1" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="end2" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="end3" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="end4" maxlength="3" style="width:40px !important;" />
                        </li>
                    </ul>
                </td>
            </tr>
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
        var R_InputIP = "<%=GetResource("InputIP") %>";
        var R_InputIPNumber = "<%=GetResource("InputIPNumber") %>";
        var R_WrongIPRange = "<%=GetResource("WrongIPRange") %>";
        var R_ErrorAccessAuth = "<%=GetResource("ErrorAccessAuth") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Portlet/AddIP.js" ></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Portlet/AddIP.js");
    </script>
</body>
</html>
