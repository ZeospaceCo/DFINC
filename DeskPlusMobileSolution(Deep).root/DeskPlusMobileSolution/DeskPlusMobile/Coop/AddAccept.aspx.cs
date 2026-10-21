using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Coop
{
    public partial class AddAccept : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string bPage = "";
        public int pageNo = 0, moreSize = 0;
        public int listID = 0, groupID = 0;
        public string status = "";
        public string searchValue = "";
        public string startCalMonth = "";
        public string endCalMonth = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Coop.AddAccept).Assembly;	//어셈블리 선언
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

                LoadData();
            }            
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            bPage = GetDecryptParam("BPage", "");
            moreSize = GetDecryptParam("MoreSize", "10").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            listID = GetDecryptParam("ListID", "0").ToNumber<int>();
            groupID = GetDecryptParam("GroupID", "0").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            status = GetDecryptParam("Status", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-02
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                ddlDifficulty.Items.Clear();
                ddlDifficulty.Items.Add(new ListItem("----", "0"));
                ddlDifficulty.Items.Add(new ListItem(resMng.GetString("Difficulty1"), "1"));
                ddlDifficulty.Items.Add(new ListItem(resMng.GetString("Difficulty2"), "2"));
                ddlDifficulty.Items.Add(new ListItem(resMng.GetString("Difficulty3"), "3"));

            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-03
        /// 작성목적 : 기본값 세팅 
        /// </summary>
        private void LoadData()
        {
            startCalMonth = System.DateTime.Now.Year + "-" + System.DateTime.Now.Month.ToString().PadLeft(2,'0');
            endCalMonth = System.DateTime.Now.Year + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0');

            tbStartDate.Value = SetDate();
            tbEndDate.Value = SetDate();          
        }

        /// <summary>
        /// 날짜 변환
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string SetDate()
        {
            string tmpVal = String.Empty;
            DateTime dt = System.DateTime.Now;
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

        public string Today { get; set; }
    }
}