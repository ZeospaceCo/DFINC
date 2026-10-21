<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewHistory.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Common.ViewHistory" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
     <div class="layer_popup">

        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("History")%></h1>
                <ul class="btn_right">
                   <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>  
            <div class="popup_layout_style3">          
                <div class="layout_scroll"  style="min-height:420px;height:420px;" > 
                    <asp:Panel ID="pnContent" runat="server" Width="100%" Height="100%"></asp:Panel>
                </div>
            </div>
        </div>
  
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/ViewHistory.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Common/ViewHistory.js");
    </script>
</body>
</html>

