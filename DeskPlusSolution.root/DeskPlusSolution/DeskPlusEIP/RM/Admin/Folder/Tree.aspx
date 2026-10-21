<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Admin.Folder.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var AccordionActive = 0;
        var RealCategoryID = "0";
        var ParentCategoryID = "0";
        var R_DeleteNoFolder = "<%=GetResource("DeleteNoFolder") %>";
        var R_DeleteSubFolder = "<%=GetResource("DeleteSubFolder") %>";
        var R_DeleteFolder = "<%=GetResource("DeleteFolder") %>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Admin/Folder/Tree.js"></script>
    <script type="text/javascript">
//            JsCall("/DeskPlusEIP/RM/Js/Admin/Folder/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
	    <div id="layout_left" class="left-menu admin">  
             <ul style="height: 90%;" class="left-menu-container">
              <li>
                    <div id="layout_treetop">
                        <span class="treetop_title"><%=GetResource("RM")%></span>
                    </div>
              </li>
              <li>
                <div id="layout_treeboxAcc"  style="height: 90%;">
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
                            <div class="menu"><a id="qrcode">QRCode</a></div>  
                            <div class="menusub" style="display:none;">
                                <ul>
                                    <li><a id="qrcodesub">QRCode</a></li>
                                </ul>
                            </div>                           
                            <div class="menu" id="MenuAdmin"><a id="AdminReserve"><%=GetResource("AdminReserveList")%>&nbsp;</a></div>
                            <div class="menusub" id="MenuSubAdmin">
                                <ul>
                                    <li><h2><a id="Usable0Reserve"><%=GetResource("Usable0Reserve")%></a></h2></li>
                                    <li><h2><a id="Usable1Reserve"><%=GetResource("Usable1Reserve")%></a></h2></li>
                                    <li><h2><a id="Usable2Reserve"><%=GetResource("Usable2Reserve")%></a></h2></li>
                                </ul>
                            </div>   
                        </div>
                    </div>
                    <div id="layout_treebuttom"></div>           
                    <div>
                        <span id="layout_treebutton" class="treetop_btn">
                            <a id="imgDel" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>
                            <a id="imgMod" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Mod")%></a>
                            <a id="imgAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Add")%></a>   
                        </span>    
                    </div>  
                   </li>
                </ul>                   
        </div>
        
        <div style="display: none">
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <asp:TextBox ID="tbCID" runat="server" Width="0"></asp:TextBox>
        </div>
    </form>
</body>
</html>
