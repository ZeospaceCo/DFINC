<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SortScheduleList.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.SortScheduleList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
 </head>
<body>
    <form id="form1" runat="server">
    <div id="divtodo" style="background:white;width:400px;height:180px;z-index:100;position: absolute;">
        
        <div class="layer_popup">
        <div class="inside" style="background:white;">
            <div class="popup_header">
                <h1 id="divPopTitle" runat="server"></h1>
                <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnClosetodo" runat="server"   /></li>
                </ul>
            </div>
        
            <div class="popup_layout_style3" style="height:190px;">
                <div class="dotLine"></div>
                    <ul id ="ulList" runat="server">             
                    </ul>
                </div>
            </div>
        </div>
        </div>
    </form>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/SortScheduleList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/SortScheduleList.js");        
    </script>
</body>
</html>
