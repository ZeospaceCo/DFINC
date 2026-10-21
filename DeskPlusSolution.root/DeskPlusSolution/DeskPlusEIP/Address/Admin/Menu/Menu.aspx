<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Admin.Menu.Menu" %>
<%@ Register TagPrefix="uc1" TagName="AddressTree" Src="/DeskPlusEIP/Address/Common/AddressTree.ascx" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin">
    <ul class="left-menu-container">
    <li>
        <div id="layout_treetop"><span class="treetop_title"><%=GetResource("AddressManage")%></span></div>

        <div id="layout_treeboxAcc" style="height:90%;">
            <div class="accordion">
                <div class="menu">
                    <a href="#"><%=GetResource("CommonFolderManage")%></a></div>
                    <div class="menusub">
                        <uc1:AddressTree ID="UCAddressTree1" UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false"
                            UseCheckBoxMember="false" UseCategory="CG-2" runat="server" SetNodeSelect="onNodeSelect"
                            SetCallbackComplete="onCallbackComplete"></uc1:AddressTree>
                    </div>

                <div class="menu">
                    <a href="#"><%=GetResource("CommonManage")%></a></div>
                    <div class="menusub">
                        <uc1:AddressTree ID="UCAddressTree2" UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false"
                            UseCheckBoxMember="false" UseCategory="GG-1,CG-2,RG-4" runat="server" SetNodeSelect="onNodeSelectList"
                            SetCallbackComplete="onCallbackCompleteList"></uc1:AddressTree>
                    </div>

                <div class="menu" style="display:none"><a href="#" id="configSetting"><%=GetResource("SettingEnvironment")%></a></div>
            </div>
        </div>       
        <div id="layout_treebuttom"></div>
        <!-- 버튼 //-->
        <div id="layout_treebutton">
            <a id="imgDel" style="cursor: pointer" class="btn btn-sm btn-danger" ><%=GetResource("Delete")%></a>
            <a id="imgAdd" style="cursor: pointer" class="btn btn-sm" ><%=GetResource("Add")%></a>                             
        </div>
    </li>
    </ul>   
    </div>
    <div style="display: none">
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton>
        <asp:TextBox ID="tbCID" runat="server" Width="0"></asp:TextBox>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var FolderType = "CG";
        var RealFolderType = "CG";
        var FolderCategoryID = "0";
        var RealFolderCategoryID = "0";
        var UseAddRootCommonCategory = "<%=UseAddRootCommonCategory %>"; //공용주소록 xml그룹옵션
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_DeleteFolder = "<%=GetResource("DeleteFolder") %>";
        var R_DeleteSubFolder = "<%=GetResource("DeleteSubFolder") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Admin/Menu/Menu.js"></script>
</body>
</html>