<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Project.Menu" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
<form id="form1" runat="server">
    <div id="layout_left" class="left-menu">

        <ul class="left-menu-container">
          <li>
           <div id="layout_treeboxAcc">
                <div class="accordion">
                    <div class="menu"><a href="#" onclick=""><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></a></div>
                    <div class="menusub">
                        <ul>
                            <li><h2><a href="#" onclick="goUrl('Main.aspx?<%=ProjParam %>')"><%=GetResource("Menu_Home") %></a></h2></li>
                            <asp:Repeater ID="rpMenuList" runat="server">
                            <ItemTemplate>
                                <li><h2><a href="#" onclick="goUrl('<%#GetMenuUrl(DataBinder.Eval(Container.DataItem,"MenuID").ToString(), DataBinder.Eval(Container.DataItem,"MenuType").ToString())%>')">
                                <%# GetLangName(
                                            Eval("MenuBase").ToString(),
                                            Eval("MenuKo").ToString(),
                                            Eval("MenuEn").ToString(),
                                            Eval("MenuJa").ToString(),
                                            Eval("MenuFr").ToString(),
                                            Eval("MenuZhcn").ToString(),
                                            Eval("MenuZhtw").ToString(),
                                            PreLang) %></a></h2></li>

                            </ItemTemplate>
                            </asp:Repeater>

                            <li runat="server" id="liMenuRelated"><h2><a href="#" onclick="goUrl('../Related/JList.aspx?<%=ProjParam %>')"><%=GetResource("Menu_Related") %></a></h2></li>
                        </ul>
                    </div>
                    <div class="menu" id="AdminMenu" runat="Server"><a href="#" onclick=""><%=GetResource("Admin_Menu")%></a></div>
                    <div class="menusub" id="AdminMenusub" runat="Server">
                        <ul>
                            <li><h2><a href="#" onclick="goUrl('../Admin/Project/ProjectInfo.aspx?<%=ProjParam %>')"><%=GetResource("Menu_admin1") %></a></h2></li>
                            <li><h2><a href="#" onclick="goUrl('../Admin/Member/JList.aspx?<%=ProjParam %>')"><%=GetResource("Menu_admin2") %></a></h2></li>
                            <li><h2><a href="#" onclick="goUrl('../Admin/Menu/Add.aspx?<%=ProjParam %>')"><%=GetResource("Menu_admin3") %></a></h2></li>
                            <li><h2><a href="#" onclick="Menu.Opinion()"><%=GetResource("Menu_admin4") %></a></h2></li>
                            <li><h2><a href="#" onclick="goUrl('../Template/JList.aspx?<%=ProjParam %>')"><%=GetResource("Menu_admin5") %></a></h2></li>
                            <li><h2><a href="#" onclick="Menu.RequestClosed()"><%=GetResource("Menu_admin6") %></a></h2></li>
                        </ul>
                    </div>
                </div>
              </div>   

           <div id="layout_treebutton">
             <a href="#" id="HomeGo" style="cursor: pointer" class="btn btn-sm"><%=GetResource("Home")%></a>
           </div> 
          </li>
        </ul> 
     </div>      

</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var projectID   = "<%=ProjectID %>";
        var closedAlarm = "<%=GetResource("Project_Menu_ClosedRequest") %>";
        var closedOk    = "<%=GetResource("Project_Menu_ClosedSuccess") %>";
        var errorAlarm  = "<%=GetResource("MSG_FAIL") %>";
        var Project_Menu_Opinion = "<%=GetResource("Project_Menu_Opinion") %>";
        var R_OnlyAdminClose = "<%=GetResource("OnlyAdminClose") %>";
        var projParam   = "<%=ProjParam %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Project/Menu.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Project/Menu.js");
    </script>
</body>
</html>
