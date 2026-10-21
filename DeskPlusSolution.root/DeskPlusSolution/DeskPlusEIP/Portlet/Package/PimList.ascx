<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PimList.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.PimList" EnableViewState="false" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<script type="text/javascript">
		var PimL_tabPortletID = "<%=TabPortletID%>";
		var PimL_portletID = "<%=PortletID%>";
		var PimL_categoryID = "<%=CategoryID%>";
		var PimL_dataURL = "<%=DataURL%>";
		var PimL_dataType = "<%=DataType%>";
		var PimL_IsAuth = "<%=IsAuth%>";
		var PimL_IsMini = "<%=IsMinimize %>";
        var PimL_TmpDate = "<%=TmpDate %>";
        var PimL_NowDate = "<%=NowDate %>";
        var PimL_NextDate = "<%=NextDate %>";
		var ImagePath = "<%=BaseImgPath%>";
		var PreLang = "<%=PreLang%>";
		var PimL_CalendarSingleObj = "<%=CalendarSingle.ClientID %>";   
		var R_NoSchedule = "<%=GetResource("NoSchedule") %>";
        var R_NoTodo = "<%=GetResource("NoTodo") %>";
        var R_Sun = "<%=GetResource("Sun") %>";
        var R_Mon = "<%=GetResource("Mon") %>";
        var R_Tue = "<%=GetResource("Tue") %>";
        var R_Wed = "<%=GetResource("Wed") %>";
        var R_Thu = "<%=GetResource("Thu") %>";
        var R_Fri = "<%=GetResource("Fri") %>";
        var R_Sat = "<%=GetResource("Sat") %>";    
   
	
</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/PimList.js"></script>
<!-- 포틀릿(일정) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-schedule">
    
        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <span class="caption-helper"></span>
            </div>
            <div class="actions">
                <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
            </div>
        </div>

        <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
            <!-- 환경설정 //-->             
            <div class="row">
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                </div>    
            </div>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show" >
            <div class="portlet-contents">
                <div class="content-tabs">
                     <div align="center" style="height:160px;text-align:center;margin:auto;width:90%;padding:10px;">
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
                        ClientSideOnSelectionChanged="OnCalendarChange"                             
                        runat="server" Font-Bold="True"            
                        width="190px">
                        </COMPONENTART:CALENDAR>  
                     </div>
                </div>      
                
                <div class="content-tabs">
                     <ul>
                        <li><span id="sDate1" class="PimsTitle"></span><a href='#' onclick='goPimUrl(1)'><i class="fas fa-plus-circle"></i></a></li>
                        <li class="schedule-list">
                            <ul>
                                <div id="divsche1"></div>
                            </ul>
                            <ul>
                                <div id="divtodo1"></div>   
                            </ul>
                        </li>
                        <li><span id="sDate2" class="PimsTitle"></span><a href='#' onclick='goPimUrl(2)'><i class="fas fa-plus-circle"></i></a></li>
                        <li class="schedule-list">
                            <ul>
                                <div id="divsche2"></div>
                            </ul>
                            <ul>
                                <div id="divtodo2"></div>
                            </ul>
                        </li>
                      </ul>
                </div>    
              </div>
        </div>

         <div style="display: none">                
                <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
         </div>
</div>
<!-- 포틀릿(일정) End //-->
