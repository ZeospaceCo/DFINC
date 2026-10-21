<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectTree.ascx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Folder.ProjectTree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<ComponentArt:TreeView id="tvProjectTree" runat="server" 
    Width="100%" 
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
            
    NoExpandImageUrl="noexpand.gif" 
    ExpandCollapseImagewidth="14" 
    ExpandCollapseImageHeight="11" 

    EnableClientScript="true"

    ClientSideOnNodeSelect="tvProjectTree_OnNodeSelect">
</ComponentArt:TreeView>
	
