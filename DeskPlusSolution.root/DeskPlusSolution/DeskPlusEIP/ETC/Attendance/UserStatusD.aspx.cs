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
    public partial class UserStatusD : Base.BasePageETC
    {
        #region 변수설정
        public int UserID = 0;
        public int DeptID = 0;        
        public string StartDate = string.Empty;         //근태시작일
        public string EndDate = string.Empty;           //근태종료일
        public string WorkType = string.Empty;          //정규직구분
        public string AttendanceType = string.Empty;    //구분

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Attendance.UserStatusD).Assembly;	//어셈블리 선언
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
                "Attendance", CurrentUserInfo.UserID, "UserStatusD");

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
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("UserName"), "ManyName"));          //성명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("LoginID"), "LoginID"));            //로그인ID
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "ManyDeptName"));      //부서명
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("TitleName"), "ManyTitleName"));    //직급명

                ddlWorkType.Items.Clear();
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("ListType0"), "0"));                //전체
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("Work_1"), "1"));                   //정규직
                ddlWorkType.Items.Add(new ListItem(this.resMng.GetString("Work_2"), "2"));                   //비정규직

                ddlAttendanceType.Items.Clear();
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("ListType0"), "0"));                //전체
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_Start"), "1"));         //출근                
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_End"), "2"));           //퇴근
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_Tardy"), "3"));         //지각
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_Early"), "4"));         //조퇴
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_vacation"), "5"));      //휴가
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_StartX"), "6"));        //미출근
                ddlAttendanceType.Items.Add(new ListItem(this.resMng.GetString("Attendance_EndX"), "7"));          //미퇴근
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
            EndDate = GetDecryptParam("endDate", "");                               //종료일자
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            WorkType = GetDecryptParam("workType", "0");                            //정규직구분
            AttendanceType = GetDecryptParam("attendanceType", "0");                //구분

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
            TotalCount = dal.GetStatusDListCount(CurrentUserInfo.CompanyID, UserID, DeptID, StartDate, EndDate, SearchField, SearchValue, WorkType, AttendanceType).ToString();
        }

        /// <summary>  
        /// 작성자 : 이지원
        /// 작성일자 : 2017-11-21
        /// 작성목적 : 날짜설정
        /// </summary>
        private void GetDateType()
        {
            StartDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + "01";
            EndDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + System.DateTime.Now.Day.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-11-21
        /// 작성목적 : 다국어 날짜 세팅
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string DateFormat(string _date)
        {
            string[] arrDate = _date.Split('-');
            string ParsingDate = "";
            switch (PreLang)
            {
                case "ko":
                    //"yy-mm-dd";
                    ParsingDate = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
                    break;
                case "en":
                    //"mm/dd/yy";
                    ParsingDate = arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
                    break;
                case "en-gb":
                    //"mm/dd/yy";
                    ParsingDate = arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
                    break;
                case "ja":
                    //"yy/mm/dd";
                    ParsingDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
                    break;
                case "fr":
                    //"dd/mm/yy";
                    ParsingDate = arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0];
                    break;
                case "zh-cn":
                    //"yy/mm/dd";
                    ParsingDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
                    break;
                case "zh-tw":
                    //"yy-mm-dd";
                    ParsingDate = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
                    break;
            }

            return ParsingDate;
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

            Zeospace.DeskPlus.ETC.Web.Ws.Attendance.UserStatus ws = new Ws.Attendance.UserStatus();
            List<object> list = ws.GetStatusDList(hdexcel.Value.ToString());
            this.GetResourceManager();

            sbData.Append("<tr>");
            sbData.Append("<td align=center colspan=11>" + GetResource("Attendance_individual") + GetResource("Attendance_CurrentStatus") + "</td>");
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center colspan=2>" + GetResource("Attendance_Userinformation") + "</td>");                 //사용자정보
            sbData.Append("<td align=center rowspan=2>" + GetResource("Attendance_Date") + "</td>");                            //일자
            sbData.Append("<td align=center rowspan=2 colspan=2>" + GetResource("Attendance_Start") + "/" + GetResource("Attendance_Tardy") + "</td>");
            sbData.Append("<td align=center rowspan=2>" + GetResource("Time") + "</td>");                                       //시간
            sbData.Append("<td align=center rowspan=2 colspan=2>" + GetResource("Attendance_End") + "/" + GetResource("Attendance_Early") + "</td>");
            sbData.Append("<td align=center rowspan=2>" + GetResource("Time") + "</td>");                                       //시간
            sbData.Append("<td align=center rowspan=2>" + GetResource("Attendance_vacation") + "</td>");                        //휴가
            sbData.Append("<td align=center rowspan=2>" + GetResource("Attendance_Reason") + "</td>");                          //사유
            sbData.Append("</tr>");

            sbData.Append("<tr>");
            sbData.Append("<td align=center>" + GetResource("Attendance_Department") + "</td>");                                //부서            
            sbData.Append("<td align=center>" + GetResource("Attendance_user") + "</td>");                                      //사용자
            sbData.Append("</tr>");

            foreach (BE.AttendanceUserStatusInfo _data in list)
            {
                if (_data.ROW != "9999")
                {
                    sbData.Append("<tr>");
                    sbData.Append("<td>" + _data.DeptName + "</td>");
                    sbData.Append("<td>" + _data.UserName + "</td>");
                    sbData.Append("<td>" + _data.CalcDate + _data.DayWeek + "</td>");

                    if (_data.bHoli != "1")
                    {
                        if (_data.bWS == "1")
                        {
                            sbData.Append("<td colspan=2>○</td>");
                        }
                        else
                        {
                            if (_data.bSysHoli != "1")
                            {
                                if (_data.bLate != "1")
                                {
                                    sbData.Append("<td colspan=2>X</td>");
                                }
                                else
                                {
                                    sbData.Append("<td colspan=2>△</td>");
                                }
                            }
                            else
                            {
                                sbData.Append("<td colspan=2></td>");
                            }
                        }
                    }
                    else
                    {
                        sbData.Append("<td colspan=2></td>");
                    }

                    if (_data.bHoli != "1")
                    {
                        sbData.Append("<td>" + _data.WSTime + "</td>");
                    }
                    else
                    {
                        sbData.Append("<td></td>");
                    }

                    if (_data.bHoli != "1")
                    {
                        if (_data.bWC == "1")
                        {
                            sbData.Append("<td colspan=2>○</td>");
                        }
                        else
                        {
                            if (_data.bSysHoli != "1")
                            {
                                if (_data.bEarly != "1")
                                {
                                    sbData.Append("<td colspan=2>X</td>");
                                }
                                else
                                {
                                    sbData.Append("<td colspan=2>△</td>");
                                }
                            }
                            else
                            {
                                sbData.Append("<td colspan=2></td>");
                            }
                        }
                    }
                    else
                    {
                        sbData.Append("<td colspan=2></td>");
                    }

                    if (_data.bHoli != "1")
                    {
                        sbData.Append("<td>" + _data.WCTime + "</td>");
                    }
                    else
                    {
                        sbData.Append("<td></td>");
                    }

                    if (_data.bHoli == "1")
                    {
                        sbData.Append("<td>○</td>");
                        sbData.Append("<td>" + _data.HoliRemark + "</td>");
                    }
                    else
                    {
                        sbData.Append("<td></td>");

                        if (_data.bSysHoli == "1")
                        {
                            sbData.Append("<td>" + _data.SysHoliRemark + "</td>");
                        }
                        else
                        {
                            if (_data.bLate == "1")
                            {
                                if (_data.bEarly == "1")
                                {
                                    sbData.Append("<td>" + _data.LateRemark + "/" + _data.EarlyRemark + "</td>");
                                }
                                else
                                {
                                    sbData.Append("<td>" + _data.LateRemark + "</td>");
                                }
                            }
                            else
                            {
                                if (_data.bEarly == "1")
                                {
                                    sbData.Append("<td>" + _data.EarlyRemark + "</td>");
                                }
                                else
                                {
                                    sbData.Append("<td></td>");
                                }
                            }
                        }
                    }

                    sbData.Append("</tr>");
                }
                else
                {
                    sbData.Append("<tr>");
                    sbData.Append("<td>" + _data.DeptName + "</td>");
                    sbData.Append("<td>" + _data.UserName + "</td>");
                    sbData.Append("<td>" + _data.CalcDate + "</td>");
                    sbData.Append("<td>" + _data.bWS + "</td>");
                    sbData.Append("<td>" + _data.bLate + "</td>");
                    sbData.Append("<td>" + _data.WSTime + "</td>");
                    sbData.Append("<td>" + _data.bWC + "</td>");
                    sbData.Append("<td>" + _data.bEarly + "</td>");
                    sbData.Append("<td>" + _data.WCTime + "</td>");
                    sbData.Append("<td>" + _data.bHoli + "</td>");
                    sbData.Append("</tr>");
                }
            }

            string strFileName = GetResource("Attendance_CurrentStatus") + "_" + StartDate + "_" + EndDate + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
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