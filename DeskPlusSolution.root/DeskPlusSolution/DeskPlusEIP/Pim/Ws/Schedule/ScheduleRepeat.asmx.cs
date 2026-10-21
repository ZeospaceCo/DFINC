using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ScheduleRepeat : Base.BaseWSPim
    {
        #region Get
        public ScheduleRepeat()
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Schedule.Schedule).Assembly;	//어셈블리 선언
            #endregion

            this.GetResourceManager();
        }
        #endregion

        #region Set
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string RepeatCalcu(string p)
        {
            List<object> listobj = new List<object>();

            string strRt = string.Empty;
            SetDecryptString(p);

            BE.RepeatScheduleInfo info = new RepeatScheduleInfo();

            info.CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            info.RepeatType = GetDecryptParam("RepeatType", "");
            info.SelectType = GetDecryptParam("SelectType", "");
            info.IntervalCnt = GetDecryptParam("IntervalCnt", "0").ToNumber<int>();

            info.Option1 = GetDecryptParam("Option1", "0");
            info.Option2 = GetDecryptParam("Option2", "0");
            info.Option3 = GetDecryptParam("Option3", "0");
            info.Option4 = GetDecryptParam("Option4", "0");
            info.Option5 = GetDecryptParam("Option5", "0");
            info.Option6 = GetDecryptParam("Option6", "0");
            info.Option7 = GetDecryptParam("Option7", "0");

            info.MaxReptCnt = GetDecryptParam("MaxReptCnt", "0").ToNumber<int>();

            if (GetDecryptParam("EndDate", "") != "")
            {
                info.EndDate = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.ToString("yyyy-MM-dd")));
                info.EndDate = DateTime.Parse(info.EndDate.ToString("yyyy-MM-dd 23:59"));
            }

            info.StartDate = DateTime.Now;//DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));

            BLL.RepeatScheduleProc biz = new BLL.RepeatScheduleProc(info);

            //biz.
            switch (info.RepeatType)
            {
                case "D":
                    strRt = MakeDay(info);
                    break;
                case "W":
                    strRt = MakeWeek(info);
                    break;
                case "M":
                    strRt = MakeMonth(info);
                    break;
            }

            return strRt;
        }

        #region Text파싱
        /// <summary>
        /// 주간
        /// </summary>
        /// <returns></returns>
        public string MakeWeek(RepeatScheduleInfo info)
        {
            string strRt = string.Empty;
            if (info.IntervalCnt == 1)
                strRt = GetResource("EveryWeek"); //"매주마다";
            else
                strRt = info.IntervalCnt + GetResource("Weeks"); //"주마다";

            if (info.Option1.Equals("1"))
                strRt += " " + GetResource("kind4"); //" 일";
            if (info.Option2.Equals("1"))
                strRt += " " + GetResource("kind5"); //" 월";
            if (info.Option3.Equals("1"))
                strRt += " " + GetResource("kind6"); //" 화";
            if (info.Option4.Equals("1"))
                strRt += " " + GetResource("kind7"); //" 수";
            if (info.Option5.Equals("1"))
                strRt += " " + GetResource("kind8"); //" 목";
            if (info.Option6.Equals("1"))
                strRt += " " + GetResource("kind9"); //" 금";
            if (info.Option7.Equals("1"))
                strRt += " " + GetResource("kind10"); //" 토";


            strRt += " " + GetResource("Days"); //" 요일에";

            if (info.EndDate.ToString("yyyyMMdd") != "00010101")
                strRt += " " + info.EndDate.ToString("yyyy-MM-dd") + GetResource("ToRepeat"); //"까지 반복합니다.";

            if (info.MaxReptCnt != 0)
                strRt += " " + info.MaxReptCnt.ToString() + GetResource("TimeRepeat"); //"회 반복합니다.";

            return strRt;
        }

        public string MakeDay(RepeatScheduleInfo info)
        {
            string strRt = string.Empty;
            if (info.IntervalCnt == 1)
            {
                strRt = GetResource("EveryDayR") + "  "; //"매일마다 ";
            }
            else
            {
                strRt = info.IntervalCnt + GetResource("DayToo"); //"일마다";
            }

            if (info.EndDate.ToString("yyyyMMdd") != "00010101")
                strRt += " " + info.EndDate.ToString("yyyy-MM-dd") + GetResource("ToRepeat"); //"까지 반복합니다.";


            if (info.MaxReptCnt != 0)
                strRt += " " + info.MaxReptCnt.ToString() + GetResource("TimeRepeat"); //"회 반복합니다.";



            return strRt;
        }

        public string MakeMonth(RepeatScheduleInfo info)
        {
            string strRt = string.Empty;
            if (info.IntervalCnt == 1)
            {
                strRt = GetResource("EveryMonth") + "  "; //"매월 ";
            }
            else
            {
                strRt = info.IntervalCnt + GetResource("MonthToo"); //"개월마다";
            }



            //strRt += " " + this.Option1 + "  ";
            //if(Option2
            if (info.SelectType == "1")
            {

                strRt += " " + info.Option1 + GetResource("TheDay"); //"일에 ";
            }
            else if (info.SelectType == "2")
            {

                strRt += " " + info.Option1 + GetResource("XOrder"); //"번째   ";

                //평일,주말,일,월,화,수,목,금
                switch (info.Option2)
                {
                    case "0": //날
                        strRt += GetResource("XDay"); //"날에";
                        break;
                    case "1": //평일
                        strRt += GetResource("XNormalDay"); //"평일에";
                        break;
                    case "2": //주말
                        strRt += GetResource("XWeekend"); //"주말에";
                        break;
                    case "3": //일
                        strRt += GetResource("XSun"); //"일요일에";
                        break;
                    case "4": //월
                        strRt += GetResource("XMon"); //"월요일에";
                        break;
                    case "5": //화
                        strRt += GetResource("XTue"); //"화요일에";
                        break;
                    case "6"://수
                        strRt += GetResource("XWed"); //"수요일에";
                        break;
                    case "7"://목
                        strRt += GetResource("XThu"); //"목요일에";
                        break;
                    case "8"://금
                        strRt += GetResource("XFri"); //"금요일에";
                        break;
                    case "9"://토
                        strRt += GetResource("XSat"); //"토요일에";
                        break;

                }

            }

            if (info.EndDate.ToString("yyyyMMdd") != "00010101")
                strRt += " " + info.EndDate.ToString("yyyy-MM-dd") + GetResource("ToRepeat"); //"까지 반복합니다.";


            if (info.MaxReptCnt != 0)
                strRt += " " + info.MaxReptCnt.ToString() + GetResource("TimeRepeat"); //"회 반복합니다.";

            return strRt;
        }
        #endregion

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.DisplayInfo> RepeatCalcuList(string p)
        {
            List<BE.DisplayInfo> listobj = new List<DisplayInfo>();

            SetDecryptString(p);

            BE.RepeatScheduleInfo info = new RepeatScheduleInfo();

            info.CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            info.RepeatType = GetDecryptParam("RepeatType", "");
            info.SelectType = GetDecryptParam("SelectType", "");
            info.IntervalCnt = GetDecryptParam("IntervalCnt", "0").ToNumber<int>();

            info.Option1 = GetDecryptParam("Option1", "0");
            info.Option2 = GetDecryptParam("Option2", "0");
            info.Option3 = GetDecryptParam("Option3", "0");
            info.Option4 = GetDecryptParam("Option4", "0");
            info.Option5 = GetDecryptParam("Option5", "0");
            info.Option6 = GetDecryptParam("Option6", "0");
            info.Option7 = GetDecryptParam("Option7", "0");



            info.MaxReptCnt = GetDecryptParam("MaxReptCnt", "0").ToNumber<int>();

            if (GetDecryptParam("EndDate", "") != "")
            {
                info.EndDate = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.ToString("yyyy-MM-dd")));
            }

            info.StartDate = DateTime.Now;//DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));

            BLL.RepeatScheduleProc biz = new BLL.RepeatScheduleProc(info);



            switch (info.RepeatType)
            {
                case "D":
                    biz.RepeatDate();
                    break;
                case "W":
                    biz.CalcWeekDayCount();
                    break;
                case "M":

                    // this.Option2 = 0~5   날, 평일,주말
                    //this.Option1 =1~5   첫째 둘째 
                    if (info.MaxReptCnt > 0)
                    {
                        /// 매달 특정일에 회수 제한 
                        if (info.SelectType == "1")
                        {
                            biz.calcMonth1RepetitionByCount();
                        }
                        else
                        {
                            //매월 반복 
                            biz.calcMonth2RepetitionByCount();
                        }
                    }
                    else
                    {
                        if (info.SelectType == "1")
                        {

                            /// 매월 반복 - 특정일까지
                            biz.calcMonth1RepetitionByTillDate();
                        }
                        else
                        {
                            //매월 반복 - 특정일까지
                            biz.calcMonth2RepetitionByTillDate();
                        }
                    }
                    break;

            }


            listobj = biz.GetDateInfo();

            return listobj;
        }


        #endregion
    }



}
