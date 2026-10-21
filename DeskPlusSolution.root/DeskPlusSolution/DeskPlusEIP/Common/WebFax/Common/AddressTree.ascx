<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressTree.ascx.cs" Inherits="Zeospace.DeskPlus.Common.Web.WebFax.Common.AddressTree" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<componentart:TreeView 
    ID="tvAddressTree" 
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
    EnableClientScript="true">
</componentart:TreeView>
