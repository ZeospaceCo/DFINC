<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopPimAdd.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.PopPimAdd" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />        
</head>
<body>
    <form id="form1" runat="server">    

    <div class="inside">
        <div class="popup_header">  
        <h1><%=GetResource("PimAddTime")%></h1>          
               <ul class="btn_right">        
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>        
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" ></cc3:ClientSmartButton></li>        
                </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="height:140px">        
            <div id="startEndDate">			
                <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>~
                <asp:DropDownList ID="ddlendTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
            </div>
        </div>
   </div>
    
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>    
    <script type="text/javascript">
        var Add_InputDate = '<%= GetResource("Add_InputDate") %>';
        var TimeAlert = '<%= GetResource("TimeAlert") %>';
        var PreLang = "<%=PreLang%>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/PopPimAdd.js" ></script> 
     <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
         //JsCall("/DeskPlusEIP/Pim/Js/Schedule/PopPimAdd.js");             
    </script>
</body>
</html>
