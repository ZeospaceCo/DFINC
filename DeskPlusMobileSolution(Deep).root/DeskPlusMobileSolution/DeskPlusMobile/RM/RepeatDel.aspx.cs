using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.RM
{
    public partial class RepeatDel : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int pageNo = 1;
        public string tmpDate = "";
        public string startDate = "";
        public string searchValue = "";
        public string writeYM = "";
        public int reservationID = 0;
        public int parentReservationID = 0;
        public string bPage = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.RM.RepeatDel).Assembly;	//어셈블리 선언
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
            }
        }

        /// <summary>
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
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            tmpDate = GetDecryptParam("OpenDate", "");
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            startDate = GetDecryptParam("startDate", "");
            reservationID = GetDecryptParam("ReservationID", "0").ToNumber<int>();
            parentReservationID = GetDecryptParam("ParentReservationID", "0").ToNumber<int>();
            bPage = GetDecryptParam("BPage", "");
            writeYM = GetDecryptParam("writeYM", "");
        }

        public string Today { get; set; }
    }
}