<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Menu.Member" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_workReport.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_workReport.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css?v=1" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="Div1">
        <div class="work_blue_t">
        </div>
        <div class="work_blue_m">
            <ul class="work_inbox">
                <li class="work_gray_int"></li>
                <li class="work_gray_inm">
                    <ComponentArt:Calendar ID="CalendarSingle" MonthSpacing="2" DayNameFormat="Short"
                        CalendarTitleType="VisibleDateText" CalendarTitleDateFormat="yyyy MMMM" NextImageUrl="btn_right.gif"
                        PrevImageUrl="btn_left.gif" TodayDayCssClass="today" MonthCssClass="month" NextPrevCssClass="nextprev"
                        CalendarCssClass="calendar" SelectMonthCssClass="monthselector" SelectWeekCssClass="weekselector"
                        SelectedDayCssClass="selectedday" WeekendDayCssClass="weekend" OtherMonthDayCssClass="othermonthday"
                        DayHoverCssClass="dayhover" DayCssClass="day" DayHeaderCssClass="dayheader" CalendarTitleCssClass="title"
                        AllowDaySelection="true" MonthColumns="1" MonthRows="1" ControlType="Calendar"
                        runat="server" Font-Bold="True" OnVisibleDateChanged="CalendarSingle_VisibleDateChanged"
                        ClientSideOnSelectionChanged="OnSelectionChanged" AutoPostBackOnVisibleDateChanged="true">
                    </ComponentArt:Calendar>
                </li>
                <li class="work_gray_inb"></li>
            </ul>
            <ul class="work_inbox">
                <!-- li class="work_darkgray_int"></li -->
                <li class="work_darkgray_inm">
                    <ul>
                        <div class="work_title">
                            보고자
                            <ul class="btn_searchImg">
                                <li class="blog"><a id="listType2" href="#" class="blog" title="">
                                    <img src="/DeskPlusEIP/Design/C/Img/Btn/list_blog.gif" alt="블로그" /></a></li>
                                <li class="board"><a id="listType1" href="#" class="board" title="">
                                    <img src="/DeskPlusEIP/Design/C/Img/Btn/list_board_on.gif" alt="리스트" /></a></li>
                            </ul>
                        </div>
                        <div class="work_listdiv">
                            ssss
                            <div id="placeholder" style="clear: both;">
                            </div>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <a href="#">
                                                <img src="http://59.150.252.155/publish/mem_01.jpg">홍길동(부장) 보고대기</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a href="#">
                                                <img src="http://59.150.252.155/publish/mem_01.jpg">홍길동(부장) 보고대기</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a href="#">
                                                <img src="http://59.150.252.155/publish/mem_01.jpg">홍길동(부장) 보고대기</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a href="#">
                                                <img src="http://59.150.252.155/publish/mem_01.jpg">홍길동(부장) 보고완료</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a href="#">
                                                <img src="http://59.150.252.155/publish/mem_01.jpg">홍길동(부장) 보고완료</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a href="#">
                                                <img src="http://59.150.252.155/publish/mem_01.jpg">홍길동(부장) 보고완료</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a href="#">
                                                <img src="http://59.150.252.155/publish/mem_01.jpg">홍길동(부장) 보고완료</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ul>
                </li>
                <!-- li class="work_darkgray_inb"></li -->
            </ul>
        </div>
        <div class="work_blue_b">
            &nbsp;</div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var Years = "<%=Years %>";
        var Months = "<%=Months %>";
        var Days = "<%=Days %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Menu/Member.js"></script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Report/Js/Menu/Member.js");
    </script>
</body>
</html>