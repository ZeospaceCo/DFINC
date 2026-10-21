<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Home.Tree" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="BoardTreeScript" Src="../Common/BoardTreeScript.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript">
        var Dcparm = "0";
        var MenuCatID = "0";
        var CatID = "0";
        var AccordionActive = 0;
        var OptionUseDept = "0";
        var MyAuthority = "";
        var OptionListType = "";
        var OptionUseView = "";
        var Types = "";
        var ListTypeUser = "0";
        var UCBoardTreeScript_tvBoardTreeScript = "UCBoardTreeScript1_tvBoardTreeScript";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Home/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Home/Tree.js");
    </script>  
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu"> 
        	<ul class="left-menu-container">
                <li class="writebtn">
                   <a id="register"><div class="btn-write btn btn-lg btn-primary btn-block"><i class="fas fa-plus"></i><%=GetResource("PublishingWriting")%></div></a>
                </li>
                <li id="layout_treeboxAcc">
                    <div class="accordion">
                        <div class="menu  left-main-menu menu ui-state-active"><a href="#" onclick="" ><%=GetResource("Folder")%></a></div>
                        <div class="menusub">
                            <uc1:BoardTreeScript ID="UCBoardTreeScript" runat="server" UseIcon="true" 
                            UseWriteAuthority="false" UseReadAuthority="true" ShowTitleNoAuth="true"
                            UseFolder="true" UseMyTemplate="true" UseMyFavorite="false" SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete">
                            </uc1:BoardTreeScript>
                        </div> 
                        <div class="menu" style="display:none"><%=GetResource("Folder")%></div>            
                    </div>            
                </li>
        	</ul>
    </div>    
    <div style="display: none">        
        <asp:TextBox ID="tbCID" runat="server" Width="0"></asp:TextBox>
        <input type="text" id="hiddenCatID" value="<%= MenuCatID %>" />
    </div>      
    </form>

</body>
</html>
