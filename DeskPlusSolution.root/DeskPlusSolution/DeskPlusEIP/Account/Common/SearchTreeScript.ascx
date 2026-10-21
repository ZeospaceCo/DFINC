<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchTreeScript.ascx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Common.SearchTreeScript" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<div>
        <ul class="btn_searchbox" style="width:100% !important;text-align:center;" >
            <li><asp:TextBox ID="tbDeptUser" ClientIDMode="Predictable" CssClass="searchtexts" style="width:98px !important; height:25px;vertical-align:middle" runat="server"></asp:TextBox></li>
            <li style="padding-top:5px"><cc3:ClientSmartButton ID="CbtnDeptUserSearch" Class="popupsearchbtn btn btn-sm btn-secondary" runat="server" Text="검색" /></li>
            <li style="padding-top:5px"><cc3:ClientSmartButton ID="CbtnTreeView" Class="popupsearchbtn btn btn-sm btn-secondary" runat="server" Text="취소" IsDisabled="0" /></li>
        </ul>
        <div class="app_gap"></div>
        <ComponentArt:CallBack ID="CallBack1" CacheContent="true" runat="server">
            <Content>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                    <ComponentArt:TreeView 
                                    ID="DeptUserTree" 
                                    runat="server" 
                                    width="98%" 
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
                                        
                                    ClientSideOnNodeSelect="DeptUserTreeScript_onNodeSelect">
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
<script type="text/javascript">
    var TBDeptUser = "<%=tbDeptUser.ClientID %>";
    var LBDeptUser = "<%=lbDeptUser.ClientID %>";
</script>
