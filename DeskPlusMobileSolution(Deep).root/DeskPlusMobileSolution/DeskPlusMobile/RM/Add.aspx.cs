using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.RM
{
    public partial class Add : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string tmpDate = "";
        public string bPage = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.RM.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
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
                LoadList();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            tmpDate = GetDecryptParam("OpenDate", "");
            bPage = GetDecryptParam("BPage", "");

            if (tmpDate == "")
            {
                tmpDate = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);
            }
        }

        /// <summary>
        /// 작성자 : 방영환
        /// 작성일자 : 2012-03-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                ddlGuard.Items.Add(new ListItem(this.resMng.GetString("TIsClose1"), "0"));     //일반
                ddlGuard.Items.Add(new ListItem(this.resMng.GetString("TIsClose2"), "1"));   //보안
                ddlGuard.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 작성자 : 방영환
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
        /// 예약 자원 선택
        /// </summary>
        private void LoadList()
        {
            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_SelectList");
            sqlQuery = String.Format(sqlQuery, CompanyID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetList", null);

            ddlList.Items.Clear();
            ddlList.Items.Add(new ListItem(this.resMng.GetString("SelectResource"), "0"));
            ddlList.AppendDataBoundItems = true;

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string CategoryName = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(),
                                                                dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                                                                dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                    if (CategoryName.Length > 9)
                    {
                        CategoryName = CategoryName.Substring(0, 9) + "..";
                    }

                    string tmpText = "[" + CategoryName + "]" + dr["Title"].ToString().ToReverseXSS();
                    if (tmpText.Length > 18)
                    {
                        tmpText = tmpText.Substring(0, 18) + "..";
                    }

                    ddlList.Items.Add(new ListItem(tmpText, dr["ListID"].ToString() + "_" + dr["UsableQty"].ToString() + "_" + dr["Authority"].ToString()));
                }
            }
        }

        public string Today { get; set; }
    }
}