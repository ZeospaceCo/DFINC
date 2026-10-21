<%@ Page Language="C#" AutoEventWireup="true" Codebehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Schedule.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />     
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
<div class="layout_full">
    <div id="layout_header">
        <!-- 페이지타이틀 //-->
        <h1><asp:Literal ID="ltlMenuName" runat="server"></asp:Literal></h1>
        <!-- 버튼 //-->
        <ul class="btn_right">
            <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
        </ul>
    </div>

    <div id="layout_srh">
        <div class="project_caldate">
            <a href="" class="b_prev" id="pvMonth"><i class="fas fa-angle-left"></i></a>
            <asp:Label ID="ltSelectedYearMonth" runat="server"></asp:Label>
            <a href="" class="b_next" id="nxMonth"><i class="fas fa-angle-right"></i></a>
        </div>
        <ul class="btn_searchbox">
            <li>
            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" CssClass="searchselect form-control"></asp:DropDownList>
            <%=GetResource("Schedule_Year")%>
            <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" CssClass="searchselect form-control"></asp:DropDownList>
            <%=GetResource("Schedule_Month")%>
            &nbsp;
            </li>     
        </ul>
    </div>

    <div id="layout_content">

        <div class="row">
			 <div class="col-12 content-area-body"><!-- width 100%(col-12) -->   

                <!-- 테이블 시작 //-->        
                <div id="placeholder" style="clear: both;">
                    <asp:Calendar ID="CalendarMonth" runat="server" 
                    FirstDayOfWeek="Sunday" 
                    DayNameFormat="Full"
                    ShowTitle="False" 
                    ShowNextPrevMonth="False" 
                    ShowGridLines="false" 
                    Height="100%"
                    Width="100%" 
                    CssClass = "table_typeJ"
                    NextPrevFormat="FullMonth"
                    OnDayRender="CalendarMonth_DayRender"
                    OnSelectionChanged="CalendarMonth_SelectionChanged">
                    <SelectedDayStyle CssClass="j_today" />
                    <TodayDayStyle CssClass="j_today" />                    
                    <OtherMonthDayStyle CssClass="otherMonth" />
                    <WeekendDayStyle CssClass="j_holiday" ForeColor="Red" />
                    </asp:Calendar> 
                </div>        
                <!-- 테이블 끝 //-->      
            </div>
        </div>
    </div>
</div>
</div>   
    <div style="display:none;">
    <asp:LinkButton ID="lbtnBefore" runat="server" OnClick="lbtnBefore_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click"></asp:LinkButton>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ProjParam = "<%=ProjParam%>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Schedule/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Schedule/List.js");
    </script>
</body>
</html>
