<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Folder.Tree" %>
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

<div id="layout_left" class="left-menu">
	<ul class="left-menu-container">

            <!--li id="layout_treetop">
                <span class="treetop_title"><%=GetResource("Title1")%></span>
            </li-->
        
            <li id="layout_treeboxAcc" style="height:90%;">
        
                <div class="accordion">
                    
                    <div class="menu"><a id="#"><%=GetResource("MenuF6")%></a></div>
                    <div class="menusub">
                        <ul class="menu_line">
                            <li><h2><a id="IngVoteNon"><%=GetResource("MenuF15")%></a></h2></li>
                            <li><h2><a id="IngVote"><%=GetResource("MenuF14")%></a></h2></li>
                        </ul>
                    </div>
        
                    <div class="menu"><a id="#"><%=GetResource("MenuF7")%></a></div>
                    <div class="menusub">
                        <ul class="menu_line">
                            <li><h2><a id="CloseVoteNon"><%=GetResource("MenuF15")%></a></h2></li>
                            <li><h2><a id="CloseVote"><%=GetResource("MenuF14")%></a></h2></li>
                        </ul>
                    </div>
        
                    <div class="menu"><a id="fav"><%=GetResource("MenuF16")%></a></div>
        
                </div>
            
            </li>

	</ul>
</div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript">
        var ListType = "0";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Folder/Tree.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //        JsCall("/DeskPlusEIP/Opinion/Js/Folder/Tree.js");
    </script> 
</body>
</html>
