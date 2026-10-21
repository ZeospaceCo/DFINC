using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Pims
{
    public partial class AlarmView : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int scheduleID = 0;
        public int toDoID = 0;
        public int displayID = 0;
        public int pageNo = 1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.AlarmView).Assembly;	//어셈블리 선언
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
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            scheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            toDoID = GetDecryptParam("ToDoID", "0").ToNumber<int>();
            displayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();

            pageNo = GetDecryptParam("PageNo", "0").ToNumber<int>();

            if (pageNo == 0)
            {
                pageNo = this.GetParm("PageNo", "1").ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-09-09
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        public string Today { get; set; }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : SSL 사용 유무      
        /// </summary>
        public string httpType
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : 도메인 정보        
        /// </summary>
        public string MobileDOMAIN
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");
            }
        }
    }
}