<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserTree.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Attendance.UserTree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript">
        var AccessAuth = "<%=AccessAuth%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>"; 
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/ETC/Js/Attendance/UserTree.js");
    </script>     
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left">
        <div id="layout_treetop">
            <span class="treetop_title"><%=GetResource("Attendance_Department")%></span>
        </div>        
        
        <div id="layout_treeboxAcc" style="height:90%;" >
                <COMPONENTART:TREEVIEW 
                id="tvFolderTree" 
                runat="server" 
                                         
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

        <div id="layout_treebuttom"></div>
        <div id="layout_treebutton">
        </div>
    </div>
    <div style="display: none">        
        <asp:TextBox ID="tbHdnValue" runat="server" Width="0"></asp:TextBox>
    </div>
    </form>
</body>
</html>
