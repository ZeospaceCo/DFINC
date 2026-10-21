using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.ETC.Web.Attendance
{
    public partial class UserStatusM : Base.BasePageETC
    {
        #region 변수설정
        public int UserID = 0;
        public int DeptID = 0; 
        public string StatusDate = "";           //일자
        public string StatusYear = "";           //년
        public string StatusMonth = "";          //월
        public string WorkType = string.Empty;          //정규직구분

        public string SearchField = string.Empty;   //검색키워드
        public string SearchValue = string.Empty;   //검색어       

        public int CurrentPage = 0;
        public string TotalCount = string.Empty;    //총카운트
        public string RecordPerPage = string.Empty; // 리스트 개수
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Attendance.UserStatusM).Assembly;	//어셈블리 선언
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
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            ddlWorkType.SelectedValue = WorkType;

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Attendance", CurrentUserInfo.UserID, "UserStatusM");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage;

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            GetYearMonth();

            if (StatusYear != "")
            {
                ddlYear.SelectedValue = StatusYear;           //년코드 설정
            }
            else
            {
                StatusYear = ddlYear.SelectedValue.ToString();
            }

            if (StatusMonth != "")
            {
                ddlMonth.SelectedValue = StatusMonth;          //년코드 설정
            }
            else
            {
                StatusMonth = ddlMonth.SelectedValue.ToString();
            }

            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 언어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = this.resMng.GetString("Administrator"); // 관리자
                cbtnExcel.Text = resMng.GetString("ExecelDown");        //엑셀
                cbtnSearch.Text = this.resMng.GetString("Search");      //검색                

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "ManyName"));          //성명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID"));            //로그인ID
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "ManyDeptName"));      //부서명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TitleName"), "ManyTitleName"));    //직급명

                ddlWorkType.Items.Clear();
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("ListType0"), "0"));                //전체
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("Work_1"), "1"));                   //정규직
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("Work_2"), "2"));                   //비정규직
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");

            StatusDate = GetYearMonthDay();                                          //현재일자                                                                
            StatusYear = GetDecryptParam("statusYear", StatusDate.Substring(0, 4));    //년
            StatusMonth = GetDecryptParam("statusMonth", StatusDate.Substring(5, 2));  //월
            WorkType = GetDecryptParam("workType", "0");                            //정규직구분

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");                   //administrator
            bool isRoleAttendanceDept = CurrentUserInfo.IsInRole("attendanceDept");     //근태부서관리자

            cbtnAdmin.Visible = false;

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin)
            {
                cbtnAdmin.Visible = true;
            }
            else
            {
                if (isRoleAttendanceDept)
                {
                    DeptID = CurrentUserInfo.DeptID;
                    UserID = CurrentUserInfo.UserID;
                }
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.AttendanceUserStatus dal = new DAL.AttendanceUserStatus();
            TotalCount = dal.GetStatusMListCount(CurrentUserInfo.CompanyID, UserID, DeptID, StatusYear, StatusMonth, SearchField, SearchValue, WorkType).ToString();
        }

        /// <summary>  
        /// 작성자 : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 날짜설정
        /// </summary>
        private void GetYearMonth()
        {
            int startYear = StatusYear.ToString().ToNumber<int>() - 5;
            int endYear = StatusYear.ToString().ToNumber<int>() + 5;

            //년 설정   
            this.ddlYear.Items.Clear();
            ddlYear.AppendDataBoundItems = true;
            for (int i = startYear; i < endYear; i++)
            {
                if (i.ToString().Length == 1)
                {
                    ddlYear.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }

            //월 설정   
            this.ddlMonth.Items.Clear();
            ddlMonth.AppendDataBoundItems = true;
            for (int i = 1; i < 13; i++)
            {
                if (i.ToString().Length == 1)
                {
                    ddlMonth.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    ddlMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
        }

        /// <summary>
        /// 작성목적   : 현재 년월일 가져오기 [2012-01-01]
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-11-24
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
        #endregion

        #region (프로퍼티)
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

        #region (이벤트핸들러)
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 엑셀 다운로드
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        protected void lbtnexcel_Click(object sender, EventArgs e)
        {

            StringBuilder sbData = new StringBuilder();

            Zeospace.DeskPlus.ETC.Web.Ws.Attendance.UserStatus ws = new Ws.Attendance.UserStatus();
            List<object> list = ws.GetStatusMList(hdexcel.Value.ToString());
            this.GetResourceManager();

            sbData.Append("<tr>");
            sbData.Append("<td align=center colspan=7>" + GetResource("Attendance_individual") + GetResource("Attendance_CurrentStatus") + "(" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString() + ")</td>");
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center colspan=2>" + GetResource("Attendance_Userinformation") + "</td>");                             //사용자정보
            sbData.Append("<td align=center rowspan=2>" + GetResource("Gubun") + "</td>");                                                  //구분
            sbData.Append("<td align=center colspan=4>" + ddlMonth.SelectedValue.ToString() + GetResource("Attendance_month") + "</td>");   //월
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center>" + GetResource("Attendance_Department") + "</td>");                                //부서            
            sbData.Append("<td align=center>" + GetResource("Attendance_user") + "</td>");                                      //사용자
            sbData.Append("<td align=center>" + GetResource("Attendance_Start") + "</td>");                                     //출근
            sbData.Append("<td align=center>" + GetResource("Attendance_normal") + GetResource("Attendance_Start") + "</td>");  //정상출근
            sbData.Append("<td align=center>" + GetResource("Attendance_Tardy") + "</td>");                                     //지각
            sbData.Append("<td align=center>" + GetResource("Attendance_End") + "</td>");                                       //퇴근
            sbData.Append("</tr>");

            foreach (BE.AttendanceUserStatusInfo _data in list)
            {
                sbData.Append("<tr>");
                sbData.Append("<td rowspan=5>" + _data.DeptName + "</td>");
                sbData.Append("<td rowspan=5>" + _data.UserName + "</td>");
                sbData.Append("<td>" + GetResource("Holi_Days") + "</td>");
                sbData.Append("<td>" + _data.TodayCount + "</td>");
                sbData.Append("<td>" + _data.WSTimeCount + "</td>");
                sbData.Append("<td>" + _data.WSTimeCountLate + "</td>");
                sbData.Append("<td>" + _data.WCTimeCount + "</td>");
                sbData.Append("</tr>");

                sbData.Append("<tr>");
                sbData.Append("<td>" + GetResource("Attendance_percent") + "</td>");
                sbData.Append("<td></td>");
                sbData.Append("<td>" + _data.WSTimePer + " % </td>");
                sbData.Append("<td>" + _data.WSTimePerLate + " % </td>");
                sbData.Append("<td>" + _data.WCTimePer + " % </td>");
                sbData.Append("</tr>");

                sbData.Append("<tr>");
                sbData.Append("<td>" + GetResource("Attendance_min") + "</td>");
                sbData.Append("<td>" + _data.TodayMIN + "</td>");
                sbData.Append("<td>" + _data.WSTimeMIN + "</td>");
                sbData.Append("<td>" + _data.WSTimeMINLate + "</td>");
                sbData.Append("<td>" + _data.WCTimeMIN + "</td>");
                sbData.Append("</tr>");

                sbData.Append("<tr>");
                sbData.Append("<td>" + GetResource("Attendance_max") + "</td>");
 
                sbData.Append("<td>" + _data.TodayMAX + "</td>");
                sbData.Append("<td>" + _data.WSTimeMAX + "</td>");
                sbData.Append("<td>" + _data.WSTimeMAXLate + "</td>");
                sbData.Append("<td>" + _data.WCTimeMAX + "</td>");
                sbData.Append("</tr>");

                sbData.Append("<tr>");
                sbData.Append("<td>" + GetResource("Attendance_avg") + "</td>");
                sbData.Append("<td>" + _data.TodayAVG + "</td>");
                sbData.Append("<td>" + _data.WSTimeAVG + "</td>");
                sbData.Append("<td>" + _data.WSTimeAVGLate + "</td>");
                sbData.Append("<td>" + _data.WCTimeAVG + "</td>");
                sbData.Append("</tr>");
            }

            string strFileName = GetResource("Attendance_CurrentStatus") + "_" + ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            strFileName = Server.UrlEncode(strFileName.Replace(" ", "")).Replace("+", "%20");

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
            <html><head><title> New Document </title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /> 
            </head><body>");

            sb.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0'>");
            sb.Append(sbData);
            sb.Append("</table>");

            sb.Append("</body></html>");

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(sb.ToString());
            Response.End();
        }

        #endregion
    }
}