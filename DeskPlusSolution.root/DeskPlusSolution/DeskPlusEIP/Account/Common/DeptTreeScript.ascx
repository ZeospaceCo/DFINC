<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptTreeScript.ascx.cs"
    Inherits="Zeospace.DeskPlus.Account.Web.Common.DeptTreeScript" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<componentart:TreeView ID="DeptTreeScript1" runat="server" Width="97%" Height="95%"
    EnableViewState="false" AutoPostBackOnSelect="False" DragAndDropEnabled="false"
    NodeEditingEnabled="false" AutoScroll="false" CssClass="TreeView" NodeCssClass="TreeNode"
    HoverNodeCssClass="HoverTreeNode" SelectedNodeCssClass="SelectedTreeNode" MultipleSelectedNodeCssClass="SelectedTreeNode"
    SelectedNodeRowCssClass="SelectedTreeNodeRow" NodeRowCssClass="TreeNodeRow" LoadingFeedbackCssClass="LoadingFeedback"
    NodeIndent="15" ShowLines="True" LineImageWidth="19" LineImageHeight="20" ExpandCollapseImageWidth="14"
    ExpandCollapseImageHeight="11" EnableClientScript="true" ClientSideOnNodeSelect="DeptNodeSelect_onNodeSelect">
</componentart:TreeView>

