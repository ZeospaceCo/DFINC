<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyPim.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.MyPim"  EnableViewState="false" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>


<script type="text/javascript">
		var Pims_tabPortletID = "<%=TabPortletID%>";
		var Pims_portletID = "<%=PortletID%>";
		var Pims_categoryID = "<%=CategoryID%>";
		var Pims_dataURL = "<%=DataURL%>";
		var Pims_dataType = "<%=DataType%>";
		var Pims_IsAuth = "<%=IsAuth%>";
		var Pims_IsMini = "<%=IsMinimize %>";
		var ImagePath = "<%=BaseImgPath%>";
		var PreLang = "<%=PreLang%>";
		var CalendarSingleObj = "<%=CalendarSingle.ClientID %>";    

		var R_InputDate = "<%=GetResource("InputDate") %>";
		var R_NoContents = "<%=GetResource("NoContents") %>";

		function OnCalendarChange(cal) {     
			var obj = eval(CalendarSingleObj);
			var selectedYear = cal.FormatDate(obj.GetSelectedDate(), "yyyy");
			var selectedMonth = cal.FormatDate(obj.GetSelectedDate(), "MM");
			var selectedDay = cal.FormatDate(obj.GetSelectedDate(), "dd");
			//alert(selectedYear + "-" + selectedMonth + "-" + selectedDay);

			ParamArray = {
			  'Date': selectedYear + "-" + selectedMonth + "-" + selectedDay
			};

			CallEncrypt(CallGetParam());

			parent.frmTop.Menu.GoParam("webpims");
			document.location.href = "/DeskPlusEIP/PIM/Default.aspx?p="+GetEncryptValue;
		}

		function DateGopage(arg,dis)
		{  
			var  url ="/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx";
			ParamArray = {
				'ScheduleID': arg,
				'DisplayID': dis
			};
			CallEncrypt(CallGetParam());
			// url+GetEncryptValue;
			WindowObjectReference = PopupWinOpen(url,"p="+ GetEncryptValue, "pimpop", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
		}

</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/MyPim.js"></script>
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
            </div>
         </div>
         
         <div style="display: none">                
                <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
         </div>     
  

        <!--div align="center"  >
        <table width="180px" border="0" cellspacing="0" cellpadding="0" class="Pim-content" >
        <tr>
        <td style="width:4px"></td>
        <td style="width:10px"><img src="<%=BaseImgPath%>/Pims/p1.gif" alt="개인일정"></td>
        <td class="Pimstext">개인</td>
        <td style="width:4px"></td>
        <td style="width:10px"><img src="<%=BaseImgPath%>/Pims/p2.gif" alt="개인일정"></td>
        <td class="Pimstext">부서</td>         
        <td style="width:4px"></td>
        <td style="width:10px"><img src="<%=BaseImgPath%>/Pims/p3.gif" alt="개인일정"></td>
        <td class="Pimstext">공유</td>         
        <td style="width:4px"></td>
        <td style="width:10px"><img src="<%=BaseImgPath%>/Pims/p4.gif" alt="개인일정"></td>
        <td class="Pimstext">전체</td>
        <td style="width:4px"></td>
        </tr>
        </table>
        </div>
        <br />

        <div class="ptl_gap5555">         
        <div id="divsche"></div>

        <div id="divtodo"></div>
        </div-->
</div>
<!-- 포틀릿(일정) End //-->