using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.HR.Web.Content.Popup
{
    public partial class PrivateMod : Base.BasePageHR
    {
        #region 변수설정
        public int AttID = 0;
        public int UserID = 0;
        public string TargetUserID = string.Empty;
        public string TargetDeptID = string.Empty;
        public string TargetUserName = string.Empty;
        public string TargetDeptName = string.Empty;
        public string TargetTitleName = string.Empty;
        public string PathID = string.Empty;
        public string DayWeek = string.Empty;
        public string WorkDate = string.Empty;
        public string IsHoli = string.Empty;

        public string bWSTimeHour = "0"; //정규 출근시간
        public string bWSTimeMinute = "00"; //정규 출근시간
        public string bWCTimeHour = "0"; //정규 퇴근시간
        public string bWCTimeMinute = "00"; //정규 퇴근시간
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
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.Popup.PrivateMod).Assembly;	    //어셈블리 선언
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
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 권한 체크
            if (!AccessAuth && !CurrentUserInfo.IsInRole("HRAttendance"))
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }
            #endregion

            LoadParam();

            GetHeaderInfo();

            SetWSTime();

            if (AttID != 0)
            {
                GetData(AttID);
            }

            GetAttendanceTime(TargetUserID, WorkDate, IsHoli);
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 언어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = this.resMng.GetString("Btn_Save"); // 저장
                cbtnClose.Text = this.resMng.GetString("BtnClose"); // 닫기
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            AttID = GetDecryptParam("attID", "0").ToNumber<int>();
            TargetUserID = GetDecryptParam("userID", "0");
            TargetDeptID = GetDecryptParam("deptID", "0");
            TargetUserName = GetDecryptParam("userName", "");
            TargetDeptName = GetDecryptParam("deptName", "");
            TargetTitleName = GetDecryptParam("titleName", "");
            PathID = GetDecryptParam("pathID", "0");
            DayWeek = GetDecryptParam("dayWeek", "");
            WorkDate = GetDecryptParam("workDate", "");
            IsHoli = GetDecryptParam("isHoli", "0");

            UserID = CurrentUserInfo.UserID;
        }

        private void GetHeaderInfo()
        {
            /*
            string DeptName = string.Empty;
            string TitleName = string.Empty;
            string Name = string.Empty;

            Zeospace.DeskPlus.Account.DAL.User uDal = new Zeospace.DeskPlus.Account.DAL.User();
            List<Zeospace.DeskPlus.Account.BE.UserInfo> ulist = uDal.GetUserInfo(CurrentUserInfo.CompanyID, TargetUserID.ToNumber<int>(), TargetDeptID.ToNumber<int>());
            foreach (Zeospace.DeskPlus.Account.BE.UserInfo info in ulist)
            {
                DeptName = GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang);
                TitleName = GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang);
                Name = GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhCn, info.NameZhTw, PreLang);
            }
            */

            ltlHeader.Text = WorkDate + " " + DayWeek + " - " + TargetDeptName + " " + TargetTitleName + " " + TargetUserName;
        }

        private void GetData(int _attID)
        {
            DAL.Attendance dal = new DAL.Attendance();

            DataTableReader dtr = dal.GetAttendanceByID(CurrentUserInfo.CompanyID, _attID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (dtr["WSTime"].ToString() != "")
                        {
                            int WSTimeHour = Convert.ToDateTime(dtr["WSTime"].ToString()).Hour;
                            ddlWSTimeHour.SelectedValue = (WSTimeHour < 10) ? "0" + WSTimeHour.ToString() : WSTimeHour.ToString();

                            int WSTimeMinute = Convert.ToDateTime(dtr["WSTime"].ToString()).Minute;
                            ddlWSTimeMinute.SelectedValue = (WSTimeMinute < 10) ? "0" + WSTimeMinute.ToString() : WSTimeMinute.ToString();
                        }

                        if (dtr["WCTime"].ToString() != "")
                        {
                            int WCTimeHour = Convert.ToDateTime(dtr["WCTime"].ToString()).Hour;
                            ddlWCTimeHour.SelectedValue = (WCTimeHour < 10) ? "0" + WCTimeHour.ToString() : WCTimeHour.ToString();

                            int WCTimeMinute = Convert.ToDateTime(dtr["WCTime"].ToString()).Minute;
                            ddlWCTimeMinute.SelectedValue = (WCTimeMinute < 10) ? "0" + WCTimeMinute.ToString() : WCTimeMinute.ToString();
                        }

                        ddlWSConn.SelectedValue = dtr["WSConn"].ToString();
                        ddlWCConn.SelectedValue = dtr["WCConn"].ToString();
                        txtLateRemark.Value = dtr["LateRemark"].ToString().ToNull().ToReverseXSS();
                        txtEarlyRemark.Value = dtr["EarlyRemark"].ToString().ToNull().ToReverseXSS();
                    }
                }
            }
        }

        private void GetAttendanceTime(string _targetUserID, string _workDate, string _isHoli)
        {
            // 오늘 정규 출근시간 가져오기
            DAL.Attendance dal = new DAL.Attendance();

            DataTableReader dtrT = dal.GetAttendanceTime(CurrentUserInfo.CompanyID, _targetUserID.ToNumber<int>(), _workDate, _isHoli);
            if (dtrT != null)
            {
                if (dtrT.HasRows)
                {
                    while (dtrT.Read())
                    {
                        if (dtrT["bWSTime"].ToString() != "") //출근시간
                        {
                            DateTime bWSTime = Convert.ToDateTime(dtrT["bWSTime"]);
                            bWSTimeHour = bWSTime.Hour.ToString();
                            bWSTimeMinute = (bWSTime.Minute < 10) ? "0" + bWSTime.Minute.ToString() : bWSTime.Minute.ToString();
                        }

                        if (dtrT["bWCTime"].ToString() != "") //퇴근시간
                        {
                            DateTime bWCTime = Convert.ToDateTime(dtrT["bWCTime"]);
                            bWCTimeHour = bWCTime.Hour.ToString();
                            bWCTimeMinute = (bWCTime.Minute < 10) ? "0" + bWCTime.Minute.ToString() : bWCTime.Minute.ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 출퇴근 시간 범위 설정
        /// </summary>
        private void SetWSTime()
        {
            ddlWSTimeHour.Items.Clear();
            ddlWCTimeHour.Items.Clear();

            ddlWSTimeHour.Items.Add(new ListItem(this.resMng.GetString("Select"), ""));
            ddlWCTimeHour.Items.Add(new ListItem(this.resMng.GetString("Select"), ""));

            string hour = "";
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    hour = "0" + i.ToString();
                }
                else
                {
                    hour = i.ToString();
                }

                ddlWSTimeHour.Items.Add(new ListItem(hour, hour));
                ddlWCTimeHour.Items.Add(new ListItem(hour, hour));
            }

            ddlWSTimeMinute.Items.Clear();
            ddlWCTimeMinute.Items.Clear();

            ddlWSTimeMinute.Items.Add(new ListItem(this.resMng.GetString("Select"), ""));
            ddlWCTimeMinute.Items.Add(new ListItem(this.resMng.GetString("Select"), ""));

            string minute = "";
            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                {
                    minute = "0" + i.ToString();
                }
                else
                {
                    minute = i.ToString();
                }

                ddlWSTimeMinute.Items.Add(new ListItem(minute, minute));
                ddlWCTimeMinute.Items.Add(new ListItem(minute, minute));
            }

            ddlWSConn.Items.Clear();
            ddlWSConn.Items.Add(new ListItem(this.resMng.GetString("Select"), ""));
            ddlWSConn.Items.Add(new ListItem(this.resMng.GetString("IN"), "IN"));
            ddlWSConn.Items.Add(new ListItem(this.resMng.GetString("OUT"), "OUT"));

            ddlWCConn.Items.Clear();
            ddlWCConn.Items.Add(new ListItem(this.resMng.GetString("Select"), ""));
            ddlWCConn.Items.Add(new ListItem(this.resMng.GetString("IN"), "IN"));
            ddlWCConn.Items.Add(new ListItem(this.resMng.GetString("OUT"), "OUT"));

        }

        #endregion

        #region (프로퍼티)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}