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
    public partial class WholeDay : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수처리
        public int CategoryID;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어  
        public string SearchUsable = string.Empty;
        public string TmpDate = String.Empty;
        public string DayTable = String.Empty;
        private DateTime dtNowDate;
        private int NextDayGap;
        private int BeforeDayGap;
        private string NextDayTag = string.Empty;
        private string BeforeDayTag = string.Empty;
        private string ListID = "0";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.WholeDay).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-02-20
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

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
        /// 작성일자 : 2012-02-20
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
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            dtNowDate = Convert.ToDateTime(TmpDate);
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
        /// 작성자 : shine
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 리스트 그리기
        /// </summary>
        private void LoadList()
        {
            StringBuilder sb = new StringBuilder();

            DAL.Reverse dal = new DAL.Reverse();
            DataSet ds = dal.GetToDayList(CurrentUserInfo.CompanyID, CategoryID, TmpDate, SearchField, SearchValue, SearchUsable);

            sb.Append(CreateHeader());

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;
                string tmpListID = "";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ListID = ds.Tables[0].Rows[i]["ListID"].ToString();
                    string authority = ds.Tables[0].Rows[i]["Authority"].ToString();

                    dv.RowFilter = "CompanyID = " + CurrentUserInfo.CompanyID + " AND ListID = " + ListID;

                    if (dv.Count > 0)
                    {
                        if (tmpListID != ListID)
                        {
                            tmpListID = ListID;

                            sb.Append(CreateContents(dv, authority));
                        }
                    }
                }
            }
            else
            {
                sb.Append("<tr>");
                sb.Append("<td class='first-child'></td>");
                sb.Append("<td colspan='15' class='last-child'>" + GetResource("NoContent") + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            sb.Append("<div class='table_typeAbottom_left'><p></p></div>");

            DayTable = sb.ToString();
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

            string weekName = WeekDayName(Convert.ToDateTime(TmpDate));
            DateTime dt = Convert.ToDateTime(TmpDate);
            string tmpWeekName = DateHelper.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);

            sb.Append("<table class='table_typeD'>");
            sb.Append("<caption>" + GetResource("RM") + "</caption>");
            sb.Append("<colgroup>");
            sb.Append("<col width='220px' />");

            for (int i = 0; i < 15; i++)
            {
                sb.Append("<col width='5%' />");
            }

            sb.Append("</colgroup>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append(" <th class='first-child'>" + GetResource("ResourcesTitle") + "</th>");
            sb.Append(" <th class='last-child' colspan='15' >" + GetResource("Time") + "&nbsp;</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tr>");
            //sb.Append(" <td class='first-child-day-c'>" + tmpWeekName + weekName + "</td>");
            sb.Append(" <td class='th2'>" + tmpWeekName + weekName + "</td>");
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

        #region 내용

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 내용 그리기
        /// </summary>
        /// <param name="_dv">The _DV.</param>
        /// <param name="_usable">The _usable.</param>
        /// <returns></returns>
        private string CreateContents(DataView _dv, string _authority)
        {            
            StringBuilder sb = new StringBuilder();
            StringBuilder sbL = new StringBuilder();
            DateTime nowDate;
            string managerName = string.Empty;
            int managerUserID = _dv[0]["ManagerUserID"].ToString().ToNumber<int>();

            //자원 담당자 이름
            string[] arrManyName = _dv[0]["MManyName"].ToString().Split('|');
            string tmpManagerName = "";
            if (arrManyName.Length == 7)
            {
                tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                       arrManyName[2], arrManyName[3], arrManyName[4],
                                                       arrManyName[5], arrManyName[6], PreLang);
            }

            managerName = GetResource("ResourcesUser") + " : " + tmpManagerName;

            sb.Append("<tr>");
            sb.Append(" <td class='first-child'>" + _dv[0]["Title"].ToString() + "<br />");
            sb.Append(" <span>");

            if (_authority == "1")
            {
                sb.Append("<a onclick=\"AccountInfo('" + _dv[0]["ManagerUserID"].ToString() + "');return false;\" class='managerA'>");
            }
            else
            {
                sb.Append("<a onclick=\"AccountInfo('" + _dv[0]["ManagerUserID"].ToString() + "');return false;\" style='cursor:pointer'>");
            }

            //sb.Append(managerName + "</a></span> </td>");
            sb.Append(managerName + "</a></span>");

            #region <-

            //sb.Append("<td>");

            string CheckYN = "N";

            if (_dv != null && _dv.Count > 0)
            {
                for (int i = 0; i < _dv.Count; i++)
                {
                    if (CheckYN == "N" && (_dv[i]["Usable"].ToString() == "0") && (CurrentUserInfo.UserID == _dv[i]["WriteUserID"].ToString().ToNumber<int>()))
                    {
                        CheckYN = "Y";
                        sb.Append("&nbsp;<span class='requestMM'><a onclick=\"javascript:WholeDay.Send('" + _dv[i]["ListID"].ToString() + "')\" class='arequest btn btn-sm btn-secondary'>" + resMng.GetString("RequestSend") + "</a></span>");
                    } 

                    bool check = false;

                    DateTime startDate = new DateTime();
                    if (!String.IsNullOrEmpty(_dv[i]["StartDate"].ToString()))
                    {
                        startDate = Convert.ToDateTime(_dv[i]["StartDate"].ToString());
                        TimeSpan span = dtNowDate.Subtract(startDate);
                        check = true;
                    }

                    if (check && CheckTime3(dtNowDate, startDate))
                    {
                        string className = "";
                        string patternImg = string.Empty;
                        string styleTag = string.Empty;
                        string conMenu = "";

                        // 사용자가 선택한 색상이미지
                        patternImg = "#" + _dv[i]["UserImg"].ToString().ToNull();

                        if (String.IsNullOrEmpty(_dv[i]["UserImg"].ToString().ToNull()))
                        {
                            patternImg = "#FF0000";
                        }

                        #region className 설정
                        switch (_dv[0]["Usable"].ToString())
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
                        if(_dv[i]["IsGuard"].ToString() == "1")
                        {
                            //관리자 권한 체크
                            //만약 보안예약일 경우
                            tmpAuth = CheckAuth(int.Parse(_dv[i]["ReservationID"].ToString()), int.Parse(_dv[i]["WriteUserID"].ToString()), managerUserID);                            
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
                                sbL.Append("<a onclick=\"javascript:WholeDay.View('" + _dv[i]["ReservationID"].ToString() + "')\">");
                                sbL.Append("<span class='" + className + "' " + styleTag + "></span></a>");
                            }
                            else
                            {
                                sbL.Append("<span class='" + className + "' " + styleTag + "></span>");
                            }
                            
                            #endregion
                        }
                    }
                }
            }
            else
            {
                sbL.Append("&nbsp;");
            }

            sb.Append("</td><td>");
            sb.Append(sbL.ToString());
            sb.Append("</td>");
            #endregion

            #region 7시~19시

            for (int hour = 2; hour < 15; hour++)
            {
                nowDate = dtNowDate.AddHours(hour + 5);
                sb.Append("<td>" + GetStatusHtml(_dv, nowDate, hour, managerUserID) + "</td>");
            }

            #endregion

            #region ->
            if (NextDayGap > 0)
            {
                sb.Append("<td class='last-child'>&nbsp;</td>");
            }
            else
            {
                // 다음날짜와 이어져있을 경우
                sb.Append("<td class='last-child'>" + NextDayTag + "</td>");
                NextDayTag = "";
                NextDayGap = 0;
            }

            #endregion

            sb.Append("</tr> ");

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
        /// 작성목적 : 정보 할당
        /// </summary>
        /// <param name="_dv">The _DV.</param>
        /// <param name="_idx">The _idx.</param>
        /// <returns></returns>
        private BE.ReserveInfo GetInfo(DataView _dv, int _idx)
        {
            BE.ReserveInfo info = new BE.ReserveInfo();

            if (_dv != null && _dv.Count > 0)
            {
                #region 정보 담기

                info.ListID = _dv[_idx]["ListID"].ToString().ToNumber<int>();
                info.ReservationID = _dv[_idx]["ReservationID"].ToString().ToNumber<int>();
                info.ParentReservationID = _dv[_idx]["ParentReservationID"].ToString().ToNumber<int>();
                info.IsGuard = _dv[_idx]["IsGuard"].ToString();
                if (info.ParentReservationID > 0)
                {
                    info.Subject = _dv[_idx]["Subject"].ToString();
                    info.UserImg = _dv[_idx]["UserImg"].ToString();
                    info.StartDate = Convert.ToDateTime(_dv[_idx]["StartDate"].ToString());
                    info.EndDate = Convert.ToDateTime(_dv[_idx]["EndDate"].ToString());
                    int startHour = Convert.ToDateTime(_dv[_idx]["StartDate"].ToString()).Hour;
                    int endHour = Convert.ToDateTime(_dv[_idx]["EndDate"].ToString()).Hour;
                    info.StartTimeGap = ((startHour - 6) > 13) ? 14 : (startHour - 6);
                    info.EndTimeGap = ((20 - endHour) > 13) ? 14 : (21 - endHour);
                    info.StartMinute = Convert.ToDateTime(_dv[_idx]["StartDate"].ToString()).Minute;
                    info.EndMinute = Convert.ToDateTime(_dv[_idx]["EndDate"].ToString()).Minute;
                    info.StartHour = startHour;
                    info.EndHour = (info.EndMinute > 0) ? endHour - 1 : endHour - 2;

                    int gap = (_dv[_idx]["DifferMinite"].ToString().ToNumber<int>() / 30);

                    if ((startHour < 7) && (endHour <= 19))
                    {
                        int startgap = 7 - startHour;
                        gap = gap - (startgap * 2);

                        if (info.StartMinute > 0)
                        {
                            gap = gap + 1;
                        }

                        if (info.EndMinute > 0)
                        {
                            gap = gap - 1;
                        }
                        info.DifferMinite = gap / 2;
                    }
                    else if ((startHour >= 7) && (endHour > 19))
                    {
                        int endgap = endHour - 20;
                        gap = gap - (endgap * 2);
                        if (info.StartMinute > 0)
                        {
                            gap = gap - 1;
                        }
                        info.DifferMinite = gap / 2;
                    }
                    else if ((startHour >= 7) && (endHour <= 19))
                    {
                        info.DifferMinite = gap / 2;
                        if (info.StartMinute > 0)
                        {
                            if (info.EndMinute > 0)
                            {
                                info.DifferMinite = (gap / 2) - 1;
                            }
                        }
                    }
                    else
                    {
                        info.DifferMinite = 13;
                    }


                    if (startHour < 7)
                    {
                        info.StartMinute = 0;
                        info.StartTimeGap = 0;
                    }

                    if (endHour > 19)
                    {
                        if ((endHour == 20) && (info.EndMinute == 0))
                        {

                        }
                        else
                        {
                            info.EndMinute = 0;
                            info.EndTimeGap = 0;
                        }
                    }

                    if (endHour < 7)
                    {
                        info.EndMinute = 0;
                        info.DifferMinite = 0;
                        info.EndHour = 5;
                    }

                    if (startHour > 19)
                    {
                        info.StartMinute = 0;
                    }


                    if (info.EndMinute > 0)
                    {
                        if (info.EndHour != 19)
                        {
                            info.EndTimeGap = info.EndTimeGap - 1;
                        }
                    }

                    // 다른날짜까지 일정이 이어져 있을 경우
                    DateTime atmpdate = Convert.ToDateTime(TmpDate);
                    if (CheckTime(atmpdate, info.EndDate))
                    {
                        if (CheckTime3(atmpdate, info.StartDate))
                        {
                            info.DifferMinite = 13;
                            info.StartTimeGap = 0;
                            info.StartMinute = 0;
                            info.EndTimeGap = 0;
                            info.EndMinute = 0;
                        }
                        else
                        {
                            if (Convert.ToDateTime(_dv[_idx]["StartDate"].ToString()).Minute > 0)
                            {
                                info.DifferMinite = 19 - startHour;
                            }
                            else
                            {
                                info.DifferMinite = 20 - startHour;
                            }
                            info.EndTimeGap = 0;
                            info.EndMinute = 0;
                        }
                    }
                    else
                    {
                        // 이전날짜까지 일정이 이어져 있을 경우 
                        if (CheckTime3(atmpdate, info.StartDate))
                        {
                            info.DifferMinite = endHour - 7;
                            info.StartTimeGap = 0;
                            info.StartMinute = 0;
                        }
                    }

                    info.DifferMinite = (info.DifferMinite > 13) ? 13 : info.DifferMinite;

                    info.Usable = _dv[_idx]["Usable"].ToString();
                    info.WriteUserID = _dv[_idx]["WriteUserID"].ToString().ToNumber<int>();

                    //예약자 이름
                    info.WriteUserName = this.GetLangName(_dv[_idx]["NameBase"].ToString(), _dv[_idx]["NameKo"].ToString(),
                                                        _dv[_idx]["NameEn"].ToString(), _dv[_idx]["NameJa"].ToString(), _dv[_idx]["NameFr"].ToString(),
                                                        _dv[_idx]["NameZhcn"].ToString(), _dv[_idx]["NameZhtw"].ToString(), PreLang);

                    info.SubjectName = info.Subject + ": " + info.WriteUserName;

                    info.ManagerUserID = _dv[_idx]["ManagerUserID"].ToString().ToNumber<int>();
                    info.Title = _dv[_idx]["Title"].ToString();

                    if (_dv[_idx]["Authority"].ToString() == "1")
                    {
                        /*
                        // 관리자 권한 
                        bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
                        bool isRoleRM = CurrentUserInfo.IsInRole("resource");//resource admin

                        if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleRM)
                        {
                            info.Admin = "Y";
                        }
                        else
                        {
                        */
                            if (info.ManagerUserID == CurrentUserInfo.UserID)
                            {
                                info.Admin = "Y";
                            }
                            else
                            {
                                info.Admin = "N";
                            }
                        //}
                    }
                    else
                    {
                        info.Admin = "N";
                    }
                }
                else
                {
                    info.Subject = GetResource("NoResource");
                    info.StartTimeGap = 0;
                    info.EndTimeGap = 0;
                    info.StartMinute = 0;
                    info.EndMinute = 0;
                    info.DifferMinite = 13;
                    info.Usable = "3";
                    info.WriteUserID = 0;
                    info.WriteUserName = String.Empty;
                    info.Title = _dv[_idx]["Title"].ToString();
                    info.ManagerUserID = _dv[_idx]["ManagerUserID"].ToString().ToNumber<int>();

                    //자원 담당자 이름
                    string[] arrManyName = _dv[_idx]["MManyName"].ToString().Split('|');
                    string tmpManagerName = "";
                    if (arrManyName.Length == 7)
                    {
                        tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                               arrManyName[2], arrManyName[3], arrManyName[4],
                                                               arrManyName[5], arrManyName[6], PreLang);
                    }

                    info.ManagerName = resMng.GetString("ResourcesUser") + " : " + tmpManagerName;
                    info.Admin = "N";
                }

                #endregion
            }

            NextDayGap = info.EndTimeGap;
            BeforeDayGap = info.StartTimeGap;

            return info;
        }

        // <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 요일 정보 가져오기
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string WeekDayName(DateTime _date)
        {
            string rtnVal = "";
            switch (_date.DayOfWeek)
            {
                case DayOfWeek.Sunday: rtnVal = "(" + GetResource("Sun") + ")"; break;
                case DayOfWeek.Monday: rtnVal = "(" + GetResource("Mon") + ")"; break;
                case DayOfWeek.Tuesday: rtnVal = "(" + GetResource("Tue") + ")"; break;
                case DayOfWeek.Wednesday: rtnVal = "(" + GetResource("Wed") + ")"; break;
                case DayOfWeek.Thursday: rtnVal = "(" + GetResource("Thu") + ")"; break;
                case DayOfWeek.Friday: rtnVal = "(" + GetResource("Fri") + ")"; break;
                case DayOfWeek.Saturday: rtnVal = "(" + GetResource("Sat") + ")"; break;
            };

            return rtnVal;
        }

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

        private bool CheckTime(DateTime _tmpDate, DateTime _eDate)
        {
            bool rtn = false;

            string tmpDate = _tmpDate.Year + _tmpDate.Month.ToString().PadLeft(2, '0') + _tmpDate.Day.ToString().PadLeft(2, '0');
            string eDate = _eDate.Year + _eDate.Month.ToString().PadLeft(2, '0') + _eDate.Day.ToString().PadLeft(2, '0');

            if (eDate.ToNumber<int>() > tmpDate.ToNumber<int>())
            {
                rtn = true;
            }

            return rtn;
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 상태별 HTML 그리기
        /// </summary>
        /// <returns></returns>
        private string GetStatusHtml(DataView _dv, DateTime _nowDate, int _hour, int _managerUserID)
        {
            StringBuilder sb = new StringBuilder();

            BE.ReserveInfo info = new BE.ReserveInfo();
            string widthTag = string.Empty;
            string className = "";
            string styleTag = string.Empty;
            string title = string.Empty;
            string beforeTitle = string.Empty;
            string patternImg = string.Empty;
            string reservationID = string.Empty;
            string conMenu = "";

            string isGuard = string.Empty;
            string writeUserID = string.Empty;

            //int reservationID = 0;
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

                    // 패턴(이미지)
                    patternImg = "#" + _dv[i]["UserImg"].ToString().ToNull();

                    if (String.IsNullOrEmpty(_dv[i]["UserImg"].ToString().ToNull()))
                    {
                        patternImg = "#FF0000";
                    }

                    if (_dv[i]["ParentReservationID"].ToString().ToNumber<int>() > 0)
                    {
                        sDate = Convert.ToDateTime(_dv[i]["StartDate"].ToString());
                        eDate = Convert.ToDateTime(_dv[i]["EndDate"].ToString());
                    }

                    #region 분단위 넓이 조절

                    if (IsWorkTime(_nowDate, sDate, eDate))
                    {
                        info = GetInfo(_dv, i);

                        check = true;
                        title = info.SubjectName;

                        #region className 설정
                        switch (info.Usable)
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
                            reservationID += info.ReservationID + "^";

                            if (beforeMinute == sDate.Minute)
                            {
                                BeforeTag += "1^";
                            }
                            else
                            {
                                beforeMinute = sDate.Minute;
                                BeforeTag += "0^";
                            }

                            isGuard += info.IsGuard + "^";
                            writeUserID += info.WriteUserID + "^";
                        }
                        else
                        {
                            beforeHour = _nowDate.Hour;
                            beforeMinute = sDate.Minute;
                            BeforeDayTag = widthTag + "^";
                            beforeTitle = title + "^";
                            reservationID = info.ReservationID.ToString() + "^";
                            BeforeTag = "0^";

                            isGuard += info.IsGuard + "^";
                            writeUserID += info.WriteUserID + "^";
                        }
                    }

                    #endregion
                }
            }

            // 예약 시간에 해당하는 막대 그리기
            if (check == true)
            {
                if (info.Admin == "Y")
                {
                    #region 관리자승인Y

                    // 같은 시간에30분 단위로 예약이 되었을 경우..
                    if (itemStep > 0)
                    {
                        string[] ArrReservationID = reservationID.Split('^');
                        string[] ArrBeforeDayTag = BeforeDayTag.Split('^');
                        string[] ArrBeforeTitle = beforeTitle.Split('^');
                        string[] ArrBeforeTag = BeforeTag.Split('^');

                        string[] ArrIsGuard = isGuard.Split('^');
                        string[] ArrWriteUserID = writeUserID.Split('^');
                        

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

                            if (tmpAuth > 0)
                            {
                                sb.Append("<a class='" + conMenu +"' id='pu_" + ArrReservationID[i] + "'>");
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
                        if (info.IsGuard == "1")
                        {
                            //해당 일정의 권한체크
                            tmpAuth = CheckAuth(info.ReservationID, info.WriteUserID, _managerUserID);
                        }

                        if (tmpAuth > 0)
                        {
                            sb.Append("<a class='" + conMenu + "' id='pu_" + info.ReservationID + "'>");
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
                        string[] ArrReservationID = reservationID.Split('^');
                        string[] ArrBeforeDayTag = BeforeDayTag.Split('^');
                        string[] ArrBeforeTitle = beforeTitle.Split('^');
                        string[] ArrBeforeTag = BeforeTag.Split('^');

                        string[] ArrIsGuard = isGuard.Split('^');
                        string[] ArrWriteUserID = writeUserID.Split('^');
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
                                sb.Append("<a onclick=\"javascript:WholeDay.View('" + ArrReservationID[i] + "')\">");
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
                        if (info.IsGuard == "1")
                        {
                            //해당 일정의 권한체크
                            tmpAuth = CheckAuth(info.ReservationID, info.WriteUserID, _managerUserID);
                        }

                        if (widthTag.IndexOf("float:right") > 0 && widthTag.IndexOf("width:50%") > 0 && itemStep == 0)
                        {
                            if (tmpAuth > 0)
                            {
                                sb.Append("<a onclick=\"javascript:WholeDay.Add('" + (_hour + 5) + " ', '" + ListID + "')\"><span style=\"float:left; width:50%\"class='dataBlank'></span></a>");
                                sb.Append("<a onclick=\"javascript:WholeDay.View('" + info.ReservationID + "')\">");
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + title + "'></span></a></td>");
                            }
                            else
                            {
                                sb.Append("<a onclick=\"javascript:WholeDay.Add('" + (_hour + 5) + " ', '" + ListID + "')\"><span style=\"float:left; width:50%\"class='dataBlank'></span></a>");
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + GetResource("SecurityReservation") + "'></span></td>");
                            }
                            
                        }
                        else if (widthTag.IndexOf("float:left") > 0 && widthTag.IndexOf("width:50%") > 0 && itemStep == 0)
                        {
                            if (tmpAuth > 0)
                            {
                                sb.Append("<a onclick=\"javascript:WholeDay.View('" + info.ReservationID + "')\">");
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + title + "'></span></a>");
                                sb.Append("<a onclick=\"javascript:WholeDay.Add('" + (_hour + 5) + " ', '" + ListID + "')\"><span style=\"float:right; width:50%\" class='dataBlank'></span></a></td>");
                            }
                            else
                            {
                                sb.Append("<span class='" + className + "' " + widthTag + " title='" + GetResource("SecurityReservation") + "'></span>");
                                sb.Append("<a onclick=\"javascript:WholeDay.Add('" + (_hour + 5) + " ', '" + ListID + "')\"><span style=\"float:right; width:50%\" class='dataBlank'></span></a></td>");
                            }
                            
                        }
                        else
                        {
                            if(tmpAuth > 0)
                            {
                                sb.Append("<a onclick=\"javascript:WholeDay.View('" + info.ReservationID + "')\">");
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
                sb.Append("<a onclick=\"javascript:WholeDay.Add('" + (_hour + 5) + " ', '" + ListID + "')\"><span class='dataBlank'></span></a>");
            }

            return sb.ToString();

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

        #endregion
    }
}