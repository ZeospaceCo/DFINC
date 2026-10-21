<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Admin.Menu.Menu" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left"  class="left-menu admin">
        <ul class="left-menu-container">
         <li>
            <div id="layout_treetop">
                <span class="treetop_title"><%=GetResource("MenuF1")%><%=GetResource("Manager")%></span>
            </div>
            <div id="layout_treeboxAcc">
                <div class="accordion">
                    <div class="menu"><a href="#"><%=GetResource("MenuF2")%></a></div>
                    <div class="menusub">
                        <ul>
                            <li><a id="exam"><%=GetResource("MenuF3")%></a></li>
                            <li><a id="unit"><%=GetResource("MenuF4")%></a></li>
                        </ul>
                    </div>
                    <div class="menu"><a href="#" onclick=""><%=GetResource("MenuF5")%></a></div>
                    <div class="menusub">
                        <ul>
                            <li><a id="qing"><%=GetResource("MenuF6")%></a></li>
                            <li><a id="qclosing"><%=GetResource("MenuF7")%></a></li>
                        </ul>
                    </div>
                    <!-- div class="menu"><a href="#" onclick=""><%=GetResource("MenuF8")%></a></div>
                    <div class="menusub">

                        <ul class="menu_line">
                            <li><h2><a id="a1"><%=GetResource("MenuF9")%></a></h2></li>
                            <li><h2><a id="a2"><%=GetResource("MenuF10")%></a></h2></li>
                            <li><h2><a id="a3"><%=GetResource("MenuF11")%></a></h2></li>
                            <li><h2><a id="a4"><%=GetResource("MenuF12")%></a></h2></li>
                            <li><h2><a id="a5"><%=GetResource("MenuF13")%></a></h2></li>
                        </ul>                
                    </div//-->
                 </div>
            </div>               
            <div id="layout_treebuttom"></div>
          </li>
        </ul>
   
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Admin/Menu/Menu.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //        JsCall("/DeskPlusEIP/Opinion/Js/Admin/Menu/Menu.js");
    </script>
</body>
</html>
