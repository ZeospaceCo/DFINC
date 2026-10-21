using System;
using System.Collections.Generic;
using System.Globalization;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    /// <summary>
    /// 작성자   : 심재운
    /// 작성일자 : 2008-09-01
    /// 작성목적 : DateHelperExtension
    /// </summary>
    public static class DateHelperExtension
    {
        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 2008년 9월 3일 월요일, 오후 2:06:02
        /// <para>The string is formatted as "Date, Time"</para>
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string ToDateTimeLongString(this DateTime date)
        {
            return string.Format("{0}, {1}", date.ToLongDateString(), date.ToLongTimeString());
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 2008-09-03, 오후 2:06
        /// <para>The string is formatted as "Date, Time"</para>
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string ToShortDateTimeString(DateTime date)
        {
            return string.Format("{0}, {1}", date.ToShortDateString(), date.ToShortTimeString());
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 9월
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string MonthName(this DateTime date)
        {
            return DateTimeFormatInfo.CurrentInfo.GetMonthName(date.Month);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 월요일
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string DayName(this DateTime date)
        {
            return DateTimeFormatInfo.CurrentInfo.GetDayName(date.DayOfWeek);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 2008-01-01 오전 12:00:00
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime StartOfYear(this DateTime date)
        {
            return DateHelper.GetStartOfYear(date.Year);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 2008-12-31 오후 11:59:59
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime EndOfYear(this DateTime date)
        {
            return DateHelper.GetEndOfYear(date.Year);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 30 (한달 간격일경우)
        /// <para>The current date will be subtracted from the current date and the difference returned</para>
        /// </summary>
        /// <param name="thisdate">The thisdate.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int GetYearsBetweenDate(this DateTime thisdate, DateTime date)
        {
            return DateHelper.GetYearsBetweenDates(thisdate, date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 1 (년도 간격)
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        /// <param name="thisdate">The thisdate.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static double GetTotalYearsBetweenDates(this DateTime thisdate, DateTime date)
        {
            return DateHelper.GetTotalYearsBetweenDates(thisdate, date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 2008-09-01 오전 12:00:00
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime date)
        {
            return DateHelper.GetStartOfMonth(date.Month, date.Year);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 2008-09-30 오후 11:59:59
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return DateHelper.GetEndOfMonth(date.Month, date.Year);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 12 (1년간격일경우, 총월 카운트)
        /// <para>The current date is subtracted form teh specified date and the difference returned</para>
        /// </summary>
        /// <param name="thisdate">The thisdate.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int GetMonthsBetweenDates(this DateTime thisdate, DateTime date)
        {
            if (thisdate > date)
            {
                return DateHelper.GetMonthsBetweenDates(date, thisdate);
            }
            else
            {
                return DateHelper.GetMonthsBetweenDates(thisdate, date);
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : (9월달 첫번째주 일요일) : 2008-08-31 오전 12:00:00
        /// <para>The default start of week is used</para>
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime date)
        {
            return DateHelper.GetStartOfWeek(date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : (이번주 토요일) 2008-09-06 오후 11:59:59
        /// <para>The default start of week is used</para>
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime EndOfWeek(this DateTime date)
        {
            return DateHelper.GetEndOfWeek(date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : (해당년도 총주) : 36
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int GetWeek(this DateTime date)
        {
            return DateHelper.GetWeekOfYear(date);
        }

        /// <summary>
        /// Returns the current week of the year using the specified first day of the week and the FirstFourDayWeek as the calendar week rule
        /// </summary>
        public static int GetWeek(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            return DateHelper.GetWeekOfYear(date, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the current week of the year using the specified first day of the week and the specified CalendarWeekRule
        /// </summary>
        public static int GetWeek(this DateTime date, DayOfWeek firstDayOfWeek, CalendarWeekRule calendarWeekRule)
        {
            return DateHelper.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : (시작일) 2008-09-01 오전 12:00:00
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime date)
        {
            return DateHelper.GetStartOfDay(date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 2008-09-30 오후 11:59:59
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return DateHelper.GetEndOfDay(date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 두날짜 비교일(30)
        /// <para>This date is subtracted from the specified date and the difference returned</para>
        /// </summary>
        /// <param name="thisDate">The this date.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int GetDaysBetweenDates(this DateTime thisDate, DateTime date)
        {
            return DateHelper.GetDaysBetweenDates(thisDate, date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 주말인지 아닌지 체크 (토,일)
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date is weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime date)
        {
            return DateHelper.IsWeekend(date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 년월 값이 동일한지 true, false
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if [is current month and year] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCurrentMonthAndYear(this DateTime date)
        {
            return DateHelper.IsCurrentMonthAndYear(date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 두개의 날짜 년월값이 동일한지
        /// </summary>
        /// <param name="thisDate">The this date.</param>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if [is month and year equal] [the specified this date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMonthAndYearEqual(this DateTime thisDate, DateTime date)
        {
            return DateHelper.IsMonthAndYearEqual(thisDate, date);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 9월1일부터 12월말까지 날짜값이 추출된다.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static List<DateTime> GetRemainingDatesOfYear(this DateTime date)
        {
            return DateHelper.GetDatesBetweenDates(date, date.EndOfYear());
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 명시한 일자부터 12월말까지,dayofweek(일요일) 의 날짜만 추출하기
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns></returns>
        public static List<DateTime> GetRemainingDatesOfYear(this DateTime date, DayOfWeek dayOfWeek)
        {
            return DateHelper.GetDatesBetweenDates(date, date.EndOfYear(), dayOfWeek);
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-09-01
        /// 작성목적 : 두 날짜 사이의 날짜값 추출하기
        /// </summary>
        /// <param name="thisDate">The this date.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static List<DateTime> GetDatesBetween(this DateTime thisDate, DateTime date)
        {
            return DateHelper.GetDatesBetweenDates(thisDate, date);
        }
    }
}