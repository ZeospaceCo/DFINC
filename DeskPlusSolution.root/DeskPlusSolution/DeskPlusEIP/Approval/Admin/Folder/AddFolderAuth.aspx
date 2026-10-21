<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFolderAuth.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Folder.AddFolderAuth" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div id="popup_header">
            <h1><%=GetResource("SelDept")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>            
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layer_scroll_box">
                <zeoDeptTree:UCDeptTreeScript ID="UCDeptTreeScript1" runat="server" UseIcon="true" UseCheckBox="false" UseActiveDept="true" />
            </div>
        </div>                
    </div>
</div>
<div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
</div>
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        RqDeptID = "<%=DeptID%>";
        RqDeptNm = "<%=DeptNm%>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_Msg_Dept = "<%=GetResource("Msg_Dept")%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Folder/AddFolderAuth.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Folder/AddFolderAuth.js");
    </script>

</body>
</html>
