using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule
{
    public partial class Add : Base.BasePageCommunity
    {
        #region 변수설정

        public int CommunityID = 0;
        public int MenuID = 0;
        protected DateTime dtTargetDate;
        public string TmpDate = String.Empty;
        public int Grade = 0;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule.Add).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
                SettingControl();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            #region 권한 체크

            if (Grade != 2)
            {
                DAL.MenuAuth dal = new DAL.MenuAuth();
                DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, MenuID);

                int ReadAuth = 9;
                int WriteAuth = 9;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();
                            WriteAuth = dtr["WriteAuth"].ToString().ToNumber<int>();
                        }
                    }
                }

                if (Grade <= ReadAuth)
                {
                    if (Grade > WriteAuth)
                    {
                        Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                        return;
                    }
                }
                else
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }

            #endregion 권한 체크

            SetStartEndTime();

            if (TmpDate == "0")
            {
                TmpDate = GetYearMonthDay();
            }

            startDate.Value = SetDate(TmpDate);
            endDate.Value = SetDate(TmpDate);

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            tbHiddenMime_contents.Text = templateHeader + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("Add.aspx") > 0)
                {
                    strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.tbHiddenMime_contents.value;" + "<" + "/script>";

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                    NonActiveX = "F";
                }
                else
                {
                    //스크립트에서 처리한다
                    NonActiveX = "T";
                }
                */
                NonActiveX = "T";

                #region 에디터 본문 이미지 첨부 경로

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = FileHelper.GetYearMonth();
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Community/" + CommunityID + "/Schedule/Editor/" + reqTime + "/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로
            }
        }

        /// <summary>
        /// 날짜 변환
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string SetDate(string _date)
        {
            string tmpVal = String.Empty;
            DateTime dt = Convert.ToDateTime(_date);
            switch (CurrentUserInfo.PreferredLanguage)
            {
                case "ko":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "en":
                    tmpVal = dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "en-gb":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "ja":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "fr":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "zh-cn":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "zh-tw":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;
            }

            return tmpVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-06
        /// 작성목적 : 일정알람
        /// </summary>
        private void SettingControl()
        {
            //알람
            ddlnotikind.Items.Clear();
            ddlnotikind.Items.Add(new ListItem("", ""));

            string[] arAlarmType = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "ScheduleAlartType", "Messenger,Mail,SMS").Split(',');

            foreach (string s in arAlarmType)
            {
                switch (s)
                {
                    case "Messenger":
                        ddlnotikind.Items.Add(new ListItem(GetResource("MESSENGER"), "Messenger"));
                        break;

                    case "Mail":
                        ddlnotikind.Items.Add(new ListItem(GetResource("MAIL"), "Mail"));
                        break;

                    case "SMS":
                        ddlnotikind.Items.Add(new ListItem(GetResource("SMS"), "SMS"));
                        break;
                }
            }           

            //알람 시간
            ddltime.Items.Clear();

            for (int i = 1; i < 12; i++)
            {
                ddltime.Items.Add(new ListItem((i * 5).ToString() + GetResource("Minute"), (i * 5).ToString()));
            }

            ddltime.Items.Add(new ListItem(GetResource("OneClock"), "60"));
            ddltime.Items.Add(new ListItem(GetResource("TwoClock"), "120"));
            ddltime.Items.Add(new ListItem(GetResource("ThreeClock"), "180"));
            ddltime.Items.Add(new ListItem(GetResource("FourClock"), "240"));
            ddltime.Items.Add(new ListItem(GetResource("FiveClock"), "300"));
            ddltime.Items.Add(new ListItem(GetResource("SixClock"), "360"));
            ddltime.Items.Add(new ListItem(GetResource("OneDay"), "1440"));
            ddltime.Items.Add(new ListItem(GetResource("TwoDay"), "2880"));
            ddltime.Items.Add(new ListItem(GetResource("ThreeDay"), "4320"));
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");       //"확인";
                cbtnCancel.Text = resMng.GetString("Cancel");         //"취소";
                cbtnRepeat.Text = resMng.GetString("Setting");        //"설정";
                cbtnRepCancel.Text = resMng.GetString("Cancel");        //"취소";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 일시 시간
        /// </summary>
        private void SetStartEndTime()
        {
            ddlstartTime.Items.Clear();
            ddlendTime.Items.Clear();

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
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 저장
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {
            int rtnVal = 0;
            BE.ScheduleInfo info = new BE.ScheduleInfo();
            BLL.Schedule bll = new BLL.Schedule();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CommunityID = CommunityID;
            info.MenuID = MenuID;
            info.Title = ctbName.Text.Trim();
            info.Content = GetContents();

            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteUserName = CurrentUserInfo.Name;
            info.WriteDate = System.DateTime.Now;

            info.ModifyUserID = CurrentUserInfo.UserID;
            info.ModifyUserName = CurrentUserInfo.Name;
            info.ModifyDate = System.DateTime.Now;

            info.PreLang = PreLang;
            info.SMSLoginID = CurrentUserInfo.LogInID;

            if (!ddlnotikind.SelectedValue.ToString().Equals(""))
            {
                info.Alert = "Y";
                info.AlertType = ddlnotikind.SelectedValue.ToString();
                info.AlertTime = int.Parse(ddltime.SelectedValue.ToString());
            }
            else
            {
                info.Alert = "N";
                info.AlertType = "";
                info.AlertTime = 0;
            }

            info.AlarmContent = tbHiddenText_contents.Text.Trim();
            info.AlarmMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

            string[] arrRepeat = tbRepeat.Text.Trim().TrimEnd('$').Split('$');

            BE.RepeatInfo repInfo = new BE.RepeatInfo();

            if (hdRepeatType.Text != "")
            {
                repInfo.CompanyID = CurrentUserInfo.CompanyID;
                repInfo.CommunityID = CommunityID;
                repInfo.RepeatType = hdRepeatType.Text;
                repInfo.SelectType = hdSelectType.Text;
                repInfo.IntervalCnt = hdIntervalCnt.Text.ToNumber<int>();
                repInfo.Option1 = (hdOption1.Text == "") ? "0" : hdOption1.Text;
                repInfo.Option2 = (hdOption2.Text == "") ? "0" : hdOption2.Text;
                repInfo.Option3 = (hdOption3.Text == "") ? "0" : hdOption3.Text;
                repInfo.Option4 = (hdOption4.Text == "") ? "0" : hdOption4.Text;
                repInfo.Option5 = (hdOption5.Text == "") ? "0" : hdOption5.Text;
                repInfo.Option6 = (hdOption6.Text == "") ? "0" : hdOption6.Text;
                repInfo.Option7 = (hdOption7.Text == "") ? "0" : hdOption7.Text;
                repInfo.MaxReptCnt = hdMaxReptCnt.Text.ToNumber<int>();
                repInfo.EndDate = (hdEndDate.Text != "") ? Convert.ToDateTime(hdEndDate.Text) : Convert.ToDateTime("2999-01-01");
            }

            List<BE.ScheduleEtcInfo> al = new List<BE.ScheduleEtcInfo>();

            // 날짜 리스트
            if (tbRepeat.Text.Trim() != "")
            {
                // 반복 일정 O
                for (int j = 0; j < arrRepeat.Length; j++)
                {
                    if (arrRepeat[j].ToString() != "")
                    {
                        BE.ScheduleEtcInfo etcInfo = new BE.ScheduleEtcInfo();
                        DateTime dtRepeat = Convert.ToDateTime(arrRepeat[j].ToString());

                        etcInfo.StartDate = new DateTime(dtRepeat.Year, dtRepeat.Month, dtRepeat.Day, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Minute, 0);
                        etcInfo.EndDate = new DateTime(dtRepeat.Year, dtRepeat.Month, dtRepeat.Day, (Convert.ToDateTime(ddlendTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlendTime.SelectedValue)).Minute, 0);
                        TimeSpan gap = etcInfo.EndDate.Subtract(etcInfo.StartDate);
                        etcInfo.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                        al.Add(etcInfo);
                    }
                }

                rtnVal = bll.Add(info, al, repInfo);
            }
            else
            {
                //반복 일정 X
                DateTime dtStart = Convert.ToDateTime(startDate.Value);
                DateTime dtEnd = Convert.ToDateTime(endDate.Value);
                info.StartDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Minute, 0);
                info.EndDate = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, (Convert.ToDateTime(ddlendTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlendTime.SelectedValue)).Minute, 0);
                TimeSpan gap = info.EndDate.Subtract(info.StartDate);
                info.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                info.ParentScheduleID = 0;

                rtnVal = bll.Add(info);
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 저장
        /// </summary>
        private void Save()
        {
            if (TmpDate == "0")
            {
                TmpDate = GetYearMonthDay();
            }

            int scheduleID = Addition();

            if (scheduleID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("scheduleID", scheduleID.ToString());
                paraDic.Add("communityID", CommunityID.ToString());
                paraDic.Add("menuID", MenuID.ToString());
                paraDic.Add("tmpDate", TmpDate);
                paraDic.Add("grade", Grade.ToString());
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoView"));
                paraDic.Add("goURL", "/DeskPlusEIP/Community/Site/Apps/Schedule/View.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 본문 내용 및 본문 이미지(업로드) MIME 하여 본문 내용가져오기
        /// </summary>
        /// <returns></returns>
        private string GetContents()
        {
            string contents = "";

            if (NonActiveX == "F") //activex
            {
                string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                string filePath = FileHelper.GetServerMapPath(UploadPath);
                string serverNameFolderName = serverName + UploadPath;

                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                string contentValue = tbHiddenMime_contents.Text;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass util
                    = new DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass();

                util.ProcessDecoding(contentValue, filePath, serverNameFolderName, 0);
                contents = util.DecodedHTML;
            }
            else if (NonActiveX == "T") //x-free
            {
                contents = tbHiddenHTML_contents.Value;
            }

            return contents;
        }

        #endregion (함수)

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
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// ActiveX 유무
        /// </summary>
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

        /// <summary>
        /// Gets or sets the upload path.
        /// </summary>
        /// <value>
        /// The upload path.
        /// </value>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        #endregion (프로퍼티)

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 확인버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            Save();
        }

        #endregion (이벤트핸들러)
    }
}