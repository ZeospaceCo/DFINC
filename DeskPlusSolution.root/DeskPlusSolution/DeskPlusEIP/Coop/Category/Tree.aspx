<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Category.Tree" %>

<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript">

        var ItemID = "<%=ItemID %>";        
        var ReLoad = "<%=ReLoad%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Coop/Js/Category/Tree.js");
    </script>    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left">

        <div id="layout_treetop">
            <span id="folderName" class="treetop_title"><%=GetResource("Collabo")%></span>
        </div>
        <div id="layout_treeboxAcc">
            <div class="accordion">
                <div class="menu"><a href="#" onclick="" ><%=GetResource("CategoryName")%></a></div>
                <div class="menusub">
                    <componentart:TreeView ID="tvTree" runat="server" 
                        Width="100%" 
                        Height="98%" 
            
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
                        LoadingFeedbackCssClass="LoadingFeedback"
             
                        NodeIndent="15" 
                        ShowLines="True" 
                        LineImageWidth="19" 
                        LineImageHeight="20" 
            
                        NoExpandImageUrl="noexpand.gif" 
                        ExpandCollapseImagewidth="14" 
                        ExpandCollapseImageHeight="11" 

            
                        EnableClientScript="true">
                    </componentart:TreeView>
                </div>                              
            </div>            
        </div>
        <div id="layout_treebuttom"></div>    
        
    </div>
    
    <div style="display:none">
        <input type="hidden" id="hdnCabinetID" runat="server" value="Request,Content/Request/List.aspx" />
    </div>        
    </form>
</body>
</html>
