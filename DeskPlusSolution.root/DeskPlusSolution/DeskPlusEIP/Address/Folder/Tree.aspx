<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Folder.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="AddressTree" Src="../Common/AddressTree.ascx" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript">
        var FolderType = "<%=Types %>";
        var RealFolderType = "<%=Types %>";        
        var UseAddRootPrivateCategory = "<%=UseAddRootPrivateCategory %>"; //개인주소록 xml그룹옵션
        var FolderCategoryID = "0";
        var RealFolderCategoryID = "0";
        var AccordionActive = <%=AccordionActive %>;
        var ListTypeUser = "0";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_DeleteFolder = "<%=GetResource("DeleteFolder") %>";
        var R_DeleteSubFolder = "<%=GetResource("DeleteSubFolder") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ImportantDel = "<%=GetResource("ImportantDel") %>";        
        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Folder/Tree.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left"  class="left-menu"> 
        <ul class="left-menu-container"> 
        <div id="layout_treeboxAcc" style="height:90%;">
            <div class="accordion">
                <div class="menu left-main-menu"><a href="#"><%=GetResource("CompanyAddress")%></a></div>
                <div class="menusub" style="display:none;">
                    <uc1:AddressTree ID="UCAddressTree"  UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false" UseCheckBoxMember="false" 
                    UseCategory="GG-1,TG-5" runat="server" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete"></uc1:AddressTree>
                </div>

                <div class="menu left-main-menu"><a href="#"><%=GetResource("CommonAddress")%></a></div>
                <div class="menusub" style="display: none">
                    <uc1:AddressTree ID="AddressTree1"  UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false" UseCheckBoxMember="false" 
                    UseCategory="CG-2" runat="server" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete"></uc1:AddressTree>                    
                </div>

                <div class="menu left-main-menu"><a href="#"><%=GetResource("PrivateAddress")%></a></div>
                <div class="menusub"  style="display: none">
                    <uc1:AddressTree ID="AddressTree2"  UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false" UseCheckBoxMember="false" 
                    UseCategory="PG-3,RG-4" runat="server" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete"></uc1:AddressTree>                    
                </div>                              

                <div class="menu left-main-menu"><a href="#"><%=GetResource("Favorite")%></a></div>
                <div class="menusub"  style="display: none">
                    <uc1:AddressTree ID="AddressTree6"  UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false" UseCheckBoxMember="false" 
                    UseCategory="FG-6" runat="server" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete"></uc1:AddressTree>                    
                </div>
                <div class="menu left-main-menu"><a href="#" id="GoImpo"><%=GetResource("Important")%></a></div>
            </div>            
        </div>
        <div id="layout_treebuttom"></div>
        <span id="layout_treebutton" class="treetop_btn">                        
            <a id="folderDelete" class="btn btn-sm btn-danger" ><%=GetResource("Delete")%></a>
            <a id="folderModify" class="btn btn-sm" ><%=GetResource("Mod")%></a> 
            <a id="folderAdd" class="btn btn-sm" ><%=GetResource("Add")%></a>
        </span>
		</ul>
    </div>
    <div style="display: none">
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <asp:TextBox ID="tbCID" runat="server" Width="0"></asp:TextBox>
    </div>    
    </form>
</body>
</html>
