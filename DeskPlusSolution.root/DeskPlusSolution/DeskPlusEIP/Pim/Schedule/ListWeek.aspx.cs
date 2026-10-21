using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class ListWeek : Base.BasePagePim
    {

        #region 변수
        public string Year = string.Empty;
        public string Month = string.Empty;
        public string Day = string.Empty;

        public string Week = string.Empty;
        public string WeekLong = string.Empty;
        public string ShortMonthname = string.Empty;
        public string monthname = string.Empty;

        public string NowYear = string.Empty;
        public string NowMonth = string.Empty;
        public string NowDay = string.Empty;
        public string strDateTime = string.Empty;

        public string Scope = string.Empty;
        public int CurrentUserID = 0;
        public string StartTime = "0";
        public string DefaultSetting = "2";//일정할일등록시 기본적으로 등록하는 공유설정

        public string Notikind = "";
        public string AlarmTime = "";
        #endregion

        #region 페이지

        protected void Page_Load(object sender, EventArgs e)
        {

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ListWeek).Assembly;	//어셈블리 선언
            #endregion    

            if (!IsPostBack)
            {
                SetLanguage();
                Start_Setting();
                SettingControl();
                LoadStatus();
                SetAlarm();
            }
        }
        #endregion

        #region 함수

        /// <summary>
        /// 분류 리스트 가져오기
        /// </summary>
        private void LoadStatus()
        {
            DAL.Status dal = new DAL.Status();
            DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, "100", "0", "", true);

            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("--------", "0"));
            ddlStatus.AppendDataBoundItems = true;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Pim.BE.StatusInfo info = new BE.StatusInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.StatusID = dtr["StatusID"].ToString().ToNumber<int>();
                        info.Orders = dtr["Orders"].ToString().ToNumber<int>();
                        info.NameBase = dtr["NameBase"].ToString();
                        info.NameKo = dtr["NameKo"].ToString();
                        info.NameEn = dtr["NameEn"].ToString();
                        info.NameJa = dtr["NameJa"].ToString();
                        info.NameFr = dtr["NameFr"].ToString();
                        info.NameZhcn = dtr["NameZhcn"].ToString();
                        info.NameZhtw = dtr["NameZhtw"].ToString();
                        info.Colors = dtr["Colors"].ToString();
                        info.UseActive = dtr["UseActive"].ToString();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        info.NameBase = this.GetLangName(
                                    info.NameBase.ToNull(), info.NameKo.ToNull(),
                                    info.NameEn.ToNull(), info.NameJa.ToNull(),
                                    info.NameFr.ToNull(), info.NameZhcn.ToNull(),
                                    info.NameZhtw.ToNull(), PreLang);

                        ListItem tmpItem = new ListItem(info.NameBase, info.StatusID.ToString());
                        tmpItem.Attributes.Add("style", "color:" + info.Colors);
                        ddlStatus.Items.Add(tmpItem);

                    }
                }
            }
        }

        private void SettingControl()
        {
            ListItem ltcate1 = new ListItem(GetResource("pim1"), "0");
            ListItem ltcate2 = new ListItem(GetResource("pim2"), "3");
            ListItem ltcate3 = new ListItem(GetResource("pim3"), "1");
            ListItem ltcate4 = new ListItem(GetResource("pim4"), "2");


            ddlShare.Items.Add(ltcate1);
            ddlShare.Items.Add(ltcate2);
            ddlShare.Items.Add(ltcate3);
            ddlShare.Items.Add(ltcate4);

            MakeTime();

            //알람 - 코드관리로 처리
            DataView dvPim = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AlarmPims");
            ddlnotikind.Items.Clear();
            ddlnotikind.AppendDataBoundItems = true;
            string name = "", vals = "";

            ddlnotikind.Items.Add(new ListItem(name, vals));
            foreach (DataRowView dr in dvPim)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                // 코드값이 특정 약어로 처리되는 경우, interfacecode를 이용
                vals = (dr["InterfaceCode"].ToString().ToNull().Trim().Equals("")) ? dr["CodeID"].ToString() : dr["InterfaceCode"].ToString().ToNull().Trim();
                ddlnotikind.Items.Add(new ListItem(name, vals));
            }

            //알람 시간
            for (int i = 1; i < 12; i++)
            {
                ListItem ltime1 = new ListItem((i * 5).ToString() + GetResource("Minute"), (i * 5).ToString());
                ddlAlarmTime.Items.Add(ltime1);
            }

            ListItem ltime2 = new ListItem(GetResource("OneClock"), "60");
            ListItem ltime3 = new ListItem(GetResource("TwoClock"), "120");
            ListItem ltime4 = new ListItem(GetResource("ThreeClock"), "180");
            ListItem ltime5 = new ListItem(GetResource("FourClock"), "240");
            ListItem ltime6 = new ListItem(GetResource("FiveClock"), "300");
            ListItem ltime7 = new ListItem(GetResource("SixClock"), "360");

            ListItem ltime8 = new ListItem(GetResource("OneDay"), "1440");
            ListItem ltime9 = new ListItem(GetResource("TwoDay"), "2880");
            ListItem ltime10 = new ListItem(GetResource("ThreeDay"), "4320");

            ListItem ltime11 = new ListItem(GetResource("FourDay"), "5760");
            ListItem ltime12 = new ListItem(GetResource("FiveDay"), "7200");
            ListItem ltime13 = new ListItem(GetResource("SixDay"), "8640");
            ListItem ltime14 = new ListItem(GetResource("SevenDay"), "10080");

            ddlAlarmTime.Items.Add(ltime2);
            ddlAlarmTime.Items.Add(ltime3);
            ddlAlarmTime.Items.Add(ltime4);
            ddlAlarmTime.Items.Add(ltime5);
            ddlAlarmTime.Items.Add(ltime6);
            ddlAlarmTime.Items.Add(ltime7);
            ddlAlarmTime.Items.Add(ltime8);
            ddlAlarmTime.Items.Add(ltime9);
            ddlAlarmTime.Items.Add(ltime10);

            ddlAlarmTime.Items.Add(ltime11);
            ddlAlarmTime.Items.Add(ltime12);
            ddlAlarmTime.Items.Add(ltime13);
            ddlAlarmTime.Items.Add(ltime14);

        }

        private void MakeTime()
        {
            string strVal = string.Empty;
            string strKey = string.Empty;

            for (int i = 0; i < 24; i++)
            {
                if (i < 12)
                {

                    strVal = GetResource("AM") + " ";

                    if (i == 0)
                    {
                        strVal = strVal + " 12";
                    }
                    else
                    {

                        strVal = strVal + " " + i.ToString();
                    }
                }
                else
                {
                    strVal = GetResource("PM") + " ";
                    if (i == 12)
                    {
                        strVal = strVal + " 12";
                    }
                    else
                    {
                        strVal = strVal + " " + (i - 12).ToString();
                    }
                }

                string strTemp = string.Empty;
                strTemp = strVal + ":00";
                strKey = i.ToString() + ":00";
                ListItem lv1 = new ListItem(strTemp, strKey);
                ListItem lv12 = new ListItem(strTemp, strKey);

                ddlstartTime.Items.Add(lv1);
                ddlendTime.Items.Add(lv12);

                strTemp = strVal + ":30";
                strKey = i.ToString() + ":30";
                ListItem lv2 = new ListItem(strTemp, strKey);
                ListItem lv22 = new ListItem(strTemp, strKey);
                ddlstartTime.Items.Add(lv2);
                ddlendTime.Items.Add(lv22);
            }
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
                CbtnScheDetail.Text = GetResource("DETAIL");
                CbtnScheDetailView.Text = GetResource("DETAIL");
                CbtnScheDel.Text = GetResource("Del");
                CbtnConfirm.Text = GetResource("Save");
                CbtnClose.Text = GetResource("Close");
                CbtnCloseView.Text = GetResource("Close");
                CbtnClosetodo.Text = GetResource("Close");
                CbtnSetting.Text = GetResource("AlarmSetting");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            strDateTime = GetDecryptParam("Date", "").Trim();
            Scope = GetDecryptParam("Scope", "").Trim();
            CurrentUserID = GetDecryptParam("UserID", "0").ToNumber<int>();
        }
         

        private void Start_Setting()
        {
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "PimViewType");
            string codeID = "2";
            for (int i = 0; i < dv.Count; i++)
            {
                codeID = dv[i]["CodeID"].ToString();
            }
            DefaultSetting = codeID;

            DateTime d = DateTime.Now;

            LoadParam();
            

            NowYear = d.Year.ToString();
            NowMonth = d.Month.ToString();
            NowDay = d.Day.ToString();
            StartTime = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "StartTIME", "9");
            if (strDateTime.Length > 0)
            {
                d = DateTime.Parse(strDateTime);
            }

            Year = d.Year.ToString();
            Month = d.Month.ToString();
            Day = d.Day.ToString();
            Week = GetResource("WeekString");
            WeekLong = GetResource("WeekString");
            ShortMonthname = GetResource("MonthString");
            monthname = GetResource("MonthString");
        }

        /// <summary>
        /// 기본 알림 설정 가져오기 
        /// </summary>
        private void SetAlarm()
        {
            DAL.Alarm bll = new DAL.Alarm();
            DataTableReader dtr = bll.GetSetting(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ddlnotikind.SelectedValue = dtr["Kind"].ToString();
                        Notikind = dtr["Kind"].ToString();

                        if (dtr["Kind"].ToString() == "M" || dtr["Kind"].ToString() == "B")
                        {
                            ddlAlarmTime.SelectedValue = dtr["Time"].ToString();
                            AlarmTime = dtr["Time"].ToString();
                        }
                    }
                }
            }
        }

        #endregion

        #region 프로퍼티
        /// <summary>
        /// 유저아이디
        /// </summary>
        public int UserId
        {
            get { return CurrentUserInfo.UserID; }
        }

        /// <summary>
        /// 유저아이디
        /// </summary>
        public int CompnayID
        {
            get { return CurrentUserInfo.CompanyID; }
        }
        #endregion
    }
}