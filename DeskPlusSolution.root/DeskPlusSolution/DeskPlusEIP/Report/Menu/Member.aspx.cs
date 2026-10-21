using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Report.Web.Menu
{
    public partial class Member : Base.BasePageReport
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Menu.Member).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            #region 이미지 처리

            this.CalendarSingle.ImagesBaseUrl = this.BaseImgPath + "/pims/";

            #endregion 이미지 처리

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     :
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadCalHoliday();
            //CalendarSingle.
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     :
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnCancel.Text = GetResource("Cancel");
            }

            switch (PreLang)
            {
                case "ko":  //1042
                    this.CalendarSingle.CultureId = 1042;
                    break;

                case "en": //1033
                    this.CalendarSingle.CultureId = 1033;
                    break;

                case "en-gb": //2057
                    this.CalendarSingle.CultureId = 2057;
                    break;

                case "ja": //1041
                    this.CalendarSingle.CultureId = 1041;
                    break;

                case "fr"://1036
                    this.CalendarSingle.CultureId = 1036;
                    break;

                case "zh-cn": //2052
                    this.CalendarSingle.CultureId = 2052;
                    break;

                case "zh-tw": //1028
                    this.CalendarSingle.CultureId = 1028;
                    break;
            }
        }

        /// <summary>
        ///  공휴일
        /// </summary>
        private void LoadCalHoliday()
        {
            string solor = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Solar", "") + ","; // 양력
            string[] lunar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Lunar", "").Split(','); // 음력

            for (int i = 0; i < lunar.Length; i++)
            {
                string[] arrLunar = lunar[i].Split('-');
                DateTime lunDate = DateHelpers.LunarToSolar(Years, arrLunar[0].ToNumber<int>(), arrLunar[1].ToNumber<int>(), false);

                if ((lunDate.Month == Months) || (lunDate.Month == (Months - 1)) || (lunDate.Month == (Months + 1)))
                {
                    solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + lunDate.Day.ToString().PadLeft(2, '0') + ",";

                    //1월,8월 경우(설날,석가탄신일)은 해당 일자 기준 -1,+1 하루씩 더 쉰다.
                    if ((arrLunar[0].ToNumber<int>() == 1) || (arrLunar[0].ToNumber<int>() == 8))
                    {
                        if (lunDate.Day == 1)
                        {
                            solor += (lunDate.Month - 1).ToString().PadLeft(2, '0') + "-" + DateHelpers.GetDayCountOfMonth(Years, (lunDate.Month - 1)) + ",";
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-02,";
                        }
                        else if (lunDate.Day == DateHelpers.GetDayCountOfMonth(Years, (lunDate.Month)))
                        {
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + (lunDate.Day - 1).ToString() + ",";
                            solor += (lunDate.Month + 1).ToString().PadLeft(2, '0') + "-01,";
                        }
                        else
                        {
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + (lunDate.Day - 1).ToString().PadLeft(2, '0') + ",";
                            solor += lunDate.Month.ToString().PadLeft(2, '0') + "-" + (lunDate.Day + 1).ToString().PadLeft(2, '0') + ",";
                        }
                    }
                }
            }

            solor = solor.TrimEnd(',');

            string holCss = "Holiday";

            //양력 공휴일
            foreach (string hol in solor.Split(','))
            {
                if (hol.Length > 0)
                {
                    DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + hol);

                    ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, holCss, null, null, null);
                    if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
                }
            }

            ////양력 공휴일
            //foreach (string _s in arSolar)
            //{
            //    if (_s.Length > 0)
            //    {
            //        DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + _s);
            //        ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            //        if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);

            //        sDate = sDate.AddYears(1);
            //        addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            //        if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
            //        sDate = sDate.AddYears(-2);
            //        addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            //        if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);

            //    }
            //}

            //BLL.CalendarSolToLun bllCon = new BLL.CalendarSolToLun();

            ////음력공휴일을 양력으로 치환
            //foreach (string _s in arLunar)
            //{
            //    if (_s.Length > 0)
            //    {
            //        DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + _s);
            //        sDate = DateTime.Parse(bllCon.FromLunToSol(sDate));

            //        ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            //        if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);

            //        sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + _s).AddYears(1);
            //        sDate = DateTime.Parse(bllCon.FromLunToSol(sDate));
            //        addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            //        if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);

            //        sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + _s).AddYears(-1);
            //        sDate = DateTime.Parse(bllCon.FromLunToSol(sDate));
            //        addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            //        if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);

            //    }
            //}
        }

        /// <summary>
        /// 달력 월 변경하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CalendarSingle_VisibleDateChanged(object sender, EventArgs e)
        {
            DateTime selCalDate = CalendarSingle.VisibleDate;
            Years = selCalDate.Year;
            Months = selCalDate.Month;
            Days = System.DateTime.Now.Day;

            LoadCalHoliday();
        }
    }
}