<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleTreeScript.ascx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Common.RoleTreeScript" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<componentart:TreeView ID="RoleTreeScript1" runat="server" 
    Width="100%" 
    Height="98%"     
            
    EnableViewState="false"
    AutoPostBackOnSelect="False" 
    DragAndDropEnabled="false" 
    NodeEditingEnabled="false" 
    AutoScroll="true"   
              
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
    EnableClientScript="true">
</componentart:TreeView>
