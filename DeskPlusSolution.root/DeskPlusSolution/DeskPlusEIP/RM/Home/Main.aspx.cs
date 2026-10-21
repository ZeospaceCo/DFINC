using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Home
{
    public partial class Main : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수설정
        public string TmpStartDate = String.Empty;
        public string TmpEndDate = String.Empty;
        public int DeptPageNo = 0;
        public int MainPageNo = 0;
        public string CalendarDate = String.Empty;
        public string ListGubun = string.Empty;
        public string CntYN = "N";
        public int RmCnt = 0;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Home.Main).Assembly;	//어셈블리 선언
            #endregion

            #region 이미지 처리
            this.CalendarSingle.ImagesBaseUrl = this.BaseImgPath + "/pims/";
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-16
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetStartEndTime();
            LoadUseRM();
            LoadList();
            //SolarCustomDays();
            //LunarCustomDays();
            CustomDays();

            // 기본리스트
            Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = envdal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (list != null)
            {
                foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo info in list)
                {
                    ListGubun = (info.RMHomeMenu.Equals("")) ? "LISTMY" : info.RMHomeMenu;
                }
            }

            //자원 주 담당자의 대기 예약 카운트 가져오기
            if (CntYN == "Y")
            {
                DAL.List dal = new DAL.List();
                int tmpCnt = dal.GetListUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                if (tmpCnt > 0)
                {
                    DAL.Reverse redal = new DAL.Reverse();
                    RmCnt = redal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "N", "0", "", "", "", "");
                }
            }
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
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
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-16
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            MainPageNo = GetDecryptParam("mainPageNo", "0").ToNumber<int>();
            //ListGubun = GetDecryptParam("listGubun", "");
            DeptPageNo = GetDecryptParam("deptPageNo", "0").ToNumber<int>();
            CntYN = GetDecryptParam("cntYN", "N");
        }

        /// <summary>
        /// 지난 3개월 예약순위 자원
        /// </summary>
        private void LoadUseRM()
        {
            string tmpWeContent1 = String.Empty;
            string tmpWeContent2 = String.Empty;
            string tmpWeContent3 = String.Empty;
            string tmpWeContent4 = String.Empty;

            DAL.Reverse dal = new DAL.Reverse();
            var dtr = dal.GetWeUseList(CurrentUserInfo.CompanyID);

            int tmpCnt = 0;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        tmpCnt += 1;
                        string tmpTitle = dtr["Title"].ToString().ToReverseXSS();
                        if (tmpTitle.Length > 8)
                        {
                            tmpTitle = tmpTitle.Substring(0, 8) + "..";
                        }

                        string[] arrManyName = dtr["MManyName"].ToString().Split('|');
                        string tmpManagerName = "";
                        if (arrManyName.Length == 7)
                        {
                            tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                    arrManyName[2], arrManyName[3], arrManyName[4],
                                                                    arrManyName[5], arrManyName[6], PreLang);
                        }

                        if (tmpManagerName.Length > 8)
                        {
                            tmpManagerName = tmpManagerName.Substring(0, 8) + "..";
                        }

                        string IconName = "";

                        if (dtr["IconName"].ToString() == "1")
                        {
                            IconName = "<i class='fa fa-building text-secondary'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "2")
                        {
                            IconName = "<i class='fa fa-address-book text-secondary'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "3")
                        {
                            IconName = "<i class='fa fa-calendar text-secondary'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "4")
                        {
                            IconName = "<i class='fa fa-car text-secondary'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "5")
                        {
                            IconName = "<i class='fa fa-comments text-secondary'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "6")
                        {
                            IconName = "<i class='fa fa-fax text-secondary'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "7")
                        {
                            IconName = "<i class='fa fa-desktop text-secondary'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "8")
                        {
                            IconName = "<i class='fa fa-microphone text-secondary'></i>";
                        }
                        else if (dtr["IconName"].ToString() == "9")
                        {
                            IconName = "<i class='fa fa-print text-secondary'></i>";
                        }

                        if (tmpCnt == 1)
                        {
                            tmpWeContent1 += "<li><a onclick=\"javascript:Main.Add('" + dtr["ListID"].ToString() + "')\" style=\"cursor:pointer\">";
                            tmpWeContent1 += IconName;
                            tmpWeContent1 += "<span>" + tmpTitle + "</span><span>" + this.resMng.GetString("Manager") + ":" + tmpManagerName + "</span>";
                            tmpWeContent1 += "</span><input type=\"button\" class=\"btn btn-primary\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";

                            /*
                            tmpWeContent1 += "<li><a onclick=\"javascript:Main.Add('" + dtr["ListID"].ToString() + "')\" style=\"cursor:pointer\"><span class=\"thumb\">";
                            tmpWeContent1 += "<img src=\"" + BaseImgPath + "/RM/rs_icon" + dtr["IconName"].ToString() + ".png\">";
                            tmpWeContent1 += "</span> <span>" + tmpTitle + "</span><br><span>" + this.resMng.GetString("Manager") + ":";
                            tmpWeContent1 += tmpManagerName + "</span><input type=\"button\" class=\"re_sbs\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";
                            */
                        }

                        if (tmpCnt == 2)
                        {
                            tmpWeContent2 += "<li><a onclick=\"javascript:Main.Add('" + dtr["ListID"].ToString() + "')\" style=\"cursor:pointer\">";
                            tmpWeContent2 += IconName;
                            tmpWeContent2 += "<span>" + tmpTitle + "</span><span>" + this.resMng.GetString("Manager") + ":" + tmpManagerName + "</span>";
                            tmpWeContent2 += "</span><input type=\"button\" class=\"btn btn-primary\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";
                        }

                        if (tmpCnt == 3)
                        {
                            tmpWeContent3 += "<li><a onclick=\"javascript:Main.Add('" + dtr["ListID"].ToString() + "')\" style=\"cursor:pointer\">";
                            tmpWeContent3 += IconName;
                            tmpWeContent3 += "<span>" + tmpTitle + "</span><span>" + this.resMng.GetString("Manager") + ":" + tmpManagerName + "</span>";
                            tmpWeContent3 += "</span><input type=\"button\" class=\"btn btn-primary\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";
                        }

                        if (tmpCnt == 4)
                        {
                            tmpWeContent4 += "<li><a onclick=\"javascript:Main.Add('" + dtr["ListID"].ToString() + "')\" style=\"cursor:pointer\">";
                            tmpWeContent4 += IconName;
                            tmpWeContent4 += "<span>" + tmpTitle + "</span><span>" + this.resMng.GetString("Manager") + ":" + tmpManagerName + "</span>";
                            tmpWeContent4 += "</span><input type=\"button\" class=\"btn btn-primary\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";
                        }
                    }
                }
            }

            if (tmpCnt == 1)
            {
                tmpWeContent2 = tmpWeContent1;
                tmpWeContent3 = tmpWeContent1;
                tmpWeContent4 = tmpWeContent1;
            }
            else if (tmpCnt == 2)
            {
                tmpWeContent3 = tmpWeContent1;
                tmpWeContent4 = tmpWeContent2;
            }
            else if (tmpCnt == 3)
            {
                tmpWeContent4 = tmpWeContent2;
            }

            if (tmpCnt == 0)
            {
                DAL.List dalList = new DAL.List();
                var dtrList = dalList.GetTopList(CurrentUserInfo.CompanyID);

                if (dtrList != null)
                {
                    if (dtrList.HasRows)
                    {
                        while (dtrList.Read())
                        {
                            tmpCnt += 1;
                            string tmpTitle = dtrList["Title"].ToString().ToReverseXSS();
                            if (tmpTitle.Length > 8)
                            {
                                tmpTitle = tmpTitle.Substring(0, 8) + "..";
                            }

                            string[] arrManyName = dtrList["MManyName"].ToString().Split('|');
                            string tmpManagerName = "";
                            if (arrManyName.Length == 7)
                            {
                                tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                        arrManyName[2], arrManyName[3], arrManyName[4],
                                                                        arrManyName[5], arrManyName[6], PreLang);
                            }

                            if (tmpManagerName.Length > 8)
                            {
                                tmpManagerName = tmpManagerName.Substring(0, 8) + "..";
                            }

                            string IconNameE = "";

                            if (dtrList["IconName"].ToString() == "1")
                            {
                                IconNameE = "<i class='fa fa-building text-secondary'></i>";
                            }
                            else if (dtrList["IconName"].ToString() == "2")
                            {
                                IconNameE = "<i class='fa fa-address-book text-secondary'></i>";
                            }
                            else if (dtrList["IconName"].ToString() == "3")
                            {
                                IconNameE = "<i class='fa fa-calendar text-secondary'></i>";
                            }
                            else if (dtrList["IconName"].ToString() == "4")
                            {
                                IconNameE = "<i class='fa fa-car text-secondary'></i>";
                            }
                            else if (dtrList["IconName"].ToString() == "5")
                            {
                                IconNameE = "<i class='fa fa-comments text-secondary'></i>";
                            }
                            else if (dtrList["IconName"].ToString() == "6")
                            {
                                IconNameE = "<i class='fa fa-fax text-secondary'></i>";
                            }
                            else if (dtrList["IconName"].ToString() == "7")
                            {
                                IconNameE = "<i class='fa fa-desktop text-secondary'></i>";
                            }
                            else if (dtrList["IconName"].ToString() == "8")
                            {
                                IconNameE = "<i class='fa fa-microphone text-secondary'></i>";
                            }
                            else if (dtrList["IconName"].ToString() == "9")
                            {
                                IconNameE = "<i class='fa fa-print text-secondary'></i>";
                            }

                            if (tmpCnt == 1)
                            {
                                tmpWeContent1 += "<li><a onclick=\"javascript:Main.Add('" + dtrList["ListID"].ToString() + "')\" style=\"cursor:pointer\">";
                                tmpWeContent1 += IconNameE;
                                tmpWeContent1 += "<span>" + tmpTitle + "</span><span>" + this.resMng.GetString("Manager") + ":" + tmpManagerName + "</span>";
                                tmpWeContent1 += "</span><input type=\"button\" class=\"btn btn-primary\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";

                                /*
                                tmpWeContent1 += "<li><a onclick=\"javascript:Main.Add('" + dtrList["ListID"].ToString() + "')\" style=\"cursor:pointer\"><span class=\"thumb\">";
                                tmpWeContent1 += "<img src=\"" + BaseImgPath + "/RM/rs_icon" + dtrList["IconName"].ToString() + ".png\">";
                                tmpWeContent1 += "</span> <span>" + tmpTitle + "</span><br><span>" + this.resMng.GetString("Manager") + ":";
                                tmpWeContent1 += tmpManagerName + "</span><input type=\"button\" class=\"re_sbs\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";
                                */
                            }

                            if (tmpCnt == 2)
                            {
                                tmpWeContent2 += "<li><a onclick=\"javascript:Main.Add('" + dtrList["ListID"].ToString() + "')\" style=\"cursor:pointer\">";
                                tmpWeContent2 += IconNameE;
                                tmpWeContent2 += "<span>" + tmpTitle + "</span><span>" + this.resMng.GetString("Manager") + ":" + tmpManagerName + "</span>";
                                tmpWeContent2 += "</span><input type=\"button\" class=\"btn btn-primary\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";
                            }

                            if (tmpCnt == 3)
                            {
                                tmpWeContent3 += "<li><a onclick=\"javascript:Main.Add('" + dtrList["ListID"].ToString() + "')\" style=\"cursor:pointer\">";
                                tmpWeContent3 += IconNameE;
                                tmpWeContent3 += "<span>" + tmpTitle + "</span><span>" + this.resMng.GetString("Manager") + ":" + tmpManagerName + "</span>";
                                tmpWeContent3 += "</span><input type=\"button\" class=\"btn btn-primary\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";
                            }

                            if (tmpCnt == 4)
                            {
                                tmpWeContent4 += "<li><a onclick=\"javascript:Main.Add('" + dtrList["ListID"].ToString() + "')\" style=\"cursor:pointer\">";
                                tmpWeContent4 += IconNameE;
                                tmpWeContent4 += "<span>" + tmpTitle + "</span><span>" + this.resMng.GetString("Manager") + ":" + tmpManagerName + "</span>";
                                tmpWeContent4 += "</span><input type=\"button\" class=\"btn btn-primary\" value=\"" + this.resMng.GetString("IconReserve") + "\"/></a></li>";
                            }
                        }
                    }
                }

                if (tmpCnt == 1)
                {
                    tmpWeContent2 = tmpWeContent1;
                    tmpWeContent3 = tmpWeContent1;
                    tmpWeContent4 = tmpWeContent1;
                }
                else if (tmpCnt == 2)
                {
                    tmpWeContent3 = tmpWeContent1;
                    tmpWeContent4 = tmpWeContent2;
                }
                else if (tmpCnt == 3)
                {
                    tmpWeContent4 = tmpWeContent2;
                }
            }

            WeContent = tmpWeContent1 + tmpWeContent2 + tmpWeContent3 + tmpWeContent4;
        }

        /// <summary>
        /// 예약 자원 선택 
        /// </summary>
        private void LoadList()
        {
            ddlList.Items.Clear();

            DAL.List dal = new DAL.List();
            var dtr = dal.GetHomeList(CurrentUserInfo.CompanyID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string CategoryName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                        if (CategoryName.Length > 9)
                        {
                            CategoryName = CategoryName.Substring(0, 9) + "..";
                        }

                        string tmpText = "[" + CategoryName + "]" + dtr["Title"].ToString().ToReverseXSS();
                        if (tmpText.Length > 18)
                        {
                            tmpText = tmpText.Substring(0, 18) + "..";
                        }

                        ddlList.Items.Add(new ListItem(tmpText, dtr["ListID"].ToString() + "_" + dtr["UsableQty"].ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-18
        /// 작성목적 : 일시 시간
        /// </summary>
        private void SetStartEndTime()
        {
            DateTime dtNow = System.DateTime.Now;
            CalendarDate = dtNow.Year.ToString() + "-" + dtNow.Month.ToString().PadLeft(2, '0') + "-" + dtNow.Day.ToString().PadLeft(2, '0');

            ddlstartTime.Items.Clear();
            ddlendTime.Items.Clear();

            DateTime dtTargetDate = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
            DateTime dtFromTime = dtTargetDate;
            DateTime dtToTime = dtTargetDate.AddDays(1);

            while (dtFromTime < dtToTime)
            {
                //직접 바인딩
                ListItem tmpItemFrom = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlstartTime.Items.Add(tmpItemFrom);

                ListItem tmpItemTo = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlendTime.Items.Add(tmpItemTo);

                dtFromTime = dtFromTime.AddMinutes(30);
            }

            ddlstartTime.SelectedIndex = 18;
            ddlendTime.SelectedIndex = ddlstartTime.SelectedIndex + 1;
        }


        /// <summary>
        /// 작성자  : 이지원
        /// 작성일자 : 2017-01-24
        /// 작성목적 : 휴일 리스트 가져옴
        /// </summary>
        /// <param name="_day"></param>
        private void CustomDays()
        {
            string CacheName = "HolidayList_";

            HttpContext context = HttpContext.Current;

            List<object> listobj = new List<object>();

            if (context.Cache[CacheName + CurrentUserInfo.CompanyID] == null)
            {
                Pim.BLL.HolidayR holiday = new Pim.BLL.HolidayR();
                holiday.SetCacheHolidayList(CurrentUserInfo.CompanyID, DateTime.Now.ToString("yyyy"));
            }

            listobj = (List<object>)context.Cache[CacheName + CurrentUserInfo.CompanyID];

            string tempDate = string.Empty;

            foreach (Pim.BE.HolidayRInfo _be in listobj)
            {
                string SolarLunar = _be.SolarLunar;                               //양음력구분
                string HOLIDAY = _be.HoliDay;                                     //일자
                string TITLE = _be.Title;                                         //제목
                string HolidayBetween = _be.HolidayBetween;                       //연휴구분
                int HolidayBetweenDay = _be.HolidayBetweenDay;                    //연휴일자
                string IsReplace = _be.IsReplace;                                 //대체휴가구분
                int IsReplaceDay = 0;                                             //대체휴가일자
                string IsHolidayKind = _be.IsHolidayKind;                         //공휴일구분      

                DateTime sDate = DateTime.Parse(HOLIDAY);

                //양력음력구분
                if (SolarLunar != "0")
                {
                    CalendarHelper calHelper = new CalendarHelper();
                    sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                }

                //대체휴일 확인
                if (IsReplace == "1")
                {
                    for (int i = 0; i <= HolidayBetweenDay; i++)
                    {
                        if (HolidayBetween == "1" || HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                if (i > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }

                        if (HolidayBetween == "2" || HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }
                    }
                }

                //대체휴일 + 토 확인
                if (IsReplace == "2")
                {
                    for (int i = 0; i <= HolidayBetweenDay; i++)
                    {
                        if (HolidayBetween == "0")
                        {
                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }
                            else if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }

                        if (HolidayBetween == "3")
                        {
                            if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                            {
                                IsReplaceDay = IsReplaceDay + 2;
                            }

                            if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                            {
                                IsReplaceDay = IsReplaceDay + 1;
                            }
                        }
                    }
                }

                //일자 저장
                SetCustomDays(sDate);

                //비교 데이터 저장
                if (IsHolidayKind == "0")
                {
                    tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                }

                if (HolidayBetween == "1")
                {
                    //이전일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        SetCustomDays(sDate.AddDays(Convert.ToDouble("-" + i)));

                        if (IsHolidayKind == "0")
                        {
                            tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
                else if (HolidayBetween == "2")
                {
                    //다음일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        SetCustomDays(sDate.AddDays(Convert.ToDouble(i)));

                        if (IsHolidayKind == "0")
                        {
                            tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }
                else if (HolidayBetween == "3")
                {
                    //이전다음일 포함
                    for (int i = 1; i <= HolidayBetweenDay; i++)
                    {
                        SetCustomDays(sDate.AddDays(Convert.ToDouble("-" + i)));

                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                        }

                        SetCustomDays(sDate.AddDays(Convert.ToDouble(i)));

                        if (IsHolidayKind == "1")
                        {
                            tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                        }
                    }
                }

                if (IsReplaceDay > 0)
                {
                    int IsReplaceDayStart = HolidayBetweenDay + 1;
                    int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                    for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                    {
                        SetCustomDays(sDate.AddDays(Convert.ToDouble(i)));
                    }
                }
            }
        }

        /// <summary>
        /// 날짜 표시
        /// </summary>
        /// <param name="_day"></param>
        private void SetCustomDays(DateTime _day)
        {
            string cssClass = "Holiday";
            DateTime sDate = _day;
            ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
        }

        /// <summary>
        /// 양력 표시(앞/뒤로 3년식)
        /// </summary>
        /// <param name="_day"></param>
        private void SetSolarCustomDays(DateTime _day)
        {
            string cssClass = "Holiday";
            DateTime sDate = _day;
            ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);

            for (int i = 1; i <= 3; i++) // 뒤로 3년
            {
                addCal = new ComponentArt.Web.UI.CalendarDay(sDate.AddYears(Convert.ToInt32("-" + i.ToString())), cssClass, null, null, null);
                if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
            }

            for (int i = 1; i <= 3; i++)  // 앞으로 3년
            {
                addCal = new ComponentArt.Web.UI.CalendarDay(sDate.AddYears(i), cssClass, null, null, null);
                if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
            }
        }

        private void SolarCustomDays()
        {
            string strSolar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Solar", "");

            string[] arSolar = strSolar.Split(',');

            string targetDay = String.Empty;
            string beforeDay = String.Empty;
            string afterDay = String.Empty;

            //양력표시
            foreach (string _s in arSolar)
            {
                //ByFreeman 20130108
                if (_s.Length > 0)
                {
                    if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                    {
                        targetDay = _s.Split('|')[0];
                        beforeDay = _s.Split('|')[1];
                        afterDay = _s.Split('|')[2];
                    }
                    else
                    {
                        targetDay = _s;
                    }

                    DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.ToString("yyyy-") + targetDay);
                    SetSolarCustomDays(sDate);

                    if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                    {
                        string[] tmpBeforDay = beforeDay.Split(':');
                        foreach (string tmp in tmpBeforDay)
                        {
                            if (tmp == "") continue;
                            {
                                DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                                SetSolarCustomDays(bDate);
                            }
                        }
                    }

                    if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                    {
                        string[] tmpAfterDay = afterDay.Split(':');
                        foreach (string tmp in tmpAfterDay)
                        {
                            if (tmp == "") continue;
                            {
                                DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                                SetSolarCustomDays(aDate);
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 음력 표시(앞/뒤로 3년식)
        /// </summary>
        /// <param name="_day"></param>
        private void SetLunarCustomDays(DateTime _day)
        {
            string cssClass = "Holiday";
            DateTime sDate = _day;
            ComponentArt.Web.UI.CalendarDay addCal = new ComponentArt.Web.UI.CalendarDay(sDate, cssClass, null, null, null);
            if (!CalendarSingle.CustomDays.Contains(addCal)) CalendarSingle.CustomDays.Add(addCal);
        }

        private void LunarCustomDays()
        {
            string strLunar = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "Lunar", "");

            string[] arLunar = strLunar.Split(',');

            string targetDay = String.Empty;
            string beforeDay = String.Empty;
            string afterDay = String.Empty;

            for (int i = 0; i <= 3; i++) // 앞으로 3년
            {
                //음력
                foreach (string _s in arLunar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.AddYears(i).ToString("yyyy-") + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환

                        SetLunarCustomDays(sDate);

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                                SetLunarCustomDays(bDate);
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                                SetLunarCustomDays(aDate);
                            }
                        }

                    }
                }
            }

            for (int i = 1; i <= 3; i++) // 뒤로 3년
            {
                //음력
                foreach (string _s in arLunar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(CalendarSingle.TodaysDate.AddYears(Convert.ToInt32("-" + i.ToString())).ToString("yyyy-") + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환

                        SetLunarCustomDays(sDate);

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                DateTime bDate = sDate.AddDays(Convert.ToDouble("-" + tmp));
                                SetLunarCustomDays(bDate);
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                DateTime aDate = sDate.AddDays(Convert.ToDouble(tmp));
                                SetLunarCustomDays(aDate);
                            }
                        }

                    }
                }
            }
        }
        #endregion                

        #region (프로퍼티)

        public string WeContent { get; set; }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}