<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Move.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.Move" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
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
                <h1><%=GetResource("ChangeBasicDept")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton>
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
                        <th><%=GetResource("Admin_UserManager_Literal2")%></th>
                        <td><input type="text" id="folderName" readonly="readonly" size="30"/></td>
                    </tr>
                </tbody>
                </table>
                <div class="layout_scroll" style="min-height:280px;height:280px;">
                <zeoDeptTree:UCDeptTreeScript ID="UCDeptTreeScript1" runat="server" UseIcon="true" UseCheckBox="true" UseActiveDept="false" />
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
        var DeptID = 0;
        var UserList = "<%=UserList %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_SUCCESS = "<%=GetResource("SUCCESS") %>";
        var R_CheckMoveBaseDept = "<%=GetResource("CheckMoveBaseDept") %>";
        
    </script>
    <script type="text/javascript" src="../../Js/Admin/User/Move.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/User/Move.js");
    </script>
</body>
</html>
