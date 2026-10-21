<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Folder.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" />   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var AccordionActive = <%=p_AccActive %>;
        var RealCategoryID = "0";
        var ParentCategoryID = "0";
        var R_DeleteNoFolder = "<%=GetResource("DeleteNoFolder") %>";
        var R_DeleteSubFolder = "<%=GetResource("DeleteSubFolder") %>";
        var R_DeleteFolder = "<%=GetResource("DeleteFolder") %>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Folder/Tree.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/RM/Js/Folder/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
	    <div id="layout_left" class="left-menu ">
        <ul class="left-menu-container">
            <li class="writebtn">
                <a id="ReserveAdd"><div class="btn-write btn btn-lg btn-primary btn-block"><i class="fas fa-plus"></i><%=GetResource("ResourcesAddTree")%></div></a>
            </li>
            <li id="layout_treeboxAcc">
             <div class="accordion">
                <div class="menu" id="private"><a href="#"><%=GetResource("RMCategory")%></a></div>
                <div class="menusub">
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
                <div class="menu"><a id="MyReserve"><%=GetResource("MyReserve")%></a></div>
                <div class="menusub">
                    <ul>
                        <li><h2><a id="MyReserveSub"><%=GetResource("MyReserve")%></a></h2></li>
                    </ul>
                </div>
                <div class="menu" id="MenuAdmin" runat="server"><a id="AdminReserve"><%=GetResource("AdminReserve")%>&nbsp;<span id="mCnt" runat="server" class="icon_Comment_view"></span></a></div>
                <div class="menusub" id="MenuSubAdmin" runat="server">
                    <ul>
                        <li><h2><a id="Usable0Reserve"><%=GetResource("Usable0Reserve")%>&nbsp;<span id="mSubCnt" runat="server" class="icon_Comment_view"></span></a></h2></li>
                        <li><h2><a id="Usable1Reserve"><%=GetResource("Usable1Reserve")%></a></h2></li>
                        <li><h2><a id="Usable2Reserve"><%=GetResource("Usable2Reserve")%></a></h2></li>
                    </ul>
                </div>
            </div>           
            </li>           
        </ul>       
        </div>
    </form>
</body>
</html>
