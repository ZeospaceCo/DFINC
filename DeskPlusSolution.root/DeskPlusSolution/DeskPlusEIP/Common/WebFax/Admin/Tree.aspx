<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.WebFax.Admin.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <style>
        html, body
        {
            height: 98%;
        }
    </style>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
     <script type="text/javascript">
         
         var ImagePath = "<%=BaseImgPath%>";
         var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript">
        JsCall("../../Js/WebFax/Admin/Tree.js");
    </script>    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left">
        <div id="layout_treetop">
            <span class="treetop_title">팩스관리</span>
        </div>
        <div id="layout_treeboxAcc" style="height:90%;" >
            <div class="accordion" >
                <div class="menusub">
                    <componentart:TreeView 
                        ID="tvFaxTree" 
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
                        LoadingFeedbackCssClass="LoadingFeedback"
    
                        NodeIndent="15"
                        ShowLines="True" 
                        LineImageWidth="19" 
                        LineImageHeight="20" 
                        NoExpandImageUrl="noexpand.gif" 
                        ExpandCollapseImageWidth="14" 
                        ExpandCollapseImageHeight="11" 
                        EnableClientScript="true"

                        ClientSideOnNodeSelect="FaxNodeSelect_onNodeSelect"
                        ClientSideOnCallbackComplete="FaxNodeSelect_onNodeComplete"
                        >
                    </componentart:TreeView>

               </div>
            </div>
        </div>

        <div id="layout_treebuttom"></div>        
    </div>
    </form>
</body>
</html>
