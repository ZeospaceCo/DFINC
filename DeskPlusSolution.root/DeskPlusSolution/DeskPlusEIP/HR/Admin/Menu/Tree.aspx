<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Admin.Menu.Tree" %>

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
        var MenuID = "<%=MenuID %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Admin/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/HR/Js/Admin/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div id="layout_left" class="left-menu admin">
        <ul class="left-menu-container">
        	<li>
                    <div id="layout_treetop">
                        <span class="treetop_title"><%=GetResource("WorkManager")%></span>
                    </div>
            
                    <div id="layout_treeboxAcc" style="height: 90%;">
            
                        <div class="accordion">
                            <span></span>
                            <div class="menusub">
                                <ul class="menu_line">
                                    <li><h2><a id="HoliAdmin"><%=GetResource("HoliAdmin")%></a></h2></li>
                                    <li><h2><a id="HoliAdminManage"><%=GetResource("HoliAdminManage")%></a></h2></li>
                                    <li><h2><a id="OverTimeAdmin"><%=GetResource("OverTimeAdmin")%></a></h2></li>
                                    <li><h2><a id="WorkAdmin"><%=GetResource("WorkAdmin")%></a></h2></li>
                                    <li><h2><a id="RoleAdmin"><%=GetResource("RoleAdmin")%></a></h2></li>
                                    <li><h2><a id="AttendanceRoleAdmin"><%=GetResource("AttendanceRoleAdmin")%></a></h2></li>
                                </ul>
                            </div>
                        </div>
            
                    </div>
       		</li>
    	</ul>
    </div>
    </form>
</body>
</html>