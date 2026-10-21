using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class WholeWeek : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수처리
        public int CategoryID;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어   
        public string SearchUsable = string.Empty;
        public string TmpDate = String.Empty;
        protected DateTime dtTargetDate;
        public string WeekTable = string.Empty;
        private int ListID;
        private string BeforeDayTag = string.Empty;
        private string NextDayTag = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.WholeWeek).Assembly;	//어셈블리 선언
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
        /// 작성자 : shine
        /// 작성일자 : 2012-02-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SettingSelectedDate(TmpDate);

            // 사용자 설정 상태 가져오기
            SearchUsable = "";
            DAL.Usable dal = new DAL.Usable();
            List<BE.UsableInfo> al = dal.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            foreach (BE.UsableInfo _Info in al)
            {
                SearchUsable = _Info.Usable;
            }

            foreach (string list in SearchUsable.Split(','))
            {
                if (list == "0")
                {
                    cbxUsable.Items[0].Selected = true;
                }

                if (list == "1")
                {
                    cbxUsable.Items[1].Selected = true;
                }

                if (list == "2")
                {
                    cbxUsable.Items[2].Selected = true;
                }
            }
            
            LoadList();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-24
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbxUsable.Items.Clear();
                cbxUsable.Items.Add(new ListItem("<p class=\"bappS\"><img src=\"" + BaseImgPath + "/RM/b1_w.gif\" /></p><p class=\"bfontS\">" + resMng.GetString("Usable0") + "</p>", "0"));
                cbxUsable.Items.Add(new ListItem("<p class=\"bappS\"><img src=\"" + BaseImgPath + "/RM/b2_w.gif\" /></p><p class=\"bfontS\">" + resMng.GetString("Usable1") + "</p>", "1"));
                cbxUsable.Items.Add(new ListItem("<p class=\"bappS\"><img src=\"" + BaseImgPath + "/RM/b3_w.gif\" /></p><p class=\"bfontS\">" + resMng.GetString("Usable2") + "</p>", "2"));
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-24
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
        }

        /// <summary>
        /// 현재 년월일 가져오기 [2012-01-01]
        /// </summary>
        /// <returns></returns>
        public string GetYearMonthDay()
        {
            string nowYearMonDay = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            if (TargetDay.ToString().Length == 1)
            {
                nowYearMonDay = nowYearMonDay + "-" + "0" + TargetDay.ToString();
            }
            else
            {
                nowYearMonDay = nowYearMonDay + "-" + TargetDay.ToString();
            }
            return nowYearMonDay;
        }

        /// <summary>
        /// 페이지 상단에 해당날짜 셋팅
        /// </summary>
        private void SettingSelectedDate(string _startDate)
        {
            //targetDate가 속한 주의 시작날짜와 마지막날짜를 알아내는 루틴.
            int interval = 0;//Convert.ToInt32(TargetDate.DayOfWeek);		

            dtTargetDate = Convert.ToDateTime(_startDate);
            TargetYear = dtTargetDate.Year;
            TargetMonth = dtTargetDate.Month;
            TargetDay = dtTargetDate.Day;

            interval = Convert.ToInt32(dtTargetDate.DayOfWeek);

            TargetStartDate = dtTargetDate.AddDays(-interval);

            //페이지 상단에 출력
            timeList.InnerText = TargetStartDate.Year + "-" + TargetStartDate.Month.ToString().PadLeft(2, '0') + "-" + TargetStartDate.Day.ToString().PadLeft(2, '0');
            EtimeList.InnerText = dtTargetDate.AddDays(6 - interval).Year + "-" + dtTargetDate.AddDays(6 - interval).Month.ToString().PadLeft(2, '0') + "-" + dtTargetDate.AddDays(6 - interval).Day.ToString().PadLeft(2, '0');

            TmpDate = TargetStartDate.Year + "-" + TargetStartDate.Month.ToString().PadLeft(2, '0') + "-" + TargetStartDate.Day.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 리스트 그리기
        /// </summary>
        private void LoadList()
        {
            StringBuilder sb = new StringBuilder();
            bool noDate = false;

            // 자원리스트
            DAL.List dalList = new DAL.List();
            DataSet dsList = dalList.GetWeekList(CurrentUserInfo.CompanyID, CategoryID);

            // 기간별 자원리스트
            DAL.Reverse dal = new DAL.Reverse();
            DataSet dsWeekList = dal.GetWeekList(CurrentUserInfo.CompanyID, CategoryID, TmpDate, SearchField, SearchValue, SearchUsable);

            sb.Append(CreateHeader());
            string tempWeekName = string.Empty;

            for (int i = 0; i < 7; i++)
            {
                string weekDay = String.Empty;
                DateTime weekDate = Convert.ToDateTime(TmpDate).AddDays(i);
                string tmpWeekName = DateHelper.GetCultureDate(weekDate, CurrentUserInfo.PreferredLanguage).Substring(0, 10) + " " + WeekDayName(i);
                string tmpWeekDay = weekDate.Year + "-" + weekDate.Month.ToString().PadLeft(2, '0') + "-" + weekDate.Day.ToString().PadLeft(2, '0');

                if (dsList != null)
                {
                    if (SearchValue == "")
                    {
                        #region 검색어 없을 경우

                        if (dsList.Tables[0].Rows.Count > 0)
                        {
                            sb.Append(CreateSubHeader(tmpWeekName));

                            foreach (DataRow dr in dsList.Tables[0].Rows)
                            {
                                ListID = dr["ListID"].ToString().ToNumber<int>();

                                // 자원명, 자원담당자
                                sb.Append("<tr>");
                                sb.Append(CreateResourceManager(dr["Title"].ToString(), dr["ManagerUserID"].ToString(), dr["ManyName"].ToString(), dr["Authority"].ToString()));

                                if (dsWeekList != null && dsWeekList.Tables[0].Rows.Count > 0)
                                {
                                    DataView dvList = dsWeekList.Tables[0].DefaultView;

                                    dvList.RowFilter = "CompanyID = " + CurrentUserInfo.CompanyID + " AND ListID = " + ListID
                                        + " AND (SDate = '" + tmpWeekDay + "'"
                                        + "     AND EDate >= '" + tmpWeekDay + "'"
                                        + "     OR SDate <= '" + tmpWeekDay + "'"
                                        + "     AND EDate = '" + tmpWeekDay + "'"
                                        + "     OR ((SDate < '" + tmpWeekDay + "') AND (EDate > '" + tmpWeekDay + "')))";

                                    if (dvList.Count > 0)
                                    {
                                        dvList.Sort = "StartDate ASC";
                                        sb.Append(CreateContents(dvList, weekDate, tmpWeekDay, dr["ManagerUserID"].ToString().ToNumber<int>()));
                                    }
                                    else
                                    {
                                        sb.Append("</td>");

                                        if (SearchValue == "")
                                        {
                                            #region 자원예약 없음
                                            sb.Append("<td>&nbsp;</td>");

                                            for (int hour = 2; hour < 15; hour++)
                                            {
                                                sb.Append("<td><a onclick=\"javascript:WholeWeek.Add('" + tmpWeekDay + "', '" + (hour + 5) + " ', '" + ListID + "')\"><span class='dataBlank'></span></a></td>");
                                            }

                                            sb.Append("<td class='last-child'>&nbsp;</td>");
                                            sb.Append("</tr>");
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    sb.Append("</td>");

                                    #region 자원예약 없음
                                    sb.Append("<td>&nbsp;</td>");

                                    for (int hour = 2; hour < 15; hour++)
                                    {
                                        sb.Append("<td><a onclick=\"javascript:WholeWeek.Add('" + tmpWeekDay + "', '" + (hour + 5) + " ', '" + ListID + "')\"><span class='dataBlank'></span></a></td>");
                                    }

                                    sb.Append("<td class='last-child'>&nbsp;</td>");
                                    sb.Append("</tr>");
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            #region 등록된 자원이 없을경우
                            noDate = true;
                            break;
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region 검색어 있을 경우

                        foreach (DataRow dr in dsList.Tables[0].Rows)
                        {
                            ListID = dr["ListID"].ToString().ToNumber<int>();

                            if (dsWeekList != null && dsWeekList.Tables[0].Rows.Count > 0)
                            {
                                DataView dvList = dsWeekList.Tables[0].DefaultView;

                                dvList.RowFilter = "CompanyID = " + CurrentUserInfo.CompanyID + " AND ListID = " + ListID
                                        + " AND (SDate = '" + tmpWeekDay + "'"
                                        + "     AND EDate >= '" + tmpWeekDay + "'"
                                        + "     OR SDate <= '" + tmpWeekDay + "'"
                                        + "     AND EDate = '" + tmpWeekDay + "'"
                                        + "     OR ((SDate < '" + tmpWeekDay + "') AND (EDate > '" + tmpWeekDay + "')))";

                                if (dvList.Count > 0)
                                {
                                    // 동일 날짜에 여러 자원이 입력 되었을 경우..
                                    if (tempWeekName != tmpWeekName)
                                    {
                                        tempWeekName = tmpWeekName;
                                        sb.Append(CreateSubHeader(tmpWeekName));
                                    }

                                    sb.Append("<tr>");
                                    // 자원명, 담당자
                                    sb.Append(CreateResourceManager(dvList[0]["Title"].ToString(), dvList[0]["ManagerUserID"].ToString(), dvList[0]["MManyName"].ToString(), dvList[0]["Authority"].ToString()));

                                    dvList.Sort = "StartDate ASC";
                                    sb.Append(CreateContents(dvList, weekDate, tmpWeekDay, dr["ManagerUserID"].ToString().ToNumber<int>()));
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            #region 검색 시 내용이 없을 경우, 등록된 자원이 없을 경우
            if ((SearchValue != "" && dsWeekList.Tables[0].Rows.Count == 0) || noDate == true)
            {
                sb.Append(CreateSubHeader(""));
                sb.Append("<tr>");
                sb.Append("<td class='first-child'></td>");
                sb.Append("<td colspan='15' class='last-child'>" + GetResource("NoContent") + "</td>");
                sb.Append("</tr>");
            }
            #endregion


            sb.Append("</table>");
            sb.Append("<div class='table_typeAbottom_left'><p></p></div>");

            WeekTable = sb.ToString();
        }

        #region 헤더

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 헤더 그리기
        /// </summary>
        /// <returns></returns>
        private string CreateHeader()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='table_typeD'>");
            sb.Append("<caption> 자원관리 주간</caption>");
            sb.Append("<colgroup>");
            sb.Append("<col width='220px' />");

            for (int i = 0; i < 15; i++)
            {
                sb.Append("<col width='5%' />");
            }

            sb.Append("</colgroup>");
            sb.Append("<tr>");
            sb.Append(" <th class='first-child'>" + GetResource("ResourcesTitle") + "</th>");
            sb.Append(" <th colspan='15' class='last-child'>" + GetResource("Time") + "&nbsp;</th>");
            sb.Append("</tr>");

            return sb.ToString();
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 서브헤더 그리기(날짜, 시간)
        /// </summary>
        /// <param name="_weekName">Name of the _week.</param>
        /// <returns></returns>
        private string CreateSubHeader(string _weekName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr>");
            //sb.Append(" <td class='first-child-day-c'>" + _weekName + "</td>");
            sb.Append(" <td class='th2'>" + _weekName + "</td>");
            sb.Append(" <td class='th2'><-</td>");

            for (int hour = 7; hour < 20; hour++)
            {
                sb.Append(" <td class='th2'>" + hour + "</td>");
            }

            //sb.Append(" <td class='last-child-day-c'>-></td>");
            sb.Append(" <td class='th2'>-></td>");
            sb.Append("</tr>");

            return sb.ToString();
        }

        #endregion

        #region 자원명, 담당자

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 자원명, 담당자 그리기
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_managerUserID"></param>
        /// <param name="_managerName"></param>
        /// <param name="_authority"></param>
        /// <returns></returns>
        private string CreateResourceManager(string _title, string _managerUserID, string _managerName, string _authority)
        {
            StringBuilder sb = new StringBuilder();

            // 자원담당자명
            string[] arrManyName = _managerName.Split('|');
            string tmpManagerName = "";
            if (arrManyName.Length == 7)
            {
                tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                       arrManyName[2], arrManyName[3], arrManyName[4],
                                                       arrManyName[5], arrManyName[6], PreLang);
            }

            tmpManagerName = resMng.GetString("ResourcesUser") + " : " + tmpManagerName;

            sb.Append(" <td class='first-child'>" + _title + "<br />");
            sb.Append(" <span>");

            if (_authority == "1")
            {
                sb.Append("<a onclick=\"AccountInfo('" + _managerUserID + "');return false;\" class='managerA'>");                
            }
            else
            {
                sb.Append("<a onclick=\"AccountInfo('" + _managerUserID + "');return false;\" style='cursor:pointer'>");                
            }

            sb.Append(tmpManagerName + "</a></span>");
            //sb.Append(tmpManagerName + "</a></span> </td>");

            return sb.ToString();
        }

        #endregion

        #region 내용

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 내용 그리기
        /// </summary>
        /// <param name="_dv">The _DV.</param>
        /// <param name="_weekDate">The _week date.</param>
        /// <returns></returns>
        private string CreateContents(DataView _dv, DateTime _weekDate, string _weekDay, int _managerUserID)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbL = new StringBuilder();
            DateTime nowDate;

            if (_dv.Count > 0)
            {         
                //sb.Append("<td>");

                string CheckYN = "N";

                if (_dv != null && _dv.Count > 0)
                {
                    for (int i = 0; i < _dv.Count; i++)
                    {
                        if (CheckYN == "N" && (_dv[i]["Usable"].ToString() == "0") && (CurrentUserInfo.UserID == _dv[i]["WriteUserID"].ToString().ToNumber<int>()))
                        {
                            CheckYN = "Y";
                            sb.Append("&nbsp;<span class='requestMM'><a onclick=\"javascript:WholeWeek.Send('" + _dv[i]["ListID"].ToString() + "', '" + _weekDay + "')\" class='arequest btn btn-sm btn-secondary'>" + resMng.GetString("RequestSend") + "</a></span>");
                        } 

                        #region <-

                        DateTime startDate = Convert.ToDateTime(_dv[i]["StartDate"].ToString());

                        if (CheckTime3(_weekDate, startDate))
                        {
                            string conMenu = "";
                            string className = "";
                            string patternImg = "#" + _dv[i]["UserImg"].ToString().ToNull();

                            if (String.IsNullOrEmpty(_dv[i]["UserImg"].ToString().ToNull()))
                            {
                                patternImg = "#FF0000";
                            }

                            string styleTag = string.Empty;

                            #region className 설정
                            switch (_dv[i]["Usable"].ToString())
                            {
                                case "0":
                                    {
                                        conMenu = "callContextAMenu";
                                        className = "unapproved";
                                        styleTag = "style='width:100%;background:" + patternImg + " url(" + BaseImgPath + "/RM/p5_w.gif)  repeat-x scroll top left;'";
                                    }
                                    break;
                                case "1":
                                    {
                                        conMenu = "callContextBMenu";
                                        className = "approved";
                                        styleTag = "style='width:100%;background:" + patternImg + ";'";
                                    }
                                    break;
                                case "2":
                                    {
                                        conMenu = "callContextCMenu";
                                        className = "refusal";
                                        styleTag = "style='width:100%;background:" + patternImg + " url(" + BaseImgPath + "/RM/p6_w.gif)  repeat-x scroll top left;'";
                                    }
                                    break;
                            }
                            #endregion


                            int tmpAuth = 1;
                            if (_dv[i]["IsGuard"].ToString() == "1")
                            {
                                //관리자 권한 체크
                                //만약 보안예약일 경우
                                tmpAuth = CheckAuth(int.Parse(_dv[i]["ReservationID"].ToString()), int.Parse(_dv[i]["WriteUserID"].ToString()), _managerUserID);
                            }

                            if (_dv[i]["Authority"].ToString() == "1")
                            {
                                #region 관리자 승인Y
                                if (tmpAuth > 0)
                                {
                                    sbL.Append("<a class='" + conMenu + "' id='pu_" + _dv[i]["ReservationID"].ToString() + "'>");
                                    sbL.Append("<span class='" + className + "' " + styleTag + "></span></a>");
                                }
                                else
                                {
                                    sbL.Append("<span class='" + className + "' " + styleTag + "></span>");
                                }
                                
                                
                                #endregion
                            }
                            else
                            {
                                #region 관리자 승인N
                                if (tmpAuth > 0)
                                {
                                    sbL.Append("<a onclick=\"javascript:WholeWeek.View('" + _dv[i]["ReservationID"].ToString() + "')\">");
                                    sbL.Append("<span class='" + className + "' " + styleTag + "></span></a>");
                                }
                                else
                                {
                                    sbL.Append("<span class='" + className + "' " + styleTag + "></span>");
                                }                                
                                
                                #endregion
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    sbL.Append("&nbsp;");
                }

                sb.Append("</td><td>");
                sb.Append(sbL.ToString());
                sb.Append("</td>");

                #region 7시 ~ 19시

                for (int hour = 2; hour < 15; hour++)
                {
                    nowDate = _weekDate.AddHours(hour + 5);
                    sb.Append("<td>" + GetStatusHtml(_dv, nowDate, hour, _weekDay, _managerUserID) + "</td>");
                }

                #endregion

                #region ->
                DateTime endDate = new DateTime();
                endDate = Convert.ToDateTime(_dv[_dv.Count - 1]["EndDate"].ToString());

                _weekDate = _weekDate.AddHours(19);

                if (CheckTime(_weekDate, endDate))
                {
                    sb.Append("<td class='last-child'>" + NextDayTag + "</td>");
                    NextDayTag = "";
                }
                else
                {
                    sb.Append("<td class='last-child'>&nbsp;</td>");
                }

                #endregion

                sb.Append("</tr> ");
            }

            #region 해당 날짜에 자원예약이 없을 경우

            if (sb.Length == 0)
            {
                sb.Append("<td>&nbsp;</td>");

                for (int hour = 2; hour < 15; hour++)
                {
                    sb.Append("<td><a onclick=\"javascript:WholeWeek.Add('" + _weekDay + "', '" + (hour + 5) + " ', '" + ListID + "')\"><span class='dataBlank'></span></a></td>");
                }

                sb.Append("<td class='last-child'>&nbsp;</td>");
            }

            #endregion

            sb.Append("</tr>");

            return sb.ToString();
        }

        #endregion

        #region 상태별 HTML 그리기

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 상태별 HTML 그리기
        /// </summary>
        /// <param name="_dv">The _DV.</param>
        /// <param name="_nowDate">The _now date.</param>
        /// <param name="_hour">The _hour.</param>
        /// <returns></returns>
        private string GetStatusHtml(DataView _dv, DateTime _nowDate, int _hour, string _weekDay, int _managerUserID)
        {
            StringBuilder sb = new StringBuilder();

            string widthTag = string.Empty;
            string className = "";
            string conMenu = "";
            string styleTag = string.Empty;
            string title = string.Empty;
            string patternImg = string.Empty;
            string writeName = string.Empty;
            string beforeTitle = string.Empty;
            string admin = string.Empty;
            int reservationID = 0;

            string beforeIsGuard = string.Empty;
            string beforeWriteUserID = string.Empty;

            string isGuard = string.Empty;
            string writeUserID = string.Empty;
            string beforeReservationID = string.Empty;
            int itemStep = 0;
            int beforeHour = 0;
            int beforeMinute = 0;
            string BeforeTag = string.Empty;
            bool check = false;

            if (_dv != null && _dv.Count > 0)
            {
                for (int i = 0; i < _dv.Count; i++)
                {
                    DateTime sDate = new DateTime();
                    DateTime eDate = new DateTime();

                    if (_dv[i]["ParentReservationID"].ToString().ToNumber<int>() > 0)
                    {
                        sDate = Convert.ToDateTime(_dv[i]["StartDate"].ToString());
                        eDate = Convert.ToDateTime(_dv[i]["EndDate"].ToString());
                    }

                    #region 분단위 넓이 조절

                    if (IsWorkTime(_nowDate, sDate, eDate))
                    {
                        check = true;

                        // 패턴(이미지)
                        patternImg = "#" + _dv[i]["UserImg"].ToString().ToNull();

                        if (String.IsNullOrEmpty(_dv[i]["UserImg"].ToString().ToNull()))
                        {
                            patternImg = "#FF0000";
                        }

                        admin = CheckAdmin(_dv[i]["Authority"].ToString(), _dv[i]["ManagerUserID"].ToString().ToNumber<int>());

                        #region 예약자
                        writeName = this.GetLangName(_dv[i]["NameBase"].ToString(), _dv[i]["NameKo"].ToString(),
                                                        _dv[i]["NameEn"].ToString(), _dv[i]["NameJa"].ToString(), _dv[i]["NameFr"].ToString(),
                                                        _dv[i]["NameZhcn"].ToString(), _dv[i]["NameZhtw"].ToString(), PreLang);
                        #endregion
                        // 제목+예약자이름
                        title = _dv[i]["Subject"].ToString() + ": " + writeName;
                        reservationID = _dv[i]["ReservationID"].ToString().ToNumber<int>();

                        isGuard = _dv[i]["IsGuard"].ToString();
                        writeUserID = _dv[i]["WriteUserID"].ToString();

                        #region className 설정
                        switch (_dv[i]["Usable"].ToString())
                        {
                            case "0":
                                {
                                    conMenu = "callContextAMenu";
                                    className = "unapproved";
                                    styleTag = "background:" + patternImg + " url(" + BaseImgPath + "/RM/p5_w.gif)  repeat-x scroll top left;'";
                                }
                                break;
                            case "1":
                                {
                                    conMenu = "callContextBMenu";
                                    className = "approved";
                                    styleTag = "background:" + patternImg + ";'";
                                }
                                break;
                            case "2":
                                {
                                    conMenu = "callContextCMenu";
                                    className = "refusal";
                                    styleTag = "background:" + patternImg + " url(" + BaseImgPath + "/RM/p6_w.gif)  repeat-x scroll top left;'";
                                }
                                break;
                        }
                        #endregion

                        #region 분단위 넓이 조절

                        if (sDate.Day == _nowDate.Day && sDate.Hour == _nowDate.Hour)
                        {
                            widthTag = "style='float:right; width:" + CalcWidth(sDate.Minute, 60) + "%;" + styleTag;
                        }
                        else if (eDate.Day == _nowDate.Day && eDate.Hour == _nowDate.Hour)
                        {
                            widthTag = "style='float:left; width:" + CalcWidth(eDate.Minute, 60) + "%;" + styleTag;
                        }
                        else
                        {
                            widthTag = "style='width:100%;" + styleTag;
                        }

                        if (sDate.Day == eDate.Day && sDate.Hour == eDate.Hour)
                        {
                            string alignTag = (sDate.Hour < 30) ? "left" : "right";
                            widthTag = "style='float:" + alignTag + "; width:" + CalcWidth(eDate.Minute, 60) + "%;" + styleTag;
                        }

                        #endregion

                        if (beforeHour == _nowDate.Hour)
                        {
                            itemStep++;
                            BeforeDayTag += widthTag + "^";
                            beforeTitle += title + "^";
                            beforeReservationID += reservationID + "^";

                            if (beforeMinute == sDate.Minute)
                            {
                                BeforeTag += "1^";
                            }
                            else
                            {
                                beforeMinute = sDate.Minute;
                                BeforeTag += "0^";
                            }

                            beforeIsGuard += _dv[i]["IsGuard"].ToString() + "^";
                            beforeWriteUserID += _dv[i]["WriteUserID"].ToString() + "^";
                        }
                        else
                        {
                            beforeHour = _nowDate.Hour;
                            beforeMinute = sDate.Minute;
                            BeforeDayTag = widthTag + "^";
                            beforeTitle = title + "^";
                            beforeReservationID = reservationID.ToString() + "^";
                            BeforeTag = "0^";

                            beforeIsGuard += _dv[i]["IsGuard"].ToString() + "^";
                            beforeWriteUserID += _dv[i]["WriteUserID"].ToString() + "^";
                        }
                    }

                    #endregion
                }
            }

            // 예약 시간에 해당하는 막대 그리기
            if (check == true)
            {
                if (admin == "Y")
                {
                    #region 관리자승인Y

                    // 같은 시간에30분 단위로 예약이 되었을 경우..
                    if (itemStep > 0)
                    {

                        if (itemStep > 0)
                        {
                            string[] ArrReservationID = beforeReservationID.Split('^');
                            string[] ArrBeforeDayTag = BeforeDayTag.Split('^');
                            string[] ArrBeforeTitle = beforeTitle.Split('^');
                            string[] ArrBeforeTag = BeforeTag.Split('^');
                            string CheckWidth = "N";

                            string[] ArrIsGuard = beforeIsGuard.Split('^');
                            string[] ArrWriteUserID = beforeWriteUserID.Split('^');

                            for (int i = 0; i < ArrReservationID.Length - 1; i++)
                            {
                                if (ArrBeforeTag[i] == "1" && CheckWidth == "N")
                                {
                                    sb.Append("<br>");
                                }

                                int tmpAuth = 1;
                                if (ArrIsGuard[i] == "1")
                                {
                                    //해당 일정의 권한체크
                                    tmpAuth = CheckAuth(int.Parse(ArrReservationID[i]), int.Parse(ArrWriteUserID[i]), _managerUserID);
                                }

                                if(tmpAuth > 0)
                                {
                                    sb.Append("<a class='" + conMenu + "' id='pu_" + ArrReservationID[i] + "'>");
                                    sb.Append("<span class='" + className + "' " + ArrBeforeDayTag[i] + " title='" + ArrBeforeTitle[i] + "'></span></a>");
                                }
                                else
                                {
                                    sb.Append("<span class='" + className + "' " + ArrBeforeDayTag[i] + " title='" + GetResource("SecurityReservation") + "'></span>");
                                }

                                

                                if (ArrBeforeDayTag[i].IndexOf("width:50%") > 0)
                                {
                                    CheckWidth = "N";
                                }
                                else
                                {
                                    CheckWidth = "Y";
                                }
                            }
                        }
                    }
                    else
                    {
                        int tmpAuth = 1;
                        if (isGuard == "1")
                        {
                            //해당 일정의 권한체크
                            tmpAuth = CheckAuth(reservationID, int.Parse(writeUserID), _managerUserID);
                        }

                        if (tmpAuth > 0)
                        {
                            sb.Append("<a class='" + conMenu + "' id='pu_" + reservationID + "'>");
                            sb.Append("<span class='" + className + "' " + widthTag + " title='" + title + "' ></span></a></td>");
                        }
                        else
                        {
                            sb.Append("<span class='" + className + "' " + widthTag + " title='" + GetResource("SecurityReservation") + "' ></span></td>");
                        }

                        
                    }

                    #endregion
                }
                else
                {
                    #region 관리자승인N

                    // 같은 시간에30분 단위로 예약이 되었을 경우..
                    if (itemStep > 0)
                    {
                        string[] ArrReservationID = beforeReservationID.Split('^');
                        string[] ArrBeforeDayTag = BeforeDayTag.Split('^');
                        string[] ArrBeforeTitle = beforeTitle.Split('^');
                        string[] ArrBeforeTag = BeforeTag.Split('^');

                        string[] ArrIsGuard = beforeIsGuard.Split('^');
                        string[] ArrWriteUserID = beforeWriteUserID.Split('^');
                        string CheckWidth = "N";

                        for (int i = 0; i < ArrReservationID.Length - 1; i++)
                        {
                            if (ArrBeforeTag[i] == "1" && CheckWidth == "N")
                            {
                                sb.Append("<br>");
                            }

                            int tmpAuth = 1;
                            if (ArrIsGuard[i] == "1")
                            {
                                //해당 일정의 권한체크
                                tmpAuth = CheckAuth(int.Parse(ArrReservationID[i]), int.Parse(ArrWriteUserID[i]), _managerUserID);
                            }

                            if(tmpAuth > 0)
                            {
                                sb.Append("<a onclick=\"javascript:WholeWeek.View('" + ArrReservationID[i] + "')\">");
                                sb.Append("<span class='" + className + "' " + ArrBeforeDayTag[i] + " title='" + ArrBeforeTitle[i] + "'></span></a>");
                            }
                            else
                            {
                                sb.Append("<span class='" + className + "' " + ArrBeforeDayTag[i] + " title='" + GetResource("SecurityReservation") + "'></span>");
                            }                            

                            if (ArrBeforeDayTag[i].IndexOf("width:50%") > 0)
                            {
                                CheckWidth = "N";
                            }
                            else
                            {
                                CheckWidth = "Y";
                            }
                        }
                    }
                    else
                    {
                        int tmpAuth = 1;
                        if (isGuard == "1")
                        {
                            //해당 일정의 권한체크
                            tmpAuth = CheckAuth(reservationID, int.Parse(writeUserID), _managerUserID);
                        }

                        if (widthTag.IndexOf("float:right") > 0 && widthTag.IndexOf("width:50%") > 0 && itemStep == 0)
                        {
                            if(tmpAuth > 0)
                            {
                                sb.Append("<a onclick=\"javascript:WholeWeek.Add('" + _weekDay + "', '" + (_hour + 5) + "', '" + ListID + "')\"><span style=\"float:left; width:50%\"class='dataBlank'></span></a>");
                                sb.Append("<a onclick=\"javascript:WholeWeek.View('" + reservationID + "')\">");
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + title + "'></span></a></td>");
                            }
                            else
                            {
                                sb.Append("<a onclick=\"javascript:WholeWeek.Add('" + _weekDay + "', '" + (_hour + 5) + "', '" + ListID + "')\"><span style=\"float:left; width:50%\"class='dataBlank'></span></a>");
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + GetResource("SecurityReservation") + "'></span></td>");
                            }
                            
                        }
                        else if (widthTag.IndexOf("float:left") > 0 && widthTag.IndexOf("width:50%") > 0 && itemStep == 0)
                        {
                            if (tmpAuth > 0)
                            {
                                sb.Append("<a onclick=\"javascript:WholeWeek.View('" + reservationID + "')\">");
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + title + "'></span></a>");
                                sb.Append("<a onclick=\"javascript:WholeWeek.Add('" + _weekDay + "', '" + (_hour + 5) + "', '" + ListID + "')\"><span style=\"float:right; width:50%\" class='dataBlank'></span></a></td>");
                            }
                            else
                            {
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + GetResource("SecurityReservation") + "'></span>");
                                sb.Append("<a onclick=\"javascript:WholeWeek.Add('" + _weekDay + "', '" + (_hour + 5) + "', '" + ListID + "')\"><span style=\"float:right; width:50%\" class='dataBlank'></span></a></td>");
                            }
                            
                        }
                        else
                        {
                            if (tmpAuth > 0)
                            {
                                sb.Append("<a onclick=\"javascript:WholeWeek.View('" + reservationID + "')\">");
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + title + "'></span></a></td>");
                            }
                            else
                            {
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + GetResource("SecurityReservation") + "'></span></td>");
                            }
                            
                        }
                    }

                    #endregion
                }

                NextDayTag = sb.ToString();
                BeforeDayTag = "";
                check = false;
            }
            else
            {
                sb.Append("<a onclick=\"javascript:WholeWeek.Add('" + _weekDay + "', '" + (_hour + 5) + "', '" + ListID + "')\"><span class='dataBlank'></span></a>");
            }

            return sb.ToString();
        }

        #endregion

        #region 기타함수

        /// <summary>
        /// 해당일정의 권한체크
        /// </summary>
        /// <param name="_reservationID"></param>
        /// <param name="_writeUserID"></param>
        /// <returns></returns>
        private int CheckAuth(int _reservationID, int _writeUserID, int _managerUserID)
        {
            int tmpAuth = 1;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleRM = CurrentUserInfo.IsInRole("resource");//resource admin
            if (_writeUserID == CurrentUserInfo.UserID || CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleRM)
            {
                tmpAuth = 1;
            }
            else
            {
                if (_managerUserID == CurrentUserInfo.UserID)
                {
                    tmpAuth = 1;
                }
                else
                {
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    tmpAuth = suDal.GetShareUserCntByID(CurrentUserInfo.CompanyID, _reservationID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
                }
            }
            return tmpAuth;
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 요일 정보 가져오기
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string WeekDayName(int _gubun)
        {
            string rtnVal = "";
            switch (_gubun)
            {
                case 0: rtnVal = "(" + GetResource("Sun") + ")"; break;
                case 1: rtnVal = "(" + GetResource("Mon") + ")"; break;
                case 2: rtnVal = "(" + GetResource("Tue") + ")"; break;
                case 3: rtnVal = "(" + GetResource("Wed") + ")"; break;
                case 4: rtnVal = "(" + GetResource("Thu") + ")"; break;
                case 5: rtnVal = "(" + GetResource("Fri") + ")"; break;
                case 6: rtnVal = "(" + GetResource("Sat") + ")"; break;
            };

            return rtnVal;
        }

        /// <summary>
        /// Checks the time.
        /// </summary>
        /// <param name="_tmpDate">The _TMP date.</param>
        /// <param name="_sDate">The _s date.</param>
        /// <param name="_eDate">The _e date.</param>
        /// <returns></returns>
        private bool CheckTime(DateTime _tmpDate, DateTime _sDate, DateTime _eDate)
        {
            bool rtn = false;

            string tmpDate = _tmpDate.Year + _tmpDate.Month.ToString().PadLeft(2, '0') + _tmpDate.Day.ToString().PadLeft(2, '0');
            string sDate = _sDate.Year + _sDate.Month.ToString().PadLeft(2, '0') + _sDate.Day.ToString().PadLeft(2, '0');
            string eDate = _eDate.Year + _eDate.Month.ToString().PadLeft(2, '0') + _eDate.Day.ToString().PadLeft(2, '0');

            if (sDate.ToNumber<int>() <= tmpDate.ToNumber<int>())
            {
                if (eDate.ToNumber<int>() >= tmpDate.ToNumber<int>())
                {
                    rtn = true;
                }
            }

            return rtn;
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 예약시간 반환
        /// </summary>
        /// <param name="_nowDate">The _now date.</param>
        /// <param name="_sDate">The _s date.</param>
        /// <param name="_eDate">The _e date.</param>
        /// <returns>
        ///   <c>true</c> if [is work time] [the specified _now date]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsWorkTime(DateTime _nowDate, DateTime _sDate, DateTime _eDate)
        {
            bool rtn = false;
            TimeSpan sp1 = _sDate.Subtract(_nowDate);
            TimeSpan sp2 = _nowDate.Subtract(_eDate);

            if (sp1.TotalMinutes <= 0 && sp2.TotalMinutes < 0)
            {
                rtn = true;
            }
            else if ((sp1.TotalMinutes > 0 && sp1.TotalMinutes < 60) && sp2.TotalMinutes < 0)
            {
                rtn = true;
            }
            return rtn;
        }

        /// <summary>
        /// Checks the time3.
        /// </summary>
        /// <param name="_tmpDate">The _TMP date.</param>
        /// <param name="_sDate">The _s date.</param>
        /// <returns></returns>
        private bool CheckTime3(DateTime _tmpDate, DateTime _sDate)
        {
            bool rtn = false;

            string tmpDate = _tmpDate.Year + _tmpDate.Month.ToString().PadLeft(2, '0') + _tmpDate.Day.ToString().PadLeft(2, '0');
            string eDate = _sDate.Year + _sDate.Month.ToString().PadLeft(2, '0') + _sDate.Day.ToString().PadLeft(2, '0');

            if (eDate.ToNumber<int>() < tmpDate.ToNumber<int>())
            {
                rtn = true;
            }

            return rtn;
        }

        /// <summary>
        /// Checks the time.
        /// </summary>
        /// <param name="_tmpDate">The _TMP date.</param>
        /// <param name="_eDate">The _e date.</param>
        /// <returns></returns>
        private bool CheckTime(DateTime _tmpDate, DateTime _eDate)
        {
            bool rtn = false;

            string tmpDate = _tmpDate.Year + _tmpDate.Month.ToString().PadLeft(2, '0') + _tmpDate.Day.ToString().PadLeft(2, '0');
            string eDate = _eDate.Year + _eDate.Month.ToString().PadLeft(2, '0') + _eDate.Day.ToString().PadLeft(2, '0');

            if (eDate.ToNumber<int>() > tmpDate.ToNumber<int>())
            {
                rtn = true;
            }
            else if (eDate.ToNumber<int>() == tmpDate.ToNumber<int>())
            {
                int tmpTime = 19;
                int eTime = _eDate.Hour.ToString().ToNumber<int>();

                if (eTime > tmpTime)
                {
                    rtn = true;
                }
                else if (eTime == tmpTime)
                {
                    int eMinute = _eDate.Minute.ToString().ToNumber<int>();

                    if (eMinute > 0)
                        rtn = true;
                }
            }

            return rtn;
        }


        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 예약 된 부분 막대 너비
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        private string CalcWidth(int s, int b)
        {
            if (s == 0) return "100";

            double rtn = (Convert.ToDouble(s) / Convert.ToDouble(b)) * 100;
            return rtn.ToString();
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 관리자 승인여부
        /// </summary>
        /// <param name="_admin">The _admin.</param>
        /// <param name="_managerUserID">The _manager user ID.</param>
        /// <returns></returns>
        private string CheckAdmin(string _admin, int _managerUserID)
        {
            string admin = "N";

            if (_admin == "1")
            {
                /*
                // 관리자 권한 
                bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
                bool isRoleRM = CurrentUserInfo.IsInRole("resource");//resource admin

                if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleRM)
                {
                    admin = "Y";
                }
                else
                {
                */
                    if (_managerUserID == CurrentUserInfo.UserID)
                    {
                        admin = "Y";
                    }
                //}
            }

            return admin;
        }
        #endregion

        #endregion

        #region (프로퍼티)
        /// <summary>
        /// 
        /// </summary>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }

        /// <summary>
        /// 시작일자
        /// </summary>
        public DateTime TargetStartDate
        {
            get
            {
                DateTime tmpDate = new DateTime(TargetYear, TargetMonth, TargetDay);
                return (ViewState["TargetStartDate"] == null) ? tmpDate.AddDays(-Convert.ToInt32(tmpDate.DayOfWeek)) : Convert.ToDateTime(ViewState["TargetStartDate"]);
            }
            set { ViewState["TargetStartDate"] = value; }
        }


        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}