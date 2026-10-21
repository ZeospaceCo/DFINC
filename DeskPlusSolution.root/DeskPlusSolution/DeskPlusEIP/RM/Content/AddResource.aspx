<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddResource.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.AddResource" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" >
        var CategoryID = "0";
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang %>";
        var R_ResourcesTitle = "<%=GetResource("ResourcesTitle") %>";
        var R_ResourcesUser = "<%=GetResource("ResourcesUser") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_SelectResources = "<%=GetResource("SelectResources") %>";
        var R_QtyZero = "<%=GetResource("QtyZero") %>";
        var R_BeforResource = "<%=GetResource("BeforResource")%>";
        var R_No = "<%=GetResource("No")%>";
        var R_Yes = "<%=GetResource("Yes")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/AddResource.js" ></script>  
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/RM/Js/Content/AddResource.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    
    <div class="inside" style="min-height:475px;">
        <div class="popup_header">
            <h1><%=GetResource("ResourcesChoice")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:400px;">

             <!-- 자원 예약 트리 -->
             <div class="resource_popup_list1">
                <div id="layout_left">
                    <div id="_layout_treetop"><span class="treetop_title" style="display:none"><%=GetResource("ResourcesAdd") %></span> </div>
                    <div id="layout_treeboxAcc" class="layout_scroll" style="width:210px;height:90%;margin:1px 15px 5px 1px;padding:10px;outline:1px solid rgba(0,0,0,.1);" >
                        <div class="accordion" >
                            <div class="menusub">
                        <COMPONENTART:TREEVIEW 
                        id="tvFolderTree" 
                        runat="server" 
                        Width="180px" 
                        Height="340px" 
                                        
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
                    <div id="layout_treebuttom"></div>
                </div>
            </div>
            <!-- 자원 예약 트리 -->

            <!-- 자원 예약 리스트 -->
            <div class="resource_popup_list2" style="overflow-y:auto;min-height:400px;height:400px;">
            <div id="phResource" style="clear: both;"></div>                
            </div>
            <!-- 자원 예약 리스트 -->

            <!-- 자원 예약 내용 -->
            <div class="resource_popup_list3">
                <div class="layout_scroll" style="min-height:430px;height:430px;">
                <table class="table_typeB">
                    <caption></caption>
                    <colgroup>
                        <col width="100px" />
                        <col />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><%=GetResource("Image")%></th>
                            <td colspan="3">
                                <div class="imageSlider">  
                                    <div class="left" id="imgLeft" style="display:none"><a href="#"><img src="<%=BaseImgPath%>/Main/ptl_laytype3_left.gif" alt="이전" title="이전" /></a></div>
                                    <div class="imageList">    
                                         <div id="phImage" style="clear: both;"></div>                    
                                    </div>
                                    <div class="right" id="imgRight" style="display:none"><a href="#"><img src="<%=BaseImgPath%>/Main/ptl_laytype3_right.gif" alt="이후" title="이후" /></a></div>                                
                                </div>                            
                            </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("ResourcesTitle")%></th>
                            <td colspan="3" id="tdTitle"></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("Code")%></th>
                            <td id="tdCode"></td>
                            <th><%=GetResource("AuthorityNeed")%></th>
                            <td id="tdAuth"></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("Qty")%></th>
                            <td id="tdUsableQty"></td>
                            <th><%=GetResource("RequestQty")%></th>
                            <td><asp:DropDownList ID="ddlQty" runat="server" CssClass="searchselect form-control" ></asp:DropDownList>  </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("Place")%></th>
                            <td colspan="3" id="tdPlace"></td>
                        </tr>
                        <tr>
                            <th style="min-height:100px;"><%=GetResource("Content")%></th>                            
                            <td colspan="3" id="tdContent"></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            </div>
            <!-- 자원 예약 내용 -->
        </div>
    </div>
</div>
    <div style="display:none;">           
        <asp:TextBox ID="tbListID" Style="display: none"   runat="server" Width="0px"></asp:TextBox>       
        <asp:TextBox ID="tbAuthority" Style="display: none"   runat="server" Width="0px"></asp:TextBox>       
    </div>
    </form>
</body>
</html>
