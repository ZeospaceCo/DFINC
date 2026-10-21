<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Search.Web.Admin.Menu.Menu" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Search.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AccordionActive = <%=AccActive %>;
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Search/Js/Admin/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Search/Js/Admin/Menu/Menu.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu admin"> 
    <ul class="left-menu-container">
        <li>
            <div id="layout_treetop">
                <span class="treetop_title">검색관리</span>
            </div>
            <div id="layout_treeboxAcc">
                <div class="accordion">   
                    <div class="menu left-main-menu"><a href="#" onclick="" >색인</a></div>
                    <div class="menusub left-sub-menu">
                        <ul style="clear: both;">
                            <li><h2><a id="indexDefault">색인실행</a></h2></li>
                            <li><h2><a id="indexResult">색인결과</a></h2></li>
                            <li><h2><a id="indexSchedule">색인스케줄러</a></h2></li>
                        </ul>
                    </div>             
                    <div class="menu left-main-menu"><a href="#" onclick="" >검색</a></div>
                    <div class="menusub left-sub-menu">
                        <ul>
                            <li><h2><a id="searchResult">통합검색</a></h2></li>
                        </ul>
                    </div>        
                 </div>
            </div>
            <div id="layout_treebuttom"></div>
         </li>
    </ul>
</div>
    </form>
</body>
</html>
