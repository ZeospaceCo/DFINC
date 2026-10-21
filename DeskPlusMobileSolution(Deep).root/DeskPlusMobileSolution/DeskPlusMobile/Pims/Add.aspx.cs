using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Pims
{
    public partial class Add : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string tmpDate = "";
        public int DeptID = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                DeptID = UserInfo.DeptID;

                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadParam();
                SetStartEndTime();
                LoadData();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            tmpDate = GetDecryptParam("OpenDate", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-04-24
        /// 작성목적 : 일시 시간
        /// </summary>
        private void SetStartEndTime()
        {
            tbStartDate.Value = SetDate(tmpDate);
            tbEndDate.Value = SetDate(tmpDate);

            ddlstartTime.Items.Clear();
            ddlendTime.Items.Clear();

            DateTime dtTargetDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);

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
        /// 날짜 변환
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string SetDate(string _date)
        {
            string tmpVal = String.Empty;
            DateTime dt = Convert.ToDateTime(_date);
            switch (UserInfo.PreferredLanguage)
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
                default:
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;
            }


            return tmpVal;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            //분류 리스트 가져오기
            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_Status");
            sqlQuery = String.Format(sqlQuery, CompanyID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetStatus", null);

            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("--------", "0"));
            ddlStatus.AppendDataBoundItems = true;

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                { 
                    string statusName = this.GetLangName(
                                dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                dr["NameZhtw"].ToString().ToNull(), PreLang);

                    ListItem tmpItem = new ListItem(statusName, dr["StatusID"].ToString());
                    tmpItem.Attributes.Add("style", "color:" + dr["Colors"].ToString());
                    ddlStatus.Items.Add(tmpItem);
                }     
            }

            //알람 설정 가져오기
            DataView dvPim = XmlDataBaseHelper.GetSysCode(CompanyID.ToNumber<int>(), "AlarmPims");
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
                    

            //분류 리스트 가져오기
            string sqlQuery1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_Alarm");
            sqlQuery1 = String.Format(sqlQuery1, CompanyID, UserID);

            DataSet dsA = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery1, "GetAlarm", null);

            if (dsA != null)
            {
                foreach (DataRow dr in dsA.Tables[0].Rows)
                {
                    ddlnotikind.SelectedValue = dr["Kind"].ToString();

                    if (dr["Kind"].ToString() == "M" || dr["Kind"].ToString() == "B")
                    {
                        ddlAlarmTime.SelectedValue = dr["Time"].ToString();
                    }
                }
            }          
        }

        public string Today { get; set; }
    }
}