<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Menu.Menu" %>
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

    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Menu/Menu.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var MySchedule = '<%=GetResource("myviewschedule")%>';
        var AllSchedule = '<%=GetResource("pim4")%>';
    </script>
</head>
<body scroll="no">
    <form id="form1" runat="server">
    <!-- layout_body -->
    <div style="padding:0px 0px 0px 10px">
            <div class="layout_menu">
                <div class="blue_t"></div>
                <div class="blue_m">
                    <ul class="inbox">
                    <li class="gray_int"></li><!-- top -->
                    <li class="gray_inm" >
                        <span>
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

                            ClientSideOnSelectionChanged="Menu.OnCalendarChange" 
                            
                            runat="server" Font-Bold="True"
                            >
                            </COMPONENTART:CALENDAR>
                            </span>
                            <ul class="btn_small">
                            <li align="center"><cc3:ClientSmartButton ID="CbtnMonth" runat="server" Text=" 월 간 "  ></cc3:ClientSmartButton></li>
                            <li align="center"><cc3:ClientSmartButton ID="CbtnWeek" runat="server" Text=" 주 간 "  ></cc3:ClientSmartButton></li>
                            <li align="center"><cc3:ClientSmartButton ID="CbtnDay" runat="server" Text=" 일 간 "     ></cc3:ClientSmartButton></li>
                            </ul>
                            <span class="gb_line"></span>
                            <asp:DropDownList ID="DDLShareUser" runat="server" style="width:100%;">
                             
                            </asp:DropDownList>
                            <ul class="btn_small">
                            <li><cc3:ClientSmartButton ID="CbtnShareTotal" runat="server" Text="전체공유일정"></cc3:ClientSmartButton></li>
                            <li><cc3:ClientSmartButton ID="CbtnGetUser" runat="server" Text="공유대상"></cc3:ClientSmartButton></li>
                            </ul>
                            <span class="gb_line"></span>
                            <ul class="btn_small">
                            <li><cc3:ClientSmartButton ID="CbtnTarget" runat="server" Text="기념일관리"></cc3:ClientSmartButton></li>
                            </ul>
                            <div style="height:140px; overflow-y:auto;clear:both;">
                            <ul class="listboxPim" >
                                <div id="divAni"></div>
                            </ul>
                            </div>
                    </li> 
                    <li class="gray_inb"></li><!-- bottom -->
                    </ul>
                </div>
                <div class="blue_b">&nbsp;</div>
            </div>

    </div>
    <!-- // layout_body -->
     
     
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
</body>
</html>
