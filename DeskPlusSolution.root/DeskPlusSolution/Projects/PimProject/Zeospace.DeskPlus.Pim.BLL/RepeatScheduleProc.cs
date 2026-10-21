using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public class RepeatScheduleProc
    {
        #region 변수
        BE.RepeatScheduleInfo info = new BE.RepeatScheduleInfo();
        private string CheckDate = string.Empty;

        private List<BE.DisplayInfo> listinfo = new List<BE.DisplayInfo>();
        #endregion

        #region 기본
        public RepeatScheduleProc()
        {
        }
        public RepeatScheduleProc(BE.RepeatScheduleInfo _info)
        {
            info = _info;
        }

        //Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday
        #endregion

        #region Text파싱
        /// <summary>
        /// 주간
        /// </summary>
        /// <returns></returns>
        //public string MakeWeek()
        //{
        //    string strRt = string.Empty;
        //    if (info.IntervalCnt == 1)
        //        strRt = "매주마다";
        //    else
        //        strRt = info.IntervalCnt + "주마다";

        //    if (info.Option1.Equals("1"))
        //        strRt += " 일";
        //    if (info.Option2.Equals("1"))
        //        strRt += " 월";
        //    if (info.Option3.Equals("1"))
        //        strRt += " 화";
        //    if (info.Option4.Equals("1"))
        //        strRt += " 수";
        //    if (info.Option5.Equals("1"))
        //        strRt += " 목";
        //    if (info.Option6.Equals("1"))
        //        strRt += " 금";
        //    if (info.Option7.Equals("1"))
        //        strRt += " 토";


        //    strRt += " 요일에";

        //    if (info.EndDate.ToString("yyyyMMdd") != "00010101")
        //        strRt += " " + info.EndDate.ToString("yyyy-MM-dd") + "까지 반복합니다.";

        //    if (info.MaxReptCnt != 0)
        //        strRt += " " + info.MaxReptCnt.ToString() + "회 반복합니다.";

        //    return strRt;
        //}

        //public string MakeDay()
        //{
        //    string strRt = string.Empty;
        //    if (info.IntervalCnt == 1)
        //    {
        //        strRt = "매일마다 ";
        //    }
        //    else
        //    {
        //        strRt = info.IntervalCnt + "일마다";
        //    }

        //    if (info.EndDate.ToString("yyyyMMdd") != "00010101")
        //        strRt += " " + info.EndDate.ToString("yyyy-MM-dd") + "까지 반복합니다.";


        //    if (info.MaxReptCnt != 0)
        //        strRt += " " + info.MaxReptCnt.ToString() + "회 반복합니다.";



        //    return strRt;
        //}

        //public string MakeMonth()
        //{
        //    string strRt = string.Empty;
        //    if (info.IntervalCnt == 1)
        //    {
        //        strRt = "매월 ";
        //    }
        //    else
        //    {
        //        strRt = info.IntervalCnt + "개월마다";
        //    }



        //    //strRt += " " + this.Option1 + "  ";
        //    //if(Option2
        //    if (info.SelectType == "1")
        //    {

        //        strRt += " " + info.Option1 + "일에 ";
        //    }
        //    else if (info.SelectType == "2")
        //    {

        //        strRt += " " + info.Option1 + "번째   ";

        //        //평일,주말,일,월,화,수,목,금
        //        switch (info.Option2)
        //        {
        //            case "0": //날
        //                strRt += "날에";
        //                break;
        //            case "1": //평일
        //                strRt += "평일에";
        //                break;
        //            case "2": //주말
        //                strRt += "주말에";
        //                break;
        //            case "3": //일
        //                strRt += "일요일에";
        //                break;
        //            case "4": //월
        //                strRt += "월요일에";
        //                break;
        //            case "5": //화
        //                strRt += "화요일에";
        //                break;
        //            case "6"://수
        //                strRt += "수요일에";
        //                break;
        //            case "7"://목
        //                strRt += "목요일에";
        //                break;
        //            case "8"://금
        //                strRt += "금요일에";
        //                break;
        //            case "9"://토
        //                strRt += "토요일에";
        //                break;

        //        }

        //    }

        //    if (info.EndDate.ToString("yyyyMMdd") != "00010101")
        //        strRt += " " + info.EndDate.ToString("yyyy-MM-dd") + "까지 반복합니다.";


        //    if (info.MaxReptCnt != 0)
        //        strRt += " " + info.MaxReptCnt.ToString() + "회 반복합니다.";

        //    return strRt;
        //}

        #endregion

        #region 날짜세팅

        private int SetDay(DateTime tartgetday, DateTime start)
        {

            if (info.IntervalCnt == -1)
            {
                PlotDateInfo(tartgetday);
                return 1;
            }
            else
            {

                TimeSpan result = tartgetday - start;

                double r2 = result.TotalDays % double.Parse(info.IntervalCnt.ToString());

                if (r2 == 0)
                {
                    PlotDateInfo(tartgetday);
                    return 1;
                }

            }
            return 0;
        }

        /// <summary>
        ///  안씀
        /// </summary>
        /// <param name="tartgetday"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private int SetWeek(DateTime tartgetday, DateTime start)
        {
            if (info.IntervalCnt == 1)
            {
                //tartgetday
                //Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday

                if (ChecKWeekDay(tartgetday.DayOfWeek.ToString()))
                {
                    PlotDateInfo(tartgetday);
                    return 1;
                }

            }
            else
            {
                int offset = Convert.ToInt32(start.DayOfWeek);
                DateTime intervalStartDay = start.AddDays(-(offset)); // 그주의 일요일로 변경

                int iStart = GetWeekOfYear(start, null);
                int iEnd = GetWeekOfYear(tartgetday, null);
                int irt = iStart - iEnd;

                TimeSpan result = tartgetday - start;
                double r2 = result.TotalDays % double.Parse((info.IntervalCnt * 7).ToString());


                if (r2 == 0)
                {
                    if (ChecKWeekDay(tartgetday.DayOfWeek.ToString()))
                    {
                        PlotDateInfo(tartgetday);
                        return 1;
                    }

                }

            }

            return 0;
        }

        private int SetMonth(DateTime targetday, DateTime start)
        {

            if (info.SelectType == "1")
            {

                if (int.Parse(info.Option1) == targetday.Day)
                {
                    PlotDateInfo(targetday);
                    return 1;
                }
            }
            else if (info.SelectType == "2")
            {
                if (info.IntervalCnt == 1)//매달
                {
                    if (info.Option1 == "1") //첫째
                    {
                        if (CheckMonthDay(targetday))
                        {
                            PlotDateInfo(targetday);
                            return 1;
                        }
                    }
                }
                else  //2,3번달
                {
                    if (CheckDate.IndexOf(targetday.ToString("yyyyMM")) > -1)
                    {
                        if (CheckMonthDay(targetday))
                        {
                            PlotDateInfo(targetday);
                            return 1;
                        }
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 주간반복
        /// </summary>
        public void CalcWeekDayCount()
        {

            //int interval = 0;

            if (info.EndDate.ToString("yyyyMMdd") == "00010101")
            {
                info.EndDate = DateTime.Parse("2030-12-31");
            }
            if (info.MaxReptCnt == 0)
            {
                info.MaxReptCnt = -1;
            }


            if (info.IntervalCnt == 0)
            {
                info.IntervalCnt = 1;
            }

            DateTime dtFromTime = info.StartDate;
            DateTime dtToTime = info.EndDate;
            DateTime dtCalcRangeTime = dtFromTime;
            DateTime SomeDayInWeek;

            int repetitionCount = info.MaxReptCnt;

            int offsetFromSunday = Convert.ToInt32(dtCalcRangeTime.DayOfWeek);

            DateTime intervalStartDay = dtCalcRangeTime.AddDays(-(offsetFromSunday)); // 그주의 일요일로 변경

            while (intervalStartDay <= dtToTime)
            {
                SomeDayInWeek = intervalStartDay;
                for (int i = 0; i < 7; i++)
                {

                    if (info.MaxReptCnt != 0)
                    {
                        if (ChecKWeekDay(SomeDayInWeek.DayOfWeek.ToString()))
                        {
                            PlotDateInfo(SomeDayInWeek);
                            if (info.MaxReptCnt != -1)
                            {
                                info.MaxReptCnt = info.MaxReptCnt - 1;
                            }
                        }
                    }

                    SomeDayInWeek = SomeDayInWeek.AddDays(1);
                }

                intervalStartDay = intervalStartDay.AddDays(info.IntervalCnt * 7);
            }


        }


        #endregion

        #region 공통함수1


        /// <summary>
        /// 해당달이  맞는지 확인
        /// </summary>
        /// <returns></returns>
        private void ValidateMonth(DateTime startdate)//,DateTime  targetdate)
        {
            int iLimit = int.Parse(info.EndDate.ToString("yyyyMM"));
            int iStart = int.Parse(startdate.ToString("yyyyMM"));
            int irt = iLimit - iStart;
            DateTime tempdate = startdate;
            CheckDate = startdate.ToString("yyyyMM");

            for (int i = 0; i <= irt; i++)
            {
                tempdate = tempdate.AddMonths(info.IntervalCnt);
                string temp = tempdate.ToString("yyyyMM");

                if (CheckDate.IndexOf(temp) == -1)
                    CheckDate += temp + ";";

            }

        }

        private bool CheckMonthDay(DateTime targetday)
        {
            switch (info.Option1)
            {
                case "1"://날
                    if (targetday.Day == 1)
                    {
                        //PlotDateInfo(targetday);
                        return true;
                    }
                    break;
                case "2"://평일
                    if (targetday.DayOfWeek.ToString() != "Saturday" && targetday.DayOfWeek.ToString() != "Sunday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM")) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM;");
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
                case "3": //주말
                    if (targetday.DayOfWeek.ToString() == "Saturday" || targetday.DayOfWeek.ToString() == "Sunday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString()) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString() + ";";
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
                case "4"://일
                    if (targetday.DayOfWeek.ToString() == "Sunday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString()) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString() + ";";
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
                case "5"://월
                    if (targetday.DayOfWeek.ToString() == "Monday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString()) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString() + ";";
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
                case "6"://화
                    if (targetday.DayOfWeek.ToString() == "Tuesday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString()) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString() + ";";
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
                case "7"://수
                    if (targetday.DayOfWeek.ToString() == "Wednesday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString()) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString() + ";";
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
                case "8"://목
                    if (targetday.DayOfWeek.ToString() == "Thursday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString()) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString() + ";";
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
                case "9"://금
                    if (targetday.DayOfWeek.ToString() == "Friday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString()) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString() + ";";
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
                case "10"://토
                    if (targetday.DayOfWeek.ToString() == "Saturday")
                    {
                        if (CheckDate.IndexOf(targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString()) == -1)
                        {
                            CheckDate += targetday.ToString("yyyy-MM") + targetday.DayOfWeek.ToString() + ";";
                            //PlotDateInfo(targetday);
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        private bool ChecKWeekDay(string dal)
        {
            //Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday

            if (info.Option1.Equals("1"))
            {
                if (dal.Equals("Sunday"))
                    return true;
            }
            if (info.Option2.Equals("1"))
            {
                if (dal.Equals("Monday"))
                    return true;
            }
            if (info.Option3.Equals("1"))
            {
                if (dal.Equals("Tuesday"))
                    return true;
            }
            if (info.Option4.Equals("1"))
            {
                if (dal.Equals("Wednesday"))
                    return true;
            }
            if (info.Option5.Equals("1"))
            {
                if (dal.Equals("Thursday"))
                    return true;
            }
            if (info.Option6.Equals("1"))
            {
                if (dal.Equals("Friday"))
                    return true;
            }
            if (info.Option7.Equals("1"))
            {
                if (dal.Equals("Saturday"))
                    return true;
            }

            return false;
        }



        /// <summary>
        /// 최종날짜 포팅
        /// </summary>
        /// <param name="dt"></param>
        public void PlotDateInfo(DateTime dt)
        {
            BE.DisplayInfo _be = new BE.DisplayInfo();
            _be.StartDate = dt;
            _be.EndDate = dt;
            _be.strStartDate = dt.ToString("yyyy-MM-dd HH:mm");
            listinfo.Add(_be);
            //Console.WriteLine(dt.ToString("yyyy-MM-dd") + " " + dt.DayOfWeek.ToString());
        }

        public List<BE.DisplayInfo> GetDateInfo()
        {
            return this.listinfo;
        }

        /// <summary>
        /// 날짜 세팅
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void RepeatDate()
        {
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            start = info.StartDate;
            end = info.EndDate;

            CheckDate = string.Empty;
            int cnt = 0;

            if (info.MaxReptCnt != 0)
            {
                end = DateTime.Parse("2030-12-31");
            }
            //if (MaxReptCnt != "0")
            //{
            //    end = DateTime.Parse("2999-12-30");
            //}
            if (info.RepeatType == "M") //달 반복일경우
                ValidateMonth(start);

            for (DateTime i = start; i <= end; i = i.AddDays(1))
            {
                if (info.MaxReptCnt != 0)
                {
                    if (info.MaxReptCnt <= cnt)
                    {
                        //Console.WriteLine("종료");
                        return;
                    }
                }

                //int c = GetCurrentWeekOfMonth(i, null); //몇번째 주인지


                switch (info.RepeatType)
                {
                    case "D"://일간
                        cnt += SetDay(i, start);
                        //Console.WriteLine(cnt);
                        break;
                    case "W": //주간
                        cnt += SetWeek(i, start);
                        break;
                    case "M": //월간
                        cnt += SetMonth(i, start);
                        break;
                }
            }
        }

        #endregion

        #region 공통함수2

        /// <summary>
        /// 해당년도에 몇번쨰 주인지
        /// </summary>
        /// <param name="targetDate"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime targetDate, CultureInfo culture)
        {
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }
            CalendarWeekRule weekRule = culture.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
            return culture.Calendar.GetWeekOfYear(targetDate, weekRule, firstDayOfWeek);
        }

        /// <summary>
        /// 총년도 몇번쨰주
        /// </summary>
        /// <param name="targetDate"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static int GetCurrentWeekOfMonth(DateTime targetDate, CultureInfo culture)
        {
            DateTime firstDayOfMonth = System.DateTime.Parse(targetDate.ToString("yyyy-MM-01"));
            int firstWeekOfMonth = GetWeekOfYear(firstDayOfMonth, culture);
            int nowWeekOfMonth = GetWeekOfYear(targetDate, culture);

            return (nowWeekOfMonth - firstWeekOfMonth) + 1;
        }
        #endregion

        #region 달반복
        /// <summary>
        /// 매달 특정일에 회수 제한 
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="end"></param>
        public void calcMonth1RepetitionByCount()
        {
            DateTime dtFromTime = info.StartDate;
            DateTime dtToTime = info.EndDate;
            DateTime dtCalcRangeTime = dtFromTime;
            DateTime theDayEveryMonth;

            int repetitionCount = info.MaxReptCnt;
            int interval = info.IntervalCnt;
            int everyThisDay = int.Parse(info.Option1);

            for (int i = 0; repetitionCount > i; )
            {
                theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, everyThisDay);

                if (theDayEveryMonth >= dtFromTime)
                {

                    PlotDateInfo(theDayEveryMonth);

                    i++;
                }

                dtCalcRangeTime = dtCalcRangeTime.AddMonths(interval);
            }
        }


        /// <summary>
        /// 매월 반복 - 특정일까지
        /// </summary>
        public void calcMonth1RepetitionByTillDate()
        {
            DateTime dtFromTime = info.StartDate;
            DateTime dtToTime = info.EndDate;
            DateTime dtCalcRangeTime = dtFromTime;
            DateTime theDayEveryMonth;

            int repetitionCount = 1;
            int interval = info.IntervalCnt;
            int everyThisDay = Convert.ToInt32(info.Option1);




            while (dtCalcRangeTime <= dtToTime)
            {
                theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, everyThisDay);

                if (theDayEveryMonth >= dtFromTime && theDayEveryMonth <= dtToTime)
                {

                    PlotDateInfo(theDayEveryMonth);
                    repetitionCount += 1;
                }

                dtCalcRangeTime = dtCalcRangeTime.AddMonths(interval);
            }
        }


        #endregion

        #region 매월 반복1-"X월마다 X번째 X날에" (특정일까지, 특정횟수만큼) 계산
        /// <summary>
        /// 매월 반복 - 특정일까지
        /// </summary>
        public void calcMonth2RepetitionByTillDate()
        {
            DateTime dtFromTime = info.StartDate;
            DateTime dtToTime = info.EndDate;
            DateTime dtCalcRangeTime = new DateTime(dtFromTime.Year, dtFromTime.Month, 1);
            DateTime theDayEveryMonth;
            DayOfWeek SelectedDayOfWeek = DayOfWeek.Sunday;

            int repetitionCount = 1;
            int interval = info.IntervalCnt;



            //while (dtCalcRangeTime <= dtToTime && repetitionCount <= MaxCountNum)
            while (dtCalcRangeTime <= dtToTime)
            {
                if (int.Parse(info.Option1) >= 0 && int.Parse(info.Option1) <= 3) //평일
                {
                    // 첫번째~네번째
                    switch (int.Parse(info.Option2))
                    {
                        case 0:	// "날"
                            theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, int.Parse(info.Option1));
                            break;
                        case 1:	// "평일"
                            theDayEveryMonth = getOrdinalWorkDay(int.Parse(info.Option1), dtCalcRangeTime);
                            break;
                        case 2:	// "주말"
                            theDayEveryMonth = getOrdinalWeekEnd(int.Parse(info.Option1), dtCalcRangeTime);
                            break;
                        default:
                            switch (int.Parse(info.Option2))
                            {
                                case 3: SelectedDayOfWeek = DayOfWeek.Sunday; break;
                                case 4: SelectedDayOfWeek = DayOfWeek.Monday; break;
                                case 5: SelectedDayOfWeek = DayOfWeek.Tuesday; break;
                                case 6: SelectedDayOfWeek = DayOfWeek.Wednesday; break;
                                case 7: SelectedDayOfWeek = DayOfWeek.Thursday; break;
                                case 8: SelectedDayOfWeek = DayOfWeek.Friday; break;
                                case 9: SelectedDayOfWeek = DayOfWeek.Saturday; break;
                            };
                            theDayEveryMonth = getOrdinalDayOfWeekDay(int.Parse(info.Option1), SelectedDayOfWeek, dtCalcRangeTime);
                            break;

                    }

                    if (theDayEveryMonth >= dtFromTime && theDayEveryMonth <= dtToTime)
                    {
                        PlotDateInfo(theDayEveryMonth);
                        //ddlResult.Items.Add(new ListItem(Convert.ToString(repetitionCount) + " . " + theDayEveryMonth.ToShortDateString() + " (" + WeekDayName[Convert.ToInt32(theDayEveryMonth.DayOfWeek)] + ")", theDayEveryMonth.ToShortDateString()));
                        repetitionCount += 1;
                    }
                }
                else
                {
                    // 마지막째
                    switch (int.Parse(info.Option2))
                    {
                        case 0:	// "날"
                            theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, System.DateTime.DaysInMonth(dtCalcRangeTime.Year, dtCalcRangeTime.Month));
                            break;
                        case 1:
                            theDayEveryMonth = getLastWorkDay(dtCalcRangeTime);
                            break;
                        case 2:
                            theDayEveryMonth = getLastWeekEnd(dtCalcRangeTime);
                            break;
                        default:
                            switch (int.Parse(info.Option2))
                            {
                                case 3: SelectedDayOfWeek = DayOfWeek.Sunday; break;
                                case 4: SelectedDayOfWeek = DayOfWeek.Monday; break;
                                case 5: SelectedDayOfWeek = DayOfWeek.Tuesday; break;
                                case 6: SelectedDayOfWeek = DayOfWeek.Wednesday; break;
                                case 7: SelectedDayOfWeek = DayOfWeek.Thursday; break;
                                case 8: SelectedDayOfWeek = DayOfWeek.Friday; break;
                                case 9: SelectedDayOfWeek = DayOfWeek.Saturday; break;
                            };
                            theDayEveryMonth = getLastDayOfWeekDay(SelectedDayOfWeek, dtCalcRangeTime);
                            break;
                    }

                    if (theDayEveryMonth >= dtFromTime && theDayEveryMonth <= dtToTime)
                    {
                        PlotDateInfo(theDayEveryMonth);
                        repetitionCount += 1;
                    }
                }

                dtCalcRangeTime = dtCalcRangeTime.AddMonths(interval);
            }
        }


        /// <summary>
        /// 매월 반복 - 특정횟수만큼
        /// </summary>
        public void calcMonth2RepetitionByCount()
        {
            DateTime dtFromTime = info.StartDate;
            DateTime dtToTime = info.EndDate;
            DateTime dtCalcRangeTime = new DateTime(dtFromTime.Year, dtFromTime.Month, 1);
            DateTime theDayEveryMonth;
            DayOfWeek SelectedDayOfWeek = DayOfWeek.Sunday;

            int repetitionCount = info.MaxReptCnt;
            int interval = info.IntervalCnt;



            for (int i = 0; repetitionCount > i; )
            {
                if (int.Parse(info.Option1) >= 0 && int.Parse(info.Option1) <= 3)
                {
                    // 첫번째~네번째
                    switch (int.Parse(info.Option2))
                    {
                        case 0:	// "날"
                            theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, int.Parse(info.Option1));
                            break;
                        case 1:	// "평일"
                            theDayEveryMonth = getOrdinalWorkDay(int.Parse(info.Option1), dtCalcRangeTime);
                            break;
                        case 2:
                            theDayEveryMonth = getOrdinalWeekEnd(int.Parse(info.Option1), dtCalcRangeTime);
                            break;
                        default:
                            switch (int.Parse(info.Option2))
                            {
                                case 3: SelectedDayOfWeek = DayOfWeek.Sunday; break;
                                case 4: SelectedDayOfWeek = DayOfWeek.Monday; break;
                                case 5: SelectedDayOfWeek = DayOfWeek.Tuesday; break;
                                case 6: SelectedDayOfWeek = DayOfWeek.Wednesday; break;
                                case 7: SelectedDayOfWeek = DayOfWeek.Thursday; break;
                                case 8: SelectedDayOfWeek = DayOfWeek.Friday; break;
                                case 9: SelectedDayOfWeek = DayOfWeek.Saturday; break;
                            };
                            theDayEveryMonth = getOrdinalDayOfWeekDay(int.Parse(info.Option1), SelectedDayOfWeek, dtCalcRangeTime);
                            break;

                    }

                    if (theDayEveryMonth >= dtFromTime)
                    {
                        PlotDateInfo(theDayEveryMonth);

                        i++;
                    }
                }
                else
                {
                    // 마지막째
                    switch (int.Parse(info.Option2))
                    {
                        case 0:	// "날"
                            theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, System.DateTime.DaysInMonth(dtCalcRangeTime.Year, dtCalcRangeTime.Month));
                            break;
                        case 1:
                            theDayEveryMonth = getLastWorkDay(dtCalcRangeTime);
                            break;
                        case 2:
                            theDayEveryMonth = getLastWeekEnd(dtCalcRangeTime);
                            break;
                        default:
                            switch (int.Parse(info.Option2))
                            {
                                case 3: SelectedDayOfWeek = DayOfWeek.Sunday; break;
                                case 4: SelectedDayOfWeek = DayOfWeek.Monday; break;
                                case 5: SelectedDayOfWeek = DayOfWeek.Tuesday; break;
                                case 6: SelectedDayOfWeek = DayOfWeek.Wednesday; break;
                                case 7: SelectedDayOfWeek = DayOfWeek.Thursday; break;
                                case 8: SelectedDayOfWeek = DayOfWeek.Friday; break;
                                case 9: SelectedDayOfWeek = DayOfWeek.Saturday; break;
                            };
                            theDayEveryMonth = getLastDayOfWeekDay(SelectedDayOfWeek, dtCalcRangeTime);
                            break;
                    }

                    if (theDayEveryMonth >= dtFromTime)
                    {
                        PlotDateInfo(theDayEveryMonth);
                        i++;
                    }
                }

                dtCalcRangeTime = dtCalcRangeTime.AddMonths(interval);
            }
        }


        /// <summary>
        /// 첫번째~네번째 평일 반환
        /// </summary>
        /// <param name="_ordinal"></param>
        /// <param name="_dt"></param>
        /// <returns>DateTime</returns>
        private DateTime getOrdinalWorkDay(int _ordinal, DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);
            int ordinalCount = 1;

            for (DateTime i = firstDateThisMonth; lastDateThisMonth >= i; i = i.AddDays(1))
            {
                if (Convert.ToInt32(i.DayOfWeek) != 0 && Convert.ToInt32(i.DayOfWeek) != 6)
                {
                    if (_ordinal == ordinalCount)
                    {
                        returnDate = i;
                        break;
                    }
                    ordinalCount++;
                }
            }

            return returnDate;
        }


        /// <summary>
        /// 마지막째 평일 반환
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns>DateTime</returns>
        private DateTime getLastWorkDay(DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);

            for (DateTime i = lastDateThisMonth; firstDateThisMonth <= i; i = i.AddDays(-1))
            {
                if (Convert.ToInt32(i.DayOfWeek) != 0 && Convert.ToInt32(i.DayOfWeek) != 6)
                {
                    returnDate = i;
                    break;
                }
            }

            return returnDate;
        }


        /// <summary>
        /// 첫번째~네번째 주말 반환
        /// 일요일부터 시작되는 달은 첫번재 일요일이 첫번째 주말이고 그 외의 경우는 토요일이 주말
        /// </summary>
        /// <param name="_ordinal"></param>
        /// <param name="_dt"></param>
        /// <returns></returns>
        private DateTime getOrdinalWeekEnd(int _ordinal, DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);
            int ordinalCount = 1;

            for (DateTime i = firstDateThisMonth; lastDateThisMonth >= i; i = i.AddDays(1))
            {
                if (Convert.ToInt32(i.DayOfWeek) == 0 || Convert.ToInt32(i.DayOfWeek) == 6)
                {
                    if (_ordinal == ordinalCount)
                    {
                        returnDate = i;
                        break;
                    }
                    ordinalCount++;
                }
            }

            return returnDate;
        }


        /// <summary>
        /// 마지막 주말 반환
        /// 무조건 뒤에서 첫번재 토요일
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        private DateTime getLastWeekEnd(DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);

            for (DateTime i = lastDateThisMonth; firstDateThisMonth <= i; i = i.AddDays(-1))
            {
                if (Convert.ToInt32(i.DayOfWeek) == 6)
                {
                    returnDate = i;
                    break;
                }
            }

            return returnDate;
        }


        /// <summary>
        /// 첫번째~네번째 X요일 반환
        /// </summary>
        /// <param name="_ordinal"></param>
        /// <param name="_dayOfWeek"></param>
        /// <param name="_dt"></param>
        /// <returns>DateTime</returns>
        private DateTime getOrdinalDayOfWeekDay(int _ordinal, DayOfWeek _dayOfWeek, DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);
            int ordinalCount = 1;

            for (DateTime i = firstDateThisMonth; lastDateThisMonth >= i; i = i.AddDays(1))
            {
                if (i.DayOfWeek == _dayOfWeek)
                {
                    if (_ordinal == ordinalCount)
                    {
                        returnDate = i;
                        break;
                    }
                    ordinalCount++;
                }
            }

            return returnDate;
        }


        /// <summary>
        /// 마지막째 X요일 반환
        /// </summary>
        /// <param name="_dayOfWeek"></param>
        /// <param name="_dt"></param>
        /// <returns>DateTime</returns>
        private DateTime getLastDayOfWeekDay(DayOfWeek _dayOfWeek, DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);

            for (DateTime i = lastDateThisMonth; firstDateThisMonth <= i; i = i.AddDays(-1))
            {
                if (i.DayOfWeek == _dayOfWeek)
                {
                    returnDate = i;
                    break;
                }
            }

            return returnDate;
        }

        #endregion


    }
}
