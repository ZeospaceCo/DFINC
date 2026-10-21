<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetPublicFolder.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Folder.GetPublicFolder" %>

<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("SelectFolder")%></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>                        
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:320px;">
                    <table class="table_typeB">
			        <caption><%=GetResource("SelectFolder")%></caption>
			        <colgroup>
                        <col width="60px;" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><%=GetResource("TargetFolder")%></th>
                            <td>
                            <cc3:ClientSmartTextBox ID="folderName" ReadOnly="true" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                    </tbody>
                    </table>
                    <div class="layout_scroll">
                        <COMPONENTART:TREEVIEW 
                        id="tvFolderTree" 
                        runat="server" 
                        Width="203px" 
                        Height="260px" 
                                        
                        EnableViewState="false"
                        AutoPostBackOnSelect="False" 
                        DragAndDropEnabled="false" 
                        NodeEditingEnabled="false" 
                        AutoScroll="false" 
                                        
                        CssClass="TreeView" 
                        NodeCssClass="TreeNode"
                        HoverNodeCssClass="HoverTreeNode" 
                        SelectedNodeCssClass="SelectedTreeNode" 
                        MultipleSelectedNodeCssClass="SelectedTreeNode"
                        SelectedNodeRowCssClass="SelectedTreeNodeRow"
                        NodeRowCssClass="TreeNodeRow" 
                                        
                        NodeIndent="15" 
                        ShowLines="True" 
                        LineImageWidth="19" 
                        LineImageHeight="20" 
                                        
                        NoExpandImageUrl="noexpand.gif" 
                        ExpandCollapseImagewidth="14" 
                        ExpandCollapseImageHeight="11" 

                                        
                        EnableClientScript="true" 
                        ClientSideOnCallbackComplete="onCallbackComplete"  
                        ClientSideOnNodeSelect="onNodeSelect">
                        </COMPONENTART:TREEVIEW>                                
                    </div>
            </div>                
        </div>
    </div>
    </form>
    <div id="zeoModalBG"></div>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript">
        var FolderID = "<%=FolderID%>";
        var FileList = "<%=FileList%>";
        var Gubun = "<%=Gubun%>";
        var UserID = "<%=UserID%>";
        var RealFolderID = "";
        var UID = "";
        var ListType = "<%=ListType%>";
        var Order = "<%=Order %>"; //정렬     
        var R_CompareFolder = "<%=GetResource("CompareFolder") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";        
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";

        var PublicFolder = "<%=GetResource("PublicFolder") %>";
        var PAdmin = "<%=PAdmin%>";
        var Types = "<%=Types%>";        
        var ImagePath = "<%=BaseImgPath%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Folder/GetPublicFolder.js"></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Folder/GetPublicFolder.js");
    </script>
</body>
</html>