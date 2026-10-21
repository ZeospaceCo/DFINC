<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUserDept.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Folder.GetUserDept" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
        <div class="popup_header">
                <h1><%=GetResource("TitGetUD")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server"  class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseIcon="true" UseActiveDept="true" UseCheckBoxMember="true" UseCheckBoxGroup="true" />

        </div>                    
</div>


<div style="display: none">    
    <input name="tvSelectedID" type="text" id="tvSelectedID" />
    <input name="tvSelectedValue" type="text" id="tvSelectedValue" />    
</div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var Func = "<%=Func%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Msg_Sel = "<%=GetResource("Msg_Sel")%>";
        var R_Msg_Add = "<%=GetResource("Msg_Add")%>";
        var R_Msg_Admin = "<%=GetResource("Msg_Admin")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Folder/GetUserDept.js"></script>     
</body>
</html>
