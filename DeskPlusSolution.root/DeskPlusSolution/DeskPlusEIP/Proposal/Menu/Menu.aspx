<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Menu.Menu" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu">

    <ul class="left-menu-container">
       <li id="layout_treeboxAcc">
        <div class="accordion">
            <div class="menu left-main-menu menu"><a href="#" onclick="" >제안제도</a></div>
            <div class="menusub">
                 <ul class="left-menu-container">
                    <li><h2><a id="Statistics">제안제도</a></h2></li>
                    <li><h2><a id="Write">제안하기</a></h2></li>
                    <li><h2><a id="Mine">나의 제안보기</a></h2></li>
                    <li><h2><a id="All">전체 제안 보기</a></h2></li>
                    <li><h2><a id="Estimate">평가하기</a></h2></li>
                </ul>
            </div>
            
            <div class="menu" id="adminTitle" runat="server"><a href="#" onclick="" >관리자모드</a></div>
            <div class="menusub" id="adminMenu" runat="server" style="display: none">
                <ul>
                    <li><h2><a id="AdminCheck">심사처리</a></h2></li>
                    <li><h2><a id="Item">평가항목 설정</a></h2></li>
                    <li><h2><a id="Judge">1차 심사위원</a></h2></li>
                </ul>
            </div>
        </div>
       </li>
    </ul>    
    </div>

    <span id="layout_treebutton"></span>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AccordionActive = <%=AccActive %>;
        var JudgeAuth = "<%=JudgeAuth %>";
        var AccessAuth = "<%=AccessAuth %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Proposal/Js/Menu/Menu.js");
    </script>
</body>
</html>
