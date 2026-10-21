<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrivateTree.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.PrivateTree" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="AddressTree" Src="../Common/AddressTree.ascx" %>
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
    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <h1><asp:literal ID="ltlTitle" runat="server"></asp:literal></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:320px;">
                <table class="table_typeB">
			    <caption><%=GetResource("CopyAdressList")%></caption>
			    <colgroup>
                    <col width="60px;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("ObjectFolder")%></th>
                        <td colspan="2"><input type="text" id="folderName" readonly="readonly" style="width:95%" /></td>
                    </tr>
                     <tr>
                        <th><%=GetResource("SubFolder") %></th>
                        <td><input type="text" id="subFolderName"  style="width:95%" /></td>
                        <td> 
                        <ul class="btn_small" style="padding-left:5px;">
                            <cc3:ClientSmartButton ID="cbtnAdd" BeginEndTag="li" Class="btn btn-sm" runat="server" />
                        </ul>
                        </td>
                    </tr>
                </tbody>
                </table>               
                    <div id="addTree" class="layout_scroll" style="min-height:292px;height:292px;">                  
                        <uc1:AddressTree ID="UCAddressTree"  UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false" UseCheckBoxMember="false" UseCategory="PG-3" runat="server" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete"></uc1:AddressTree>  
                    </div>                
            </div>
        </div>        
    </div>    

    <div style="display: none">    
        <asp:TextBox ID="tbUserValue" runat="server" Width="0"></asp:TextBox>              
        <asp:TextBox ID="tbCategoryID" runat="server" Width="0"></asp:TextBox>              
        <asp:TextBox ID="tbSubName" runat="server" Width="0"></asp:TextBox>        
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <asp:linkbutton id="LBTNAddCategory" runat="server" OnClick="LBTNAddCategory_Click"></asp:linkbutton>
    </div>   
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var FolderID = 0;       
        var ListAll = "<%=ListAll %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";               
        var R_SelectFolder = "<%=GetResource("SelectFolder") %>";
        var R_AfterProcessingError = "<%=GetResource("AfterProcessingError") %>";
        var R_CompleteSelectCopyAddress = "<%=GetResource("CompleteSelectCopyAddress")%>";
        var R_CompleteSelectMoveAddress = "<%=GetResource("CompleteSelectMoveAddress")%>";
        var R_SelectCategory = "<%=GetResource("SelectCategory")%>";
        var R_WriteName = "<%=GetResource("WriteName")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/PrivateTree.js"></script>
</body>
</html>
