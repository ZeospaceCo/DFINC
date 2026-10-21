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
    public partial class DocOpinion : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string dID = "";
        public string bPage = "";
        public int pageNo = 0;
        public string accTag = "";
        public string groupID = "";
        public string searchValue = "";
        public int targetDeptID = 0;
        public int targetTitleID = 0;
        public int keyID = 0;
        public int key2ID = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.DocOpinion).Assembly;	//어셈블리 선언
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
            searchValue = GetDecryptParam("Sv", "");
            targetDeptID = GetDecryptParam("TargetDeptID", UserInfo.DeptID.ToString()).ToNumber<int>();
            targetTitleID = GetDecryptParam("TargetTitleID", UserInfo.CodeTitleID.ToString()).ToNumber<int>();
            keyID = GetDecryptParam("KeyID", "0").ToNumber<int>();
            key2ID = GetDecryptParam("Key2ID", "0").ToNumber<int>();
            accTag = GetDecryptParam("AccTag", "");
            groupID = GetDecryptParam("GroupID", "0");
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