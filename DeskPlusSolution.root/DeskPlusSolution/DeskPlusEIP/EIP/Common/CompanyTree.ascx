<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyTree.ascx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Common.CompanyTree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<componentart:TreeView ID="CompanyTreeScript" runat="server" 
    Width="97%" 
    Height="95%" 
            
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
    ExpandCollapseImagewidth="14" 
    ExpandCollapseImageHeight="11"             
    EnableClientScript="true"     
    ClientSideOnNodeSelect="CompanyNodeSelect">
</componentart:TreeView>
