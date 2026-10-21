<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptSearchTree.ascx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Common.DeptSearchTree" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<div class="com_treebox">
        <div class="com_srhSec">
            <ul class="btn_searchboxL" style="width:100% !important; text-align:center;" >
                <li><asp:TextBox ID="tbDeptSearch" ClientIDMode="Predictable" CssClass="searchtext" style="width:98px !important; margin-left:3px;" runat="server"></asp:TextBox></li>
                <li><input type="text" ID="tbDeptSearch2"  CssClass="searchtext" style="width:98px !important; margin-left:3px;display:none;" ></li>
                <li><cc3:ClientSmartButton ID="CbtnDeptUserSearch" Class="popupsearchbtn btn btn-sm btn-secondary" runat="server" /></li>
                <li><cc3:ClientSmartButton ID="CbtnTreeView" Class="popupsearchbtn btn btn-sm btn-secondary" runat="server" IsDisabled="0" CssClass=" " /></li>
            </ul>
        </div>
        <div class="com_cotSec">
            <ComponentArt:CallBack ID="CallBack1" CacheContent="true" runat="server">
                <Content>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        <ComponentArt:TreeView 
                                        ID="SearchDeptTree" 
                                        runat="server" 
                                        width="98%" 
                                        Height="300px" 
                                        
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
                                        
                                        ClientSideOnNodeSelect="DeptSearchTreeScript_onNodeSelect">
                        </ComponentArt:TreeView>
                        <asp:ListBox ID="lbDeptUser" runat="server" BorderWidth="1" Height="340px" width="100%" SelectionMode="Multiple" Visible="false"></asp:ListBox>
                    </asp:PlaceHolder>
                </Content>
                <LoadingPanelClientTemplate>
                    Loading...<br />
                    <img src="<%=BaseImgPath%>%>/Component/CallBack/spinner.gif" width="16" height="16" border="0">
                </LoadingPanelClientTemplate>
            </ComponentArt:CallBack>
        </div>
</div>
<script type="text/javascript">
    var TBDeptUser = "<%=tbDeptSearch.ClientID %>";
    var LBDeptUser = "<%=lbDeptUser.ClientID %>";
</script>
