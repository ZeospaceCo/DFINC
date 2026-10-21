<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Menu.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" />  

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AccordionActive = <%=p_AccActive %>;
        var ReLoad = "<%=ReLoad%>";
        var ItemID = "<%=ItemID %>"; //대분류
        var IsRoleTask = "<%=isRoleTask %>";
        var DefaultTab = "<%=DefaultTab %>";
        var HStatus  = "<%=hStatus %>";
        var SearchDateValue = "<%=SearchDateValue %>"; //검색날짜값        
        var SearchField = "<%=SearchField %>"; //검색키워드
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="layout_left" class="left-menu">
        <ul class="left-menu-container">
            <li class="writebtn">
                <a id="ReserveAdd"><div class="btn-write btn btn-lg btn-primary btn-block"><i class="fas fa-plus"></i><%=GetResource("TaskAdd")%></div></a>
            </li>
            <li id="layout_treeboxAcc">
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
                    <div class="menu"><a href="#" onclick="" ><%=GetResource("PrivateSetting")%></a></div>
                    <div class="menusub">
                        <ul>
                            <li><h2><a id="ImpoUser"><%=GetResource("ImpoUser")%></a></h2></li>
                            <li><h2><a id="DraftList"><%=GetResource("Draft")%></a></h2></li>
                            <li><h2><a id="MenuList"><%=GetResource("MenuSetting")%></a></h2></li>                  
                        </ul>
                    </div>             
                    <div class="menu" id="MenuAdmin" runat="server"><a href="#" onclick="" ><%=GetResource("Admin")%></a></div>
                    <div class="menusub" id="MenuSubAdmin" runat="server">
                        <ul>
                            <li><h2><a id="AdminItem"><%=GetResource("GroupManage")%></a></h2></li>
                            <li><h2><a id="AdminSource"><%=GetResource("SourceSetting")%></a></h2></li>
                            <li><h2><a id="AdminList"><%=GetResource("AllCoWorkList")%></a></h2></li>                    
                        </ul>
                    </div>     
                </div>
            </li>
        </ul>
        </div> 
    </form>
</body>
</html>
