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

namespace Zeospace.DeskPlus.HR.Web.Admin.Content
{
    public partial class WorkAdmin : Base.BasePageHRAdmin
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        protected DateTime dtTargetDate;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Admin.Content.WorkAdmin).Assembly;	    //어셈블리 선언
            #endregion

            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);

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
            LoadParam();

            SetWorkYN();
            SetCecomYN();
            SetStartEndTime();
            SetWSTimeLimit();
            SetWCTimeYN();

            GetHREnv();
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
                cbtnUserAdd.Text = this.resMng.GetString("AddUser"); // 사용자 추가
                cbtnADDIP.Text = resMng.GetString("AddIP");
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            
        }

        /// <summary>
        /// 근태 사용 설정
        /// </summary>
        private void SetWorkYN()
        {
            ddlWorkYN.Items.Clear();

            ListItem tmpItemN = new ListItem(this.resMng.GetString("No"), "N");
            ddlWorkYN.Items.Add(tmpItemN);

            ListItem tmpItemY = new ListItem(this.resMng.GetString("Yes"), "Y");
            ddlWorkYN.Items.Add(tmpItemY);
        }

        /// <summary>
        /// 세콤 연동 설정
        /// </summary>
        private void SetCecomYN()
        {
            ddlCecomYN.Items.Clear();

            ListItem tmpItemN = new ListItem(this.resMng.GetString("No"), "N");
            ddlCecomYN.Items.Add(tmpItemN);

            ListItem tmpItemY = new ListItem(this.resMng.GetString("Yes"), "Y");
            ddlCecomYN.Items.Add(tmpItemY);
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 일시 시간
        /// </summary>
        private void SetStartEndTime()
        {
            ddlStartTime.Items.Clear();
            ddlEndTime.Items.Clear();
            ddlWCTimeExec.Items.Clear();

            DateTime dtFromTime = dtTargetDate;
            DateTime dtToTime = dtTargetDate.AddDays(1);

            while (dtFromTime < dtToTime)
            {
                //출근시간
                ListItem tmpItemFrom = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlStartTime.Items.Add(tmpItemFrom);

                //퇴근시간
                ListItem tmpItemTo = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlEndTime.Items.Add(tmpItemTo);

                //퇴근 자동 등록 실행 시간
                ListItem tmpItem = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlWCTimeExec.Items.Add(tmpItem);

                dtFromTime = dtFromTime.AddMinutes(30);
            }

            //ddlStartTime.SelectedIndex = 18;
            //ddlEndTime.SelectedIndex = ddlStartTime.SelectedIndex + 18;

            ddlStartTime.SelectedIndex = 0;
            ddlEndTime.SelectedIndex = 0;
        }

        /// <summary>
        /// 출근 인정 범위 설정
        /// </summary>
        private void SetWSTimeLimit()
        {
            ddlWSTimeLimit.Items.Clear();
            ddlWSTimeLimit.Items.Add(new ListItem("0" + this.resMng.GetString("Minute"), "0"));
            ddlWSTimeLimit.Items.Add(new ListItem("5" + this.resMng.GetString("Minute"), "5"));

            int mi = 10;
            for (int i = 0; i < 6; i++)
            {
                ListItem tmpItem = new ListItem(mi.ToString() + this.resMng.GetString("Minute"), mi.ToString());
                ddlWSTimeLimit.Items.Add(tmpItem);

                mi = mi + 10;
            }
        }

        /// <summary>
        /// 퇴근 자동 등록 설정
        /// </summary>
        private void SetWCTimeYN()
        {
            ddlWCTimeYN.Items.Clear();

            ListItem tmpItemN = new ListItem(this.resMng.GetString("No"), "N");
            ddlWCTimeYN.Items.Add(tmpItemN);

            ListItem tmpItemY = new ListItem(this.resMng.GetString("Yes"), "Y");
            ddlWCTimeYN.Items.Add(tmpItemY);
        }

        /// <summary>
        /// 기본근무시간 설정 가져오기
        /// </summary>
        private void GetHREnv()
        {
            DAL.WorkEnv dal = new DAL.WorkEnv();

            DataTableReader dtr = dal.GetHREnv(CurrentUserInfo.CompanyID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        DateTime dtStart = Convert.ToDateTime(dtr["WSTime"].ToString());
                        DateTime dtEnd = Convert.ToDateTime(dtr["WCTime"].ToString());
                        DateTime dtExec = Convert.ToDateTime(dtr["WCTimeExec"].ToString());

                        ddlWorkYN.SelectedValue = dtr["WorkYN"].ToString();
                        ddlStartTime.SelectedValue = dtStart.ToShortTimeString();
                        ddlEndTime.SelectedValue = dtEnd.ToShortTimeString();
                        ddlWSTimeLimit.SelectedValue = dtr["Scope"].ToString();
                        ddlWCTimeYN.SelectedValue = dtr["WCTimeYN"].ToString();
                        ddlWCTimeExec.SelectedValue = dtExec.ToShortTimeString();
                        ddlCecomYN.SelectedValue = dtr["CecomYN"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <returns></returns>
        public bool Mod()
        {
            WorkEnvInfo winfo = new WorkEnvInfo();
            winfo.CompanyID = CurrentUserInfo.CompanyID;
            winfo.WorkYN = ddlWorkYN.SelectedValue;
            winfo.WSTime = new DateTime(2000, 1, 1, (Convert.ToDateTime(ddlStartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlStartTime.SelectedValue)).Minute, 0);
            winfo.WCTime = new DateTime(2000, 1, 1, (Convert.ToDateTime(ddlEndTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlEndTime.SelectedValue)).Minute, 0);
            winfo.Scope = ddlWSTimeLimit.SelectedValue.ToNumber<int>();
            winfo.WCTimeYN = ddlWCTimeYN.SelectedValue;
            winfo.WCTimeExec = new DateTime(2000, 1, 1, (Convert.ToDateTime(ddlWCTimeExec.SelectedValue)).Hour, (Convert.ToDateTime(ddlWCTimeExec.SelectedValue)).Minute, 0);
            winfo.CecomYN = ddlCecomYN.SelectedValue;

            DAL.WorkEnv wdal = new DAL.WorkEnv();
            return wdal.Mod(winfo);
        }
        
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

        #region (이벤트핸들러)

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                switch (EventArgs.ToUpper())
                {
                    case "MOD":
                        bool rtnval = Mod();
                        if (rtnval)
                        {
                            Dictionary<string, string> parmDic = new Dictionary<string, string>();

                            //parmDic.Add("alertType", AlertMsgEnum.Alert_Success.ToString());
                            parmDic.Add("alertMsg", GetResource("GoToInputInfo"));
                            parmDic.Add("goURL", "/DeskPlusEIP/HR/Admin/Content/WorkAdmin.aspx");

                            string encParams = GetEncryptParam(Utility.GetParameterDic(parmDic));
                            Utility.Redirect(encParams);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", AlertMsgEnum.Alert_Error.ToString(), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리
            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion
    }
}