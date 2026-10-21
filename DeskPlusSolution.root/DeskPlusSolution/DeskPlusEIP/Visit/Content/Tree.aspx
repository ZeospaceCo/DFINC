<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Visit.Web.Content.Tree" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Visit.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Visit.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>
    <script type="text/javascript">
        var AccordionActive = 0;
        var CalendarDate = "<%=NowDate%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Visit/Js/Content/Tree.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Visit/Js/Content/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
	   <div id="layout_left" class="left-menu">
         <ul class="menu_inbox">          
            <li class="menu_gray_inm">
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
                        ClientSideOnSelectionChanged="Tree.OnCalendarChange" 
                        runat="server" Font-Bold="True"  width="190px" >
                    </COMPONENTART:CALENDAR>
            </li>              
          </ul>            
       </div>     
    </form>
</body>
</html>
