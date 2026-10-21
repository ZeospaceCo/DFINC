<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Menu.Tree" %>

<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css"/>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AccordionActive = <%=AccActive %>;
        var FolderStatusID  = "<%=FolderStatusID%>";
        var DefaultMenuID   = "<%=DefaultMenuID%>";
        var SearchTerm      = "<%=SearchTerm %>";       //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>";  //날자값        
        var SearchField     = "<%=SearchField %>";      //검색키워드
        var SearchValue     = "<%=SearchValue %>";      //검색어
        var CurrentPage     = "<%=CurrentPage %>";      //현재페이지
        var Order           = "<%=Order %>";            //정렬

        var menuID          = "";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/HR/Js/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu">
        <ul class="left-menu-container">    
            <li>
            <div id="layout_treeboxAcc" class="left-menu-container">
    
                <div class="accordion">                
                    <div class="menu left-main-menu menu"><a href="#" onclick="" ><%=GetResource("HRStatus")%></a></div>                
                    <div class="menusub">                    
                        <zeoHRFolderStatusTree:UCHRFolderStatusTreeScript ID="UCFolderStatusTreeScript1" SetCallbackComplete="onCallbackComplete" SetNodeSelect="onNodeSelect" 
                        SetFolderStatusList = "" UseCheckBox="false" runat="server" />
                    </div>
                    <div class="menu  left-main-menu menu" id="MenuAdmin" runat="server"><a href="#" onclick="" ><asp:Literal ID="ltlMenuAdmin" runat="server"></asp:Literal></a></div>
                    <div class="menusub" id="MenuSubAdmin" runat="server">
                        <ul>
                            <li><h2><a id="ListPrivateMod"><%=GetResource("HRMod")%></a></h2></li>
                            <li id="MenuSubBaseDept" runat="server"><h2><a id="ListUserDept"><asp:Literal ID="ltlSubUserDept" runat="server"></asp:Literal></a></h2></li>
                        </ul>
                    </div>
                    <div class="menu  left-main-menu menu"><a href="#" onclick="" ><%=GetResource("HoliList")%></a></div>
                    <div class="menusub">
                        <ul>
                            <li><h2><a id="HoliListUser"><%=GetResource("HoliList")%></a></h2></li>
                        </ul>
                    </div>
                    <div class="menu left-main-menu menu"><a href="#" onclick="" ><%=GetResource("WorkList")%></a></div>
                    <div class="menusub">
                        <ul>
                            <li><h2><a id="WorkListUser"><%=GetResource("WorkList")%></a></h2></li>
                        </ul>
                    </div>
                </div> <!-- end accordion  -->   
	            <div id="layout_treebuttom"></div>

            </div> <!-- end layout_treeboxAcc  -->   
            </li>
   	    </ul>
    </div><!-- end layout_left  -->
    </form>
</body>
</html>
