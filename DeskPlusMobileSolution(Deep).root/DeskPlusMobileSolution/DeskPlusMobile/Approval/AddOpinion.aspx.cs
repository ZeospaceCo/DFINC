using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class AddOpinion : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string dID = "", stat = "";
        public string bPage = "";
        public int pageNo = 0;
        public string aTag = "", accTag = "", num2 = "", seq = "";
        public string groupID = "";
        public int targetUID = 0;
        public int targetDID = 0;
        public string searchValue = "";
        public string alarm;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.AddOpinion).Assembly;	//어셈블리 선언
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
            bPage = GetDecryptParam("BPage", "");
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            dID = GetDecryptParam("DID", "0");
            stat = GetDecryptParam("Stat", "");
            aTag = GetDecryptParam("ATag", "");
            accTag = GetDecryptParam("AccTag", "");
            num2 = GetDecryptParam("Num2", "");
            seq = GetDecryptParam("Seq", "0");
            groupID = GetDecryptParam("GroupID", "0");
            targetUID = GetDecryptParam("TargetUID", UserID).ToNumber<int>();
            targetDID = GetDecryptParam("TargetDID", UserInfo.DeptID.ToString()).ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            alarm = GetDecryptParam("Alarm", "");
        }

        #region (함수)

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        #endregion
        public string Today { get; set; }
    }
}