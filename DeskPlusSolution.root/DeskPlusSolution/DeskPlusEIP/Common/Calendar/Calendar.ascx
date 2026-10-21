<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Calendar.ascx.cs" Inherits="Zeospace.DeskPlus.Common.Web.Calendar.Calendar" targetSchema="http://schemas.microsoft.com/intellisense/nav4-0"%>
<asp:calendar id="CalendarSingle" CssClass="table_typeB" CellPadding="1" CellSpacing="1"  runat="server" OnDayRender="CalendarSingle_DayRender">
	<TodayDayStyle ForeColor="#FFA707" CssClass="TodayDayStyle"></TodayDayStyle>
	<SelectorStyle CssClass="SelectorStyle"></SelectorStyle>
	<NextPrevStyle  CssClass="NextPrevStyle"></NextPrevStyle>
	<DayHeaderStyle  CssClass="DayHeaderStyle"></DayHeaderStyle>
	<DayStyle ForeColor="#5F5F5F" CssClass="DayStyle"/>
	<SelectedDayStyle CssClass="SelectedDayStyle"></SelectedDayStyle>
	<TitleStyle CssClass="TitleStyle"></TitleStyle>
	<OtherMonthDayStyle ForeColor="#E0E0E0" CssClass="OtherMonthDayStyle"></OtherMonthDayStyle>
</asp:calendar>
