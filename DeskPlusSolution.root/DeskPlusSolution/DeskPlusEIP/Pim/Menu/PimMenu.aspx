<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PimMenu.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Menu.PimMenu" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Menu/PimMenu.js"></script>
    <script type="text/javascript">
        var CompanyID = '<%=CompanyID %>';
        var UserID = '<%=UserID %>';
        var SelDate = '<%=SelDate %>';

        var Schedule1 = '<%= GetResource("pim1") %>'; //개인일정
        var Schedule2 = '<%= GetResource("pim2") %>'; //부서일정
        var Schedule3 = '<%= GetResource("pim3") %>'; //공유일정
        var Schedule4 = '<%= GetResource("pim4") %>'; //전체일정
        var GUN = '';
        var NowDate = '<%=NowDate %>';
        var PreLang = "<%=PreLang%>";
        var PageScope = '<%=PageScope%>';
        //var AnniversaInquery = "";//기념일조회
        function ChildCommunication(arg) {
            //alert(arg);
            parentpage = arg;
            PimMenu.GetAnniday();
            PimMenu.GetScheduleStatic();
        }
        var parentpage = "";


        function MenuReload() {

            PimMenu.GetScheduleStatic();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="pim_layout_left">
    
    <div  class="left-menu">
        <ul class="pim_inbox">
            <li class="pim_gray_int"></li>
            <li class="pim_gray_inm">

                            <COMPONENTART:CALENDAR 
                            id="CalendarSingle" 
                            MonthSpacing="2" 
                            DayNameFormat="Short" 
                            CalendarTitleType="VisibleDateText"
                            CalendarTitleDateFormat="yyyy MMMM"
                            NextImageUrl="btn_right.gif" 
                            PrevImageUrl="btn_left.gif" 
                            TodayDayCssClass="today"					    
                            MonthCssClass="month"
                            NextPrevCssClass="nextprev" 
                            CalendarCssClass="calendar" 
                            SelectMonthCssClass="monthselector" 
                            SelectWeekCssClass="weekselector"
                            SelectedDayCssClass="selectedday"  
                            WeekendDayCssClass="weekend" 
                            OtherMonthDayCssClass="othermonthday" 
                            DayHoverCssClass="dayhover"
                            DayCssClass="day" 
                            DayHeaderCssClass="dayheader" 
                            CalendarTitleCssClass="title" 
                            AllowDaySelection="true" 
                            MonthColumns="1"
                            MonthRows="1" 
                            ControlType="Calendar" 

                            ClientSideOnSelectionChanged="PimMenu.OnCalendarChange" 
                            
                            runat="server" Font-Bold="True"
                            >
                            </COMPONENTART:CALENDAR>

            </li>
            <li class="pim_gray_inb"></li>
        </ul>   
        <div class="left-menu-container pim_darkgray_inm">
        		<div class="row">
                    <div class="col-6 divscheicon align-center pl-4 pr-1 pt-4"><i class="lni lni-users display-4 text-primary"></i></div>
                    <ul id="divsche" class="col-6">
                    </ul>
                </div>
        </div> 

        <ul class="list_title left-menu-container" id="AniUl">
			<li class="left-main-menu menu"><%= GetResource("AnniversaryInquery") %></li>
            <li class="left-sub-menu menusub "><ul id="divAni" ></ul></li>
        </ul>

    </div>
    </div>
    </form>
</body>
</html>
