<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoveCopy.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.MoveCopy" %>
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
            <div class="popup_layout_style3" style="min-height:327px;">
                <table class="table_typeB">
			    <caption><%=GetResource("CopyAdressList")%></caption>
			    <colgroup>
                    <col width="60px;" />
                    <col />
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("ObjectFolder")%></th>
                        <td><input type="text" id="folderName" readonly="readonly" style="width:95%" /></td>
                    </tr>
                </tbody>
                </table>
                <div class="layout_scroll" style="min-height:285px;height:285px;">
                    <uc1:AddressTree ID="UCAddressTree"  UseActiveDept="true" UseIcon="true" UseCheckBoxGroup="false" UseCheckBoxMember="false" UseCategory="PG-3" runat="server" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete"></uc1:AddressTree>
                </div>
            </div>
        </div>        
    </div>    

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var FolderID = 0;
        var CategoryID = "<%=CategoryID %>";
        var ListID = "<%=ListID %>"; //PM|36,PM|46,PM|42
        var Types = "<%=Types %>"; //PG
        var Gubun = "<%=Gubun %>"; //COPY
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_SelectFolder = "<%=GetResource("SelectFolder") %>";
        var R_AfterProcessingError = "<%=GetResource("AfterProcessingError") %>";
        var R_CompleteSelectCopyAddress = "<%=GetResource("CompleteSelectCopyAddress")%>";
        var R_CompleteSelectMoveAddress = "<%=GetResource("CompleteSelectMoveAddress")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/MoveCopy.js"></script>
</body>
</html>
