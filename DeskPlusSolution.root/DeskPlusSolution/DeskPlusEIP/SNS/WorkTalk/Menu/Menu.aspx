<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.WorkTalk.Menu.Menu" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_SNS.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_SNS.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript">        
        var ReLoad = "<%=ReLoad%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/SNS/Js/WorkTalk/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/SNS/Js/WorkTalk/Menu/Menu.js");
    </script>    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu ">
    <ul class="left-menu-container">
    <li>

        <div id="layout_treeboxAcc">
            <div class="accordion">
                <div class="menu  left-main-menu menu ui-state-active"><a href="#" onclick="" ><%=GetResource("Folder")%></a></div>
                <div class="menusub" >
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
        </li>
        </ul>
    </div>
    
    <div style="display:none">
        <input type="hidden" id="hdnCabinetID" runat="server" value="PrivateList" />
    </div>        
    </form>
</body>
</html>

