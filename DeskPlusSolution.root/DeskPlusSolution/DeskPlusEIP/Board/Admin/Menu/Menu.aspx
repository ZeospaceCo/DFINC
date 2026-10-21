<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Menu.Menu" %>
<%@ Register TagPrefix="uc1" TagName="BoardTreeScript" Src="../../Common/Admin/BoardTreeScript.ascx" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var MenuCatID = "<%=MenuCatID %>";
        var CatID = "<%=CatID %>";
        var OptionUseDept = "<%=OptionUseDept %>";
        var FolderListCount = 0;
        var IsTotBoard = "<%=IsTotBoard %>"; //통합게시판 선택한건지 여부
        var IsTotAccount = "<%=IsTotAccount %>"; //통합관리자 여부
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_DeleteFolder = "<%=GetResource("DeleteFolder") %>";
        var R_Description1 = "<%=GetResource("Description1") %>";
        var R_Description2 = "<%=GetResource("Description2") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Menu/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Admin/Menu/Menu.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin"> 
        	<ul class="left-menu-container">
            <li id="layout_treetop"><%=GetResource("BoardManagement")%></li>
            <li id="layout_treeboxAcc">
                <div class="accordion">
                    <div class="menu"><a href="#"><%=GetResource("FolderManagement")%></a></div>
                    <div class="menusub">
                        <uc1:BoardTreeScript ID="UCBoardTreeScript" runat="server" 
                        UseAdmin = "true"
                        SetNodeSelect="onNodeSelect" 
                        ShowTitleNoAuth="true"
                        SetCallbackComplete="onCallbackComplete"></uc1:BoardTreeScript>
                    </div>
                    <div class="menu"><a href="#"><%=GetResource("ContentsManagement")%></a></div>
                    <div class="menusub" style="display:none;">
                        <uc1:BoardTreeScript ID="BoardTreeScript1" runat="server" 
                        UseAdmin = "true"
                        ShowTitleNoAuth="true"
                        SetNodeSelect="onNodeSelectList" SetCallbackComplete="onCallbackCompleteList"></uc1:BoardTreeScript>
                    </div>

                    <div class="menu IsTotBoardManager"><a href="#"><%=GetResource("WholeBoardManager")%></a></div>
                    <div class="menusub IsTotBoardManager" style="display:none;">
                        <uc1:BoardTreeScript ID="UCBoardTotTreeScript" runat="server" 
                        UseAdmin = "true"
                        UseTotBoard = "true"
                        SetNodeSelect="onNodeTotSelect" 
                        ShowTitleNoAuth="true"
                        SetCallbackComplete="onCallbackComplete"></uc1:BoardTreeScript>
                    </div>
                    <div class="menu IsTotBoardManager"><a href="#"><%=GetResource("WholeBoardListManager")%></a></div>
                    <div class="menusub IsTotBoardManager" style="display:none;">
                        <uc1:BoardTreeScript ID="UCBoardTotTreeListScript" runat="server" 
                        UseAdmin = "true"
                        UseTotBoard = "true"
                        ShowTitleNoAuth="true"
                        SetNodeSelect="onNodeSelectList" SetCallbackComplete="onCallbackCompleteList"></uc1:BoardTreeScript>
                    </div>

                    <div class="menu">
                        <a id="chart"><%=GetResource("Statistics")%></a></div>
                    <div class="menusub" style="display:none;">
                        <ul>
                            <!--<li><h1><a id="readCount"><%=GetResource("NoticeCheck")%></a></h1></li>
                            <li><h1><a id="writeCount"><%=GetResource("WriteContent")%></a></h1></li>-->
                            <li><a id="chartSub"><%=GetResource("Statistics")%></a></li>
                        </ul>
                    </div>
                    <div class="menu">
                        <a id="tempManage"><%=GetResource("TempManage")%></a>
                    </div>
                    <div class="menusub" style="display:none;">
                        <ul>
                            <li><a id="tempManageSub"><%=GetResource("TempManage")%></a></li>
                        </ul>
                    </div>
                    <div class="menu">
                        <a id="configManage"><%=GetResource("SettingEnvironment")%></a>
                    </div>
                    <div class="menusub" style="display:none;">
                        <ul>
                            <li><a id="configManageSub"><%=GetResource("SettingEnvironment")%></a></li>
                        </ul>
                    </div>
                </div>               

                <div id="layout_treebutton">
                    <a id="imgDel" style="cursor: pointer" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>
                    <a id="imgAdd" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Add")%></a>  
                    <a id="imgReload" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Refresh")%></a>
                </div>
           </li>
    	   </ul>   
    </div>
    
    <div style="display: none">
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <asp:TextBox ID="tbHdnValue" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbHdnTotValue" runat="server" Width="0"></asp:TextBox>
    </div>
    </form>
</body>
</html>
