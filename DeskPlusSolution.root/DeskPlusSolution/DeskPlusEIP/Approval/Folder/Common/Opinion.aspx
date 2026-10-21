<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Opinion.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Common.Opinion" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("AddOpinion")%></h1>
            <ul class="btn_right">
                <li><a href="javascript:history.back()" title="" class="btn btn-secondary"><%=GetResource("Confirm")%></a></li>
                <li><a href="javascript:history.back()" title="" class="btn"><%=GetResource("Close")%></a></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:120px;">

            <div class="memo"><textarea style="width:98%;"></textarea></div>
            
        </div> 
    </div>
</div>
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
</body>
</html>
