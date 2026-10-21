<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.Menu.Menu" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Etc.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Etc.css"/>
</head>
<body>
    <form id="form1" runat="server">

    <div id="layout_left">

        <div id="layout_treetop">
            <span class="treetop_title">비용정산</span>
        </div>

        <div id="layout_treeboxAcc" style="height:90%;">
            <div class="accordion">
                <div class="menu" style="display:none"></div>
                <div class="menusub">   
                    <ul class="menu_line">
                        <li><h2><a id="LegacyAdd">비용 정산 입력</a></h2></li>
                        <li><h2><a id="LegacyList">비용 정산 리스트</a></h2></li>
                        <li><h2><a id="LegacyAdmin">비용 확정(관리자)</a></h2></li>
                        <li><h2><a id="CardListAdmin">법인카드 사용내역</a></h2></li>
                        <li style="display:none;"><h2><a id="KRPIA">Promotion 비용정산</a></h2></li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="layout_treebuttom"></div>
    </div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
        var AdminRole = "<%=AdminRole %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/Menu/Menu.js");
    </script> 

</body>
</html>

