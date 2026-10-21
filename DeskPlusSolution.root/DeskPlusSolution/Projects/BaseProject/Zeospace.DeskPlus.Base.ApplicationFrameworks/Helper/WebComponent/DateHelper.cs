using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    /// <summary>
    /// 작성자   : 심재운
    /// 작성일자 : 2008-09-01
    /// 작성목적 : DateHelper
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Returns the first date of the specified year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfYear(int year)
        {
            return new DateTime(year, 1, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of the year provided at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfYear(int year)
        {
            return new DateTime(year, 12, DateTime.DaysInMonth(year, 12), 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the start date of last year with a time of 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfLastYear()
        {
            return GetStartOfYear(DateTime.Now.Year - 1);
        }

        /// <summary>
        /// Returns the last date of last year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfLastYear()
        {
            return GetEndOfYear(DateTime.Now.Year - 1);
        }

        /// <summary>
        /// Returns the first date of the current year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfCurrentYear()
        {
            return GetStartOfYear(DateTime.Now.Year);
        }

        /// <summary>
        /// Returns the last date of the current year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfCurrentYear()
        {
            return GetEndOfYear(DateTime.Now.Year);
        }

        /// <summary>
        /// Returns the first date of next year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfNextYear()
        {
            return GetStartOfYear(DateTime.Now.Year + 1);
        }

        /// <summary>
        /// Returns the last date of next year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfNextYear()
        {
            return GetEndOfYear(DateTime.Now.Year + 1);
        }

        /// <summary>
        /// Returns the number of whole years between two dates
        /// <para>oldDate is subtracted from newDate and the difference returned</para>
        /// </summary>
        public static int GetYearsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            double years = days / 365;
            return (int)years;
        }

        /// <summary>
        /// Returns the number of whole and fractional years between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static double GetTotalYearsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            double years = days / 365;
            return years;
        }

        /// <summary>
        /// Returns the first date of the month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfMonth(int month, int year)
        {
            return new DateTime(year, month, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of the month and year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfMonth(int month, int year)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the first date of last month's month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
            {
                return GetStartOfMonth(12, DateTime.Now.Year - 1);
            }
            else
            {
                return GetStartOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Returns the last date of last month's month and year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
            {
                return GetEndOfMonth(12, DateTime.Now.Year - 1);
            }
            else
            {
                return GetEndOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Returns the first date of current month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfCurrentMonth()
        {
            return GetStartOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        /// <summary>
        /// Returns the last date of current month and year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfCurrentMonth()
        {
            return GetEndOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        /// <summary>
        /// Returns the first date of next month's month and year at 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfNextMonth()
        {
            if (DateTime.Now.Month == 12)
            {
                return GetStartOfMonth(1, DateTime.Now.Year + 1);
            }
            else
            {
                return GetStartOfMonth(DateTime.Now.Month + 1, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Returns the last date of next month's month and year at 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfNextMonth()
        {
            if (DateTime.Now.Month == 12)
            {
                return GetEndOfMonth(1, DateTime.Now.Year + 1);
            }
            else
            {
                return GetEndOfMonth(DateTime.Now.Month + 1, DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Returns the number of whole months between two dates
        /// <para>An average of 30 days per month is used to determine the number of months between the two dates provided.</para>
        /// </summary>
        public static int GetMonthsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            double months = days / 30;
            return (int)months;
        }

        /// <summary>
        /// Returns number of whole and fractional months between two dates
        /// <para>An average of 30 days per month is used to determine the number of months between the two dates provided.</para>
        /// </summary>
        public static double GetTotalMonthsBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            return days / 30;
        }

        /// <summary>
        /// Returns the first date of the week at 00:00:00:000 using the defualt start of week
        /// </summary>
        public static DateTime GetStartOfWeek(DateTime date)
        {
            int daysToSubtract = (int)date.DayOfWeek;
            DateTime tempDate = date.Subtract(TimeSpan.FromDays(daysToSubtract));
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of the week at 23:59:59:999 using the defualt start of week
        /// </summary>
        public static DateTime GetEndOfWeek(DateTime date)
        {
            DateTime tempDate = GetStartOfWeek(date).AddDays(6);
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the first date of last week at 00:00:00:000 using the defualt start of week
        /// </summary>
        public static DateTime GetStartOfLastWeek()
        {
            int daysToSubtract = (int)DateTime.Now.DayOfWeek + 7;
            DateTime tempDate = DateTime.Now.Subtract(TimeSpan.FromDays(daysToSubtract));
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of last week at 23:59:59:999 using the defualt start of week
        /// </summary>
        public static DateTime GetEndOfLastWeek()
        {
            DateTime tempDate = GetStartOfLastWeek().AddDays(6);
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the first date of the current week at 00:00:00:000 using the defualt start of week
        /// </summary>
        public static DateTime GetStartOfCurrentWeek()
        {
            return GetStartOfWeek(DateTime.Now);
        }

        /// <summary>
        /// Returns the last date of the current week at 23:59:59:999 using the defualt start of week
        /// </summary>
        public static DateTime GetEndOfCurrentWeek()
        {
            return GetEndOfWeek(DateTime.Now);
        }

        /// <summary>
        /// Returns the first date of next week at 00:00:00:000 using the defualt start of week
        /// </summary>
        public static DateTime GetStartOfNextWeek()
        {
            DateTime tempDate = GetEndOfCurrentWeek().AddDays(1);
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last date of next week at 23:59:59:999 using the defualt start of week
        /// </summary>
        public static DateTime GetEndOfNextWeek()
        {
            DateTime tempDate = GetStartOfNextWeek().AddDays(6);
            return new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the whole number of weeks between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static int GetWeeksBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            double weeks = days / 7;
            return (int)weeks;
        }

        /// <summary>
        /// Returns the number whole and fractional weeks between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static double GetTotalWeeksBetweenDates(DateTime oldDate, DateTime newDate)
        {
            int days = GetDaysBetweenDates(oldDate, newDate);
            return days / 7;
        }

        /// <summary>
        /// Returns the week of the year where the first week is at least fours long.
        /// <para>The week starts on Sunday</para>
        /// </summary>
        public static int GetWeekOfCurrentYear()
        {
            return GetWeekOfCurrentYear(CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns the current week of the year where the first week is at least fours days long
        /// </summary>
        public static int GetWeekOfCurrentYear(DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfCurrentYear(CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the current week of the year where the week starts on Sunday
        /// </summary>
        public static int GetWeekOfCurrentYear(CalendarWeekRule calendarWeekRule)
        {
            return GetWeekOfCurrentYear(calendarWeekRule, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns the current week of the year
        /// </summary>
        public static int GetWeekOfCurrentYear(CalendarWeekRule calendarWeekRule, DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the week of the year where the first week is at least fours long.
        /// <para>The week starts on Sunday</para>
        /// </summary>
        public static int GetWeekOfYear(DateTime date)
        {
            return GetWeekOfYear(date, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns the week of the year where the first week is at least fours days long
        /// </summary>
        public static int GetWeekOfYear(DateTime date, DayOfWeek firstDayOfWeek)
        {
            return GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the week of the year where the week starts on Sunday
        /// </summary>
        public static int GetWeekOfYear(DateTime date, CalendarWeekRule calendarWeekRule)
        {
            return GetWeekOfYear(date, calendarWeekRule, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns the week of the year
        /// </summary>
        public static int GetWeekOfYear(DateTime date, CalendarWeekRule calendarWeekRule, DayOfWeek firstDayOfWeek)
        {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            return calendar.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the date with a time of 00:00:00:000
        /// </summary>
        public static DateTime GetStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the date with a time of 23:59:59:999
        /// </summary>
        public static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the whole number of days between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static int GetDaysBetweenDates(DateTime oldDate, DateTime newDate)
        {
            TimeSpan ts = (newDate - oldDate);
            return ts.Days;
        }

        /// <summary>
        /// Returns the whole and fractional number of days between two dates
        /// <para>The oldDate is subtracted from the new date and the difference returned</para>
        /// </summary>
        public static double GetTotalDaysBetweenDates(DateTime oldDate, DateTime newDate)
        {
            TimeSpan ts = (newDate - oldDate);
            return ts.TotalDays;
        }

        /// <summary>
        /// Returns the name of the specified month using the Current Culture
        /// </summary>
        public static string GetMonthName(int month)
        {
            return DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
        }

        /// <summary>
        /// Returns the name of the specified DayOfWeek using the current culture
        /// </summary>
        public static string GetDayName(DayOfWeek dayOfWeek)
        {
            return DateTimeFormatInfo.CurrentInfo.GetDayName(dayOfWeek);
        }

        /// <summary>
        /// Returns a long string representation of the specified date and time using the ToLongDateString() and ToLongTimeString()
        /// <para>The string is formatted as "Date, Time"</para>
        /// </summary>
        public static string GetLongDateTimeString(DateTime date)
        {
            return string.Format("{0}, {1}", date.ToLongDateString(), date.ToLongTimeString());
        }

        /// <summary>
        /// Returns a short string representation of the specified date and time using the ToShortDateString() and ToShortTimeString()
        /// <para>The string is formatted as "Date, Time"</para>
        /// </summary>
        public static string GetShortDateTimeString(DateTime date)
        {
            return string.Format("{0}, {1}", date.ToShortDateString(), date.ToShortTimeString());
        }

        /// <summary>
        /// Returns whether the specified date is a Saturday or Sunday
        /// </summary>
        public static bool IsWeekend(DateTime dateTime)
        {
            return (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        /// Indicates whether the specified date is the same year and month as the current date
        /// </summary>
        public static bool IsCurrentMonthAndYear(DateTime date)
        {
            return IsMonthAndYearEqual(DateTime.Now, date);
        }

        /// <summary>
        /// Indicates whether the specified year and month is the same as the current month and year
        /// </summary>
        public static bool IsCurrentMonthAndYear(int year, int month)
        {
            return IsCurrentMonthAndYear(new DateTime(year, month, 1));
        }

        /// <summary>
        /// Indicates whether the specified date is the same as the month and year after the current month
        /// </summary>
        public static bool IsNextMonthAndYear(DateTime date)
        {
            return IsMonthAndYearEqual(DateTime.Now, date);
        }

        /// <summary>
        /// Inficates whether the specified month and year is the same as the month and year after the current month
        /// </summary>
        public static bool IsNextMonthAndYear(int year, int month)
        {
            return IsNextMonthAndYear(new DateTime(year, month, 1));
        }

        /// <summary>
        /// Indicates whether the specified date is the same as the month and year before the current month
        /// </summary>
        public static bool IsPreviousMonthAndYear(DateTime date)
        {
            return IsMonthAndYearEqual(DateTime.Now, date);
        }

        /// <summary>
        /// Indicates whether the specified month and year is the same as the month and year before the current month
        /// </summary>
        public static bool IsPreviousMonthAndYear(int year, int month)
        {
            return IsPreviousMonthAndYear(new DateTime(year, month, 1));
        }

        /// <summary>
        /// Indicates whether the specified dates have the same month and year
        /// </summary>
        public static bool IsMonthAndYearEqual(DateTime dateOne, DateTime dateTwo)
        {
            if (dateOne.Year == dateTwo.Year)
            {
                if (dateOne.Month == dateTwo.Month)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of all the dates between two dates
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetDatesBetweenDates(DateTime beginDate, DateTime endDate)
        {
            List<DateTime> list = new List<DateTime>();
            if (endDate < beginDate)
            {
                return list;
            }

            int days = GetDaysBetweenDates(beginDate, endDate);
            for (int i = 0; i < days; i++)
            {
                DateTime currentDate = beginDate.AddDays(i);
                list.Add(currentDate);
            }

            return list;
        }

        /// <summary>
        /// Returns a list of all the Sundays remaining in the current year
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetSundaysThroughCurrentYear()
        {
            return GetDatesBetweenDates(DateTime.Today, GetStartOfCurrentYear(), DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns a list of Sundays between two dates
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetSundaysBetweenDates(DateTime beginDate, DateTime endDate)
        {
            return GetDatesBetweenDates(beginDate, endDate, DayOfWeek.Sunday);
        }

        /// <summary>
        /// Returns a list of Saturdays remaining in the current year
        /// </summary>
        public static List<DateTime> GetSaturdaysThroughCurrentYear()
        {
            return GetDatesBetweenDates(DateTime.Today, GetStartOfCurrentYear(), DayOfWeek.Saturday);
        }

        /// <summary>
        /// Returns a list of Saturdays between two dates
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetSaturdaysBetweenDates(DateTime beginDate, DateTime endDate)
        {
            return GetDatesBetweenDates(beginDate, endDate, DayOfWeek.Saturday);
        }

        /// <summary>
        /// Returns the number dates between two dates and of the specified day of the week
        /// <para>The begin date must be before the end date</para>
        /// </summary>
        public static List<DateTime> GetDatesBetweenDates(DateTime beginDate, DateTime endDate, DayOfWeek dayOfWeek)
        {
            List<DateTime> list = new List<DateTime>();
            if (endDate < beginDate)
            {
                return list;
            }

            int days = GetDaysBetweenDates(beginDate, endDate);
            for (int i = 0; i < days; i++)
            {
                DateTime currentDate = beginDate.AddDays(i);
                if (currentDate.DayOfWeek == dayOfWeek)
                {
                    list.Add(currentDate);
                }
            }

            return list;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 날자형 데이터를 다국어 날짜에 맞게 문자열로 반환
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_date">The _date.</param>
        /// <param name="_preLangReal">The _pre lang real.</param>
        /// <returns></returns>
        public static string GetCultureDate(DateTime _date, string _preLangReal)
        {
            try
            {
                //6/1/2012 7:28:54 PM
                System.Globalization.CultureInfo cf = UtilHelper.GetGlobal(_preLangReal);
                string[] dateStr = _date.ToString(cf).Split(' ');
                return dateStr[0].PadRight(10);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 날자형 데이터를 다국어 날짜에 맞게 문자열로 반환
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_date">The _date.</param>
        /// <param name="_preLangReal">The _pre lang real.</param>
        /// <returns></returns>
        public static string GetCultureDate(DateTime _date, string _preLangReal, bool _isFull)
        {
            try
            {
                //6/1/2012 7:28:54 PM
                System.Globalization.CultureInfo cf = UtilHelper.GetGlobal(_preLangReal);
                return _date.ToString(cf);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 문자열 날짜를 다국어맞게 YYYYMMDD(dateformat)형태로 반환
        /// 사용법     : DateHelper.GetCultureDate("30/11/2011", "yyyyMMdd", "en-gb"); 결과 (20111130)
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_dateStr">The _date STR.</param>
        /// <param name="_dateFormat">The _date format.</param>
        /// <param name="_preLangReal">The _pre lang real.</param>
        /// <returns></returns>
        public static string GetCultureDate(string _dateStr, string _dateFormat, string _preLangReal)
        {
            try
            {
                _dateStr = _dateStr.Trim();
                if (string.IsNullOrEmpty(_dateStr) || _dateStr.Length == 0) return "";

                System.Globalization.CultureInfo cf = UtilHelper.GetGlobal(_preLangReal);
                DateTime dt = Convert.ToDateTime(_dateStr, cf);
                return dt.ToString(_dateFormat);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 문자열 날짜를 다국어맞게 datetime 반환
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_date">The _date.</param>
        /// <param name="_time">The _time.</param>
        /// <param name="_preLangReal">The _pre lang real.</param>
        /// <returns></returns>
        public static DateTime GetCultureDateConvertString(string _date, string _time, string _preLangReal)
        {
            if (_date.Length == 0) return System.DateTime.MaxValue;
            string dateString = _date;
            if (_time.Length > 0) dateString += " " + _time;

            return Convert.ToDateTime(dateString, UtilHelper.GetGlobal(_preLangReal));
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 슷자형날짜 8자리를 다국어 날짜 문자열로 반환
        /// 용도      : DateHelper.GetCultureDateChar8("20111115", PreLangReal);
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_date">The _date.</param>
        /// <param name="_preLangReal">The _pre lang real.</param>
        /// <returns></returns>
        public static string GetCultureDateChar8(string _dateYMD, string _preLangReal)
        {
            try
            {
                //날자형이 숫자형 8자리 (20110105)
                if (string.IsNullOrEmpty(_dateYMD) || _dateYMD.Trim().Length != 8) return "";

                _preLangReal = _preLangReal.ToLower();

                System.Globalization.CultureInfo cf = UtilHelper.GetGlobal(_preLangReal);

                DateTime dt = new DateTime(
                    _dateYMD.Substring(0, 4).ToNumber<int>(),
                    _dateYMD.Substring(4, 2).ToNumber<int>(),
                    _dateYMD.Substring(6, 2).ToNumber<int>());

                return dt.ToString(cf).Split(' ')[0];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 국가별 월,일 표기하기
        /// </summary>
        /// <param name="_dateYMD"></param>
        /// <param name="_preLangReal"></param>
        /// <returns></returns>
        public static string GetCultureMonthDay(string _dateYMD, string _preLangReal)
        {
            try
            {
                //날자형이 숫자형 8자리 (20110105)
                if (string.IsNullOrEmpty(_dateYMD) || _dateYMD.Trim().Length != 8) return "";

                _preLangReal = _preLangReal.ToLower();

                DateTime dt = new DateTime(
                    _dateYMD.Substring(0, 4).ToNumber<int>(),
                    _dateYMD.Substring(4, 2).ToNumber<int>(),
                    _dateYMD.Substring(6, 2).ToNumber<int>());

                string response = "";

                switch (_preLangReal.ToLower())
                {
                    case "ko":
                        response = dt.Month + "-" + dt.Day;
                        break;

                    case "en":
                        response = dt.Month + "/" + dt.Day;
                        break;

                    case "en-gb":
                        response = dt.Day + "/" + dt.Month;
                        break;

                    case "ja":
                        response = dt.Month + "/" + dt.Day;
                        break;

                    case "fr":
                        response = dt.Day + "/" + dt.Month;
                        break;

                    case "zh-cn":
                        response = dt.Month + "/" + dt.Day;
                        break;

                    case "zh-tw":
                        response = dt.Month + "-" + dt.Day;
                        break;
                }

                return response;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 해당 년월 기준으로 날짜 1주,1달, 시작 및 종료일자 반환하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_fromYMD">The _from YMD.</param>
        /// <param name="_preLangReal">The _pre lang real.</param>
        /// <returns></returns>
        public static string GetCultureStartEndDate(string gubun, string _preLangReal)
        {
            try
            {
                _preLangReal = _preLangReal.ToLower();

                System.Globalization.CultureInfo cf = UtilHelper.GetGlobal(_preLangReal);

                DateTime dtStart = new DateTime();
                DateTime dtEnd = new DateTime();

                if (gubun == "week")
                {
                    dtStart = GetStartOfCurrentWeek();
                    dtEnd = GetEndOfCurrentWeek();
                }

                if (gubun == "month")
                {
                    dtStart = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1);
                    dtEnd = dtStart.AddMonths(1).AddDays(-1);
                }

                string[] dateStrS = dtStart.ToString(cf).Split(' ');
                string[] dateStrE = dtEnd.ToString(cf).Split(' ');

                return dateStrS[0].PadRight(10) + "," + dateStrE[0].PadRight(10);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 해당 년월 기준으로 날짜 6개월,1년, 2년, 3년 시작 및 종료일자 반환하기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-13
        /// </summary>
        /// <param name="_fromYMD">The _from YMD.</param>
        /// <param name="_preLangReal">The _pre lang real.</param>
        /// <returns></returns>
        public static string GetCultureStartEndDatePeriod(string gubun, string _preLangReal)
        {
            try
            {
                _preLangReal = _preLangReal.ToLower();

                System.Globalization.CultureInfo cf = UtilHelper.GetGlobal(_preLangReal);

                DateTime dtStart = new DateTime();
                DateTime dtEnd = new DateTime();

                if (gubun == "6M")
                {                    
                    dtEnd = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
                    dtStart = dtEnd.AddMonths(-6);
                }

                if (gubun == "1Y")
                {
                    dtEnd = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
                    dtStart = dtEnd.AddYears(-1);
                }

                if (gubun == "2Y")
                {
                    dtEnd = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
                    dtStart = dtEnd.AddYears(-2);
                }

                if (gubun == "3Y")
                {
                    dtEnd = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
                    dtStart = dtEnd.AddYears(-3);
                }

                string[] dateStrS = dtStart.ToString(cf).Split(' ');
                string[] dateStrE = dtEnd.ToString(cf).Split(' ');

                return dateStrS[0].PadRight(10) + "," + dateStrE[0].PadRight(10);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 날짜 검색에 따른 다국어 날짜형태 년월일을 숫자형 yyyyMMdd 변환하기
        /// 검색 날짜 (0 : 시작일자 ; 1 : 종료일자)
        /// </summary>
        /// <param name="_searchTerm">The _search term.</param>
        /// <param name="_searchTermValue">The _search term value.</param>
        /// <param name="_preLangReal">The _pre lang real.</param>
        /// <returns></returns>
        public static string[] GetSearchTerm(string _searchTerm, string _searchTermValue, string _preLangReal)
        {
            try
            {
                string[] dateValue = new string[2];
                string getDate = "";

                //주간 및 월간 검색
                if (_searchTerm == "week" || _searchTerm == "month")
                {
                    if (_searchTerm == "week")
                        getDate = DateHelper.GetCultureStartEndDate("week", _preLangReal);
                    else if (_searchTerm == "month")
                        getDate = DateHelper.GetCultureStartEndDate("month", _preLangReal);

                    dateValue[0] = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", _preLangReal);
                    dateValue[1] = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", _preLangReal);
                }
                else if (_searchTerm == "period") //사용자 입력시
                {
                    dateValue[0] = DateHelper.GetCultureDate(_searchTermValue.Split(',')[0], "yyyyMMdd", _preLangReal);
                    dateValue[1] = DateHelper.GetCultureDate(_searchTermValue.Split(',')[1], "yyyyMMdd", _preLangReal);
                }

                return dateValue;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 심재운
        /// 월별 월, 금 날짜 리스트
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="_weekCnt">The _week CNT.</param>
        /// <returns></returns>
        public static Dictionary<DateTime, DateTime> GetMonFriByYearMonth(int year, int month, int _weekCnt)
        {
            string yearmonth = month.ToString().Length == 1 ? year.ToString() + "0" + month.ToString() : year.ToString() + month.ToString();
            var getMonFriList = new Dictionary<DateTime, DateTime>();

            #region 이전달

            var beforeMDic = GetBeforeMonthMonFriByYearMonth(year, month);//이전달

            foreach (KeyValuePair<DateTime, DateTime> dic in beforeMDic)
            {
                string startDate = dic.Key.ToString("yyyyMMdd");
                string endDate = dic.Value.ToString("yyyyMMdd");

                if (startDate.IndexOf(yearmonth) != -1 || endDate.IndexOf(yearmonth) != -1) getMonFriList.Add(dic.Key, dic.Value);
            }

            #endregion 이전달

            #region 금월

            List<DateTime> getMonList = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .Where(dt => dt.DayOfWeek == DayOfWeek.Monday)
                             .ToList();

            int i = 1;

            //몇주의 값을 알고자 할 경우
            if (_weekCnt > 0)
            {
                foreach (DateTime dt in getMonList)
                {
                    if (i == _weekCnt)
                    {
                        getMonFriList.Add(dt, dt.AddDays(4));//월,금
                        break;
                    }

                    i += 1;
                }
            }
            else
            {
                foreach (DateTime dt in getMonList)
                    getMonFriList.Add(dt, dt.AddDays(4));//월,금
            }

            #endregion 금월

            #region 다음달

            var nextMDic = GetNextMonthMonFriByYearMonth(year, month);//다음달

            foreach (KeyValuePair<DateTime, DateTime> dic in nextMDic)
            {
                string startDate = dic.Key.ToString("yyyyMMdd");
                string endDate = dic.Value.ToString("yyyyMMdd");

                if (startDate.IndexOf(yearmonth) != -1 || endDate.IndexOf(yearmonth) != -1) getMonFriList.Add(dic.Key, dic.Value);
            }

            #endregion 다음달

            return getMonFriList;
        }

        /// <summary>
        /// 이전 달의 마지막 주
        /// 심재운
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Dictionary<DateTime, DateTime> GetBeforeMonthMonFriByYearMonth(int year, int month)
        {
            if (month == 1)
            {
                month = 12;
                year -= 1;
            }
            else
            {
                month -= 1;
            }

            List<DateTime> getMonList = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .Where(dt => dt.DayOfWeek == DayOfWeek.Monday)
                             .ToList();

            var getMonFriList = new Dictionary<DateTime, DateTime>();
            DateTime getLast = getMonList.Last();
            getMonFriList.Add(getLast, getLast.AddDays(4));//월,금

            return getMonFriList;
        }

        /// <summary>
        /// 다음 달의 첫번째 주
        /// 심재운
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Dictionary<DateTime, DateTime> GetNextMonthMonFriByYearMonth(int year, int month)
        {
            if (month == 12)
            {
                month = 1;
                year += 1;
            }
            else
            {
                month += 1;
            }

            List<DateTime> getMonList = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .Where(dt => dt.DayOfWeek == DayOfWeek.Monday)
                             .ToList();

            var getMonFriList = new Dictionary<DateTime, DateTime>();
            DateTime getLast = getMonList.First();
            getMonFriList.Add(getLast, getLast.AddDays(4));//월,금

            return getMonFriList;
        }

        #region 음력을 양력으로 치환 (한국용)

        /// <summary>
        ///  요일을 숫자료 변환
        /// </summary>
        /// <param name="_dow"></param>
        /// <returns></returns>
        public static int GetCountOfWeek(DayOfWeek _dow)
        {
            int rtn = 0;

            switch (_dow)
            {
                case DayOfWeek.Sunday: rtn = 0; break;
                case DayOfWeek.Monday: rtn = 1; break;
                case DayOfWeek.Tuesday: rtn = 2; break;
                case DayOfWeek.Wednesday: rtn = 3; break;
                case DayOfWeek.Thursday: rtn = 4; break;
                case DayOfWeek.Friday: rtn = 5; break;
                case DayOfWeek.Saturday: rtn = 6; break;
            }

            return rtn;
        }

        /// <summary>
        /// 월별 날짜수 계산
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetDayCountOfMonth(int year, int month)
        {
            int leapYear = 0;
            int rtn = 0;

            //윤년 체크
            if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0)) leapYear = 1;

            switch (month)
            {
                case 1: rtn = 31; break;
                case 2: rtn = 28 + leapYear; break;
                case 3: rtn = 31; break;
                case 4: rtn = 30; break;
                case 5: rtn = 31; break;
                case 6: rtn = 30; break;
                case 7: rtn = 31; break;
                case 8: rtn = 31; break;
                case 9: rtn = 30; break;
                case 10: rtn = 31; break;
                case 11: rtn = 30; break;
                case 12: rtn = 31; break;
                default: rtn = -1; break;
            }
            return rtn;
        }

        /// <summary>
        /// 음력을 양력으로 변환
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="isLeapMonth"></param>
        /// <returns></returns>
        public static DateTime LunarToSolar(int year, int month, int day, bool isLeapMonth)
        {
            EastAsianLunisolarCalendar kLunar = new KoreanLunisolarCalendar();
            int leapMonth = 0;

            // 윤달이 존재하는지 체크
            if (kLunar.GetMonthsInYear(year) > 12)
            {
                leapMonth = kLunar.GetLeapMonth(year);

                if (isLeapMonth)
                {
                    month++;
                }
                else if (month > leapMonth - 1)
                {
                    month++;
                }
            }

            DateTime solDate = kLunar.ToDateTime(year, month, day, 0, 0, 0, 0);

            return solDate;
        }

        #endregion 음력을 양력으로 치환 (한국용)
    }
}