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
    public partial class AdminStatusD : Base.BasePageETC
    {
        #region 변수설정
        public int UserID = 0;
        public int DeptID = 0;  
        public string StartDate = string.Empty;     //기준일자
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Attendance.AdminStatusD).Assembly;	//어셈블리 선언
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

            #region PageSize 설정하기

            DAL.FolderUser dalPageSize = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Attendance", CurrentUserInfo.UserID, "AdminStatusD");

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            #endregion

            ddlListCount.SelectedValue = RecordPerPage;

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            GetDateType();

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
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "ManyDeptName"));      //부서명
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            StartDate = GetDecryptParam("startDate", "");                           //시작일자
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");

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
            DAL.AttendanceAdminStatus dal = new DAL.AttendanceAdminStatus();
            TotalCount = dal.GetStatusDListCount(CurrentUserInfo.CompanyID, UserID, DeptID, StartDate, SearchField, SearchValue).ToString();
        }

        /// <summary>  
        /// 작성자 : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 날짜설정
        /// </summary>
        private void GetDateType()
        {
            StartDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + System.DateTime.Now.Day.ToString().PadLeft(2, '0');
        }
        #endregion

        #region (프로퍼티)
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

            Zeospace.DeskPlus.ETC.Web.Ws.Attendance.AdminStatus ws = new Ws.Attendance.AdminStatus();
            List<object> list = ws.GetStatusDList(hdexcel.Value.ToString());
            this.GetResourceManager();

            sbData.Append("<tr>");
            sbData.Append("<td align=center colspan=10>" + GetResource("Attendance_Department") + GetResource("Attendance_CurrentStatus") + "(" + sd.Value.ToString() + ")</td>");
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center>" + GetResource("Attendance_Department") + "</td>");                                    //부서
            sbData.Append("<td align=center>" + GetResource("ListType0") + GetResource("Attendance_personnel") + "</td>");          //전체인원
            sbData.Append("<td align=center>" + GetResource("HoliType1") + "</td>");                                                //연차
            sbData.Append("<td align=center>" + GetResource("Attendance_Today") + GetResource("Attendance_personnel") + "</td>");   //금일인원
            sbData.Append("<td align=center>" + GetResource("Gubun") + "</td>");                                                    //구분
            sbData.Append("<td align=center>" + GetResource("Attendance_normal") + GetResource("Attendance_Start") + "</td>");      //정상출근
            sbData.Append("<td align=center>" + GetResource("Attendance_Tardy") + "</td>");                                         //지각
            sbData.Append("<td align=center>" + GetResource("Attendance_End") + "</td>");                                           //퇴근
            sbData.Append("<td align=center>" + GetResource("Attendance_absenteeism") + "</td>");                                   //결근
            sbData.Append("<td align=center>" + GetResource("Attendance_Now") + GetResource("Attendance_personnel") + "</td>");     //현재인원
            sbData.Append("</tr>");

            foreach (BE.AttendanceAdminStatusInfo _data in list)
            {
                sbData.Append("<tr>");
                sbData.Append("<td rowspan=5>" + _data.DeptName + "</td>");
                sbData.Append("<td rowspan=5>" + _data.DeptCount + "</td>");
                sbData.Append("<td rowspan=5>" + _data.HoliCount + "<br>" + _data.HoliPer + " % </td>");
                sbData.Append("<td rowspan=5>" + _data.TodayCount + "<br>" + _data.TodayPer + " % </td>");
                sbData.Append("<td>" + GetResource("Attendance_personnel") + "</td>");
                sbData.Append("<td>" + _data.WSTimeCount + "</td>");
                sbData.Append("<td>" + _data.WSTimeCountLate + "</td>");
                sbData.Append("<td>" + _data.WCTimeCount + "</td>");
                sbData.Append("<td rowspan=5>" + _data.AbsenteeismCount + "<br>" + _data.AbsenteeismPer + " % </td>");
                sbData.Append("<td rowspan=5>" + _data.NowCount + "<br>" + _data.NowPer + " % </td>");
                sbData.Append("</tr>");

                sbData.Append("<tr>");
                sbData.Append("<td>" + GetResource("Attendance_percent") + "</td>");
                sbData.Append("<td>" + _data.WSTimePer + " % </td>");
                sbData.Append("<td>" + _data.WSTimePerLate + " % </td>");
                sbData.Append("<td>" + _data.WCTimePer + " % </td>");
                sbData.Append("</tr>");

                sbData.Append("<tr>");
                sbData.Append("<td>" + GetResource("Attendance_min") + "</td>");
                sbData.Append("<td>" + _data.WSTimeMIN + "</td>");
                sbData.Append("<td>" + _data.WSTimeMINLate + "</td>");
                sbData.Append("<td>" + _data.WCTimeMIN + "</td>");
                sbData.Append("</tr>");

                sbData.Append("<tr>");
                sbData.Append("<td>" + GetResource("Attendance_max") + "</td>");
                sbData.Append("<td>" + _data.WSTimeMAX + "</td>");
                sbData.Append("<td>" + _data.WSTimeMAXLate + "</td>");
                sbData.Append("<td>" + _data.WCTimeMAX + "</td>");
                sbData.Append("</tr>");

                sbData.Append("<tr>");
                sbData.Append("<td>" + GetResource("Attendance_avg") + "</td>");
                sbData.Append("<td>" + _data.WSTimeAVG + "</td>");
                sbData.Append("<td>" + _data.WSTimeAVGLate + "</td>");
                sbData.Append("<td>" + _data.WCTimeAVG + "</td>");
                sbData.Append("</tr>");
            }

            string strFileName = GetResource("Attendance_CurrentStatus") + "_" + sd.Value.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
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